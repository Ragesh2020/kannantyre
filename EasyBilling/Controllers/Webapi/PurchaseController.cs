using CrystalDecisions.CrystalReports.Engine;
using EasyBilling.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Mvc;

namespace EasyBilling.Controllers.Webapi
{
    public class PurchaseController : ApiController
    {

        [System.Web.Http.HttpGet]
        public JsonResult GetPurchaseData(string Token_Number)
        {
            //dynamic json = jsonData;
            string token = Token_Number;
            string data = null;
            using (var ctx = new EasyBillingEntities())
            {
                var existingMarchent = ctx.Products.Select(x => new {  x.Token_Number }).Where(s => s.Token_Number == token).FirstOrDefault();

               
            }
            var result = new JsonResult
            {
                Data = JsonConvert.SerializeObject(data)
            };
            return new JsonResult { Data = data, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public IHttpActionResult GetPurchaselastId()
        {
            PurchaseMasterclass PM = null;
            using (var ctx = new EasyBillingEntities())
            {
                var pmexits = ctx.Purchase_Invoices.Select(s => new PurchaseMasterclass()
                {
                    Purchase_Number = s.Purchase_invoice_number
                }).Any();
                if (pmexits == true)
                {
                    PM = ctx.Purchase_Invoices.Select(s => new PurchaseMasterclass()
                    {
                        Purchase_Number = s.Purchase_invoice_number
                    }).OrderByDescending(o => o.Purchase_Number).FirstOrDefault();
                }
            }
            if (PM == null)
            {
                return NotFound();
            }
            return Ok(PM);
        }

        public IHttpActionResult PostNewMerchant([FromBody]Purchase_Master purchase,[FromUri] string BussinessId)
        {
            bool status = false;
            if (!ModelState.IsValid)
                return BadRequest("Sorry there is some problem. Please check and try again");

            using (EasyBillingEntities dc = new EasyBillingEntities())
            {
                bool prchsnocheck = dc.Purchase_Masters.Where(a => a.Purchase_Number == purchase.Purchase_Number).Any();
                if (prchsnocheck == false)
                {
                    purchase.Token_Number = (Guid.NewGuid()).ToString();
                    purchase.Marchent_Token_number = dc.Marchent_Accounts.Where(x => x.Email_Id == BussinessId).Select(z => z.Token_number).Distinct().FirstOrDefault();
                    var chkst = (from tkn in dc.Marchent_Accounts
                                 join st in dc.States on tkn.State_Code equals st.State_Code
                                 where tkn.Email_Id == BussinessId
                                 select new { st.SGST, st.CGST, st.IGST, st.UTGST }).Distinct().FirstOrDefault();
                    if (chkst.SGST == true && chkst.CGST == true)
                    {
                        purchase.CGST = purchase.Total_tax / 2;
                        purchase.SGST = purchase.CGST;
                    }
                    else if (chkst.CGST == true && chkst.IGST == true)
                    {
                        purchase.CGST = purchase.Total_tax / 2;
                        purchase.IGST = purchase.CGST;
                    }
                    else if (chkst.CGST == true && chkst.UTGST == true)
                    {
                        purchase.CGST = purchase.Total_tax / 2;
                        purchase.UTGST = purchase.CGST;
                    }
                    else if (chkst.SGST == true && chkst.IGST == true)
                    {
                        purchase.SGST = purchase.Total_tax / 2;
                        purchase.IGST = purchase.SGST;
                    }
                    else if (chkst.SGST == true && chkst.UTGST == true)
                    {
                        purchase.SGST = purchase.Total_tax / 2;
                        purchase.UTGST = purchase.SGST;
                    }
                    else if (chkst.IGST == true && chkst.UTGST == true)
                    {
                        purchase.IGST = purchase.Total_tax / 2;
                        purchase.UTGST = purchase.IGST;
                    }
                    else
                    { }

                    var dtls = purchase.Purchase_details.Distinct().ToList();

                    //List<Stock> stocklist = null;
                    foreach (var echdtls in dtls)
                    {
                        echdtls.Date = DateTime.Now.Date;
                       
                        //bool chk = dc.Stocks.Where(a => a.Product_Token == echdtls.Product_Token).Any();
                        //if (chk == true)
                        //{
                        //    Stock stk = dc.Stocks.Where(a => a.Product_Token == echdtls.Product_Token).FirstOrDefault();
                        //    stk.Date = DateTime.Now.Date;
                        //    stk.Pieces = stk.Pieces + echdtls.Pieces;
                        //    stk.Quantity = stk.Quantity + echdtls.Quantity;
                        //    stk.Product_Token = echdtls.Product_Token;
                        //    dc.SaveChanges();

                        //}
                        //else
                        //{

                        //    Stock stk = new Stock();
                        //    stk.Date = DateTime.Now.Date;
                        //    stk.Pieces = echdtls.Pieces;
                        //    stk.Quantity = echdtls.Quantity;
                        //    stk.Product_Token = echdtls.Product_Token;
                        //    dc.Stocks.Add(stk);
                        //    dc.SaveChanges();

                        //}
                    }
                    purchase.Date = DateTime.Now.Date;
                    dc.Purchase_Masters.Add(purchase);
                    dc.SaveChanges();
                    status = true;
                }else
                {
                    status = false;
                    return BadRequest("Duplicate purchase number cannot be saved");
                   
                }
            }

            return Ok();
        }
      

        public IHttpActionResult GetPurchaseMasterList()
        {
            IList<PurchaseMasterclass> pmc = null;
            using (var ctx = new EasyBillingEntities())
            {
                pmc = ctx.Purchase_Masters.Select(s => new PurchaseMasterclass()
                {
                    Token_Number = s.Token_Number,
                    CGST = s.CGST,

                    Dealer_Token_number = s.Dealer_Token_number,
                    Date = s.Date,
                    Discount_percent = s.Discount_percent,
                    Purchase_Number = s.Purchase_Number,

                    IGST = s.IGST,
                    Marchent_Token_number = s.Marchent_Token_number,
                    Narration = s.Narration,
                    Rate_including_tax = s.Rate_including_tax,
                    SGST = s.SGST,
                    Tax_token = s.Tax_token,
                    Total_amount = s.Total_amount,
                    Total_discount = s.Total_discount,
                    Total_tax = s.Total_tax,
                    UTGST = s.UTGST


                }).ToList();



            }
            if (pmc.Count == 0)
            {
                return NotFound();
            }


            return Ok(pmc);

        }
        public IHttpActionResult GetPurchaseDetailsList()
        {

            IList<PurchaseDetailsClass> purchdet = null;

            using (var ctx = new EasyBillingEntities())
            {

                purchdet = ctx.Purchase_details.Select(l => new PurchaseDetailsClass()
                {
                    Pieces = l.Pieces,

                    Product_Token = l.Product_Token,
                    Purcahse_number = l.Purcahse_number,
                    Date = l.Date,

                    Purchase_Token_number = l.Purchase_Token_number,
                    Quantity = l.Quantity,
                    Amount = l.Amount,
                    Discount = l.Discount,
                    Purchase_details_number = l.Purchase_details_number,
                    Discount_percent =l.Discount_percent,
                    Taxable_amount=l.Taxable_amount,
                    Sub_Total = l.Sub_Total,
                    Tax = l.Tax,
                    Tax_Token = l.Tax_Token

                }).Distinct().ToList();

            }

            if (purchdet.Count == 0)
            {
                return NotFound();
            }

            return Ok(purchdet);
        }
        public IHttpActionResult GetPurchaseStockList()
        {

            IList<StockClass> stc = null;

            using (var ctx = new EasyBillingEntities())
            {

                stc = ctx.Stocks.Select(l => new StockClass()
                {
                    Pieces = l.Pieces,

                    Product_Token = l.Product_Token,
                    
                    Date = l.Date,
                    
                    Stock_Id = l.Stock_Id,
                   
                     CGST= l.CGST,
                   SGST=l.SGST

                }).Distinct().ToList();

            }

            if (stc.Count == 0)
            {
                return NotFound();
            }

            return Ok(stc);
        }

        public HttpResponseMessage GetPurchaseListReport(string prdctno)
        {
            List<Product> pp = new List<Product>();
            if (prdctno != null)
            {
                EasyBillingEntities db = new EasyBillingEntities();
                var stks = (from i in db.Purchase_Masters
                            select new
                            {
                                Token_Number = i.Token_Number,
                                Purchase_Number = i.Purchase_Number,
                                Date = i.Date,
                                Dealer_Token_number=i.Dealer_Token_number,
                                Marchent_Token_number=i.Marchent_Token_number
                                

                            }).Where(u => u.Purchase_Number == prdctno).Distinct().FirstOrDefault();
                if (stks != null)
                {
                    var mrchnt = (from i in db.Marchent_Accounts
                                 select new
                                 {
                                     Token_number = i.Token_number,
                                     GSTIN_Number = i.GSTIN_Number,
                                     State_Name = i.State_Name
                                    
                                 }).Where(u => u.Token_number == stks.Marchent_Token_number).Distinct().ToList();

                    var stks1 = (from i in db.Purchase_details
                                 select new
                                 {
                                     Purchase_Token_number = i.Purchase_Token_number,
                                     Purcahse_number = i.Purcahse_number,
                                     Date = i.Date,
                                     Purchase_details_number = i.Purchase_details_number,
                                     Pieces = i.Pieces,
                                     Quantity = i.Quantity,
                                     Amount = i.Amount,
                                     Tax = i.Tax,
                                     Product_Token=i.Product_Token,
                                     Discount= i.Discount,
                                     Sub_Total = i.Sub_Total,
                                    i.Product_name
                                 }).Where(u => u.Purchase_Token_number == stks.Token_Number).Distinct().ToList();

                    foreach(var t in stks1)
                    {

                        Product prdct = db.Products.Where(r => r.Token_Number == t.Product_Token).Distinct().FirstOrDefault();
                        pp.Add(prdct);

                    }

                    var stks2 = (from i in db.Purchase_Masters
                                 select new
                                 {
                                     Token_Number = i.Token_Number,
                                     Purchase_Number = i.Purchase_Number,
                                     Date = i.Date,
                                     Narration=i.Narration,
                                     Total_discount=i.Total_discount,
                                     Total_tax = i.Total_tax,
                                     Total_amount = i.Total_amount,
                                     CGST=i.CGST,
                                     SGST= i.SGST,
                                     IGST=i.IGST,
                                     UTGST=i.UTGST,
                                     i.Rate_including_tax

                                 }).Where(u => u.Token_Number == stks.Token_Number).Distinct().ToList();
                    var dlr = (from i in db.Dealers
                                 select new
                                 {
                                     Address = i.Address,
                                     Name = i.Name,
                                     State_Name = i.State_Name,
                                     Phone_number = i.Phone_number,
                                     Token_number=i.Token_number
                                    

                                 }).Where(u => u.Token_number == stks.Dealer_Token_number).Distinct().ToList();
                    ReportDocument rpt = new ReportDocument();
                    
                    rpt.Load(Path.Combine(HostingEnvironment.MapPath("~/Reportsdataset/AllReports"), "PurchaseReport.rpt"));

                    rpt.Database.Tables[0].SetDataSource(stks2);
                    rpt.Database.Tables[1].SetDataSource(stks1);
                    rpt.Database.Tables[2].SetDataSource(dlr);
                    rpt.Database.Tables[3].SetDataSource(pp);
                    rpt.Database.Tables[4].SetDataSource(mrchnt);
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
            else
            {
                var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
                return response;
            }
        }
    }
}
