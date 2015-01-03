using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.ba.cashlessproject.model
{
    public class Organisation
    {
        private int _ID;

        [Required]
        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        private string _login;

        [Required(ErrorMessage = "Login is verplicht")]
        [MinLength(2, ErrorMessage = "Login minimaal 2 karakters")]
        [MaxLength(50, ErrorMessage = "Login maximaal 50 karakters")]
        public string Login
        {
            get { return _login; }
            set { _login = value; }
        }

        private string _password;

        [Required(ErrorMessage = "Wachtwoord is verplicht")]
        [MinLength(2, ErrorMessage = "Wachtwoord minimaal 2 karakters")]
        [MaxLength(50, ErrorMessage = "Wachtwoord maximaal 50 karakters")]
        [DisplayName("Wachtwoord")]
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        private string _dbName;

        [Required(ErrorMessage = "Naam database is verplicht")]
        [MinLength(2, ErrorMessage = "Naam database minimaal 2 karakters")]
        [MaxLength(50, ErrorMessage = "Naam database maximaal 50 karakters")]
        [DisplayName("Naam database")]
        public string DbName
        {
            get { return _dbName; }
            set { _dbName = value; }
        }

        private string _dbLogin;

        [Required(ErrorMessage = "Login database is verplicht")]
        [MinLength(2, ErrorMessage = "Login database minimaal 2 karakters")]
        [MaxLength(50, ErrorMessage = "Login database maximaal 50 karakters")]
        [DisplayName("Login database")]
        public string DbLogin
        {
            get { return _dbLogin; }
            set { _dbLogin = value; }
        }

        private string _dbPassword;

        [Required(ErrorMessage = "Wachtwoord database is verplicht")]
        [MinLength(2, ErrorMessage = "Wachtwoord database minimaal 2 karakters")]
        [MaxLength(50, ErrorMessage = "Wachtwoord database maximaal 50 karakters")]
        [DisplayName("Wachtwoord database")]
        public string DbPassword
        {
            get { return _dbPassword; }
            set { _dbPassword = value; }
        }

        private string _organisationName;

        [Required(ErrorMessage = "Naam vereniging is verplicht")]
        [MinLength(2, ErrorMessage="Naam vereniging minimaal 2 karakters")]
        [MaxLength(50, ErrorMessage = "Naam vereniging maximaal 50 karakters")]
        [DisplayName("Naam vereniging")]
        public string OrganisationName
        {
            get { return _organisationName; }
            set { _organisationName = value; }
        }

        private string _street;

        [Required(ErrorMessage = "Straat is verplicht")]
        [RegularExpression(@"^[a-zA-Z''-'\s]{2,50}$", ErrorMessage = "Straat tussen de 2 en 50 karakters, geen cijfers of speciale tekens")]
        [DisplayName("Straat")]
        public string Street
        {
            get { return _street; }
            set { _street = value; }
        }

        private string _streetNumber;

        [Required(ErrorMessage = "Straatnummer is verplicht")]
        [RegularExpression(@"^[0-9a-zA-Z''-'\s]{1,10}$", ErrorMessage = "Straat tussen de 1 en 10 karakters, geen speciale tekens")]
        [DisplayName("Straatnummer")]
        public string StreetNumber
        {
            get { return _streetNumber; }
            set { _streetNumber = value; }
        }

        private string _postcode;

        [Required(ErrorMessage = "Postcode is verplicht")]
        [RegularExpression(@"^[0-9a-zA-Z''-'\s]{4,10}$", ErrorMessage = "Postcode tussen de 4 en 10 karakters, geen speciale tekens")]
        public string Postcode
        {
            get { return _postcode; }
            set { _postcode = value; }
        }

        private string _city;

        [Required(ErrorMessage = "Gemeente is verplicht")]
        [RegularExpression(@"^[a-zA-Z''-'\s]{2,40}$", ErrorMessage = "Gemeente tussen de 2 en 40 karakters, geen cijfers en speciale tekens")]
        [DisplayName("Gemeente")]
        public string City
        {
            get { return _city; }
            set { _city = value; }
        }

        private string _email;

        [Required(ErrorMessage = "Emailadres is verplicht")]
        [EmailAddress(ErrorMessage = "Geef een correct emailadres op")]
        [DisplayName("Emailadres")]
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        private string _phone;

        [Required(ErrorMessage = "Telefoonnummer is verplicht")]
        [RegularExpression(@"^[0-9]{3,50}$", ErrorMessage = "Telefoonnummer tussen de 3 en 50 karakters, enkel cijfers")]
        [DisplayName("Telefoonnummer")]
        public string Phone
        {
            get { return _phone; }
            set { _phone = value; }
        }
    }
}