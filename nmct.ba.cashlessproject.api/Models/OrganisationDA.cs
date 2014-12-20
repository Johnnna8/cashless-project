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
    public class OrganisationDA
    {
        public static Organisation CheckCredentials(string username, string password)
        {
            string sql = "SELECT * FROM Organisations WHERE Login=@Login AND Password=@Password";
            DbParameter par1 = Database.AddParameter("AdminDB", "@Login", Cryptography.Encrypt(username));
            DbParameter par2 = Database.AddParameter("AdminDB", "@Password", Cryptography.Encrypt(password));
            try
            {
                DbDataReader reader = Database.GetData(Database.GetConnection("AdminDB"), sql, par1, par2);
                reader.Read();
                return new Organisation()
                {
                    ID = Int32.Parse(reader["ID"].ToString()),
                    Login = reader["Login"].ToString(),
                    Password = reader["Password"].ToString(),
                    DbName = reader["DbName"].ToString(),
                    DbLogin = reader["DbLogin"].ToString(),
                    DbPassword = reader["DbPassword"].ToString(),
                    OrganisationName = reader["OrganisationName"].ToString(),
                    Street = reader["Street"].ToString(),
                    StreetNumber = reader["StreetNumber"].ToString(),
                    Postcode = reader["Postcode"].ToString(),
                    City = reader["City"].ToString(),
                    Email = reader["Email"].ToString(),
                    Phone = reader["Phone"].ToString()
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public static Boolean checkOldPassword(string dbLogin, string dbPassword, string oldPassword)
        {
            string sql = "SELECT * FROM Organisations WHERE DbLogin=@DbLogin AND DbPassword=@DbPassword AND Password=@Password";

            DbParameter par1 = Database.AddParameter("AdminDB", "@DbLogin", dbLogin);
            DbParameter par2 = Database.AddParameter("AdminDB", "@DbPassword", dbPassword);
            DbParameter par3 = Database.AddParameter("AdminDB", "@Password", Cryptography.Encrypt(oldPassword));

            DbDataReader reader = Database.GetData(Database.GetConnection("AdminDB"), sql, par1, par2, par3);
            return reader.HasRows;
        }

        public static void UpdatePassword(string dbLogin, string dbPassword, string NewPassword)
        {
            string sql = "UPDATE Organisations SET Password=@Password WHERE DbLogin=@DbLogin AND DbPassword=@DbPassword";

            DbParameter par1 = Database.AddParameter("AdminDB", "@DbLogin", dbLogin);
            DbParameter par2 = Database.AddParameter("AdminDB", "@DbPassword", dbPassword);
            DbParameter par3 = Database.AddParameter("AdminDB", "@Password", Cryptography.Encrypt(NewPassword));

            Database.ModifyData(Database.GetConnection("AdminDB"), sql, par1, par2, par3);
        }

    }
}