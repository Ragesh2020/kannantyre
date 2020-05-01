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
    [RoutePrefix("api/balancesheet")]
    public class ProductsForSaleController : ApiController
    {
        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> GetAll()
        {
            IList<ProductForSaleClass> productForSaleClasses = null;

            using (var ctx = new EasyBillingEntities())
            {

                productForSaleClasses = ctx.Products_For_Sales.Select(s => new ProductForSaleClass()
                {Token_Number=s.Token_Number,
                    Product_name = s.Product_name,
                    Total = s.Total,
                    CGST = s.CGST,
                    SGST = s.SGST,
                    Selling_CGST = s.Selling_CGST,
                    Selling_SGST = s.Selling_SGST,

                }).Distinct().ToList();


            }
            return Ok(productForSaleClasses);

        }
    }
}
