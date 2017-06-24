using System;
using System.IO;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using SQLEditor.General;
using SQLEditor.Database;


namespace SQLEditor.General
{
	/// <summary>
	/// Summary description for QCTreeNode.
	/// </summary>
	public class QCTreeNode : System.Windows.Forms.TreeNode
	{
		#region Enums
		public enum ObjectType
		{
			Empty = -1,
			Top = 0, 
			Server=1,
			Database=2, 
			Tables=3, 
			StoredProcedures=4, 
			Functions=5, 
			Table=6, 
			StoredProcedure=7, 
			Function=8,
			Views=9,
			View=10,
			Project=11,
			Unknown=17,
			CheckedOutItem=13,
			ScriptFile=14,
			Triggers = 15,
			Trigger = 16,
			MyWorkSpaces,
			Folder,
			WorkSpaces,
			WorkSpace,
			WorkSpaceItem,
			Fields,
			Filed,
			UDTs,
			UDT,
			Defaults,
			Default,
			Rules,
			Rule,
			Indexes,
			Index,
			PK,
			Jobs,
			Job,
			JobStep,
			SysTables
		};
		#endregion
		#region Constructor		
		public QCTreeNode(string objectName, ObjectType objecttype, string vssConnection,string server, IDbConnection connection, string database,string tcConame,int nCompId)
		{
			this.Text=objectName;
			this.objectName=objectName;
			this.objecttype=objecttype;
			this.server=server;
			this.database= database;
            this.CompId = (nCompId==null?0:nCompId);
            this.Tag = database;
            this.Co_Name = (tcConame.Trim() == "" ? tcConame : database);
			this.Connection=connection;
			this.ImageIndex=GetImageIndex();
			this.SelectedImageIndex=GetImageIndex();
		}
		#endregion
		#region Fields
		public string objectName;
		public string server;
		public string database;
        public string Co_Name;
        public int CompId;
		public ObjectType objecttype;
		#endregion
		#region Properies
		public bool IsUnderSourceControl
		{
			get
			{
				return true;
			}
		}

		public IDbConnection Connection=null;
		#endregion
		#region private Methods
		private int GetImageIndex()
		{
			switch(this.objecttype)
			{
				case ObjectType.Top:
					return 7;
				case ObjectType.Server:
					return 0;
				case ObjectType.Database:
				{
					if(this.IsUnderSourceControl)
						return 11;
					else
						return 1;
				}
				case ObjectType.Tables:
					return 28; //6
				case ObjectType.StoredProcedures:
					return 28; //6
				case ObjectType.Views:
					return 28; //6
				case ObjectType.Functions:
					return 28; //6
				case ObjectType.Table:
					return 4;
				case ObjectType.View:
					return 14;
				case ObjectType.StoredProcedure:
					return 3;
				case ObjectType.Function:
					return 2;
				case ObjectType.ScriptFile:
					return 4;

				case ObjectType.WorkSpaces:
					return 0;
				case ObjectType.WorkSpace:
					return 1;
				case ObjectType.WorkSpaceItem:
					return 2;
				case ObjectType.Fields:
					return 18;
				case ObjectType.Filed:
					return 15;
				case ObjectType.Triggers:
					return 28; //6
				case ObjectType.Trigger:
					return 16;
				case ObjectType.UDTs:
					return 28; //6
				case ObjectType.UDT:
					return 17;
				case ObjectType.Defaults:
					return 28; //6
				case ObjectType.Default:
					return 15;
				case ObjectType.Rules:
					return 28; //6
				case ObjectType.Rule:
					return 15;
				case ObjectType.Indexes:
					return 27;
				case ObjectType.Index:
				{
					return 15;
				}
				case ObjectType.PK:
				{
					return 29;
				}
				case ObjectType.Jobs:
				{
					if(this.IsUnderSourceControl)
						return 25;
					else
						return 22;
				}
				case ObjectType.Job:
					if (this.Text.IndexOf("disabled") > 0 || this.Text.IndexOf("suspended") > 0 )
						return 26;
					else if (this.Text.IndexOf("executing") > 0 )
						return 24;
					else if (this.NodeFont != null && this.NodeFont.Italic)
						return 23;
					else
						return 21;
				case ObjectType.SysTables:
					return 30;
				default:
					return 17;  // the properties page image
			}
		}
		private string GetScript(IDbConnection sqlConnection)
		{
			IDatabaseManager db = DatabaseFactory.CreateNew(sqlConnection);
			string CreateScript;
			//Database db = new Database();				
				
			if(this.objecttype == ObjectType.Table)
				CreateScript = db.GetObjectConstructorString(this.database,
					this.objectName,
					sqlConnection,
					DBCommon.ScriptType.CREATE,DBCommon.ScriptObjectType.TABLE);
			else if(this.objecttype == ObjectType.Job)
			{
				CreateScript = db.GetCreateJobString( this.objectName, "master", sqlConnection );
			}
			else
				CreateScript = db.GetObjectConstructorString(this.database,
					this.objectName,
					sqlConnection,
					DBCommon.ScriptType.CREATE,DBCommon.ScriptObjectType.PROCEDURE);

			return CreateScript;
		}

		private void CreateFile(string CreateScript, string filePath)
		{
			FileInfo fi = new FileInfo(filePath);
			if(fi.Exists)
			{

				// remove readonly attribute
				if ((fi.Attributes & System.IO.FileAttributes.ReadOnly) != 0)
					fi.Attributes -= System.IO.FileAttributes.ReadOnly;

				fi.Delete();
			}
			DirectoryInfo di = fi.Directory;
			if (!di.Exists)
			{
				di.Create();
			}
			StreamWriter sr = fi.CreateText();
			sr.Write(CreateScript);
			sr.Close();			
		}

		#endregion
		#region public Methods
		public void RefreshImage()
		{
			this.ImageIndex=GetImageIndex();
			this.SelectedImageIndex=GetImageIndex();
		}
		#endregion	
	}
}
