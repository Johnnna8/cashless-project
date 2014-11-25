using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.ba.cashlessproject.model
{
    class ErrorLog
    {
        private Register _register;

        public Register Register
        {
            get { return _register; }
            set { _register = value; }
        }

        private int _timestamp;

        public int Timestamp
        {
            get { return _timestamp; }
            set { _timestamp = value; }
        }

        private string _message;

        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        private string _stacktrace;

        public string Stacktrace
        {
            get { return _stacktrace; }
            set { _stacktrace = value; }
        }
    }
}
