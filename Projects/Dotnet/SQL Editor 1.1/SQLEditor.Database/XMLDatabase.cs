using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Xml;

namespace SQLEditor.Database
{
	public class XMLDatabase
	{
		#region Private Members
		bool _createKeys;
		XmlReader _reader;
		TableCollection _tables = new TableCollection();
		TableInstanceCollection _instances = new TableInstanceCollection(); 
		string _insertStatement;
		#endregion
		#region Public Methods
		public XMLDatabase(XmlReader reader)
		{
			this._reader=reader;
		}
		/// <summary>
		/// Returns table scripts from xml-file
		/// </summary>
		/// <param name="createKeys"></param>
		/// <returns></returns>
		public string GetDatabaseSQLScript(bool createKeys)
		{
			_createKeys = createKeys;
			XmlNode root = CreateDatabaseStructureFromXML();
			ProcessNode(root, new Table("root"));
			return _tables.ToSQLString();		
		}
		/// <summary>
		/// Generates insert scripts from xml-file
		/// </summary>
		/// <param name="createKeys"></param>
		/// <returns></returns>
		public string GetInsertScript(bool createKeys)
		{
			_createKeys = createKeys;
			XmlNode root = CreateDatabaseStructureFromXML();
			TableInstance t = new TableInstance("root");
			ProcessNodeForInsertScript(root,new TableInstance("root"));
			return _instances.ToSQLString();
		}

		#endregion
		#region Private Methods
		private XmlNode CreateDatabaseStructureFromXML()
		{
			_reader.MoveToContent();
			XmlDocument doc = new XmlDocument();
			doc.Load(_reader);
			return doc.FirstChild; //Database name
		}
		
		private void ProcessNode(XmlNode node, Table parentTable)
		{
			Table table = _tables.GetTable(node.Name);
			table.Parent=parentTable;
			if(node.Attributes.Count>0)
			{
				Exception ex = new Exception("XmlContainsAttributes");
				//throw ex;
			}


			if(_createKeys)
			{
				table.Columns.Add(new Column("#ID","UNIQUEIDENTIFIER",""));
				
				if(node.ParentNode.Name!="#document")
				{
					if(parentTable.HasColumns)
						table.Columns.Add(new Column("#fk_"+ node.ParentNode.Name ,"UNIQUEIDENTIFIER",""));
					else
					{
						Table parent = table.Parent.Parent;
						if(parent.Name!="root")
							table.Columns.Add(new Column("#fk_"+ parent.Name ,"UNIQUEIDENTIFIER",""));
					}
				}
			}
			
			foreach(XmlAttribute attr in node.Attributes)
			{
				table.Columns.Add(new Column(attr.Name ,"VARCHAR","255"));
				//table.Columns.Add(attr.Name);
			}
			foreach(XmlNode childNode in node.ChildNodes)
			{
				if(childNode.HasChildNodes)
				{ 
					if(childNode.ChildNodes[0].NodeType==System.Xml.XmlNodeType.Text)
						table.Columns.Add(new Column(childNode.Name ,"VARCHAR","255"));
					else
						ProcessNode(childNode,table);
				}
				else
				{
					table.Columns.Add(new Column(childNode.Name ,"VARCHAR","255"));
				}
			}
		}
		private void ProcessNodeForInsertScript(XmlNode node, TableInstance parentTable)
		{
			if(node.Attributes.Count>0)
			{
				Exception ex = new Exception("XmlContainsAttributes");
				//throw ex;
			}

			TableInstance table = _instances.Add(new TableInstance(node.Name));
			table.Parent=parentTable;

			if(_createKeys)
			{
				table.Columns.Add(new ColumnInstance("#ID","UNIQUEIDENTIFIER",Guid.NewGuid().ToString()));

				if(node.ParentNode.Name!="#document")
				{
					if(parentTable.HasColumns)
						table.Columns.Add(new ColumnInstance("#fk_"+ node.ParentNode.Name ,"UNIQUEIDENTIFIER",parentTable.PrimaryKey.ToString()));
					else
					{
						TableInstance parent = table.Parent.Parent;
						if(parent.Name!="root")
							table.Columns.Add(new ColumnInstance("#fk_"+ parent.Name ,"UNIQUEIDENTIFIER", parent.PrimaryKey.ToString()));
					}
				}
			}
			foreach(XmlAttribute attr in node.Attributes)
			{
				table.Columns.Add(new ColumnInstance(attr.Name ,"VARCHAR",attr.Value));
			}
			foreach(XmlNode childNode in node.ChildNodes)
			{
				if(childNode.HasChildNodes)
				{ 
					if(childNode.ChildNodes[0].NodeType==System.Xml.XmlNodeType.Text)
						table.Columns.Add(new ColumnInstance(childNode.Name ,"VARCHAR",childNode.InnerText));
					else
						ProcessNodeForInsertScript(childNode, table);
				}
				else
				{
					if(childNode.ChildNodes.Count>0)
						table.Columns.Add(new ColumnInstance(childNode.Name ,"VARCHAR",childNode.ChildNodes[0].Value));
					else
						table.Columns.Add(new ColumnInstance(childNode.Name ,"VARCHAR",""));
				}
			}
		}
		private bool IsCollectionNode(XmlNode node)
		{
			foreach(XmlNode childNode in node.ChildNodes)
			{
				if(childNode.HasChildNodes)
				{ 
					if(childNode.ChildNodes[0].NodeType==System.Xml.XmlNodeType.Text)
						return false;	
				}
			}
			return true;
		}
		#endregion
		#region Table Classes
		private class Table
		{
			const string STATEMENT = "CREATE TABLE [{0}](\n{1})ON [PRIMARY]\n";
			const string DROP_STATEMENT = "DROP TABLE [{0}]\n";
			
