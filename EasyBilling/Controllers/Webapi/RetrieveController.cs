using EasyBilling.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace EasyBilling.Controllers.Webapi
{
    //[RoutePrefix("api/Retrieve")]
    public class RetrieveController : ApiController
    {
        [HttpGet]
        //[Route("{id}/{id}")]
        public async Task<IHttpActionResult> GetAll([FromUri]string Sdate, [FromUri]string Edate)
        {
            IList<ProductClass> product = null;

            using (var ctx = new EasyBillingEntities())
            {
                product = ctx.Products.Select(s => new ProductClass()
                {
                    Token_Number = s.Token_Number,
                    Product_Code = s.Product_Code,
                    Product_name = s.Product_name,
                    Date = s.Date
                    
                }).OrderByDescending(x => x.Date).Distinct().ToList<ProductClass>();
                
            }
            return Ok(product);

        }
    }
}
