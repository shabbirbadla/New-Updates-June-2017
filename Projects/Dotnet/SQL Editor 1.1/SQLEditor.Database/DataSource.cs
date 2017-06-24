using System;
using System.IO;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace SQLEditor.Database
{
	/// <summary>
	/// DataSource.cs includs three classes:
	/// DataSource - Encapsulates all attributes assosiated with with the connection.
	/// DataSourceCollection - A collection of DataSources
	/// DataSourceFactory - Deserializes DataSource.config to a DataSourseCollection
	/// </summary>
	[Serializable]
	public class DataSource
	{
		public Guid ID;
		public string Provider;
		public string IntegratedSecurity;
		public string Name;
		public string FriendlyName;
		public string PersistSecurityInfo;
		public string InitialCatalog;
		public string UserName;
		public string Password;
		public string TimeOut;
		public bool IsConnected; 
		public object Connection;
		public DBConnectionType ConnectionType;
		public bool IsEncrypted;
		public string ConnectionString
		{
			get
			{
				string cString="";
				switch (ConnectionType)
				{
					case DBConnectionType.MicrosoftSqlClient:
						cString=MicrosoftSqlClientConnectionString;
						break;
					case DBConnectionType.MicrosoftOleDb:
						cString=MicrosoftOleDbConnectionString;
						break;
					default:
						cString=MicrosoftSqlClientConnectionString;
						break;
				}
				return cString;
			}
		}
		private string MicrosoftOleDbConnectionString
		{
			get
			{
				if(IntegratedSecurity.Length==0)
				{
					return String.Format("Provider=sqloledb;Data Source={0};Initial Catalog={1};User Id={2};Password={3};",
						Name,
						InitialCatalog,
						UserName,
						Password);
				}
				else
				{
					return String.Format("Provider=sqloledb;Data Source={0};Initial Catalog={1};;Integrated Security=SSPI;",
						Name,
						InitialCatalog);
				}
			}
		}
		private string MicrosoftSqlClientConnectionString
		{
			get
			{
				string cString = "Data Source=" + Name +
									  "; Persist Security Info=" + PersistSecurityInfo +
									  "; Initial Catalog=" + InitialCatalog +
									  "; User ID=" + UserName + 
									  "; Password=" + Password + 
									  "; Connection Timeout=" + TimeOut;
					
				if(IntegratedSecurity.Length>0)
					cString += "; Integrated Security=" + IntegratedSecurity;
				return cString;	
			}
		}
	}

    [Serializable]
	public class DataSourceCollection:CollectionBase
	{
		public virtual void Add( string Provider,string IntegratedSecurity,string Name,
			string PersistSecurityInfo,string InitialCatalog,string UserName,
			string Password,string TimeOut,bool IsConnected,
            DBConnectionType connectionType) {
			DataSource ds = new DataSource();
			ds.ID = Guid.NewGuid();
			ds.Provider = Provider;
			ds.IntegratedSecurity = IntegratedSecurity;
			ds.Name = Name;
			ds.PersistSecurityInfo = PersistSecurityInfo;
			ds.InitialCatalog = InitialCatalog;
			ds.UserName = UserName;
			ds.Password = Password;
			ds.TimeOut = TimeOut;
			ds.IsConnected = IsConnected;
			ds.ConnectionType=connectionType;
			this.Add(ds);
		}
		public virtual void Add(DataSource NewDataSource){this.List.Add(NewDataSource);}
		public virtual DataSource this[int Index]{get{return (DataSource)this.List[Index];}}
		public DataSource FindByID(Guid id)
		{
			foreach(DataSource ds in this)
			{
                if (ds.ID == id) { return ds; }
			}
			return null;
		}
		public void Delete(DataSource ds)
		{
			int index = 0;
			foreach(DataSource d in this)
			{
				if(d.ID == ds.ID)
				{
					this.RemoveAt(index);
					DataSourceFactory.Save(this);
					return;
				}
				index ++;
			}
		}
	}
	public enum DBConnectionType
	{
		MicrosoftSqlClient=0,
		MicrosoftOleDb=1
	}
	public class DataSourceFactory
	{
		public static DataSourceCollection GetDataSources()
		{
			DataSourceCollection dataSourceCollection;
			bool SaveEncrypted=false;

			string filename = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "DataSources.config");
            if (!System.IO.File.Exists(filename))
            {
                return (new DataSourceCollection());
            }
			XmlSerializer ser = new XmlSerializer(typeof(DataSourceCollection));
			TextReader reader = new StreamReader(filename);
			dataSourceCollection = (DataSourceCollection)ser.Deserialize(reader);
			reader.Close();
			foreach(DataSource ds in dataSourceCollection)
			{
				if(!ds.IsEncrypted)
					SaveEncrypted=true;
				else
					ds.Password = Security.Decrypt(ds.Password);
			}
            if (SaveEncrypted)
            {
                Save(dataSourceCollection);
            }
			return dataSourceCollection;
		}
		public static void Save(DataSourceCollection dataSourceCollection)
		{
			foreach(DataSource ds in dataSourceCollection)
			{
				ds.IsEncrypted = true;
				ds.Password = Security.Encrypt(ds.Password);	
			}
			string filename = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "DataSources.config");
			XmlSerializer ser = new XmlSerializer(typeof(DataSourceCollection));
			TextWriter writer = new StreamWriter(filename);
			ser.Serialize(writer, dataSourceCollection);
			writer.Close();
            foreach (DataSource ds in dataSourceCollection)
            {
                ds.Password = Security.Decrypt(ds.Password);
            }
		}	
	}
}