			public Table(string name)
			{
				this.Name=name;
			}
			public Table Parent;
			public bool HasColumns
			{
				get
				{
					foreach(Column c in this.Columns)
					{
						if(c.Name.Substring(0,1)!="#")
							return true;
					}
					return false;
				}
			}
			public string Name;
			public ColumnCollection Columns = new ColumnCollection();
			public string ToSQLString()
			{
				if(this.HasColumns)
					return String.Format(STATEMENT,Name,this.Columns.ToSQLString());
				else
					return "";
			}
			public string ToSQLDropString()
			{
				if(this.HasColumns)
					return String.Format(DROP_STATEMENT,Name);
				else
					return "";
			}
		}
		private class TableCollection :CollectionBase	
		{	
			public virtual void Add(Table newTable)
			{
				this.List.Add(newTable);        
			}
			public virtual Table this[int Index]
			{
				get
				{
					return (Table)this.List[Index];        
				}
			}
			public Table GetTable(string name)
			{
				foreach(Table table in this)
				{
					if(table.Name==name)
						return table;
				}
				Table newTable = new Table(name);
				this.Add(newTable);
				return newTable;
			}
			public string ToSQLString()
			{

				string ret="";
				foreach(Table t in this)
					ret+=t.ToSQLDropString();
				foreach(Table t in this)
					ret+=t.ToSQLString() + "\n";

				return ret;
			}
		}
		private class Column
		{
			const string STATEMENT = "\t[{0}] [{1}] {2} NULL";
			public Column(string name, string type, string length)
			{
				this.Name=name;
				this.Type=type;
				this.Length=length;
			}
			public string Name;
			public string Type;
			public string Length;
			public string ToSQLString()
			{
				if(Length.Length==0)
					return String.Format(STATEMENT,Name,Type,Length);
				else
					return String.Format(STATEMENT,Name,Type,"("+Length+")");
			}
		}
		private class ColumnCollection :CollectionBase	
		{	
			public virtual void Add(Column newColumn)
			{
				foreach(Column column in this)
				{
					if(column.Name==newColumn.Name)
						return;
				}
				this.List.Add(newColumn);        
			}
			public virtual Column this[int Index]
			{
				get
				{
					return (Column)this.List[Index];        
				}
			}
			public string ToSQLString()
			{
				string ret="";
				foreach(Column c in this)
					ret+=c.ToSQLString() + ",\n";

				if(ret.Length>0)
					return ret.Substring(0,ret.Length-2);
				else
					return "";
			}
		}
		#endregion
		#region TableInstance Classes
		private class TableInstance
		{
			const string STATEMENT = "INSERT INTO [{0}]({1})\nVALUES({2})\n";
			public TableInstance(string name)
			{
				this.Name=name;
			}

			public TableInstance Parent;
			public Guid PrimaryKey
			{
				get
				{
					foreach(ColumnInstance c in this.Columns)
					{
						if(c.Name=="#ID")
							return new Guid( c.Value );
					}
					return Guid.Empty;
				}
			}
			public bool HasColumns
			{
				get
				{
					foreach(ColumnInstance c in this.Columns)
					{
						if(c.Name.Substring(0,1)!="#")
							return true;
					}
					return false;
				}
			}
			public string Name;
			public ColumnInstanceCollection Columns = new ColumnInstanceCollection();
			public string ToSQLInsertString()
			{
				if(this.HasColumns)
					return String.Format(STATEMENT,Name,this.Columns.ToSQLColumnString(),this.Columns.ToSQLValueString());
				else
					return "";
			}
		}
		private class TableInstanceCollection :CollectionBase	
		{	
			public virtual TableInstance Add(TableInstance newTable)
			{
				this.List.Add(newTable);
				return newTable;
			}
			public virtual TableInstance this[int Index]
			{
				get
				{
					return (TableInstance)this.List[Index];        
				}
			}
			public TableInstance FindByID(Guid id)
			{
				foreach(TableInstance t in this)
				{
					if(t.PrimaryKey==id)
						return t;
				}
				return null;
			}
			public string ToSQLString()
			{
				string ret="";
				foreach(TableInstance t in this)
					ret+=t.ToSQLInsertString() + "\n";

				return ret;
			}
		}
		private class ColumnInstance
		{
			public ColumnInstance(string name, string type, string columnValue)
			{
				this.Name=name;
				this.Type=type;
				this.Value=columnValue;
			}
			public string Name;
			public string Type;
			public string Value;
			public bool IsSynthetic()
			{
				if(this.Name.Substring(0,1)=="#")
					return true;
				else
					return false;
			}
		}
		private class ColumnInstanceCollection :CollectionBase	
		{	
			public virtual void Add(ColumnInstance newColumn)
			{
				this.List.Add(newColumn);        
			}
			public virtual Column this[int Index]
			{
				get
				{
					return (Column)this.List[Index];        
				}
			}
			public string ToSQLColumnString()
			{
				string ret="";
				
				foreach(ColumnInstance c in this)
				{
					ret+="["+c.Name+"],";
				}

				if(ret.Length>0)
					return ret.Substring(0,ret.Length-1);
				else
					return "";
			}
			public string ToSQLValueString()
			{
				string ret="";
				foreach(ColumnInstance c in this)
					ret+="'"+c.Value.Replace("'","''")+"',";

				if(ret.Length>0)
					return ret.Substring(0,ret.Length-1);
				else
					return "";
			}
		}

		#endregion
	}
}
