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
    public class RegisterDA
    {
        private static ConnectionStringSettings CreateConnectionString(IEnumerable<Claim> claims)
        {
            string dblogin = claims.FirstOrDefault(c => c.Type == "dblogin").Value;
            string dbpass = claims.FirstOrDefault(c => c.Type == "dbpass").Value;
            string dbname = claims.FirstOrDefault(c => c.Type == "dbname").Value;

            return Database.CreateConnectionString("System.Data.SqlClient", @"JONA\DATAMANAGEMENT", Cryptography.Decrypt(dbname), Cryptography.Decrypt(dblogin), Cryptography.Decrypt(dbpass));
        }

        public static List<Register> GetRegisters(IEnumerable<Claim> claims)
        {
            List<Register> registers = new List<Register>();
            string sql = "SELECT * FROM Register";
            DbDataReader reader = Database.GetData(Database.GetConnection(CreateConnectionString(claims)), sql);

            while (reader.Read())
            {
                registers.Add(CreateRegister(reader));
            }
            reader.Close();

            return registers;
        }

        public static Register GetRegister(int id, IEnumerable<Claim> claims)
        {
            Register register = new Register();
            string sql = "SELECT * FROM Register WHERE ID=@ID";
            DbParameter par1 = Database.AddParameter("ConnectionString", "@ID", id);
            DbDataReader reader = Database.GetData(Database.GetConnection(CreateConnectionString(claims)), sql, par1);

            while (reader.Read())
            {
                register = CreateRegister(reader);
            }
            reader.Close();

            return register;
        }

        private static Register CreateRegister(IDataRecord record)
        {
            return new Register()
            {
                ID = Convert.ToInt32(record["ID"]),
                RegisterName = record["RegisterName"].ToString(),
                Device = record["Device"].ToString()
            };
        }

        public static List<Employee> GetEmployeesFromRegister(int id, IEnumerable<Claim> claims)
        {
            List<Employee> employees = new List<Employee>();

            string sql = "SELECT e.* FROM Register AS r INNER JOIN RegisterEmployee AS re ON r.ID = re.RegisterID INNER JOIN Employee as e ON re.EmployeeID = e.ID WHERE r.ID = @ID";
            DbParameter par1 = Database.AddParameter("AdminDB", "@ID", id);
            DbDataReader reader = Database.GetData(Database.GetConnection(CreateConnectionString(claims)), sql, par1);

            while (reader.Read())
            {
                employees.Add(CreateEmployee(reader));
            }
            reader.Close();

            return employees;
        }

        private static Employee CreateEmployee(IDataRecord record)
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

        public static List<RegisterEmployee> GetRegistersEmployees(int registerid, int employeeid, IEnumerable<Claim> claims)
        {
            List<RegisterEmployee> registersEmployees = new List<RegisterEmployee>();
            DbParameter par1 = Database.AddParameter("AdminDB", "@RegisterID", registerid);
            DbParameter par2 = Database.AddParameter("AdminDB", "@EmployeeID", employeeid);
            string sql = "SELECT re.[From], re.Until, re.RegisterID, re.EmployeeID FROM Employee AS e INNER JOIN RegisterEmployee AS re ON e.ID = re.EmployeeID WHERE re.RegisterID = @RegisterID AND re.EmployeeID = @EmployeeID";
            DbDataReader reader = Database.GetData(Database.GetConnection(CreateConnectionString(claims)), sql, par1, par2);

            while (reader.Read())
            {
                registersEmployees.Add(CreateRegisterEmployee(reader));
            }
            reader.Close();

            return registersEmployees;
        }

        private static RegisterEmployee CreateRegisterEmployee(IDataRecord record)
        {
            return new RegisterEmployee()
            {
                RegisterID = Convert.ToInt32(record["RegisterID"]),
                EmployeeID = Convert.ToInt32(record["EmployeeID"]),
                From = Convert.ToInt32(record["From"]),
                Until = Convert.ToInt32(record["Until"])
            };
        }
    }
}