using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.ba.cashlessproject.model
{
    public class Product : IDataErrorInfo
    {
        private int _ID;

        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        private string _productName;

        [Required(ErrorMessage = "Productnaam is verplicht")]
        [RegularExpression(@"^[a-zA-Z''-'\s]{2,50}$", ErrorMessage = "Productnaam tussen de 2 en 50 karakters")]
        public string ProductName
        {
            get { return _productName; }
            set { _productName = value; }
        }

        private double _price;

        [Required(ErrorMessage = "Prijs is verplicht")]
        [Range(0.01, 10000, ErrorMessage="Prijs tussen 0.01 en 10000")]
        public double Price
        {
            get { return _price; }
            set { _price = value; }
        }

        public override string ToString()
        {
            return ProductName + " (" + Price + ")";
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
