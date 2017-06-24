using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;

namespace udDataTableQuery
{
    public class cSelectDistinct
    {
        public DataTable SelectDistinct(DataTable SourceTable, string filtcond, params string[] FieldNames)
        {
            object[] lastValues;
            DataTable newTable;
            DataRow[] orderedRows;

            if (FieldNames == null || FieldNames.Length == 0)
                throw new ArgumentNullException("FieldNames");

            lastValues = new object[FieldNames.Length];
            newTable = new DataTable();

            foreach (string fieldName in FieldNames)
            {
                if (string.IsNullOrEmpty(fieldName)==false)
                {
                    newTable.Columns.Add(fieldName, SourceTable.Columns[fieldName].DataType);
                }
            }
            orderedRows = SourceTable.Select(filtcond, string.Join(", ", FieldNames));

            foreach (DataRow row in orderedRows)
            {
                if (!fieldValuesAreEqual(lastValues, row, FieldNames))
                {
                    newTable.Rows.Add(createRowClone(row, newTable.NewRow(), FieldNames));
                    setLastValues(lastValues, row, FieldNames);
                }

            }
            //}
            return newTable;
        }
        private static bool fieldValuesAreEqual(object[] lastValues, DataRow currentRow, string[] fieldNames)
        {
            bool areEqual = true;

            for (int i = 0; i < fieldNames.Length; i++)
            {
                if (lastValues[i] == null || !lastValues[i].Equals(currentRow[fieldNames[i]]))
                {
                    areEqual = false;
                    break;
                }
            }

            return areEqual;
        }
        private static DataRow createRowClone(DataRow sourceRow, DataRow newRow, string[] fieldNames)
        {
            foreach (string field in fieldNames)
            {
                newRow[field] = sourceRow[field];
            }
            return newRow;
        }
        private static void setLastValues(object[] lastValues, DataRow sourceRow, string[] fieldNames)
        {
            for (int i = 0; i < fieldNames.Length; i++)
            {
                lastValues[i] = sourceRow[fieldNames[i]];
            }


        }
    }
    public class cFilterDataTable
    {
        public DataTable FilterDataTable(DataTable SourceTable, string filtcond, params string[] FieldNames)
        {

            //object[] lastValues;
            //DataTable newTable;
            //DataRow[] orderedRows;
            //newTable = new DataTable();
            //if (FieldNames == null || FieldNames.Length == 0)
            //{
            //    string[] FieldNames1 = new string[1024]; //Max Sql Table Columns
            //    int cnt = 0;
            //    foreach (DataColumn dtcl in SourceTable.Columns)
            //    {
            //        FieldNames1[cnt] = dtcl.ColumnName;
            //        newTable.Columns.Add(dtcl.ColumnName, SourceTable.Columns[dtcl.ColumnName].DataType);
            //        cnt = cnt + 1;
            //    }
            //    FieldNames = FieldNames1;
            //}
            //else
            //{
            //    foreach (string fieldName in FieldNames)
            //    {
            //        newTable.Columns.Add(fieldName, SourceTable.Columns[fieldName].DataType);
            //    }
            //}
            //object[] lastValues;
            DataTable newTable;
            DataRow[] orderedRows;

            if (FieldNames == null || FieldNames.Length == 0)
                throw new ArgumentNullException("FieldNames");

            //lastValues = new object[FieldNames.Length];
            newTable = new DataTable();

            foreach (string fieldName in FieldNames)
            {
                if (string.IsNullOrEmpty(fieldName) == false)
                {
                    newTable.Columns.Add(fieldName, SourceTable.Columns[fieldName].DataType);
                }
            }
            orderedRows = SourceTable.Select(filtcond,"");

            foreach (DataRow row in orderedRows)
            {
                    newTable.Rows.Add(createRowClone(row, newTable.NewRow(), FieldNames));
            }
            //}
            return newTable;
        }
        private static bool fieldValuesAreEqual(object[] lastValues, DataRow currentRow, string[] fieldNames)
        {
            bool areEqual = true;

            for (int i = 0; i < fieldNames.Length; i++)
            {
                if (lastValues[i] == null || !lastValues[i].Equals(currentRow[fieldNames[i]]))
                {
                    areEqual = false;
                    break;
                }
            }

            return areEqual;
        }
        private static DataRow createRowClone(DataRow sourceRow, DataRow newRow, string[] fieldNames)
        {
            foreach (string field in fieldNames)
            {
                if (field != null)
                {
                    newRow[field] = sourceRow[field];
                }
            }
            return newRow;
        }
        private static void setLastValues(object[] lastValues, DataRow sourceRow, string[] fieldNames)
        {
            for (int i = 0; i < fieldNames.Length; i++)
            {
                lastValues[i] = sourceRow[fieldNames[i]];
            }


        }
    }
    public class cCalc
    {
        public Decimal funcSum(DataTable SourceTable, string vFldNm, string filtcond)
        {
            string[] FieldNames = { vFldNm };

            DataRow[] orderedRows = SourceTable.Select(filtcond, string.Join(", ", FieldNames));

            Decimal vTotFld = 0;
            foreach (DataRow drfunSum in orderedRows)
            {
                if (drfunSum[vFldNm] != DBNull.Value )
                {
                    vTotFld = vTotFld + Convert.ToDecimal(drfunSum[vFldNm]);
                }
            }
            return vTotFld;
        }
    }
}
