using nmct.ba.cashlessproject.api.helper;
using nmct.ba.cashlessproject.helper;
using nmct.ba.cashlessproject.model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace nmct.ba.cashlessproject.api.Models
{
    public class ErrorlogDA
    {
        private static ConnectionStringSettings CreateConnectionString(IEnumerable<Claim> claims)
        {
            string dblogin = claims.FirstOrDefault(c => c.Type == "dblogin").Value;
            string dbpass = claims.FirstOrDefault(c => c.Type == "dbpass").Value;
            string dbname = claims.FirstOrDefault(c => c.Type == "dbname").Value;

            return Database.CreateConnectionString("System.Data.SqlClient", @"JONA\DATAMANAGEMENT", Cryptography.Decrypt(dbname), Cryptography.Decrypt(dblogin), Cryptography.Decrypt(dbpass));
        }

        public static int InsertError(ErrorLog el, IEnumerable<Claim> claims)
        {
            string sql = "INSERT INTO ErrorLog VALUES(@RegisterID, @Timestamp, @Message, @Stacktrace)";
            DbParameter par1 = Database.AddParameter("AdminDB", "@RegisterID", el.Register.ID);
            DbParameter par2 = Database.AddParameter("AdminDB", "@Timestamp", el.Timestamp.ToUnixTimestamp());
            DbParameter par3 = Database.AddParameter("AdminDB", "@Message", el.Message);
            DbParameter par4 = Database.AddParameter("AdminDB", "@Stacktrace", el.Stacktrace);
            return Database.InsertData(Database.GetConnection(CreateConnectionString(claims)), sql, par1, par2, par3, par4);
        }
    }
}