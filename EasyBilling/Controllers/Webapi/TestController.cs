using EasyBilling.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace EasyBilling.Controllers.Webapi
{
    [RoutePrefix("api/stocksitms1")]
    public class TestController : ApiController
    {
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAll()
        {
            IList<ProductForSaleClass> productForSaleClasses = null;

            using (var ctx = new EasyBillingEntities())
            {

                productForSaleClasses = ctx.Products_For_Sales.Select(s => new ProductForSaleClass()
                {
                    Token_Number=s.Token_Number,
                    Date = s.Date,
                    Tyre_make = s.Tyre_make,
                    Product_name = s.Product_name,
                    Tyre_Size = s.Tyre_Size,
                    Supplier_name = s.Supplier_name,
                    Pieces = s.Pieces,
                    Tyre_type=s.Tyre_type,
                    Approve = s.Approve
                   
                    
                }).Distinct().ToList<ProductForSaleClass>();
                
            }
            return Ok(productForSaleClasses);

        }

        [HttpPut]
        [Route("")]
        public async Task<IHttpActionResult> Save([FromBody] Products_For_Sale products_For_Sale)
        {
            using (EasyBillingEntities db = new EasyBillingEntities())
            {
                products_For_Sale.Token_Number = Guid.NewGuid().ToString();
                if (ModelState.IsValid)
                {

                    products_For_Sale.Product_name = db.Products.Where(z => z.Token_Number == products_For_Sale.Product_Token).Select(z => z.Product_name).FirstOrDefault();
                    //bool checkitemid = db.Products_For_Sales.Where(z => z.Item_tyre_Id == products_For_Sale.Item_tyre_Id).Any();
                    products_For_Sale.Item_tyre_Id = db.Item_Tyres.Where(z => z.Token_number == products_For_Sale.Item_tyre_token).Select(z => z.Item_Id).FirstOrDefault();
                    if (string.IsNullOrEmpty(products_For_Sale.Item_tyre_token))
                    {
                        return BadRequest("Item tyre Id should be seleted.");
                    }
                    else if (string.IsNullOrEmpty(products_For_Sale.Purchase_number))
                    {
                        return BadRequest("Purchase number should not be empty.");
                    }else
                    {
                        products_For_Sale.Approve = false;
                        products_For_Sale.Supplier_name = (from sup in db.Dealers
                                                           where sup.Token_number == products_For_Sale.Supplier_token
                                                           select sup.Name).FirstOrDefault();

                        products_For_Sale.Selling_Price = 0;
                        products_For_Sale.Up_Selling_Price = 0;
                        products_For_Sale.Total = 0;
                        //products_For_Sale.Tyre_Size = (from sup in db.Tyre_sizes
                        //                               where sup.Token_number == products_For_Sale.Tyre_token
                        //                               select sup.Tyre_size1).FirstOrDefault();

                        //products_For_Sale.Vehicle_type = (from sup in db.Vehicles
                        //                                  where sup.Token_number == products_For_Sale.Vehicle_Token
                        //                                  select sup.Vehicle_type).FirstOrDefault();

                        Purchase_Invoice purchase_Invoice = new Purchase_Invoice();

                        purchase_Invoice.Token_number = Guid.NewGuid().ToString();
                        purchase_Invoice.Purchase_invoice_number = products_For_Sale.Purchase_number;
                        bool chkinv = db.Purchase_Invoices.Where(z => z.Purchase_invoice_number == purchase_Invoice.Purchase_invoice_number).Any();
                        if (chkinv == true)
                        {
                            return BadRequest(purchase_Invoice.Purchase_invoice_number + " number already exist. Try with another.");
                        }
                        else
                        {
                            purchase_Invoice.Date = products_For_Sale.PurchaseDate;
                            purchase_Invoice.Stock_entry_token = products_For_Sale.Token_Number;
                            // products_For_Sale.Product = db.Products.Where(x=>x.Token_Number==products_For_Sale.Product_Token).FirstOrDefault();
                            db.Products_For_Sales.Add(products_For_Sale);
                            db.Purchase_Invoices.Add(purchase_Invoice);
                            await db.SaveChangesAsync();

                            return Created("", products_For_Sale);
                        }
                    }
                }
                else
                {
                    var snglerrr = string.Empty;
                    //snglerrr = "-----------Error------------------";
                    foreach (var errr in ModelState.Values)
                    {

                        for (int i = 0; i < errr.Errors.Count; i++)
                        {
                            int j = i + 1;
                            if (string.IsNullOrEmpty(errr.Errors[i].ErrorMessage))
                            {
                                if (string.IsNullOrEmpty(snglerrr))
                                    snglerrr = "Something went wrong. Please check your data and try again.";
                               
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(snglerrr))
                                    snglerrr = j + ". " + errr.Errors[i].ErrorMessage;
                                else
                                    snglerrr = snglerrr + "\n" + j + ". " + errr.Errors[i].ErrorMessage;
                            }
                            //errors.Add(snglerrr);
                        }
                    }

                    return BadRequest(snglerrr);
                }
            }
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IHttpActionResult> GetById([FromUri] string id)
        {
            using (EasyBillingEntities db = new EasyBillingEntities())
            {
                return await Task.FromResult(Ok(db.Products_For_Sales.Where(z => z.Token_Number == id).Distinct().FirstOrDefault()));
            }
        }


        [HttpDelete]
        [Route("{id}")]
        public async Task<IHttpActionResult> Delete([FromUri]string id)
        {
            using (EasyBillingEntities db = new EasyBillingEntities())
            {
                Products_For_Sale products_For_Sale = db.Products_For_Sales.Where(z => z.Token_Number == id).Distinct().FirstOrDefault();
                Purchase_Invoice purchase_Invoice = db.Purchase_Invoices.Where(z => z.Purchase_invoice_number == products_For_Sale.Purchase_number).Distinct().FirstOrDefault();
                if (products_For_Sale != null)
                {
                    db.Products_For_Sales.Remove(products_For_Sale);
                    db.Purchase_Invoices.Remove(purchase_Invoice);
                    await db.SaveChangesAsync();
                    ModelState.Clear();
                    return Ok();
                }
                return BadRequest($"Invalid token number {id}");
            }
        }
    }
}
