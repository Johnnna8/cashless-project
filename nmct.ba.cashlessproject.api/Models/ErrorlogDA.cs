using nmct.ba.cashlessproject.api.helper;
using nmct.ba.cashlessproject.helper;
using nmct.ba.cashlessproject.model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
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
            string sql = "INSERT INTO Errorlog VALUES(@RegisterID, @Timestamp, @Message, @Stacktrace)";
            DbParameter par1 = Database.AddParameter("AdminDB", "@RegisterID", el.Register.ID);
            DbParameter par2 = Database.AddParameter("AdminDB", "@Timestamp", el.Timestamp.ToUnixTimestamp());
            DbParameter par3 = Database.AddParameter("AdminDB", "@Message", el.Message);
            DbParameter par4 = Database.AddParameter("AdminDB", "@Stacktrace", el.Stacktrace);
            return Database.InsertData(Database.GetConnection(CreateConnectionString(claims)), sql, par1, par2, par3, par4);
        }

        public static List<ErrorLog> GetErrorLogs(IEnumerable<Claim> claims)
        {
            List<ErrorLog> errorLogs = new List<ErrorLog>();
            string sql = "SELECT * FROM Errorlog";
            DbDataReader reader = Database.GetData(Database.GetConnection(CreateConnectionString(claims)), sql);

            while (reader.Read())
            {
                errorLogs.Add(Create(reader));
            }
            reader.Close();

            return errorLogs;
        }

        private static ErrorLog Create(IDataRecord record)
        {
            return new ErrorLog()
            {
                ID = Convert.ToInt32(record["ID"]),
                Register = new RegisterCompany() {
                    ID = Convert.ToInt32(record["RegisterID"])
                },
                Message = record["Message"].ToString(),
                Timestamp = DateTime.Now,
                Stacktrace = record["Stacktrace"].ToString()
            };
        }

        public static void DeleteErrors(IEnumerable<Claim> claims)
        {
            string sql = "DELETE FROM Errorlog";
            Database.ModifyData(Database.GetConnection(CreateConnectionString(claims)), sql);
        }
    }
}