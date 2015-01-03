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
    public class RegisterEmployeeDA
    {
        private static ConnectionStringSettings CreateConnectionString(IEnumerable<Claim> claims)
        {
            string dblogin = claims.FirstOrDefault(c => c.Type == "dblogin").Value;
            string dbpass = claims.FirstOrDefault(c => c.Type == "dbpass").Value;
            string dbname = claims.FirstOrDefault(c => c.Type == "dbname").Value;

            return Database.CreateConnectionString("System.Data.SqlClient", @"JONA\DATAMANAGEMENT", Cryptography.Decrypt(dbname), Cryptography.Decrypt(dblogin), Cryptography.Decrypt(dbpass));
        }

        public static List<RegisterEmployee> GetEmployeesPerRegister(int id, IEnumerable<Claim> claims)
        {
            List<RegisterEmployee> registerEmployee = new List<RegisterEmployee>();

            string sql = "SELECT e.*, FromTime, UntilTime FROM Register AS r INNER JOIN RegisterEmployee AS re ON r.ID = re.RegisterID INNER JOIN Employee as e ON re.EmployeeID = e.ID WHERE r.ID = @ID";
            DbParameter par1 = Database.AddParameter("AdminDB", "@ID", id);
            DbDataReader reader = Database.GetData(Database.GetConnection(CreateConnectionString(claims)), sql, par1);

            while (reader.Read())
            {
                registerEmployee.Add(CreateRegisterEmployee(reader));
            }
            reader.Close();

            return registerEmployee;
        }

        private static RegisterEmployee CreateRegisterEmployee(IDataRecord record)
        {
            return new RegisterEmployee()
            {
                Employee = new Employee() {
                    ID = Convert.ToInt32(record["ID"]),
                    Firstname = record["Firstname"].ToString(),
                    Lastname = record["Lastname"].ToString(),
                    Street = record["Street"].ToString(),
                    StreetNumber = record["StreetNumber"].ToString(),
                    Postcode = record["Postcode"].ToString(),
                    City = record["City"].ToString(),
                    Email = record["Email"].ToString(),
                    Phone = record["Phone"].ToString()
                },
                FromTime = DateTime.Now.ToDateTime(Convert.ToInt32(record["FromTime"])),
                UntilTime = DateTime.Now.ToDateTime(Convert.ToInt32(record["UntilTime"]))
            }; 
        }

        public static int InsertRegisterEmployee(RegisterEmployee re, IEnumerable<Claim> claims)
        {
            string sql = "INSERT INTO RegisterEmployee VALUES(@RegisterID, @EmployeeID, @FromTime, @UntilTime)";
            DbParameter par1 = Database.AddParameter("AdminDB", "@RegisterID", re.Register.ID);
            DbParameter par2 = Database.AddParameter("AdminDB", "@EmployeeID", re.Employee.ID);
            DbParameter par3 = Database.AddParameter("AdminDB", "@FromTime", re.FromTime);
            DbParameter par4 = Database.AddParameter("AdminDB", "@UntilTime", re.UntilTime);
            return Database.InsertData(Database.GetConnection(CreateConnectionString(claims)), sql, par1, par2, par3, par4);
        }

        public static void UpdateRegisterEmployee(RegisterEmployee p, IEnumerable<Claim> claims)
        {
            string sql = "UPDATE RegisterEmployee SET UntilTime=@UntilTime WHERE ID=@ID";
            DbParameter par1 = Database.AddParameter("AdminDB", "@UntilTime", p.UntilTime);
            DbParameter par2 = Database.AddParameter("AdminDB", "@ID", p.ID);
            Database.ModifyData(Database.GetConnection(CreateConnectionString(claims)), sql, par1, par2);
        }
    }
}