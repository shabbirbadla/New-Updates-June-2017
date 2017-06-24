using System;
using System.Collections;
using System.Threading;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Data;
using System.Data.SqlClient;
using SQLEditor.Database;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;



namespace SQLEditor.General.TestBench
{
	#region ActionType
	public enum ActionType
	{
		Script,
		Wait,
		None,
		Root
	}
	#endregion 
	#region State
	public class State
	{
		public State(ManualResetEvent manualEvent)
		{
			this.manualEvent = manualEvent;
		}
		public ManualResetEvent manualEvent;
	}
	#endregion
	#region ActionEventArgs
	public class ActionEventArgs: EventArgs 
	{
		public ActionEventArgs(object msg) 
		{
			this.Message = msg;
		}
		public object Message;
	}
	#endregion
	#region RootActionEventArgs
	public class RootActionEventArgs: EventArgs 
	{
		public RootActionEventArgs(Guid actionID,ActionProperties actionProperties) 
		{
			this.ActionID=actionID;
			this.actionProperties=actionProperties;
		}
		public Guid ActionID;
		public ActionProperties actionProperties;
	}
	#endregion
	#region Action
	[Serializable]
	public class Action
	{
		public Action()
		{
			ID = Guid.NewGuid();
		}	
		#region Events
		public delegate void DisposeEventHandler(object sender, ActionEventArgs args);
		public delegate void StartEventHandler(object sender, ActionEventArgs args);
		public event DisposeEventHandler DisposeEvent;
		public event StartEventHandler StartEvent;
		#endregion
		#region Properties
		// Propertygrid
		private DateTime _executionBegin;
		private DateTime _executionEnd;
		private DateTime _totalExecutionBegin;
		private DateTime _totalExecutionEnd;
		private bool _exceptionOccurd;
		private DataSet _dataSet;

		public string DatabaseName;
		private IDatabaseManager dbManager=null;
		
		private string _script= string.Empty;
		private ActionProperties actionProperties = null;
		
		public string ConnectionType;
		public string ConnectionString;

		private System.Data.IDbConnection dataConnection=null;
		//SqlConnection _dataConnection=null;

		//public object dataConnection=null;
		public string FileName= string.Empty;
		public Action rootAction;
		public string ScriptFile= string.Empty;
		public void ReadScript()
		{
			
			if(_script.Length>0)
				return;

			using (StreamReader sr = new StreamReader(ScriptFile)) 
			{
				String line;
				while ((line = sr.ReadLine()) != null) 
				{
					_script+=line + " \n";
				}
			}

		}
		public string Name= string.Empty;
		public string Description= string.Empty;
		public Guid ID = Guid.Empty;
		public string Command = string.Empty;
		public int Multiplier = 1;
		public int WaitMilliSeconds;
		public ActionType Type = ActionType.None;
		public ActionCollection Actions = new ActionCollection();
		
		#endregion
		#region Methods
		public void Run(Object stateInfo)
		{
			
			_totalExecutionBegin=DateTime.Now;
			if (this.Type == ActionType.Wait)
			{
				RaiseStartEvent("WAIT");
				_executionBegin=DateTime.Now;
				Thread.Sleep(new TimeSpan(0,0,this.WaitMilliSeconds));
				_executionEnd=DateTime.Now;
	
			}
			else if (this.Type == ActionType.Script)
			{
				RaiseStartEvent("SCRIPT");
				try
				{
					ReadScript();
					SetConnection();
					SetDataConnection(dataConnection, DatabaseName);
					_executionBegin=DateTime.Now;
					_dataSet= dbManager.ExecuteCommand_DataSet(_script, (System.Data.IDbConnection)dataConnection,DatabaseName);
					_executionEnd=DateTime.Now;

				}
				catch
				{
					_exceptionOccurd=true;
				}
				//Thread.Sleep(1000); // Sleep a second just to give the illusion of work being performed.
			}
			

			if (this.Actions.Count > 0)
			{
				// Yes it does, start each of them in turn in a new thread.

				ManualResetEvent[] manualEvents = new ManualResetEvent[this.Actions.Count];

				for (int i = 0; i<this.Actions.Count; i++)
				{
					Action a = (Action)this.Actions[i];
					if(this.rootAction==null)
						a.rootAction=this;
					else
						a.rootAction=this.rootAction;

					for (int j = 0; j<a.Multiplier; j++)
					{
						manualEvents[i] = new ManualResetEvent(false);
						State state = new State(manualEvents[i]);
						ThreadPool.QueueUserWorkItem(new WaitCallback(a.Run),state);							
					}
				}

				// Since ThreadPool threads are background threads, 
				// wait for the work items to signal before exiting.
				WaitHandle.WaitAll(manualEvents);
			}
			_totalExecutionEnd=DateTime.Now;
			this.actionProperties = new ActionProperties(this.Name,
				this.Description,
				this._executionBegin,
				this._executionEnd,
				this._exceptionOccurd,
				this._dataSet,
				this._totalExecutionBegin,
				this._totalExecutionEnd);
			RaiseDisposeEvent(actionProperties);

			if (stateInfo != null)
			{
				
				((State)stateInfo).manualEvent.Set(); // Signal calling Thread that we have completed
			}
			

		}

