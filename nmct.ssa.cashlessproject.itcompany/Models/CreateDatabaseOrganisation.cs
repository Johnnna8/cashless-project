using nmct.ba.cashlessproject.helper;
using nmct.ba.cashlessproject.model;
using nmct.ssa.cashlessproject.itcompany.DataAccess;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace nmct.ssa.cashlessproject.itcompany.Models
{
    //gekopieerd van Frederik Duchi en aangepast
    public class CreateDatabaseOrganisation
    {
        public static void CreateDatabase(Organisation o)
        {
            // create the actual database
            string create = File.ReadAllText(HostingEnvironment.MapPath(@"~/App_Data/create.txt")); // only for the web
            //string create = File.ReadAllText(@"..\..\Data\create.txt"); // only for desktop
            string sql = create.Replace("@@DbName", Cryptography.Decrypt(o.DbName)).Replace("@@DbLogin", Cryptography.Decrypt(o.DbLogin).Replace("@@DbPassword", Cryptography.Decrypt(o.DbPassword)));
            foreach (string commandText in RemoveGo(sql))
            {
                Database.ModifyData("ConnectionString", commandText);
            }

            // create login, user and tables
            DbTransaction trans = null;
            try
            {
                trans = Database.BeginTransaction("ConnectionString");

                string fill = File.ReadAllText(HostingEnvironment.MapPath(@"~/App_Data/fill.txt")); // only for the web
                //string fill = File.ReadAllText(@"..\..\Data\fill.txt"); // only for desktop
                string sql2 = fill.Replace("@@DbName", Cryptography.Decrypt(o.DbName)).Replace("@@DbLogin", Cryptography.Decrypt(o.DbLogin)).Replace("@@DbPassword", Cryptography.Decrypt(o.DbPassword));

                foreach (string commandText in RemoveGo(sql2))
                {
                    Database.ModifyData(trans, commandText);
                }

                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                Console.WriteLine(ex.Message);
            }
        }

        private static string[] RemoveGo(string input)
        {
            //split the script on "GO" commands
            string[] splitter = new string[] { "\r\nGO\r\n" };
            string[] commandTexts = input.Split(splitter, StringSplitOptions.RemoveEmptyEntries);
            return commandTexts;
        }
    }
}