using nmct.ba.cashlessproject.api.helper;
using nmct.ba.cashlessproject.helper;
using nmct.ba.cashlessproject.model;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace nmct.ba.cashlessproject.api.Models
{
    public class ErrorlogAdminDA
    {
        public static int InsertError(ErrorLog el)
        {
            string sql = "INSERT INTO Errorlog(RegisterID, Timestamp, Message, Stacktrace) VALUES(@RegisterID, @Timestamp, @Message, @Stacktrace)";
            DbParameter par1 = Database.AddParameter("AdminDB", "@RegisterID", el.Register.ID);
            DbParameter par2 = Database.AddParameter("AdminDB", "@Timestamp", el.Timestamp.ToUnixTimestamp());
            DbParameter par3 = Database.AddParameter("AdminDB", "@Message", el.Message);
            DbParameter par4 = Database.AddParameter("AdminDB", "@Stacktrace", el.Stacktrace);
            return Database.InsertData(Database.GetConnection("AdminDB"), sql, par1, par2, par3, par4);
        }
    }
}