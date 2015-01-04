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
    public class ProductController : ApiController
    {
        // GET: api/Product
        public List<Product> Get()
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            return ProductDA.GetProducts(p.Claims);
        }

        // GET: api/Product/5
        public Product Get(int id)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            return ProductDA.GetProduct(id, p.Claims);
        }

        public HttpResponseMessage Post(Product pr)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            int id = ProductDA.InsertProduct(pr, p.Claims);

            HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.OK);
            message.Content = new StringContent(id.ToString());
            return message;
        }

        public HttpResponseMessage Put(Product pr)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            ProductDA.UpdateProduct(pr, p.Claims);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        //public HttpResponseMessage Delete(int id)
        //{
        //    ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
        //    ProductDA.DeleteProduct(id, p.Claims);
        //    return new HttpResponseMessage(HttpStatusCode.OK);
        //}
    }
}
