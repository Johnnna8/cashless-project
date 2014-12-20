using nmct.ba.cashlessproject.model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace nmct.ssa.cashlessproject.itcompany.DataAccess
{
    public class RegisterDA
    {
        private const string CONNECTIONSTRING = "ConnectionString";

        public static List<OrganisationRegister> GetOrganisationsWithRegisters()
        {
            List<OrganisationRegister> organisationsRegisters = new List<OrganisationRegister>();

            string sql = "SELECT o.OrganisationName, orgr.ID, orgr.OrganisationID, orgr.RegisterID, r.RegisterName, r.Device, r.PurchaseDate, r.ExpiresDate ";
            sql += "FROM Organisations AS o ";
            sql += "INNER JOIN OrganisationRegister AS orgr ON o.ID = orgr.OrganisationID ";
            sql += "INNER JOIN Register as r ON orgr.RegisterID = r.ID";

            DbDataReader reader = Database.GetData(CONNECTIONSTRING, sql);
            while (reader.Read())
            {
                organisationsRegisters.Add(Create(reader));
            }
            reader.Close();

            return organisationsRegisters;
        }

        private static OrganisationRegister Create(IDataRecord record)
        {
            return new OrganisationRegister()
            {
                ID = Convert.ToInt32(record["ID"]),
                Organisation = new Organisation()
                {
                    ID = Convert.ToInt32(record["OrganisationID"]),
                    OrganisationName = record["OrganisationName"].ToString()
                },
                Register = new RegisterCompany()
                {
                    ID = Convert.ToInt32(record["RegisterID"]),
                    RegisterName = record["RegisterName"].ToString(),
                    Device = record["Device"].ToString(),
                    ExpiresDate = Convert.ToInt32(record["ExpiresDate"]),
                    PurchaseDate = Convert.ToInt32(record["ExpiresDate"])
                }
            };
        }

        public static int CreateRegister(OrganisationRegister or)
        {
            string sql = "INSERT INTO Register (RegisterName, Device, PurchaseDate, ExpiresDate) ";
            sql += "VALUES (@RegisterName, @Device, @PurchaseDate, @ExpiresDate)";

            DbParameter par1 = Database.AddParameter(CONNECTIONSTRING, "@RegisterName", or.Register.RegisterName);
            DbParameter par2 = Database.AddParameter(CONNECTIONSTRING, "@Device", or.Register.Device);
            DbParameter par3 = Database.AddParameter(CONNECTIONSTRING, "@PurchaseDate", or.Register.PurchaseDate);
            DbParameter par4 = Database.AddParameter(CONNECTIONSTRING, "@ExpiresDate", or.Register.ExpiresDate);

            return Database.InsertData(CONNECTIONSTRING, sql, par1, par2, par3, par4);
        }

        public static void CreateRegisterOrganisation(OrganisationRegister or, int registerID)
        {
            string sql = "INSERT INTO OrganisationRegister (OrganisationID, RegisterID, FromDate, UntilDate) ";
            sql += "VALUES (@OrganisationID, @RegisterID, @FromDate, @UntilDate)";

            DbParameter par1 = Database.AddParameter(CONNECTIONSTRING, "@OrganisationID", or.Organisation.ID);
            DbParameter par2 = Database.AddParameter(CONNECTIONSTRING, "@RegisterID", registerID);
            DbParameter par3 = Database.AddParameter(CONNECTIONSTRING, "@FromDate", or.FromDate);
            DbParameter par4 = Database.AddParameter(CONNECTIONSTRING, "@UntilDate", or.UntilDate);

            Database.InsertData(CONNECTIONSTRING, sql, par1, par2, par3, par4);


        }
    }
}