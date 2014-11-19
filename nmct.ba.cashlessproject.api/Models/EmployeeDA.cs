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
    public class EmployeeDA
    {
        public static List<Employee> GetEmployees()
        {
            List<Employee> bankAccounts = new List<Employee>();

            DbDataReader reader = Database.GetData("ConnectionString", "SELECT * FROM Employee");
            while (reader.Read())
            {
                bankAccounts.Add(Create(reader));
            }
            reader.Close();

            return bankAccounts;
        }

        private static Employee Create(IDataRecord record)
        {
            return new Employee()
            {
                ID = Convert.ToInt32(record["ID"]),
                Firstname = record["Firstname"].ToString(),
                Lastname = record["Lastname"].ToString(),
                Street = record["Street"].ToString(),
                StreetNumber = record["StreetNumber"].ToString(),
                Postcode = record["Postcode"].ToString(),
                City = record["City"].ToString(),
                Email = record["Email"].ToString(),
                Phone = record["Phone"].ToString()
            };
        }

        public static int CreateEmployee(Employee em)
        {
            string sql = "INSERT INTO Employee (Firstname, Lastname, Street, StreetNumber, Postcode, City, Email, Phone) VALUES (@Firstname, @Lastname, @Street, @StreetNumber, @Postcode, @City, @Email, @Phone)";

            DbParameter par1 = Database.AddParameter("ConnectionString", "@Firstname", em.Firstname);
            DbParameter par2 = Database.AddParameter("ConnectionString", "@Lastname", em.Lastname);
            DbParameter par3 = Database.AddParameter("ConnectionString", "@Street", em.Street);
            DbParameter par4 = Database.AddParameter("ConnectionString", "@StreetNumber", em.StreetNumber);
            DbParameter par5 = Database.AddParameter("ConnectionString", "@Postcode", em.Postcode);
            DbParameter par6 = Database.AddParameter("ConnectionString", "@City", em.City);
            DbParameter par7 = Database.AddParameter("ConnectionString", "@Email", em.Email);
            DbParameter par8 = Database.AddParameter("ConnectionString", "@Phone", em.Phone);

            return Database.InsertData("ConnectionString", sql, par1, par2, par3, par4, par5, par6, par7, par8);
        }

        public static int UpdateEmployee(Employee em)
        {
            string sql = "UPDATE Employee SET Firstname=@firstname, Lastname=@Lastname, Street=@Street, StreetNumber=@StreetNumber, Postcode=@Postcode, City=@City, Email=@Email, Phone=@Phone WHERE ID=@ID";

            DbParameter par1 = Database.AddParameter("ConnectionString", "@Firstname", em.Firstname);
            DbParameter par2 = Database.AddParameter("ConnectionString", "@Lastname", em.Lastname);
            DbParameter par3 = Database.AddParameter("ConnectionString", "@Street", em.Street);
            DbParameter par4 = Database.AddParameter("ConnectionString", "@StreetNumber", em.StreetNumber);
            DbParameter par5 = Database.AddParameter("ConnectionString", "@Postcode", em.Postcode);
            DbParameter par6 = Database.AddParameter("ConnectionString", "@City", em.City);
            DbParameter par7 = Database.AddParameter("ConnectionString", "@Email", em.Email);
            DbParameter par8 = Database.AddParameter("ConnectionString", "@Phone", em.Phone);
            DbParameter par9 = Database.AddParameter("ConnectionString", "@ID", em.ID);

            return Database.ModifyData("ConnectionString", sql, par1, par2, par3, par4, par5, par6, par7, par8, par9);
        }

        public static int DeleteEmployee(int id)
        {
            string sql = "DELETE FROM employee WHERE ID=@ID";
            DbParameter par1 = Database.AddParameter("ConnectionString", "@ID", id);
            return Database.ModifyData("ConnectionString", sql, par1);
        }


    }
}