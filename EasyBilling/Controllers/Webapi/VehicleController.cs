using EasyBilling.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Web.Http;

namespace EasyBilling.Controllers.Webapi
{
    [RoutePrefix("api/vehicles")]
    public class VehicleController : ApiController
    {
        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> GetAll()
        {
            IList<VehicleClass> product = null;

            using (var ctx = new EasyBillingEntities())
            {

                product = ctx.Vehicles.Select(s => new VehicleClass()
                {
                    Token_number = s.Token_number,
                    Vehicle_type = s.Vehicle_type,
                    Vehicle_make = s.Vehicle_make,
                    Date = s.Date

                }).OrderByDescending(x => x.Date).Distinct().ToList<VehicleClass>();


            }
            return Ok(product);

        }

        [HttpPost]
        [Route("PostSave")]
        public async Task<IHttpActionResult> PostSave([FromBody] Vehicle vehicle)
        {
            using (EasyBillingEntities db = new EasyBillingEntities())
            {
               // vehicle.Date = DateTime.Parse(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));
                if (ModelState.IsValid)
                {
                    if (!string.IsNullOrEmpty(vehicle.Vehicle_make) || !string.IsNullOrEmpty(vehicle.Vehicle_type))
                    {
                            bool vehiclecheck = db.Vehicles.Where(z => z.Vehicle_type == vehicle.Vehicle_type && z.Vehicle_make == vehicle.Vehicle_make).Any();
                      
                            if (vehiclecheck == true)
                            {
                                return BadRequest("Vehicle make and type should not duplicate");
                            }
                        else
                        {
                            vehicle.Token_number = Guid.NewGuid().ToString();
                            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
                            String sMacAddress = string.Empty;
                            foreach (NetworkInterface adapter in nics)
                            {
                                IPInterfaceProperties properties = adapter.GetIPProperties();
                                sMacAddress = adapter.GetPhysicalAddress().ToString();
                                if (!string.IsNullOrEmpty(sMacAddress))// only return MAC Address from first card  
                                    break;
                            }
                            vehicle.Mac_id = sMacAddress;
                            vehicle.Date = DateTime.Now;
                            vehicle.User_Id = User.Identity.Name;
                            vehicle.User_name = db.Employees.Where(z => z.Employee_Id == User.Identity.Name).Select(z => z.Employee_name).FirstOrDefault();
                            db.Vehicles.Add(vehicle);
                            await db.SaveChangesAsync();
                            return Created("", vehicle);

                        }

                    }
                    else
                    {
                        return BadRequest("Vehicle make and type should not empty");
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
                return await Task.FromResult(Ok(db.Vehicles.Where(z => z.Token_number == id).Distinct().FirstOrDefault()));
            }
        }


        [HttpDelete]
        [Route("{id}")]
        public async Task<IHttpActionResult> Delete([FromUri]string id)
        {
            using (EasyBillingEntities db = new EasyBillingEntities())
            {
                Vehicle vehicleforDeletion = db.Vehicles.Where(z => z.Token_number == id).Distinct().FirstOrDefault();
                if (db.Item_Tyres.Where(x => x.Vehicle_token == id).Any())
                {
                    int count_tube = db.Item_Tyres.Where(x => x.Vehicle_token == id).Count();

                    if (count_tube > 1)
                        return BadRequest("One or the other records are associated hence unable to delete");
                    else
                        return BadRequest("The record is associated hence unable to delete");
                }
                else
                {
                    if (vehicleforDeletion != null)
                    {
                        db.Vehicles.Remove(vehicleforDeletion);
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
                    Vehicle companyforDeletion = ctx.Vehicles.Where(z => z.Token_number == id).Distinct().FirstOrDefault();
                    if (ctx.Item_Tyres.Where(x => x.Vehicle_token == id).Any())
                    {
                        int count_tube = ctx.Item_Tyres.Where(x => x.Vehicle_token == id).Count();

                        if (count_tube > 1)
                            return BadRequest("One or the other records are associated hence unable to delete");
                        else
                            return BadRequest("The record is associated hence unable to delete");
                    }
                }
                ctx.Vehicles.RemoveRange(ctx.Vehicles.Where(x => list.Contains(x.Token_number)).ToList());
                ctx.SaveChanges();
                return Ok();
            }
        }
    }
}
