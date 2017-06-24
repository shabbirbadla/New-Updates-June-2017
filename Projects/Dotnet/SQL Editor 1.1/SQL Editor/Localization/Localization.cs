using System;
using System.Reflection;
using System.Resources;

namespace SQLEditor
{
	/// <summary>
	/// Summary description for Localization.
	/// </summary>
	public class Localization
	{
		private static ResourceManager stringsResource;

		static Localization()
		{
			Assembly assembly = Assembly.GetExecutingAssembly();
			stringsResource = new System.Resources.ResourceManager("SQLEditor.Localization.Strings", assembly);
		}
		public static string GetString(string stringCode)
		{
			if (stringsResource == null)
				return "[Missing localized string]";

			string localString = (string)stringsResource.GetObject(stringCode);
			if (localString == null) return "[Missing localized string]";

			return localString.Replace("\\n","\n");
		}
	}
}
