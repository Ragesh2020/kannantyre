using EasyBilling.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Web.Http;

namespace EasyBilling.Controllers.Webapi
{
    [RoutePrefix("api/tyrelist")]
    public class ItemTyreController : ApiController
    {
        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> GetAll()
        {
            IList<TyrelistClass> tyrelistClasses = null;

            using (var ctx = new EasyBillingEntities())
            {

                tyrelistClasses = ctx.Item_Tyres.Select(s => new TyrelistClass()
                {
                    Token_number = s.Token_number,
                    Item_Id = s.Item_Id,
                    Tyre_make = s.Tyre_make,
                    Tyre_type = s.Tyre_type,
                    Tyre_feel = s.Tyre_feel,
                    Company_name = s.Company_name,
                    Tyre_size = s.Tyre_size,
                    Vehicle_type = s.Vehicle_type,
                    Description = s.Description,
                     Date = s.Date
                }).OrderByDescending(x => x.Date).Distinct().ToList<TyrelistClass>();

            }
            return Ok(tyrelistClasses);

        }

        [HttpPost]
        [Route("PostSave")]
        public async Task<IHttpActionResult> PostSave([FromBody] Item_Tyre item_Tyre)
        {
            using (EasyBillingEntities db = new EasyBillingEntities())
            {
               // item_Tyre.Date = DateTime.Parse(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));
                if (ModelState.IsValid)
                {
                    if (!string.IsNullOrEmpty(item_Tyre.Company_token) || !string.IsNullOrEmpty(item_Tyre.Tyre_token) || !string.IsNullOrEmpty(item_Tyre.Tyre_feel) || !string.IsNullOrEmpty(item_Tyre.Tyre_make) || !string.IsNullOrEmpty(item_Tyre.Tyre_type))

                    {
                        item_Tyre.Token_number = Guid.NewGuid().ToString();
                        item_Tyre.Company_name = db.Products.Where(z => z.Token_Number == item_Tyre.Company_token).Select(z => z.Product_name).Distinct().FirstOrDefault();
                        item_Tyre.Tyre_size = db.Tyre_sizes.Where(z => z.Token_number == item_Tyre.Tyre_token).Select(z => z.Tyre_size1).Distinct().FirstOrDefault();

                        item_Tyre.Vehicle_type = db.Vehicles.Where(z => z.Token_number == item_Tyre.Vehicle_token).Select(z => z.Vehicle_type+" + "+ z.Vehicle_make).Distinct().FirstOrDefault();
                        Random rd = new Random();
                        string id = rd.Next(1, 9999).ToString();
                        //var lastId = (from a in db.Item_Tyres
                        //              orderby a.Item_Id descending
                        //              select a.Item_Id).Distinct().FirstOrDefault();
                        //if (!string.IsNullOrEmpty(lastId))
                        //{
                        //    var text = lastId;
                        //    var fstfr = text.Substring(0, 9);
                        //    var lstfr = text.Substring(text.Length - 8);
                        //    string newlstversn = (int.Parse(lstfr) + 100000001).ToString();
                        //    string fstfr1 = (newlstversn.Substring(newlstversn.Length - 8)).ToString();
                            if (item_Tyre.Tyre_type.ToLower().Equals("tubeless"))
                                item_Tyre.Item_Id = "TY" + item_Tyre.Company_name.Substring(0, 3) + item_Tyre.Tyre_size + "TUL-" + id;
                            else
                                item_Tyre.Item_Id = "TY" + item_Tyre.Company_name.Substring(0, 3) + item_Tyre.Tyre_size + "TUB-" + id;
                        //}
                        //else
                        //{
                        //    if (item_Tyre.Tyre_type.ToLower().Equals("tubeless"))
                        //        item_Tyre.Item_Id = "TY" + item_Tyre.Company_name.Remove(3) + item_Tyre.Tyre_size + "TUL-00000001";
                        //    else
                        //        item_Tyre.Item_Id = "TY" + item_Tyre.Company_name.Remove(3) + item_Tyre.Tyre_size + "TUB-00000001";
                        //}

                        if (!string.IsNullOrEmpty(item_Tyre.Item_Id))
                        {
                            bool chk = db.Item_Tyres.Where(z => z.Company_name.ToLower() == item_Tyre.Company_name.ToLower()
                            && z.Tyre_type.ToLower() == item_Tyre.Tyre_type.ToLower()
                            && z.Tyre_feel.ToLower() == item_Tyre.Tyre_feel.ToLower()
                            && z.Tyre_make.ToLower() == item_Tyre.Tyre_make.ToLower()
                            && z.Tyre_size.ToLower() == item_Tyre.Tyre_size.ToLower()
                            && z.Vehicle_type.ToLower() == item_Tyre.Vehicle_type.ToLower()).Any();
                            if (chk == true)
                            {
                                return BadRequest("Your entry already exist. Please try again another...");
                            }
                            else
                            {
                                NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
                                String sMacAddress = string.Empty;
                                foreach (NetworkInterface adapter in nics)
                                {
                                    IPInterfaceProperties properties = adapter.GetIPProperties();
                                    sMacAddress = adapter.GetPhysicalAddress().ToString();
                                    if (!string.IsNullOrEmpty(sMacAddress))// only return MAC Address from first card  
                                        break;
                                }
                                item_Tyre.Mac_id = sMacAddress;
                                item_Tyre.Date = DateTime.Now;
                                item_Tyre.User_Id = User.Identity.Name;
                                item_Tyre.User_name = db.Employees.Where(z => z.Employee_Id == User.Identity.Name).Select(z => z.Employee_name).FirstOrDefault();
                                db.Item_Tyres.Add(item_Tyre);
                                await db.SaveChangesAsync();
                                return Created("", item_Tyre);

                            }
                        }
                        else
                        {
                            return BadRequest("Something went wrong to generate Item id. Please refresh the page and try again. Thank you.");
                        }
                    }
                    else
                    {
                        return BadRequest("Company name, tyre type, tyre feel, tyre make, tyre size must be selected. ");
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
                return await Task.FromResult(Ok(db.Item_Tyres.Where(z => z.Token_number == id).Distinct().FirstOrDefault()));
            }
        }
        
        [HttpDelete]
        [Route("{id}")]
        public async Task<IHttpActionResult> Delete([FromUri]string id)
        {
            using (EasyBillingEntities db = new EasyBillingEntities())
            {
                Item_Tyre tyreforDeletion = db.Item_Tyres.Where(z => z.Token_number == id).Distinct().FirstOrDefault();
                if (tyreforDeletion != null)
                {
                    db.Item_Tyres.Remove(tyreforDeletion);
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
                foreach (string id in list)
                {
                    Item_Tyre companyforDeletion = ctx.Item_Tyres.Where(z => z.Token_number == id).Distinct().FirstOrDefault();
                    if (ctx.Products_For_Sales.Where(x => x.Item_tyre_token == id).Any())
                    {
                        int count_tube = ctx.Products_For_Sales.Where(x => x.Item_tyre_token == id).Count();

                        if (count_tube > 1)
                            return BadRequest("One or the other records are associated hence unable to delete");
                        else
                            return BadRequest("The record is associated hence unable to delete");
                    }
                }
                ctx.Item_Tyres.RemoveRange(ctx.Item_Tyres.Where(x => list.Contains(x.Token_number)).ToList());
                ctx.SaveChanges();
                return Ok();
            }
        }
    }
}
