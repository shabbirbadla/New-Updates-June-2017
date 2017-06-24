// *******************************************************************************
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
using System.IO;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System.Windows.Forms;
using System.Data.SqlClient;
using QueryCommander.VSS;

namespace QueryCommander.Database
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
		public enum DBConnectionType
		{
			MicrosoftSqlClient=0,
			MicrosoftOleDb=1,
			OracleOleDb=2,
			MySQL=3,
			SybaseASE=4
		}
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
		public VSSConnectionCollection vssConnectionCollection;

		public string ConnectionString
		{
			get
			{
				string cString="";
				switch (ConnectionType)
				{
					case DataSource.DBConnectionType.MicrosoftSqlClient:
						cString=MicrosoftSqlClientConnectionString;
						break;
					case DataSource.DBConnectionType.MicrosoftOleDb:
						cString=MicrosoftOleDbConnectionString;
						break;
					case DataSource.DBConnectionType.OracleOleDb:
						cString=OracleOleDbConnectionString;
						break;
					case DataSource.DBConnectionType.MySQL:
						cString=MySQLClientConnectionString;
						break;
					case DataSource.DBConnectionType.SybaseASE:
						cString=MicrosoftSqlClientConnectionString;
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
		private string OracleOleDbConnectionString
		{
			get
			{
				if(IntegratedSecurity.Length==0)
				{
					return String.Format("Data Source={0};User Id={1};Password={2}",
						Name,
						UserName,
						Password);
				}
				else
				{
					return String.Format("Data Source={0}",
						Name);
				}
			}
		}
		private string MySQLClientConnectionString
		{
			get
			{
				string pattern = "server={0};user id={1}; password={2}; database=mysql; pooling=false";
				return String.Format(pattern,
					Name,
					UserName,
					Password);
			}
		}
	}

	[Serializable]
	public class DataSourceCollection:CollectionBase
	{
		public virtual void Add( string Provider,
			string IntegratedSecurity,
			string Name,
			string PersistSecurityInfo,
			string InitialCatalog,
			string UserName,
			string Password,
			string TimeOut,
			bool IsConnected,
			DataSource.DBConnectionType connectionType)
		{
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
		public void Save()
		{
			string filename = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "DataSources.config");
			XmlSerializer ser = new XmlSerializer(typeof(DataSourceCollection));
			TextWriter writer = new StreamWriter(filename);
			ser.Serialize(writer, this);
			writer.Close();
		}
		
		public DataSource FindByID(Guid id)
		{
			foreach(DataSource ds in this)
			{
				if(ds.ID == id)
					return ds;
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
					this.Save();
					return;
				}
				index ++;
			}
			
		}
	}
	public class DataSourceFactory
	{
		public static DataSourceCollection GetDataSources()
		{
			DataSourceCollection dataSourceCollection;
			
			string filename = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "DataSources.config");
			if(!System.IO.File.Exists(filename))
				return (new DataSourceCollection());

			XmlSerializer ser = new XmlSerializer(typeof(DataSourceCollection));
			TextReader reader = new StreamReader(filename);
			dataSourceCollection = (DataSourceCollection)ser.Deserialize(reader);
			reader.Close();
			
			return dataSourceCollection;
		}
		public static void Save(DataSourceCollection dataSourceCollection)
		{
			string filename = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "DataSources.config");
			XmlSerializer ser = new XmlSerializer(typeof(DataSourceCollection));
			TextWriter writer = new StreamWriter(filename);
			ser.Serialize(writer, dataSourceCollection);
			writer.Close();
		}
	}
}
