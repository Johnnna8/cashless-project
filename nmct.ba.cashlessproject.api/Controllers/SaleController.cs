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
    public class SaleController : ApiController
    {
        // GET: api/Sale
        public List<Sale> Get()
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            List<Sale> sales = SaleDA.GetSales(p.Claims);
            return sales;
        }

        // GET: api/Sale/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Sale
        public HttpResponseMessage Post(Sale s)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            int id = SaleDA.AddSale(s, p.Claims);

            HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.OK);
            message.Content = new StringContent(id.ToString());
            return message;
        }

        // PUT: api/Sale/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Sale/5
        public void Delete(int id)
        {
        }
    }
}
