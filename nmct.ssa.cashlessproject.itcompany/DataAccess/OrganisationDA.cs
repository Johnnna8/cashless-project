using nmct.ba.cashlessproject.model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace nmct.ssa.cashlessproject.itcompany.DataAccess
{
    public class OrganisationDA
    {
        private const string CONNECTIONSTRING = "ConnectionString";

        public static List<Organisation> GetOrganisations()
        {
            List<Organisation> organisations = new List<Organisation>();

            string sql = "SELECT * FROM Organisations";

            DbDataReader reader = Database.GetData(CONNECTIONSTRING, sql);
            while (reader.Read())
            {
                organisations.Add(Create(reader));
            }
            reader.Close();

            return organisations;
        }

        public static Organisation GetOrganisationByID(int id)
        {
            string sql = "SELECT * FROM Organisations WHERE ID=@ID";
            DbParameter par1 = Database.AddParameter(CONNECTIONSTRING, "@ID", id);

            DbDataReader reader = Database.GetData(CONNECTIONSTRING, sql, par1);
            Organisation o = null;
            while (reader.Read())
            {
                o = Create(reader);

            }
            reader.Close();

            return o;
        }

        private static Organisation Create(IDataRecord record)
        {
            return new Organisation()
            {
                ID = int.Parse(record["ID"].ToString()),
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

        public static void NewOrganisation(Organisation o)
        {
            string sql = "INSERT INTO Organisations (Login, Password, DbName, DbLogin, DbPassword, OrganisationName, Street, StreetNumber, Postcode, City, Email, Phone) ";
            sql += "VALUES (@Login, @Password, @DbName, @DbLogin, @DbPassword, @OrganisationName, @Street, @StreetNumber, @Postcode, @City, @Email, @Phone)";

            DbParameter par1 = Database.AddParameter(CONNECTIONSTRING, "@Login", o.Login);
            DbParameter par2 = Database.AddParameter(CONNECTIONSTRING, "@Password", o.Password);
            DbParameter par3 = Database.AddParameter(CONNECTIONSTRING, "@DbName", o.DbName);
            DbParameter par4 = Database.AddParameter(CONNECTIONSTRING, "@DbLogin", o.DbLogin);
            DbParameter par5 = Database.AddParameter(CONNECTIONSTRING, "@DbPassword", o.DbPassword);
            DbParameter par6 = Database.AddParameter(CONNECTIONSTRING, "@OrganisationName", o.OrganisationName);
            DbParameter par7 = Database.AddParameter(CONNECTIONSTRING, "@Street", o.Street);
            DbParameter par8 = Database.AddParameter(CONNECTIONSTRING, "@StreetNumber", o.StreetNumber);
            DbParameter par9 = Database.AddParameter(CONNECTIONSTRING, "@Postcode", o.Postcode);
            DbParameter par10 = Database.AddParameter(CONNECTIONSTRING, "@City", o.City);
            DbParameter par11 = Database.AddParameter(CONNECTIONSTRING, "@Email", o.Email);
            DbParameter par12 = Database.AddParameter(CONNECTIONSTRING, "@Phone", o.Phone);
            DbParameter par13 = Database.AddParameter(CONNECTIONSTRING, "@ID", o.ID);

            Database.InsertData(CONNECTIONSTRING, sql, par1, par2, par3, par4, par5, par6, par7, par8, par9, par10, par11, par12, par13);
        }

        public static void EditOrganisation(Organisation o)
        {
            string sql = "UPDATE Organisations SET Login=@Login, Password=@Password, DbName=@DbName, DbLogin=@DbLogin, DbPassword=@DbPassword, ";
            sql += "OrganisationName=@OrganisationName, Street=@Street, StreetNumber=@StreetNumber, Postcode=@Postcode, City=@City, Email=@Email, Phone=@Phone ";
            sql += "WHERE ID=@ID";

            DbParameter par1 = Database.AddParameter(CONNECTIONSTRING, "@Login", o.Login);
            DbParameter par2 = Database.AddParameter(CONNECTIONSTRING, "@Password", o.Password);
            DbParameter par3 = Database.AddParameter(CONNECTIONSTRING, "@DbName", o.DbName);
            DbParameter par4 = Database.AddParameter(CONNECTIONSTRING, "@DbLogin", o.DbLogin);
            DbParameter par5 = Database.AddParameter(CONNECTIONSTRING, "@DbPassword", o.DbPassword);
            DbParameter par6 = Database.AddParameter(CONNECTIONSTRING, "@OrganisationName", o.OrganisationName);
            DbParameter par7 = Database.AddParameter(CONNECTIONSTRING, "@Street", o.Street);
            DbParameter par8 = Database.AddParameter(CONNECTIONSTRING, "@StreetNumber", o.StreetNumber);
            DbParameter par9 = Database.AddParameter(CONNECTIONSTRING, "@Postcode", o.Postcode);
            DbParameter par10 = Database.AddParameter(CONNECTIONSTRING, "@City", o.City);
            DbParameter par11 = Database.AddParameter(CONNECTIONSTRING, "@Email", o.Email);
            DbParameter par12 = Database.AddParameter(CONNECTIONSTRING, "@Phone", o.Phone);
            DbParameter par13 = Database.AddParameter(CONNECTIONSTRING, "@ID", o.ID);

            Database.ModifyData(CONNECTIONSTRING, sql, par1, par2, par3, par4, par5, par6, par7, par8, par9, par10, par11, par12, par13);
        }

        public static void DeleteOrganisation(int id)
        {
            string sql = "DELETE FROM Organisations WHERE ID = @ID";
            DbParameter par1 = Database.AddParameter(CONNECTIONSTRING, "@ID", id);
            Database.ModifyData(CONNECTIONSTRING, sql, par1);
        }
    }
}