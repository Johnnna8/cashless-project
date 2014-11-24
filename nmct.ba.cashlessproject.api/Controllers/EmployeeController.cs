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
            return EmployeeDA.GetEmployees();
        }

        // GET: api/Employee/5
        public Employee Get(int id)
        {
            return EmployeeDA.GetEmployee(id);
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
            if (id == 0) {
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
