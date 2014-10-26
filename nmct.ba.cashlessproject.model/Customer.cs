using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace nmct.ba.cashlessproject.model
{
    class Customer
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

        private BitmapImage _picture;

        public BitmapImage Picture
        {
            get { return _picture; }
            set { _picture = value; }
        }

        private double _balance;

        public double Balance
        {
            get { return _balance; }
            set { _balance = value; }
        }
    }
}
