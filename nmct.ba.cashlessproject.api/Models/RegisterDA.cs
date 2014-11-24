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
    public class RegisterDA
    {
        public static List<Register> GetRegisters()
        {
            List<Register> register = new List<Register>();

            DbDataReader reader = Database.GetData("ConnectionString", "SELECT * FROM Register");
            while (reader.Read())
            {
                register.Add(Create(reader));
            }
            reader.Close();

            return register;
        }

        public static Register GetRegister(int id)
        {
            Register register = new Register();

            DbParameter par1 = Database.AddParameter("ConnectionString", "@ID", id);
            DbDataReader reader = Database.GetData("ConnectionString", "SELECT * FROM Register WHERE ID=@ID", par1);

            while (reader.Read())
            {
                register = Create(reader);
            }
            reader.Close();

            return register;
        }

        private static Register Create(IDataRecord record)
        {
            return new Register()
            {
                ID = Convert.ToInt32(record["ID"]),
                RegisterName = record["RegisterName"].ToString(),
                Device = record["Device"].ToString()
            };
        }
    }
}