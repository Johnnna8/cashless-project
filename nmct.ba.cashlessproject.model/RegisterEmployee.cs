using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.ba.cashlessproject.model
{
    public class RegisterEmployee
    {
        private Register _registerID;

        public Register RegisterID
        {
            get { return _registerID; }
            set { _registerID = value; }
        }

        private Employee _employeeID;

        public Employee EmployeeID
        {
            get { return _employeeID; }
            set { _employeeID = value; }
        }

        private int _from;

        public int From
        {
            get { return _from; }
            set { _from = value; }
        }

        private int _until;

        public int Until
        {
            get { return _until; }
            set { _until = value; }
        }
    }
}
