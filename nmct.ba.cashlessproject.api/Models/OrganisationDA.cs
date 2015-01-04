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
    public class OrganisationDA
    {
        //gekopieerd van Frederik Duchi
        public static Organisation CheckCredentials(string username, string password)
        {
            string sql = "SELECT * FROM Organisations WHERE Login=@Login AND Password=@Password";
            DbParameter par1 = Database.AddParameter("AdminDB", "@Login", Cryptography.Encrypt(username));
            DbParameter par2 = Database.AddParameter("AdminDB", "@Password", Cryptography.Encrypt(password));
            try
            {
                DbDataReader reader = Database.GetData(Database.GetConnection("AdminDB"), sql, par1, par2);
                reader.Read();
                return Create(reader);
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

        public static Organisation getOrganisation(string dbLogin, string dbPassword)
        {
            Organisation organisation = new Organisation();
            string sql = "SELECT * FROM Organisations WHERE DbLogin=@DbLogin AND DbPassword=@DbPassword";
            DbParameter par1 = Database.AddParameter("AdminDB", "@DbLogin", dbLogin);
            DbParameter par2 = Database.AddParameter("AdminDB", "@DbPassword", dbPassword);
            DbDataReader reader = Database.GetData(Database.GetConnection("AdminDB"), sql, par1, par2);

            while (reader.Read())
            {
                organisation = Create(reader);
            }
            reader.Close();

            return organisation;
        }

        private static Organisation Create(IDataRecord record)
        {
            return new Organisation()
            {
                ID = Int32.Parse(record["ID"].ToString()),
                Login = record["Login"].ToString(),
                Password = record["Password"].ToString(),
                DbName = record["DbName"].ToString(),
                DbLogin = record["DbLogin"].ToString(),
                DbPassword = record["DbPassword"].ToString(),
                OrganisationName = record["OrganisationName"].ToString(),
                Street = record["Street"].ToString(),
                StreetNumber = record["StreetNumber"].ToString(),
                Postcode = record["Postcode"].ToString(),
                City = record["City"].ToString(),
                Email = record["Email"].ToString(),
                Phone = record["Phone"].ToString()
            };
        }

    }
}