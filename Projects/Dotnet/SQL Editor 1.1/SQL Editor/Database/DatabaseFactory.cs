// *******************************************************************************
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
// *******************************************************************************
using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using Oracle.DataAccess.Client;
using MySql.Data.MySqlClient;
using Sybase.Data.AseClient;

namespace QueryCommander.Database
{
	/// <summary>
	/// Summary description for DatabaseFactory.
	/// </summary>
	public abstract class DatabaseFactory
	{
		public static IDatabaseManager CreateNew(IDbConnection dataConnection)
		{
			if(dataConnection is SqlConnection)
				return new QueryCommander.Database.Microsoft.Sql2000.DataManager();
			else if(dataConnection is OleDbConnection)
				return new QueryCommander.Database.Microsoft.Sql2000.OleDbDataManager();
			else if(dataConnection is OracleConnection)
				return new QueryCommander.Database.Oracle._9i.DataManager();
			else if(dataConnection is MySqlConnection)
				return new QueryCommander.Database.MySQL._4x.DataManager();
			else if(dataConnection is AseConnection)
				return new QueryCommander.Database.Sybase.ASE.DataManager();

			else
				return null;
		}
	}
}
