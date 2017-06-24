using System;
using System.Collections.Generic;
using System.Text;
using System.Resources;
using System.Reflection;

namespace Udyog.Application.License
{
    public class ResourceHelper
    {
        public static string GetValue(string resourceFileName, string key)
        {
            ResourceManager resMgr = new ResourceManager("Udyog.Application.License." + resourceFileName, Assembly.GetCallingAssembly());
            string value = resMgr.GetString(key);
            return value;
        }

        public static string GetFormatedValue(string resourceFileName, string key, params object[] args)
        {
            ResourceManager resMgr = new ResourceManager("Udyog.Application.License." + resourceFileName, Assembly.GetCallingAssembly());
            string value = resMgr.GetString(key);
            return string.Format(value, args);
        }
    }
}
