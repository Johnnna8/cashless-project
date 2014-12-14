using System.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
//voor memorystream
using System.IO;
using System.Linq;
using System.Web;
using System.Security.Claims;
using nmct.ba.cashlessproject.helper;
using nmct.ba.cashlessproject.api.helper;
using nmct.ba.cashlessproject.model;

namespace nmct.ba.cashlessproject.api.Models
{
    public class CustomerDA
    {
        private static ConnectionStringSettings CreateConnectionString(IEnumerable<Claim> claims)
        {
            string dblogin = claims.FirstOrDefault(c => c.Type == "dblogin").Value;
            string dbpass = claims.FirstOrDefault(c => c.Type == "dbpass").Value;
            string dbname = claims.FirstOrDefault(c => c.Type == "dbname").Value;

            return Database.CreateConnectionString("System.Data.SqlClient", @"JONA\DATAMANAGEMENT", Cryptography.Decrypt(dbname), Cryptography.Decrypt(dblogin), Cryptography.Decrypt(dbpass));
        }

        public static List<Customer> GetCustomers(IEnumerable<Claim> claims)
        {
            List<Customer> customers = new List<Customer>();

            string sql = "SELECT * FROM Customer";
            DbDataReader reader = Database.GetData(Database.GetConnection(CreateConnectionString(claims)), sql);
            while (reader.Read())
            {
                customers.Add(Create(reader));
            }
            reader.Close();

            return customers;
        }

        public static Customer GetCustomer(int id, IEnumerable<Claim> claims)
        {
            Customer customer = new Customer();

            string sql = "SELECT * FROM Customer WHERE ID=@ID";
            DbParameter par1 = Database.AddParameter("ConnectionString", "@ID", id);
            DbDataReader reader = Database.GetData(Database.GetConnection(CreateConnectionString(claims)), sql, par1);

            while (reader.Read())
            {
                customer = Create(reader);
            }
            reader.Close();

            return customer;
        }

        private static Customer Create(IDataRecord record)
        {
            byte[] picture = new byte[0];

            if (!DBNull.Value.Equals(record["Picture"]))
                picture = (byte[])record["Picture"];

            return new Customer()
            {
                ID = Convert.ToInt32(record["ID"]),
                Firstname = record["Firstname"].ToString(),
                Lastname = record["Lastname"].ToString(),
                Street = record["Street"].ToString(),
                StreetNumber = record["StreetNumber"].ToString(),
                Postcode = record["Postcode"].ToString(),
                City = record["City"].ToString(),
                Picture = picture,
                Balance = Convert.ToDouble(record["Balance"])
            };
        }

        public static int InsertCustomer(Customer c, IEnumerable<Claim> claims)
        {
            string sql = "INSERT INTO Customer VALUES(@Firstname,@Lastname,@Street,@StreetNumber,@Postcode,@City,@Picture,@Balance)";
            DbParameter par1 = Database.AddParameter("AdminDB", "@Firstname", c.Firstname);
            DbParameter par2 = Database.AddParameter("AdminDB", "@Lastname", c.Lastname);
            DbParameter par3 = Database.AddParameter("AdminDB", "@Street", c.Street);
            DbParameter par4 = Database.AddParameter("AdminDB", "@StreetNumber", c.StreetNumber);
            DbParameter par5 = Database.AddParameter("AdminDB", "@Postcode", c.Postcode);
            DbParameter par6 = Database.AddParameter("AdminDB", "@City", c.City);
            DbParameter par7 = Database.AddParameter("AdminDB", "@Picture", c.Picture);
            DbParameter par8 = Database.AddParameter("AdminDB", "@Balance", c.Balance);
            return Database.InsertData(Database.GetConnection(CreateConnectionString(claims)), sql, par1, par2, par3, par4, par5, par6, par7, par8);
        }

        public static void UpdateCustomer(Customer c, IEnumerable<Claim> claims)
        {
            string sql = "UPDATE Customer SET Firstname=@Firstname, Lastname=@Lastname, Street=@Street, StreetNumber=@StreetNumber, Postcode=@Postcode, City=@City, Picture=@Picture, Balance=@Balance WHERE ID=@ID";
            DbParameter par1 = Database.AddParameter("AdminDB", "@Firstname", c.Firstname);
            DbParameter par2 = Database.AddParameter("AdminDB", "@Lastname", c.Lastname);
            DbParameter par3 = Database.AddParameter("AdminDB", "@Street", c.Street);
            DbParameter par4 = Database.AddParameter("AdminDB", "@StreetNumber", c.StreetNumber);
            DbParameter par5 = Database.AddParameter("AdminDB", "@Postcode", c.Postcode);
            DbParameter par6 = Database.AddParameter("AdminDB", "@City", c.City);
            DbParameter par7 = Database.AddParameter("AdminDB", "@Picture", c.Picture);
            DbParameter par8 = Database.AddParameter("AdminDB", "@Balance", c.Balance);
            Database.ModifyData(Database.GetConnection(CreateConnectionString(claims)), sql, par1, par2, par3, par4, par5, par6, par7, par8);
        }

        public static void DeleteCustomer(int id, IEnumerable<Claim> claims)
        {
            string sql = "DELETE FROM Customer WHERE ID=@ID";
            DbParameter par1 = Database.AddParameter("AdminDB", "@ID", id);
            DbConnection con = Database.GetConnection(CreateConnectionString(claims));
            Database.ModifyData(con, sql, par1);
        }
    }
}