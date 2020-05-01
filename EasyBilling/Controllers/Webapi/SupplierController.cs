using EasyBilling.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Web.Http;

namespace EasyBilling.Controllers.Webapi
{
    [RoutePrefix("api/supplier")]
    public class SupplierController : ApiController
    {
        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> GetAll()
        {
            IList<DealerAccount> product = null;

            using (var ctx = new EasyBillingEntities())
            {

                product = ctx.Dealers.Select(s => new DealerAccount()
                {
                    Token_number = s.Token_number,
                    Name = s.Name,
                    Phone_number = s.Phone_number,
                    Address = s.Address,
                    Email = s.Email,
                    Office_number = s.Office_number,
                    GST_number=s.GST_number,Pan_number=s.Pan_number,
                    Date=s.Date

                }).OrderByDescending(x => x.Date).Distinct().ToList<DealerAccount>();


            }
            return Ok(product);

        }
      
        [HttpPost]
        [Route("PostSave")]
        public async Task<IHttpActionResult> PostSave([FromBody] Dealer dealer)
        {
            using (EasyBillingEntities db = new EasyBillingEntities())
            {
               // dealer.Date = DateTime.Parse(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));
                if (ModelState.IsValid)
                {
                    if (!string.IsNullOrEmpty(dealer.Name) || !string.IsNullOrEmpty(dealer.Phone_number))
                    {

                        Dealer dealerforupdate = db.Dealers.Where(z => z.Token_number == dealer.Token_number)
                            .Distinct().FirstOrDefault();
                        if (dealerforupdate != null)
                        {
                            dealerforupdate.Name = dealerforupdate.Name;
                            dealerforupdate.Phone_number = dealerforupdate.Phone_number;
                            dealerforupdate.Address = dealerforupdate.Address;
                            await db.SaveChangesAsync();
                            return Ok();
                        }
                        else
                        {

                            bool Employeecheck = db.Dealers.Where(z => z.Name == dealer.Name).Any();
                            bool phncheck = db.Dealers.Where(z => z.Phone_number == dealer.Phone_number).Any();
                            if (Employeecheck == true)
                            {
                                return BadRequest("Supplier name should not duplicate");
                            }
                            else
                            {
                                if (phncheck == true)
                                {
                                    return BadRequest("Phone number should not duplicate");
                                }
                                else
                                {
                                    dealer.Token_number = Guid.NewGuid().ToString();
                                    NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
                                    String sMacAddress = string.Empty;
                                    foreach (NetworkInterface adapter in nics)
                                    {
                                        IPInterfaceProperties properties = adapter.GetIPProperties();
                                        sMacAddress = adapter.GetPhysicalAddress().ToString();
                                        if (!string.IsNullOrEmpty(sMacAddress))// only return MAC Address from first card  
                                            break;
                                    }
                                    dealer.Mac_id = sMacAddress;
                                    dealer.Isactive = true;
                                    dealer.Date = DateTime.Now;
                                    dealer.User_Id = User.Identity.Name;
                                    dealer.User_name = db.Employees.Where(z => z.Employee_Id == User.Identity.Name).Select(z => z.Employee_name).FirstOrDefault();
                                    db.Dealers.Add(dealer);
                                    await db.SaveChangesAsync();
                                    return Created("", dealer);
                                }
                            }
                        }
                    }
                    else
                    {
                        return BadRequest("Name or phone number should not empty");
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
                return await Task.FromResult(Ok(db.Dealers.Where(z => z.Token_number == id).Distinct().FirstOrDefault()));
            }
        }


        [HttpDelete]
        [Route("{id}")]
        public async Task<IHttpActionResult> Delete([FromUri]string id)
        {
            using (EasyBillingEntities db = new EasyBillingEntities())
            {
                Dealer dealerforDeletion = db.Dealers.Where(z => z.Token_number == id).Distinct().FirstOrDefault();
                if (db.Products_For_Sales.Where(x => x.Supplier_token == id).Any())
                {
                    int count_tube = db.Products_For_Sales.Where(x => x.Supplier_token == id).Count();

                    if (count_tube > 1)
                        return BadRequest("One or the other records are associated hence unable to delete");
                    else
                        return BadRequest("The record is associated hence unable to delete");
                }
                else
                {
                    if (dealerforDeletion != null)
                    {
                        db.Dealers.Remove(dealerforDeletion);
                        await db.SaveChangesAsync();
                        ModelState.Clear();
                        return Ok();
                    }
                    return BadRequest($"Invalid token number {id}");
                }
            }
        }
        [HttpPost]
        [Route("Deletesel")]
        public async Task<IHttpActionResult> Deletesel(List<string> list)
        {

            using (var ctx = new EasyBillingEntities())
            {
                foreach (string id in list)
                {
                    Dealer companyforDeletion = ctx.Dealers.Where(z => z.Token_number == id).Distinct().FirstOrDefault();
                    if (ctx.Products_For_Sales.Where(x => x.Supplier_token == id).Any())
                    {
                        int count_tube = ctx.Products_For_Sales.Where(x => x.Supplier_token == id).Count();

                        if (count_tube > 1)
                            return BadRequest("One or the other records are associated hence unable to delete");
                        else
                            return BadRequest("The record is associated hence unable to delete");
                    }

                }
                ctx.Dealers.RemoveRange(ctx.Dealers.Where(x => list.Contains(x.Token_number)).ToList());
                ctx.SaveChanges();

                return Ok();

            }


        }
    }
}
