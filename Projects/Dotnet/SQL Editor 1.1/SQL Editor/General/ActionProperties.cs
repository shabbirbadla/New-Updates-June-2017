using System;
using System.Data;
using System.ComponentModel;
using System.Xml.Serialization;

namespace SQLEditor.General.TestBench
{
	[Serializable]
	public class ActionProperties
	{
		public ActionProperties(
			string name,
			string description,
			DateTime executionBegin,
			DateTime executionEnd,
			bool exceptionOccurd,
			DataSet dataSet,
			DateTime totalExecutionBegin,
			DateTime totalExecutionEnd)
		{
			_name=name;
			_description=description;
			_executionBegin=executionBegin;
			_executionEnd=executionEnd;
			_exceptionOccurd=exceptionOccurd;
			_dataSet=dataSet;
			_totalExecutionBegin=totalExecutionBegin;
			_totalExecutionEnd=totalExecutionEnd;
		
			if(_dataSet!=null)
			{
				foreach(DataTable dt in _dataSet.Tables)
					_totalRetrievedRows+=dt.Rows.Count;
			}
			
			_executionTime=_executionEnd.Subtract(_executionBegin);
			_totalExecutionTime=_totalExecutionEnd.Subtract(_totalExecutionBegin);
		}

		string _name;
		string _description;
		TimeSpan _executionTime;
		DateTime _executionBegin;
		DateTime _executionEnd;
		TimeSpan _totalExecutionTime;
		DateTime _totalExecutionBegin;
		DateTime _totalExecutionEnd;

		int _totalRetrievedRows;
		bool _exceptionOccurd;
		DataSet _dataSet;

		[DescriptionAttribute("The name of the action."),
		CategoryAttribute("General Settings"),DefaultValueAttribute(true)]
		public string Name
		{
			get{return _name;}

		}
		[DescriptionAttribute("The description of the action."),
		CategoryAttribute("General Settings"),DefaultValueAttribute(true)]
		public string Description
		{
			get{return _description;}

		}

		[DescriptionAttribute("The total time to execute this action (HH:MM:SS:Milliseconds)."),
		CategoryAttribute("Execution time result"),DefaultValueAttribute(true)]
		public TimeSpan ExecutionTime
		{
			get{return _executionTime;}
			
		}
		[DescriptionAttribute("The time when this action stared executing."),
		CategoryAttribute("Execution time result"),DefaultValueAttribute(true)]
		public DateTime ExecutionBegin
		{
			get{return _executionBegin;}
			
		}
		[DescriptionAttribute("The time when this action stopped executing."),
		CategoryAttribute("Execution time result"),DefaultValueAttribute(true)]
		public DateTime ExecutionEnd
		{
			get{return _executionEnd;}
		}
		[DescriptionAttribute("The total time to execute this etire action, subactions included (HH:MM:SS:Milliseconds)."),
		CategoryAttribute("Total execution time result"),DefaultValueAttribute(true)]
		public TimeSpan TotalExecutionTime
		{
			get{return _totalExecutionTime;}
			
		}
		[DescriptionAttribute("The time when this action stared executing."),
		CategoryAttribute("Total execution time result"),DefaultValueAttribute(true)]
		public DateTime TotalExecutionBegin
		{
			get{return _totalExecutionBegin;}
			
		}
		[DescriptionAttribute("The time when this action stopped executing."),
		CategoryAttribute("Total execution time result"),DefaultValueAttribute(true)]
		public DateTime TotalExecutionEnd
		{
			get{return _totalExecutionEnd;}
		}

		[DescriptionAttribute("If the action executed a script, and returned any data, this is where you'd find the result."),
		CategoryAttribute("Data result"),DefaultValueAttribute(true)]
		public DataSet DataSet
		{
			get{return _dataSet;}
		}
		[DescriptionAttribute("Number of rows returned from all queries."),
		CategoryAttribute("Data result"),DefaultValueAttribute(true)]
		public int TotalRetrievedRows
		{
			get{return _totalRetrievedRows;}
		}

		[DescriptionAttribute("If the action executed without errors, this value is [False]."),
		CategoryAttribute("Exceptions"),DefaultValueAttribute(true)]
		public bool ExceptionOccurd
		{
			get{return _exceptionOccurd;}
		}
	}
}
