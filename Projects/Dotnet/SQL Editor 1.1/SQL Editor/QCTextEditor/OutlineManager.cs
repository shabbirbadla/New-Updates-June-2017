// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
// *******************************************************************************
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using System.Text.RegularExpressions;
using System.Threading;

namespace QueryCommander
{
	
	public class OutlineManager : System.ComponentModel.ISynchronizeInvoke
	{
		#region Constants
		const string START_STRING = @"\bbegin\b| /\*";
		const string END_STRING = @"\bbegin\b|\bend\b";
		const string COMMENTEND_STRING = @"\*/| /\*";
		#endregion
		#region enums
		enum Ends{Section, Comment};
		#endregion
		#region Public Members
		public bool DoOutLine
		{
			get
			{
				return _doOutLine;
			}
			set
			{
				_doOutLine=value;
				
				if(_doOutLine)
					AppendOutline();
				else
				{
					for(int i=_outLineCollection.Count-1;i>-1;i--)
					{
						_outLineCollection.Delete(i);//removeOutLines.Add(i);
						
					}
				}
			}
		}
		public QCRichEditor textControl;
		public OutLineCollection outLineCollection
		{
			get{return _outLineCollection;}
		}
		public Panel panel
		{
			get{return _panel;}
		}
		#endregion
		#region Private Members
		bool _doOutLine=true;
		Panel _panel;
		OutLineCollection _outLineCollection = new OutLineCollection();
		int _numberOfLines=0;
		#endregion
		#region Constructor
		public OutlineManager(QCRichEditor textControl, Panel panel)
		{
			return; // Disabling outlining

			this.textControl = textControl;
			_panel=panel;
			
			this.Initialize();

			textControl.KeyPress+=new KeyPressEventHandler(OnKeyPress);
		}
		#endregion
		#region Private Member
		private int GetEndPos(string text, int startPos, Ends endType)
		{
			
			Match m;
			int diff;
			string pat;
			string matchString;
			int nestLevel=0;

			if(endType == Ends.Comment)
			{
				pat=COMMENTEND_STRING;
				diff=2;
				matchString="*/";
			}
			else
			{
				pat=END_STRING;
				diff=3;
				matchString="END";
			}
			
			
			Regex r=new Regex(pat,RegexOptions.IgnorePatternWhitespace|RegexOptions.IgnoreCase);
			
			for (m = r.Match(text); m.Success ; m = m.NextMatch())
			{
				if(m.Value.ToUpper()==matchString && nestLevel==0)
					break;
				else if(m.Value.ToUpper()!=matchString)
					nestLevel++;
				else
					nestLevel--;
			}
			
			if(m.Index==0)
				return 0;
			else
				return m.Index+startPos+diff;
		
		}

		#endregion
		#region Public Member
		public void AppendOutline()
		{
			try
			{	
				if(!DoOutLine)
					return;

				_panel.SuspendLayout();
				ArrayList removeOutLines = new ArrayList();
				string text = textControl.Text;

				string pat=START_STRING;   //@"\x27(.|[\r\n])*?\x27 | /\*(.|[\r\n])*?\*/";
				Regex r = new Regex(pat,RegexOptions.IgnorePatternWhitespace|RegexOptions.IgnoreCase);

				Match m;

				for (m = r.Match(text); m.Success ; m = m.NextMatch())
				{
					int startLine = textControl.GetLineFromCharIndex(m.Index);
					
					int endMatch;
					if(m.Value.ToUpper()=="BEGIN")
						endMatch = GetEndPos(text.Substring(m.Index+1),m.Index+1,Ends.Section);
					else
						endMatch = GetEndPos(text.Substring(m.Index+1),m.Index+1,Ends.Comment);

					if(endMatch<=0)
						continue;
					Point p =textControl.GetPositionFromCharIndex(m.Index);
					
					string outLineText = text.Substring(m.Index,endMatch-m.Index);
					int endLine = textControl.GetLineFromCharIndex(endMatch);

					OutLine outLine = new OutLine(_panel,p,outLineText,m.Index,endMatch,startLine,endLine,0,this);
					_outLineCollection.Add(outLine);
				}

				if(outLineCollection.Count>0)
				{
					for(int i=_outLineCollection.Count-1;i>-1;i--)
					{
						OutLine ol = _outLineCollection[i];
						string testText = textControl.GetText(ol.StartPos,ol.EndPos);
						int startLine	= textControl.GetLineFromCharIndex(ol.StartPos);
						int endLine		= textControl.GetLineFromCharIndex(ol.EndPos-1);

						if(!ol.IsValid(testText, startLine, endLine))
							_outLineCollection.Delete(i);//removeOutLines.Add(i);
						
					}
				}
				_panel.ResumeLayout();
				
			}
			catch(Exception ex)
			{
				_panel.ResumeLayout();
				return;
			}
		}

