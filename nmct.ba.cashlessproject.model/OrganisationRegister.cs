using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.ba.cashlessproject.model
{
    public class OrganisationRegister
    {
        private int _ID;

        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        private Organisation _organisation;

        public Organisation Organisation
        {
            get { return _organisation; }
            set { _organisation = value; }
        }

        private RegisterCompany _register;

        public RegisterCompany Register
        {
            get { return _register; }
            set { _register = value; }
        }

        private DateTime _fromDate;

        [ReadOnly(true)]
        [DisplayName("In bezit van")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime FromDate
        {
            get { return _fromDate; }
            set { _fromDate = value; }
        }

        private DateTime _untilDate;

        [ReadOnly(true)]
        [DisplayName("In bezit tot")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime UntilDate
        {
            get { return _untilDate; }
            set { _untilDate = value; }
        }
    }
}
