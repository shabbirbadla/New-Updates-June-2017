using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace U2TPlus.DAL.Interface
{
    public interface IGenerateTransXML
    {
        string PresentTransTableName { get;set; }
        string dbError { get; set; }
        DataTable TallyTagsTable { get; set;}
        string ConnectionString { get; set; }
        string CompanyDBName { get; set;}
        DataSet ReturnDataSet { get; set; }
        string GeneratingOptions { get; set;}
        string Configuration_ID { get;set; }
        string GenerateFromDate { get;set; }
        string GenerateToDate { get;set; }
        bool isGeneratesDefaultSettings { get;set;}
        string GenaretingTransName { get;set; }
        string[] tablename { get;set; }
        string TableEntryType { get;set; }
    }
}
