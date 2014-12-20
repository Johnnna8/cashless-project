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

            string sql = "SELECT e.*, [from], Until FROM Register AS r INNER JOIN RegisterEmployee AS re ON r.ID = re.RegisterID INNER JOIN Employee as e ON re.EmployeeID = e.ID WHERE r.ID = @ID";
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
                EmployeeID = new Employee() {
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
                From = Convert.ToInt32(record["From"]),
                Until = Convert.ToInt32(record["Until"])
            };
        }
    }
}