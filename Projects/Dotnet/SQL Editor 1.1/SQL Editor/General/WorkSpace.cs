using System;
using System.Collections;
using System.Xml.Serialization;
using System.IO;

namespace SQLEditor.General.WorkSpace
{
	[Serializable]
	public class WorkSpace
	{
		public string Name;
		public WorkSpaceItemCollection WorkSpaceItems = new WorkSpaceItemCollection();
	}
	[Serializable]
	public class WorkSpaceItem
	{
		public string FileName;
		public string FilePath;
		
	}
	[Serializable]
	public class WorkSpaceItemCollection :CollectionBase
	{
		public virtual void Add(WorkSpaceItem NewWorkSpaceItem)
		{
			foreach(WorkSpaceItem w in this.List)
			{
				if(w.FilePath==NewWorkSpaceItem.FilePath)
					return;
			}
			this.List.Add(NewWorkSpaceItem);
		}
		public virtual WorkSpaceItem this[int Index]{get{return (WorkSpaceItem)this.List[Index];}}
	}
	[Serializable]
	public class WorkSpaceCollection :CollectionBase
	{
		public virtual int Add(WorkSpace workSpace)
		{
			return this.List.Add(workSpace);
		}		
		public virtual WorkSpace this[int Index]
		{
			get{return (WorkSpace)this.List[Index];}
		}
	}
	public abstract class WorkSpaceFactory
	{
		public static WorkSpaceCollection Load(string configFile)
		{
			try
			{
				XmlSerializer ser = new XmlSerializer(typeof(WorkSpaceCollection));
				TextReader reader = new StreamReader(configFile);
				WorkSpaceCollection workSpaceCollection = (WorkSpaceCollection)ser.Deserialize(reader);
				reader.Close();
				return workSpaceCollection;
			}
			catch
			{
				return new WorkSpaceCollection();
			}
		}
		public static void  Save(string configFile, WorkSpaceCollection workSpaceCollection)
		{
			try
			{
				XmlSerializer ser = new XmlSerializer(typeof(WorkSpaceCollection));
				TextWriter writer = new StreamWriter(configFile);
				ser.Serialize(writer, workSpaceCollection);
				writer.Close();
			}
			catch
			{
				return;
			}
		}
	}

	public class WorkSpaceException :Exception
	{
		public WorkSpaceException(string message):base()
		{}
	}
}
