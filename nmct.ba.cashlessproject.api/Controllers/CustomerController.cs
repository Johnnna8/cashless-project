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
    public class CustomerController : ApiController
    {
        [Authorize]
        // GET: api/Customer
        public List<Customer> Get()
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            return CustomerDA.GetCustomers(p.Claims);
        }

        // GET: api/Customer/5
        public Customer Get(int id)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            return CustomerDA.GetCustomer(id, p.Claims);
        }

        public int customerExists(int NationalNumber)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            Customer customer = CustomerDA.GetCustomerByNationalNumber(NationalNumber, p.Claims);

            if (customer == null)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }

        public HttpResponseMessage Post(Customer c)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            int id = CustomerDA.InsertCustomer(c, p.Claims);

            HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.OK);
            message.Content = new StringContent(id.ToString());
            return message;
        }

        public HttpResponseMessage Put(Customer c)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            CustomerDA.UpdateCustomer(c, p.Claims);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        public HttpResponseMessage Delete(int id)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            CustomerDA.DeleteCustomer(id, p.Claims);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
