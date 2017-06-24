using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Lifetime;

namespace SQLEditor.WinGui.Base
{
	/// <summary>
	/// Summary description for SingleInstance.
	/// </summary>
	public class SingleInstanceHandler : MarshalByRefObject
	{
		private System.Threading.Mutex m_Mutex;
		private string m_UniqueIdentifier;
		private TcpChannel m_TCPChannel;

		public event InstanceEventHandler InstanceEvent;

		public void Run(string []strArgs)
		{
            strArgs = new string[] { "T010809","90" };
            m_UniqueIdentifier = Application.ProductName + Application.ProductVersion;
			m_Mutex = new System.Threading.Mutex(false, m_UniqueIdentifier);
			if (strArgs.Length == 0)
			{
				strArgs = new string [] {""};
			}
			if(m_Mutex.WaitOne(1,true))
			{
				CreateInstanceChannel();
				InstanceEventArgs EvArg = new InstanceEventArgs(strArgs, true);
				RaiseStartUpEvent(EvArg);
			}
			else
			{
				InstanceEventArgs EvArg = new InstanceEventArgs(strArgs, false);
				UseInstanceChannel(EvArg);
			}
		}

		private void CreateInstanceChannel()
		{ 
			LifetimeServices.LeaseTime = TimeSpan.Zero;
			LifetimeServices.LeaseManagerPollTime = TimeSpan.FromMinutes(1);
			LifetimeServices.RenewOnCallTime = TimeSpan.FromMinutes(2);
			LifetimeServices.SponsorshipTimeout = TimeSpan.FromMinutes(1);
			
			System.Runtime.Remoting.ObjRef or = 
				System.Runtime.Remoting.RemotingServices.Marshal(this, m_UniqueIdentifier, typeof(SingleInstanceHandler));

			IDictionary tcp_properties = new Hashtable(2);
			tcp_properties.Add("bindTo","127.0.0.1");
			tcp_properties.Add("port",0);
			m_TCPChannel = new TcpChannel(tcp_properties,null,null);
			System.Runtime.Remoting.Channels.ChannelServices.RegisterChannel(m_TCPChannel);
			Microsoft.Win32.RegistryKey key = Application.UserAppDataRegistry;
			key.SetValue(m_UniqueIdentifier, m_TCPChannel.GetUrlsForUri(m_UniqueIdentifier));
		}

		private void UseInstanceChannel(InstanceEventArgs event_args)
		{
			Microsoft.Win32.RegistryKey key = Application.UserAppDataRegistry;
			SingleInstanceHandler remote_component = (SingleInstanceHandler)System.Runtime.Remoting.RemotingServices.Connect(typeof(SingleInstanceHandler), ((string[])key.GetValue(m_UniqueIdentifier))[0]);
			
			try
			{
				if (System.Runtime.Remoting.RemotingServices.IsTransparentProxy(remote_component))
				{
					remote_component.RaiseStartUpEvent(event_args);
				}
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}

		public void RaiseStartUpEvent(InstanceEventArgs event_args)
		{
			InstanceEvent(this, event_args);
		}

	}

	public delegate void InstanceEventHandler(object sender, InstanceEventArgs mea);

	[ Serializable ]
	public class InstanceEventArgs: EventArgs 
	{
		public InstanceEventArgs(string []strNewArgs, bool NewInst) 
		{
			this.strArgs = strNewArgs;
			this.bNewInstance = NewInst;
		}

		public string []strArgs;
		public bool	bNewInstance;
	}
}
