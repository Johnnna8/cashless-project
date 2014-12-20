using nmct.ba.cashlessproject.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nmct.ssa.cashlessproject.itcompany.Models.PM
{
    public class PMRegister
    {
        public PMRegister()
        {
            Register = new Register();
        }

        public Register Register { get; set; }
        public int From { get; set; }
        public int Until { get; set; }
        public Organisation Organisation { get; set; }
    }
}