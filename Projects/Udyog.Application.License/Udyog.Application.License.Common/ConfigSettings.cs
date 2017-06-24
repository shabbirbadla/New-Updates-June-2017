using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Reflection;

namespace Udyog.Application.License
{
    public class ConfigSettings
    {

        private static string NodePath = "//system.serviceModel//client//endpoint";
        private ConfigSettings() { }

        public static string GetEndpointAddress(string configPath)
        {
            return ConfigSettings.loadConfigDocument(configPath).SelectSingleNode(NodePath).Attributes["address"].Value;
        }

        //public static void SaveEndpointAddress(string endpointAddress)
        //{
        //    // load config document for current assembly 
        //    XmlDocument doc = loadConfigDocument();

        //    // retrieve appSettings node 
        //    XmlNode node = doc.SelectSingleNode(NodePath);

        //    if (node == null)
        //        throw new InvalidOperationException("Error. Could not find endpoint node in config file.");

        //    try
        //    {
        //        // select the 'add' element that contains the key 
        //        //XmlElement elem = (XmlElement)node.SelectSingleNode(string.Format("//add[@key='{0}']", key)); 
        //        node.Attributes["address"].Value = endpointAddress;

        //        doc.Save(getConfigFilePath());
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}

        public static XmlDocument loadConfigDocument(string configPath)
        {
            XmlDocument doc = null;
            try
            {
                doc = new XmlDocument();
                doc.Load(GetConfigFilePath(configPath));
                return doc;
            }
            catch (System.IO.FileNotFoundException e)
            {
                throw new Exception("No configuration file found.", e);
            }
        }

        public static string GetConfigFilePath(string configPath)
        {
            return configPath + ".config";
        }
    }
}
