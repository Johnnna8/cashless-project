using nmct.ba.cashlessproject.api.Helper;
using nmct.ba.cashlessproject.model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace nmct.ba.cashlessproject.api.Models
{
    public class ProductDA
    {
        public static List<Product> GetProducts()
        {
            List<Product> products = new List<Product>();

            DbDataReader reader = Database.GetData("ConnectionString", "SELECT * FROM Products");
            while (reader.Read())
            {
                products.Add(Create(reader));
            }
            reader.Close();

            return products;
        }

        public static Product GetProduct(int id)
        {
            Product product = new Product();

            DbParameter par1 = Database.AddParameter("ConnectionString", "@ID", id);
            DbDataReader reader = Database.GetData("ConnectionString", "SELECT * FROM Products WHERE ID=@ID", par1);

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
    }
}