		private void SetConnection()
		{
			dataConnection = SQLEditor.Database.DatabaseFactory.GetConnection(ConnectionType, ConnectionString);
			dataConnection.Open();
		}
		public void Save()
		{
			try
			{
				XmlSerializer ser = new XmlSerializer(typeof(Action));
				TextWriter writer = new StreamWriter(FileName);
				ser.Serialize(writer, this);
				writer.Close();
			}
			catch(Exception ex)
			{
				throw(ex);
			}
		}
		public static Action Load(string fileName)
		{
			Action _action=new Action();
			try
			{
				XmlSerializer ser = new XmlSerializer(typeof(Action));
				TextReader reader = new StreamReader(fileName);
				_action = (Action)ser.Deserialize(reader);
				reader.Close();
				return _action;
			}
			catch
			{
				return null;
			}
			
		}
		private void RaiseDisposeEvent(object msg) 
		{
			ActionEventArgs d = new ActionEventArgs(msg);

			try
			{
				if(rootAction==null)
					this.DisposeEvent(this,d); 
				else
					rootAction.DisposeEvent(this,d); 
			}
			catch
			{
				return;
			}
		}
		
		private void RaiseStartEvent(string msg) 
		{
			ActionEventArgs d = new ActionEventArgs(this.Name + " ["+msg+"]");

			try
			{
				if(rootAction==null)
					this.StartEvent(this,d); 
				else
					rootAction.StartEvent(this,d); 
			}
			catch
			{
				return;
			}
		}
		
		public void SetDataConnection( System.Data.IDbConnection dataConnection, string DatabaseName)
		{
			if(this.Type==ActionType.Script)
			{
				this.dataConnection= dataConnection;
				this.DatabaseName=DatabaseName;
				this.dbManager=SQLEditor.Database.DatabaseFactory.CreateNew((System.Data.IDbConnection)this.dataConnection);
			}
			foreach(Action action in this.Actions)
				action.SetDataConnection(dataConnection,DatabaseName);
		
		}
		public ActionProperties GetActionProperties()
		{
				return actionProperties;
		}

		public void SetActionProperties(ActionProperties actionProperties)
		{
			this.actionProperties=actionProperties;
		}
		#endregion
	}
	
	[Serializable]
	public class ActionCollection : CollectionBase
	{
		
		public virtual void Add(Action NewAction)
		{
			this.List.Add(NewAction);
		}
		public virtual Action this[int Index]{get{return (Action)this.List[Index];}}
		
	}
	#endregion
	#region ActionStarter
	public class ActionStarter
	{
//		System.Data.IDbConnection _dataConnection;
//		string _databaseName;
		Thread _thread;
		public delegate void DisposeEventHandler(object sender, RootActionEventArgs args);
		public delegate void StartEventHandler(object sender, RootActionEventArgs args);
		public event DisposeEventHandler DisposeEvent;
		public event StartEventHandler StartEvent;

		static string _fileName;
		public void Start(string fileName,System.Data.IDbConnection dataConnection, string DatabaseName)
		{
			_fileName=fileName;
//			_databaseName=DatabaseName;
//			_dataConnection=dataConnection;
			ThreadStart threadDelegate = new ThreadStart(this.Run);
			_thread = new Thread(threadDelegate);
			_thread.ApartmentState = System.Threading.ApartmentState.MTA;
			_thread.Start();
		}
		public void Abort()
		{
			_thread.Abort();
		}
		public void Suspend()
		{
			_thread.Suspend();
		}
		public void Resume()
		{
			_thread.Resume();
		}
		private  void Run()
		{
			Action action = Action.Load(_fileName);
			action.StartEvent+=new SQLEditor.General.TestBench.Action.StartEventHandler(action_StartEvent);
			action.DisposeEvent+=new SQLEditor.General.TestBench.Action.DisposeEventHandler(action_DisposeEvent);
			//action.SetDataConnection(_dataConnection,_databaseName);
			action.Run(null);
		}

		
		private void RaiseDisposeEvent(Guid actionID, ActionProperties actionProperties) 
		{
			RootActionEventArgs d = new RootActionEventArgs(actionID,actionProperties);
			
			try
			{
				this.DisposeEvent(this,d);	 
			}
			catch
			{
				return;
			}
		}
		
		private void RaiseStartEvent(Guid actionID) 
		{
			RootActionEventArgs d = new RootActionEventArgs(actionID,null);

			try
			{
				this.StartEvent(this,d); 
			}
			catch
			{
				return;
			}
		}
		
		private void action_DisposeEvent(object sender, ActionEventArgs args)
		{	
			RaiseDisposeEvent(((Action)sender).ID, (ActionProperties)args.Message );
		}

		private void action_StartEvent(object sender, ActionEventArgs args)
		{
			RaiseStartEvent(((Action)sender).ID);
		}
	}
	#endregion
}
