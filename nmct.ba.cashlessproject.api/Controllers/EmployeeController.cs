using nmct.ba.cashlessproject.api.Models;
using nmct.ba.cashlessproject.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace nmct.ba.cashlessproject.api.Controllers
{
    public class EmployeeController : ApiController
    {
        // GET: api/Employee
        public List<Employee>Get()
        {
            List<Employee> employees = EmployeeDA.GetEmployees();
            return employees;
        }

        // GET: api/Employee/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Employee
        public HttpResponseMessage Post(Employee em)
        {
            if (em == null ||
                em.Firstname == null ||
                em.Lastname == null ||
                em.Street == null ||
                em.StreetNumber == null ||
                em.Postcode == null ||
                em.City == null ||
                em.Email == null ||
                em.Phone == null
                )
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            else
            {
                EmployeeDA.CreateEmployee(em);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
        }

        // PUT: api/Employee/5
        public HttpResponseMessage Put(Employee em)
        {
            if (em == null ||
                em.Firstname == null ||
                em.Lastname == null ||
                em.Street == null ||
                em.StreetNumber == null ||
                em.Postcode == null ||
                em.City == null ||
                em.Email == null ||
                em.Phone == null
                )
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            else
            {
                EmployeeDA.UpdateEmployee(em);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
        }

        // DELETE: api/Employee/5
        public HttpResponseMessage Delete(int id)
        {
            if (id == null) {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            else
            {
                EmployeeDA.DeleteEmployee(id);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
        }
    }
}
