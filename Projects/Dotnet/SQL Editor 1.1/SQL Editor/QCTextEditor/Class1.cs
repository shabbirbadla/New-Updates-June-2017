using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Collections;
using System.ComponentModel;

namespace FSBug
{
	delegate bool PushDataPacketDelegate();
	class MainClass
	{
		[STAThread]
		static void Main(string[] args)
		{
			string filename    = "test.pkt";
			PacketStream pktStream = new PacketStream(filename);
			pktStream.Initialize();
			int count = 1;
			try
			{
				while(true)
				{
					if(!pktStream.PushDataPacketSimpleAsync())
					{
						break;
					}
					System.Threading.Thread.Sleep(2);
					if(pktStream.BinaryStreamWriter.BaseStream.Length >= 0x10000)
					{
						string tmpstr =    String.Format("Iteration #{0}. Re-opening file.",
							count++);
						Console.WriteLine(tmpstr);
						pktStream.Close();
						pktStream.Open(filename);
					}
				}
			}
			catch(Exception    ex)
			{
				Console.WriteLine("Exiting Program with    Exception.");
				Console.WriteLine(ex);
				System.Diagnostics.Debug.WriteLine(ex);
			}
		}
	}

	#region    PacketStream Class
	public class PacketStream : ISynchronizeInvoke
	{
		private    Stream m_stream    = null;
		// the Binary writer for data.
		private    BinaryWriter bw    = null;
		private    int m_packetNumber = 0;
		private    string m_filename = "";
		protected WorkerThread m_WorkerThread =    null;

		public PacketStream(string filename)
		{
			m_filename = filename;
		}

		~PacketStream()
		{
			if(m_WorkerThread != null)
			{
				m_WorkerThread.Kill();
			}
		}

		public BinaryWriter BinaryStreamWriter
		{
			get { return bw; }
		}

		public int NextPacketNumber
		{
			get { return m_packetNumber++; }
		}

		/// <summary>
		/// Initializes    the object including starting any processing threads, etc...
		/// </summary>
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

		public int InitializeResources()
		{
			Open(m_filename);
			return 0;
		}

		public void Flush()
		{
			if(m_stream != null)
			{
				if(m_stream.CanWrite)
					m_stream.Flush();
			}
		}

		public void Open(string    filename)
		{
			m_stream = new FileStream(filename, FileMode.Create);
			// m_stream = new MemoryStream(MaxSizeOfBuffer);
			bw = new BinaryWriter(m_stream);
			if(m_WorkerThread.IsActive)
			{
				m_WorkerThread.Reset();
			}
		}

		public    void Close()
		{
			// wait    for thread to process it's entries...
			m_WorkerThread.Reset();
			Flush();
			if(bw != null)
			{
				bw.Close();
				bw = null;
			}
			if(m_stream != null)
			{
				m_stream.Close();
				m_stream = null;
			}
		}

		#region    PushData Functions
		public bool PushDataPacketSimpleAsync()
		{
			bool rb    = true;
			if(InvokeRequired)
			{
				if(BeginInvoke(new PushDataPacketDelegate(PushDataPacketSimple), null
					)    == null)
				{
					rb = false;
				}
			}
			else
			{
				PushDataPacketSimple();
			}
			return rb;
		}

		public bool PushDataPacketSimple()
		{
			bool rb    = false;
			uint pktsize = 4;
			int idx;
			System.Random rnd = new    Random();
			int NumberBytes    = rnd.Next(0,25) * 4; // make this a multiple of 4
			pktsize    += (uint)NumberBytes;

			// create buffer here
			byte []buffer =    new byte[NumberBytes];
			for(idx    = 0; idx < NumberBytes;    idx++)
			{
				buffer[idx] = (byte)(NextPacketNumber&0xFF);
			}

			if(bw != null)
			{
				// get position    before writing packet out
				long beforePos = bw.BaseStream.Position;
				// write out data
				bw.Write((int)0); // 0x00000000    will begin a new 'packet'
				bw.Write(buffer); // incrementing sequence of bytes for    the data payload
				// get position    of bytes written
				long afterPos =    bw.BaseStream.Position;
				if((afterPos - beforePos) != pktsize)
				{
					// NOTE: duplicate packet will be seen at end of file (starting    with
					0x00000000)
								  string exstr = String.Format("PushDataPacketSimple ERROR {0} - {1} =
								  {3} != {2}    ; 0x{0:X} - 0x{1:X} = {3:X} != {2:X}", afterPos, beforePos,
																						pktsize, (afterPos - beforePos));
					System.Diagnostics.Debug.WriteLine(exstr);
					Console.WriteLine(exstr);
					throw(new Exception(exstr));
				}
				rb = true;
			}
			return rb;
		}
		#endregion

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
	#endregion

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
			if(this.m_BaseObject !=    null)
			{
				if(m_BaseObject    is PacketStream)
				{
					// tried to initialize filestream in thread that is accessing it
					instead of main thread
								   PacketStream pkt = m_BaseObject    as PacketStream;
					pkt.InitializeResources();
				}
			}
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
}
