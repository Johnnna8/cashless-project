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
    public class OrganisationController : ApiController
    {
        // GET: api/Organisation
        public Boolean GetPasswordCorrect(string oldPassword)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            //string dbDatabase = p.Claims.ElementAt(0).Value;
            string dbLogin = p.Claims.ElementAt(1).Value;
            string dbPassword = p.Claims.ElementAt(2).Value;

            Boolean passwordCorrect = OrganisationDA.checkOldPassword(dbLogin, dbPassword, oldPassword);
            return passwordCorrect;
        }

        // POST: api/Organisation
        public void Post([FromBody]string value)
        {
        }

        public HttpResponseMessage Put(string NewPassword)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            string dbLogin = p.Claims.ElementAt(1).Value;
            string dbPassword = p.Claims.ElementAt(2).Value;

            OrganisationDA.UpdatePassword(dbLogin, dbPassword, NewPassword);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        // DELETE: api/Organisation/5
        public void Delete(int id)
        {
        }
    }
}
