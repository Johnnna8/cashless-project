using nmct.ba.cashlessproject.helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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
        [Required(ErrorMessage = "Naam kassa is verplicht")]
        [RegularExpression(@"^[a-zA-Z''-'\s]{2,50}$", ErrorMessage = "Naam kassa tussen de 2 en 50 karakters, geen cijfers en speciale tekens")]
        public string RegisterName
        {
            get { return _registerName; }
            set { _registerName = value; }
        }

        private string _device;

        [DisplayName("Soort toestel")]
        [Required(ErrorMessage = "Naam toestel is verplicht")]
        [RegularExpression(@"^[a-zA-Z''-'\s]{2,50}$", ErrorMessage = "Naam kassa tussen de 2 en 50 karakters, geen cijfers en speciale tekens")]
        public string Device
        {
            get { return _device; }
            set { _device = value; }
        }

        private DateTime _purchaseDate;

        [DisplayName("Aangekocht op")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime PurchaseDate
        {
            get { return _purchaseDate; }
            set { _purchaseDate = value; }
        }

        private DateTime _expiresDate;

        [DisplayName("Verloopt op")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime ExpiresDate
        {
            get { return _expiresDate; }
            set { _expiresDate = value; }
        }
    }
}
