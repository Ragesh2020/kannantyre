using EasyBilling.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EasyBilling.Controllers.Webapi
{
    public class ProductController : ApiController
    {
        public IHttpActionResult GetAllProducts(bool includeAddress = false)
        {
            IList<ProductClass> product = null;

            using (var ctx = new EasyBillingEntities())
            {

                product = ctx.Products.Select(s => new ProductClass()
                {
                    Token_Number=s.Token_Number,
                    Product_Code = s.Product_Code,
                    Product_name = s.Product_name
                  
                  
                }).ToList<ProductClass>();


            }


            if (product.Count == 0)
            {
                return NotFound();
            }

            return Ok(product);
        }
        public IHttpActionResult PostNewProduct([FromBody]Product product)
        {
            if (!ModelState.IsValid)
                return BadRequest("Sorry there is some problem. Please check and try again");

            using (var ctx = new EasyBillingEntities())
            {
                     
                        product.Token_Number = (Guid.NewGuid()).ToString();
                        ctx.Products.Add(product);
                        ctx.SaveChanges();
                    
               
            }

            return Ok();
        }

        public IHttpActionResult GetAllProducts(string id)
        {
           
            ProductClass product = null;

            using (var ctx = new EasyBillingEntities())
            {

                product = ctx.Products.Where(x => x.Token_Number == id).Select(s => new ProductClass()
                {
                    Token_Number = s.Token_Number,
                    Product_Code = s.Product_Code,
                    Product_name = s.Product_name
                 
                    
                }).FirstOrDefault();


            }


            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }
        public IHttpActionResult PutProducts([FromBody]Product product)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid data");

            using (var ctx = new EasyBillingEntities())
            {
                var existingProduct = ctx.Products.Where(s => s.Token_Number == product.Token_Number).FirstOrDefault();

                if (existingProduct != null)
                {
                    
                        //existingProduct.GL_CODE = product.txgrp;
                      
                        existingProduct.Product_name = product.Product_name;
                        existingProduct.Product_Code = product.Product_Code;
                      

                        ctx.SaveChanges();
                   
                    
                }
                else
                {
                    return NotFound();
                }
            }

            return Ok();
        }

        public IHttpActionResult Delete(string id)
        {
            if (id == null)
                return BadRequest("Not a valid student id");

            using (var ctx = new EasyBillingEntities())
            {
                var product = ctx.Products
                    .Where(s => s.Token_Number == id)
                    .FirstOrDefault();

                ctx.Entry(product).State = EntityState.Deleted;
                ctx.SaveChanges();
            }

            return Ok();
        }
      
        [System.Web.Http.ActionName("PostRateUpdate")]
        [System.Web.Http.HttpPost]
        public IHttpActionResult PostRateUpdate(JObject jsonData)
        {
            dynamic json = jsonData;
            string token = json.Token_number;
            string Upselling = json.UpSellingp;
            string Upcgst = json.Upcgst;
            string Upsgst = json.Upsgst;
            string UpsellingNetTotal = json.UpsellingNetTotal;

            using (var ctx = new EasyBillingEntities())
            {
                var existingProduct = ctx.Products_For_Sales.Where(s => s.Token_Number == token).FirstOrDefault();

                if (existingProduct != null)
                {
                    try {
                        if (decimal.Parse(Upselling) < 1 || decimal.Parse(Upselling) > 9999999)
                        {
                            return BadRequest("Selling price cannot be less that 1 or greater than 9999999");
                        }
                        else if (decimal.Parse(Upselling)*existingProduct.Pieces < existingProduct.Purchase_Price)
                        {
                            return BadRequest("Selling price cannot be less purchase price");
                        }
                        else
                        {
                           var existingitemiddetailsaslist= ctx.Products_For_Sales.Where(s => s.Item_tyre_Id == existingProduct.Item_tyre_Id).ToList();
                            if (existingitemiddetailsaslist.Count > 0)
                            {
                                Rate_update_Backup rate_Update_Backup = new Rate_update_Backup();
                                foreach (var echdtl in existingitemiddetailsaslist)
                                {
                                    rate_Update_Backup = new Rate_update_Backup();
                                    rate_Update_Backup.Token_number = Guid.NewGuid().ToString();
                                    rate_Update_Backup.Item_number = echdtl.Item_tyre_Id;
                                    rate_Update_Backup.Selling_CGST = echdtl.Selling_CGST;
                                    rate_Update_Backup.Selling_SGST = echdtl.Selling_SGST;
                                    rate_Update_Backup.Selling_rate = echdtl.Selling_Price;
                                    rate_Update_Backup.Date = DateTime.Now.Date;
                                    rate_Update_Backup.Date_of_stock_entry = echdtl.Approve_date;

                                    echdtl.Selling_CGST = decimal.Parse("0.00");
                                    echdtl.Selling_SGST = decimal.Parse("0.00");
                                    echdtl.Selling_Price = decimal.Parse("0.00");
                                    echdtl.Selling_net_total = decimal.Parse("0.00");
                                    ctx.Rate_update_Backups.Add(rate_Update_Backup);
                                    ctx.SaveChanges();
                                }
                            }
                            existingProduct.Selling_Price = decimal.Parse(Upselling);
                            existingProduct.Selling_CGST = decimal.Parse(Upcgst);
                            existingProduct.Selling_SGST = decimal.Parse(Upsgst);
                            existingProduct.Selling_net_total = decimal.Parse(UpsellingNetTotal);
                            existingProduct.Amout_after_tax = existingProduct.Selling_Price + (existingProduct.Selling_Price * (existingProduct.CGST + existingProduct.SGST) / 100);
                            existingProduct.Total = existingProduct.Amout_after_tax;
                            existingProduct.Rate_update_user_id = User.Identity.Name;
                            existingProduct.Rate_update_user_name = ctx.Employees.Where(z => z.Employee_Id == User.Identity.Name).Select(z => z.Employee_name).FirstOrDefault();
                            ctx.SaveChanges();
                        }
                    }
                    catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                    {
                        var snglerrr = string.Empty;
                        int j = 0;
                        Exception raise = dbEx;
                        foreach (var validationErrors in dbEx.EntityValidationErrors)
                        {
                            foreach (var validationError in validationErrors.ValidationErrors)
                            {
                                j++;
                                if (string.IsNullOrEmpty(snglerrr))
                                    snglerrr = j + ". " + validationError.ErrorMessage;
                                else
                                    snglerrr = snglerrr + "\n" + j + ". " + validationError.ErrorMessage;
                                //string message = string.Format(validationError.ErrorMessage);
                                
                                //raise = new InvalidOperationException(message, raise);
                            }
                        }
                       
                        return BadRequest(snglerrr);
                    }
                }
                else
                {
                    return NotFound();
                }
            }

            return Ok();
        }
       
     }
    }