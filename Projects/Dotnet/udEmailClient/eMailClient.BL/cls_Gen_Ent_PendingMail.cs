using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace eMailClient.BLL
{
    public class cls_Gen_Ent_PendingMail
    {
        #region Properties
        private DataSet dsSelect;

        public DataSet DsSelect
        {
            get { return dsSelect; }
            set { dsSelect = value; }
        }

        private string id;

        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        #endregion
    }
}
