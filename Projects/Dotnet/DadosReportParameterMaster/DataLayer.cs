using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace QualityControlProcess
{
    class DataLayer
    {
        public static string connectionstring
        {
            get
            {
                return System.Configuration.ConfigurationManager.ConnectionStrings["Connparamater"].ConnectionString;

            }

        }
        
    }
}
