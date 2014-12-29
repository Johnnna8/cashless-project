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

        private int _fromTime;

        public int FromTime
        {
            get { return _fromTime; }
            set { _fromTime = value; }
        }

        private int _untilTime;

        public int UntilTime
        {
            get { return _untilTime; }
            set { _untilTime = value; }
        }
    }
}
