using System;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace SQLEditor.Config
{
	/// <summary>
	/// Summary description for Settings.
	/// </summary>
	[Serializable]
	public class Settings
	{
		public bool ShowEOLMarkers=false;
		public bool ShowSpaces=false;
		public bool ShowTabs=false;
		public bool ShowLineNumbers=true;
		public bool ShowMatchingBracket=true;
		public bool ShowSplash=true;
		public bool ReadOnlyOutput=false;
		
		public string fontFamily;
		public GraphicsUnit fontGraphicsUnit=System.Drawing.GraphicsUnit.Point;
		public float fontSize=11.25F;
		public FontStyle fontStyle;
		public int TabIndent=4;

		public bool RunWithIOStatistics=false;
		public int  DifferencialPercentage=101;
		public bool  ShowFrmDocumentHeader=true;

		public bool ShowStartPage=false;
		
		public bool Exists()
		{
			string filepath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Settings.config");
			return File.Exists(filepath);
		}
		public static Settings Load()
		{
			Settings _settings=new Settings();
			try
			{
				string filename = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Settings.config");
				XmlSerializer ser = new XmlSerializer(typeof(Settings));
				TextReader reader = new StreamReader(filename);
				_settings = (Settings)ser.Deserialize(reader);
				reader.Close();
				return _settings;
			}
			catch
			{
				_settings.ShowEOLMarkers=false;
				_settings.ShowSpaces=false;
				_settings.ShowTabs=false;
				return _settings;
			}
			
		}
		public void Save()
		{
			string filename = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Settings.config");
			XmlSerializer ser = new XmlSerializer(typeof(Settings));
			TextWriter writer = new StreamWriter(filename);
			ser.Serialize(writer, this);
			writer.Close();
		}
		
		public Font GetFont()
		{
			if(this.fontFamily==null)
				this.fontFamily="Courier New";
			if(this.fontSize<1)
				this.fontSize=11;
			return new Font(this.fontFamily,this.fontSize,this.fontStyle,this.fontGraphicsUnit);
		}
	}
}
