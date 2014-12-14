using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.ba.cashlessproject.model
{
    public class Organisation
    {
        private int _ID;

        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        private string _login;

        public string Login
        {
            get { return _login; }
            set { _login = value; }
        }

        private string _password;

        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        private string _dbName;

        public string DbName
        {
            get { return _dbName; }
            set { _dbName = value; }
        }

        private string _dbLogin;

        public string DbLogin
        {
            get { return _dbLogin; }
            set { _dbLogin = value; }
        }

        private string _dbPassword;

        public string DbPassword
        {
            get { return _dbPassword; }
            set { _dbPassword = value; }
        }

        private string _organisationName;

        public string OrganisationName
        {
            get { return _organisationName; }
            set { _organisationName = value; }
        }

        private string _street;

        public string Street
        {
            get { return _street; }
            set { _street = value; }
        }

        private string _streetNumber;

        public string StreetNumber
        {
            get { return _streetNumber; }
            set { _streetNumber = value; }
        }

        private string _postcode;

        public string Postcode
        {
            get { return _postcode; }
            set { _postcode = value; }
        }

        private string _city;

        public string City
        {
            get { return _city; }
            set { _city = value; }
        }

        private string _email;

        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        private string _phone;

        public string Phone
        {
            get { return _phone; }
            set { _phone = value; }
        }
    }
}