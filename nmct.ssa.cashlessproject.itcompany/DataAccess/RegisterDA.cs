using nmct.ba.cashlessproject.helper;
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

            string sql = "SELECT o.OrganisationName, orgr.ID, orgr.OrganisationID, orgr.RegisterID, orgr.FromDate, orgr.UntilDate , r.RegisterName, r.Device, r.PurchaseDate, r.ExpiresDate ";
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

        public static OrganisationRegister GetRegisterByID(int id)
        {
            OrganisationRegister organisationRegister = new OrganisationRegister();

            string sql = "SELECT o.OrganisationName, orgr.ID, orgr.OrganisationID, orgr.RegisterID,  orgr.FromDate, orgr.UntilDate, r.RegisterName, r.Device, r.PurchaseDate, r.ExpiresDate ";
            sql += "FROM Organisations AS o ";
            sql += "INNER JOIN OrganisationRegister AS orgr ON o.ID = orgr.OrganisationID ";
            sql += "INNER JOIN Register as r ON orgr.RegisterID = r.ID ";
            sql += "WHERE orgr.ID = @ID";

            DbParameter par1 = Database.AddParameter(CONNECTIONSTRING, "@ID", id);
            DbDataReader reader = Database.GetData(CONNECTIONSTRING, sql, par1);

            while (reader.Read())
            {
                organisationRegister = Create(reader);
            }
            reader.Close();

            return organisationRegister;
        }

        private static OrganisationRegister Create(IDataRecord record)
        {
            return new OrganisationRegister()
            {
                ID = Convert.ToInt32(record["ID"]),
                FromDate = DateTime.UtcNow.ToDateTime(Convert.ToInt32((record["FromDate"]))),
                UntilDate = DateTime.UtcNow.ToDateTime(Convert.ToInt32((record["UntilDate"]))),
                Organisation = new Organisation()
                {
                    ID = Convert.ToInt32(record["OrganisationID"]),
                    OrganisationName = record["OrganisationName"].ToString(),
                },
                Register = new RegisterCompany()
                {
                    ID = Convert.ToInt32(record["RegisterID"]),
                    RegisterName = record["RegisterName"].ToString(),
                    Device = record["Device"].ToString(),
                    PurchaseDate = DateTime.UtcNow.ToDateTime(Convert.ToInt32((record["PurchaseDate"]))),
                    ExpiresDate = DateTime.UtcNow.ToDateTime(Convert.ToInt32((record["ExpiresDate"])))
                }
            };
        }

        public static int CreateRegister(RegisterCompany r)
        {
            string sql = "INSERT INTO Register (RegisterName, Device, PurchaseDate, ExpiresDate) ";
            sql += "VALUES (@RegisterName, @Device, @PurchaseDate, @ExpiresDate)";

            DbParameter par1 = Database.AddParameter(CONNECTIONSTRING, "@RegisterName", r.RegisterName);
            DbParameter par2 = Database.AddParameter(CONNECTIONSTRING, "@Device", r.Device);
            DbParameter par3 = Database.AddParameter(CONNECTIONSTRING, "@PurchaseDate", r.PurchaseDate.ToUnixTimestamp());
            DbParameter par4 = Database.AddParameter(CONNECTIONSTRING, "@ExpiresDate", r.ExpiresDate.ToUnixTimestamp());

            return Database.InsertData(CONNECTIONSTRING, sql, par1, par2, par3, par4);
        }

        public static void DeleteRegisterOrganisation(int registerID)
        {
            string sql = "DELETE FROM OrganisationRegister WHERE RegisterID=@RegisterID";
            DbParameter par1 = Database.AddParameter(CONNECTIONSTRING, "@RegisterID", registerID);
            Database.ModifyData(CONNECTIONSTRING, sql, par1);
        }

        public static void CreateRegisterOrganisation(OrganisationRegister or, int registerID)
        {
            string sql = "INSERT INTO OrganisationRegister (OrganisationID, RegisterID, FromDate, UntilDate) ";
            sql += "VALUES (@OrganisationID, @RegisterID, @FromDate, @UntilDate)";

            DbParameter par1 = Database.AddParameter(CONNECTIONSTRING, "@OrganisationID", or.Organisation.ID);
            DbParameter par2 = Database.AddParameter(CONNECTIONSTRING, "@RegisterID", registerID);
            DbParameter par3 = Database.AddParameter(CONNECTIONSTRING, "@FromDate", or.FromDate.ToUnixTimestamp());
            DbParameter par4 = Database.AddParameter(CONNECTIONSTRING, "@UntilDate", or.UntilDate.ToUnixTimestamp());

            Database.InsertData(CONNECTIONSTRING, sql, par1, par2, par3, par4);
        }

        public static void EditRegister(RegisterCompany r)
        {
            string sql = "UPDATE Register SET RegisterName=@RegisterName, Device=@Device, PurchaseDate=@PurchaseDate, ExpiresDate=@ExpiresDate WHERE ID=@ID";

            DbParameter par1 = Database.AddParameter(CONNECTIONSTRING, "@RegisterName", r.RegisterName);
            DbParameter par2 = Database.AddParameter(CONNECTIONSTRING, "@Device", r.Device);
            DbParameter par3 = Database.AddParameter(CONNECTIONSTRING, "@PurchaseDate", r.PurchaseDate.ToUnixTimestamp());
            DbParameter par4 = Database.AddParameter(CONNECTIONSTRING, "@ExpiresDate", r.ExpiresDate.ToUnixTimestamp());
            DbParameter par5 = Database.AddParameter(CONNECTIONSTRING, "@ID", r.ID);

            Database.ModifyData(CONNECTIONSTRING, sql, par1, par2, par3, par4, par5);
        }
    }
}