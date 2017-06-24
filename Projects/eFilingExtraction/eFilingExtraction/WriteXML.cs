using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace eFillingExtraction
{
    class WriteXML
    {
        public static void WriteXmlToFile(DataSet WriteDataSet,string FileName)
        {
            if (WriteDataSet == null) { return; }
            // Create the FileStream to write with.
            System.IO.FileStream fileStream = new System.IO.FileStream(FileName, System.IO.FileMode.Create);

            // Create an XmlTextWriter with the fileStream.
            System.Xml.XmlTextWriter xmlWriter = new System.Xml.XmlTextWriter(fileStream, System.Text.Encoding.Unicode);

            // Write to the file with the WriteXml method.
            WriteDataSet.WriteXml(xmlWriter);
            xmlWriter.Close();
        }
    }
}
