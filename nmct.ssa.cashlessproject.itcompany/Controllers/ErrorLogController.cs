using nmct.ba.cashlessproject.model;
using nmct.ssa.cashlessproject.itcompany.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nmct.ssa.cashlessproject.itcompany.Controllers 
{
    [Authorize]
    public class ErrorLogController : Controller
    {
        // GET: ErrorLog
        [HttpGet]
        public ActionResult Index()
        {
            List<ErrorLog> errorLogs = new List<ErrorLog>();
            errorLogs = ErrorLogDA.GetErrorLogs();

            return View(errorLogs);
        }
    }
}