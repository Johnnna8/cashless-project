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
    [Authorize]
    public class ErrorLogAdminController : ApiController
    {
        // GET: api/ErrorLogAdmin
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/ErrorLogAdmin/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/ErrorLogAdmin
        public HttpResponseMessage Post(ErrorLog el)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            ErrorlogAdminDA.InsertError(el);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        // PUT: api/ErrorLogAdmin/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ErrorLogAdmin/5
        public void Delete(int id)
        {
        }
    }
}