		public void UpdateOutLine(int startPos, int endPos, string text, bool collapsed)
		{
			textControl.SetText(startPos,endPos,text);
		}
		
		#endregion
		#region Events
		protected void OnKeyPress(object sender,KeyPressEventArgs e)
		{
			if(_numberOfLines!=textControl.Lines.Length)
			{
				AppendOutline();
				_numberOfLines= textControl.Lines.Length;
			}
		}
		#endregion

		delegate void AppendOutlineDelegate();
		protected WorkerThread m_WorkerThread =    null;
		public void AppendOutlineAsync()
		{
			if(InvokeRequired)
			{
				//BeginInvoke(new AppendOutlineDelegate(AppendOutline), null);
				Invoke(new AppendOutlineDelegate(AppendOutline), null);
			}
			else
			{
				AppendOutline();
			}
			
		}
		public int Initialize()
		{
			if(m_WorkerThread == null)
			{
				m_WorkerThread = new WorkerThread(this);
				// wait    for thread to get running before returning...
				m_WorkerThread.WaitOnThreadReady();
				string tmpstr =    String.Format("Created PacketStream Thread : {0}",
					m_WorkerThread.m_ThreadObj.Name);
				Console.WriteLine(tmpstr);
			}
			return 0;
		}
		#region    ISynchronizeInvoke Members
		

		public IAsyncResult BeginInvoke(Delegate method, object[] args)
		{
			if(!m_WorkerThread.IsActive)
			{
				return null;
			}
			WorkItem result    = new WorkItem(null,method,args);
			if(m_WorkerThread != null)
			{
				m_WorkerThread.QueueWorkItem(result);
			}
			return result;
		}
		public object EndInvoke(IAsyncResult result)
		{
			result.AsyncWaitHandle.WaitOne();
			WorkItem workItem = (WorkItem)result;
			return    workItem.MethodReturnedValue;
		}

		public object Invoke(Delegate method, object[] args)
		{
			IAsyncResult asyncResult;
			asyncResult = BeginInvoke(method,args);
			return EndInvoke(asyncResult);
		}

		public bool InvokeRequired
		{
			get
			{
				bool res = false;
				if(m_WorkerThread != null)
				{
					res = (Thread.CurrentThread.GetHashCode() ==
						m_WorkerThread.m_ThreadObj.GetHashCode());
				}
				return    ! res;
			}
		}
		#endregion ISynchronizeInvoke

	}

	#region Thread manager classes
	#region    Worker Thread Class
	public class WorkerThread
	{
		public Thread m_ThreadObj;
		bool m_EndLoop;
		Mutex m_EndLoopMutex;
		AutoResetEvent m_ItemAdded;
		Object m_BaseObject;
		Queue m_WorkItemQueue;
		long m_nMaxWorkItemsQueued =0;
		ManualResetEvent m_ReadyEvent =    new ManualResetEvent(false);

		internal void QueueWorkItem(WorkItem workItem)
		{
			lock(m_WorkItemQueue.SyncRoot)
			{
				m_WorkItemQueue.Enqueue(workItem);
				m_ItemAdded.Set();
				if (m_WorkItemQueue.Count > m_nMaxWorkItemsQueued)
				{
					m_nMaxWorkItemsQueued =    (long)(m_WorkItemQueue.Count);
				}
			}
		}
		internal WorkerThread(Object baseObject)
		{
			m_BaseObject = baseObject;
			m_EndLoop = false;
			m_ThreadObj = null;
			m_EndLoopMutex = new Mutex();
			m_ItemAdded = new AutoResetEvent(false);
			m_WorkItemQueue    = new Queue();
			CreateThread(true);
		}
		private    bool EndLoop
		{
			set
			{
				m_EndLoopMutex.WaitOne();
				m_EndLoop = value;
				m_EndLoopMutex.ReleaseMutex();
			}
			get
			{
				bool result = false;
				m_EndLoopMutex.WaitOne();
				result = m_EndLoop;
				m_EndLoopMutex.ReleaseMutex();
				return result;
			}
		}

		public bool IsActive
		{
			get { return !m_EndLoop; }
		}

