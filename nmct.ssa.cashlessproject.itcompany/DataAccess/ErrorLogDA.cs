using nmct.ba.cashlessproject.helper;
using nmct.ba.cashlessproject.model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace nmct.ssa.cashlessproject.itcompany.DataAccess
{
    public class ErrorLogDA
    {
        private const string CONNECTIONSTRING = "ConnectionString";

        public static List<ErrorLog> GetErrorLogs()
        {
            List<ErrorLog> ErrorLogs = new List<ErrorLog>();

            string sql = "SELECT el.ID as ErrorLogID, el.RegisterID, el.Timestamp, el.Message, el.Stacktrace, ";
            sql += "r.RegisterName, r.Device, r.PurchaseDate, r.ExpiresDate ";
            sql += "FROM Errorlog as el ";
            sql += "INNER JOIN Register as r ON el.RegisterID = r.ID";

            DbDataReader reader = Database.GetData(CONNECTIONSTRING, sql);
            while (reader.Read())
            {
                ErrorLogs.Add(Create(reader));
            }
            reader.Close();

            return ErrorLogs;
        }

        private static ErrorLog Create(IDataRecord reader)
        {
            return new ErrorLog()
            {
                ID = Convert.ToInt32(reader["ErrorLogID"]),
                Register = new RegisterCompany()
                {
                    ID = Convert.ToInt32(reader["RegisterID"].ToString()),
                    RegisterName = reader["RegisterName"].ToString(),
                    Device = reader["Device"].ToString(),
                    PurchaseDate = DateTime.Now.ToDateTime(Convert.ToInt32(reader["PurchaseDate"])),
                    ExpiresDate = DateTime.Now.ToDateTime(Convert.ToInt32(reader["ExpiresDate"]))
                },
                Timestamp = DateTime.Now.ToDateTime(Convert.ToInt32(reader["Timestamp"])),
                Message = reader["Message"].ToString(),
                Stacktrace = reader["Stacktrace"].ToString()
            };
        }
    }
}