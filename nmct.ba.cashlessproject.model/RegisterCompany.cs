using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.ba.cashlessproject.model
{
    public class RegisterCompany
    {
        private int _ID;

        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        private string _registerName;

        [DisplayName("Naam kassa")]
        public string RegisterName
        {
            get { return _registerName; }
            set { _registerName = value; }
        }

        private string _device;

        [DisplayName("Soort toestel")]
        public string Device
        {
            get { return _device; }
            set { _device = value; }
        }

        private int _purchaseDate;

        [DisplayName("Aangekocht op")]
        public int PurchaseDate
        {
            get { return _purchaseDate; }
            set { _purchaseDate = value; }
        }

        private int _expiresDate;

        [DisplayName("Verloopt op")]
        public int ExpiresDate
        {
            get { return _expiresDate; }
            set { _expiresDate = value; }
        }

    }
}
