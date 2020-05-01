using EasyBilling.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Web.Http;


namespace EasyBilling.Controllers.Webapi
{
    [RoutePrefix("api/company")]
    public class CompanyController : ApiController
    {
        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> GetAll()
        {
            IList<ProductClass> product = null;

            using (var ctx = new EasyBillingEntities())
            {

                product = ctx.Products.Select(s => new ProductClass()
                {
                    Token_Number = s.Token_Number,
                    Product_Code = s.Product_Code,
                    Product_name = s.Product_name,
                    Date=s.Date


                }).OrderByDescending(x => x.Date).Distinct().ToList<ProductClass>();


            }
            return Ok(product);
         
        }
       
        [HttpPost]
        [Route("Deletesel")]
        public IHttpActionResult Deletesel(List<string> list)
        {

            using (var ctx = new EasyBillingEntities())
            {
                foreach (string id in list)
                {
                    Product companyforDeletion = ctx.Products.Where(z => z.Token_Number == id).Distinct().FirstOrDefault();
                    if (ctx.Item_Tubes.Where(x => x.Company_token == id).Any() || ctx.Item_Tyres.Where(x => x.Company_token == id).Any())
                    {
                        int count_tube = ctx.Item_Tubes.Where(x => x.Company_token == id).Count();
                        int count_tyre = ctx.Item_Tyres.Where(x => x.Company_token == id).Count();
                        if (count_tube + count_tyre > 1)
                            return BadRequest("One or the other records are associated hence unable to delete");
                        else

                            return BadRequest("The record is associated hence unable to delete");
                    }
                   
                }
                ctx.Products.RemoveRange(ctx.Products.Where(x => list.Contains(x.Token_Number)).ToList());
                ctx.SaveChanges();
                //foreach (var id in list)
                //{
                //    var kst = ctx.Products.Where(x => x.Token_Number.Contains(id)).FirstOrDefault();
                //    if (kst != null)
                //    {
                //        ctx.Products.Remove(kst);
                //        ctx.SaveChanges();

                //    }
                //}
                return Ok();

            }


        }
        [HttpPost]
        [Route("PostSave")]
        public async Task<IHttpActionResult> PostSave([FromBody] Product company)
        {
            //IList<ProductClass> product = null;

            //using (var ctx = new EasyBillingEntities())
            //{

            //    company.Token_Number = Guid.NewGuid().ToString();
            //    company.Date = DateTime.Now;
            //    NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            //    String sMacAddress = string.Empty;
            //    foreach (NetworkInterface adapter in nics)
            //    {
            //        IPInterfaceProperties properties = adapter.GetIPProperties();
            //        sMacAddress = adapter.GetPhysicalAddress().ToString();
            //        if (!string.IsNullOrEmpty(sMacAddress))// only return MAC Address from first card  
            //            break;
            //    }
            //    company.Mac_id = sMacAddress;
            //    ctx.Products.Add(company);
            //    ctx.SaveChanges();
            //}
            //return Ok(company);

            using (EasyBillingEntities db = new EasyBillingEntities())
            {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(company.Product_name))
                {
                    bool Employeecheck = db.Products.Where(z => z.Product_name == company.Product_name).Any();
                    if (Employeecheck == true)
                    {
                        return BadRequest("Name should not duplicate");
                    }
                    else
                    {
                        Product Employeeforupdate = db.Products.Where(z => z.Token_Number == company.Token_Number)
                            .Distinct().FirstOrDefault();
                        if (Employeeforupdate != null)
                        {
                            Employeeforupdate.Product_name = company.Product_name;

                            await db.SaveChangesAsync();
                            return Ok();
                        }
                        company.Token_Number = Guid.NewGuid().ToString();
                            company.Date = DateTime.Now;
                        NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
                        String sMacAddress = string.Empty;
                        foreach (NetworkInterface adapter in nics)
                        {
                            IPInterfaceProperties properties = adapter.GetIPProperties();
                            sMacAddress = adapter.GetPhysicalAddress().ToString();
                            if (!string.IsNullOrEmpty(sMacAddress))// only return MAC Address from first card  
                                break;
                        }
                            company.User_Id = User.Identity.Name;
                           company.User_name= db.Employees.Where(z => z.Employee_Id == User.Identity.Name).Select(z => z.Employee_name).FirstOrDefault();
                        company.Mac_id = sMacAddress;
                        db.Products.Add(company);
                        await db.SaveChangesAsync();
                        return Created("", company);
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
                return await Task.FromResult(Ok(db.Products.Where(z => z.Token_Number == id).Distinct().FirstOrDefault()));
            }
        }


        [HttpDelete]
        [Route("{id}")]
        public async Task<IHttpActionResult> Delete([FromUri]string id)
        {
            try {
                using (EasyBillingEntities db = new EasyBillingEntities())
                {
                    Product companyforDeletion = db.Products.Where(z => z.Token_Number == id).Distinct().FirstOrDefault();
                    if (db.Item_Tubes.Where(x => x.Company_token == id).Any() || db.Item_Tyres.Where(x => x.Company_token == id).Any())
                    {
                       int count_tube= db.Item_Tubes.Where(x => x.Company_token == id).Count();
                        int count_tyre = db.Item_Tyres.Where(x => x.Company_token == id).Count();
                        if(count_tube+count_tyre>1)
                        return BadRequest("One or the other records are associated hence unable to delete");
                        else
                            
                            return BadRequest("The record is associated hence unable to delete");
                    }
                    else
                    {
                        if (companyforDeletion != null)
                        {
                            db.Products.Remove(companyforDeletion);
                            await db.SaveChangesAsync();
                            ModelState.Clear();
                            return Ok();
                        }
                        return BadRequest($"Invalid token number {id}");
                    }
                }
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [System.Web.Http.HttpPost]
       
        public async Task<IHttpActionResult> DeleteSelected([FromUri]System.Web.Mvc.FormCollection formCollection)
        {
            try
            {
                using (EasyBillingEntities db = new EasyBillingEntities())
                {
                    string[] ids = formCollection["ID"].Split(new char[] { ',' });
                    foreach (string id in ids)
                    {
                        Product companyforDeletion = db.Products.Where(z => z.Token_Number == id).Distinct().FirstOrDefault();
                        if (db.Item_Tubes.Where(x => x.Company_token == id).Any() || db.Item_Tyres.Where(x => x.Company_token == id).Any())
                        {
                            int count_tube = db.Item_Tubes.Where(x => x.Company_token == id).Count();
                            int count_tyre = db.Item_Tyres.Where(x => x.Company_token == id).Count();
                            if (count_tube + count_tyre > 1)
                                return BadRequest("One or the other records are associated hence unable to delete");
                            else

                                return BadRequest("The record is associated hence unable to delete");
                        }
                        else
                        {
                            if (companyforDeletion != null)
                            {
                                db.Products.Remove(companyforDeletion);
                                await db.SaveChangesAsync();
                                ModelState.Clear();
                                return Ok();
                            }
                            return BadRequest($"Invalid token number {id}");
                        }
                        //var employee = this.db.Employees.Find(int.Parse(id));
                        //this.db.Employees.Remove(employee);
                        //this.db.SaveChanges();
                    }
                    return BadRequest($"Invalid token number");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
