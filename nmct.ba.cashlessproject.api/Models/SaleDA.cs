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
    public class SaleDA
    {
        private static ConnectionStringSettings CreateConnectionString(IEnumerable<Claim> claims)
        {
            string dblogin = claims.FirstOrDefault(c => c.Type == "dblogin").Value;
            string dbpass = claims.FirstOrDefault(c => c.Type == "dbpass").Value;
            string dbname = claims.FirstOrDefault(c => c.Type == "dbname").Value;

            return Database.CreateConnectionString("System.Data.SqlClient", @"JONA\DATAMANAGEMENT", Cryptography.Decrypt(dbname), Cryptography.Decrypt(dblogin), Cryptography.Decrypt(dbpass));
        }

        public static int AddSale(Sale s, IEnumerable<Claim> claims)
        {
            string sql = "INSERT INTO Sale(Timestamp, CustomerID,RegisterID,ProductID,Amount,TotalPrice) ";
            sql += "VALUES(@Timestamp,@CustomerID,@RegisterID,@ProductID,@Amount,@TotalPrice)";

            DbParameter par1 = Database.AddParameter("AdminDB", "@Timestamp", s.Timestamp);
            DbParameter par2 = Database.AddParameter("AdminDB", "@CustomerID", s.Customer.ID);
            DbParameter par3 = Database.AddParameter("AdminDB", "@RegisterID", s.Register.ID);
            DbParameter par4 = Database.AddParameter("AdminDB", "@ProductID", s.Product.ID);
            DbParameter par5 = Database.AddParameter("AdminDB", "@Amount", s.Amount);
            DbParameter par6 = Database.AddParameter("AdminDB", "@TotalPrice", s.TotalPrice);

            return Database.InsertData(Database.GetConnection(CreateConnectionString(claims)), sql, par1, par2, par3, par4, par5, par6);
        }
    }
}