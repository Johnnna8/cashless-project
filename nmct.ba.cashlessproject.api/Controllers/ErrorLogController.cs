using nmct.ba.cashlessproject.api.Models;
using nmct.ba.cashlessproject.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace nmct.ba.cashlessproject.api.Controllers
{
    public class ErrorLogController : ApiController
    {
        // GET: api/ErrorLog
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/ErrorLog/5
        public string Get(int id)
        {
            return "value";
        }

        // PUT: api/ErrorLog/5
        public HttpResponseMessage Post(ErrorLog el)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            ErrorlogDA.InsertError(el, p.Claims);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        // PUT: api/ErrorLog/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ErrorLog/5
        public void Delete(int id)
        {
        }
    }
}
