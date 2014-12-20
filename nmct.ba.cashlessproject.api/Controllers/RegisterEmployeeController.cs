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
    public class RegisterEmployeeController : ApiController
    {
        // GET: api/RegisterEmployee
        public List<RegisterEmployee> GetEmployeesFromRegister(int registerid)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            return RegisterEmployeeDA.GetEmployeesPerRegister(registerid, p.Claims);
        }

        // GET: api/RegisterEmployee/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/RegisterEmployee
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/RegisterEmployee/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/RegisterEmployee/5
        public void Delete(int id)
        {
        }
    }
}
