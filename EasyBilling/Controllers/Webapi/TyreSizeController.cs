using EasyBilling.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Web.Http;


namespace EasyBilling.Controllers.Webapi
{
    [RoutePrefix("api/tyresizes")]
    public class TyreSizeController : ApiController
    {
        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> GetAll()
        {
            IList<TyreSizes> product = null;

            using (var ctx = new EasyBillingEntities())
            {

                product = ctx.Tyre_sizes.Select(s => new TyreSizes()
                {
                    Token_number = s.Token_number,
                    Tyre_size1 = s.Tyre_size1,
                    Date = s.Date


                }).OrderByDescending(x => x.Date).Distinct().ToList<TyreSizes>();


            }
            return Ok(product);

        }
        [HttpPost]
        [Route("Deletesel")]
        public async Task<IHttpActionResult> Deletesel(List<string> list)
        {

            using (var ctx = new EasyBillingEntities())
            {
                foreach (string id in list)
                {
                    Tyre_size companyforDeletion = ctx.Tyre_sizes.Where(z => z.Token_number == id).Distinct().FirstOrDefault();
                    if (ctx.Item_Tubes.Where(x => x.Size_token == id).Any() || ctx.Item_Tyres.Where(x => x.Tyre_token == id).Any())
                    {
                        int count_tube = ctx.Item_Tubes.Where(x => x.Size_token == id).Count();
                        int count_tyre = ctx.Item_Tyres.Where(x => x.Tyre_token == id).Count();
                        if (count_tube + count_tyre > 1)
                            return BadRequest("One or the other records are associated hence unable to delete");
                        else
                            return BadRequest("The record is associated hence unable to delete");
                    }

                }
                ctx.Tyre_sizes.RemoveRange(ctx.Tyre_sizes.Where(x => list.Contains(x.Token_number)).ToList());
               ctx.SaveChanges();
             
                return Ok();

            }


        }
        [HttpPost]
        [Route("PostSave")]
        public async Task<IHttpActionResult> PostSave([FromBody] Tyre_size tyre_Size)
        {
            using (EasyBillingEntities db = new EasyBillingEntities())
            {
                //tyre_Size.Date = DateTime.Parse(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));
                if (ModelState.IsValid)
                {
                    if (!string.IsNullOrEmpty(tyre_Size.Tyre_size1))
                    {
                        bool Employeecheck = db.Tyre_sizes.Where(z => z.Tyre_size1 == tyre_Size.Tyre_size1).Any();
                        if (Employeecheck == true)
                        {
                            return BadRequest("Size should not duplicate");
                        }
                        else
                        {
                            Tyre_size tyre_Sizeforupdate = db.Tyre_sizes.Where(z => z.Token_number == tyre_Size.Token_number)
                                .Distinct().FirstOrDefault();
                            
                            if (tyre_Sizeforupdate != null)
                            {
                                tyre_Sizeforupdate.Tyre_size1 = tyre_Size.Tyre_size1;

                                await db.SaveChangesAsync();
                                return Ok();
                            }
                            tyre_Size.Token_number = Guid.NewGuid().ToString();
                            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
                            String sMacAddress = string.Empty;
                            foreach (NetworkInterface adapter in nics)
                            {
                                IPInterfaceProperties properties = adapter.GetIPProperties();
                                sMacAddress = adapter.GetPhysicalAddress().ToString();
                                if (!string.IsNullOrEmpty(sMacAddress))// only return MAC Address from first card  
                                    break;
                            }
                            tyre_Size.User_Id = User.Identity.Name;
                            tyre_Size.User_name = db.Employees.Where(z => z.Employee_Id == User.Identity.Name).Select(z => z.Employee_name).FirstOrDefault();
                            tyre_Size.Mac_id = sMacAddress;
                            tyre_Size.Date = DateTime.Now;
                            db.Tyre_sizes.Add(tyre_Size);
                            await db.SaveChangesAsync();
                            return Created("", tyre_Size);
                        }
                    }
                    else
                    {
                        return BadRequest("Name should not empty");
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
                return await Task.FromResult(Ok(db.Tyre_sizes.Where(z => z.Token_number == id).Distinct().FirstOrDefault()));
            }
        }


        [HttpDelete]
        [Route("{id}")]
        public async Task<IHttpActionResult> Delete([FromUri]string id)
        {
            using (EasyBillingEntities db = new EasyBillingEntities())
            {
                Tyre_size tyresizeforDeletion = db.Tyre_sizes.Where(z => z.Token_number == id).Distinct().FirstOrDefault();
                if (db.Item_Tubes.Where(x => x.Size_token == id).Any() || db.Item_Tyres.Where(x => x.Tyre_token == id).Any())
                {
                    int count_tube = db.Item_Tubes.Where(x => x.Size_token == id).Count();
                    int count_tyre = db.Item_Tyres.Where(x => x.Tyre_token == id).Count();
                    if (count_tube + count_tyre > 1)
                        return BadRequest("One or the other records are associated hence unable to delete");
                    else
                        return BadRequest("The record is associated hence unable to delete");
                }
                else
                {
                    if (tyresizeforDeletion != null)
                    {
                        db.Tyre_sizes.Remove(tyresizeforDeletion);
                        await db.SaveChangesAsync();
                        ModelState.Clear();
                        return Ok();
                    }
                    return BadRequest($"Invalid token number {id}");
                }
            }
        }
    }
}
