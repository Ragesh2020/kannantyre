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
using System.Web.Hosting;
using System.Web.Http;

namespace EasyBilling.Controllers.Webapi
{
    public class DealerController : ApiController
    {
        public IHttpActionResult GetAllDealers(bool includeAddress = false)
        {
            IList<DealerAccount> dealer = null;

            using (var ctx = new EasyBillingEntities())
            {

                dealer = ctx.Dealers.Include("State").Select(s => new DealerAccount()
                {
                    Dealer_code = s.Dealer_code,
                    Name = s.Name,
                    Email = s.Email,
                    Phone_number = s.Phone_number,
                    Token_number = s.Token_number,
                    Address = s.Address,
                    Isactive = s.Isactive,
                    State = s.State,
                    //Statecd = s.State1 == null || includeAddress == false ? null : new StateData()
                    //{
                    //    State_Code = s.State.Value,

                    //},
                    //stcd = (from a in ctx.States
                    //        where a.State_Code == s.State
                    //        select a.Name).Distinct().FirstOrDefault(),
                    State_Name=s.State_Name,
                }).ToList<DealerAccount>();


            }


            if (dealer.Count == 0)
            {
                return NotFound();
            }

            return Ok(dealer);
        }
        public IHttpActionResult PostNewDealer([FromBody]Dealer dealer)
        {
            if (!ModelState.IsValid)
                return BadRequest("Sorry there is some problem. Please check and try again");

            using (var ctx = new EasyBillingEntities())
            {

                var stcode = 29;
                var chkstate = ctx.States.Select(a=>new { a.State_Code,a.Name}).Where(x => x.State_Code == stcode).FirstOrDefault();
                if (chkstate != null)
                {
                    dealer.Isactive = true;
                    dealer.State_Name = chkstate.Name;
                    dealer.State = stcode;
                    dealer.Token_number = (Guid.NewGuid()).ToString();
                    ctx.Dealers.Add(dealer);
                    ctx.SaveChanges();
                }
                else
                {
                    return BadRequest("State does not exists");
                }
            }

            return Ok();
        }

        public IHttpActionResult GetAllDealers(string id)
        {
            bool includeAddress = false;
            DealerAccount dealer = null;

            using (var ctx = new EasyBillingEntities())
            {

                dealer = ctx.Dealers.Include("State").Where(x => x.Token_number == id).Select(s => new DealerAccount()
                {
                    Token_number = s.Token_number,
                    Name = s.Name,
                    Email = s.Email,
                    Phone_number = s.Phone_number,

                    Address = s.Address,
                    Dealer_code = s.Dealer_code,
                    State_Name = s.State_Name,
                    Isactive = s.Isactive,
                    //stcd = (from a in ctx.States
                    //        where a.State_Code == s.State
                    //        select a.Name).Distinct().FirstOrDefault(),
                    //Statecd = s.State == null || includeAddress == false ? null : new StateData()
                    //{
                    //    State1 = s.State1.Name,

                    //},
                    State = s.State,
                }).FirstOrDefault();


            }


            if (dealer == null)
            {
                return NotFound();
            }

            return Ok(dealer);
        }
        public IHttpActionResult PutDealers([FromBody]Dealer dealer)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid data");

