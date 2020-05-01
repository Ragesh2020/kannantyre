using EasyBilling.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Web.Http;

namespace EasyBilling.Controllers.Webapi
{
    [RoutePrefix("api/tubelist")]
    public class ItemTubeController : ApiController
    {
        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> GetAll()
        {
            IList<TubelistClass> tubelistClasses = null;

            using (var ctx = new EasyBillingEntities())
            {

                tubelistClasses = ctx.Item_Tubes.Select(s => new TubelistClass()
                {
                    Token_number = s.Token_number,
                    Company_name = s.Company_name,
                    Item_Id = s.Item_Id,
                    Tube_size = s.Tube_size,
                    Date = s.Date


                }).OrderByDescending(x => x.Date).Distinct().ToList<TubelistClass>();


            }
            return Ok(tubelistClasses);

        }

        [HttpPost]
        [Route("PostSave")]
        public async Task<IHttpActionResult> PostSave([FromBody] Item_Tube item_Tube)
        {
            using (EasyBillingEntities db = new EasyBillingEntities())
            {

                //item_Tube.Date = DateTime.Parse(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));
                if (ModelState.IsValid)
                {
                    if (!string.IsNullOrEmpty(item_Tube.Company_token) || !string.IsNullOrEmpty(item_Tube.Size_token))
                    {
                        item_Tube.Token_number = Guid.NewGuid().ToString();
                        item_Tube.Company_name = db.Products.Where(z => z.Token_Number == item_Tube.Company_token).Select(z => z.Product_name).FirstOrDefault();
                        item_Tube.Tube_size = db.Tyre_sizes.Where(z => z.Token_number == item_Tube.Size_token).Select(z => z.Tyre_size1).FirstOrDefault();
                        Random rd = new Random();
                        string id = rd.Next(1, 9999).ToString();
                        //var lastId = (from a in db.Item_Tubes
                        //              orderby a.Item_Id descending
                        //              select a.Item_Id).Distinct().FirstOrDefault();
                        //if (!string.IsNullOrEmpty(lastId))
                        //{
                            //var text = lastId;
                            //var fstfr = text.Substring(0, 9);
                            //var lstfr = text.Substring(text.Length - 8);
                            //string newlstversn = (int.Parse(lstfr) + 100000001).ToString();
                            //string fstfr1 = (newlstversn.Substring(newlstversn.Length - 8)).ToString();
                            item_Tube.Item_Id = "TU" + item_Tube.Company_name.Substring(0, 3) + item_Tube.Tube_size + "-" + id;
                        //}
                        //else
                        //{
                        //    item_Tube.Item_Id = "TU" + item_Tube.Company_name.Substring(0, 3) + item_Tube.Tube_size + "-00000001";
                        //}

                        if (!string.IsNullOrEmpty(item_Tube.Item_Id))
                        {

                            bool chk = db.Item_Tubes.Where(z => z.Company_name.ToLower() == item_Tube.Company_name.ToLower() && z.Tube_size.ToLower() == item_Tube.Tube_size.ToLower()).Any();
                            if (chk == false)
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
                                item_Tube.Mac_id = sMacAddress;
                                item_Tube.Date = DateTime.Now;
                                item_Tube.User_Id = User.Identity.Name;
                                item_Tube.User_name = db.Employees.Where(z => z.Employee_Id == User.Identity.Name).Select(z => z.Employee_name).FirstOrDefault();
                                db.Item_Tubes.Add(item_Tube);
                                await db.SaveChangesAsync();

                                return Created("", item_Tube);
                            }
                            else
                            {
                                return BadRequest(item_Tube.Company_name + " along with " + item_Tube.Tube_size + " is already exist. Please try with another.");

                            }
                        }
                        else
                        {
                            return BadRequest("Item Id cannot be blank. Please try again.");
                        }
                    }
                    else
                    {
                        return BadRequest("Company name and size must be selected.");
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
                return await Task.FromResult(Ok(db.Item_Tubes.Where(z => z.Token_number == id).Distinct().FirstOrDefault()));
            }
        }


        [HttpDelete]
        [Route("{id}")]
        public async Task<IHttpActionResult> Delete([FromUri]string id)
        {
            using (EasyBillingEntities db = new EasyBillingEntities())
            {
                Item_Tube tubeforDeletion = db.Item_Tubes.Where(z => z.Token_number == id).Distinct().FirstOrDefault();
                if (tubeforDeletion != null)
                {
                    db.Item_Tubes.Remove(tubeforDeletion);
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
                    Item_Tube companyforDeletion = ctx.Item_Tubes.Where(z => z.Token_number == id).Distinct().FirstOrDefault();
                    if (ctx.Products_For_Sales.Where(x => x.Item_tyre_token == id).Any())
                    {
                        int count_tube = ctx.Products_For_Sales.Where(x => x.Item_tyre_token == id).Count();

                        if (count_tube > 1)
                            return BadRequest("One or the other records are associated hence unable to delete");
                        else
                            return BadRequest("The record is associated hence unable to delete");
                    }
                }
                ctx.Item_Tubes.RemoveRange(ctx.Item_Tubes.Where(x => list.Contains(x.Token_number)).ToList());
                ctx.SaveChanges();
                return Ok();
            }
        }
    }
}
