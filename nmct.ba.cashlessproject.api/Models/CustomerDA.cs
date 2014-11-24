using nmct.ba.cashlessproject.api.Helper;
using nmct.ba.cashlessproject.model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
//voor memorystream
using System.IO;
using System.Linq;
using System.Web;
using System.Windows.Media.Imaging;

namespace nmct.ba.cashlessproject.api.Models
{
    public class CustomerDA
    {
        public static List<Customer> GetCustomers()
        {
            List<Customer> customers = new List<Customer>();

            DbDataReader reader = Database.GetData("ConnectionString", "SELECT * FROM Customer");
            while (reader.Read())
            {
                customers.Add(Create(reader));
            }
            reader.Close();

            return customers;
        }

        public static Customer GetCustomer(int id)
        {
            Customer customer = new Customer();

            DbParameter par1 = Database.AddParameter("ConnectionString", "@ID", id);
            DbDataReader reader = Database.GetData("ConnectionString", "SELECT * FROM Customer WHERE ID=@ID", par1);

            while (reader.Read())
            {
                customer = Create(reader);
            }
            reader.Close();

            return customer;
        }

        private static Customer Create(IDataRecord record)
        {
            BitmapImage bitmap = new BitmapImage();

            if (record["picture"] == System.DBNull.Value)  {
                bitmap = null;
            }
            else
            {
                byte[] photoSource = (byte[])record["Picture"];
                MemoryStream strm = new MemoryStream();

                int offset = 78;
                strm.Write(photoSource, offset, photoSource.Length - offset);

                bitmap.BeginInit();
                bitmap.StreamSource = strm;
                bitmap.EndInit();
            }

            return new Customer()
            {
                ID = Convert.ToInt32(record["ID"]),
                Firstname = record["Firstname"].ToString(),
                Lastname = record["Lastname"].ToString(),
                Street = record["Street"].ToString(),
                StreetNumber = record["StreetNumber"].ToString(),
                Postcode = record["Postcode"].ToString(),
                City = record["City"].ToString(),
                Picture = bitmap,
                Balance = Convert.ToDouble(record["Balance"])
            };
        }
    }
}