            using (var ctx = new EasyBillingEntities())
            {
                var existingdealer = ctx.Dealers.Where(s => s.Token_number == dealer.Token_number).FirstOrDefault();

                if (existingdealer != null)
                {
                    var stcode = 29;
                    var chkstate = ctx.States.Select(a=>new { a.State_Code,a.Name}).Where(x => x.State_Code == stcode).FirstOrDefault();
                    if (chkstate != null)
                    {
                        existingdealer.Name = dealer.Name;
                        existingdealer.Email = dealer.Email;
                        existingdealer.Address = dealer.Address;
                        existingdealer.Phone_number = dealer.Phone_number;
                        existingdealer.Dealer_code = dealer.Dealer_code;
                        existingdealer.State = stcode;
                        existingdealer.State_Name = chkstate.Name;
                        existingdealer.Isactive = true;

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
                var dealer = ctx.Dealers
                    .Where(s => s.Token_number == id)
                    .FirstOrDefault();

                ctx.Entry(dealer).State = EntityState.Deleted;
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
                var existingdealer = ctx.Dealers.Where(s => s.Token_number == token).FirstOrDefault();

                if (existingdealer != null)
                {
                    var chkkactive = (from a in ctx.Dealers
                                      where a.Token_number == token
                                      select a.Isactive).Distinct().FirstOrDefault();
                    if (chkkactive == true)
                    {
                        existingdealer.Isactive = false;
                    }
                    else if (chkkactive == false)
                    {
                        existingdealer.Isactive = true;
                    }
                    else
                    {
                        existingdealer.Isactive = false;
                    }
                    ctx.SaveChanges();
                }
                else
                {
                    return NotFound();
                }
            }

            return Ok();
        }

        public HttpResponseMessage GetDealerListReport()
        {
            EasyBillingEntities db = new EasyBillingEntities();
            object _null = null;
            var mrchnt = _null;

            var dlr = (from i in db.Dealers
                       select new
                       {
                           i.Address,
                           i.Dealer_code,
                           i.Email,
                           i.Name,
                           i.Phone_number,
                           i.State_Name,
                           i.Token_number,
                           i.Marchent_Token_number
                          
                          
                       }).ToList();
            foreach(var dlrsngle in dlr)
            {
                mrchnt = (from i in db.Marchent_Accounts
                       select new
                       {
                           Token_number = i.Token_number,
                           GSTIN_Number = i.GSTIN_Number,
                           State_Name = i.State_Name

                       }).Where(u => u.Token_number == dlrsngle.Marchent_Token_number).Distinct().ToList();

            }
            ReportDocument rpt = new ReportDocument();

            rpt.Load(Path.Combine(HostingEnvironment.MapPath("~/Reportsdataset/AllReports"), "DealerReport.rpt"));
            rpt.Database.Tables[0].SetDataSource(dlr);
            rpt.Database.Tables[1].SetDataSource(mrchnt);
           
           
            Stream s = rpt.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            var response = new HttpResponseMessage(HttpStatusCode.OK);

            response.Content = new StreamContent(s);
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf");

            return response;

        }

        public HttpResponseMessage GetDealerDetailsReport(string dldt)
        {
            EasyBillingEntities db = new EasyBillingEntities();
            List<Product> pp1 = new List<Product>();
            List<Purchase_detail> pd1 = new List<Purchase_detail>();
            List<Marchent_Account> mrchnt = new List<Marchent_Account>();
            if (dldt != null)
            {
                List<Product> pp = new List<Product>();
                

               
                var stks = (from i in db.Purchase_Masters
                            select new
                            {
                                Token_Number = i.Token_Number,
                                Purchase_Number = i.Purchase_Number,
                                Date = i.Date,
                                Dealer_Token_number = i.Dealer_Token_number,
                                Marchent_Token_number = i.Marchent_Token_number


                            }).Where(u => u.Dealer_Token_number == dldt).Distinct().FirstOrDefault();
                if (stks != null)
                {
                    
                    var stks2 = (from i in db.Purchase_Masters
                                 select new
                                 {
                                     Token_Number = i.Token_Number,
                                     Purchase_Number = i.Purchase_Number,
                                     Date = i.Date,
                                     Narration = i.Narration,
                                     Total_discount = i.Total_discount,
                                     Total_tax = i.Total_tax,
                                     Total_amount = i.Total_amount,
                                     CGST = i.CGST,
                                     SGST = i.SGST,
                                     IGST = i.IGST,
                                     UTGST = i.UTGST,
                                     i.Dealer_Token_number

                                 }).Where(u => u.Dealer_Token_number == dldt).Distinct().ToList();
                    List<Purchase_detail> prdtlnw = new List<Purchase_detail>();
                    List<Purchase_detail> prdtl = new List<Purchase_detail>();
                    foreach (var u in stks2)
                    {
                        prdtl = db.Purchase_details.Where(r => r.Purchase_Token_number == u.Token_Number).Distinct().ToList();
                        foreach (var y in prdtl)
                        {
                            prdtlnw.Add(y);

                        }
                       
                        foreach (var t in prdtl)
                        {

                            Product prdct = db.Products.Where(r => r.Token_Number == t.Product_Token).Distinct().FirstOrDefault();

                            pp.Add(prdct);

                        }
                    }
                    pd1 = prdtlnw.Distinct().ToList();
                    pp1 = pp.Distinct().ToList();
                }else
                {
                    pp1 = new List<Product>();
                    pd1 = new List<Purchase_detail>();
                    mrchnt = new List<Marchent_Account>();
                }
               

                var dlr = (from i in db.Dealers
                               select new
                               {
                                   Address = i.Address,
                                   Name = i.Name,
                                   State_Name = i.State_Name,
                                   Phone_number = i.Phone_number,
                                   Token_number = i.Token_number,
                                   Dealer_code = i.Dealer_code,

                                   Email = i.Email,

                                   Isactive = i.Isactive



                               }).Where(u => u.Token_number == dldt).Distinct().ToList();
                    ReportDocument rpt = new ReportDocument();

                    rpt.Load(Path.Combine(HostingEnvironment.MapPath("~/Reportsdataset/AllReports"), "DealerDetailsReport.rpt"));
                rpt.Database.Tables[0].SetDataSource(dlr);
                rpt.Database.Tables[1].SetDataSource(pp1);

                if (stks != null)
                {
                    var mrchnt1 = (from i in db.Marchent_Accounts
                                   select new
                                   {
                                       Token_number = i.Token_number,
                                       i.Marchent_name,
                                       GSTIN_Number = i.GSTIN_Number,
                                       State_Name = i.State_Name

                                   }).Where(u => u.Token_number == stks.Marchent_Token_number).Distinct().ToList();
                    rpt.Database.Tables[2].SetDataSource(mrchnt1);
                }
                else
                {
                    rpt.Database.Tables[2].SetDataSource(mrchnt);
                }

                //rpt.Database.Tables[0].SetDataSource(stks2);
                //rpt.Database.Tables[1].SetDataSource(stks1);
                
                    rpt.Database.Tables[3].SetDataSource(pd1);
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
