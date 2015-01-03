using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.ba.cashlessproject.model
{
    public class RegisterEmployee
    {
        private int _ID;

        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        private Register _register;

        public Register Register
        {
            get { return _register; }
            set { _register = value; }
        }

        private Employee _employee;

        public Employee Employee
        {
            get { return _employee; }
            set { _employee = value; }
        }

        private DateTime _fromTime;

        public DateTime FromTime
        {
            get { return _fromTime; }
            set { _fromTime = value; }
        }

        private DateTime _untilTime;

        public DateTime UntilTime
        {
            get { return _untilTime; }
            set { _untilTime = value; }
        }
    }
}
