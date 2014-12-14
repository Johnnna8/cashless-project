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
    public class ProductDA
    {
        private static ConnectionStringSettings CreateConnectionString(IEnumerable<Claim> claims)
        {
            string dblogin = claims.FirstOrDefault(c => c.Type == "dblogin").Value;
            string dbpass = claims.FirstOrDefault(c => c.Type == "dbpass").Value;
            string dbname = claims.FirstOrDefault(c => c.Type == "dbname").Value;

            return Database.CreateConnectionString("System.Data.SqlClient", @"JONA\DATAMANAGEMENT", Cryptography.Decrypt(dbname), Cryptography.Decrypt(dblogin), Cryptography.Decrypt(dbpass));
        }

        public static List<Product> GetProducts(IEnumerable<Claim> claims)
        {
            List<Product> products = new List<Product>();
            string sql = "SELECT * FROM Product";
            DbDataReader reader = Database.GetData(Database.GetConnection(CreateConnectionString(claims)), sql);

            while (reader.Read())
            {
                products.Add(Create(reader));
            }
            reader.Close();

            return products;
        }

        public static Product GetProduct(int id, IEnumerable<Claim> claims)
        {
            Product product = new Product();
            string sql = "SELECT * FROM Product WHERE ID=@ID";
            DbParameter par1 = Database.AddParameter("AdminDB", "@ID", id);
            DbDataReader reader = Database.GetData(Database.GetConnection(CreateConnectionString(claims)), sql, par1);

            while (reader.Read())
            {
                product = Create(reader);
            }
            reader.Close();

            return product;
        }

        private static Product Create(IDataRecord record)
        {
            return new Product()
            {
                ID = Convert.ToInt32(record["ID"]),
                ProductName = record["ProductName"].ToString(),
                Price = Convert.ToDouble(record["Price"])
            };
        }

        public static int InsertProduct(Product p, IEnumerable<Claim> claims)
        {
            string sql = "INSERT INTO Product VALUES(@ProductName, @Price)";
            DbParameter par1 = Database.AddParameter("AdminDB", "@ProductName", p.ProductName);
            DbParameter par2 = Database.AddParameter("AdminDB", "@Price", p.Price);
            return Database.InsertData(Database.GetConnection(CreateConnectionString(claims)), sql, par1, par2);
        }

        public static void UpdateProduct(Product p, IEnumerable<Claim> claims)
        {
            string sql = "UPDATE Product SET ProductName=@ProductName, Price=@Price WHERE ID=@ID";
            DbParameter par1 = Database.AddParameter("AdminDB", "@ProductName", p.ProductName);
            DbParameter par2 = Database.AddParameter("AdminDB", "@Price", p.Price);
            DbParameter par3 = Database.AddParameter("AdminDB", "@ID", p.ID);
            Database.ModifyData(Database.GetConnection(CreateConnectionString(claims)), sql, par1, par2, par3);
        }

        public static void DeleteProduct(int id, IEnumerable<Claim> claims)
        {
            string sql = "DELETE FROM Product WHERE ID=@ID";
            DbParameter par1 = Database.AddParameter("AdminDB", "@ID", id);
            DbConnection con = Database.GetConnection(CreateConnectionString(claims));
            Database.ModifyData(con, sql, par1);
        }
    }
}