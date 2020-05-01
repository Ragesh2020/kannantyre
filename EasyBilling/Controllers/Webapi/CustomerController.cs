using CrystalDecisions.CrystalReports.Engine;
using EasyBilling.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Http;

namespace EasyBilling.Controllers.Webapi
{
    [RoutePrefix("api/Customer")]
    public class CustomerController : ApiController
    {
        [HttpPost]
        [Route("PostSave")]
        public async Task<IHttpActionResult> PostSave([FromBody] Products_For_Sale products_For_Sale)
        {
            try {
                using (EasyBillingEntities db = new EasyBillingEntities())
                {
                    Customer customer = new Customer();
                    var User_Id = User.Identity.Name;
                    var User_name = db.Employees.Where(z => z.Employee_Id == User.Identity.Name).Select(z => z.Employee_name).FirstOrDefault();
                    if (string.IsNullOrEmpty(products_For_Sale.Customer_Name) || string.IsNullOrEmpty(products_For_Sale.Phone_number)
                        || string.IsNullOrEmpty(products_For_Sale.Vehicle_number) || string.IsNullOrEmpty(products_For_Sale.Email)
                        || string.IsNullOrEmpty(products_For_Sale.Address))
                    {
                        return BadRequest("Please fill all details.");
                    }
                    else
                    {
                        bool checkvehcle = db.Customers.Where(z => z.Vehicle_number == products_For_Sale.Vehicle_number).Any();
                        if (!checkvehcle)
                        {
                            //bool checkcustdet = db.Customers.Where(z => z.Phone_number == products_For_Sale.Phone_number && z.Customer_Name == products_For_Sale.Customer_Name).Any();
                            //if (!checkcustdet)
                            //{
                            
                                customer.Token_number = Guid.NewGuid().ToString();
                            customer.Address = products_For_Sale.Address;
                            customer.Customer_Name = products_For_Sale.Customer_Name;
                            customer.Email = products_For_Sale.Email;
                            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
                            String sMacAddress = string.Empty;
                            foreach (NetworkInterface adapter in nics)
                            {
                                IPInterfaceProperties properties = adapter.GetIPProperties();
                                sMacAddress = adapter.GetPhysicalAddress().ToString();
                                if (!string.IsNullOrEmpty(sMacAddress))// only return MAC Address from first card  
                                    break;
                            }

                            customer.Mac_id = sMacAddress;
                            customer.Phone_number = products_For_Sale.Phone_number;

                            customer.User_Id = User_Id;
                            customer.User_name = User_name;
                            customer.Vehicle_number = products_For_Sale.Vehicle_number;
                            var vtype=db.Vehicles.Where(z=>z.Token_number== products_For_Sale.Vehicle_Token).Select(s => new VehicleClass()
                            {
                                Vehicle_type = s.Vehicle_type + " + " + s.Vehicle_make

                            }).Distinct().FirstOrDefault();
                            customer.Vehicle_type = vtype.Vehicle_type;
                            db.Customers.Add(customer);
                            await db.SaveChangesAsync();
                            return Ok();
                            //}
                            //else
                            //    return BadRequest("Custome name and phone number combination already exists. Please try new.");
                        }
                        else
                            return BadRequest("Vehicle number already exists. Please try new.");
                    }
                }
                //}else
                //    {
                //        var snglerrr = string.Empty;
                //        //snglerrr = "-----------Error------------------";
                //        foreach (var errr in ModelState.Values)
                //        {

                //            for (int i = 0; i < errr.Errors.Count; i++)
                //            {
                //                int j = i + 1;
                //                if (string.IsNullOrEmpty(errr.Errors[i].ErrorMessage))
                //                {
                //                    if (string.IsNullOrEmpty(snglerrr))
                //                        snglerrr = "Please select date.";

                //                }
                //                else
                //                {
                //                    if (string.IsNullOrEmpty(snglerrr))
                //                        snglerrr = j + ". " + errr.Errors[i].ErrorMessage;
                //                    else
                //                        snglerrr = snglerrr + "\n" + j + ". " + errr.Errors[i].ErrorMessage;
                //                }
                //                //errors.Add(snglerrr);
                //            }
                //        }

                //        return BadRequest(snglerrr);
                //    }
              
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Route("PostEditCustSave")]
        public async Task<IHttpActionResult> PostEditCustSave([FromBody] Products_For_Sale products_For_Sale)
        {
            try
            {
                using (EasyBillingEntities db = new EasyBillingEntities())
                {
                    
                    var User_Id = User.Identity.Name;
                    var User_name = db.Employees.Where(z => z.Employee_Id == User.Identity.Name).Select(z => z.Employee_name).FirstOrDefault();
                    if (string.IsNullOrEmpty(products_For_Sale.CustToken_number) || string.IsNullOrEmpty(products_For_Sale.Customer_Name) || string.IsNullOrEmpty(products_For_Sale.Phone_number))
                    {
                        return BadRequest("Please fill all details.");
                    }
                    else
                    {
                        Customer customer = db.Customers.Where(z=>z.Token_number== products_For_Sale.CustToken_number).FirstOrDefault();
               
                        customer.Address = products_For_Sale.Address;
                        customer.Customer_Name = products_For_Sale.Customer_Name;
                        customer.Email = products_For_Sale.Email;
                        NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
                        String sMacAddress = string.Empty;
                        foreach (NetworkInterface adapter in nics)
                        {
                            IPInterfaceProperties properties = adapter.GetIPProperties();
                            sMacAddress = adapter.GetPhysicalAddress().ToString();
                            if (!string.IsNullOrEmpty(sMacAddress))// only return MAC Address from first card  
                                break;
                        }

                        customer.Mac_id = sMacAddress;
                        customer.Phone_number = products_For_Sale.Phone_number;

                        customer.User_Id = User_Id;
                        customer.User_name = User_name;
                        customer.Vehicle_number = products_For_Sale.Vehicle_number;
                      
                        await db.SaveChangesAsync();
                        return Ok();
                    }
                }
            

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        public IHttpActionResult GetAllCustomers(bool includeAddress = false)
        {
            IList<CustomerClass> customer = null;

            using (var ctx = new EasyBillingEntities())
            {

                customer = ctx.Customers.Include("State").Select(s => new CustomerClass()
                {
                    Token_number = s.Token_number,
                    Customer_Name = s.Customer_Name,
                    Email = s.Email,
                    Phone_number = s.Phone_number,
                   // Alternate_phone_number = s.Alternate_phone_number,
                    Address = s.Address,
                    //Credit_Card_number = s.Credit_Card_number,
                    //Credit_limit = s.Credit_limit,
                    //Pan_number = s.Pan_number,
                    //Second_Address = s.Second_Address,
                    //IsActive = s.IsActive,

                    //Statecd = s.State1 == null || includeAddress == false ? null : new StateData()
                    //{
                    //    State_Code = s.State.Value,

                    //},
                    //stcd = (from a in ctx.States
                    //        where a.State_Code == s.State
                    //        select a.Name).Distinct().FirstOrDefault(),
                }).ToList<CustomerClass>();


            }


            if (customer.Count == 0)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        public IHttpActionResult PostCustomerLogin([FromBody]Marchent_Account marchent)
        {
            if (!ModelState.IsValid)
                return BadRequest("Sorry there is some problem. Please check and try again");

            using (var ctx = new EasyBillingEntities())
            {
                if (marchent.Email_Id != null && marchent.Verification_code != null)
                {
                    if (marchent.Forcustomer == true)
                    {
                        //bool chck = ctx.Customers.Where(a => a.Email == marchent.Email_Id && a.Password == marchent.Verification_code).Any();
                        //if (chck == true)
                        //{
                        //    var mrchnt = ctx.Customers.Where(a => a.Email == marchent.Email_Id && a.Password == marchent.Verification_code).FirstOrDefault();
                        //    if (mrchnt.IsActive == true)
                        //    {

                           return Ok(marchent);

                        //    }
                        //    else
                        //    {
                        //        return BadRequest();
                        //    }
                        //}
                        //else
                        //{
                        //    return NotFound();
                        //}
                    }else
                    {
                        bool chck = ctx.Users.Where(a => a.Email == marchent.Email_Id && a.Password == marchent.Verification_code).Any();
                        if (chck == true)
                        {
                            var mrchnt = ctx.Users.Where(a => a.Email == marchent.Email_Id && a.Password == marchent.Verification_code).FirstOrDefault();
                            if (mrchnt.IsActive == true)
                            {

                                return Ok(marchent);

                            }
                            else
                            {
                                return BadRequest();
                            }
                        }
                        else
                        {
                            return NotFound();
                        }
                    }
                }
                else
                {
                    return BadRequest("Require fields mandotory.");
                }
            }

            
        }
        public IHttpActionResult GetFilterCustomers([FromUri]string key, bool includeAddress = false)
        {
            IList<CustomerClass> customer = null;

            using (var ctx = new EasyBillingEntities())
            {
                if (key.ToLower() == "false")
                {
                    customer = ctx.Customers.Include("State").Select(s => new CustomerClass()
                    {
                        Token_number = s.Token_number,
                        Customer_Name = s.Customer_Name,
                        Email = s.Email,
                        Phone_number = s.Phone_number,
                     //   Alternate_phone_number = s.Alternate_phone_number,
                        Address = s.Address,
                        //Credit_Card_number = s.Credit_Card_number,
                        //Credit_limit = s.Credit_limit,
                        //Pan_number = s.Pan_number,
                        //Second_Address = s.Second_Address,
                        //IsActive = s.IsActive,

                        //Statecd = s.State1 == null || includeAddress == false ? null : new StateData()
                        //{
                        //    State_Code = s.State.Value,

                        //},
                        //stcd = (from a in ctx.States
                        //        where a.State_Code == s.State
                        //        select a.Name).Distinct().FirstOrDefault(),
                    }).Where(z => z.IsActive == false).Distinct().ToList();

                }
                else if (key.ToLower() == "falsetrue")
                {
                    customer = ctx.Customers.Include("State").Select(s => new CustomerClass()
                    {
                        Token_number = s.Token_number,
                        Customer_Name = s.Customer_Name,
                        Email = s.Email,
                        Phone_number = s.Phone_number,
                        //Alternate_phone_number = s.Alternate_phone_number,
                        Address = s.Address,
                        //Credit_Card_number = s.Credit_Card_number,
                        //Credit_limit = s.Credit_limit,
                        //Pan_number = s.Pan_number,
                        //Second_Address = s.Second_Address,
                        //IsActive = s.IsActive,
                        //New_customer=s.New_customer,
                        //Statecd = s.State1 == null || includeAddress == false ? null : new StateData()
                        //{
                        //    State_Code = s.State.Value,

                        //},
                        //stcd = (from a in ctx.States
                        //        where a.State_Code == s.State
                        //        select a.Name).Distinct().FirstOrDefault(),
                    }).Where(z => z.IsActive == false && z.New_customer==true).Distinct().ToList();

                }
                else
                {

                    customer = ctx.Customers.Include("State").Select(s => new CustomerClass()
                    {
                        Token_number = s.Token_number,
                        Customer_Name = s.Customer_Name,
                        Email = s.Email,
                        Phone_number = s.Phone_number,
                      //  Alternate_phone_number = s.Alternate_phone_number,
                        Address = s.Address,
                        //Credit_Card_number = s.Credit_Card_number,
                        //Credit_limit = s.Credit_limit,
                        //Pan_number = s.Pan_number,
                        //Second_Address = s.Second_Address,
                        //IsActive = s.IsActive,

                        //Statecd = s.State1 == null || includeAddress == false ? null : new StateData()
                        //{
                        //    State_Code = s.State.Value,

                        //},
                        //stcd = (from a in ctx.States
                        //        where a.State_Code == s.State
                        //        select a.Name).Distinct().FirstOrDefault(),
                    }).Distinct().ToList();
                }

            }


            if (customer.Count == 0)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        public bool Chkemail(string email)
        {
            using (EasyBillingEntities db = new EasyBillingEntities())
            {
                bool chkeml = db.Customers.Where(z => z.Email == email).Any();
                if(chkeml==true)
                {
                    return true;
                }else
                {
                    return false;
                }
            }
               
           
        }

        public bool Chkemailforusers(string email)
        {
            using (EasyBillingEntities db = new EasyBillingEntities())
            {
                bool chkeml = db.Users.Where(z => z.Email == email).Any();
                if (chkeml == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }


        }
        public HttpResponseMessage PostNewCustomer([FromBody]Customer customer)
        {
            if (!ModelState.IsValid)
            {
                var response3 = new HttpResponseMessage(HttpStatusCode.BadRequest);
                return response3;
            }
            using (var ctx = new EasyBillingEntities())
            {
                var chkeml = Chkemail(customer.Email);
                if (chkeml == true)
                {
                    var response2 = new HttpResponseMessage(HttpStatusCode.Found);
                    return response2;
                  
                }
                else
                {
//                    if (customer.New_customer == true)
//                    {

//#pragma warning disable CS0472 // The result of the expression is always the same since a value of this type is never equal to 'null'
//                        if (customer.IsActive != true || customer.IsActive == null)
//#pragma warning restore CS0472 // The result of the expression is always the same since a value of this type is never equal to 'null'
//                        {
//                            customer.IsActive = false;
//                        }
//                        //customer.IsActive = true;
//                        customer.Applied_date = DateTime.Now.Date;
//                        customer.Approve_date = DateTime.Parse("15/08/1947");
//                        customer.New_customer = true;

//                        customer.Token_number = (Guid.NewGuid()).ToString();
//                        ctx.Customers.Add(customer);
//                        ctx.SaveChanges();
//                    }
//                    else
//                    {
                        var stcode = 29;
                        var chkstate = ctx.States.Where(x => x.State_Code == stcode).FirstOrDefault();
                        if (chkstate != null)
                        {
#pragma warning disable CS0472 // The result of the expression is always the same since a value of this type is never equal to 'null'
//                            if (customer.IsActive != true || customer.IsActive == null)
//#pragma warning restore CS0472 // The result of the expression is always the same since a value of this type is never equal to 'null'
//                            {
//                                customer.IsActive = false;
//                            }
//                            //customer.IsActive = true;
//                            customer.Applied_date = DateTime.Now.Date;
//                            if (customer.IsActive == true)
//                            {
//                                customer.Approve_date = DateTime.Now.Date;
//                            }else
//                            {
//                                customer.Approve_date = DateTime.Parse("15/08/1947");
//                            }
//                            customer.New_customer = false;
//                            customer.State = stcode;
//                            customer.State_Name = (from a in ctx.States
//                                                   where a.State_Code == stcode
//                                                   select a.Name).Distinct().FirstOrDefault();
                            customer.Token_number = (Guid.NewGuid()).ToString();
                            ctx.Customers.Add(customer);
                            ctx.SaveChanges();
                        }
                        else
                        {
                            var response1 = new HttpResponseMessage(HttpStatusCode.NotFound);
                            return response1;
                        }
                    }
                //}
            }

            var response = new HttpResponseMessage(HttpStatusCode.OK);
            return response;
        }

        public HttpResponseMessage PostNewUser([FromBody]Customer customer)
        {
            if (!ModelState.IsValid)
            {
                var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
                return response;
            }

            using (var ctx = new EasyBillingEntities())
            {
                bool chkeml = Chkemailforusers(customer.Email);
                if (chkeml == true)
                {
                    var response = new HttpResponseMessage(HttpStatusCode.Found);
                    return response;
                }
                else
                {
                    User users = new User();
                    users.Token_number = (Guid.NewGuid()).ToString();
                    users.IsActive = true;
                    users.Name = customer.Customer_Name;
                    users.Email = customer.Email;
                   // users.Password = customer.Password;
                    users.Phone_number = customer.Phone_number;
                    users.Applied_date = DateTime.Now.Date;

                    ctx.Users.Add(users);
                    ctx.SaveChanges();
                }
            }

            var response1 = new HttpResponseMessage(HttpStatusCode.OK);
            return response1;
        }
        public IHttpActionResult GetAllCustomers(string id)
        {
            bool includeAddress = false;
            CustomerClass customer = null;

            using (var ctx = new EasyBillingEntities())
            {

                customer = ctx.Customers.Include("State").Where(x => x.Token_number == id).Select(s => new CustomerClass()
                {
                    Token_number = s.Token_number,
                    Customer_Name = s.Customer_Name,
                    Email = s.Email,
                    Phone_number = s.Phone_number,
                   // Alternate_phone_number = s.Alternate_phone_number,
                    Address = s.Address,
                    //Credit_Card_number = s.Credit_Card_number,
                    //Credit_limit = s.Credit_limit,
                    //Pan_number = s.Pan_number,
                    //Second_Address = s.Second_Address,
                    //IsActive = s.IsActive,
                    //stcd = (from a in ctx.States
                    //        where a.State_Code == s.State
                    //        select a.Name).Distinct().FirstOrDefault(),
                    //Statecd = s.State == null || includeAddress == false ? null : new StateData()
                    //{
                    //    State1 = s.State1.Name,

                    //},
                    //State = s.State,
                }).FirstOrDefault();


            }


            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }
        public IHttpActionResult PutCustomers([FromBody]Customer customer)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid data");

            using (var ctx = new EasyBillingEntities())
            {
                var existingcustomer = ctx.Customers.Where(s => s.Token_number == customer.Token_number).FirstOrDefault();

                if (existingcustomer != null)
                {
                    var stcode = 29;
                    var chkstate = ctx.States.Select(a=> new { a.Name,a.State_Code }).Where(x => x.State_Code == stcode).FirstOrDefault();
                    if (chkstate != null)
                    {
                        existingcustomer.Customer_Name = customer.Customer_Name;
                        existingcustomer.Email = customer.Email;
                        existingcustomer.Phone_number = customer.Phone_number;
                       // existingcustomer.Alternate_phone_number = customer.Alternate_phone_number;
                        existingcustomer.Address = customer.Address;
                        //existingcustomer.Credit_Card_number = customer.Credit_Card_number;
                        //existingcustomer.Credit_limit = customer.Credit_limit;
                        //existingcustomer.Pan_number = customer.Pan_number;
                        //existingcustomer.Second_Address = customer.Second_Address;
                        //existingcustomer.IsActive = true;
                        //existingcustomer.State = stcode;
                        //existingcustomer.State_Name = chkstate.Name;
                        ctx.SaveChanges();
                    }
                    else
                    {
                        return NotFound();
                    }
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
                return BadRequest("Not a valid dealer id");

            using (var ctx = new EasyBillingEntities())
            {
                var customer = ctx.Customers
                    .Where(s => s.Token_number == id)
                    .FirstOrDefault();

                ctx.Entry(customer).State = EntityState.Deleted;
                ctx.SaveChanges();
            }

            return Ok();
        }
        [ActionName("PostActivate")]
        [HttpPost]
        public IHttpActionResult PostActivate(JObject jsonData)
        {
            dynamic json = jsonData;
            string token = json.Token_number;
            using (var ctx = new EasyBillingEntities())
            {
                var existingcustomer = ctx.Customers.Where(s => s.Token_number == token).FirstOrDefault();

                if (existingcustomer != null)
                {
                    //var chkkactive = (from a in ctx.Customers
                    //                  where a.Token_number == token
                    //                  select a.IsActive).Distinct().FirstOrDefault();
                    //if (chkkactive == true)
                    //{
                    //    existingcustomer.IsActive = false;
                    //}
                    //else if (chkkactive == false)
                    //{
                    //    existingcustomer.IsActive = true;
                    //}
                    //else
                    //{
                    //    existingcustomer.IsActive = false;
                    //}
                    //if(existingcustomer.New_customer==true)
                    //{
                    //    existingcustomer.New_customer = false;
                    //}
                    ctx.SaveChanges();
                }

                else
                {
                    return NotFound();
                }
            }

            return Ok();
        }

        public HttpResponseMessage GetCustomerListReport()
        {
            EasyBillingEntities db = new EasyBillingEntities();
           
            var cstmrs = (from i in db.Customers
                        select new
                        {
                            i.Address,
                            i.Customer_Name,
                            i.Email,
                            
                            i.Phone_number,
                          //  i.State_Name,
                            i.Token_number,

                           // i.Marchent_Token_number

                        }).ToList();
            object _null = null;
            var mrc = _null;
            //foreach (var dlrsngle in cstmrs)
            //{
            //    mrc = (from i in db.Marchent_Accounts
            //                  select new
            //                  {
            //                      Token_number = i.Token_number,
            //                      GSTIN_Number = i.GSTIN_Number,
            //                      State_Name = i.State_Name

            //                  }).Where(u => u.Token_number == dlrsngle.Marchent_Token_number).Distinct().ToList();
                
            //}
            ReportDocument rpt = new ReportDocument();

            rpt.Load(Path.Combine(HostingEnvironment.MapPath("~/Reportsdataset/AllReports"), "CustomerReport.rpt"));
            rpt.Database.Tables[0].SetDataSource(cstmrs);
            rpt.Database.Tables[1].SetDataSource(mrc);
            // rpt.Database.Tables[1].SetDataSource(dsCustomers.Tables["Table1"]);

            //rpt.SetDataSource(rpt);

            Stream s = rpt.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            var response = new HttpResponseMessage(HttpStatusCode.OK);

            response.Content = new StreamContent(s);
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf");

            return response;

        }

        public HttpResponseMessage GetCustomerDetailsReport(string csdt)
        {
            EasyBillingEntities db = new EasyBillingEntities();
            List<Products_For_Sale> pp1 = new List<Products_For_Sale>();
            List<Billing_Detail> prdtlnw = new List<Billing_Detail>();
            List<Billing_Detail> prdtl = new List<Billing_Detail>();
            List<Marchent_Account> mrchnt = new List<Marchent_Account>();
            string tokenfrmrchnt = null;
            if (csdt != null)
            {
                List<Products_For_Sale> pp = new List<Products_For_Sale>();



                //var stks = (from i in db.Billing_Masters
                //            select new
                //            {
                //                Token_Number = i.Token_Number,
                //                Billing_Number = i.Billing_Number,
                //                Date = i.Date,
                //                Customer_Token_number = i.Customer_Token_number,
                //                Marchent_Token_number = i.Marchent_Token_number


                //            }).Where(u => u.Customer_Token_number == csdt).Distinct().FirstOrDefault();
                //if (stks != null)
                //{
                //    //mrchnt = db.Marchent_Accounts.Where(u => u.Token_number == stks.Marchent_Token_number).Select(s=>new Marchent_Account { s.Marchent_name,s.GSTIN_Number,s.State_Name }).Distinct().ToList();

                //    tokenfrmrchnt = stks.Marchent_Token_number;
                //    var stks2 = db.Billing_Masters.Select(a=> new { a.Token_Number,a.Customer_Token_number }).Where(u => u.Customer_Token_number == csdt).Distinct().ToList();
                    
                //    foreach (var u in stks2)
                //    {
                //        prdtl = db.Billing_Details.Where(r => r.Billing_Token_number == u.Token_Number).Distinct().ToList();
                //        prdtlnw.AddRange(prdtl);
                //        foreach (var t in prdtl)
                //        {

                //            Products_For_Sale prdct = db.Products_For_Sales.Where(r => r.Token_Number == t.Product_Token).Distinct().FirstOrDefault();

                //            pp.Add(prdct);

                //        }
                //    }
                //    pp1 = pp.Distinct().ToList();
                //}
                //else
                //{
                //    pp1 = new List<Products_For_Sale>();
                //    mrchnt = new List<Marchent_Account>();
                    
                //}

               
                    var dlr = (from i in db.Customers
                               select new
                               {
                                   Address = i.Address,
                                   i.Customer_Name,
                                   //State_Name = i.State_Name,
                                   Phone_number = i.Phone_number,
                                   Token_number = i.Token_number,

                                   //i.Credit_Card_number,
                                   //i.Credit_limit,
                                   //i.Pan_number,
                                   //i.Second_Address,
                                   //i.Alternate_phone_number,
                                   Email = i.Email,

                                   //Isactive = i.IsActive



                               }).Where(u => u.Token_number == csdt).Distinct().ToList();
           
                ReportDocument rpt = new ReportDocument();

                rpt.Load(Path.Combine(HostingEnvironment.MapPath("~/Reportsdataset/AllReports"), "CustomerDetailsReport.rpt"));

                //rpt.Database.Tables[0].SetDataSource(stks2);
                //rpt.Database.Tables[1].SetDataSource(stks1);

                //if (stks != null)
                //{
                //    var mrchnt1 = (from i in db.Marchent_Accounts
                //                   select new
                //                   {
                //                       Token_number = i.Token_number,
                //                       i.Marchent_name,
                //                       GSTIN_Number = i.GSTIN_Number,
                //                       State_Name = i.State_Name

                //                   }).Where(u => u.Token_number == stks.Marchent_Token_number).Distinct().ToList();
                //    rpt.Database.Tables[0].SetDataSource(mrchnt1);
                //}else
                //{
                //    rpt.Database.Tables[0].SetDataSource(mrchnt);
                //}
                
                rpt.Database.Tables[1].SetDataSource(dlr);
                rpt.Database.Tables[2].SetDataSource(prdtlnw);
                rpt.Database.Tables[3].SetDataSource(pp1);

                Stream s = rpt.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                var response = new HttpResponseMessage(HttpStatusCode.OK);

                response.Content = new StreamContent(s);
                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf");

                return response;
            }
            else
            {
                var response = new HttpResponseMessage(HttpStatusCode.NotFound);
                return response;
            }

        }
    }


}
