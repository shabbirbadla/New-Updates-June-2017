using System;

namespace SQLEditor.General
{
	/// <summary>
	/// Summary description for Common.
	/// </summary>
	
	struct WordAndPosition
	{
		public string Word;
		public int Position;
		public int Length;
		public override string ToString()
		{
			string s = "Word = " + Word + ", Position = " + Position + ", Length = " + Length + "\n";
			return s;
		}
	}
}
