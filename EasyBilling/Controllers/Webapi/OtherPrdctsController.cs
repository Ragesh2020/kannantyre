using EasyBilling.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Web.Http;

namespace EasyBilling.Controllers.Webapi
{
    [RoutePrefix("api/otherproducts")]
    public class OtherPrdctsController : ApiController
    {
        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> GetAll()
        {
            IList<OtherProductClass> product = null;

            using (var ctx = new EasyBillingEntities())
            {

                product = ctx.Other_Products.Select(s => new OtherProductClass()
                {
                    Token_number = s.Token_number,
                    Product_id = s.Product_id,
                    Product_name = s.Product_name,
                    Product_type = s.Product_type,
                    Description = s.Description,
                    Date = s.Date


                }).OrderByDescending(x => x.Date).Distinct().ToList<OtherProductClass>();


            }
            return Ok(product);

        }

        [HttpPost]
        [Route("PostSave")]
        public async Task<IHttpActionResult> PostSave([FromBody] Other_Product other_Product)
        {
            using (EasyBillingEntities db = new EasyBillingEntities())
            {
                //other_Product.Date = DateTime.Parse(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));
                if (ModelState.IsValid)
                {
                    if (!string.IsNullOrEmpty(other_Product.Product_name) || !string.IsNullOrEmpty(other_Product.Product_type))
                    {
                        bool otherproductcheck = db.Other_Products.Where(z => z.Product_name == other_Product.Product_name && z.Product_type == other_Product.Product_type).Any();

                        if (otherproductcheck == true)
                        {
                            return BadRequest("Product name and type should not duplicate");
                        }
                        else
                        {
                            other_Product.Token_number = Guid.NewGuid().ToString();
                            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
                            String sMacAddress = string.Empty;
                            foreach (NetworkInterface adapter in nics)
                            {
                                IPInterfaceProperties properties = adapter.GetIPProperties();
                                sMacAddress = adapter.GetPhysicalAddress().ToString();
                                if (!string.IsNullOrEmpty(sMacAddress))// only return MAC Address from first card  
                                    break;
                            }
                            Random rd = new Random();
                            string id = rd.Next(1, 9999).ToString();
                            other_Product.Product_id = "PO-" + id;
                            other_Product.Mac_id = sMacAddress;
                            other_Product.Date = DateTime.Now;
                            other_Product.User_Id = User.Identity.Name;
                            other_Product.User_name = db.Employees.Where(z => z.Employee_Id == User.Identity.Name).Select(z => z.Employee_name).FirstOrDefault();
                            db.Other_Products.Add(other_Product);
                            await db.SaveChangesAsync();
                            return Created("", other_Product);

                        }

                    }
                    else
                    {
                        return BadRequest("Product name or type should not empty");
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

                            if (string.IsNullOrEmpty(snglerrr))
                                snglerrr = j + ". " + errr.Errors[i].ErrorMessage;
                            else
                                snglerrr = snglerrr + "\n" + j + ". " + errr.Errors[i].ErrorMessage;
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
                return await Task.FromResult(Ok(db.Other_Products.Where(z => z.Token_number == id).Distinct().FirstOrDefault()));
            }
        }


        [HttpDelete]
        [Route("{id}")]
        public async Task<IHttpActionResult> Delete([FromUri]string id)
        {
            using (EasyBillingEntities db = new EasyBillingEntities())
            {
               var Other_ProductothrprdctsforDeletion = db.Other_Products.Where(z => z.Token_number == id).Distinct().FirstOrDefault();
                if (Other_ProductothrprdctsforDeletion != null)
                {
                    db.Other_Products.Remove(Other_ProductothrprdctsforDeletion);
                    await db.SaveChangesAsync();
                    ModelState.Clear();
                    return Ok();
                }
                return BadRequest($"Invalid token number {id}");
            }
        }
        [HttpPost]
        [Route("Deletesel")]
        public async Task<IHttpActionResult> Deletesel(List<string> list)
        {
            using (var ctx = new EasyBillingEntities())
            {
               
                ctx.Other_Products.RemoveRange(ctx.Other_Products.Where(x => list.Contains(x.Token_number)).ToList());
                ctx.SaveChanges();
                return Ok();
            }
        }
    }
}

