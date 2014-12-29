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
    public class RegisterEmployeeController : ApiController
    {
        // GET: api/RegisterEmployee
        public List<RegisterEmployee> GetEmployeesFromRegister(int registerid)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            return RegisterEmployeeDA.GetEmployeesPerRegister(registerid, p.Claims);
        }

        public HttpResponseMessage Post(RegisterEmployee re)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            int id = RegisterEmployeeDA.InsertRegisterEmployee(re, p.Claims);

            HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.OK);
            message.Content = new StringContent(id.ToString());
            return message;
        }

        public HttpResponseMessage Put(RegisterEmployee re)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            RegisterEmployeeDA.UpdateRegisterEmployee(re, p.Claims);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
