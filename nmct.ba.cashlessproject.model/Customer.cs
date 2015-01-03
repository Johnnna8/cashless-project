using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace nmct.ba.cashlessproject.model
{
    public class Customer : IDataErrorInfo
    {
        private int _ID;

        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        private string _nationalNumber;

        public string NationalNumber
        {
            get { return _nationalNumber; }
            set { _nationalNumber = value; }
        }

        private string _firstname;

        [Required(ErrorMessage = "Voornaam is verplicht")]
        [RegularExpression(@"^[a-zA-Z''-'\s]{2,50}$", ErrorMessage = "Voornaam tussen de 2 en 30 karakters")]
        public string Firstname
        {
            get{ return _firstname; }
            set { _firstname = value; }
        }

        private string _lastname;

        [Required(ErrorMessage = "Familienaam is verplicht")]
        [RegularExpression(@"^[a-zA-Z''-'\s]{2,50}$", ErrorMessage = "Familienaam tussen de 2 en 30 karakters")]
        public string Lastname
        {
            get { return _lastname; }
            set { _lastname = value; }
        }

        private string _street;

        [Required(ErrorMessage = "Straat is verplicht")]
        [RegularExpression(@"^[0-9a-zA-Z''-'\s]{2,50}$", ErrorMessage = "Straat tussen de 2 en 50 karakters")]
        public string Street
        {
            get { return _street; }
            set { _street = value; }
        }

        //private string _streetNumber;

        //public string StreetNumber
        //{
        //    get { return _streetNumber; }
        //    set { _streetNumber = value; }
        //}

        private string _postcode;

        [Required(ErrorMessage = "Postcode is verplicht")]
        [RegularExpression(@"^[0-9a-zA-Z''-'\s]{4,10}$", ErrorMessage = "Postcode tussen de 4 en 10 karakters")]
        public string Postcode
        {
            get { return _postcode; }
            set { _postcode = value; }
        }

        private string _city;

        [Required(ErrorMessage = "Gemeente is verplicht")]
        [RegularExpression(@"^[a-zA-Z''-'\s]{2,40}$", ErrorMessage = "Gemeente tussen de 2 en 40 karakters")]
        public string City
        {
            get { return _city; }
            set { _city = value; }
        }

        private byte[] _picture;

        public byte[] Picture
        {
            get { return _picture; }
            set { _picture = value; }
        }

        private double _balance;

        [Required(ErrorMessage = "Saldo is verplicht")]
        [Range(0.01, 100, ErrorMessage = "Prijs tussen 0.01 en 100")]
        public double Balance
        {
            get { return _balance; }
            set { _balance = value; }
        }

        public override string ToString()
        {
            return Firstname + " " + Lastname;
        }

        public string Error
        {
            get { return null; }
        }

        public bool IsValid()
        {
            return Validator.TryValidateObject(this, new ValidationContext(this, null, null), null, true);
        }

        public string this[string columnName]
        {
            get
            {
                try
                {
                    object value = this.GetType().GetProperty(columnName).GetValue(this);
                    Validator.ValidateProperty(value, new ValidationContext(this, null, null) { MemberName = columnName });
                }
                catch (ValidationException ex)
                {
                    return ex.Message;
                }
                return String.Empty;
            }
        }
    }
}
