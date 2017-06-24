using System;
using System.Collections.Generic;
using System.Text;

namespace U2TPlus.DAL.Interface
{
    public interface ICompanyInfo
    {
        string ConnectionString { get; set; }
        string CompanyDBName { get;set;}
        string CompanyMasterConnectionString { get; set; }
    }
}
