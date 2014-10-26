using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.ba.cashlessproject.model
{
    class Employee
    {
        private int _ID;

        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        private string _customerName;

        public string CustomerName
        {
            get { return _customerName; }
            set { _customerName = value; }
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

        private int _phone;

        public int Phone
        {
            get { return _phone; }
            set { _phone = value; }
        }
    }
}
