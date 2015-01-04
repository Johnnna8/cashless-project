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
    public class EmployeeDA
    {
        private static ConnectionStringSettings CreateConnectionString(IEnumerable<Claim> claims)
        {
            string dblogin = claims.FirstOrDefault(c => c.Type == "dblogin").Value;
            string dbpass = claims.FirstOrDefault(c => c.Type == "dbpass").Value;
            string dbname = claims.FirstOrDefault(c => c.Type == "dbname").Value;

            return Database.CreateConnectionString("System.Data.SqlClient", @"JONA\DATAMANAGEMENT", Cryptography.Decrypt(dbname), Cryptography.Decrypt(dblogin), Cryptography.Decrypt(dbpass));
        }

        public static List<Employee> GetEmployees(IEnumerable<Claim> claims)
        {
            List<Employee> employees = new List<Employee>();
            string sql = "SELECT * FROM Employee WHERE available = 1";
            DbDataReader reader = Database.GetData(Database.GetConnection(CreateConnectionString(claims)), sql);

            while (reader.Read())
            {
                employees.Add(Create(reader));
            }
            reader.Close();

            return employees;
        }

        public static Employee GetEmployee(int id, IEnumerable<Claim> claims)
        {
            Employee employee = new Employee();

            string sql = "SELECT * FROM Employee WHERE ID=@ID";
            DbParameter par1 = Database.AddParameter("ConnectionString", "@ID", id);
            DbDataReader reader = Database.GetData(Database.GetConnection(CreateConnectionString(claims)), sql, par1);

            while (reader.Read())     
            {
                employee = Create(reader);
            }
            reader.Close();

            return employee;
        }

        private static Employee Create(IDataRecord record)
        {
            return new Employee()
            {
                ID = Convert.ToInt32(record["ID"]),
                Pincode = Cryptography.Decrypt(Convert.ToString(record["Pincode"])),
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

        public static int InsertEmployee(Employee e, IEnumerable<Claim> claims)
        {
            string sql = "INSERT INTO Employee(Pincode,Firstname,Lastname,Street,StreetNumber,Postcode,City,Email,Phone)";
            sql += "VALUES(@Pincode,@Firstname,@Lastname,@Street,@StreetNumber,@Postcode,@City,@Email,@Phone)";
            
            DbParameter par1 = Database.AddParameter("AdminDB", "@Pincode", Cryptography.Encrypt(e.Pincode));
            DbParameter par2 = Database.AddParameter("AdminDB", "@Firstname", e.Firstname);
            DbParameter par3 = Database.AddParameter("AdminDB", "@Lastname", e.Lastname);
            DbParameter par4 = Database.AddParameter("AdminDB", "@Street", e.Street);
            DbParameter par5 = Database.AddParameter("AdminDB", "@StreetNumber", e.StreetNumber);
            DbParameter par6 = Database.AddParameter("AdminDB", "@Postcode", e.Postcode);
            DbParameter par7 = Database.AddParameter("AdminDB", "@City", e.City);
            DbParameter par8 = Database.AddParameter("AdminDB", "@Email", e.Email);
            DbParameter par9 = Database.AddParameter("AdminDB", "@Phone", e.Phone);
            
            return Database.InsertData(Database.GetConnection(CreateConnectionString(claims)), sql, par1, par2, par3, par4, par5, par6, par7, par8, par9);
        }

        public static void UpdateEmployee(Employee e, IEnumerable<Claim> claims)
        {
            string sql = "UPDATE Employee SET Available=@Available, Pincode=@Pincode, Firstname=@Firstname, Lastname=@Lastname, Street=@Street, StreetNumber=@StreetNumber, Postcode=@Postcode, City=@City, Email=@Email, Phone=@Phone WHERE ID=@ID";
            
            DbParameter par1 = Database.AddParameter("AdminDB", "@Pincode", Cryptography.Encrypt(e.Pincode));
            DbParameter par2 = Database.AddParameter("AdminDB", "@Firstname", e.Firstname);
            DbParameter par3 = Database.AddParameter("AdminDB", "@Lastname", e.Lastname);
            DbParameter par4 = Database.AddParameter("AdminDB", "@Street", e.Street);
            DbParameter par5 = Database.AddParameter("AdminDB", "@StreetNumber", e.StreetNumber);
            DbParameter par6 = Database.AddParameter("AdminDB", "@Postcode", e.Postcode);
            DbParameter par7 = Database.AddParameter("AdminDB", "@City", e.City);
            DbParameter par8 = Database.AddParameter("AdminDB", "@Email", e.Email);
            DbParameter par9 = Database.AddParameter("AdminDB", "@Phone", e.Phone);
            DbParameter par10 = Database.AddParameter("AdminDB", "@Available", e.Available);
            DbParameter par11 = Database.AddParameter("AdminDB", "@ID", e.ID); 
            
            Database.ModifyData(Database.GetConnection(CreateConnectionString(claims)), sql, par1, par2, par3, par4, par5, par6, par7, par8, par9, par10, par11);
        }

        //public static void DeleteEmployee(int id, IEnumerable<Claim> claims)
        //{
        //    string sql = "DELETE FROM Employee WHERE ID=@ID";
        //    DbParameter par1 = Database.AddParameter("AdminDB", "@ID", id);
        //    DbConnection con = Database.GetConnection(CreateConnectionString(claims));
        //    Database.ModifyData(con, sql, par1);
        //}
    }
}