		public bool Reset()
		{
			lock(m_WorkItemQueue.SyncRoot)
			{
				if(m_WorkItemQueue.Count > 0)
				{
					m_WorkItemQueue.Clear();
				}
			}
			return true;
		}

		Thread CreateThread(bool autoStart)
		{
			if(m_ThreadObj != null)
			{
				System.Diagnostics.Debug.Assert(false);
				return m_ThreadObj;
			}
			m_nMaxWorkItemsQueued =    0;
			ThreadStart threadStart    = new ThreadStart(Run);
			m_ThreadObj = new Thread(threadStart);
			m_ThreadObj.Name = m_BaseObject.ToString() + " PacketStream Worker Thread";
			if(autoStart ==    true)
			{
				m_ThreadObj.Start();
			}
			return m_ThreadObj;
		}
		void Start()
		{
			System.Diagnostics.Debug.Assert(m_ThreadObj != null);
			System.Diagnostics.Debug.Assert(m_ThreadObj.IsAlive == false);
			m_ThreadObj.Start();
		}
		public bool QueueEmpty
		{
			get
			{
				lock(m_WorkItemQueue.SyncRoot)
				{
					if(m_WorkItemQueue.Count > 0)
					{
						return false;
					}
					return true;
				}
			}
		}
		WorkItem GetNext()
		{
			if(QueueEmpty)
			{
				return null;
			}
			lock(m_WorkItemQueue.SyncRoot)
			{
				return (WorkItem)m_WorkItemQueue.Dequeue();
			}
		}
		public void WaitOnThreadReady(){ m_ReadyEvent.WaitOne(); }
		void Run()
		{
			// we are in this thread now...
				
			m_ReadyEvent.Set();
			while(EndLoop == false)
			{
				while(QueueEmpty == false)
				{
					if(EndLoop == true)
					{
						return;
					}
					WorkItem workItem = GetNext();
					workItem.CallBack();
					if(workItem.MethodReturnedValue    is System.Int32)
					{
						if((int)workItem.MethodReturnedValue ==    -1)
						{
							EndLoop    = true;
						}
					}

				}
				m_ItemAdded.WaitOne();
			}
		}
		public void Kill()
		{
			//Kill is called on client thread - must use cached thread object
			System.Diagnostics.Debug.Assert(m_ThreadObj != null);
			if(m_ThreadObj.IsAlive == false)
			{
				return;
			}
			EndLoop    = true;
			m_ItemAdded.Set();

			//Wait for thread to die
			m_ThreadObj.Join();
			if(m_EndLoopMutex != null)
			{
				m_EndLoopMutex.Close();
			}
			if(m_ItemAdded != null)
			{
				m_ItemAdded.Close();
			}
		}
	}
	#endregion Worker Thread
	#region    Worker Thread Work Item
	internal class WorkItem: IAsyncResult
	{
		object[] m_Args;
		object m_AsyncState;
		bool m_Completed;
		Delegate m_Method;
		ManualResetEvent m_Event;
		object m_MethodReturnedValue;
		internal WorkItem(object AsyncState,Delegate method,object[] args)
		{
			m_AsyncState = AsyncState;
			m_Method = method;
			m_Args = args;
			m_Event    = new ManualResetEvent(false);
			m_Completed = false;
		}
		//IAsyncResult properties
		object IAsyncResult.AsyncState
		{
			get { return m_AsyncState; }
		}
		WaitHandle IAsyncResult.AsyncWaitHandle
		{
			get    { return m_Event; }
		}
		bool IAsyncResult.CompletedSynchronously
		{
			get { return false;    }
		}
		bool IAsyncResult.IsCompleted
		{
			get    { return Completed;    }
		}
		bool Completed
		{
			get    { lock(this) { return m_Completed;    }}
			set { lock(this) { m_Completed = value;    }}
		}

		//This method is called    on the worker thread to    execute    the method
		internal void CallBack()
		{
			try
			{
				MethodReturnedValue = m_Method.DynamicInvoke(m_Args);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				MethodReturnedValue = (int)-1;
				// System.Threading.Thread.CurrentThread.thre
			}
			//Method is done. Signal the world
			m_Event.Set();
			Completed = true;
		}
		internal object    MethodReturnedValue
		{
			get
			{
				object methodReturnedValue;
				lock(this) { methodReturnedValue = m_MethodReturnedValue; }
				return methodReturnedValue;
			}
			set { lock(this) { m_MethodReturnedValue = value; }}
		}
	}
	#endregion

	#endregion
}



