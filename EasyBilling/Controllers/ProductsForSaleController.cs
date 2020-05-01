using EasyBilling.Models;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using Newtonsoft.Json;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using System.Web.UI;

namespace EasyBilling.Controllers
{
    [Authorize]
    public class ProductsForSaleController : MybaseController
    {

        private EasyBillingEntities db = new EasyBillingEntities();

        
        public async Task<ActionResult> ConvertToBill(string token)
        {
            ViewBag.vehicletype = db.Vehicles.Select(s => new VehicleClass()
            {
                Token_number = s.Token_number,

                Vehicle_type = s.Vehicle_type + " + " + s.Vehicle_make

            }).Distinct().ToList();
            if (db.Temp_Bill.Count() > 0)
            {
                db.Temp_Bill.RemoveRange(db.Temp_Bill.ToList());
                await db.SaveChangesAsync();
            }
            var dcmlempty = decimal.Parse("0.000");
            var products_For_Saleschk = db.Products_For_Sales.Where(z => z.Approve == false).Any();
            if (products_For_Saleschk)
                ViewBag.chk = "1";
            var billno = db.Billing_Masters.OrderByDescending(z => z.Billing_Number).Select(z => z.Billing_Number).FirstOrDefault();

            List<BillingMasterClass> BMlist = new List<BillingMasterClass>();

            BillingMasterClass BMfnal = null;
            using (var ctx = new EasyBillingEntities())
            {
                var pmexits = ctx.Billing_Masters.Select(s => new BillingMasterClass()
                {
                    Billing_Number = s.Billing_Number
                }).Any();
                var pmexitsforquot = ctx.Quotation_Masters.Select(s => new BillingMasterClass()
                {
                    Billing_Number = s.Quotation_Number
                }).Any();
                var pmexitsforord = ctx.Order_Masters.Select(s => new BillingMasterClass()
                {
                    Billing_Number = s.Order_Number
                }).Any();
                if (pmexits == true)
                {
                    BMlist.Add(ctx.Billing_Masters.Select(s => new BillingMasterClass()
                    {
                        Billing_Number = s.Billing_Number
                    }).OrderByDescending(o => o.Billing_Number).FirstOrDefault());

                }
                if (pmexitsforquot == true)
                {
                    BMlist.Add(ctx.Quotation_Masters.Select(s => new BillingMasterClass()
                    {
                        Billing_Number = s.Quotation_Number
                    }).OrderByDescending(o => o.Billing_Number).FirstOrDefault());

                }
                if (pmexitsforord == true)
                {
                    BMlist.Add(ctx.Order_Masters.Select(s => new BillingMasterClass()
                    {
                        Billing_Number = s.Order_Number
                    }).OrderByDescending(o => o.Billing_Number).FirstOrDefault());
                }
                BMfnal = BMlist.OrderByDescending(o => o.Billing_Number).FirstOrDefault();
            }
            //ViewBag.billno = BMfnal;
            if (BMfnal != null)
            {
                var fstfr = BMfnal.Billing_Number.Substring(0, 4);
                var lstfr = BMfnal.Billing_Number.Substring(BMfnal.Billing_Number.Length - 6);
                string newlstversn = (int.Parse(lstfr) + 1000001).ToString();
                string fstfr1 = (newlstversn.Substring(newlstversn.Length - 6)).ToString();
                String totalvrsn = fstfr + fstfr1;
                ViewBag.billno = totalvrsn;
                //using (var client = new HttpClient())
                //{
                //    var purchlastid = client.GetAsync("http://localhost:8087/api/Billing/GetBillinglastId");
                //    purchlastid.Wait();
                //    var lstid = purchlastid.Result;
                //    ViewBag.billno = lstid;
                //if (lstid.IsSuccessStatusCode)
                //{
                //var readTask3 = lstid.Content.ReadAsAsync<Billing_Master>();
                //readTask3.Wait();
                //var lst = readTask3.Result;
                //var text = lst.Billing_Number;
                //var fstfr = text.Substring(0, 4);
                //var lstfr = text.Substring(text.Length - 6);
                //string newlstversn = (int.Parse(lstfr) + 1000001).ToString();
                //string fstfr1 = (newlstversn.Substring(newlstversn.Length - 6)).ToString();
                //String totalvrsn = fstfr + fstfr1;
                //ViewBag.billno = totalvrsn;
            }
            else //web api sent error response 
            {
                ViewBag.billno = "KANP000001";
            }

            //}
            //}
            //    else //web api sent error response 
            //            {
            //        ViewBag.billno = "KANP000001";
            //    }
            return View();
        }
        public ActionResult DeleteEdit()
        {
            List<RetrieveAllClass> retrieveAllClassList = new List<RetrieveAllClass>();
            RetrieveAllClass retrieveAllClass = new RetrieveAllClass();

            var billing_master = db.Billing_Masters.ToList();
            var quotation_master = db.Quotation_Masters.ToList();
            var order_master = db.Order_Masters.ToList();

            foreach (var eachbilling_master in billing_master)
            {
                retrieveAllClass = new RetrieveAllClass();
                retrieveAllClass.billtype = "Purchase";
                retrieveAllClass.Tokennumber = eachbilling_master.Token_Number;
                retrieveAllClass.orderNumber = eachbilling_master.Billing_Number;
                retrieveAllClass.orderDate = eachbilling_master.Date.ToString();
               
                retrieveAllClass.customerName = db.Customers.Where(z => z.Token_number == eachbilling_master.Customer_token_number).Select(x => x.Customer_Name).FirstOrDefault();
                retrieveAllClass.Amount = eachbilling_master.Total_amount.ToString();
                retrieveAllClass.Balance = eachbilling_master.Balance.ToString();
                retrieveAllClassList.Add(retrieveAllClass);
            }
            foreach (var eachquotation_master in quotation_master)
            {
                retrieveAllClass = new RetrieveAllClass();
                retrieveAllClass.billtype = "Quotation";
                retrieveAllClass.Tokennumber = eachquotation_master.Token_Number;
                retrieveAllClass.orderNumber = eachquotation_master.Quotation_Number;
                retrieveAllClass.orderDate = eachquotation_master.Date.ToString();
               
                retrieveAllClass.customerName = db.Customers.Where(z => z.Token_number == eachquotation_master.Customer_token_number).Select(x => x.Customer_Name).FirstOrDefault();
                retrieveAllClass.Amount = eachquotation_master.Total_amount.ToString();
                retrieveAllClass.Balance = "";
                retrieveAllClassList.Add(retrieveAllClass);
            }
            foreach (var eachorder_master in order_master)
            {
                retrieveAllClass = new RetrieveAllClass();
                retrieveAllClass.billtype = "Order";
                retrieveAllClass.Tokennumber = eachorder_master.Token_Number;
                retrieveAllClass.orderNumber = eachorder_master.Order_Number;
                retrieveAllClass.orderDate = eachorder_master.Date.ToString();
                
                retrieveAllClass.customerName = db.Customers.Where(z => z.Token_number == eachorder_master.Customer_token_number).Select(x => x.Customer_Name).FirstOrDefault();
                retrieveAllClass.Amount = eachorder_master.Total_amount.ToString();
                retrieveAllClass.Balance = eachorder_master.Balance.ToString();
                retrieveAllClassList.Add(retrieveAllClass);
            }

            return View(retrieveAllClassList);
        }
        [Authorize]
        [HttpPost]
        public ActionResult DeleteEdit(string srchitems, string Sdate, string Edate)
        {
            List<RetrieveAllClass> retrieveAllClassList = new List<RetrieveAllClass>();
            RetrieveAllClass retrieveAllClass = new RetrieveAllClass();
            var billing_master = new List<Billing_Master>();
            var quotation_master = new List<Quotation_Master>();
            var order_master = new List<Order_Master>();
            if (!string.IsNullOrEmpty(Sdate))
            {
                DateTime SDate = DateTime.Parse(Sdate);
                DateTime EDate = DateTime.Now;
                if (!string.IsNullOrEmpty(Edate))
                {
                    EDate = DateTime.Parse(Edate);
                }
                billing_master = db.Billing_Masters.Where(x => x.Date >= SDate && x.Date <= EDate).ToList();
                quotation_master = db.Quotation_Masters.Where(x => x.Date >= SDate && x.Date <= EDate).ToList();
                order_master = db.Order_Masters.Where(x => x.Date >= SDate && x.Date <= EDate).ToList();
            }
            else
            {
                billing_master = db.Billing_Masters.ToList();
                quotation_master = db.Quotation_Masters.ToList();
                order_master = db.Order_Masters.ToList();
            }


            foreach (var eachbilling_master in billing_master)
            {
                retrieveAllClass = new RetrieveAllClass();
                retrieveAllClass.billtype = "Purchase";
                retrieveAllClass.Tokennumber = eachbilling_master.Token_Number;
                retrieveAllClass.orderNumber = eachbilling_master.Billing_Number;
                retrieveAllClass.orderDate = eachbilling_master.Date.ToString();
                
                retrieveAllClass.customerName = db.Customers.Where(z => z.Token_number == eachbilling_master.Customer_token_number).Select(x => x.Customer_Name).FirstOrDefault();
                retrieveAllClass.Amount = eachbilling_master.Total_amount.ToString();
                retrieveAllClass.Balance = eachbilling_master.Balance.ToString();
                retrieveAllClassList.Add(retrieveAllClass);
            }
            foreach (var eachquotation_master in quotation_master)
            {
                retrieveAllClass = new RetrieveAllClass();
                retrieveAllClass.billtype = "Quotation";
                retrieveAllClass.Tokennumber = eachquotation_master.Token_Number;
                retrieveAllClass.orderNumber = eachquotation_master.Quotation_Number;
                retrieveAllClass.orderDate = eachquotation_master.Date.ToString();
                
                retrieveAllClass.customerName = db.Customers.Where(z => z.Token_number == eachquotation_master.Customer_token_number).Select(x => x.Customer_Name).FirstOrDefault();
                retrieveAllClass.Amount = eachquotation_master.Total_amount.ToString();
                retrieveAllClass.Balance = "";
                retrieveAllClassList.Add(retrieveAllClass);
            }
            foreach (var eachorder_master in order_master)
            {
                retrieveAllClass = new RetrieveAllClass();
                retrieveAllClass.billtype = "Order";
                retrieveAllClass.Tokennumber = eachorder_master.Token_Number;
                retrieveAllClass.orderNumber = eachorder_master.Order_Number;
                retrieveAllClass.orderDate = eachorder_master.Date.ToString();
               
                retrieveAllClass.customerName = db.Customers.Where(z => z.Token_number == eachorder_master.Customer_token_number).Select(x => x.Customer_Name).FirstOrDefault();
                retrieveAllClass.Amount = eachorder_master.Total_amount.ToString();
                retrieveAllClass.Balance = eachorder_master.Balance.ToString();
                retrieveAllClassList.Add(retrieveAllClass);
            }

            bool containsInt = true;
            bool containsAllInt = false;

            if (!string.IsNullOrEmpty(srchitems))
            {
                if (srchitems.ToLower().Contains("."))
                {
                    retrieveAllClassList = retrieveAllClassList.Where(z => z.Amount.Contains(srchitems)).ToList();
                }
                else
                {
                    containsInt = srchitems.Any(char.IsDigit);
                    containsAllInt = srchitems.All(char.IsDigit);
                    if (containsInt == true)
                    {
                        if (containsAllInt == false)
                        {
                            if (retrieveAllClassList.Where(z => z.orderNumber.ToLower().Contains(srchitems)).ToList().Count() > 0)
                            {
                                retrieveAllClassList = retrieveAllClassList.Where(z => z.orderNumber.ToLower().Contains(srchitems)).ToList();
                            }
                           
                        }
                        else
                        {
                            if (retrieveAllClassList.Where(z => z.orderNumber.ToLower().Contains(srchitems)).ToList().Count() > 0)
                            {
                                retrieveAllClassList = retrieveAllClassList.Where(z => z.orderNumber.ToLower().Contains(srchitems)).ToList();
                            }
                            
                            else
                            {
                                retrieveAllClassList = retrieveAllClassList.Where(z => z.Amount.Contains(srchitems)).ToList();
                            }
                        }
                    }
                    else
                    {
                        if (srchitems.ToLower().Contains("purchase") || srchitems.ToLower().Contains("quotation") || srchitems.ToLower().Contains("order")
                            || srchitems.ToLower().Contains("purch") || srchitems.ToLower().Contains("quot") || srchitems.ToLower().Contains("ord"))
                        {
                            retrieveAllClassList = retrieveAllClassList.Where(z => z.billtype.ToLower().Contains(srchitems)).ToList();
                        }
                        else
                        {
                            retrieveAllClassList = retrieveAllClassList.Where(z => z.customerName.ToLower().Contains(srchitems)).ToList();
                        }
                    }
                }
            }

            return View(retrieveAllClassList);
        }
        [Authorize]
        public ActionResult RetrieveResoleConverToBill()
        {
            List<RetrieveAllClass> retrieveAllClassList = new List<RetrieveAllClass>();
            RetrieveAllClass retrieveAllClass = new RetrieveAllClass();
            
            var order_master = db.Order_Masters.ToList();

            foreach (var eachorder_masterr in order_master)
            {
                retrieveAllClass = new RetrieveAllClass();
                retrieveAllClass.billtype = "Resole";
                retrieveAllClass.Tokennumber = eachorder_masterr.Token_Number;
                retrieveAllClass.orderNumber = eachorder_masterr.Order_Number;
                retrieveAllClass.orderDate = eachorder_masterr.Date.ToString();
                retrieveAllClass.customerName = db.Customers.Where(z => z.Token_number == eachorder_masterr.Customer_token_number).Select(x => x.Customer_Name).FirstOrDefault();
                if(eachorder_masterr.Balance==decimal.Zero)
                retrieveAllClass.Status = "Complete";
                else
                    retrieveAllClass.Status = "Pending";
                retrieveAllClass.Amount = eachorder_masterr.Amount_paid.ToString();
                retrieveAllClass.Balance = eachorder_masterr.Balance.ToString();
                retrieveAllClassList.Add(retrieveAllClass);
            }

            return View(retrieveAllClassList);
        }
        [HttpPost]
        public ActionResult RetrieveResoleConverToBill(string srchitems, string Sdate, string Edate)
        {
            List<RetrieveAllClass> retrieveAllClassList = new List<RetrieveAllClass>();
            RetrieveAllClass retrieveAllClass = new RetrieveAllClass();
            var order_master = db.Order_Masters.ToList();
            
                if (!string.IsNullOrEmpty(Sdate))
            {
                DateTime SDate = DateTime.Parse(Sdate);
                DateTime EDate = DateTime.Now;
                if (!string.IsNullOrEmpty(Edate))
                {
                    EDate = DateTime.Parse(Edate);
                }
                order_master = db.Order_Masters.Where(x => x.Date >= SDate && x.Date <= EDate).ToList();
            }
            else
            {
                order_master = db.Order_Masters.ToList();
            }


            foreach (var eachorder_master in order_master)
            {
                retrieveAllClass = new RetrieveAllClass();
                retrieveAllClass.billtype = "Resole";
                retrieveAllClass.Tokennumber = eachorder_master.Token_Number;
                retrieveAllClass.orderNumber = eachorder_master.Order_Number;
                retrieveAllClass.orderDate = eachorder_master.Date.ToString();
                retrieveAllClass.customerName = db.Customers.Where(z => z.Token_number == eachorder_master.Customer_token_number).Select(x => x.Customer_Name).FirstOrDefault();
                if (eachorder_master.Balance == decimal.Zero)
                    retrieveAllClass.Status = "Complete";
                else
                    retrieveAllClass.Status = "Pending";
                retrieveAllClass.Amount = eachorder_master.Amount_paid.ToString();
                retrieveAllClass.Balance = eachorder_master.Balance.ToString();
                retrieveAllClassList.Add(retrieveAllClass);
            }

            bool containsInt = true;
            bool containsAllInt = false;

            if (!string.IsNullOrEmpty(srchitems))
            {
                if (srchitems.ToLower().Contains("."))
                {
                    retrieveAllClassList = retrieveAllClassList.Where(z => z.Amount.Contains(srchitems)).ToList();
                }
                else
                {
                    containsInt = srchitems.Any(char.IsDigit);
                    containsAllInt = srchitems.All(char.IsDigit);
                    if (containsInt == true)
                    {
                        if (containsAllInt == false)
                        {
                            if (retrieveAllClassList.Where(z => z.orderNumber.ToLower().Contains(srchitems)).ToList().Count() > 0)
                            {
                                retrieveAllClassList = retrieveAllClassList.Where(z => z.orderNumber.ToLower().Contains(srchitems)).ToList();
                            }

                        }
                        else
                        {
                            if (retrieveAllClassList.Where(z => z.orderNumber.ToLower().Contains(srchitems)).ToList().Count() > 0)
                            {
                                retrieveAllClassList = retrieveAllClassList.Where(z => z.orderNumber.ToLower().Contains(srchitems)).ToList();
                            }

                            else
                            {
                                retrieveAllClassList = retrieveAllClassList.Where(z => z.Amount.Contains(srchitems)).ToList();
                            }
                        }
                    }
                    else
                    {
                        if (srchitems.ToLower().Contains("resole") || srchitems.ToLower().Contains("res"))
                        {
                            retrieveAllClassList = retrieveAllClassList.Where(z => z.billtype.ToLower().Contains(srchitems)).ToList();
                        }
                        else
                        {
                            retrieveAllClassList = retrieveAllClassList.Where(z => z.customerName.ToLower().Contains(srchitems)).ToList();
                        }
                    }
                }
            }

            return View(retrieveAllClassList);
        }
        [Authorize]
        public ActionResult RetrievePaymentConverToBill()
        {
            List<RetrieveAllClass> retrieveAllClassList = new List<RetrieveAllClass>();
            RetrieveAllClass retrieveAllClass = new RetrieveAllClass();

            var billing_master = db.Billing_Masters.ToList();

            foreach (var eachbilling_master in billing_master)
            {
                retrieveAllClass = new RetrieveAllClass();
                retrieveAllClass.billtype = "Purchase";
                retrieveAllClass.Tokennumber = eachbilling_master.Token_Number;
                retrieveAllClass.orderNumber = eachbilling_master.Billing_Number;
                retrieveAllClass.orderDate = eachbilling_master.Date.ToString();
                retrieveAllClass.customerName = db.Customers.Where(z => z.Token_number == eachbilling_master.Customer_token_number).Select(x => x.Customer_Name).FirstOrDefault();
                retrieveAllClass.Amount = eachbilling_master.Amount_paid.ToString();
                retrieveAllClass.Balance = eachbilling_master.Balance.ToString();
                retrieveAllClassList.Add(retrieveAllClass);
            }
           
            return View(retrieveAllClassList);
        }
        [Authorize]
        [HttpPost]
        public ActionResult RetrievePaymentConverToBill(string srchitems, string Sdate, string Edate)
        {
            List<RetrieveAllClass> retrieveAllClassList = new List<RetrieveAllClass>();
            RetrieveAllClass retrieveAllClass = new RetrieveAllClass();
            var billing_master = new List<Billing_Master>();
            if (!string.IsNullOrEmpty(Sdate))
            {
                DateTime SDate = DateTime.Parse(Sdate);
                DateTime EDate = DateTime.Now;
                if (!string.IsNullOrEmpty(Edate))
                {
                    EDate = DateTime.Parse(Edate);
                }
                billing_master = db.Billing_Masters.Where(x => x.Date >= SDate && x.Date <= EDate).ToList();
            }
            else
            {
                billing_master = db.Billing_Masters.ToList();
            }


            foreach (var eachbilling_master in billing_master)
            {
                retrieveAllClass = new RetrieveAllClass();
                retrieveAllClass.billtype = "Purchase";
                retrieveAllClass.Tokennumber = eachbilling_master.Token_Number;
                retrieveAllClass.orderNumber = eachbilling_master.Billing_Number;
                retrieveAllClass.orderDate = eachbilling_master.Date.ToString();
                retrieveAllClass.customerName = db.Customers.Where(z => z.Token_number == eachbilling_master.Customer_token_number).Select(x => x.Customer_Name).FirstOrDefault();
                retrieveAllClass.Amount = eachbilling_master.Amount_paid.ToString();
                retrieveAllClass.Balance = eachbilling_master.Balance.ToString();
                retrieveAllClassList.Add(retrieveAllClass);
            }
           
            bool containsInt = true;
            bool containsAllInt = false;

            if (!string.IsNullOrEmpty(srchitems))
            {
                if (srchitems.ToLower().Contains("."))
                {
                    retrieveAllClassList = retrieveAllClassList.Where(z => z.Amount.Contains(srchitems)).ToList();
                }
                else
                {
                    containsInt = srchitems.Any(char.IsDigit);
                    containsAllInt = srchitems.All(char.IsDigit);
                    if (containsInt == true)
                    {
                        if (containsAllInt == false)
                        {
                            if (retrieveAllClassList.Where(z => z.orderNumber.ToLower().Contains(srchitems)).ToList().Count() > 0)
                            {
                                retrieveAllClassList = retrieveAllClassList.Where(z => z.orderNumber.ToLower().Contains(srchitems)).ToList();
                            }
                            
                        }
                        else
                        {
                            if (retrieveAllClassList.Where(z => z.orderNumber.ToLower().Contains(srchitems)).ToList().Count() > 0)
                            {
                                retrieveAllClassList = retrieveAllClassList.Where(z => z.orderNumber.ToLower().Contains(srchitems)).ToList();
                            }
                            
                            else
                            {
                                retrieveAllClassList = retrieveAllClassList.Where(z => z.Amount.Contains(srchitems)).ToList();
                            }
                        }
                    }
                    else
                    {
                        if (srchitems.ToLower().Contains("purchase") || srchitems.ToLower().Contains("purch") )
                        {
                            retrieveAllClassList = retrieveAllClassList.Where(z => z.billtype.ToLower().Contains(srchitems)).ToList();
                        }
                        else
                        {
                            retrieveAllClassList = retrieveAllClassList.Where(z => z.customerName.ToLower().Contains(srchitems)).ToList();
                        }
                    }
                }
            }

            return View(retrieveAllClassList);
        }
        [Authorize]
        public ActionResult RetrieveConverToBill()
        {
            List<RetrieveAllClass> retrieveAllClassList = new List<RetrieveAllClass>();
            RetrieveAllClass retrieveAllClass = new RetrieveAllClass();
            
            var quotation_master = db.Quotation_Masters.ToList();

            foreach (var eachquotation_master in quotation_master)
            {
                retrieveAllClass = new RetrieveAllClass();
                retrieveAllClass.billtype = "Quotation";
                retrieveAllClass.Tokennumber = eachquotation_master.Token_Number;
                retrieveAllClass.orderNumber = eachquotation_master.Quotation_Number;
                retrieveAllClass.orderDate = eachquotation_master.Date.ToString();
                retrieveAllClass.customerName = db.Customers.Where(z => z.Token_number == eachquotation_master.Customer_token_number).Select(x => x.Customer_Name).FirstOrDefault();
                retrieveAllClass.Amount = eachquotation_master.Total_amount.ToString();
                retrieveAllClass.AmountOnQuotation = eachquotation_master.Total_amount.ToString();
                retrieveAllClassList.Add(retrieveAllClass);
            }
           
            return View(retrieveAllClassList);
        }
        [Authorize]
        [HttpPost]
        public ActionResult RetrieveConverToBill(string srchitems, string Sdate, string Edate)
        {
            List<RetrieveAllClass> retrieveAllClassList = new List<RetrieveAllClass>();
            RetrieveAllClass retrieveAllClass = new RetrieveAllClass();
            var quotation_master = new List<Quotation_Master>();
            if (!string.IsNullOrEmpty(Sdate))
            {
                DateTime SDate = DateTime.Parse(Sdate);
                DateTime EDate = DateTime.Now;
                if (!string.IsNullOrEmpty(Edate))
                {
                    EDate = DateTime.Parse(Edate);
                }
               
                quotation_master = db.Quotation_Masters.Where(x => x.Date >= SDate && x.Date <= EDate).ToList();
            }
            else
            {
                quotation_master = db.Quotation_Masters.ToList();
            }
            
            foreach (var eachquotation_master in quotation_master)
            {
                retrieveAllClass = new RetrieveAllClass();
                retrieveAllClass.billtype = "Quotation";
                retrieveAllClass.Tokennumber = eachquotation_master.Token_Number;
                retrieveAllClass.orderNumber = eachquotation_master.Quotation_Number;
                retrieveAllClass.orderDate = eachquotation_master.Date.ToString();
                retrieveAllClass.customerName = db.Customers.Where(z => z.Token_number == eachquotation_master.Customer_token_number).Select(x => x.Customer_Name).FirstOrDefault();
                retrieveAllClass.Amount = eachquotation_master.Total_amount.ToString();
                retrieveAllClass.AmountOnQuotation = eachquotation_master.Total_amount.ToString();
                retrieveAllClassList.Add(retrieveAllClass);
            }
          

            bool containsInt = true;
            bool containsAllInt = false;

            if (!string.IsNullOrEmpty(srchitems))
            {
                if (srchitems.ToLower().Contains("."))
                {
                    retrieveAllClassList = retrieveAllClassList.Where(z => z.Amount.Contains(srchitems)).ToList();
                }
                else
                {
                    containsInt = srchitems.Any(char.IsDigit);
                    containsAllInt = srchitems.All(char.IsDigit);
                    if (containsInt == true)
                    {
                        if (containsAllInt == false)
                        {
                            if (retrieveAllClassList.Where(z => z.orderNumber.ToLower().Contains(srchitems)).ToList().Count() > 0)
                            {
                                retrieveAllClassList = retrieveAllClassList.Where(z => z.orderNumber.ToLower().Contains(srchitems)).ToList();
                            }
                           
                        }
                        else
                        {
                            if (retrieveAllClassList.Where(z => z.orderNumber.ToLower().Contains(srchitems)).ToList().Count() > 0)
                            {
                                retrieveAllClassList = retrieveAllClassList.Where(z => z.orderNumber.ToLower().Contains(srchitems)).ToList();
                            }else
                            {
                                retrieveAllClassList = retrieveAllClassList.Where(z => z.Amount.Contains(srchitems)).ToList();
                            }
                        }
                    }
                    else
                    {
                        if (srchitems.ToLower().Contains("quotation") || srchitems.ToLower().Contains("quot"))
                        {
                            retrieveAllClassList = retrieveAllClassList.Where(z => z.billtype.ToLower().Contains(srchitems)).ToList();
                        }
                        else
                        {
                            retrieveAllClassList = retrieveAllClassList.Where(z => z.customerName.ToLower().Contains(srchitems)).ToList();
                        }
                    }
                }
            }

            return View(retrieveAllClassList);
        }
        [Authorize]
        public ActionResult Retrieve()
        {
           List<RetrieveAllClass> retrieveAllClassList = new List<RetrieveAllClass>();
            RetrieveAllClass retrieveAllClass = new RetrieveAllClass();

            var billing_master = db.Billing_Masters.ToList();
            var quotation_master = db.Quotation_Masters.ToList();
            var order_master = db.Order_Masters.ToList();

            foreach (var eachbilling_master in billing_master)
            {
                retrieveAllClass = new RetrieveAllClass();
                retrieveAllClass.billtype = "Purchase";
                retrieveAllClass.Tokennumber = eachbilling_master.Token_Number;
                retrieveAllClass.orderNumber = eachbilling_master.Billing_Number;
                retrieveAllClass.orderDate = eachbilling_master.Date.ToString();
                retrieveAllClass.VehicleNumber = db.Customers.Where(z => z.Token_number == eachbilling_master.Customer_token_number).Select(x => x.Vehicle_number).FirstOrDefault();
                retrieveAllClass.customerName =db.Customers.Where(z=>z.Token_number==eachbilling_master.Customer_token_number).Select(x=>x.Customer_Name).FirstOrDefault();
                retrieveAllClass.Amount = eachbilling_master.Total_amount.ToString();
                retrieveAllClassList.Add(retrieveAllClass);
            }
            foreach (var eachquotation_master in quotation_master)
            {
                retrieveAllClass = new RetrieveAllClass();
                retrieveAllClass.billtype = "Quotation";
                retrieveAllClass.Tokennumber = eachquotation_master.Token_Number;
                retrieveAllClass.orderNumber = eachquotation_master.Quotation_Number;
                retrieveAllClass.orderDate = eachquotation_master.Date.ToString();
                retrieveAllClass.VehicleNumber = db.Customers.Where(z => z.Token_number == eachquotation_master.Customer_token_number).Select(x => x.Vehicle_number).FirstOrDefault();
                retrieveAllClass.customerName = db.Customers.Where(z => z.Token_number == eachquotation_master.Customer_token_number).Select(x => x.Customer_Name).FirstOrDefault();
                retrieveAllClass.Amount = eachquotation_master.Total_amount.ToString();
                retrieveAllClassList.Add(retrieveAllClass);
            }
            foreach (var eachorder_master in order_master)
            {
                retrieveAllClass = new RetrieveAllClass();
                retrieveAllClass.billtype = "Order";
                retrieveAllClass.Tokennumber = eachorder_master.Token_Number;
                retrieveAllClass.orderNumber = eachorder_master.Order_Number;
                retrieveAllClass.orderDate = eachorder_master.Date.ToString();
                retrieveAllClass.VehicleNumber = db.Customers.Where(z => z.Token_number == eachorder_master.Customer_token_number).Select(x => x.Vehicle_number).FirstOrDefault();
                retrieveAllClass.customerName = db.Customers.Where(z => z.Token_number == eachorder_master.Customer_token_number).Select(x => x.Customer_Name).FirstOrDefault();
                retrieveAllClass.Amount = eachorder_master.Total_amount.ToString();
                retrieveAllClassList.Add(retrieveAllClass);
            }

            return View(retrieveAllClassList);
        }
        [Authorize]
        [HttpPost]
        public ActionResult Retrieve(string srchitems,string Sdate, string Edate)
        {
            List<RetrieveAllClass> retrieveAllClassList = new List<RetrieveAllClass>();
            RetrieveAllClass retrieveAllClass = new RetrieveAllClass();
            var billing_master = new List<Billing_Master>();
            var quotation_master = new List<Quotation_Master>();
            var order_master = new List<Order_Master>();
            if (!string.IsNullOrEmpty(Sdate))
            {
                DateTime SDate = DateTime.Parse(Sdate);
                DateTime EDate = DateTime.Now;
                if (!string.IsNullOrEmpty(Edate))
                {
                     EDate = DateTime.Parse(Edate);
                }
                billing_master = db.Billing_Masters.Where(x => x.Date >= SDate && x.Date <= EDate).ToList();
                quotation_master = db.Quotation_Masters.Where(x => x.Date >= SDate && x.Date <= EDate).ToList();
                order_master = db.Order_Masters.Where(x => x.Date >= SDate && x.Date <= EDate).ToList();
            }
            else
            {
                billing_master = db.Billing_Masters.ToList();
                quotation_master = db.Quotation_Masters.ToList();
                order_master = db.Order_Masters.ToList();
            }

            
            foreach (var eachbilling_master in billing_master)
            {
                retrieveAllClass = new RetrieveAllClass();
                retrieveAllClass.billtype = "Purchase";
                retrieveAllClass.Tokennumber = eachbilling_master.Token_Number;
                retrieveAllClass.orderNumber = eachbilling_master.Billing_Number;
                retrieveAllClass.orderDate = eachbilling_master.Date.ToString();
                retrieveAllClass.VehicleNumber = db.Customers.Where(z => z.Token_number == eachbilling_master.Customer_token_number).Select(x => x.Vehicle_number).FirstOrDefault();
                retrieveAllClass.customerName = db.Customers.Where(z => z.Token_number == eachbilling_master.Customer_token_number).Select(x => x.Customer_Name).FirstOrDefault();
                retrieveAllClass.Amount = eachbilling_master.Total_amount.ToString();
                retrieveAllClassList.Add(retrieveAllClass);
            }
            foreach (var eachquotation_master in quotation_master)
            {
                retrieveAllClass = new RetrieveAllClass();
                retrieveAllClass.billtype = "Quotation";
                retrieveAllClass.Tokennumber = eachquotation_master.Token_Number;
                retrieveAllClass.orderNumber = eachquotation_master.Quotation_Number;
                retrieveAllClass.orderDate = eachquotation_master.Date.ToString();
                retrieveAllClass.VehicleNumber = db.Customers.Where(z => z.Token_number == eachquotation_master.Customer_token_number).Select(x => x.Vehicle_number).FirstOrDefault();
                retrieveAllClass.customerName = db.Customers.Where(z => z.Token_number == eachquotation_master.Customer_token_number).Select(x => x.Customer_Name).FirstOrDefault();
                retrieveAllClass.Amount = eachquotation_master.Total_amount.ToString();
                retrieveAllClassList.Add(retrieveAllClass);
            }
            foreach (var eachorder_master in order_master)
            {
                retrieveAllClass = new RetrieveAllClass();
                retrieveAllClass.billtype = "Order";
                retrieveAllClass.Tokennumber = eachorder_master.Token_Number;
                retrieveAllClass.orderNumber = eachorder_master.Order_Number;
                retrieveAllClass.orderDate = eachorder_master.Date.ToString();
                retrieveAllClass.VehicleNumber = db.Customers.Where(z => z.Token_number == eachorder_master.Customer_token_number).Select(x => x.Vehicle_number).FirstOrDefault();
                retrieveAllClass.customerName = db.Customers.Where(z => z.Token_number == eachorder_master.Customer_token_number).Select(x => x.Customer_Name).FirstOrDefault();
                retrieveAllClass.Amount = eachorder_master.Total_amount.ToString();
                retrieveAllClassList.Add(retrieveAllClass);
            }

            bool containsInt = true;
            bool containsAllInt = false;

            if (!string.IsNullOrEmpty(srchitems))
            {
                if (srchitems.ToLower().Contains("."))
                {
                    retrieveAllClassList = retrieveAllClassList.Where(z => z.Amount.Contains(srchitems)).ToList();
                }
                else
                {
                    containsInt = srchitems.Any(char.IsDigit);
                    containsAllInt = srchitems.All(char.IsDigit);
                    if (containsInt == true)
                    {
                        if (containsAllInt == false)
                        {
                            if (retrieveAllClassList.Where(z => z.orderNumber.ToLower().Contains(srchitems)).ToList().Count() > 0)
                            {
                                retrieveAllClassList = retrieveAllClassList.Where(z => z.orderNumber.ToLower().Contains(srchitems)).ToList();
                            }
                            else
                            {
                                retrieveAllClassList = retrieveAllClassList.Where(z => z.VehicleNumber.ToLower().Contains(srchitems)).ToList();
                            }
                        }
                        else
                        {
                            if (retrieveAllClassList.Where(z => z.orderNumber.ToLower().Contains(srchitems)).ToList().Count() > 0)
                            {
                                retrieveAllClassList = retrieveAllClassList.Where(z => z.orderNumber.ToLower().Contains(srchitems)).ToList();
                            }
                            else if (retrieveAllClassList.Where(z => z.VehicleNumber.ToLower().Contains(srchitems)).ToList().Count() > 0)
                            {
                                retrieveAllClassList = retrieveAllClassList.Where(z => z.VehicleNumber.ToLower().Contains(srchitems)).ToList();
                            }
                            else
                            {
                                retrieveAllClassList = retrieveAllClassList.Where(z => z.Amount.Contains(srchitems)).ToList();
                            }
                        }
                    }
                    else
                    {
                        if (srchitems.ToLower().Contains("purchase") || srchitems.ToLower().Contains("quotation") || srchitems.ToLower().Contains("order")
                            || srchitems.ToLower().Contains("purch") || srchitems.ToLower().Contains("quot") || srchitems.ToLower().Contains("ord"))
                        {
                            retrieveAllClassList = retrieveAllClassList.Where(z => z.billtype.ToLower().Contains(srchitems)).ToList();
                        }
                        else
                        {
                            retrieveAllClassList = retrieveAllClassList.Where(z => z.customerName.ToLower().Contains(srchitems)).ToList();
                        }
                    }
                }
            }

            return View(retrieveAllClassList);
        }
        [Authorize]
        public ActionResult Expenses()
        {
            return View();
        }
        [Authorize]
        public ActionResult OtherExpenses()
        {
            ViewBag.prdcts = db.Other_Products.ToList();
            return View();
        }
        [Authorize]
        public ActionResult ProductsExpenses()
        {
            ViewBag.prdcts = db.Other_Products.ToList();
            return View();
        }
        [Authorize]               
        public ActionResult EmpSalExpenses()
        {
            ViewBag.emps = db.Employees.Distinct().ToList();
            return View();
        }
        public JsonResult GetEmpsal(string id)
        {
            DateTime now = DateTime.Now;
            var startDate = new DateTime(now.Year, now.Month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);

            var PreviousDate = startDate.AddMonths(-1);
            var PreviousEndDate = endDate.AddMonths(-1);

            //startDate =DateTime.Parse(startDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
            //endDate = DateTime.Parse(endDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));

            //PreviousDate = DateTime.Parse(PreviousDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
            //PreviousEndDate = DateTime.Parse(PreviousEndDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));

            var zero = Decimal.Zero;
            var empLastPaymentDue = new Employee_salary_expenseClass();
            var emps = new Employee_salary_expenseClass();
            var empdetails= db.Employees.Select(s => new Employee_salary_expenseClass()
            {
                Employee_token_number = s.Token_number,
                Advance_collected = zero - empLastPaymentDue.salary_to_be_paid,
                Monthly_salary = s.Salary,
                Date = s.Date,

            }).Where(z => z.Employee_token_number == id).OrderByDescending(t => t.Date).FirstOrDefault();
            if (db.Employee_salary_expenses.Where(z => z.Date >= startDate && z.Date <= endDate).Any())
            {
                emps = db.Employee_salary_expenses.Select(s => new Employee_salary_expenseClass()
                {
                    Employee_token_number = s.Employee_token_number,
                    Advance_collected = s.Advance_collected,
                    Monthly_salary = s.Monthly_salary,
                    Date = s.Date,

                }).Where(z => z.Employee_token_number == id && z.Date >= startDate && z.Date <= endDate).OrderByDescending(t => t.Date).FirstOrDefault();
                if(emps!=null && emps.Date< empdetails.Date && empdetails.Employee_token_number == emps.Employee_token_number && emps.Monthly_salary!=empdetails.Monthly_salary)
                {
                    emps.salary_to_be_paid = ((emps.Monthly_salary / 30) * empdetails.Date.AddDays(-1).Day - emps.Advance_collected) + ((empdetails.Monthly_salary / 30) * (30-empdetails.Date.AddDays(-1).Day));
                    emps.Monthly_salary = empdetails.Monthly_salary;
                }
            }
            else if (db.Employee_salary_expenses.Where(z => z.Date >= PreviousDate && z.Date <= PreviousEndDate).Any())
            {
                empLastPaymentDue = db.Employee_salary_expenses.Select(s => new Employee_salary_expenseClass()
                {
                    Employee_token_number = s.Employee_token_number,
                    salary_to_be_paid = s.salary_to_be_paid,
                    Monthly_salary = s.Monthly_salary,
                    Date = s.Date,

                }).Where(z => z.Employee_token_number == id && z.Date >= PreviousDate && z.Date <= PreviousEndDate).OrderByDescending(t => t.Date).FirstOrDefault();
                if (empLastPaymentDue.salary_to_be_paid < 0)
                {
                    emps = db.Employee_salary_expenses.Select(s => new Employee_salary_expenseClass()
                    {
                        Employee_token_number = s.Employee_token_number,
                        Advance_collected = s.Advance_collected - empLastPaymentDue.salary_to_be_paid,
                        Monthly_salary = s.Monthly_salary,
                        Date = s.Date,

                    }).Where(z => z.Employee_token_number == id && z.Date >= startDate && z.Date <= endDate).OrderByDescending(t => t.Date).FirstOrDefault();
                }else
                {
                    emps = db.Employee_salary_expenses.Select(s => new Employee_salary_expenseClass()
                    {
                        Employee_token_number = s.Employee_token_number,
                        Advance_collected = s.Advance_collected,
                        Monthly_salary = s.Monthly_salary,
                        Date = s.Date,

                    }).Where(z => z.Employee_token_number == id && z.Date >= startDate && z.Date <= endDate).OrderByDescending(t => t.Date).FirstOrDefault();
                }
                if (emps != null && emps.Date < empdetails.Date && empdetails.Employee_token_number == emps.Employee_token_number && emps.Monthly_salary != empdetails.Monthly_salary)
                {
                    emps.salary_to_be_paid = ((emps.Monthly_salary / 30) * empdetails.Date.AddDays(-1).Day - emps.Advance_collected) + ((empdetails.Monthly_salary / 30) * (30 - empdetails.Date.AddDays(-1).Day));
                    emps.Monthly_salary = empdetails.Monthly_salary;
                }
            }
            else
            {
                emps = db.Employee_salary_expenses.Select(s => new Employee_salary_expenseClass()
                {
                    Employee_token_number = s.Employee_token_number,
                    Advance_collected = s.Advance_collected,
                    Monthly_salary = s.Monthly_salary,
                    Date = s.Date,

                }).Where(z => z.Employee_token_number == id && z.Date >= startDate && z.Date <= endDate).OrderByDescending(t => t.Date).FirstOrDefault();
                if (emps != null && emps.Date < empdetails.Date && empdetails.Employee_token_number == emps.Employee_token_number && emps.Monthly_salary != empdetails.Monthly_salary)
                {
                    emps.salary_to_be_paid = ((emps.Monthly_salary / 30) * empdetails.Date.AddDays(-1).Day - emps.Advance_collected) + ((empdetails.Monthly_salary / 30) * (30 - empdetails.Date.AddDays(-1).Day));
                    emps.Monthly_salary = empdetails.Monthly_salary;
                }
            }
            if (emps == null)
            {
                emps = db.Employees.Select(s => new Employee_salary_expenseClass()
                {
                    Employee_token_number = s.Token_number,
                    Advance_collected = zero- empLastPaymentDue.salary_to_be_paid,
                    Monthly_salary = s.Salary,
                    Date = s.Date,

                }).Where(z => z.Employee_token_number == id).OrderByDescending(t => t.Date).FirstOrDefault();
                if (emps != null && emps.Date < empdetails.Date && empdetails.Employee_token_number == emps.Employee_token_number && emps.Monthly_salary != empdetails.Monthly_salary)
                {
                    emps.salary_to_be_paid = ((emps.Monthly_salary / 30) * empdetails.Date.AddDays(-1).Day - emps.Advance_collected) + ((empdetails.Monthly_salary / 30) * (30 - empdetails.Date.AddDays(-1).Day));
                    emps.Monthly_salary = empdetails.Monthly_salary;
                }
            }
                return Json(emps, JsonRequestBehavior.AllowGet);
        }
   
        public void GetBalancesheetRpt(string SDate, string EDate)
        {
            BalanceSheetClass balanceSheetClass = new BalanceSheetClass();
            List<BalanceSheetClass> balanceSheetClassList = new List<BalanceSheetClass>();
            List<BalanceSheetClassesAll> balanceSheetClassesAll = new List<BalanceSheetClassesAll>();
            BalanceSheetClassesAll balanceSheetClassesAllSengle = new BalanceSheetClassesAll();
            // while (balanceSheetClass.EDate == balanceSheetClass.SDate)
            if (string.IsNullOrEmpty(SDate) && string.IsNullOrEmpty(EDate))
            {
                balanceSheetClass.SDate = DateTime.Now;
                balanceSheetClass.EDate = DateTime.Now;
            }
            else
            {
                balanceSheetClass.SDate = DateTime.Parse(SDate);
                balanceSheetClass.EDate = DateTime.Parse(EDate);
            }
            DateTime _fileStartDate = balanceSheetClass.SDate;
            DateTime _fileEndDate = balanceSheetClass.EDate;

            var _fileNextDate = _fileStartDate.AddDays(1);
            //var crryForwrd = decimal.Zero;
            var nowDate = _fileStartDate;
            while (_fileStartDate <= _fileEndDate)
            {
                var crryForwrd = decimal.Zero;
                balanceSheetClassList = new List<BalanceSheetClass>();
                _fileNextDate = _fileStartDate.AddDays(1);
                //  crryForwrd = decimal.Zero;
                nowDate = _fileStartDate;
                var billingall = db.Billing_Masters.Where(x => x.Date < _fileNextDate && x.Date >= nowDate).Distinct().ToList();
                var billingDetall = db.Billing_Details.Where(x => x.Date < _fileNextDate && x.Date >= nowDate).Distinct().ToList();
                var stockDetall = db.Products_For_Sales.Distinct().ToList();
                var custlist = db.Customers.Distinct().ToList();
                foreach (var eachbilling in billingall)
                {
                    balanceSheetClass = new BalanceSheetClass();
                    foreach (var echbilldet in billingDetall)
                    {
                        if (eachbilling.Billing_Number == echbilldet.Billing_number)
                        {
                            foreach (var eachstocks in stockDetall)
                            {
                                if (echbilldet.Selling_item_token == eachstocks.Token_Number)
                                {

                                    balanceSheetClass.Billing_Number = eachbilling.Billing_Number;
                                    foreach (var eachcust in custlist)
                                    {
                                        if (eachbilling.Customer_token_number == eachcust.Token_number)
                                        {
                                            balanceSheetClass.Billing_Details = eachbilling.Billing_Number + " : " + eachcust.Customer_Name + " : "
                                                + eachstocks.Product_name + " : " + eachcust.Vehicle_type + " : " + eachcust.Vehicle_number + " : " + eachstocks.Tyre_make + " : " + eachstocks.Tyre_feel + " : " +
                                       eachstocks.Tyre_type;
                                            break;
                                        }
                                    }
                                    balanceSheetClass.Billing_Total = eachbilling.Total_amount;
                                    balanceSheetClass.Billing_Date = eachbilling.Date.ToString("dd/MM/yyyy hh:mm tt");
                                    balanceSheetClass.Billing_Amount_paid = eachbilling.Amount_paid;
                                    balanceSheetClass.Billing_Balance = eachbilling.Balance;
                                    balanceSheetClass.Billing_Total_tax = echbilldet.Pieces * eachstocks.Selling_Price * ((eachstocks.Selling_CGST + eachstocks.Selling_SGST) / 100);
                                    balanceSheetClass.SDate = _fileStartDate;
                                    balanceSheetClass.EDate = _fileEndDate;
                                    balanceSheetClass.Bill_BillBy = eachbilling.User_name;
                                    balanceSheetClassList.Add(balanceSheetClass);
                                    break;
                                }
                            }

                        }
                    }
                }
                var previousDateTime = nowDate.AddDays(-1);
                var Balance_calculation_data_tables = db.Balance_calculation_data_tables.Where(x => x.Date < nowDate).Distinct().ToList();

                foreach (var eachBalance_calculation_data_tables in Balance_calculation_data_tables)
                {
                    if (!string.IsNullOrEmpty(eachBalance_calculation_data_tables.Billing_Amount_paid.ToString()))
                    {
                        crryForwrd = crryForwrd + eachBalance_calculation_data_tables.Billing_Amount_paid;
                    }
                    if (!string.IsNullOrEmpty(eachBalance_calculation_data_tables.Order_Amount_paid.ToString()))
                    {
                        crryForwrd = crryForwrd + eachBalance_calculation_data_tables.Order_Amount_paid;
                    }
                    if (!string.IsNullOrEmpty(eachBalance_calculation_data_tables.Employee_salary_expense_salary_to_be_paid.ToString()))
                    {
                        crryForwrd = crryForwrd - eachBalance_calculation_data_tables.Employee_salary_expense_salary_to_be_paid;
                    }
                    if (!string.IsNullOrEmpty(eachBalance_calculation_data_tables.Other_expense_amount.ToString()))
                    {
                        crryForwrd = crryForwrd - eachBalance_calculation_data_tables.Other_expense_amount;
                    }
                    if (!string.IsNullOrEmpty(eachBalance_calculation_data_tables.Product_expense_Amount_for_expense.ToString()))
                    {
                        crryForwrd = crryForwrd - eachBalance_calculation_data_tables.Product_expense_Amount_for_expense;
                    }
                }

                var purchaseall = db.Purchase_Masters.Where(x => x.Date < _fileNextDate && x.Date >= nowDate).Distinct().ToList();
                foreach (var eachpurchase in purchaseall)
                {
                    balanceSheetClass = new BalanceSheetClass();
                    balanceSheetClass.Purchase_Number = eachpurchase.Purchase_Number;
                    balanceSheetClass.Purchase_Date = eachpurchase.Date.ToString("dd/MM/yyyy hh:mm tt");
                    balanceSheetClass.Purchase_CGST = eachpurchase.CGST;
                    balanceSheetClass.Purchase_SGST = eachpurchase.SGST;
                    balanceSheetClass.Purchase_Total_amount = eachpurchase.Total_amount;
                    balanceSheetClass.SDate = _fileStartDate;
                    balanceSheetClass.EDate = _fileEndDate;
                    balanceSheetClassList.Add(balanceSheetClass);
                }

                var empsalExpeseall = db.Employee_salary_expenses.Where(x => x.Date < _fileNextDate && x.Date >= nowDate).Distinct().ToList();
                foreach (var empsalExpese in empsalExpeseall)
                {
                    balanceSheetClass = new BalanceSheetClass();
                    balanceSheetClass.ExpenseId = empsalExpese.ExpenseId;
                    balanceSheetClass.ExpenseTotal = empsalExpese.salary_to_be_paid;
                   // balanceSheetClass.ExenseDate = empsalExpese.Date.ToString("dd/MM/yyyy hh:mm tt");
                    balanceSheetClass.ExenseDate = empsalExpese.Date.ToString();
                    balanceSheetClass.ExpenseTotalGST = 0;
                    balanceSheetClass.ExpenseBalance = 0;
                    balanceSheetClass.Expense_Details = empsalExpese.ExpenseId + " : " + "Employee salary expense" + " : " + empsalExpese.Employee_name;
                    balanceSheetClass.SDate = _fileStartDate;
                    balanceSheetClass.EDate = _fileEndDate;
                    balanceSheetClass.Expense_BillBy = empsalExpese.User_name;
                    balanceSheetClassList.Add(balanceSheetClass);
                }
                var otherexpenesall = db.Other_expenses.Where(x => x.Date < _fileNextDate && x.Date >= nowDate).Distinct().ToList();
                foreach (var otherexpense in otherexpenesall)
                {
                    balanceSheetClass = new BalanceSheetClass();
                    balanceSheetClass.ExpenseId = otherexpense.ExpenseId;
                    balanceSheetClass.Expense_Details = otherexpense.ExpenseId + " : " + "Other expense" + " : " + otherexpense.Other_expense_type;
                    balanceSheetClass.ExenseDate =otherexpense.Date.ToString();
                    balanceSheetClass.ExpenseTotal = 0;
                    balanceSheetClass.ExpenseBalance = 0;
                    balanceSheetClass.ExpenseTotal = otherexpense.Other_expense_amount;
                    balanceSheetClass.SDate = _fileStartDate;
                    balanceSheetClass.EDate = _fileEndDate;
                    balanceSheetClass.Expense_BillBy = otherexpense.User_name;
                    balanceSheetClassList.Add(balanceSheetClass);
                }
                var productexpenseall = db.Product_expenses.Where(x => x.Date < _fileNextDate && x.Date >= nowDate).Distinct().ToList();
                foreach (var productexpense in productexpenseall)
                {
                    balanceSheetClass = new BalanceSheetClass();
                    balanceSheetClass.ExpenseId = productexpense.ExpenseId;
                    balanceSheetClass.ExpenseTotal = productexpense.Amount_for_expense;
                    balanceSheetClass.ExenseDate = productexpense.Date.ToString();
                    balanceSheetClass.ExpenseTotalGST = 0;
                    balanceSheetClass.ExpenseBalance = 0;
                    balanceSheetClass.Expense_Details = productexpense.ExpenseId + " : " + "Product expense" + " : " + productexpense.Product_name;
                    balanceSheetClass.SDate = _fileStartDate;
                    balanceSheetClass.EDate = _fileEndDate;
                    balanceSheetClass.Expense_BillBy = productexpense.User_name;
                    balanceSheetClassList.Add(balanceSheetClass);
                }


                //////////////////////////////////////////////////////////////
                //var purchaseallPreviousDay = db.Purchase_Masters.Where(x => x.Date < nowDate && x.Date >= previousDateTime).Distinct().ToList();
                //foreach (var eachpurchase in purchaseallPreviousDay)
                //{
                //    crryForwrd = crryForwrd - eachpurchase.Total_amount;
                //}

                //var empsalExpeseallPreviousDay = db.Employee_salary_expenses.Where(x => x.Date < nowDate && x.Date >= previousDateTime).Distinct().ToList();
                //foreach (var empsalExpese in empsalExpeseallPreviousDay)
                //{
                //    crryForwrd = crryForwrd - empsalExpese.salary_to_be_paid;
                //}
                //var otherexpenesallPreviousDay = db.Other_expenses.Where(x => x.Date < nowDate && x.Date >= previousDateTime).Distinct().ToList();
                //foreach (var otherexpense in otherexpenesallPreviousDay)
                //{
                //    crryForwrd = crryForwrd - otherexpense.Other_expense_amount;
                //}
                //var productexpenseallPreviousDay = db.Product_expenses.Where(x => x.Date < nowDate && x.Date >= previousDateTime).Distinct().ToList();
                //foreach (var productexpense in productexpenseallPreviousDay)
                //{
                //    crryForwrd = crryForwrd - productexpense.Amount_for_expense;
                //}

                ///////////////////////////////////////////////////////////
                var orderall = db.Order_Masters.Where(x => x.Date < _fileNextDate && x.Date >= nowDate).Distinct().ToList();
                var echorderdetall = db.Order_Details.Where(x => x.Date < _fileNextDate && x.Date >= nowDate).Distinct().ToList();
                foreach (var eachorder in orderall)
                {
                    balanceSheetClass = new BalanceSheetClass();
                    foreach (var echorderdet in echorderdetall)
                    {
                        if (eachorder.Order_Number == echorderdet.Order_number)
                        {
                            foreach (var eachstocks in stockDetall)
                            {
                                if (echorderdet.Selling_item_token == eachstocks.Token_Number)
                                {
                                    foreach (var eachcust in custlist)
                                    {
                                        if (eachorder.Customer_token_number == eachcust.Token_number)
                                        {
                                            balanceSheetClass.Order_Details = eachorder.Order_Number + " : " + eachcust.Customer_Name + " : "
                                                + eachstocks.Product_name + " : " + eachcust.Vehicle_type + " : " + eachcust.Vehicle_number + " : " + eachstocks.Tyre_make + " : " + eachstocks.Tyre_feel + " : " +
                                       eachstocks.Tyre_type;
                                            break;
                                        }
                                    }
                                    balanceSheetClass.Order_Amount_paid = eachorder.Amount_paid;
                                    balanceSheetClass.Order_Balance = eachorder.Balance;
                                    balanceSheetClass.Order_Date = eachorder.Date.ToString();
                                    balanceSheetClass.Order_Number = eachorder.Order_Number;
                                    balanceSheetClass.Order_Total = eachorder.Total_amount;
                                    balanceSheetClass.Order_Total_tax = echorderdet.Pieces * eachstocks.Selling_Price * ((eachstocks.Selling_CGST + eachstocks.Selling_SGST) / 100);
                                    balanceSheetClass.SDate = _fileStartDate;
                                    balanceSheetClass.EDate = _fileEndDate;
                                    balanceSheetClass.Order_BillBy = eachorder.User_name;
                                    balanceSheetClassList.Add(balanceSheetClass);
                                    break;
                                }
                            }

                        }
                    }
                }

                //var orderallPreviousDay = db.Order_Masters.Where(x => x.Date < nowDate && x.Date >= previousDateTime).Distinct().ToList();
                //var echorderdetallPreviousDay = db.Order_Details.Where(x => x.Date < nowDate && x.Date >= previousDateTime).Distinct().ToList();
                //foreach (var eachorder in orderallPreviousDay)
                //{
                //    foreach (var echorderdet in echorderdetallPreviousDay)
                //    {
                //        if (eachorder.Order_Number == echorderdet.Order_number)
                //        {
                //            foreach (var eachstocks in stockDetall)
                //            {
                //                if (echorderdet.Selling_item_token == eachstocks.Token_Number)
                //                {
                //                    crryForwrd = crryForwrd + eachorder.Amount_paid;
                //                    break;
                //                }
                //            }

                //        }
                //    }
                //}
                //if (crryForwrd > decimal.Zero)
                //{
                balanceSheetClass = new BalanceSheetClass();
                balanceSheetClass.SDate = _fileStartDate;
                balanceSheetClass.EDate = _fileEndDate;
                balanceSheetClass.CarryForwardDate = previousDateTime.ToString();
                balanceSheetClass.CarryForward = crryForwrd;
                balanceSheetClassList.Add(balanceSheetClass);
                //}

                balanceSheetClass = new BalanceSheetClass();
                balanceSheetClass.TotalincomeCGST = 0;
                balanceSheetClass.TotalincomeSGST = 0;
                balanceSheetClass.TotalExpensesCGST = 0;
                balanceSheetClass.TotalExpensesSGST = 0;
                balanceSheetClass.TotalIncmBalance = 0;
                balanceSheetClass.TotalExpBalance = 0;
                foreach (var echbalsheet in balanceSheetClassList)
                {
                    balanceSheetClass.TotalincomeCGST = balanceSheetClass.TotalincomeCGST + (echbalsheet.Billing_Total_tax / 2) + (echbalsheet.Order_Total_tax / 2);
                    balanceSheetClass.TotalincomeSGST = balanceSheetClass.TotalincomeSGST + (echbalsheet.Billing_Total_tax / 2) + (echbalsheet.Order_Total_tax / 2);
                    balanceSheetClass.TotalExpensesCGST = balanceSheetClass.TotalExpensesCGST + echbalsheet.Purchase_CGST;
                    balanceSheetClass.TotalExpensesSGST = balanceSheetClass.TotalExpensesSGST + echbalsheet.Purchase_SGST;
                    balanceSheetClass.TotalIncmBalance = balanceSheetClass.TotalIncmBalance + echbalsheet.Billing_Amount_paid + echbalsheet.Order_Amount_paid;
                    balanceSheetClass.TotalExpBalance = balanceSheetClass.TotalExpBalance + echbalsheet.ExpenseTotal;
                }
                if (balanceSheetClassList.Count() > 0)
                {
                    // if(balanceSheetClass.TotalIncmBalance!=0)
                    balanceSheetClass.TotalBalance = balanceSheetClass.TotalIncmBalance - balanceSheetClass.TotalExpBalance + crryForwrd;

                    balanceSheetClassList.Add(balanceSheetClass);


                    balanceSheetClassesAllSengle = new BalanceSheetClassesAll();
                    balanceSheetClassesAllSengle.BalanceSheetClassDateWise = new List<BalanceSheetClass>();
                    balanceSheetClassesAllSengle.Date = _fileStartDate;
                    balanceSheetClassesAllSengle.CarryForward = crryForwrd;
                    balanceSheetClassesAllSengle.CarryForwardDate = nowDate.ToString();
                    balanceSheetClassesAllSengle.TotalBalance = balanceSheetClass.TotalIncmBalance - balanceSheetClass.TotalExpBalance + crryForwrd;
                    balanceSheetClassesAllSengle.BalanceSheetClassDateWise.AddRange(balanceSheetClassList);
                    balanceSheetClassesAll.Add(balanceSheetClassesAllSengle);
                }
                _fileStartDate = _fileStartDate.AddDays(1);
            }
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[5] {
                            new DataColumn("Date time",typeof(string)),
                             new DataColumn("BillBy",typeof(string)),
                            new DataColumn("Details", typeof(string)),
                            new DataColumn("Income(Rs.)", typeof(string)),
                            new DataColumn("Expenses(Rs.)", typeof(string)),
                           });
            bool flag = false;
            if (balanceSheetClassesAll.Count > 0)
            {
                foreach (var singleitem in balanceSheetClassesAll)
                {
                    flag = false;
                    dt.Rows.Add(singleitem.CarryForwardDate,"", "Opening balance", singleitem.CarryForward, "");
                    foreach (var item in singleitem.BalanceSheetClassDateWise)
                    {
                        //dt.Rows.Add(singleitem.CarryForwardDate, "Carry forward", singleitem.CarryForward, "");
                        if(item.Order_Amount_paid.ToString() != "0")
                        {
                            if (item.Order_Amount_paid.ToString() != "0.00")
                            {
                                flag = true;
                                dt.Rows.Add(item.Order_Date,item.Order_BillBy, item.Order_Details, item.Order_Amount_paid, "");
                            }
                        }
                        else if (item.Billing_Amount_paid.ToString() != "0")
                        {
                            flag = true;
                            dt.Rows.Add(item.Billing_Date, item.Bill_BillBy, item.Billing_Details, item.Billing_Amount_paid, "");
                        }
                        else if (item.Purchase_Total_amount.ToString() != "0")
                        {
                            flag = true;
                            dt.Rows.Add(item.Purchase_Date,"", item.Purchase_Number, "",item.Purchase_Total_amount);
                          
                        }
                        else if (item.ExpenseTotal.ToString() != "0")
                        {
                            flag = true;
                            dt.Rows.Add(item.ExenseDate, item.Expense_BillBy, item.Expense_Details, "",item.ExpenseTotal);
                        }
                    }
                    int ttlcnt = 0;

                    foreach (var item in singleitem.BalanceSheetClassDateWise)
                    {
                        ttlcnt++;
                        if (ttlcnt == singleitem.BalanceSheetClassDateWise.Count())
                        {
                            if (item.TotalincomeCGST.ToString() != "0")
                            {
                                dt.Rows.Add("","", "Total income CGST", item.TotalincomeCGST,"");
                            
                            }
                            if (item.TotalincomeSGST.ToString() != "0")
                            {
                                dt.Rows.Add("", "","Total income SGST", item.TotalincomeSGST, "");
                            }
                            if (@item.TotalIncmBalance.ToString() != "0" || @item.TotalExpBalance.ToString() != "0")
                            {
                                if (item.TotalIncmBalance.ToString() != "0")
                                {
                                    if (@item.TotalExpBalance.ToString() != "0")
                                        dt.Rows.Add("", "","Total", item.TotalIncmBalance, item.TotalExpBalance);
                                    else
                                        dt.Rows.Add("","", "Total", item.TotalIncmBalance, "");
                                }
                                else
                                {
                                    if (@item.TotalExpBalance.ToString() != "0")
                                        dt.Rows.Add("", "","Total", "", item.TotalExpBalance);
                                    else
                                        dt.Rows.Add("","", "Total", "", "");
                                }
                        
                            }
                            if (flag == true)
                            {
                                dt.Rows.Add("","", "Balance(with added previous day opening balance amount)", singleitem.TotalBalance, "");
                           
                            }
                        }
                    }
                }

                using (StringWriter sw = new StringWriter())
                {
                    using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                    {
                        StringBuilder sb = new StringBuilder();

                        //Generate Invoice (Bill) Items Grid.
                        if (string.IsNullOrEmpty(SDate) && string.IsNullOrEmpty(EDate))
                        {
                            sb.Append($"Selected date {DateTime.Now.ToString("yyyy-MM-dd")} to {DateTime.Now.ToString("yyyy-MM-dd")} <br/><br/>");
                        }
                        else
                        {
                            sb.Append($"Selected date {SDate} to {EDate} <br/><br/>");
                        }
                       
                        sb.Append("<table border = '1'>");
                        sb.Append("<tr style='font-size:10px'>");
                        foreach (DataColumn column in dt.Columns)
                        {
                            if (column.ColumnName == "Details")
                                sb.Append("<th width='40%'>");
                            else
                                sb.Append("<th width='10%'>");
                            sb.Append(column.ColumnName);
                            sb.Append("</th>");
                        }
                        sb.Append("</tr>");
                        foreach (DataRow row in dt.Rows)
                        {
                            sb.Append("<tr style='font-size:7px'>");
                            foreach (DataColumn column in dt.Columns)
                            {
                                if (column.ColumnName == "Details")
                                    sb.Append("<td width='40%'>");
                                else
                                    sb.Append("<td width='10%'>");
                               // sb.Append("<td>");
                                sb.Append(row[column]);
                                sb.Append("</td>");
                            }
                            sb.Append("</tr>");
                        }
                        sb.Append("</table>");

                        //Export HTML String as PDF.
                        StringReader sr = new StringReader(sb.ToString());
                        Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
                        HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                        PdfWriter writer = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                        pdfDoc.Open();
                        htmlparser.Parse(sr);
                        pdfDoc.Close();
                        Response.ContentType = "application/pdf";
                        Response.AddHeader("content-disposition", "attachment;filename=BalanceSheet.pdf");
                        Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        Response.Write(pdfDoc);
                        Response.End();
                    }
                }
               
            }
            else
            {
                var response = new HttpResponseMessage(HttpStatusCode.NotFound);
                 //return View();
            }

        }
        string[] formats = { "dd-MM-yyyy,MM-dd-yyyy,MM-dd-yyyy hh:mm tt,dd-MM-yyyy hh:mm tt,dd/MM/yyyy,MM/dd/yyyy,MM/dd/yyyy hh:mm tt,dd/MM/yyyy hh:mm tt" };
        DateTime tradedate = new DateTime();

        public ActionResult BalanceSheet()
        {
            Balance_calculation_data_table balance_Calculation_Data_Table = new Balance_calculation_data_table();
            List<Balance_calculation_data_table> balance_Calculation_Data_TableList = new List<Balance_calculation_data_table>();
            try
            {
                db.Balance_calculation_data_tables.RemoveRange(db.Balance_calculation_data_tables.ToList());
                db.SaveChanges();

                var Billing_Masters = db.Billing_Masters.Distinct().ToList();
                var Order_Masters = db.Order_Masters.Distinct().ToList();
                var Employee_salary_expenses = db.Employee_salary_expenses.Distinct().ToList();
                var Other_expenses = db.Other_expenses.Distinct().ToList();
                var Product_expenses = db.Product_expenses.Distinct().ToList();

                foreach (var eachBilling_Masters in Billing_Masters)
                {
                    balance_Calculation_Data_Table = new Balance_calculation_data_table();
                    balance_Calculation_Data_Table.Token_Number = Guid.NewGuid().ToString();
                    balance_Calculation_Data_Table.Date = eachBilling_Masters.Date;
                    balance_Calculation_Data_Table.Billing_Token_Number = eachBilling_Masters.Token_Number;
                    balance_Calculation_Data_Table.Billing_Number = eachBilling_Masters.Billing_Number;
                    balance_Calculation_Data_Table.Bill_Date = eachBilling_Masters.Date;
                    balance_Calculation_Data_Table.Billing_Total_amount = eachBilling_Masters.Total_amount;
                    balance_Calculation_Data_Table.Billing_Amount_paid = eachBilling_Masters.Amount_paid;
                    balance_Calculation_Data_Table.Billing_Balance = eachBilling_Masters.Balance;
                    balance_Calculation_Data_Table.BillBy = eachBilling_Masters.User_name;

                    balance_Calculation_Data_Table.Billing_Total_amount = balance_Calculation_Data_Table.Billing_Total_amount;
                    balance_Calculation_Data_Table.Billing_Amount_paid = balance_Calculation_Data_Table.Billing_Amount_paid;
                    balance_Calculation_Data_Table.Billing_Balance = balance_Calculation_Data_Table.Billing_Balance;

                    balance_Calculation_Data_Table.Order_Date = eachBilling_Masters.Date;
                    balance_Calculation_Data_Table.Order_Total_amount = decimal.Zero;
                    balance_Calculation_Data_Table.Order_Amount_paid = decimal.Zero;
                    balance_Calculation_Data_Table.Order_Balance = decimal.Zero;

                    balance_Calculation_Data_Table.Employee_salary_expense_Date = eachBilling_Masters.Date;
                    balance_Calculation_Data_Table.Employee_salary_expense_salary_to_be_paid = decimal.Zero;

                    balance_Calculation_Data_Table.Other_expense_Date = eachBilling_Masters.Date;
                    balance_Calculation_Data_Table.Other_expense_amount = decimal.Zero;

                    balance_Calculation_Data_Table.Product_expense_Date = eachBilling_Masters.Date;
                    balance_Calculation_Data_Table.Product_expense_Amount_for_expense = decimal.Zero;

                    balance_Calculation_Data_TableList.Add(balance_Calculation_Data_Table);
                }

                foreach (var eachOrder_Masters in Order_Masters)
                {
                    balance_Calculation_Data_Table = new Balance_calculation_data_table();
                    balance_Calculation_Data_Table.Token_Number = Guid.NewGuid().ToString();
                    balance_Calculation_Data_Table.Date = eachOrder_Masters.Date;
                    balance_Calculation_Data_Table.Order_Token_Number = eachOrder_Masters.Token_Number;
                    balance_Calculation_Data_Table.Order_Number = eachOrder_Masters.Order_Number;
                    balance_Calculation_Data_Table.Order_Date = eachOrder_Masters.Date;
                    balance_Calculation_Data_Table.Order_Total_amount = eachOrder_Masters.Total_amount;
                    balance_Calculation_Data_Table.Order_Amount_paid = eachOrder_Masters.Amount_paid;
                    balance_Calculation_Data_Table.Order_Balance = eachOrder_Masters.Balance;
                    balance_Calculation_Data_Table.BillBy = eachOrder_Masters.User_name;

                    balance_Calculation_Data_Table.Bill_Date = eachOrder_Masters.Date;
                    balance_Calculation_Data_Table.Billing_Total_amount = decimal.Zero;
                    balance_Calculation_Data_Table.Billing_Amount_paid = decimal.Zero;
                    balance_Calculation_Data_Table.Billing_Balance = decimal.Zero;

                    balance_Calculation_Data_Table.Order_Total_amount = balance_Calculation_Data_Table.Order_Total_amount;
                    balance_Calculation_Data_Table.Order_Amount_paid = balance_Calculation_Data_Table.Order_Amount_paid;
                    balance_Calculation_Data_Table.Order_Balance = balance_Calculation_Data_Table.Order_Balance;

                    balance_Calculation_Data_Table.Employee_salary_expense_Date = eachOrder_Masters.Date;
                    balance_Calculation_Data_Table.Employee_salary_expense_salary_to_be_paid = decimal.Zero;

                    balance_Calculation_Data_Table.Other_expense_Date = eachOrder_Masters.Date;
                    balance_Calculation_Data_Table.Other_expense_amount = decimal.Zero;

                    balance_Calculation_Data_Table.Product_expense_Date = eachOrder_Masters.Date;
                    balance_Calculation_Data_Table.Product_expense_Amount_for_expense = decimal.Zero;

                    balance_Calculation_Data_TableList.Add(balance_Calculation_Data_Table);
                }

                foreach (var eachEmployee_salary_expenses in Employee_salary_expenses)
                {
                    balance_Calculation_Data_Table = new Balance_calculation_data_table();
                    balance_Calculation_Data_Table.Token_Number = Guid.NewGuid().ToString();
                    //if (DateTime.TryParseExact(eachEmployee_salary_expenses.Date.Value.ToString(), formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime eachEmployee_salary_expensesDatetime))
                    //    tradedate = DateTime.Parse(eachEmployee_salary_expensesDatetime.ToString("dd-MM-yyyy"));
                    if (!string.IsNullOrEmpty(tradedate.ToString()))
                        balance_Calculation_Data_Table.Date = eachEmployee_salary_expenses.Date /*DateTime.Parse(tradedate.ToString())*//*+ TimeSpan.Parse(TimeSpan.Parse(eachEmployee_salary_expenses.Time.ToString()).ToString("HH:mm:ss"))*/;
                    else
                        balance_Calculation_Data_Table.Date = DateTime.Now;

                    balance_Calculation_Data_Table.Employee_salary_expense_Token_number = eachEmployee_salary_expenses.Token_number;
                    balance_Calculation_Data_Table.Employee_salary_expense_Date = eachEmployee_salary_expenses.Date;
                    balance_Calculation_Data_Table.Employee_salary_expense_salary_to_be_paid = eachEmployee_salary_expenses.salary_to_be_paid;
                    balance_Calculation_Data_Table.Employee_salary_expense_ExpenseId = eachEmployee_salary_expenses.ExpenseId;
                    balance_Calculation_Data_Table.BillBy = eachEmployee_salary_expenses.User_name;

                    balance_Calculation_Data_Table.Bill_Date = DateTime.Now;
                    balance_Calculation_Data_Table.Billing_Total_amount = decimal.Zero;
                    balance_Calculation_Data_Table.Billing_Amount_paid = decimal.Zero;
                    balance_Calculation_Data_Table.Billing_Balance = decimal.Zero;

                    balance_Calculation_Data_Table.Order_Date = DateTime.Now;
                    balance_Calculation_Data_Table.Order_Total_amount = decimal.Zero;
                    balance_Calculation_Data_Table.Order_Amount_paid = decimal.Zero;
                    balance_Calculation_Data_Table.Order_Balance = decimal.Zero;

                    balance_Calculation_Data_Table.Employee_salary_expense_salary_to_be_paid = balance_Calculation_Data_Table.Employee_salary_expense_salary_to_be_paid;

                    balance_Calculation_Data_Table.Other_expense_Date = DateTime.Now;
                    balance_Calculation_Data_Table.Other_expense_amount = decimal.Zero;

                    balance_Calculation_Data_Table.Product_expense_Date = DateTime.Now;
                    balance_Calculation_Data_Table.Product_expense_Amount_for_expense = decimal.Zero;

                    balance_Calculation_Data_TableList.Add(balance_Calculation_Data_Table);
                }
                if (Other_expenses.Count() == 0)
                {
                    foreach (var eachOther_expenses in Other_expenses)
                    {
                        balance_Calculation_Data_Table = new Balance_calculation_data_table();
                        balance_Calculation_Data_Table.Token_Number = Guid.NewGuid().ToString();
                        //if (DateTime.TryParseExact(eachOther_expenses.Date.Value.ToString(), formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime eachOther_expensesdateTime))
                        //    tradedate = DateTime.Parse(eachOther_expensesdateTime.ToString("dd-MM-yyyy"));
                        if (!string.IsNullOrEmpty(tradedate.ToString()))
                            balance_Calculation_Data_Table.Date = eachOther_expenses.Date/*DateTime.Parse(tradedate.ToString())*/ /*+ TimeSpan.Parse(TimeSpan.Parse(eachOther_expenses.Time.ToString()).ToString("HH:mm:ss"))*/;
                        else
                            balance_Calculation_Data_Table.Date = DateTime.Now;
                        balance_Calculation_Data_Table.Other_expense_Token_number = eachOther_expenses.Token_number;
                        balance_Calculation_Data_Table.Other_expense_Date = eachOther_expenses.Date;
                        balance_Calculation_Data_Table.Other_expense_amount = eachOther_expenses.Other_expense_amount;
                        balance_Calculation_Data_Table.Other_expense_ExpenseId = eachOther_expenses.ExpenseId;
                        balance_Calculation_Data_Table.BillBy = eachOther_expenses.User_name;

                        balance_Calculation_Data_Table.Bill_Date = DateTime.Now.Date;
                        balance_Calculation_Data_Table.Billing_Total_amount = decimal.Zero;
                        balance_Calculation_Data_Table.Billing_Amount_paid = decimal.Zero;
                        balance_Calculation_Data_Table.Billing_Balance = decimal.Zero;

                        balance_Calculation_Data_Table.Order_Date = DateTime.Now.Date;
                        balance_Calculation_Data_Table.Order_Total_amount = decimal.Zero;
                        balance_Calculation_Data_Table.Order_Amount_paid = decimal.Zero;
                        balance_Calculation_Data_Table.Order_Balance = decimal.Zero;

                        balance_Calculation_Data_Table.Employee_salary_expense_Date = DateTime.Now.Date;
                        balance_Calculation_Data_Table.Employee_salary_expense_salary_to_be_paid = decimal.Zero;

                        balance_Calculation_Data_Table.Other_expense_Date = balance_Calculation_Data_Table.Other_expense_Date;
                        balance_Calculation_Data_Table.Other_expense_amount = balance_Calculation_Data_Table.Other_expense_amount;

                        balance_Calculation_Data_Table.Product_expense_Date = DateTime.Now.Date;
                        balance_Calculation_Data_Table.Product_expense_Amount_for_expense = decimal.Zero;

                        balance_Calculation_Data_TableList.Add(balance_Calculation_Data_Table);
                    }
                }
                foreach (var eachProduct_expenses in Product_expenses)
                {
                    balance_Calculation_Data_Table = new Balance_calculation_data_table();
                    balance_Calculation_Data_Table.Token_Number = Guid.NewGuid().ToString();

                    //if (DateTime.TryParseExact(eachProduct_expenses.Date.Value.ToString(), formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime eachProduct_expensesdateTime))
                    //    tradedate = DateTime.Parse(eachProduct_expensesdateTime.ToString("dd-MM-yyyy"));
                    if (!string.IsNullOrEmpty(tradedate.ToString()))
                        balance_Calculation_Data_Table.Date = eachProduct_expenses.Date /*DateTime.Parse(tradedate.ToString())*/ /*+ TimeSpan.Parse(TimeSpan.Parse(eachProduct_expenses.Time.ToString()).ToString("HH:mm:ss"))*/;
                    else
                        balance_Calculation_Data_Table.Date = DateTime.Now;
                    balance_Calculation_Data_Table.Product_expense_Token_number = eachProduct_expenses.Token_number;
                    balance_Calculation_Data_Table.Product_expense_Date = eachProduct_expenses.Date;
                    balance_Calculation_Data_Table.Product_expense_Amount_for_expense = eachProduct_expenses.Amount_for_expense;
                    balance_Calculation_Data_Table.Product_expense_ExpenseId = eachProduct_expenses.ExpenseId;
                    balance_Calculation_Data_Table.BillBy = eachProduct_expenses.User_name;

                    balance_Calculation_Data_Table.Bill_Date = DateTime.Now;
                    balance_Calculation_Data_Table.Billing_Total_amount = decimal.Zero;
                    balance_Calculation_Data_Table.Billing_Amount_paid = decimal.Zero;
                    balance_Calculation_Data_Table.Billing_Balance = decimal.Zero;

                    balance_Calculation_Data_Table.Order_Date = DateTime.Now;
                    balance_Calculation_Data_Table.Order_Total_amount = decimal.Zero;
                    balance_Calculation_Data_Table.Order_Amount_paid = decimal.Zero;
                    balance_Calculation_Data_Table.Order_Balance = decimal.Zero;

                    balance_Calculation_Data_Table.Employee_salary_expense_Date = DateTime.Now;
                    balance_Calculation_Data_Table.Employee_salary_expense_salary_to_be_paid = decimal.Zero;

                    balance_Calculation_Data_Table.Other_expense_Date = DateTime.Now;
                    balance_Calculation_Data_Table.Other_expense_amount = decimal.Zero;

                    balance_Calculation_Data_Table.Product_expense_Amount_for_expense = balance_Calculation_Data_Table.Product_expense_Amount_for_expense;

                    balance_Calculation_Data_TableList.Add(balance_Calculation_Data_Table);
                }
                db.Balance_calculation_data_tables.AddRange(balance_Calculation_Data_TableList);
                db.SaveChanges();
            }catch(Exception ex)
            {
                
            }
            var nowDate = DateTime.Now.Date;
            var Balance_calculation_data_tables = db.Balance_calculation_data_tables.Where(x => x.Date < nowDate).ToList();

            List<BalanceSheetClass> balanceSheetClassList = new List<BalanceSheetClass>();
            BalanceSheetClass balanceSheetClass = new BalanceSheetClass();
            List<BalanceSheetClassesAll> balanceSheetClassesAll = new List<BalanceSheetClassesAll>();
            BalanceSheetClassesAll balanceSheetClassesAllSengle = new BalanceSheetClassesAll();
            var crryForwrd = decimal.Zero;
            foreach (var eachBalance_calculation_data_tables in Balance_calculation_data_tables)
            {
                if (!string.IsNullOrEmpty(eachBalance_calculation_data_tables.Billing_Amount_paid.ToString()))
                {
                    crryForwrd = crryForwrd + eachBalance_calculation_data_tables.Billing_Amount_paid;
                }
                if (!string.IsNullOrEmpty(eachBalance_calculation_data_tables.Order_Amount_paid.ToString()))
                {
                    crryForwrd = crryForwrd + eachBalance_calculation_data_tables.Order_Amount_paid;
                }
                if (!string.IsNullOrEmpty(eachBalance_calculation_data_tables.Employee_salary_expense_salary_to_be_paid.ToString()))
                {
                    crryForwrd = crryForwrd - eachBalance_calculation_data_tables.Employee_salary_expense_salary_to_be_paid;
                }
                if (!string.IsNullOrEmpty(eachBalance_calculation_data_tables.Other_expense_amount.ToString()))
                {
                    crryForwrd = crryForwrd - eachBalance_calculation_data_tables.Other_expense_amount;
                }
                if (!string.IsNullOrEmpty(eachBalance_calculation_data_tables.Product_expense_Amount_for_expense.ToString()))
                {
                    crryForwrd = crryForwrd - eachBalance_calculation_data_tables.Product_expense_Amount_for_expense;
                }
                //crryForwrd = crryForwrd + eachBalance_calculation_data_tables.Billing_Amount_paid ?? decimal.Zero
                //    + eachBalance_calculation_data_tables.Order_Amount_paid ?? decimal.Zero
                //    - eachBalance_calculation_data_tables.Employee_salary_expense_salary_to_be_paid ?? decimal.Zero
                //    - eachBalance_calculation_data_tables.Other_expense_amount ?? decimal.Zero
                //    - eachBalance_calculation_data_tables.Product_expense_Amount_for_expense ?? decimal.Zero;
            }


            // while (balanceSheetClass.EDate == balanceSheetClass.SDate)
            DateTime _fileStartDate = nowDate;
            DateTime _fileEndDate = nowDate;

            var _fileNextDate = nowDate.AddDays(1);

            balanceSheetClassList = new List<BalanceSheetClass>();
            var billingall = db.Billing_Masters.Where(x => x.Date < _fileNextDate && x.Date >= nowDate).Distinct().ToList();
            var billingDetall = db.Billing_Details.Where(x => x.Date < _fileNextDate && x.Date >= nowDate).Distinct().ToList();
            var stockDetall = db.Products_For_Sales.Distinct().ToList();
            var custlist = db.Customers.Distinct().ToList();
            foreach (var eachbilling in billingall)
            {
                balanceSheetClass = new BalanceSheetClass();
                foreach (var echbilldet in billingDetall)
                {
                    if (eachbilling.Billing_Number == echbilldet.Billing_number)
                    {
                        foreach (var eachstocks in stockDetall)
                        {
                            if (echbilldet.Selling_item_token == eachstocks.Token_Number)
                            {

                                balanceSheetClass.Billing_Number = eachbilling.Billing_Number;
                                foreach (var eachcust in custlist)
                                {
                                    if (eachbilling.Customer_token_number == eachcust.Token_number)
                                    {
                                        balanceSheetClass.Billing_Details = eachbilling.Billing_Number + " : " + eachcust.Customer_Name + " : "
                                            + eachstocks.Product_name + " : " + eachcust.Vehicle_type + " : " + eachcust.Vehicle_number + " : " + eachstocks.Tyre_make + " : " + eachstocks.Tyre_feel + " : " +
                                   eachstocks.Tyre_type;
                                        break;
                                    }
                                }
                                balanceSheetClass.Bill_BillBy = eachbilling.User_name;
                                balanceSheetClass.Billing_Total = eachbilling.Total_amount;
                                //if (DateTime.TryParseExact(eachbilling.Date.ToString(), formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime eachbillingdateTime))
                                //    tradedate = DateTime.Parse(eachbillingdateTime.ToString("dd-MM-yyyy hh:mm tt"));
                                balanceSheetClass.Billing_Date = eachbilling.Date.ToString();
                                balanceSheetClass.Billing_Amount_paid = eachbilling.Amount_paid;
                                balanceSheetClass.Billing_Balance = eachbilling.Balance;
                                balanceSheetClass.Billing_Total_tax = echbilldet.Pieces * eachstocks.Selling_Price * ((eachstocks.Selling_CGST + eachstocks.Selling_SGST) / 100);
                                balanceSheetClass.SDate = _fileStartDate;
                                balanceSheetClass.EDate = _fileEndDate;
                                balanceSheetClassList.Add(balanceSheetClass);
                                break;
                            }
                        }

                    }
                }
            }
            var previousDateTime = nowDate.AddDays(-1);
            //var billingallPreviousDay = db.Billing_Masters.Where(x => x.Date < nowDate && x.Date >= previousDateTime).Distinct().ToList();
            //var billingDetallPreviousDay = db.Billing_Details.Where(x => x.Date < nowDate && x.Date >= previousDateTime).Distinct().ToList();

            //foreach (var eachbilling in billingallPreviousDay)
            //{
            //    foreach (var echbilldet in billingDetallPreviousDay)
            //    {
            //        if (eachbilling.Billing_Number == echbilldet.Billing_number)
            //        {
            //            foreach (var eachstocks in stockDetall)
            //            {
            //                if (echbilldet.Selling_item_token == eachstocks.Token_Number)
            //                {
            //                    crryForwrd = crryForwrd + eachbilling.Amount_paid;
            //                    break;
            //                }
            //            }

            //        }
            //    }
            //}

            var purchaseall = db.Purchase_Masters.Where(x => x.Date < _fileNextDate && x.Date >= nowDate).Distinct().ToList();
            foreach (var eachpurchase in purchaseall)
            {
                balanceSheetClass = new BalanceSheetClass();
                balanceSheetClass.Purchase_Number = eachpurchase.Purchase_Number;
                //if (DateTime.TryParseExact(eachpurchase.Date.ToString(), formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime eachpurchasedateTime))
                //    tradedate = DateTime.Parse(eachpurchasedateTime.ToString("dd-MM-yyyy hh:mm tt"));
                balanceSheetClass.Purchase_Date = eachpurchase.Date.ToString();
                balanceSheetClass.Purchase_CGST = eachpurchase.CGST;
                balanceSheetClass.Purchase_SGST = eachpurchase.SGST;
                balanceSheetClass.Purchase_Total_amount = eachpurchase.Total_amount;
                balanceSheetClass.SDate = _fileStartDate;
                balanceSheetClass.EDate = _fileEndDate;
               
                balanceSheetClassList.Add(balanceSheetClass);
            }

            var empsalExpeseall = db.Employee_salary_expenses.Where(x => x.Date < _fileNextDate && x.Date >= nowDate).Distinct().ToList();
            foreach (var empsalExpese in empsalExpeseall)
            {
                balanceSheetClass = new BalanceSheetClass();
                balanceSheetClass.ExpenseId = empsalExpese.ExpenseId;
                balanceSheetClass.ExpenseTotal = empsalExpese.salary_to_be_paid;
                //if (DateTime.TryParseExact(empsalExpese.Date.ToString(), formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime empsalExpesedateTime))
                //    tradedate = DateTime.Parse(empsalExpesedateTime.ToString("dd-MM-yyyy"));
                balanceSheetClass.ExenseDate = empsalExpese.Date.ToString() /*tradedate.ToString() + " " + DateTime.Parse(empsalExpese.Time.ToString()).ToString("hh:mm tt")*/;
                balanceSheetClass.ExpenseTotalGST = 0;
                balanceSheetClass.ExpenseBalance = 0;
                balanceSheetClass.Expense_Details = empsalExpese.ExpenseId + " : " + "Employee salary expense" + " : " + empsalExpese.Employee_name;
                balanceSheetClass.SDate = _fileStartDate;
                balanceSheetClass.EDate = _fileEndDate;
                balanceSheetClass.Expense_BillBy = empsalExpese.User_name;
                balanceSheetClassList.Add(balanceSheetClass);
            }
            var otherexpenesall = db.Other_expenses.Where(x => x.Date < _fileNextDate && x.Date >= nowDate).Distinct().ToList();
            foreach (var otherexpense in otherexpenesall)
            {
                balanceSheetClass = new BalanceSheetClass();
                balanceSheetClass.ExpenseId = otherexpense.ExpenseId;
                balanceSheetClass.Expense_Details = otherexpense.ExpenseId + " : " + "Other expense" + " : " + otherexpense.Other_expense_type;
                //if (DateTime.TryParseExact(otherexpense.Date.ToString(), formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime otherexpensedateTime))
                //    tradedate = DateTime.Parse(otherexpensedateTime.ToString("dd-MM-yyyy"));
                balanceSheetClass.ExenseDate = otherexpense.Date.ToString()/*tradedate.ToString() + " " + DateTime.Parse(otherexpense.Time.ToString()).ToString("hh:mm tt")*/;
                balanceSheetClass.ExpenseTotal = 0;
                balanceSheetClass.ExpenseBalance = 0;
                balanceSheetClass.ExpenseTotal = otherexpense.Other_expense_amount;
                balanceSheetClass.SDate = _fileStartDate;
                balanceSheetClass.EDate = _fileEndDate;
                balanceSheetClass.Expense_BillBy = otherexpense.User_name;
                balanceSheetClassList.Add(balanceSheetClass);
            }
            var productexpenseall = db.Product_expenses.Where(x => x.Date < _fileNextDate && x.Date >= nowDate).Distinct().ToList();
            foreach (var productexpense in productexpenseall)
            {
                balanceSheetClass = new BalanceSheetClass();
                balanceSheetClass.ExpenseId = productexpense.ExpenseId;
                balanceSheetClass.ExpenseTotal = productexpense.Amount_for_expense;
                //if (DateTime.TryParseExact(productexpense.Date.ToString(), formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime productexpensedateTime))
                //    tradedate = DateTime.Parse(productexpensedateTime.ToString("dd-MM-yyyy"));
                balanceSheetClass.ExenseDate = productexpense.Date.ToString()/*tradedate.ToString() + " " + DateTime.Parse(productexpense.Time.ToString()).ToString("hh:mm tt")*/;
                balanceSheetClass.ExpenseTotalGST = 0;
                balanceSheetClass.ExpenseBalance = 0;
                balanceSheetClass.Expense_Details = productexpense.ExpenseId + " : " + "Product expense" + " : " + productexpense.Product_name;
                balanceSheetClass.SDate = _fileStartDate;
                balanceSheetClass.EDate = _fileEndDate;
                balanceSheetClass.Expense_BillBy = productexpense.User_name;
                balanceSheetClassList.Add(balanceSheetClass);
            }


            //////////////////////////////////////////////////////////////
            //var purchaseallPreviousDay = db.Purchase_Masters.Where(x => x.Date < nowDate && x.Date >= previousDateTime).Distinct().ToList();
            //foreach (var eachpurchase in purchaseallPreviousDay)
            //{
            //    crryForwrd = crryForwrd - eachpurchase.Total_amount;
            //}

            //var empsalExpeseallPreviousDay = db.Employee_salary_expenses.Where(x => x.Date < nowDate && x.Date >= previousDateTime).Distinct().ToList();
            //foreach (var empsalExpese in empsalExpeseallPreviousDay)
            //{
            //    crryForwrd = crryForwrd - empsalExpese.salary_to_be_paid;
            //}
            //var otherexpenesallPreviousDay = db.Other_expenses.Where(x => x.Date < nowDate && x.Date >= previousDateTime).Distinct().ToList();
            //foreach (var otherexpense in otherexpenesallPreviousDay)
            //{
            //    crryForwrd = crryForwrd - otherexpense.Other_expense_amount;
            //}
            //var productexpenseallPreviousDay = db.Product_expenses.Where(x => x.Date < nowDate && x.Date >= previousDateTime).Distinct().ToList();
            //foreach (var productexpense in productexpenseallPreviousDay)
            //{
            //    crryForwrd = crryForwrd - productexpense.Amount_for_expense;
            //}

            ///////////////////////////////////////////////////////////
            var orderall = db.Order_Masters.Where(x => x.Date < _fileNextDate && x.Date >= nowDate).Distinct().ToList();
            var echorderdetall = db.Order_Details.Where(x => x.Date < _fileNextDate && x.Date >= nowDate).Distinct().ToList();
            foreach (var eachorder in orderall)
            {
                balanceSheetClass = new BalanceSheetClass();
                foreach (var echorderdet in echorderdetall)
                {
                    if (eachorder.Order_Number == echorderdet.Order_number)
                    {
                        foreach (var eachstocks in stockDetall)
                        {
                            if (echorderdet.Selling_item_token == eachstocks.Token_Number)
                            {
                                foreach (var eachcust in custlist)
                                {
                                    if (eachorder.Customer_token_number == eachcust.Token_number)
                                    {
                                        balanceSheetClass.Order_Details = eachorder.Order_Number + " : " + eachcust.Customer_Name + " : "
                                            + eachstocks.Product_name + " : " + eachcust.Vehicle_type + " : " + eachcust.Vehicle_number + " : " + eachstocks.Tyre_make + " : " + eachstocks.Tyre_feel + " : " +
                                   eachstocks.Tyre_type;
                                        break;
                                    }
                                }
                                balanceSheetClass.Order_Amount_paid = eachorder.Amount_paid;
                                balanceSheetClass.Order_Balance = eachorder.Balance;
                                //if (DateTime.TryParseExact(eachorder.Date.ToString(), formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime eachorderdateTime))
                                //    tradedate = DateTime.Parse(eachorderdateTime.ToString("dd-MM-yyyy hh:mm tt"));
                                balanceSheetClass.Order_Date = eachorder.Date.ToString()/* tradedate.ToString()*/;
                                balanceSheetClass.Order_Number = eachorder.Order_Number;
                                balanceSheetClass.Order_Total = eachorder.Total_amount;
                                balanceSheetClass.Order_BillBy = eachorder.User_name;
                                balanceSheetClass.Order_Total_tax = echorderdet.Pieces * eachstocks.Selling_Price * ((eachstocks.Selling_CGST + eachstocks.Selling_SGST) / 100);
                                balanceSheetClass.SDate = _fileStartDate;
                                balanceSheetClass.EDate = _fileEndDate;
                                balanceSheetClassList.Add(balanceSheetClass);
                                break;
                            }
                        }

                    }
                }
            }

            //var orderallPreviousDay = db.Order_Masters.Where(x => x.Date < nowDate && x.Date >= previousDateTime).Distinct().ToList();
            //var echorderdetallPreviousDay = db.Order_Details.Where(x => x.Date < nowDate && x.Date >= previousDateTime).Distinct().ToList();
            //foreach (var eachorder in orderallPreviousDay)
            //{
            //    foreach (var echorderdet in echorderdetallPreviousDay)
            //    {
            //        if (eachorder.Order_Number == echorderdet.Order_number)
            //        {
            //            foreach (var eachstocks in stockDetall)
            //            {
            //                if (echorderdet.Selling_item_token == eachstocks.Token_Number)
            //                {
            //                    crryForwrd = crryForwrd + eachorder.Amount_paid;
            //                    break;
            //                }
            //            }

            //        }
            //    }
            //}
            //if (crryForwrd > decimal.Zero)
            //{
            balanceSheetClass = new BalanceSheetClass();
            balanceSheetClass.SDate = _fileStartDate;
            balanceSheetClass.EDate = _fileEndDate;
            //if (DateTime.TryParseExact(previousDateTime.Date.ToString(), formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTime))
            //    tradedate = DateTime.Parse(dateTime.ToString("dd-MM-yyyy hh:mm tt"));
            balanceSheetClass.CarryForwardDate = previousDateTime.Date.ToString();
            balanceSheetClass.CarryForward = crryForwrd;
            balanceSheetClassList.Add(balanceSheetClass);
            //}

            balanceSheetClass = new BalanceSheetClass();
            balanceSheetClass.TotalincomeCGST = 0;
            balanceSheetClass.TotalincomeSGST = 0;
            balanceSheetClass.TotalExpensesCGST = 0;
            balanceSheetClass.TotalExpensesSGST = 0;
            balanceSheetClass.TotalIncmBalance = 0;
            balanceSheetClass.TotalExpBalance = 0;
            foreach (var echbalsheet in balanceSheetClassList)
            {
                balanceSheetClass.TotalincomeCGST = balanceSheetClass.TotalincomeCGST + (echbalsheet.Billing_Total_tax / 2) + (echbalsheet.Order_Total_tax / 2);
                balanceSheetClass.TotalincomeSGST = balanceSheetClass.TotalincomeSGST + (echbalsheet.Billing_Total_tax / 2) + (echbalsheet.Order_Total_tax / 2);
                balanceSheetClass.TotalExpensesCGST = balanceSheetClass.TotalExpensesCGST + echbalsheet.Purchase_CGST;
                balanceSheetClass.TotalExpensesSGST = balanceSheetClass.TotalExpensesSGST + echbalsheet.Purchase_SGST;
                balanceSheetClass.TotalIncmBalance = balanceSheetClass.TotalIncmBalance + echbalsheet.Billing_Amount_paid + echbalsheet.Order_Amount_paid;
                balanceSheetClass.TotalExpBalance = balanceSheetClass.TotalExpBalance + echbalsheet.ExpenseTotal;
            }
            if (balanceSheetClassList.Count() > 0)
            {
                // if(balanceSheetClass.TotalIncmBalance!=0)
                balanceSheetClass.TotalBalance = balanceSheetClass.TotalIncmBalance - balanceSheetClass.TotalExpBalance;

                balanceSheetClassList.Add(balanceSheetClass);


                balanceSheetClassesAllSengle = new BalanceSheetClassesAll();
                balanceSheetClassesAllSengle.BalanceSheetClassDateWise = new List<BalanceSheetClass>();
                balanceSheetClassesAllSengle.Date = _fileStartDate;
                balanceSheetClassesAllSengle.CarryForward = crryForwrd;
                //if (DateTime.TryParseExact(nowDate.Date.ToString(), formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
                //    tradedate = DateTime.Parse(dateTime.ToString("dd-MM-yyyy"));
                balanceSheetClassesAllSengle.CarryForwardDate = nowDate.Date.ToString();
                balanceSheetClassesAllSengle.TotalBalance = balanceSheetClass.TotalIncmBalance - balanceSheetClass.TotalExpBalance + crryForwrd;
                balanceSheetClassesAllSengle.BalanceSheetClassDateWise.AddRange(balanceSheetClassList);
                balanceSheetClassesAll.Add(balanceSheetClassesAllSengle);
            }
            return View(balanceSheetClassesAll);


        }
        //public ActionResult BalanceSheet_singlebackup()
        //{
        //   List<BalanceSheetClass> balanceSheetClassList = new List<BalanceSheetClass>();
        //    BalanceSheetClass balanceSheetClass = new BalanceSheetClass();
        //    List<BalanceSheetClassesAll> balanceSheetClassesAll = new List<BalanceSheetClassesAll>();
        //    BalanceSheetClassesAll balanceSheetClassesAllSengle = new BalanceSheetClassesAll();
        //    var crryForwrd = decimal.Zero;
        //    var nowDate = DateTime.Now.Date;
        //    var billingall = db.Billing_Masters.Where(x => x.Date == nowDate).Distinct().ToList();
        //    var billingDetall = db.Billing_Details.Where(x => x.Date == nowDate).Distinct().ToList();
        //    var stockDetall = db.Products_For_Sales.Distinct().ToList();
        //    var custlist = db.Customers.Distinct().ToList();
        //    foreach (var eachbilling in billingall)
        //    {
        //        balanceSheetClass = new BalanceSheetClass();
        //        foreach (var echbilldet in billingDetall)
        //        {
        //            if (eachbilling.Billing_Number == echbilldet.Billing_number)
        //            {
        //                foreach(var eachstocks in stockDetall)
        //                {
        //                    if(echbilldet.Selling_item_token == eachstocks.Token_Number)
        //                    {

        //                        balanceSheetClass.Billing_Number = eachbilling.Billing_Number;
        //                        foreach (var eachcust in custlist)
        //                        {
        //                            if (eachbilling.Customer_token_number == eachcust.Token_number)
        //                            {
        //                                balanceSheetClass.Billing_Details = eachbilling.Billing_Number+" : "+ eachcust.Customer_Name+" : "
        //                                    + eachstocks.Product_name + " : " + eachcust.Vehicle_type + " : "+ eachcust.Vehicle_number +" : "+ eachstocks.Tyre_make + " : " + eachstocks.Tyre_feel + " : " +
        //                           eachstocks.Tyre_type ;
        //                                break;
        //                            }
        //                        }
        //                        balanceSheetClass.Billing_Total = eachbilling.Total_amount;
        //                        balanceSheetClass.Billing_Date =eachbilling.Date.Date;
        //                        balanceSheetClass.Billing_Amount_paid = eachbilling.Amount_paid;
        //                        balanceSheetClass.Billing_Balance = eachbilling.Balance;
        //                        balanceSheetClass.Billing_Total_tax = echbilldet.Pieces*eachstocks.Selling_Price* ((eachstocks.Selling_CGST+ eachstocks.Selling_SGST)/100);
        //                        balanceSheetClassList.Add(balanceSheetClass);
        //                        break;
        //                    }
        //                }

        //            }
        //        }
        //    }
        //    var previousDateTime = nowDate.AddDays(-1);
        //    var billingallPreviousDay = db.Billing_Masters.Where(x => x.Date == previousDateTime).Distinct().ToList();
        //    var billingDetallPreviousDay = db.Billing_Details.Where(x => x.Date == previousDateTime).Distinct().ToList();

        //    foreach (var eachbilling in billingallPreviousDay)
        //    {
        //        balanceSheetClass = new BalanceSheetClass();
        //        foreach (var echbilldet in billingDetallPreviousDay)
        //        {
        //            if (eachbilling.Billing_Number == echbilldet.Billing_number)
        //            {
        //                foreach (var eachstocks in stockDetall)
        //                {
        //                    if (echbilldet.Selling_item_token == eachstocks.Token_Number)
        //                    {
        //                        crryForwrd = crryForwrd + eachbilling.Amount_paid;
        //                        break;
        //                    }
        //                }

        //            }
        //        }
        //    }

        //    var purchaseall = db.Purchase_Masters.Where(x => x.Date == nowDate).Distinct().ToList();
        //    foreach (var eachpurchase in purchaseall)
        //    {
        //        balanceSheetClass = new BalanceSheetClass();
        //        balanceSheetClass.Purchase_Number = eachpurchase.Purchase_Number;
        //        balanceSheetClass.Purchase_Date = eachpurchase.Date.Date;
        //        balanceSheetClass.Purchase_CGST = eachpurchase.CGST;
        //        balanceSheetClass.Purchase_SGST = eachpurchase.SGST;
        //        balanceSheetClass.Purchase_Total_amount = eachpurchase.Total_amount;
        //        balanceSheetClassList.Add(balanceSheetClass);
        //    }

        //    var empsalExpeseall = db.Employee_salary_expenses.Where(x => x.Date == nowDate).Distinct().ToList();
        //    foreach (var empsalExpese in empsalExpeseall)
        //    {
        //        balanceSheetClass = new BalanceSheetClass();
        //        balanceSheetClass.ExpenseId = empsalExpese.ExpenseId;
        //        balanceSheetClass.ExpenseTotal = empsalExpese.salary_to_be_paid;
        //        balanceSheetClass.ExenseDate = empsalExpese.Date; 
        //        balanceSheetClass.ExpenseTotalGST = 0;
        //        balanceSheetClass.ExpenseBalance = 0;
        //        balanceSheetClass.Expense_Details = empsalExpese.ExpenseId + " : " + "Employee salary expense" + " : " + empsalExpese.Employee_name;
        //        balanceSheetClassList.Add(balanceSheetClass);
        //    }
        //    var otherexpenesall = db.Other_expenses.Where(x => x.Date == nowDate).Distinct().ToList();
        //    foreach (var otherexpense in otherexpenesall)
        //    {
        //        balanceSheetClass = new BalanceSheetClass();
        //        balanceSheetClass.ExpenseId = otherexpense.ExpenseId;
        //        balanceSheetClass.Expense_Details = otherexpense.ExpenseId + " : " + "Other expense" + " : " + otherexpense.Other_expense_type;
        //        balanceSheetClass.ExenseDate =otherexpense.Date;
        //        balanceSheetClass.ExpenseTotal = 0;
        //        balanceSheetClass.ExpenseBalance =0;
        //        balanceSheetClass.ExpenseTotal = otherexpense.Other_expense_amount;
        //        balanceSheetClassList.Add(balanceSheetClass);
        //    }
        //    var productexpenseall = db.Product_expenses.Where(x => x.Date == nowDate).Distinct().ToList();
        //    foreach (var productexpense in productexpenseall)
        //    {
        //        balanceSheetClass = new BalanceSheetClass();
        //        balanceSheetClass.ExpenseId = productexpense.ExpenseId ;
        //        balanceSheetClass.ExpenseTotal = productexpense.Amount_for_expense;
        //        balanceSheetClass.ExenseDate = productexpense.Date;
        //        balanceSheetClass.ExpenseTotalGST =0;
        //        balanceSheetClass.ExpenseBalance = 0;
        //        balanceSheetClass.Expense_Details = productexpense.ExpenseId + " : " + "Product expense" + " : " + productexpense.Product_name;
        //        balanceSheetClassList.Add(balanceSheetClass);
        //    }

        //    var orderall = db.Order_Masters.Where(x => x.Date == nowDate).Distinct().ToList();
        //    var echorderdetall = db.Order_Details.Where(x => x.Date == nowDate).Distinct().ToList();
        //    foreach (var eachorder in orderall)
        //    {
        //        balanceSheetClass = new BalanceSheetClass();
        //        foreach (var echorderdet in echorderdetall)
        //        {
        //            if (eachorder.Order_Number == echorderdet.Order_number)
        //            {
        //                foreach (var eachstocks in stockDetall)
        //                {
        //                    if (echorderdet.Selling_item_token == eachstocks.Token_Number)
        //                    {
        //                        foreach (var eachcust in custlist)
        //                        {
        //                            if (eachorder.Customer_token_number == eachcust.Token_number)
        //                            {
        //                                balanceSheetClass.Order_Details = eachorder.Order_Number + " : " + eachcust.Customer_Name + " : "
        //                                    + eachstocks.Product_name + " : " + eachcust.Vehicle_type + " : " + eachcust.Vehicle_number + " : " + eachstocks.Tyre_make + " : " + eachstocks.Tyre_feel + " : " +
        //                           eachstocks.Tyre_type;
        //                                break;
        //                            }
        //                        }
        //                        balanceSheetClass.Order_Amount_paid = eachorder.Amount_paid;
        //                        balanceSheetClass.Order_Balance = eachorder.Balance;
        //                        balanceSheetClass.Order_Date = eachorder.Date.Date;
        //                        balanceSheetClass.Order_Number = eachorder.Order_Number;
        //                        balanceSheetClass.Order_Total = eachorder.Total_amount;
        //                        balanceSheetClass.Order_Total_tax = echorderdet.Pieces * eachstocks.Selling_Price * ((eachstocks.Selling_CGST + eachstocks.Selling_SGST) / 100);
        //                        balanceSheetClassList.Add(balanceSheetClass);
        //                        break;
        //                    }
        //                }

        //            }
        //        }
        //    }

        //    var orderallPreviousDay = db.Order_Masters.Where(x => x.Date == previousDateTime).Distinct().ToList();
        //    var echorderdetallPreviousDay = db.Order_Details.Where(x => x.Date == previousDateTime).Distinct().ToList();
        //    foreach (var eachorder in orderallPreviousDay)
        //    {
        //        balanceSheetClass = new BalanceSheetClass();
        //        foreach (var echorderdet in echorderdetallPreviousDay)
        //        {
        //            if (eachorder.Order_Number == echorderdet.Order_number)
        //            {
        //                foreach (var eachstocks in stockDetall)
        //                {
        //                    if (echorderdet.Selling_item_token == eachstocks.Token_Number)
        //                    {
        //                        crryForwrd = crryForwrd+ eachorder.Amount_paid;
        //                        break;
        //                    }
        //                }

        //            }
        //        }
        //    }
        //    if (crryForwrd > decimal.Zero)
        //    {
        //        balanceSheetClass = new BalanceSheetClass();
        //        balanceSheetClass.CarryForwardDate = previousDateTime;
        //        balanceSheetClass.CarryForward = crryForwrd;
        //        balanceSheetClassList.Add(balanceSheetClass);
        //    }

        //    balanceSheetClass = new BalanceSheetClass();
        //    balanceSheetClass.TotalincomeCGST = 0;
        //    balanceSheetClass.TotalincomeSGST = 0;
        //    balanceSheetClass.TotalExpensesCGST =0;
        //    balanceSheetClass.TotalExpensesSGST = 0;
        //    balanceSheetClass.TotalIncmBalance = 0;
        //    balanceSheetClass.TotalExpBalance = 0;
        //    foreach (var echbalsheet in balanceSheetClassList)
        //    {
        //        balanceSheetClass.TotalincomeCGST = balanceSheetClass.TotalincomeCGST + (echbalsheet.Billing_Total_tax/2) + (echbalsheet.Order_Total_tax/2);
        //        balanceSheetClass.TotalincomeSGST = balanceSheetClass.TotalincomeSGST + (echbalsheet.Billing_Total_tax / 2) + (echbalsheet.Order_Total_tax / 2);
        //        balanceSheetClass.TotalExpensesCGST = balanceSheetClass.TotalExpensesCGST + echbalsheet.Purchase_CGST;
        //        balanceSheetClass.TotalExpensesSGST = balanceSheetClass.TotalExpensesSGST + echbalsheet.Purchase_SGST;
        //        balanceSheetClass.TotalIncmBalance = balanceSheetClass.TotalIncmBalance + echbalsheet.Billing_Amount_paid + echbalsheet.Order_Amount_paid;
        //        balanceSheetClass.TotalExpBalance = balanceSheetClass.TotalExpBalance + echbalsheet.ExpenseTotal;
        //    }
        //    //if (balanceSheetClass.TotalIncmBalance != 0)
        //    //    balanceSheetClass.TotalBalance = balanceSheetClass.TotalIncmBalance - balanceSheetClass.TotalExpBalance;
        //    //balanceSheetClassList.Add(balanceSheetClass);

        //    if (balanceSheetClassList.Count() > 0)
        //    {
        //        if (balanceSheetClass.TotalIncmBalance != 0)
        //            balanceSheetClass.TotalBalance = balanceSheetClass.TotalIncmBalance - balanceSheetClass.TotalExpBalance;

        //        balanceSheetClassList.Add(balanceSheetClass);


        //        balanceSheetClassesAllSengle = new BalanceSheetClassesAll();
        //        balanceSheetClassesAllSengle.BalanceSheetClassDateWise = new List<BalanceSheetClass>();
        //        balanceSheetClassesAllSengle.Date = nowDate;
        //        balanceSheetClassesAllSengle.BalanceSheetClassDateWise.AddRange(balanceSheetClassList);
        //        balanceSheetClassesAll.Add(balanceSheetClassesAllSengle);
        //    }
        //    return View(balanceSheetClassesAll);
        //}
        [HttpPost]
        public ActionResult BalanceSheet(BalanceSheetClass balanceSheetClass)
        {
            ViewBag.SDate = balanceSheetClass.SDate.ToString("yyyy-MM-dd");
            ViewBag.EDate = balanceSheetClass.EDate.ToString("yyyy-MM-dd");
            List<BalanceSheetClass> balanceSheetClassList = new List<BalanceSheetClass>();
            List<BalanceSheetClassesAll> balanceSheetClassesAll = new List<BalanceSheetClassesAll>();
            BalanceSheetClassesAll balanceSheetClassesAllSengle = new BalanceSheetClassesAll();
            // while (balanceSheetClass.EDate == balanceSheetClass.SDate)
            DateTime _fileStartDate = balanceSheetClass.SDate;
            DateTime _fileEndDate = balanceSheetClass.EDate;

            var _fileNextDate = _fileStartDate.AddDays(1);
            //var crryForwrd = decimal.Zero;
            var nowDate = _fileStartDate;
            while (_fileStartDate <= _fileEndDate)
            {
                var crryForwrd = decimal.Zero;
                balanceSheetClassList = new List<BalanceSheetClass>();
                 _fileNextDate = _fileStartDate.AddDays(1);
               //  crryForwrd = decimal.Zero;
                 nowDate = _fileStartDate;
                var billingall = db.Billing_Masters.Where(x => x.Date < _fileNextDate && x.Date >= nowDate).Distinct().ToList();
                var billingDetall = db.Billing_Details.Where(x => x.Date < _fileNextDate && x.Date >= nowDate).Distinct().ToList();
                var stockDetall = db.Products_For_Sales.Distinct().ToList();
                var custlist = db.Customers.Distinct().ToList();
                foreach (var eachbilling in billingall)
                {
                    balanceSheetClass = new BalanceSheetClass();
                    foreach (var echbilldet in billingDetall)
                    {
                        if (eachbilling.Billing_Number == echbilldet.Billing_number)
                        {
                            foreach (var eachstocks in stockDetall)
                            {
                                if (echbilldet.Selling_item_token == eachstocks.Token_Number)
                                {

                                    balanceSheetClass.Billing_Number = eachbilling.Billing_Number;
                                    foreach (var eachcust in custlist)
                                    {
                                        if (eachbilling.Customer_token_number == eachcust.Token_number)
                                        {
                                            balanceSheetClass.Billing_Details = eachbilling.Billing_Number + " : " + eachcust.Customer_Name + " : "
                                                + eachstocks.Product_name + " : " + eachcust.Vehicle_type + " : " + eachcust.Vehicle_number + " : " + eachstocks.Tyre_make + " : " + eachstocks.Tyre_feel + " : " +
                                       eachstocks.Tyre_type;
                                            break;
                                        }
                                    }
                                    balanceSheetClass.Billing_Total = eachbilling.Total_amount;
                                    balanceSheetClass.Billing_Date = eachbilling.Date.ToString();
                                    balanceSheetClass.Billing_Amount_paid = eachbilling.Amount_paid;
                                    balanceSheetClass.Billing_Balance = eachbilling.Balance;
                                    balanceSheetClass.Billing_Total_tax = echbilldet.Pieces * eachstocks.Selling_Price * ((eachstocks.Selling_CGST + eachstocks.Selling_SGST) / 100);
                                    balanceSheetClass.SDate = _fileStartDate;
                                    balanceSheetClass.EDate = _fileEndDate;
                                    balanceSheetClass.Bill_BillBy = eachbilling.User_name;
                                    balanceSheetClassList.Add(balanceSheetClass);
                                    break;
                                }
                            }

                        }
                    }
                }
                var previousDateTime = nowDate.AddDays(-1);
                var Balance_calculation_data_tables = db.Balance_calculation_data_tables.Where(x => x.Date < nowDate).Distinct().ToList();

                foreach (var eachBalance_calculation_data_tables in Balance_calculation_data_tables)
                {
                    if (!string.IsNullOrEmpty(eachBalance_calculation_data_tables.Billing_Amount_paid.ToString()))
                    {
                        crryForwrd = crryForwrd + eachBalance_calculation_data_tables.Billing_Amount_paid;
                    }
                    if (!string.IsNullOrEmpty(eachBalance_calculation_data_tables.Order_Amount_paid.ToString()))
                    {
                        crryForwrd = crryForwrd + eachBalance_calculation_data_tables.Order_Amount_paid;
                    }
                    if (!string.IsNullOrEmpty(eachBalance_calculation_data_tables.Employee_salary_expense_salary_to_be_paid.ToString()))
                    {
                        crryForwrd = crryForwrd - eachBalance_calculation_data_tables.Employee_salary_expense_salary_to_be_paid;
                    }
                    if (!string.IsNullOrEmpty(eachBalance_calculation_data_tables.Other_expense_amount.ToString()))
                    {
                        crryForwrd = crryForwrd - eachBalance_calculation_data_tables.Other_expense_amount;
                    }
                    if (!string.IsNullOrEmpty(eachBalance_calculation_data_tables.Product_expense_Amount_for_expense.ToString()))
                    {
                        crryForwrd = crryForwrd - eachBalance_calculation_data_tables.Product_expense_Amount_for_expense;
                    }
                }

                var purchaseall = db.Purchase_Masters.Where(x => x.Date < _fileNextDate && x.Date >= nowDate).Distinct().ToList();
                foreach (var eachpurchase in purchaseall)
                {
                    balanceSheetClass = new BalanceSheetClass();
                    balanceSheetClass.Purchase_Number = eachpurchase.Purchase_Number;
                    balanceSheetClass.Purchase_Date = eachpurchase.Date.ToString();
                    balanceSheetClass.Purchase_CGST = eachpurchase.CGST;
                    balanceSheetClass.Purchase_SGST = eachpurchase.SGST;
                    balanceSheetClass.Purchase_Total_amount = eachpurchase.Total_amount;
                    balanceSheetClass.SDate = _fileStartDate;
                    balanceSheetClass.EDate = _fileEndDate;
                    balanceSheetClassList.Add(balanceSheetClass);
                }

                var empsalExpeseall = db.Employee_salary_expenses.Where(x => x.Date < _fileNextDate && x.Date >= nowDate).Distinct().ToList();
                foreach (var empsalExpese in empsalExpeseall)
                {
                    balanceSheetClass = new BalanceSheetClass();
                    balanceSheetClass.ExpenseId = empsalExpese.ExpenseId;
                    balanceSheetClass.ExpenseTotal = empsalExpese.salary_to_be_paid;
                    balanceSheetClass.ExenseDate = empsalExpese.Date.ToString(); 
                    balanceSheetClass.ExpenseTotalGST = 0;
                    balanceSheetClass.ExpenseBalance = 0;
                    balanceSheetClass.Expense_Details = empsalExpese.ExpenseId + " : " + "Employee salary expense" + " : " + empsalExpese.Employee_name;
                    balanceSheetClass.SDate = _fileStartDate;
                    balanceSheetClass.EDate = _fileEndDate;
                    balanceSheetClass.Expense_BillBy = empsalExpese.User_name;
                    balanceSheetClassList.Add(balanceSheetClass);
                }
                var otherexpenesall = db.Other_expenses.Where(x => x.Date < _fileNextDate && x.Date >= nowDate).Distinct().ToList();
                foreach (var otherexpense in otherexpenesall)
                {
                    balanceSheetClass = new BalanceSheetClass();
                    balanceSheetClass.ExpenseId = otherexpense.ExpenseId;
                    balanceSheetClass.Expense_Details = otherexpense.ExpenseId + " : " + "Other expense" + " : " + otherexpense.Other_expense_type;
                    balanceSheetClass.ExenseDate = otherexpense.Date.ToString();
                    balanceSheetClass.ExpenseTotal = 0;
                    balanceSheetClass.ExpenseBalance = 0;
                    balanceSheetClass.ExpenseTotal = otherexpense.Other_expense_amount;
                    balanceSheetClass.SDate = _fileStartDate;
                    balanceSheetClass.EDate = _fileEndDate;
                    balanceSheetClass.Expense_BillBy = otherexpense.User_name;
                    balanceSheetClassList.Add(balanceSheetClass);
                }
                var productexpenseall = db.Product_expenses.Where(x => x.Date < _fileNextDate && x.Date >= nowDate).Distinct().ToList();
                foreach (var productexpense in productexpenseall)
                {
                    balanceSheetClass = new BalanceSheetClass();
                    balanceSheetClass.ExpenseId = productexpense.ExpenseId;
                    balanceSheetClass.ExpenseTotal = productexpense.Amount_for_expense;
                    balanceSheetClass.ExenseDate = productexpense.Date.ToString();
                    balanceSheetClass.ExpenseTotalGST = 0;
                    balanceSheetClass.ExpenseBalance = 0;
                    balanceSheetClass.Expense_Details = productexpense.ExpenseId + " : " + "Product expense" + " : " + productexpense.Product_name;
                    balanceSheetClass.SDate = _fileStartDate;
                    balanceSheetClass.EDate = _fileEndDate;
                    balanceSheetClass.Expense_BillBy = productexpense.User_name;
                    balanceSheetClassList.Add(balanceSheetClass);
                }


                //////////////////////////////////////////////////////////////
                //var purchaseallPreviousDay = db.Purchase_Masters.Where(x => x.Date < nowDate && x.Date >= previousDateTime).Distinct().ToList();
                //foreach (var eachpurchase in purchaseallPreviousDay)
                //{
                //    crryForwrd = crryForwrd - eachpurchase.Total_amount;
                //}

                //var empsalExpeseallPreviousDay = db.Employee_salary_expenses.Where(x => x.Date < nowDate && x.Date >= previousDateTime).Distinct().ToList();
                //foreach (var empsalExpese in empsalExpeseallPreviousDay)
                //{
                //    crryForwrd = crryForwrd - empsalExpese.salary_to_be_paid;
                //}
                //var otherexpenesallPreviousDay = db.Other_expenses.Where(x => x.Date < nowDate && x.Date >= previousDateTime).Distinct().ToList();
                //foreach (var otherexpense in otherexpenesallPreviousDay)
                //{
                //    crryForwrd = crryForwrd - otherexpense.Other_expense_amount;
                //}
                //var productexpenseallPreviousDay = db.Product_expenses.Where(x => x.Date < nowDate && x.Date >= previousDateTime).Distinct().ToList();
                //foreach (var productexpense in productexpenseallPreviousDay)
                //{
                //    crryForwrd = crryForwrd - productexpense.Amount_for_expense;
                //}

                ///////////////////////////////////////////////////////////
                var orderall = db.Order_Masters.Where(x => x.Date < _fileNextDate && x.Date >= nowDate).Distinct().ToList();
                var echorderdetall = db.Order_Details.Where(x => x.Date < _fileNextDate && x.Date >= nowDate).Distinct().ToList();
                foreach (var eachorder in orderall)
                {
                    balanceSheetClass = new BalanceSheetClass();
                    foreach (var echorderdet in echorderdetall)
                    {
                        if (eachorder.Order_Number == echorderdet.Order_number)
                        {
                            foreach (var eachstocks in stockDetall)
                            {
                                if (echorderdet.Selling_item_token == eachstocks.Token_Number)
                                {
                                    foreach (var eachcust in custlist)
                                    {
                                        if (eachorder.Customer_token_number == eachcust.Token_number)
                                        {
                                            balanceSheetClass.Order_Details = eachorder.Order_Number + " : " + eachcust.Customer_Name + " : "
                                                + eachstocks.Product_name + " : " + eachcust.Vehicle_type + " : " + eachcust.Vehicle_number + " : " + eachstocks.Tyre_make + " : " + eachstocks.Tyre_feel + " : " +
                                       eachstocks.Tyre_type;
                                            break;
                                        }
                                    }
                                    balanceSheetClass.Order_Amount_paid = eachorder.Amount_paid;
                                    balanceSheetClass.Order_Balance = eachorder.Balance;
                                    balanceSheetClass.Order_Date = eachorder.Date.ToString();
                                    balanceSheetClass.Order_Number = eachorder.Order_Number;
                                    balanceSheetClass.Order_Total = eachorder.Total_amount;
                                    balanceSheetClass.Order_Total_tax = echorderdet.Pieces * eachstocks.Selling_Price * ((eachstocks.Selling_CGST + eachstocks.Selling_SGST) / 100);
                                    balanceSheetClass.SDate = _fileStartDate;
                                    balanceSheetClass.EDate = _fileEndDate;
                                    balanceSheetClass.Order_BillBy = eachorder.User_name;
                                    balanceSheetClassList.Add(balanceSheetClass);
                                    break;
                                }
                            }

                        }
                    }
                }

                //var orderallPreviousDay = db.Order_Masters.Where(x => x.Date < nowDate && x.Date >= previousDateTime).Distinct().ToList();
                //var echorderdetallPreviousDay = db.Order_Details.Where(x => x.Date < nowDate && x.Date >= previousDateTime).Distinct().ToList();
                //foreach (var eachorder in orderallPreviousDay)
                //{
                //    foreach (var echorderdet in echorderdetallPreviousDay)
                //    {
                //        if (eachorder.Order_Number == echorderdet.Order_number)
                //        {
                //            foreach (var eachstocks in stockDetall)
                //            {
                //                if (echorderdet.Selling_item_token == eachstocks.Token_Number)
                //                {
                //                    crryForwrd = crryForwrd + eachorder.Amount_paid;
                //                    break;
                //                }
                //            }

                //        }
                //    }
                //}
                //if (crryForwrd > decimal.Zero)
                //{
                    balanceSheetClass = new BalanceSheetClass();
                    balanceSheetClass.SDate = _fileStartDate;
                    balanceSheetClass.EDate = _fileEndDate;
                    balanceSheetClass.CarryForwardDate = previousDateTime.ToString();
                    balanceSheetClass.CarryForward = crryForwrd;
                    balanceSheetClassList.Add(balanceSheetClass);
                //}

                balanceSheetClass = new BalanceSheetClass();
                balanceSheetClass.TotalincomeCGST = 0;
                balanceSheetClass.TotalincomeSGST = 0;
                balanceSheetClass.TotalExpensesCGST = 0;
                balanceSheetClass.TotalExpensesSGST = 0;
                balanceSheetClass.TotalIncmBalance = 0;
                balanceSheetClass.TotalExpBalance = 0;
                foreach (var echbalsheet in balanceSheetClassList)
                {
                    balanceSheetClass.TotalincomeCGST = balanceSheetClass.TotalincomeCGST + (echbalsheet.Billing_Total_tax / 2) + (echbalsheet.Order_Total_tax / 2);
                    balanceSheetClass.TotalincomeSGST = balanceSheetClass.TotalincomeSGST + (echbalsheet.Billing_Total_tax / 2) + (echbalsheet.Order_Total_tax / 2);
                    balanceSheetClass.TotalExpensesCGST = balanceSheetClass.TotalExpensesCGST + echbalsheet.Purchase_CGST;
                    balanceSheetClass.TotalExpensesSGST = balanceSheetClass.TotalExpensesSGST + echbalsheet.Purchase_SGST;
                    balanceSheetClass.TotalIncmBalance = balanceSheetClass.TotalIncmBalance + echbalsheet.Billing_Amount_paid + echbalsheet.Order_Amount_paid;
                    balanceSheetClass.TotalExpBalance = balanceSheetClass.TotalExpBalance + echbalsheet.ExpenseTotal;
                }
                if (balanceSheetClassList.Count() > 0)
                {
                   // if(balanceSheetClass.TotalIncmBalance!=0)
                    balanceSheetClass.TotalBalance = balanceSheetClass.TotalIncmBalance - balanceSheetClass.TotalExpBalance + crryForwrd;
                   
                    balanceSheetClassList.Add(balanceSheetClass);
              
            
                balanceSheetClassesAllSengle = new BalanceSheetClassesAll();
                    balanceSheetClassesAllSengle.BalanceSheetClassDateWise = new List<BalanceSheetClass>();
                balanceSheetClassesAllSengle.Date = _fileStartDate;
                    balanceSheetClassesAllSengle.CarryForward = crryForwrd;
                    balanceSheetClassesAllSengle.CarryForwardDate = nowDate.ToString();
                    balanceSheetClassesAllSengle.TotalBalance = balanceSheetClass.TotalIncmBalance - balanceSheetClass.TotalExpBalance + crryForwrd;
                    balanceSheetClassesAllSengle.BalanceSheetClassDateWise.AddRange(balanceSheetClassList);
                balanceSheetClassesAll.Add(balanceSheetClassesAllSengle);
                }
                _fileStartDate = _fileStartDate.AddDays(1);
            }
            return View(balanceSheetClassesAll);
        }
        //[HttpPost]
        //public ActionResult BalanceSheet1(BalanceSheetClass balanceSheetClass)
        //{
        //    List<BalanceSheetClass> balanceSheetClassList = new List<BalanceSheetClass>();
        //    var crryForwrd = decimal.Zero;
        //    var nowDate = balanceSheetClass.SDate;
        //    var billingall = db.Billing_Masters.Where(x => x.Date <= balanceSheetClass.EDate && x.Date >= balanceSheetClass.SDate).Distinct().ToList();
        //    var billingDetall = db.Billing_Details.Where(x => x.Date <= balanceSheetClass.EDate && x.Date >= balanceSheetClass.SDate).Distinct().ToList();
        //    var stockDetall = db.Products_For_Sales.Distinct().ToList();
        //    var custlist = db.Customers.Distinct().ToList();
        //    foreach (var eachbilling in billingall)
        //    {
        //        balanceSheetClass = new BalanceSheetClass();
        //        foreach (var echbilldet in billingDetall)
        //        {
        //            if (eachbilling.Billing_Number == echbilldet.Billing_number)
        //            {
        //                foreach (var eachstocks in stockDetall)
        //                {
        //                    if (echbilldet.Selling_item_token == eachstocks.Token_Number)
        //                    {

        //                        balanceSheetClass.Billing_Number = eachbilling.Billing_Number;
        //                        foreach (var eachcust in custlist)
        //                        {
        //                            if (eachbilling.Customer_token_number == eachcust.Token_number)
        //                            {
        //                                balanceSheetClass.Billing_Details = eachbilling.Billing_Number + " : " + eachcust.Customer_Name + " : "
        //                                    + eachstocks.Product_name + " : " + eachcust.Vehicle_type + " : " + eachcust.Vehicle_number + " : " + eachstocks.Tyre_make + " : " + eachstocks.Tyre_feel + " : " +
        //                           eachstocks.Tyre_type;
        //                                break;
        //                            }
        //                        }
        //                        balanceSheetClass.Billing_Total = eachbilling.Total_amount;
        //                        balanceSheetClass.Billing_Date = eachbilling.Date.Date;
        //                        balanceSheetClass.Billing_Amount_paid = eachbilling.Amount_paid;
        //                        balanceSheetClass.Billing_Balance = eachbilling.Balance;
        //                        balanceSheetClass.Billing_Total_tax = echbilldet.Pieces * eachstocks.Selling_Price * ((eachstocks.Selling_CGST + eachstocks.Selling_SGST) / 100);
        //                        balanceSheetClassList.Add(balanceSheetClass);
        //                        break;
        //                    }
        //                }

        //            }
        //        }
        //    }

        //    var previousDateTime = nowDate.AddDays(-1);
        //    var billingallPreviousDay = db.Billing_Masters.Where(x => x.Date == previousDateTime).Distinct().ToList();
        //    var billingDetallPreviousDay = db.Billing_Details.Where(x => x.Date == previousDateTime).Distinct().ToList();

        //    foreach (var eachbilling in billingallPreviousDay)
        //    {
        //        balanceSheetClass = new BalanceSheetClass();
        //        foreach (var echbilldet in billingDetallPreviousDay)
        //        {
        //            if (eachbilling.Billing_Number == echbilldet.Billing_number)
        //            {
        //                foreach (var eachstocks in stockDetall)
        //                {
        //                    if (echbilldet.Selling_item_token == eachstocks.Token_Number)
        //                    {
        //                        crryForwrd = crryForwrd + eachbilling.Amount_paid;
        //                        break;
        //                    }
        //                }

        //            }
        //        }
        //    }

        //    var purchaseall = db.Purchase_Masters.Where(x => x.Date <= balanceSheetClass.EDate && x.Date >= balanceSheetClass.SDate).Distinct().ToList();
        //    foreach (var eachpurchase in purchaseall)
        //    {
        //        balanceSheetClass = new BalanceSheetClass();
        //        balanceSheetClass.Purchase_Number = eachpurchase.Purchase_Number;
        //        balanceSheetClass.Purchase_Date = eachpurchase.Date.Date;
        //        balanceSheetClass.Purchase_CGST = eachpurchase.CGST;
        //        balanceSheetClass.Purchase_SGST = eachpurchase.SGST;
        //        balanceSheetClass.Purchase_Total_amount = eachpurchase.Total_amount;
        //        balanceSheetClassList.Add(balanceSheetClass);
        //    }

        //    var empsalExpeseall = db.Employee_salary_expenses.Where(x => x.Date <= balanceSheetClass.EDate && x.Date >= balanceSheetClass.SDate).Distinct().ToList();
        //    foreach (var empsalExpese in empsalExpeseall)
        //    {
        //        balanceSheetClass = new BalanceSheetClass();
        //        balanceSheetClass.ExpenseId = empsalExpese.ExpenseId;
        //        balanceSheetClass.ExpenseTotal = empsalExpese.salary_to_be_paid;
        //        balanceSheetClass.ExenseDate = empsalExpese.Date;
        //        balanceSheetClass.ExpenseTotalGST = 0;
        //        balanceSheetClass.ExpenseBalance = 0;
        //        balanceSheetClass.Expense_Details = empsalExpese.ExpenseId+" : "+ "Employee salary expense"+" : "+ empsalExpese.Employee_name;
        //        balanceSheetClassList.Add(balanceSheetClass);
        //    }
        //    var otherexpenesall = db.Other_expenses.Where(x => x.Date <= balanceSheetClass.EDate && x.Date >= balanceSheetClass.SDate).Distinct().ToList();
        //    foreach (var otherexpense in otherexpenesall)
        //    {
        //        balanceSheetClass = new BalanceSheetClass();
        //        balanceSheetClass.ExpenseId = otherexpense.ExpenseId;
        //        balanceSheetClass.Expense_Details = otherexpense.ExpenseId + " : " + "Other expense" + " : " + otherexpense.Other_expense_type;
        //        balanceSheetClass.ExenseDate =otherexpense.Date;
        //        balanceSheetClass.ExpenseTotal = 0;
        //        balanceSheetClass.ExpenseBalance = 0;
        //        balanceSheetClass.ExpenseTotal = otherexpense.Other_expense_amount;
        //        balanceSheetClassList.Add(balanceSheetClass);
        //    }
        //    var productexpenseall = db.Product_expenses.Where(x => x.Date <= balanceSheetClass.EDate && x.Date >= balanceSheetClass.SDate).Distinct().ToList();
        //    foreach (var productexpense in productexpenseall)
        //    {
        //        balanceSheetClass = new BalanceSheetClass();
        //        balanceSheetClass.ExpenseId = productexpense.ExpenseId;
        //        balanceSheetClass.ExpenseTotal = productexpense.Amount_for_expense;
        //        balanceSheetClass.ExenseDate = productexpense.Date;
        //        balanceSheetClass.ExpenseTotalGST = 0;
        //        balanceSheetClass.ExpenseBalance = 0;
        //        balanceSheetClass.Expense_Details = productexpense.ExpenseId + " : " + "Product expense" + " : " + productexpense.Product_name;
        //        balanceSheetClassList.Add(balanceSheetClass);
        //    }

        //    var orderall = db.Order_Masters.Where(x => x.Date <= balanceSheetClass.EDate && x.Date >= balanceSheetClass.SDate).Distinct().ToList();
        //    var echorderdetall = db.Order_Details.Where(x => x.Date <= balanceSheetClass.EDate && x.Date >= balanceSheetClass.SDate).Distinct().ToList();
        //    foreach (var eachorder in orderall)
        //    {
        //        balanceSheetClass = new BalanceSheetClass();
        //        foreach (var echorderdet in echorderdetall)
        //        {
        //            if (eachorder.Order_Number == echorderdet.Order_number)
        //            {
        //                foreach (var eachstocks in stockDetall)
        //                {
        //                    if (echorderdet.Selling_item_token == eachstocks.Token_Number)
        //                    {
        //                        foreach (var eachcust in custlist)
        //                        {
        //                            if (eachorder.Customer_token_number == eachcust.Token_number)
        //                            {
        //                                balanceSheetClass.Order_Details = eachorder.Order_Number + " : " + eachcust.Customer_Name + " : "
        //                                    + eachstocks.Product_name + " : " + eachcust.Vehicle_type + " : " + eachcust.Vehicle_number + " : " + eachstocks.Tyre_make + " : " + eachstocks.Tyre_feel + " : " +
        //                           eachstocks.Tyre_type;
        //                                break;
        //                            }
        //                        }
        //                        balanceSheetClass.Order_Amount_paid = eachorder.Amount_paid;
        //                        balanceSheetClass.Order_Balance = eachorder.Balance;
        //                        balanceSheetClass.Order_Date = eachorder.Date.Date;
        //                        balanceSheetClass.Order_Number = eachorder.Order_Number;
        //                        balanceSheetClass.Order_Total = eachorder.Total_amount;
        //                        balanceSheetClass.Order_Total_tax = echorderdet.Pieces * eachstocks.Selling_Price * ((eachstocks.Selling_CGST + eachstocks.Selling_SGST) / 100);
        //                        balanceSheetClassList.Add(balanceSheetClass);
        //                        break;
        //                    }
        //                }

        //            }
        //        }
        //    }

        //    var orderallPreviousDay = db.Order_Masters.Where(x => x.Date == previousDateTime).Distinct().ToList();
        //    var echorderdetallPreviousDay = db.Order_Details.Where(x => x.Date == previousDateTime).Distinct().ToList();
        //    foreach (var eachorder in orderallPreviousDay)
        //    {
        //        balanceSheetClass = new BalanceSheetClass();
        //        foreach (var echorderdet in echorderdetallPreviousDay)
        //        {
        //            if (eachorder.Order_Number == echorderdet.Order_number)
        //            {
        //                foreach (var eachstocks in stockDetall)
        //                {
        //                    if (echorderdet.Selling_item_token == eachstocks.Token_Number)
        //                    {
        //                        crryForwrd = crryForwrd + eachorder.Amount_paid;
        //                        break;
        //                    }
        //                }

        //            }
        //        }
        //    }
        //    if (crryForwrd > decimal.Zero)
        //    {
        //        balanceSheetClass = new BalanceSheetClass();
        //        balanceSheetClass.CarryForwardDate = previousDateTime;
        //        balanceSheetClass.CarryForward = crryForwrd;
        //        balanceSheetClassList.Add(balanceSheetClass);
        //    }

        //    balanceSheetClass = new BalanceSheetClass();
        //    balanceSheetClass.TotalincomeCGST = 0;
        //    balanceSheetClass.TotalincomeSGST = 0;
        //    balanceSheetClass.TotalExpensesCGST = 0;
        //    balanceSheetClass.TotalExpensesSGST = 0;
        //    balanceSheetClass.TotalIncmBalance = 0;
        //    balanceSheetClass.TotalExpBalance = 0;
        //    foreach (var echbalsheet in balanceSheetClassList)
        //    {
        //        balanceSheetClass.TotalincomeCGST = balanceSheetClass.TotalincomeCGST + (echbalsheet.Billing_Total_tax / 2) + (echbalsheet.Order_Total_tax / 2);
        //        balanceSheetClass.TotalincomeSGST = balanceSheetClass.TotalincomeSGST + (echbalsheet.Billing_Total_tax / 2) + (echbalsheet.Order_Total_tax / 2);
        //        balanceSheetClass.TotalExpensesCGST = balanceSheetClass.TotalExpensesCGST + echbalsheet.Purchase_CGST;
        //        balanceSheetClass.TotalExpensesSGST = balanceSheetClass.TotalExpensesSGST + echbalsheet.Purchase_SGST;
        //        balanceSheetClass.TotalIncmBalance = balanceSheetClass.TotalIncmBalance + echbalsheet.Billing_Total + echbalsheet.Order_Total;
        //        balanceSheetClass.TotalExpBalance = balanceSheetClass.TotalExpBalance + echbalsheet.ExpenseTotal;
        //    }
        //    balanceSheetClass.TotalBalance = balanceSheetClass.TotalIncmBalance - balanceSheetClass.TotalExpBalance;
        //    balanceSheetClassList.Add(balanceSheetClass);
        //    return View(balanceSheetClassList);
        //}
        [Authorize]
        public void GetPurchaseBalancesheetRpt()
        {
                EasyBillingEntities db = new EasyBillingEntities();
                var Billing_Masters = (from i in db.Billing_Masters
                                       select i).Distinct().ToList();
                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[5] {
                            new DataColumn("No", typeof(int)),
                            new DataColumn("Invoice number", typeof(string)),
                            new DataColumn("Date", typeof(string)),
                            new DataColumn("Paid amount", typeof(int)),
                            new DataColumn("Balance", typeof(decimal))
                           });

                int count = 0;
                if (Billing_Masters.Count>0)
                {
                foreach (var t in Billing_Masters)
                    {
                        count++;
                        dt.Rows.Add(count, t.Billing_Number, t.Date, t.Amount_paid, t.Balance);
                       
                    }

                    using (StringWriter sw = new StringWriter())
                    {
                        using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                        {
                            StringBuilder sb = new StringBuilder();
                            
                            //Generate Invoice (Bill) Items Grid.
                            sb.Append("<table border = '1'>");
                            sb.Append("<tr style='font-size:10px'>");
                            foreach (DataColumn column in dt.Columns)
                            {
                                if (column.ColumnName == "No")
                                    sb.Append("<th width='4%'>");
                                else
                                    sb.Append("<th>");
                                sb.Append(column.ColumnName);
                                sb.Append("</th>");
                            }
                            sb.Append("</tr>");
                            foreach (DataRow row in dt.Rows)
                            {
                                sb.Append("<tr style='font-size:7px'>");
                                foreach (DataColumn column in dt.Columns)
                                {
                                    sb.Append("<td>");
                                    sb.Append(row[column]);
                                    sb.Append("</td>");
                                }
                                sb.Append("</tr>");
                            }
                            sb.Append("</table>");

                            //Export HTML String as PDF.
                            StringReader sr = new StringReader(sb.ToString());
                            Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
                            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                            PdfWriter writer = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                            pdfDoc.Open();
                            htmlparser.Parse(sr);
                            pdfDoc.Close();
                            Response.ContentType = "application/pdf";
                            Response.AddHeader("content-disposition", "attachment;filename=Invoice_balanceSheet.pdf");
                            Response.Cache.SetCacheability(HttpCacheability.NoCache);
                            Response.Write(pdfDoc);
                            Response.End();
                        }
                    }
               
                }
                else
                {
                    var response = new HttpResponseMessage(HttpStatusCode.NotFound);
                    // return response;
                }
           
        }
        public void GetOrderBalancesheetRpt()
        {
            EasyBillingEntities db = new EasyBillingEntities();
            var order_Masters = (from i in db.Order_Masters
                                   select i).Distinct().ToList();
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[5] {
                            new DataColumn("No", typeof(int)),
                            new DataColumn("Order number", typeof(string)),
                            new DataColumn("Date", typeof(string)),
                            new DataColumn("Paid amount", typeof(int)),
                            new DataColumn("Balance", typeof(decimal))
                           });

            int count = 0;
            if (order_Masters.Count > 0)
            {
                foreach (var t in order_Masters)
                {
                    count++;
                    dt.Rows.Add(count, t.Order_Number, t.Date, t.Amount_paid, t.Balance);

                }

                using (StringWriter sw = new StringWriter())
                {
                    using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                    {
                        StringBuilder sb = new StringBuilder();

                        //Generate Invoice (Bill) Items Grid.
                        sb.Append("<table border = '1'>");
                        sb.Append("<tr style='font-size:10px'>");
                        foreach (DataColumn column in dt.Columns)
                        {
                            if (column.ColumnName == "No")
                                sb.Append("<th width='4%'>");
                            else
                                sb.Append("<th>");
                            sb.Append(column.ColumnName);
                            sb.Append("</th>");
                        }
                        sb.Append("</tr>");
                        foreach (DataRow row in dt.Rows)
                        {
                            sb.Append("<tr style='font-size:7px'>");
                            foreach (DataColumn column in dt.Columns)
                            {
                                sb.Append("<td>");
                                sb.Append(row[column]);
                                sb.Append("</td>");
                            }
                            sb.Append("</tr>");
                        }
                        sb.Append("</table>");

                        //Export HTML String as PDF.
                        StringReader sr = new StringReader(sb.ToString());
                        Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
                        HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                        PdfWriter writer = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                        pdfDoc.Open();
                        htmlparser.Parse(sr);
                        pdfDoc.Close();
                        Response.ContentType = "application/pdf";
                        Response.AddHeader("content-disposition", "attachment;filename=Order_balanceSheet.pdf");
                        Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        Response.Write(pdfDoc);
                        Response.End();
                    }
                }

            }
            else
            {
                var response = new HttpResponseMessage(HttpStatusCode.NotFound);
                // return response;
            }

        }
        public ActionResult StockReportRpt()
        {
            return View();
        }
        public void GetStockReportRpt(StockReportClass stockReportClass, string isall)
        {
            string reportname = null;
            EasyBillingEntities db = new EasyBillingEntities();
            var stocks = new List<Products_For_Sale>();
            var flag = false;

            if(isall=="true")
            {
                flag = true;
                reportname = "All stock reports";
                stocks = (from i in db.Products_For_Sales
                              where i.Pieces > 0 && i.Purchase_Price > 1
                          select i).Distinct().ToList();
            }
            if (!string.IsNullOrEmpty(stockReportClass.Tubes) && stockReportClass.Tubes.ToLower()=="all")
            {
                flag = true;
                reportname = "All tube releted stock report";
                stocks = (from i in db.Products_For_Sales
                          join itmtube in db.Item_Tubes on i.Item_tyre_Id equals itmtube.Item_Id
                          where i.Pieces > 0 && i.Purchase_Price > 1
                          select i).Distinct().ToList();
            }
            if (!string.IsNullOrEmpty(stockReportClass.Product_type))
            {
                if (stockReportClass.Product_type.ToLower().Contains("all"))
                {
                    flag = true;
                    reportname = "All product releted stock report";
                    stocks = (from i in db.Products_For_Sales
                              join prodct in db.Other_Products on i.Item_tyre_Id equals prodct.Product_id
                              where i.Pieces > 0 && i.Purchase_Price > 1
                              select i).Distinct().ToList();
                }
                else
                {
                    flag = false;
                    reportname = stockReportClass.Product_type +" releted product stock report";
                    stocks = (from i in db.Products_For_Sales
                              join prodct in db.Other_Products on i.Item_tyre_Id equals prodct.Product_id
                              where i.Pieces > 0 && prodct.Product_type == stockReportClass.Product_type && i.Purchase_Price > 1
                              select i).Distinct().ToList();
                }
            }
            else if (!string.IsNullOrEmpty(stockReportClass.Tyre_make) && !string.IsNullOrEmpty(stockReportClass.Tyre_feel) && !string.IsNullOrEmpty(stockReportClass.Tyre_type))
            {
                if (stockReportClass.Tyre_make.ToLower().Contains("all") && stockReportClass.Tyre_type.ToLower().Contains("all") && stockReportClass.Tyre_feel.ToLower().Contains("all"))
                {
                    flag = true;
                    reportname = "All tyre releted stock report";
                    stocks = (from i in db.Products_For_Sales
                              join itmtyre in db.Item_Tyres on i.Item_tyre_Id equals itmtyre.Item_Id
                              where i.Pieces > 0 && i.Purchase_Price > 1
                              select i).Distinct().ToList();
                }
                else if (!stockReportClass.Tyre_make.ToLower().Contains("all") && !stockReportClass.Tyre_type.ToLower().Contains("all") && !stockReportClass.Tyre_feel.ToLower().Contains("all"))
                {
                    flag = false;
                    reportname = "Tyre releted stock report";
                    stocks = (from i in db.Products_For_Sales
                              join itmtyre in db.Item_Tyres on i.Item_tyre_Id equals itmtyre.Item_Id
                              where i.Pieces > 0 && i.Purchase_Price > 1 && itmtyre.Tyre_make == stockReportClass.Tyre_make && itmtyre.Tyre_feel == stockReportClass.Tyre_feel && itmtyre.Tyre_type == stockReportClass.Tyre_type
                              select i).Distinct().ToList();
                }
                else if (stockReportClass.Tyre_make.ToLower().Contains("all") && !stockReportClass.Tyre_type.ToLower().Contains("all") && !stockReportClass.Tyre_feel.ToLower().Contains("all"))
                {
                    flag = false;
                    reportname = "Tyre releted stock report";
                    stocks = (from i in db.Products_For_Sales
                              join itmtyre in db.Item_Tyres on i.Item_tyre_Id equals itmtyre.Item_Id
                              where i.Pieces > 0 && i.Purchase_Price > 1 && itmtyre.Tyre_feel == stockReportClass.Tyre_feel && itmtyre.Tyre_type == stockReportClass.Tyre_type
                              select i).Distinct().ToList();
                }
                else if (!stockReportClass.Tyre_make.ToLower().Contains("all") && stockReportClass.Tyre_type.ToLower().Contains("all") && !stockReportClass.Tyre_feel.ToLower().Contains("all"))
                {
                    flag = false;
                    reportname = "Tyre releted stock report";
                    stocks = (from i in db.Products_For_Sales
                              join itmtyre in db.Item_Tyres on i.Item_tyre_Id equals itmtyre.Item_Id
                              where i.Pieces > 0 && i.Purchase_Price > 1 && itmtyre.Tyre_make == stockReportClass.Tyre_make && itmtyre.Tyre_feel == stockReportClass.Tyre_feel 
                              select i).Distinct().ToList();
                }
                else if (!stockReportClass.Tyre_make.ToLower().Contains("all") && !stockReportClass.Tyre_type.ToLower().Contains("all") && stockReportClass.Tyre_feel.ToLower().Contains("all"))
                {
                    flag = false;
                    reportname = "Tyre releted stock report";
                    stocks = (from i in db.Products_For_Sales
                              join itmtyre in db.Item_Tyres on i.Item_tyre_Id equals itmtyre.Item_Id
                              where i.Pieces > 0 && i.Purchase_Price > 1 && itmtyre.Tyre_make == stockReportClass.Tyre_make && itmtyre.Tyre_type == stockReportClass.Tyre_type
                              select i).Distinct().ToList();
                }
                else if (!stockReportClass.Tyre_make.ToLower().Contains("all") && !stockReportClass.Tyre_type.ToLower().Contains("all") && stockReportClass.Tyre_feel.ToLower().Contains("all"))
                {
                    flag = false;
                    reportname = "Tyre releted stock report";
                    stocks = (from i in db.Products_For_Sales
                              join itmtyre in db.Item_Tyres on i.Item_tyre_Id equals itmtyre.Item_Id
                              where i.Pieces > 0 && i.Purchase_Price > 1 && itmtyre.Tyre_make == stockReportClass.Tyre_make && itmtyre.Tyre_type == stockReportClass.Tyre_type
                              select i).Distinct().ToList();
                }
                else if (!stockReportClass.Tyre_make.ToLower().Contains("all") && stockReportClass.Tyre_type.ToLower().Contains("all") && !stockReportClass.Tyre_feel.ToLower().Contains("all"))
                {
                    flag = false;
                    reportname = "Tyre releted stock report";
                    stocks = (from i in db.Products_For_Sales
                              join itmtyre in db.Item_Tyres on i.Item_tyre_Id equals itmtyre.Item_Id
                              where i.Pieces > 0 && i.Purchase_Price > 1 && itmtyre.Tyre_make == stockReportClass.Tyre_make && itmtyre.Tyre_feel == stockReportClass.Tyre_feel
                              select i).Distinct().ToList();
                }
                else if (stockReportClass.Tyre_make.ToLower().Contains("all") && !stockReportClass.Tyre_type.ToLower().Contains("all") && !stockReportClass.Tyre_feel.ToLower().Contains("all"))
                {
                    flag = false;
                    reportname = "Tyre releted stock report";
                    stocks = (from i in db.Products_For_Sales
                              join itmtyre in db.Item_Tyres on i.Item_tyre_Id equals itmtyre.Item_Id
                              where i.Pieces > 0 && i.Purchase_Price > 1 && itmtyre.Tyre_feel == stockReportClass.Tyre_feel && itmtyre.Tyre_type == stockReportClass.Tyre_type
                              select i).Distinct().ToList();
                }
            }
          
            DataTable dt = new DataTable();
            //dt.Columns.AddRange(new DataColumn[10] {
            //                new DataColumn("SL.no", typeof(int)),
            //                 new DataColumn("Itemid", ty7peof(string)),
            //                 new DataColumn("Details", typeof(string)),
            //                new DataColumn("Total stock", typeof(string)),
            //                new DataColumn("Buying CGST", typeof(string)),
            //                new DataColumn("Buying SGST", typeof(int)),
            //                new DataColumn("Buying Rate", typeof(decimal)),
            //                 new DataColumn("Selling CGST", typeof(string)),
            //                new DataColumn("Selling SGST", typeof(int)),
            //                new DataColumn("Selling Rate", typeof(decimal))
            //               });
            dt.Columns.AddRange(new DataColumn[12] {
                            new DataColumn("SL.no", typeof(int)),
                             new DataColumn("Itemid", typeof(string)),
                             new DataColumn("Details", typeof(string)),
                            new DataColumn("Total stock", typeof(string)),
                            new DataColumn("Buying Rate(Rs.)", typeof(decimal)),
                            new DataColumn("Buying CGST(%)", typeof(decimal)),
                            new DataColumn("Buying SGST(%)", typeof(decimal)),
                             new DataColumn("Buying Total(Rs.)", typeof(decimal)),
                              new DataColumn("Selling Rate(Rs.)", typeof(decimal)),
                            new DataColumn("Selling CGST(%)", typeof(decimal)),
                            new DataColumn("Selling SGST(%)", typeof(decimal)),
                             new DataColumn("Selling Total(Rs.)", typeof(decimal))
                           });
            int count = 0;
            if (stocks.Count > 0)
            {
                if (flag == false)
                {
                    foreach (var t in stocks)
                    {
                        count++;
                        dt.Rows.Add(count, t.Item_tyre_Id, t.Product_name + " " + t.Tyre_Size + " " + t.Tyre_type + " " + t.Tyre_feel + " " + t.Tyre_make + " " + t.Vehicle_type, (from ord in db.Products_For_Sales
                                                                                                                                                                                   where ord.Item_tyre_Id == t.Item_tyre_Id
                                                                                                                                                                                   select ord.Pieces).Sum(), t.Purchase_Price, t.CGST, t.SGST, t.Total, t.Selling_Price, t.Selling_CGST, t.Selling_SGST, t.Selling_net_total);

                    }
                }
                using (StringWriter sw = new StringWriter())
                {
                    using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                    {
                        StringBuilder sb = new StringBuilder();

                        //Generate Invoice (Bill) Items Grid.
                        sb.Append("<table border = '1'>");
                      
                        sb.Append("<tr><td align='center' style='background-color: #18B5F0' colspan = '12'><b>"+ reportname+"</b></td></tr>");
                        sb.Append("<tr style='font-size:10px'>");
                      
                        
                        foreach (DataColumn column in dt.Columns)
                        {
                            if (column.ColumnName == "SL.no")
                                sb.Append("<th width='4%'>");
                            else
                                sb.Append("<th>");
                            sb.Append(column.ColumnName);
                            sb.Append("</th>");
                        }
                        sb.Append("</tr>");

                        var rowcount = dt.Rows.Count;
                        var ttlpcs = 0;
                        var ttlprchseprice = 0.00;
                        var ttlpurchasecgst = 0.00;
                        var ttlpurchasesgst = 0.00;
                        var grndttlpurchase = 0.00;
                        var ttlsellingprice = 0.00;
                        var ttlsellingcgst = 0.00;
                        var ttlsellingsgst = 0.00;
                        var grndttlselling = 0.00;

                        if (flag == false)
                        {
                            foreach (DataRow row in dt.Rows)
                            {
                                sb.Append("<tr style='font-size:7px'>");
                                foreach (DataColumn column in dt.Columns)
                                {
                                    sb.Append("<td>");
                                    sb.Append(row[column]);
                                    sb.Append("</td>");
                                    if (column.ToString() == "Total stock")
                                        ttlpcs = ttlpcs + int.Parse(row[column].ToString());
                                    if (column.ToString() == "Buying Rate(Rs.)")
                                        ttlprchseprice = ttlprchseprice + Math.Round(float.Parse(row[column].ToString()), 2);
                                    if (column.ToString() == "Buying CGST(%)")
                                        ttlpurchasecgst = ttlpurchasecgst + Math.Round(float.Parse(row[column].ToString()), 2);
                                    if (column.ToString() == "Buying SGST(%)")
                                        ttlpurchasesgst = ttlpurchasesgst + Math.Round(float.Parse(row[column].ToString()), 2);
                                    if (column.ToString() == "Buying Total(Rs.)")
                                        grndttlpurchase = grndttlpurchase + Math.Round(float.Parse(row[column].ToString()), 2);
                                    if (column.ToString() == "Selling Rate(Rs.)")
                                        ttlsellingprice = ttlsellingprice + Math.Round(float.Parse(row[column].ToString()), 2);
                                    if (column.ToString() == "Selling CGST(%)")
                                        ttlsellingcgst = ttlsellingcgst + Math.Round(float.Parse(row[column].ToString()), 2);
                                    if (column.ToString() == "Selling SGST(%)")
                                        ttlsellingsgst = ttlsellingsgst + Math.Round(float.Parse(row[column].ToString()), 2);
                                    if (column.ToString() == "Selling Total(Rs.)")
                                    {
                                        if (row[column].ToString() == "")
                                            row[column] = 0;
                                        grndttlselling = grndttlselling + Math.Round(float.Parse(row[column].ToString()), 2);
                                    }
                                }
                                sb.Append("</tr>");
                            }
                            dt.Rows.Clear();
                            dt.Rows.Add(rowcount, "Total", "", ttlpcs, ttlprchseprice, ttlpurchasecgst, ttlpurchasesgst, grndttlpurchase, ttlsellingprice, ttlsellingcgst, ttlsellingsgst, grndttlselling);


                            foreach (DataRow row in dt.Rows)
                            {
                                sb.Append("<tr style='font-size:7px'>");
                                foreach (DataColumn column in dt.Columns)
                                {
                                    sb.Append("<td>");
                                    sb.Append(row[column]);
                                    sb.Append("</td>");
                                }
                                sb.Append("</tr>");
                            }
                        }else
                        {
                            var grndttlforrowcount = 0;
                            var grndttlforttlpcs = 0;
                            var grndttlforttlprchseprice = 0.00;
                            var grndttlforttlpurchasecgst = 0.00;
                            var grndttlforttlpurchasesgst = 0.00;
                            var grndttlforgrndttlpurchase = 0.00;
                            var grndttlforttlsellingprice = 0.00;
                            var grndttlforttlsellingcgst = 0.00;
                            var grndttlforttlsellingsgst = 0.00;
                            var grndttlforgrndttlselling = 0.00;

                            ///////================tyre============================================
                            count = 0;
                            dt.Rows.Clear();
                            if (stocks.Where(t => t.Item_tyre_Id.ToLower().StartsWith("ty")).ToList().Count > 0)
                            {
                                foreach (var t in stocks.Where(t => t.Item_tyre_Id.ToLower().StartsWith("ty")))
                                {
                                    count++;
                                    dt.Rows.Add(count, t.Item_tyre_Id, t.Product_name + " " + t.Tyre_Size + " " + t.Tyre_type + " " + t.Tyre_feel + " " + t.Tyre_make + " " + t.Vehicle_type, (from ord in db.Products_For_Sales
                                                                                                                                                                                               where ord.Item_tyre_Id == t.Item_tyre_Id
                                                                                                                                                                                               select ord.Pieces).Sum(), t.Purchase_Price, t.CGST, t.SGST, t.Total, t.Selling_Price, t.Selling_CGST, t.Selling_SGST, t.Selling_net_total);

                                }
                                rowcount = dt.Rows.Count;
                                ttlpcs = 0;
                                ttlprchseprice = 0.00;
                                ttlpurchasecgst = 0.00;
                                ttlpurchasesgst = 0.00;
                                grndttlpurchase = 0.00;
                                ttlsellingprice = 0.00;
                                ttlsellingcgst = 0.00;
                                ttlsellingsgst = 0.00;
                                grndttlselling = 0.00;
                                sb.Append("<tr><td align='left' style='background-color: #18B5F0' colspan = '12'><b>Tyre</b></td></tr>");
                                foreach (DataRow row in dt.Rows)
                                {
                                    sb.Append("<tr style='font-size:7px'>");
                                    foreach (DataColumn column in dt.Columns)
                                    {
                                        sb.Append("<td>");
                                        sb.Append(row[column]);
                                        sb.Append("</td>");
                                        if (column.ToString() == "Total stock")
                                            ttlpcs = ttlpcs + int.Parse(row[column].ToString());
                                        if (column.ToString() == "Buying Rate(Rs.)")
                                            ttlprchseprice = ttlprchseprice + Math.Round(float.Parse(row[column].ToString()), 2);
                                        if (column.ToString() == "Buying CGST(%)")
                                            ttlpurchasecgst = ttlpurchasecgst + Math.Round(float.Parse(row[column].ToString()), 2);
                                        if (column.ToString() == "Buying SGST(%)")
                                            ttlpurchasesgst = ttlpurchasesgst + Math.Round(float.Parse(row[column].ToString()), 2);
                                        if (column.ToString() == "Buying Total(Rs.)")
                                            grndttlpurchase = grndttlpurchase + Math.Round(float.Parse(row[column].ToString()), 2);
                                        if (column.ToString() == "Selling Rate(Rs.)")
                                            ttlsellingprice = ttlsellingprice + Math.Round(float.Parse(row[column].ToString()), 2);
                                        if (column.ToString() == "Selling CGST(%)")
                                            ttlsellingcgst = ttlsellingcgst + Math.Round(float.Parse(row[column].ToString()), 2);
                                        if (column.ToString() == "Selling SGST(%)")
                                            ttlsellingsgst = ttlsellingsgst + Math.Round(float.Parse(row[column].ToString()), 2);
                                        if (column.ToString() == "Selling Total(Rs.)")
                                        {
                                            if (row[column].ToString() == "")
                                                row[column] = 0;
                                            grndttlselling = grndttlselling + Math.Round(float.Parse(row[column].ToString()), 2);
                                        }
                                    }
                                    sb.Append("</tr>");
                                }
                                 grndttlforrowcount = rowcount;
                                 grndttlforttlpcs = ttlpcs;
                                 grndttlforttlprchseprice = ttlprchseprice;
                                 grndttlforttlpurchasecgst = ttlpurchasecgst;
                                 grndttlforttlpurchasesgst = ttlpurchasesgst;
                                 grndttlforgrndttlpurchase = grndttlpurchase;
                                 grndttlforttlsellingprice = ttlsellingprice;
                                 grndttlforttlsellingcgst = ttlsellingcgst;
                                 grndttlforttlsellingsgst = ttlsellingsgst;
                                 grndttlforgrndttlselling = grndttlselling;


                                dt.Rows.Clear();
                                dt.Rows.Add(rowcount, "Total", "", ttlpcs, ttlprchseprice, ttlpurchasecgst, ttlpurchasesgst, grndttlpurchase, ttlsellingprice, ttlsellingcgst, ttlsellingsgst, grndttlselling);


                                foreach (DataRow row in dt.Rows)
                                {
                                    sb.Append("<tr style='font-size:7px'>");
                                    foreach (DataColumn column in dt.Columns)
                                    {
                                        sb.Append("<td>");
                                        sb.Append(row[column]);
                                        sb.Append("</td>");
                                    }
                                    sb.Append("</tr>");
                                }
                            }
                            ///////================tube============================================
                            count = 0;
                            dt.Rows.Clear();
                            if (stocks.Where(t => t.Item_tyre_Id.ToLower().StartsWith("tu")).ToList().Count > 0)
                            {
                                foreach (var t in stocks.Where(t => t.Item_tyre_Id.ToLower().StartsWith("tu")))
                                {
                                    count++;
                                    dt.Rows.Add(count, t.Item_tyre_Id, t.Product_name + " " + t.Tyre_Size + " " + t.Tyre_type + " " + t.Tyre_feel + " " + t.Tyre_make + " " + t.Vehicle_type, (from ord in db.Products_For_Sales
                                                                                                                                                                                               where ord.Item_tyre_Id == t.Item_tyre_Id
                                                                                                                                                                                               select ord.Pieces).Sum(), t.Purchase_Price, t.CGST, t.SGST, t.Total, t.Selling_Price, t.Selling_CGST, t.Selling_SGST, t.Selling_net_total);

                                }
                                rowcount = dt.Rows.Count;
                                ttlpcs = 0;
                                ttlprchseprice = 0.00;
                                ttlpurchasecgst = 0.00;
                                ttlpurchasesgst = 0.00;
                                grndttlpurchase = 0.00;
                                ttlsellingprice = 0.00;
                                ttlsellingcgst = 0.00;
                                ttlsellingsgst = 0.00;
                                grndttlselling = 0.00;
                                sb.Append("<tr><td align='left' style='background-color: #18B5F0' colspan = '12'><b>Tube</b></td></tr>");
                                foreach (DataRow row in dt.Rows)
                                {
                                    sb.Append("<tr style='font-size:7px'>");
                                    foreach (DataColumn column in dt.Columns)
                                    {
                                        sb.Append("<td>");
                                        sb.Append(row[column]);
                                        sb.Append("</td>");
                                        if (column.ToString() == "Total stock")
                                            ttlpcs = ttlpcs + int.Parse(row[column].ToString());
                                        if (column.ToString() == "Buying Rate(Rs.)")
                                            ttlprchseprice = ttlprchseprice + Math.Round(float.Parse(row[column].ToString()), 2);
                                        if (column.ToString() == "Buying CGST(%)")
                                            ttlpurchasecgst = ttlpurchasecgst + Math.Round(float.Parse(row[column].ToString()), 2);
                                        if (column.ToString() == "Buying SGST(%)")
                                            ttlpurchasesgst = ttlpurchasesgst + Math.Round(float.Parse(row[column].ToString()), 2);
                                        if (column.ToString() == "Buying Total(Rs.)")
                                            grndttlpurchase = grndttlpurchase + Math.Round(float.Parse(row[column].ToString()), 2);
                                        if (column.ToString() == "Selling Rate(Rs.)")
                                            ttlsellingprice = ttlsellingprice + Math.Round(float.Parse(row[column].ToString()), 2);
                                        if (column.ToString() == "Selling CGST(%)")
                                            ttlsellingcgst = ttlsellingcgst + Math.Round(float.Parse(row[column].ToString()), 2);
                                        if (column.ToString() == "Selling SGST(%)")
                                            ttlsellingsgst = ttlsellingsgst + Math.Round(float.Parse(row[column].ToString()), 2);
                                        if (column.ToString() == "Selling Total(Rs.)")
                                        {
                                            if (row[column].ToString() == "")
                                                row[column] = 0;
                                            grndttlselling = grndttlselling + Math.Round(float.Parse(row[column].ToString()), 2);
                                        }
                                    }
                                    sb.Append("</tr>");
                                }
                                grndttlforrowcount = grndttlforrowcount+rowcount;
                                grndttlforttlpcs = grndttlforttlpcs+ttlpcs;
                                grndttlforttlprchseprice = grndttlforttlprchseprice+ttlprchseprice;
                                grndttlforttlpurchasecgst = grndttlforttlpurchasecgst+ ttlpurchasecgst;
                                grndttlforttlpurchasesgst = grndttlforttlpurchasesgst+ttlpurchasesgst;
                                grndttlforgrndttlpurchase = grndttlforgrndttlpurchase+grndttlpurchase;
                                grndttlforttlsellingprice = grndttlforttlsellingprice+ttlsellingprice;
                                grndttlforttlsellingcgst = grndttlforttlsellingcgst+ttlsellingcgst;
                                grndttlforttlsellingsgst = grndttlforttlsellingsgst+ttlsellingsgst;
                                grndttlforgrndttlselling = grndttlforgrndttlselling+grndttlselling;

                                dt.Rows.Clear();
                                dt.Rows.Add(rowcount, "Total", "", ttlpcs, ttlprchseprice, ttlpurchasecgst, ttlpurchasesgst, grndttlpurchase, ttlsellingprice, ttlsellingcgst, ttlsellingsgst, grndttlselling);


                                foreach (DataRow row in dt.Rows)
                                {
                                    sb.Append("<tr style='font-size:7px'>");
                                    foreach (DataColumn column in dt.Columns)
                                    {
                                        sb.Append("<td>");
                                        sb.Append(row[column]);
                                        sb.Append("</td>");
                                    }
                                    sb.Append("</tr>");
                                }
                            }
                            //////////////////=================================product===================================
                            count = 0;
                            dt.Rows.Clear();
                            if (stocks.Where(t => t.Item_tyre_Id.ToLower().StartsWith("po")).ToList().Count > 0)
                            {
                                foreach (var t in stocks.Where(t => t.Item_tyre_Id.ToLower().StartsWith("po")))
                                {
                                    count++;
                                    dt.Rows.Add(count, t.Item_tyre_Id, t.Product_name + " " + t.Tyre_Size + " " + t.Tyre_type + " " + t.Tyre_feel + " " + t.Tyre_make + " " + t.Vehicle_type, (from ord in db.Products_For_Sales
                                                                                                                                                                                               where ord.Item_tyre_Id == t.Item_tyre_Id
                                                                                                                                                                                               select ord.Pieces).Sum(), t.Purchase_Price, t.CGST, t.SGST, t.Total, t.Selling_Price, t.Selling_CGST, t.Selling_SGST, t.Selling_net_total);

                                }
                                rowcount = dt.Rows.Count;
                                ttlpcs = 0;
                                ttlprchseprice = 0.00;
                                ttlpurchasecgst = 0.00;
                                ttlpurchasesgst = 0.00;
                                grndttlpurchase = 0.00;
                                ttlsellingprice = 0.00;
                                ttlsellingcgst = 0.00;
                                ttlsellingsgst = 0.00;
                                grndttlselling = 0.00;
                                sb.Append("<tr><td align='left' style='background-color: #18B5F0' colspan = '12'><b>Product</b></td></tr>");
                                foreach (DataRow row in dt.Rows)
                                {
                                    sb.Append("<tr style='font-size:7px'>");
                                    foreach (DataColumn column in dt.Columns)
                                    {
                                        sb.Append("<td>");
                                        sb.Append(row[column]);
                                        sb.Append("</td>");
                                        if (column.ToString() == "Total stock")
                                            ttlpcs = ttlpcs + int.Parse(row[column].ToString());
                                        if (column.ToString() == "Buying Rate(Rs.)")
                                            ttlprchseprice = ttlprchseprice + Math.Round(float.Parse(row[column].ToString()), 2);
                                        if (column.ToString() == "Buying CGST(%)")
                                            ttlpurchasecgst = ttlpurchasecgst + Math.Round(float.Parse(row[column].ToString()), 2);
                                        if (column.ToString() == "Buying SGST(%)")
                                            ttlpurchasesgst = ttlpurchasesgst + Math.Round(float.Parse(row[column].ToString()), 2);
                                        if (column.ToString() == "Buying Total(Rs.)")
                                            grndttlpurchase = grndttlpurchase + Math.Round(float.Parse(row[column].ToString()), 2);
                                        if (column.ToString() == "Selling Rate(Rs.)")
                                            ttlsellingprice = ttlsellingprice + Math.Round(float.Parse(row[column].ToString()), 2);
                                        if (column.ToString() == "Selling CGST(%)")
                                            ttlsellingcgst = ttlsellingcgst + Math.Round(float.Parse(row[column].ToString()), 2);
                                        if (column.ToString() == "Selling SGST(%)")
                                            ttlsellingsgst = ttlsellingsgst + Math.Round(float.Parse(row[column].ToString()), 2);
                                        if (column.ToString() == "Selling Total(Rs.)")
                                        {
                                            if (row[column].ToString() == "")
                                                row[column] = 0;
                                            grndttlselling = grndttlselling + Math.Round(float.Parse(row[column].ToString()), 2);
                                        }
                                    }
                                    sb.Append("</tr>");
                                }

                                grndttlforrowcount = grndttlforrowcount + rowcount;
                                grndttlforttlpcs = grndttlforttlpcs + ttlpcs;
                                grndttlforttlprchseprice = grndttlforttlprchseprice + ttlprchseprice;
                                grndttlforttlpurchasecgst = grndttlforttlpurchasecgst + ttlpurchasecgst;
                                grndttlforttlpurchasesgst = grndttlforttlpurchasesgst + ttlpurchasesgst;
                                grndttlforgrndttlpurchase = grndttlforgrndttlpurchase + grndttlpurchase;
                                grndttlforttlsellingprice = grndttlforttlsellingprice + ttlsellingprice;
                                grndttlforttlsellingcgst = grndttlforttlsellingcgst + ttlsellingcgst;
                                grndttlforttlsellingsgst = grndttlforttlsellingsgst + ttlsellingsgst;
                                grndttlforgrndttlselling = grndttlforgrndttlselling + grndttlselling;

                                dt.Rows.Clear();
                                dt.Rows.Add(rowcount, "Total", "", ttlpcs, ttlprchseprice, ttlpurchasecgst, ttlpurchasesgst, grndttlpurchase, ttlsellingprice, ttlsellingcgst, ttlsellingsgst, grndttlselling);


                                foreach (DataRow row in dt.Rows)
                                {
                                    sb.Append("<tr style='font-size:7px'>");
                                    foreach (DataColumn column in dt.Columns)
                                    {
                                        sb.Append("<td>");
                                        sb.Append(row[column]);
                                        sb.Append("</td>");
                                    }
                                    sb.Append("</tr>");
                                }
                            }
                            dt.Rows.Clear();
                            dt.Rows.Add(grndttlforrowcount, "Grand Total", "", grndttlforttlpcs, grndttlforttlprchseprice, grndttlforttlpurchasecgst, grndttlforttlpurchasesgst, 
                                grndttlforgrndttlpurchase, grndttlforttlsellingprice, grndttlforttlsellingcgst, grndttlforttlsellingsgst, grndttlforgrndttlselling);


                            foreach (DataRow row in dt.Rows)
                            {
                                sb.Append("<tr style='font-size:7px'>");
                                foreach (DataColumn column in dt.Columns)
                                {
                                    sb.Append("<td><b>");
                                    sb.Append(row[column]);
                                    sb.Append("</b></td>");
                                }
                                sb.Append("</tr>");
                            }
                        }
                        sb.Append("</table>");

                        //Export HTML String as PDF.
                        StringReader sr = new StringReader(sb.ToString());
                        Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
                        HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                        PdfWriter writer = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                        pdfDoc.Open();
                        htmlparser.Parse(sr);
                        pdfDoc.Close();
                        Response.ContentType = "application/pdf";
                        Response.AddHeader("content-disposition", "attachment;filename=Stockreport.pdf");
                        Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        Response.Write(pdfDoc);
                        Response.End();
                    }
                }

            }
            else
            {
                var response = new HttpResponseMessage(HttpStatusCode.NotFound);
                // return response;
            }

        }
        //[HttpPost]
        //public void GetStockReportRpt(string Token_number, string Product_type, string Tyre_make)
        //{
        //    EasyBillingEntities db = new EasyBillingEntities();
        //    var stocks = (from i in db.Products_For_Sales
        //                  where i.Pieces>0
        //                         select i).Distinct().ToList();
        //    DataTable dt = new DataTable();
        //    dt.Columns.AddRange(new DataColumn[9] {
        //                    new DataColumn("No", typeof(int)),
        //                     new DataColumn("Item/product", typeof(string)),
        //                    new DataColumn("No of stocks", typeof(string)),
        //                    new DataColumn("Buying CGST", typeof(string)),
        //                    new DataColumn("Buying SGST", typeof(int)),
        //                    new DataColumn("Buying Rate", typeof(decimal)),
        //                     new DataColumn("Selling CGST", typeof(string)),
        //                    new DataColumn("Selling SGST", typeof(int)),
        //                    new DataColumn("Selling Rate", typeof(decimal))
        //                   });

        //    int count = 0;
        //    if (stocks.Count > 0)
        //    {
        //        foreach (var t in stocks)
        //        {
        //            count++;
        //            dt.Rows.Add(count,t.Item_tyre_Id + " - " + t.Product_name, (from ord in db.Products_For_Sales
        //                                                                        where ord.Item_tyre_Id == t.Item_tyre_Id
        //                                                                        select ord.Pieces).Sum(), t.CGST, t.SGST, t.Purchase_Price, t.Selling_CGST, t.Selling_SGST, t.Selling_Price);

        //        }

        //        using (StringWriter sw = new StringWriter())
        //        {
        //            using (HtmlTextWriter hw = new HtmlTextWriter(sw))
        //            {
        //                StringBuilder sb = new StringBuilder();

        //                //Generate Invoice (Bill) Items Grid.
        //                sb.Append("<table border = '1'>");
                       
        //                sb.Append("<tr style='font-size:10px'>");
        //                foreach (DataColumn column in dt.Columns)
        //                {
        //                    if (column.ColumnName == "No")
        //                        sb.Append("<th width='4%'>");
        //                    else
        //                        sb.Append("<th>");
        //                    sb.Append(column.ColumnName);
        //                    sb.Append("</th>");
        //                }
        //                sb.Append("</tr>");
        //                foreach (DataRow row in dt.Rows)
        //                {
        //                    sb.Append("<tr style='font-size:7px'>");
        //                    foreach (DataColumn column in dt.Columns)
        //                    {
        //                        sb.Append("<td>");
        //                        sb.Append(row[column]);
        //                        sb.Append("</td>");
        //                    }
        //                    sb.Append("</tr>");
        //                }
        //                sb.Append("</table>");

        //                //Export HTML String as PDF.
        //                StringReader sr = new StringReader(sb.ToString());
        //                Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
        //                HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
        //                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
        //                pdfDoc.Open();
        //                htmlparser.Parse(sr);
        //                pdfDoc.Close();
        //                Response.ContentType = "application/pdf";
        //                Response.AddHeader("content-disposition", "attachment;filename=Stockreport.pdf");
        //                Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //                Response.Write(pdfDoc);
        //                Response.End();
        //            }
        //        }

        //    }
        //    else
        //    {
        //        var response = new HttpResponseMessage(HttpStatusCode.NotFound);
        //        // return response;
        //    }

        //}
        [HttpGet]
        public JsonResult AjaxGetCall()
        {
            Temp_Stock temp_Stock = new Temp_Stock();
            bool anyval = db.Temp_Stock.Any();
            if (!anyval)
            {
                temp_Stock.Total = 0;
                temp_Stock.Pieces =0;
                temp_Stock.ttlitms = 0;
            }
            else
            {
                temp_Stock.Total = db.Temp_Stock.Select(z => z.Total).Sum();
                temp_Stock.Pieces = db.Temp_Stock.Select(z => z.Pieces).Sum();
                temp_Stock.ttlitms = db.Temp_Stock.Select(z => z.Item_tyre_Id).Count();
            }
            return Json(temp_Stock, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetproductsforBill()
        {
            Temp_Bill temp_Bill = new Temp_Bill();
            bool anyval = db.Temp_Bill.Any();
            if (!anyval)
            {
                temp_Bill.Total = 0;
                temp_Bill.Pieces = 0;
                temp_Bill.itemscount = 0;
            }
            else
            {
                temp_Bill.Total = db.Temp_Bill.Select(z => z.Total).Sum();
                temp_Bill.Pieces = db.Temp_Bill.Select(z => z.Pieces).Sum();
                temp_Bill.itemscount = db.Temp_Bill.Select(z => z.Item_tyre_Id).Distinct().Count();
            }
           
            return Json(temp_Bill, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetSupplier(string term)
        {
            if (db.Temp_Stock.Any())
            {
                db.Temp_Stock.RemoveRange(db.Temp_Stock.ToList());

                db.SaveChanges();
            }
            List<string> suppname = (from dealer in db.Dealers

                                     where dealer.Name.Contains(term)
                                     select dealer.Name).Distinct().ToList();
            if (suppname.Count == 0)
            {
                suppname.Add("No supplier is matched with this entry...");
            }
            return Json(suppname, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCustname(string term)
        {
            List<string> custnms = (from custnm in db.Customers
                                    where custnm.Customer_Name.Contains(term)
                                    select custnm.Customer_Name).Distinct().ToList();
           
            if (custnms.Count == 0)
            {
                custnms.Add("No cusomer name is matched with this entry...");
            }
            return Json(custnms, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetMobileNo(string term)
        {
            List<string> custnos = (from custno in db.Customers
                                    where custno.Phone_number.Contains(term)
                                    select custno.Phone_number).Distinct().ToList();

            if (custnos.Count == 0)
            {
                custnos.Add("No cusomer number is matched with this entry...");
            }
            return Json(custnos, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetVehicleNo(string term,string mobile,string name)
        {
            List<string> custVnos = new List<string>();
            if (!string.IsNullOrEmpty(mobile) && !string.IsNullOrEmpty(name))
            {
                 custVnos = (from custVno in db.Customers
                                         where custVno.Vehicle_number.Contains(term) && custVno.Phone_number==mobile && custVno.Customer_Name == name
                             select custVno.Vehicle_number).Distinct().ToList();
            }else if (!string.IsNullOrEmpty(name))
            {
               custVnos = (from custVno in db.Customers
                                         where custVno.Vehicle_number.Contains(term) && custVno.Customer_Name == name
                           select custVno.Vehicle_number).Distinct().ToList();
            }else if (!string.IsNullOrEmpty(mobile))
            {
                custVnos = (from custVno in db.Customers
                                         where custVno.Vehicle_number.Contains(term) && custVno.Phone_number == mobile
                            select custVno.Vehicle_number).Distinct().ToList();
            }
            else
            {
                custVnos = (from custVno in db.Customers
                            where custVno.Vehicle_number.Contains(term)
                            select custVno.Vehicle_number).Distinct().ToList();
            }
            if (custVnos.Count == 0)
            {
                custVnos.Add("No cusomer vehicle number is matched with this entry...");
            }
            return Json(custVnos, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetItemId(string term)
        {
         
            List<string> temp_Stocks = (from temp in db.Temp_Stock
                                        select temp.Item_tyre_Id).Distinct().ToList();
            IEnumerable<string> itemidchk = (from item in db.Item_Tyres
                                  
                                   where item.Item_Id.Contains(term)
                                   select item.Item_Id).Distinct().ToList();
            List<string> itemid = itemidchk.Except(temp_Stocks).ToList();
            if (itemid.Count == 0)
            {
                itemid.Add("No item id is matched with this entry...");
            }
            return Json(itemid, JsonRequestBehavior.AllowGet);
        }
        
        public void GenerateInvoicePDF()
        {
            //Dummy data for Invoice (Bill).
            string companyName = "Kannantyres";
            int orderNo = 2303;
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[5] {
                            new DataColumn("ProductId", typeof(string)),
                            new DataColumn("Product", typeof(string)),
                            new DataColumn("Price", typeof(int)),
                            new DataColumn("Quantity", typeof(int)),
                            new DataColumn("Total", typeof(int))});
            dt.Rows.Add(101, "Sun Glasses", 200, 5, 1000);
            dt.Rows.Add(102, "Jeans", 400, 2, 800);
            dt.Rows.Add(103, "Trousers", 300, 3, 900);
            dt.Rows.Add(104, "Shirts", 550, 2, 1100);

            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                {
                    StringBuilder sb = new StringBuilder();

                    //Generate Invoice (Bill) Header.
                    sb.Append("<table width='100%' cellspacing='0'  cellpadding='2'>");
                    sb.Append("<tr><td align='center' style='background-color: #18B5F0' colspan = '2'><b>Invoice Sheet</b></td></tr>");
                    sb.Append("<tr><td colspan = '2'></td></tr>");
                    sb.Append("<tr><td><b>Order No: </b>");
                    sb.Append(orderNo);
                    sb.Append("</td><td align = 'right'><b>Date: </b>");
                    sb.Append(DateTime.Now);
                    sb.Append(" </td></tr>");
                    sb.Append("<tr><td colspan = '2'><b>Company Name: </b>");
                    sb.Append(companyName);
                    sb.Append("</td></tr>");
                    sb.Append("</table>");
                    sb.Append("<br />");

                    //Generate Invoice (Bill) Items Grid.
                    sb.Append("<table border = '1'>");
                    sb.Append("<tr>");
                    foreach (DataColumn column in dt.Columns)
                    {
                        sb.Append("<th style = 'background-color: #D20B0C;color:#ffffff'>");
                        sb.Append(column.ColumnName);
                        sb.Append("</th>");
                    }
                    sb.Append("</tr>");
                    foreach (DataRow row in dt.Rows)
                    {
                        sb.Append("<tr>");
                        foreach (DataColumn column in dt.Columns)
                        {
                            sb.Append("<td>");
                            sb.Append(row[column]);
                            sb.Append("</td>");
                        }
                        sb.Append("</tr>");
                    }
                    sb.Append("<tr><td align = 'right' colspan = '");
                    sb.Append(dt.Columns.Count - 1);
                    sb.Append("'>Total</td>");
                    sb.Append("<td>");
                    sb.Append(dt.Compute("sum(Total)", ""));
                    sb.Append("</td>");
                    sb.Append("</tr>");

                    sb.Append("</tr></table>");

                    //Export HTML String as PDF.
                    StringReader sr = new StringReader(sb.ToString());
                    Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
                    HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                    pdfDoc.Open();
                    htmlparser.Parse(sr);
                    pdfDoc.Close();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment;filename=Invoice_" + orderNo + ".pdf");
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.Write(pdfDoc);
                    Response.End();
                }
            }
        }
        public ActionResult GetBillingListRptForPrint(string Bllno)
        {
            string spath = System.Web.HttpContext.Current.Server.MapPath("~/kannanInvoice");
            //Process p = new Process();
            //Process.Start($@"http://etreesales.com\kannanInvoice\Invoice_{Bllno}.pdf");
            // Process.Start($@"{spath}\Invoice_{Bllno}.pdf","about:blank --d");
            //p.StartInfo = new ProcessStartInfo()
            //{
            //    CreateNoWindow = true,
            //    Verb = "print",
            //    FileName = @"http://localhost:8087/Invoice_" + Bllno + ".pdf" //put the correct path here
            //};
            //p.Start();
            return Redirect($"http://localhost:8087/kannanInvoice/Invoice_{Bllno}.pdf");
           // return Redirect($@"{spath}\Invoice_{Bllno}.pdf");
        }
        public void GetQuotationListRptForPrint(string Bllno)
        {
            string spath = System.Web.HttpContext.Current.Server.MapPath("~/kannanInvoice");
            Process p = new Process();
            Process.Start($@"{spath}\Invoice_{Bllno}.pdf", "about:blank --d");
            //p.StartInfo = new ProcessStartInfo()
            //{
            //    CreateNoWindow = true,
            //    Verb = "print",
            //    FileName = @"http://localhost:8087/Invoice_" + Bllno + ".pdf" //put the correct path here
            //};
            //p.Start();
        }
        public void GetOrderListRptForPrint(string Bllno)
        {
            string spath = System.Web.HttpContext.Current.Server.MapPath("~/kannanInvoice");
            Process p = new Process();
            Process.Start($@"{spath}\Invoice_{Bllno}.pdf", "about:blank --d");
            //p.StartInfo = new ProcessStartInfo()
            //{
            //    CreateNoWindow = true,
            //    Verb = "print",
            //    FileName = @"http://localhost:8087/Invoice_" + Bllno + ".pdf" //put the correct path here
            //};
            //p.Start();
        }
        public void GetBillingListRpt(string Bllno)
        {
            List<StockForBillingRPT> StockForBillingRPTlist = new List<StockForBillingRPT>();
            StockForBillingRPT StockForBillingRPT = new StockForBillingRPT();
            if (Bllno != null)
            {
                EasyBillingEntities db = new EasyBillingEntities();
                var Billing_Masters = (from i in db.Billing_Masters
                                       select i).Where(u => u.Billing_Number == Bllno).Distinct().FirstOrDefault();
                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[8] {
                            new DataColumn("No", typeof(int)),
                            new DataColumn("ItemId", typeof(string)),
                            new DataColumn("Details", typeof(string)),
                            new DataColumn("QTY", typeof(int)),
                            new DataColumn("Price", typeof(decimal)),
                            new DataColumn("CGST (in %)", typeof(string)),
                            new DataColumn("SGST (in %)", typeof(string)),
                           // new DataColumn("GST cal by", typeof(string)),
                            new DataColumn("Total", typeof(decimal))});

                int count = 0;
                if (Billing_Masters != null)
                {
                    db.StockForBillingRPTs.RemoveRange(db.StockForBillingRPTs.ToList());
                    db.SaveChanges();

                    var Billing_Details = (from i in db.Billing_Details
                                           select i).Where(u => u.Billing_Token_number == Billing_Masters.Token_Number).Distinct().ToList();

                    foreach (var t in Billing_Details)
                    {
                        count++;
                        Products_For_Sale prdctfrsl = db.Products_For_Sales.Where(r => r.Token_Number == t.Selling_item_token).Distinct().FirstOrDefault();
                        if (prdctfrsl == null)
                        {
                            var productAsService = db.Other_Products.Where(z=>z.Token_number==t.Selling_item_token).FirstOrDefault();
                            StockForBillingRPT.Token_Number = t.Selling_item_token;
                            StockForBillingRPT.Tyre_Size = "";
                            StockForBillingRPT.Selling_SGST = decimal.Zero;
                            StockForBillingRPT.Selling_Price = t.Sub_Total;
                            StockForBillingRPT.Selling_CGST = decimal.Zero;
                            StockForBillingRPT.Product_name = productAsService.Product_name;
                            StockForBillingRPT.Item_tyre_Id = t.Selling_item_id;
                            if (t.IsGstPercent == true)
                                dt.Rows.Add(count, t.Selling_item_id, StockForBillingRPT.Product_name + StockForBillingRPT.Tyre_Size, t.Pieces, StockForBillingRPT.Selling_Price, StockForBillingRPT.Selling_CGST.ToString(), StockForBillingRPT.Selling_SGST.ToString(), t.Sub_Total);
                            else if (prdctfrsl.IsGstPercent == false)
                                dt.Rows.Add(count, t.Selling_item_id, StockForBillingRPT.Product_name + StockForBillingRPT.Tyre_Size, t.Pieces, StockForBillingRPT.Selling_Price, StockForBillingRPT.Selling_CGST.ToString() + " (in Rs.)", StockForBillingRPT.Selling_SGST.ToString() + " (in Rs.)", t.Sub_Total);
                            else
                                dt.Rows.Add(count, t.Selling_item_id, StockForBillingRPT.Product_name + StockForBillingRPT.Tyre_Size, t.Pieces, StockForBillingRPT.Selling_Price, StockForBillingRPT.Selling_CGST.ToString(), StockForBillingRPT.Selling_SGST.ToString(), t.Sub_Total);
                        }
                        else
                        {
                            StockForBillingRPT.Token_Number = prdctfrsl.Token_Number;
                            StockForBillingRPT.Tyre_Size = prdctfrsl.Tyre_Size;
                            StockForBillingRPT.Selling_SGST = prdctfrsl.Selling_SGST;
                            StockForBillingRPT.Selling_Price = prdctfrsl.Selling_Price;
                            StockForBillingRPT.Selling_CGST = prdctfrsl.Selling_CGST;
                            StockForBillingRPT.Product_name = prdctfrsl.Product_name;
                            StockForBillingRPT.Item_tyre_Id = prdctfrsl.Item_tyre_Id;
                            if (prdctfrsl.IsGstPercent == true)
                                dt.Rows.Add(count, t.Selling_item_id, StockForBillingRPT.Product_name + StockForBillingRPT.Tyre_Size, t.Pieces, StockForBillingRPT.Selling_Price, StockForBillingRPT.Selling_CGST.ToString(), StockForBillingRPT.Selling_SGST.ToString(), t.Sub_Total);
                            else if (prdctfrsl.IsGstPercent == false)
                                dt.Rows.Add(count, t.Selling_item_id, StockForBillingRPT.Product_name + StockForBillingRPT.Tyre_Size, t.Pieces, StockForBillingRPT.Selling_Price, StockForBillingRPT.Selling_CGST.ToString() + " (in Rs.)", StockForBillingRPT.Selling_SGST.ToString() + " (in Rs.)", t.Sub_Total);
                            else
                                dt.Rows.Add(count, t.Selling_item_id, StockForBillingRPT.Product_name + StockForBillingRPT.Tyre_Size, t.Pieces, StockForBillingRPT.Selling_Price, StockForBillingRPT.Selling_CGST.ToString(), StockForBillingRPT.Selling_SGST.ToString(), t.Sub_Total);
                        }
                        StockForBillingRPTlist.Add(StockForBillingRPT);

                    }

                    db.SaveChanges();

                    var Billing_MastersList = (from i in db.Billing_Masters
                                               select i).Where(u => u.Token_Number == Billing_Masters.Token_Number).Distinct().ToList();
                    var Customers = (from i in db.Customers
                                     select i).Where(u => u.Token_number == Billing_Masters.Customer_token_number).Distinct().ToList();

                    db.StockForBillingRPTs.AddRange(StockForBillingRPTlist);

                    string orderNo = Billing_Masters.Billing_Number;

                    using (StringWriter sw = new StringWriter())
                    {
                        using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                        {
                            StringBuilder sb = new StringBuilder();
                            sb.Append("<!DOCTYPE html><html><head></head><body>");
                            sb.Append("<br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><table  cellspacing='0'  border = '1' >");
                            //sb.Append("<table  cellspacing='0'  border = '1' >");
                            //sb.Append("<tr><td colspan = '2' ><img width='578%' style='float:left' src = 'http://localhost:8087/new 022.JPG' /></td></tr>");

                            sb.Append("<tr ><td><b>Invoice No: </b>");
                            sb.Append(orderNo);
                            sb.Append("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" +
                                "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Date: " + DateTime.Now.ToString("dd MMM yyyy hh:mm tt"));

                            sb.Append(" </td></tr>");

                            sb.Append($"<tr><td align='left'><b>Customer name and address : {db.Customers.Where(z => z.Token_number == Billing_Masters.Customer_token_number).Select(z => z.Customer_Name).FirstOrDefault()} <br/>{db.Customers.Where(z => z.Token_number == Billing_Masters.Customer_token_number).Select(z => z.Address).FirstOrDefault()}<br/>Vehicle no. {db.Customers.Where(z => z.Token_number == Billing_Masters.Customer_token_number).Select(z => z.Vehicle_number).FirstOrDefault()} </b>");
                            sb.Append("</td>");

                            sb.Append("</tr>");
                            
                            sb.Append("</table>");
                            sb.Append("<br />");
                            sb.Append("<table border = '1'>");
                            sb.Append("<tr style='font-size:10px'>");
                            foreach (DataColumn column in dt.Columns)
                            {
                                if (column.ColumnName == "No")
                                    sb.Append("<th width='4%'>");
                                else
                                    sb.Append("<th>");
                                sb.Append(column.ColumnName);
                                sb.Append("</th>");
                            }
                            sb.Append("</tr>");
                            foreach (DataRow row in dt.Rows)
                            {
                                sb.Append("<tr style='font-size:7px'>");
                                foreach (DataColumn column in dt.Columns)
                                {

                                    sb.Append("<td>");

                                    sb.Append(row[column]);
                                    sb.Append("</td>");
                                }
                                sb.Append("</tr>");
                            }
                            sb.Append("<tr><td align = 'right' colspan = '");
                            sb.Append(dt.Columns.Count - 1);
                            if (Billing_Masters.Discountper == false)
                                sb.Append($"'>Discount({Billing_Masters.Discount} calculated as Rs.)</td>");
                            else
                                sb.Append($"'>Discount({Billing_Masters.Discount} calculated as %)</td>");
                            sb.Append("<td>");
                            sb.Append(Billing_Masters.Total_discount);
                            sb.Append("</td>");
                            sb.Append("</tr>");
                            sb.Append("<tr><td align = 'right' colspan = '");
                            sb.Append(dt.Columns.Count - 1);
                            sb.Append("'>Amount Paid</td>");
                            sb.Append("<td>");
                            sb.Append(Billing_Masters.Amount_paid);
                            sb.Append("</td>");
                            sb.Append("</tr>");


                            sb.Append("<tr><td align = 'right' colspan = '");
                            sb.Append(dt.Columns.Count - 1);
                            sb.Append("'>Balance</td>");
                            sb.Append("<td>");
                            sb.Append(Billing_Masters.Balance);

                            sb.Append("</td>");
                            sb.Append("</tr>");
                            var amtString= Billing_Masters.Amount_paid.ToString();
                           
                            long number1 = Convert.ToInt64(decimal.Parse(amtString.Split('.')[0]));
                            long number2 = Convert.ToInt64(decimal.Parse(amtString.Split('.')[1]));
                            string result1 = null;
                            string result2 = null;
                            if (number1 == 0)
                                result1 = "ZERO";
                            if (number1 < 0 && number2<0)
                                result1 = "minus " + ConvertNumbertoWords(Math.Abs(number1));
                            if (number2 < 0)
                                result2 = ConvertNumbertoWords(Math.Abs(number2));
                            string words1 = "";
                            if ((number1 / 1000000) > 0)
                            {
                                words1 += ConvertNumbertoWords(number1 / 100000) + " LAKES ";
                                number1 %= 1000000;
                            }
                            if ((number1 / 1000) > 0)
                            {
                                words1 += ConvertNumbertoWords(number1 / 1000) + " THOUSAND ";
                                number1 %= 1000;
                            }
                            if ((number1 / 100) > 0)
                            {
                                words1 += ConvertNumbertoWords(number1 / 100) + " HUNDRED ";
                                number1 %= 100;
                            }

                            if (number1 > 0 || !string.IsNullOrEmpty(words1))
                            {
                                if (words1 != "") words1 += "AND ";
                                var unitsMap = new[]
                                {
                                "ZERO", "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINE", "TEN", "ELEVEN", "TWELVE", "THIRTEEN", "FOURTEEN", "FIFTEEN", "SIXTEEN", "SEVENTEEN", "EIGHTEEN", "NINETEEN"
                            };
                                var tensMap = new[]
                                {
                                "ZERO", "TEN", "TWENTY", "THIRTY", "FORTY", "FIFTY", "SIXTY", "SEVENTY", "EIGHTY", "NINETY"
                            };
                                if (number1 < 20) words1 += unitsMap[number1];
                                else
                                {
                                    words1 += tensMap[number1 / 10];
                                    if ((number1 % 10) > 0) words1 += " " + unitsMap[number1 % 10];
                                }
                                result1 = words1;
                            }
                            if (number2 != 0)
                            {
                                string words2 = "";
                                if ((number2 / 1000000) > 0)
                                {
                                    words2 += ConvertNumbertoWords(number2 / 100000) + " LAKES ";
                                    number2 %= 1000000;
                                }
                                if ((number2 / 1000) > 0)
                                {
                                    words2 += ConvertNumbertoWords(number2 / 1000) + " THOUSAND ";
                                    number2 %= 1000;
                                }
                                if ((number2 / 100) > 0)
                                {
                                    words2 += ConvertNumbertoWords(number2 / 100) + " HUNDRED ";
                                    number2 %= 100;
                                }

                                if (number2 > 0 || !string.IsNullOrEmpty(words2))
                                {
                                    if (words2 != "") words2 += "AND ";
                                    var unitsMap = new[]
                                    {
                                "ZERO", "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINE", "TEN", "ELEVEN", "TWELVE", "THIRTEEN", "FOURTEEN", "FIFTEEN", "SIXTEEN", "SEVENTEEN", "EIGHTEEN", "NINETEEN"
                            };
                                    var tensMap = new[]
                                    {
                                "ZERO", "TEN", "TWENTY", "THIRTY", "FORTY", "FIFTY", "SIXTY", "SEVENTY", "EIGHTY", "NINETY"
                            };
                                    if (number2 < 20) words2 += unitsMap[number2];
                                    else
                                    {
                                        words2 += tensMap[number2 / 10];
                                        if ((number2 % 10) > 0) words2 += " " + unitsMap[number2 % 10];
                                    }
                                    result2 = words2;
                                }
                            }
                            sb.Append("<tr><td align = 'left' colspan = '");
                            sb.Append(dt.Columns.Count);
                            if(result2!=null)
                            sb.Append($"'>Amount paid in words : {result1} rupees and {result2} paisa only</td>");
                            else
                                sb.Append($"'>Amount paid in words : {result1} rupees only</td>");
                            sb.Append("</tr>");
                            sb.Append("<tr><td align = 'left' colspan = '");
                            sb.Append(dt.Columns.Count);
                            sb.Append($"'>Mode of payment : {Billing_Masters.Mode_of_payment}</td>");

                            sb.Append("</tr>");

                            sb.Append("<tr><td align = 'left' colspan = '");
                            sb.Append(dt.Columns.Count);
                            sb.Append($"'>Conditions : <br/>We won't give any gurrantee for the bill respective company will you give you the gurrantee</td>");
                            sb.Append("</tr>");
                            
                            var empid = db.Billing_Masters.Where(r => r.Billing_Number == Bllno).Select(x => x.User_Id).Distinct().FirstOrDefault();
                            if (db.Marchent_Accounts.Where(r => r.Email_Id == empid).Any())
                            {
                                sb.Append("<tr><td align = 'right' colspan = '");
                                sb.Append(dt.Columns.Count);
                                sb.Append($"' > For Kannan Tyres<br/><br/>{db.Marchent_Accounts.Where(r => r.Email_Id == empid).Select(x => x.Marchent_name).Distinct().FirstOrDefault()} , Admin<br/>Role name</td>");
                                sb.Append("</tr>");
                            }
                            else
                            {
                                sb.Append("<tr><td align = 'right' colspan = '");
                                sb.Append(dt.Columns.Count);
                                sb.Append($"' > For Kannan Tyres<br/><br/>{db.Employees.Where(r => r.Employee_Id == empid).Select(x => x.Employee_name).Distinct().FirstOrDefault()} , {db.Employees.Where(r => r.Employee_Id == empid).Select(x => x.Designation).Distinct().FirstOrDefault()}<br/>Role name</td>");
                                sb.Append("</tr>");
                            }
    
                            //sb.Append("<tr><td align = 'left' colspan = '");
                            //sb.Append(dt.Columns.Count);
                            //sb.Append($"'><img  style='float:left;width:25%' src = 'http://localhost:8087/new 02.JPG' /> </td>");

                            //sb.Append("</tr>");

                            sb.Append("</table><br/><br/><br/><br/><br/><br/>");
                            sb.Append("</body></html>");

                            //////////////////////////////////////////////////////////////////////////////

                            ///////////////////////////////////////////////////////////////////////////////////

                            StringReader sr = new StringReader(sb.ToString());
                            Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
                            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                            PdfWriter writer = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                            //XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                            //XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, new StringReader("<p>helloworld</p>"));
                            string spath = System.Web.HttpContext.Current.Server.MapPath("~/kannanInvoice");
                          //  System.IO.File.WriteAllLines(spath + "/Invoice_" + orderNo + ".pdf", pdfDoc);

                            PdfWriter.GetInstance(pdfDoc, new FileStream($@"{spath}\Invoice_{ orderNo }.pdf", FileMode.OpenOrCreate));
                            pdfDoc.Open();
                            // XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                            htmlparser.Parse(sr);
                            pdfDoc.Close();
                            Response.ContentType = "application/pdf";
                            Response.AddHeader("content-disposition", "attachment;filename=Invoice_" + orderNo + ".pdf");
                           
                            Response.Cache.SetCacheability(HttpCacheability.NoCache);
                            Response.Write(pdfDoc);
                           
                            Response.End();
                           
                            //var doc1 = new iTextSharp.text.Document();
                            //string path = Server.MapPath("PDF_Files");
                           



                        }
                    }

                }
                    //        else
                    //        {
                    //            var response = new HttpResponseMessage(HttpStatusCode.NotFound);

                    //        }
                }
            else
            {
                var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
               
            }
        }
        public void GetQuotationListRpt_backup(string Bllno)
        {
            List<StockForBillingRPT> StockForBillingRPTlist = new List<StockForBillingRPT>();
            StockForBillingRPT StockForBillingRPT = new StockForBillingRPT();
            if (Bllno != null)
            {
                EasyBillingEntities db = new EasyBillingEntities();
                var Billing_Masters = (from i in db.Quotation_Masters
                                       select i).Where(u => u.Quotation_Number == Bllno).Distinct().FirstOrDefault();
                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[9] {
                            new DataColumn("No", typeof(int)),
                            new DataColumn("ItemId", typeof(string)),
                            new DataColumn("Details", typeof(string)),
                            new DataColumn("QTY", typeof(int)),
                            new DataColumn("Price", typeof(decimal)),
                            new DataColumn("CGST", typeof(decimal)),
                            new DataColumn("SGST", typeof(decimal)),
                            new DataColumn("GST cal by", typeof(string)),
                            new DataColumn("Total", typeof(decimal))});

                int count = 0;
                if (Billing_Masters != null)
                {
                    db.StockForBillingRPTs.RemoveRange(db.StockForBillingRPTs.ToList());
                    db.SaveChanges();

                    var Billing_Details = (from i in db.Quotation_Details
                                           select i).Where(u => u.Quotation_Token_number == Billing_Masters.Token_Number).Distinct().ToList();

                    foreach (var t in Billing_Details)
                    {
                        count++;
                        Products_For_Sale prdctfrsl = db.Products_For_Sales.Where(r => r.Token_Number == t.Selling_item_token).Distinct().FirstOrDefault();
                        StockForBillingRPT.Token_Number = prdctfrsl.Token_Number;
                        StockForBillingRPT.Tyre_Size = prdctfrsl.Tyre_Size;
                        StockForBillingRPT.Selling_SGST = prdctfrsl.Selling_SGST;
                        StockForBillingRPT.Selling_Price = prdctfrsl.Selling_Price;
                        StockForBillingRPT.Selling_CGST = prdctfrsl.Selling_CGST;
                        StockForBillingRPT.Product_name = prdctfrsl.Product_name;
                        StockForBillingRPT.Item_tyre_Id = prdctfrsl.Item_tyre_Id;
                        if (prdctfrsl.IsGstPercent == true)
                            dt.Rows.Add(count, t.Selling_item_id, StockForBillingRPT.Product_name + StockForBillingRPT.Tyre_Size, t.Pieces, StockForBillingRPT.Selling_Price, StockForBillingRPT.Selling_CGST, StockForBillingRPT.Selling_SGST, "(in %)", t.Sub_Total);
                        else if (prdctfrsl.IsGstPercent == false)
                            dt.Rows.Add(count, t.Selling_item_id, StockForBillingRPT.Product_name + StockForBillingRPT.Tyre_Size, t.Pieces, StockForBillingRPT.Selling_Price, StockForBillingRPT.Selling_CGST, StockForBillingRPT.Selling_SGST, "(in Rs.)", t.Sub_Total);
                       else
                            dt.Rows.Add(count, t.Selling_item_id, StockForBillingRPT.Product_name + StockForBillingRPT.Tyre_Size, t.Pieces, StockForBillingRPT.Selling_Price, StockForBillingRPT.Selling_CGST, StockForBillingRPT.Selling_SGST, "(in %)", t.Sub_Total);
                        //string productline = StockForBillingRPT.Item_tyre_Id.PadRight(10) + StockForBillingRPT.Tyre_Size + StockForBillingRPT.Product_name.PadRight(10) + t.Pieces.ToString().PadRight(10) +
                        //   StockForBillingRPT.Selling_Price.ToString().PadRight(10) + StockForBillingRPT.Selling_CGST.ToString().PadRight(10) + StockForBillingRPT.Selling_SGST.ToString().PadRight(10) +
                        //   t.Sub_Total.ToString().PadRight(10);

                        StockForBillingRPTlist.Add(StockForBillingRPT);

                    }

                    db.SaveChanges();

                    var Billing_MastersList = (from i in db.Billing_Masters
                                               select i).Where(u => u.Token_Number == Billing_Masters.Token_Number).Distinct().ToList();
                    var Customers = (from i in db.Customers
                                     select i).Where(u => u.Token_number == Billing_Masters.Customer_token_number).Distinct().ToList();
                    //offset = offset + 20;
                    //graphics.DrawString("Total amount:".PadRight(10) + Billing_Masters.Total_amount.ToString(), font, new SolidBrush(Color.Black), startx, starty + offset);
                    db.StockForBillingRPTs.AddRange(StockForBillingRPTlist);

                    //  string companyName = "Kannantyres";
                    string orderNo = Billing_Masters.Quotation_Number;

                    using (StringWriter sw = new StringWriter())
                    {
                        using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                        {
                            StringBuilder sb = new StringBuilder();

                            //Generate Invoice (Bill) Header.
                            sb.Append("<table width='100%' cellspacing='0' cellpadding='2'>");

                            sb.Append("<tr><td align='center' style='background-color: #18B5F0' colspan = '2'><b>Quotation</b></td></tr>");
                            //sb.Append("<tr><td align='left' style=' colspan = '2' rowspan='2'><img src = 'C:\\Users\\user\\Desktop\\work\\BILLING PROJECT\\Kanantyres tires\\EasyBilling_11032019-up\\EasyBilling\\EasyBilling\\Images\\kannanimg.png' alt = '' border = 3 height = 100 width = 100 ></img ></td></tr>");
                            sb.Append("<tr ><td align='center' style=' colspan = '2' rowspan='1'><img src = 'http://localhost:8087/Images/Untitled.png' ></img ></td><td align='center' style='color:#59cfd7;font:bold 50px Algerian; background-color: #18B5F0';colspan = '2'><b style='font-size:30px'>KANNAN TYRES</b></td></tr>");
                            sb.Append("<tr><td align='center' style='background-color: #18B5F0' colspan = '2'>Cold & Hot Re-treading Vulcanizing,safest Retread and for more mileage </td></tr>");
                            sb.Append("<tr><td align='center' style='background-color: #18B5F0' colspan = '2'>Specialist in : Tractor to scooter</td></tr>");
                            sb.Append("<tr><td align='center' style='background-color: #18B5F0' colspan = '2'>Main Road Gonikoppal S.kodagu 571213 office no : 8274-247525</td></tr>");
                            sb.Append("<tr><td colspan = '2'></td></tr>");
                            sb.Append("<tr><td><b>Quotation No: </b>");
                            sb.Append(orderNo);

                            sb.Append("</td><td align = 'right'><b>Mobile : 9448448730, 8762237724 </b>");
                            sb.Append(" </td></tr>");
                            sb.Append("<tr><td><b>GST no : 29AEIPK0164PIZI </b>");
                            sb.Append("</td><td align = 'right'><b>Date: </b>");
                            sb.Append(DateTime.Now.Date);
                            sb.Append(" </td></tr>");
                            //sb.Append($"<tr><td><b>Customer name and address : {db.Customers.Where(z=>z.Token_number==Billing_Masters.Customer_token_number).Select(z=>z.Customer_Name).FirstOrDefault()} <br/>{db.Customers.Where(z => z.Token_number == Billing_Masters.Customer_token_number).Select(z => z.Address).FirstOrDefault()} </b>");
                            //sb.Append("</td><td align = 'right'><b>Vehicle no: </b>");
                            //sb.Append(db.Customers.Where(z => z.Token_number == Billing_Masters.Customer_token_number).Select(z => z.Vehicle_number).FirstOrDefault());
                            //sb.Append(" </td></tr>");

                            sb.Append("<tr><td colspan = '2'>Dear sir,</td></tr>");
                            sb.Append("<tr><td colspan = '2' align='center'> We have pleasure in quoting out rates for retreading of tyres as under</td></tr>");

                            //sb.Append("<tr><td colspan = '2'><b>Company Name: </b>");
                            //sb.Append(companyName);
                            sb.Append("</td></tr>");
                            sb.Append("</table>");
                            sb.Append("<br />");

                            //Generate Invoice (Bill) Items Grid.
                            sb.Append("<table border = '1'>");
                            sb.Append("<tr style='font-size:10px'>");
                            foreach (DataColumn column in dt.Columns)
                            {
                                sb.Append("<th>");
                                sb.Append(column.ColumnName);
                                sb.Append("</th>");
                            }
                            sb.Append("</tr>");
                            foreach (DataRow row in dt.Rows)
                            {
                                sb.Append("<tr style='font-size:7px'>");
                                foreach (DataColumn column in dt.Columns)
                                {
                                    sb.Append("<td>");
                                    sb.Append(row[column]);
                                    sb.Append("</td>");
                                }
                                sb.Append("</tr>");
                            }
                            //sb.Append("<tr><td align = 'right' colspan = '");
                            //sb.Append(dt.Columns.Count - 1);
                            //sb.Append("'>Total amount</td>");
                            //sb.Append("<td>");
                            //sb.Append(dt.Compute("sum(Total)", ""));
                            //sb.Append("</td>");
                            //sb.Append("</tr>");

                            sb.Append("<tr><td align = 'right' colspan = '");
                            sb.Append(dt.Columns.Count - 1);
                            if (Billing_Masters.Discountper == false)
                                sb.Append($"'>Discount({Billing_Masters.Discount} calculated as Rs.)</td>");
                            else
                                sb.Append($"'>Discount({Billing_Masters.Discount} calculated as %)</td>");
                            sb.Append("<td>");
                            sb.Append(Billing_Masters.Total_discount);
                            sb.Append("</td>");
                            sb.Append("</tr>");
                            sb.Append("<tr><td align = 'right' colspan = '");
                            sb.Append(dt.Columns.Count - 1);
                            sb.Append("'>Total amount</td>");
                            sb.Append("<td>");
                            sb.Append(Billing_Masters.Total_amount - Billing_Masters.Total_discount);
                            sb.Append("</td>");
                            sb.Append("</tr>");
                            sb.Append("<tr><td align = 'center' colspan = '");
                            sb.Append(dt.Columns.Count);
                            sb.Append("'>We Trust you will found our rate most, Reasonable and expect it receive your early valued other</td>");

                            sb.Append("</tr>");
                            sb.Append("<tr><td align = 'left' colspan = '");
                            sb.Append(dt.Columns.Count);
                            sb.Append("'>Thanking you</td>");

                            sb.Append("</tr>");
                            long number = Convert.ToInt64(Billing_Masters.Total_amount - Billing_Masters.Total_discount);
                            string result = null;
                            if (number == 0)
                                result = "ZERO";
                            if (number < 0)
                                result = "minus " + ConvertNumbertoWords(Math.Abs(number));
                            string words = "";
                            if ((number / 1000000) > 0)
                            {
                                words += ConvertNumbertoWords(number / 100000) + " LAKES ";
                                number %= 1000000;
                            }
                            if ((number / 1000) > 0)
                            {
                                words += ConvertNumbertoWords(number / 1000) + " THOUSAND ";
                                number %= 1000;
                            }
                            if ((number / 100) > 0)
                            {
                                words += ConvertNumbertoWords(number / 100) + " HUNDRED ";
                                number %= 100;
                            }

                            if (number > 0)
                            {
                                if (words != "") words += "AND ";
                                var unitsMap = new[]
                                {
            "ZERO", "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINE", "TEN", "ELEVEN", "TWELVE", "THIRTEEN", "FOURTEEN", "FIFTEEN", "SIXTEEN", "SEVENTEEN", "EIGHTEEN", "NINETEEN"
        };
                                var tensMap = new[]
                                {
            "ZERO", "TEN", "TWENTY", "THIRTY", "FORTY", "FIFTY", "SIXTY", "SEVENTY", "EIGHTY", "NINETY"
        };
                                if (number < 20) words += unitsMap[number];
                                else
                                {
                                    words += tensMap[number / 10];
                                    if ((number % 10) > 0) words += " " + unitsMap[number % 10];
                                }
                                result = words;
                            }

                            sb.Append("<tr><td align = 'left' colspan = '");
                            sb.Append(dt.Columns.Count);
                            sb.Append($"'>Amount in words : {result} </td>");

                            sb.Append("</tr>");

                            sb.Append("<tr><td align = 'right' colspan = '");
                            sb.Append(dt.Columns.Count);
                            sb.Append("' >For Kannan Tyres<br/><br/>Role name</td>");
                            sb.Append("</tr>");


                            sb.Append("<tr style='background-color:#F6F6F6' ><td align = 'center' style='background-color:cornflowerblue' colspan = '");
                            sb.Append(dt.Columns.Count);
                            sb.Append("'>Customer Satisfaction is our Aim</td>");
                            sb.Append("</tr>");



                            //sb.Append("<tr><td align = 'right' colspan = '");
                            //sb.Append(dt.Columns.Count - 1);
                            //sb.Append("'>Discount</td>");
                            //sb.Append("<td>");
                            //sb.Append(Billing_Masters.Total_discount);
                            //sb.Append("</td>");
                            sb.Append("</table>");

                            //Export HTML String as PDF.
                            StringReader sr = new StringReader(sb.ToString());
                            Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
                            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                            PdfWriter writer = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                            pdfDoc.Open();
                            htmlparser.Parse(sr);
                            pdfDoc.Close();
                            Response.ContentType = "application/pdf";
                            Response.AddHeader("content-disposition", "attachment;filename=Invoice_" + orderNo + ".pdf");
                            Response.Cache.SetCacheability(HttpCacheability.NoCache);
                            Response.Write(pdfDoc);
                            Response.End();
                        }
                    }
                    // ReportDocument rpt = new ReportDocument();

                    // rpt.Load(Path.Combine(HostingEnvironment.MapPath("~/Reportsdataset/AllReports"), "BillReport.rpt"));

                    // rpt.Load("http://localhost:8087/Reportsdataset/AllReports/BillReport.rpt", CrystalDecisions.Shared.OpenReportMethod.OpenReportByDefault);
                    //rpt.Database.Tables[0].SetDataSource(Billing_MastersList);
                    //rpt.Database.Tables[1].SetDataSource(Billing_Details);
                    //rpt.Database.Tables[2].SetDataSource(Customers);
                    //rpt.Database.Tables[3].SetDataSource(StockForBillingRPTlist);

                    //Stream s = rpt.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    //var response = new HttpResponseMessage(HttpStatusCode.OK);


                    //PrinterSettings printerSettings = new PrinterSettings();
                    //printerSettings.PrinterName = "Microsoft XPS Document Writer";
                    // rpt.PrintToPrinter(printerSettings, new PageSettings(), false);
                    // rpt.PrintToPrinter(1, true, 1, 1);


                    //response.Content = new StreamContent(s);
                    //response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf");
                    //string SetValueForpdf = DateTime.Now.ToString("ddMMyyhhmmtt");
                    // rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, "http://localhost:8087/Reportsdataset/AllReports" + SetValueForpdf + ".pdf");

                    //return response;
                }
                else
                {
                    var response = new HttpResponseMessage(HttpStatusCode.NotFound);
                    // return response;
                }
            }
            else
            {
                var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
                //return response;
            }
        }
        public void GetQuotationListRpt(string Bllno)
        {
            List<StockForBillingRPT> StockForBillingRPTlist = new List<StockForBillingRPT>();
            StockForBillingRPT StockForBillingRPT = new StockForBillingRPT();
            if (Bllno != null)
            {
                EasyBillingEntities db = new EasyBillingEntities();
                var Billing_Masters = (from i in db.Quotation_Masters
                                       select i).Where(u => u.Quotation_Number == Bllno).Distinct().FirstOrDefault();
                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[8] {
                            new DataColumn("No", typeof(int)),
                            new DataColumn("ItemId", typeof(string)),
                            new DataColumn("Details", typeof(string)),
                            new DataColumn("QTY", typeof(int)),
                            new DataColumn("Price", typeof(decimal)),
                           new DataColumn("CGST (in %)", typeof(string)),
                            new DataColumn("SGST (in %)", typeof(string)),
                            //new DataColumn("GST cal by", typeof(string)),
                            new DataColumn("Total", typeof(decimal))});

                int count = 0;
                if (Billing_Masters != null)
                {
                    db.StockForBillingRPTs.RemoveRange(db.StockForBillingRPTs.ToList());
                    db.SaveChanges();

                    var Billing_Details = (from i in db.Quotation_Details
                                           select i).Where(u => u.Quotation_Token_number == Billing_Masters.Token_Number).Distinct().ToList();

                    foreach (var t in Billing_Details)
                    {
                        count++;
                        Products_For_Sale prdctfrsl = db.Products_For_Sales.Where(r => r.Token_Number == t.Selling_item_token).Distinct().FirstOrDefault();
                        StockForBillingRPT.Token_Number = prdctfrsl.Token_Number;
                        StockForBillingRPT.Tyre_Size = prdctfrsl.Tyre_Size;
                        StockForBillingRPT.Selling_SGST = prdctfrsl.Selling_SGST;
                        StockForBillingRPT.Selling_Price = prdctfrsl.Selling_Price;
                        StockForBillingRPT.Selling_CGST = prdctfrsl.Selling_CGST;
                        StockForBillingRPT.Product_name = prdctfrsl.Product_name;
                        StockForBillingRPT.Item_tyre_Id = prdctfrsl.Item_tyre_Id;
                        if (prdctfrsl.IsGstPercent == true)
                            dt.Rows.Add(count, t.Selling_item_id, StockForBillingRPT.Product_name + StockForBillingRPT.Tyre_Size, t.Pieces, StockForBillingRPT.Selling_Price, StockForBillingRPT.Selling_CGST.ToString() , StockForBillingRPT.Selling_SGST.ToString() , t.Sub_Total);
                        else if (prdctfrsl.IsGstPercent == false)
                            dt.Rows.Add(count, t.Selling_item_id, StockForBillingRPT.Product_name + StockForBillingRPT.Tyre_Size, t.Pieces, StockForBillingRPT.Selling_Price, StockForBillingRPT.Selling_CGST.ToString() + " (in Rs.)", StockForBillingRPT.Selling_SGST.ToString() + " (in Rs.)", t.Sub_Total);
                        else
                            dt.Rows.Add(count, t.Selling_item_id, StockForBillingRPT.Product_name + StockForBillingRPT.Tyre_Size, t.Pieces, StockForBillingRPT.Selling_Price, StockForBillingRPT.Selling_CGST.ToString() , StockForBillingRPT.Selling_SGST.ToString() , t.Sub_Total);
                        StockForBillingRPTlist.Add(StockForBillingRPT);

                    }

                    db.SaveChanges();

                    var Billing_MastersList = (from i in db.Quotation_Masters
                                               select i).Where(u => u.Token_Number == Billing_Masters.Token_Number).Distinct().ToList();
                    var Customers = (from i in db.Customers
                                     select i).Where(u => u.Token_number == Billing_Masters.Customer_token_number).Distinct().ToList();

                    db.StockForBillingRPTs.AddRange(StockForBillingRPTlist);

                    string orderNo = Billing_Masters.Quotation_Number;

                    using (StringWriter sw = new StringWriter())
                    {
                        using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                        {
                            StringBuilder sb = new StringBuilder();
                            sb.Append("<!DOCTYPE html><html><head></head><body>");
                            sb.Append("<br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><table  cellspacing='0'  border = '1' >");

                            sb.Append("<tr><td><b>Quotation No : </b>");
                            sb.Append(orderNo);
                            sb.Append("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" +
                                " Date: " + DateTime.Now.ToString("dd MMM yyyy hh:mm tt"));
                            sb.Append(" </td></tr>");

                            //sb.Append("<tr><td colspan = '2'>Dear sir,</td></tr>");
                            //sb.Append("<tr><td colspan = '2' align='center'> We have pleasure in quoting out rates for retreading of tyres as under</td></tr>");
                           
                            sb.Append($"<tr><td align='center'><b>Customer name and address : {db.Customers.Where(z => z.Token_number == Billing_Masters.Customer_token_number).Select(z => z.Customer_Name).FirstOrDefault()} <br/>{db.Customers.Where(z => z.Token_number == Billing_Masters.Customer_token_number).Select(z => z.Address).FirstOrDefault()}<br/>Vehicle no. {db.Customers.Where(z => z.Token_number == Billing_Masters.Customer_token_number).Select(z => z.Vehicle_number).FirstOrDefault()} </b>");
                            sb.Append("</td>");

                            sb.Append("</tr>");
                            sb.Append("</table>");
                            sb.Append("<br />");

                            sb.Append("<table border = '1'>");
                            sb.Append("<tr style='font-size:10px'>");
                            foreach (DataColumn column in dt.Columns)
                            {
                                if (column.ColumnName == "No")
                                    sb.Append("<th width='4%'>");
                                else if (column.ColumnName == "QTY")
                                    sb.Append("<th width='5%'>");
                                else if (column.ColumnName == "Price")
                                    sb.Append("<th width='7%'>");
                                else if (column.ColumnName == "CGST (in %)")
                                    sb.Append("<th width='6%'>");
                                else if (column.ColumnName == "SGST (in %)")
                                    sb.Append("<th width='6%'>");
                                else if (column.ColumnName == "Total")
                                    sb.Append("<th width='9%'>");
                                else
                                    sb.Append("<th>");
                                sb.Append(column.ColumnName);
                                sb.Append("</th>");
                            }
                            sb.Append("</tr>");
                            foreach (DataRow row in dt.Rows)
                            {
                                sb.Append("<tr style='font-size:7px'>");
                                foreach (DataColumn column in dt.Columns)
                                {

                                    sb.Append("<td>");

                                    sb.Append(row[column]);
                                    sb.Append("</td>");
                                }
                                sb.Append("</tr>");
                            }
                            sb.Append("<tr><td align = 'right' colspan = '");
                            sb.Append(dt.Columns.Count - 1);
                            if (Billing_Masters.Discountper == false)
                                sb.Append($"'>Discount({Billing_Masters.Discount} calculated as Rs.)</td>");
                            else
                                sb.Append($"'>Discount({Billing_Masters.Discount} calculated as %)</td>");
                            sb.Append("<td>");
                            sb.Append(Billing_Masters.Total_discount);
                            sb.Append("</td>");
                            sb.Append("</tr>");

                            long number = Convert.ToInt64(Billing_Masters.Total_amount - Billing_Masters.Total_discount);
                            string result = null;
                            if (number == 0)
                                result = "ZERO";
                            if (number < 0)
                                result = "minus " + ConvertNumbertoWords(Math.Abs(number));
                            string words = "";
                            if ((number / 1000000) > 0)
                            {
                                words += ConvertNumbertoWords(number / 100000) + " LAKES ";
                                number %= 1000000;
                            }
                            if ((number / 1000) > 0)
                            {
                                words += ConvertNumbertoWords(number / 1000) + " THOUSAND ";
                                number %= 1000;
                            }
                            if ((number / 100) > 0)
                            {
                                words += ConvertNumbertoWords(number / 100) + " HUNDRED ";
                                number %= 100;
                            }

                            if (number > 0 || !string.IsNullOrEmpty(words))
                            {
                                if (words != "") words += "AND ";
                                var unitsMap = new[]
                                {
                                "ZERO", "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINE", "TEN", "ELEVEN", "TWELVE", "THIRTEEN", "FOURTEEN", "FIFTEEN", "SIXTEEN", "SEVENTEEN", "EIGHTEEN", "NINETEEN"
                            };
                                var tensMap = new[]
                                {
                                "ZERO", "TEN", "TWENTY", "THIRTY", "FORTY", "FIFTY", "SIXTY", "SEVENTY", "EIGHTY", "NINETY"
                            };
                                if (number < 20) words += unitsMap[number];
                                else
                                {
                                    words += tensMap[number / 10];
                                    if ((number % 10) > 0) words += " " + unitsMap[number % 10];
                                }
                                result = words;
                            }
                            
                            sb.Append("<tr><td align = 'right' colspan = '");
                            sb.Append(dt.Columns.Count - 1);
                            sb.Append("'>Total amount</td>");
                            sb.Append("<td>");
                            sb.Append(Billing_Masters.Total_amount - Billing_Masters.Total_discount);
                            sb.Append("</td>");
                            sb.Append("</tr>");
                            sb.Append("<tr><td align = 'left' colspan = '");
                            sb.Append(dt.Columns.Count);
                            sb.Append($"'>Amount paid in words : {result} rupees only</td>");
                            sb.Append("</td></tr>");
                         
                            //sb.Append("<td align = 'left' colspan = '");
                            //sb.Append(dt.Columns.Count - 1);
                            //sb.Append($"'><h5  style='text-align:left'>Amount in words : {result} rupees only</h5>");

                            //sb.Append("</td></tr>");

                            //sb.Append("<tr><td align = 'right' colspan = '");
                            //sb.Append(dt.Columns.Count - 1);
                            //sb.Append("'>Total amount</td>");
                            //sb.Append("<td>");
                            //sb.Append(Billing_Masters.Total_amount - Billing_Masters.Total_discount);
                            //sb.Append("</td>");
                            //sb.Append("</tr>");
                            sb.Append("<tr><td align = 'center' colspan = '");
                            sb.Append(dt.Columns.Count);
                            sb.Append("'>The rate mentioned in this bill remains as is, if there is no change in rate of the time of purchase</td>");

                            sb.Append("</tr>");
                            sb.Append("<tr><td align = 'left' colspan = '");
                            sb.Append(dt.Columns.Count);
                            sb.Append("'>Thanking you</td>");

                            sb.Append("</tr>");
                           

                            //sb.Append("<tr><td align = 'left' colspan = '");
                            //sb.Append(dt.Columns.Count);
                            //sb.Append($"'>Amount in words : {result} rupees only</td>");

                            //sb.Append("</tr>");

                            var empid = db.Quotation_Masters.Where(r => r.Quotation_Number == Bllno).Select(x => x.User_Id).Distinct().FirstOrDefault();
                            if (db.Marchent_Accounts.Where(r => r.Email_Id == empid).Any())
                            {
                                sb.Append("<tr><td align = 'right' colspan = '");
                                sb.Append(dt.Columns.Count);
                                sb.Append($"' >For Kannan Tyres<br/><br/>{db.Marchent_Accounts.Where(r => r.Email_Id == empid).Select(x => x.Marchent_name).Distinct().FirstOrDefault()} , Admin<br/>Role name</td>");
                                sb.Append("</tr>");
                            }
                            else
                            {
                                sb.Append("<tr><td align = 'right' colspan = '");
                                sb.Append(dt.Columns.Count);
                                sb.Append($"' >For Kannan Tyres<br/><br/>{db.Employees.Where(r => r.Employee_Id == empid).Select(x => x.Employee_name).Distinct().FirstOrDefault()} , {db.Employees.Where(r => r.Employee_Id == empid).Select(x => x.Designation).Distinct().FirstOrDefault()}<br/>Role name</td>");
                                sb.Append("</tr>");
                            }

                            //sb.Append("<tr><td align = 'left' colspan = '");
                            //sb.Append(dt.Columns.Count);
                            //sb.Append($"'><img  style='float:left;width:25%' src = 'http://localhost:8087/new 02.JPG' /> </td>");

                            //sb.Append("</tr>");

                            sb.Append("</table><br/><br/><br/><br/><br/><br/>");
                            sb.Append("</body></html>");

                            //////////////////////////////////////////////////////////////////////////////

                            ///////////////////////////////////////////////////////////////////////////////////

                            StringReader sr = new StringReader(sb.ToString());
                            Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
                            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                            PdfWriter writer = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                            //XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                            //XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, new StringReader("<p>helloworld</p>"));
                            string spath = System.Web.HttpContext.Current.Server.MapPath("~/kannanInvoice");
                          
                            PdfWriter.GetInstance(pdfDoc, new FileStream($@"{spath}\Invoice_{ orderNo }.pdf", FileMode.OpenOrCreate));
                            pdfDoc.Open();
                            // XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                            htmlparser.Parse(sr);
                            pdfDoc.Close();
                            Response.ContentType = "application/pdf";
                            Response.AddHeader("content-disposition", "attachment;filename=Invoice_" + orderNo + ".pdf");
                            Response.Cache.SetCacheability(HttpCacheability.NoCache);
                            Response.Write(pdfDoc);
                            Response.End();
                        }
                    }

                }
                //        else
                //        {
                //            var response = new HttpResponseMessage(HttpStatusCode.NotFound);

                //        }
            }
            else
            {
                var response = new HttpResponseMessage(HttpStatusCode.BadRequest);

            }
        }
        public void GetQuotationListRpt_1(string Bllno)
        {
            List<StockForBillingRPT> StockForBillingRPTlist = new List<StockForBillingRPT>();
            StockForBillingRPT StockForBillingRPT = new StockForBillingRPT();
            if (Bllno != null)
            {
                EasyBillingEntities db = new EasyBillingEntities();
                var Billing_Masters = (from i in db.Quotation_Masters
                                       select i).Where(u => u.Quotation_Number == Bllno).Distinct().FirstOrDefault();
                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[9] {
                            new DataColumn("No", typeof(int)),
                            new DataColumn("ItemId", typeof(string)),
                            new DataColumn("Details", typeof(string)),
                            new DataColumn("QTY", typeof(int)),
                            new DataColumn("Price", typeof(decimal)),
                            new DataColumn("CGST", typeof(decimal)),
                            new DataColumn("SGST", typeof(decimal)),
                            new DataColumn("GST cal by", typeof(string)),
                            new DataColumn("Total", typeof(decimal))});

                int count = 0;
                if (Billing_Masters != null)
                {
                    db.StockForBillingRPTs.RemoveRange(db.StockForBillingRPTs.ToList());
                    db.SaveChanges();

                    var Billing_Details = (from i in db.Quotation_Details
                                           select i).Where(u => u.Quotation_Token_number == Billing_Masters.Token_Number).Distinct().ToList();

                    foreach (var t in Billing_Details)
                    {
                        count++;
                        Products_For_Sale prdctfrsl = db.Products_For_Sales.Where(r => r.Token_Number == t.Selling_item_token).Distinct().FirstOrDefault();
                        StockForBillingRPT.Token_Number = prdctfrsl.Token_Number;
                        StockForBillingRPT.Tyre_Size = prdctfrsl.Tyre_Size;
                        StockForBillingRPT.Selling_SGST = prdctfrsl.Selling_SGST;
                        StockForBillingRPT.Selling_Price = prdctfrsl.Selling_Price;
                        StockForBillingRPT.Selling_CGST = prdctfrsl.Selling_CGST;
                        StockForBillingRPT.Product_name = prdctfrsl.Product_name;
                        StockForBillingRPT.Item_tyre_Id = prdctfrsl.Item_tyre_Id;
                        if (prdctfrsl.IsGstPercent == true)
                            dt.Rows.Add(count, t.Selling_item_id, StockForBillingRPT.Product_name + StockForBillingRPT.Tyre_Size, t.Pieces, StockForBillingRPT.Selling_Price, StockForBillingRPT.Selling_CGST, StockForBillingRPT.Selling_SGST, "(in %)", t.Sub_Total);
                        else
                            dt.Rows.Add(count, t.Selling_item_id, StockForBillingRPT.Product_name + StockForBillingRPT.Tyre_Size, t.Pieces, StockForBillingRPT.Selling_Price, StockForBillingRPT.Selling_CGST, StockForBillingRPT.Selling_SGST, "(in Rs.)", t.Sub_Total);
                       
                        StockForBillingRPTlist.Add(StockForBillingRPT);

                    }

                    db.SaveChanges();

                    var Billing_MastersList = (from i in db.Billing_Masters
                                               select i).Where(u => u.Token_Number == Billing_Masters.Token_Number).Distinct().ToList();
                    var Customers = (from i in db.Customers
                                     select i).Where(u => u.Token_number == Billing_Masters.Customer_token_number).Distinct().ToList();
                    

                    db.StockForBillingRPTs.AddRange(StockForBillingRPTlist);
                    
                    string orderNo = Billing_Masters.Quotation_Number;

                    using (StringWriter sw = new StringWriter())
                    {
                        using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                        {
                            StringBuilder sb = new StringBuilder();

                            //Generate Invoice (Bill) Header.
                            sb.Append("<!DOCTYPE html><html><head></head><body>");
                            sb.Append("<br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><table  cellspacing='0'  border = '1' >");
                            //sb.Append("<tr><td style='colspan = '2' ><img width='578%' style='float:left' src = 'http://localhost:8087/new 01.JPG' ></img ></td></tr>");
                            sb.Append("<tr><td><b>Quotation No : </b>");
                            sb.Append(orderNo);
                            sb.Append("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" +
                                " Date: " + DateTime.Now.ToString("dd MMM yyyy hh:mm tt"));
                            sb.Append(" </td></tr>");
                            
                            sb.Append("<tr><td colspan = '2'>Dear sir,</td></tr>");
                            sb.Append("<tr><td colspan = '2' align='center'> We have pleasure in quoting out rates for retreading of tyres as under</td></tr>");
                            
                            sb.Append("</table>");
                            sb.Append("<br />");
                            
                            //Generate Invoice (Bill) Items Grid.
                            sb.Append("<table border = '1'>");
                            sb.Append("<tr style='font-size:10px'>");
                            foreach (DataColumn column in dt.Columns)
                            {
                                if (column.ColumnName == "No")
                                    sb.Append("<th width='4%'>");
                                else
                                    sb.Append("<th>");
                                sb.Append(column.ColumnName);
                                sb.Append("</th>");
                            }
                            sb.Append("</tr>");
                            foreach (DataRow row in dt.Rows)
                            {
                                sb.Append("<tr style='font-size:7px'>");
                                foreach (DataColumn column in dt.Columns)
                                {
                                    sb.Append("<td>");
                                    sb.Append(row[column]);
                                    sb.Append("</td>");
                                }
                                sb.Append("</tr>");
                            }
                            
                            sb.Append("<tr><td align = 'right' colspan = '");
                            sb.Append(dt.Columns.Count - 1);
                            if (Billing_Masters.Discountper == false)
                                sb.Append($"'>Discount({Billing_Masters.Discount} calculated as Rs.)</td>");
                            else
                                sb.Append($"'>Discount({Billing_Masters.Discount} calculated as %)</td>");
                            sb.Append("<td>");
                            sb.Append(Billing_Masters.Total_discount);
                            sb.Append("</td>");
                            sb.Append("</tr>");
                            sb.Append("<tr><td align = 'right' colspan = '");
                            sb.Append(dt.Columns.Count - 1);
                            sb.Append("'>Total amount</td>");
                            sb.Append("<td>");
                            sb.Append(Billing_Masters.Total_amount - Billing_Masters.Total_discount);
                            sb.Append("</td>");
                            sb.Append("</tr>");
                            sb.Append("<tr><td align = 'center' colspan = '");
                            sb.Append(dt.Columns.Count);
                            sb.Append("'>We Trust you will found our rate most, Reasonable and expect it receive your early valued other</td>");

                            sb.Append("</tr>");
                            sb.Append("<tr><td align = 'left' colspan = '");
                            sb.Append(dt.Columns.Count);
                            sb.Append("'>Thanking you</td>");

                            sb.Append("</tr>");
                            long number = Convert.ToInt64(Billing_Masters.Total_amount - Billing_Masters.Total_discount);
                            string result = null;
                            if (number == 0)
                                result = "ZERO";
                            if (number < 0)
                                result = "minus " + ConvertNumbertoWords(Math.Abs(number));
                            string words = "";
                            if ((number / 1000000) > 0)
                            {
                                words += ConvertNumbertoWords(number / 100000) + " LAKES ";
                                number %= 1000000;
                            }
                            if ((number / 1000) > 0)
                            {
                                words += ConvertNumbertoWords(number / 1000) + " THOUSAND ";
                                number %= 1000;
                            }
                            if ((number / 100) > 0)
                            {
                                words += ConvertNumbertoWords(number / 100) + " HUNDRED ";
                                number %= 100;
                            }

                            if (number > 0 || !string.IsNullOrEmpty(words))
                            {
                                if (words != "") words += "AND ";
                                var unitsMap = new[]
                                {
            "ZERO", "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINE", "TEN", "ELEVEN", "TWELVE", "THIRTEEN", "FOURTEEN", "FIFTEEN", "SIXTEEN", "SEVENTEEN", "EIGHTEEN", "NINETEEN"
        };
                                var tensMap = new[]
                                {
            "ZERO", "TEN", "TWENTY", "THIRTY", "FORTY", "FIFTY", "SIXTY", "SEVENTY", "EIGHTY", "NINETY"
        };
                                if (number < 20) words += unitsMap[number];
                                else
                                {
                                    words += tensMap[number / 10];
                                    if ((number % 10) > 0) words += " " + unitsMap[number % 10];
                                }
                                result = words;
                            }

                            sb.Append("<tr><td align = 'left' colspan = '");
                            sb.Append(dt.Columns.Count);
                            sb.Append($"'>Amount paid in words : {result} rupees only </td>");

                            sb.Append("</tr>");
                            //==========================================================
                            var empid = db.Quotation_Masters.Where(r => r.Quotation_Number == Bllno).Select(x => x.User_Id).Distinct().FirstOrDefault();
                            if (db.Marchent_Accounts.Where(r => r.Email_Id == empid).Any())
                            {
                                sb.Append("<tr><td align = 'right' colspan = '");
                                sb.Append(dt.Columns.Count);
                                sb.Append($"' >For Kannan Tyres<br/>{db.Marchent_Accounts.Where(r => r.Email_Id == empid).Select(x => x.Marchent_name).Distinct().FirstOrDefault()} , Admin<br/>Role name</td>");
                                sb.Append("</tr>");
                            }
                            else
                            {
                                sb.Append("<tr><td align = 'right' colspan = '");
                                sb.Append(dt.Columns.Count);
                                sb.Append($"' >For Kannan Tyres<br/>{db.Employees.Where(r => r.Employee_Id == empid).Select(x => x.Employee_name).Distinct().FirstOrDefault()} , {db.Employees.Where(r => r.Employee_Id == empid).Select(x => x.Designation).Distinct().FirstOrDefault()}<br/>Role name</td>");
                                sb.Append("</tr>");
                            }

                            sb.Append("<tr><td align = 'left' colspan = '");
                            sb.Append(dt.Columns.Count);
                            //sb.Append($"'><img  style='float:left;width:25%' src = 'http://localhost:8087/new 02.JPG' ></img > </td>");

                            sb.Append("</tr>");
                            //==========================================================

                            sb.Append("</table><br/><br/><br/><br/><br/><br/>");
                            sb.Append("</body></html>");

                            //Export HTML String as PDF.
                            StringReader sr = new StringReader(sb.ToString());
                            Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
                            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                            PdfWriter writer = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                            pdfDoc.Open();
                            htmlparser.Parse(sr);
                            pdfDoc.Close();
                            Response.ContentType = "application/pdf";
                            Response.AddHeader("content-disposition", "attachment;filename=Invoice_" + orderNo + ".pdf");
                            Response.Cache.SetCacheability(HttpCacheability.NoCache);
                            Response.Write(pdfDoc);
                            Response.End();
                        }
                    }
                    
                }
                else
                {
                    var response = new HttpResponseMessage(HttpStatusCode.NotFound);
                    // return response;
                }
            }
            else
            {
                var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
                //return response;
            }
        }
        public void GetOrderListRpt_backup(string Bllno)
        {
            List<StockForBillingRPT> StockForBillingRPTlist = new List<StockForBillingRPT>();
            StockForBillingRPT StockForBillingRPT = new StockForBillingRPT();
            if (Bllno != null)
            {
                EasyBillingEntities db = new EasyBillingEntities();
                var Billing_Masters = (from i in db.Order_Masters
                                       select i).Where(u => u.Order_Number == Bllno).Distinct().FirstOrDefault();
                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[9] {
                            new DataColumn("No", typeof(int)),
                            new DataColumn("ItemId", typeof(string)),
                            new DataColumn("Details", typeof(string)),
                            new DataColumn("QTY", typeof(int)),
                            new DataColumn("Price", typeof(decimal)),
                            new DataColumn("CGST", typeof(decimal)),
                            new DataColumn("SGST", typeof(decimal)),
                            new DataColumn("GST cal by", typeof(string)),
                            new DataColumn("Total", typeof(decimal))});

                int count = 0;
                if (Billing_Masters != null)
                {
                    db.StockForBillingRPTs.RemoveRange(db.StockForBillingRPTs.ToList());
                    db.SaveChanges();

                    var Billing_Details = (from i in db.Order_Details
                                           select i).Where(u => u.Order_Token_number == Billing_Masters.Token_Number).Distinct().ToList();

                    foreach (var t in Billing_Details)
                    {
                        count++;
                        Products_For_Sale prdctfrsl = db.Products_For_Sales.Where(r => r.Token_Number == t.Selling_item_token).Distinct().FirstOrDefault();
                        StockForBillingRPT.Token_Number = prdctfrsl.Token_Number;
                        StockForBillingRPT.Tyre_Size = prdctfrsl.Tyre_Size;
                        StockForBillingRPT.Selling_SGST = prdctfrsl.Selling_SGST;
                        StockForBillingRPT.Selling_Price = prdctfrsl.Selling_Price;
                        StockForBillingRPT.Selling_CGST = prdctfrsl.Selling_CGST;
                        StockForBillingRPT.Product_name = prdctfrsl.Product_name;
                        StockForBillingRPT.Item_tyre_Id = prdctfrsl.Item_tyre_Id;
                        if (prdctfrsl.IsGstPercent == true)
                            dt.Rows.Add(count, t.Selling_item_id, StockForBillingRPT.Product_name + StockForBillingRPT.Tyre_Size, t.Pieces, StockForBillingRPT.Selling_Price, StockForBillingRPT.Selling_CGST, StockForBillingRPT.Selling_SGST, "(in %)", t.Sub_Total);
                        else
                            dt.Rows.Add(count, t.Selling_item_id, StockForBillingRPT.Product_name + StockForBillingRPT.Tyre_Size, t.Pieces, StockForBillingRPT.Selling_Price, StockForBillingRPT.Selling_CGST, StockForBillingRPT.Selling_SGST, "(in Rs.)", t.Sub_Total);
                     
                        StockForBillingRPTlist.Add(StockForBillingRPT);

                    }

                    db.SaveChanges();

                    var Billing_MastersList = (from i in db.Billing_Masters
                                               select i).Where(u => u.Token_Number == Billing_Masters.Token_Number).Distinct().ToList();
                    var Customers = (from i in db.Customers
                                     select i).Where(u => u.Token_number == Billing_Masters.Customer_token_number).Distinct().ToList();
                    //offset = offset + 20;
                    //graphics.DrawString("Total amount:".PadRight(10) + Billing_Masters.Total_amount.ToString(), font, new SolidBrush(Color.Black), startx, starty + offset);
                    db.StockForBillingRPTs.AddRange(StockForBillingRPTlist);

                    //  string companyName = "Kannantyres";
                    string orderNo = Billing_Masters.Order_Number;

                    using (StringWriter sw = new StringWriter())
                    {
                        using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                        {
                            StringBuilder sb = new StringBuilder();

                            //Generate Invoice (Bill) Header.
                            sb.Append("<table width='100%' cellspacing='0' cellpadding='2'>");

                            sb.Append("<tr><td align='center' style='background-color: #18B5F0' colspan = '2'><b>ORDER SHEET</b></td></tr>");
                            //sb.Append("<tr><td align='left' style=' colspan = '2' rowspan='2'><img src = 'C:\\Users\\user\\Desktop\\work\\BILLING PROJECT\\Kanantyres tires\\EasyBilling_11032019-up\\EasyBilling\\EasyBilling\\Images\\kannanimg.png' alt = '' border = 3 height = 100 width = 100 ></img ></td></tr>");
                            sb.Append("<tr ><td align='center' style=' colspan = '2' rowspan='1'><img src = 'http://localhost:8087/Images/Untitled.png' ></img ></td><td align='center' style='color:#59cfd7;font:bold 50px Algerian; background-color: #18B5F0';colspan = '2'><b style='font-size:30px'>KANNAN TYRES</b></td></tr>");
                            sb.Append("<tr><td align='center' style='background-color: #18B5F0' colspan = '2'>Cold & Hot Re-treading Vulcanizing,safest Retread and for more mileage </td></tr>");
                            sb.Append("<tr><td align='center' style='background-color: #18B5F0' colspan = '2'>Specialist in : Tractor to scooter</td></tr>");
                            sb.Append("<tr><td align='center' style='background-color: #18B5F0' colspan = '2'>Main Road Gonikoppal S.kodagu 571213 office no : 8274-247525</td></tr>");
                            sb.Append("<tr><td colspan = '2'></td></tr>");
                            sb.Append("<tr><td><b>Invoice No: </b>");
                            sb.Append(orderNo);

                            sb.Append("</td><td align = 'right'><b>Mobile : 9448448730, 8762237724 </b>");
                            sb.Append(" </td></tr>");
                            sb.Append("<tr><td><b>GST no : 29AEIPK0164PIZI </b>");
                            sb.Append("</td><td align = 'right'><b>Date: </b>");
                            sb.Append(DateTime.Now.Date);
                            sb.Append(" </td></tr>");
                            sb.Append($"<tr><td><b>Customer name and address : {db.Customers.Where(z => z.Token_number == Billing_Masters.Customer_token_number).Select(z => z.Customer_Name).FirstOrDefault()} <br/>{db.Customers.Where(z => z.Token_number == Billing_Masters.Customer_token_number).Select(z => z.Address).FirstOrDefault()} </b>");
                            sb.Append("</td><td align = 'right'><b>Vehicle no: </b>");
                            sb.Append(db.Customers.Where(z => z.Token_number == Billing_Masters.Customer_token_number).Select(z => z.Vehicle_number).FirstOrDefault());
                            sb.Append(" </td></tr>");
                            
                            sb.Append("</td></tr>");
                            sb.Append("</table>");
                            sb.Append("<br />");

                            //Generate Invoice (Bill) Items Grid.
                            sb.Append("<table border = '1'>");
                            sb.Append("<tr style='font-size:10px'>");
                            foreach (DataColumn column in dt.Columns)
                            {
                                sb.Append("<th>");
                                sb.Append(column.ColumnName);
                                sb.Append("</th>");
                            }
                            sb.Append("</tr>");
                            foreach (DataRow row in dt.Rows)
                            {
                                sb.Append("<tr style='font-size:7px'>");
                                foreach (DataColumn column in dt.Columns)
                                {
                                    sb.Append("<td>");
                                    sb.Append(row[column]);
                                    sb.Append("</td>");
                                }
                                sb.Append("</tr>");
                            }
                            sb.Append("<tr><td align = 'right' colspan = '");
                            sb.Append(dt.Columns.Count - 1);
                            if (Billing_Masters.Discountper == false)
                                sb.Append($"'>Discount({Billing_Masters.Discount} calculated as Rs.)</td>");
                            else
                                sb.Append($"'>Discount({Billing_Masters.Discount} calculated as %)</td>");
                            sb.Append("<td>");
                            sb.Append(Billing_Masters.Total_discount);
                            sb.Append("</td>");
                            sb.Append("</tr>");
                            sb.Append("<tr><td align = 'right' colspan = '");
                            sb.Append(dt.Columns.Count - 1);
                            sb.Append("'>Amount Paid</td>");
                            sb.Append("<td>");
                            sb.Append(Billing_Masters.Amount_paid);
                            sb.Append("</td>");
                            sb.Append("</tr>");


                            sb.Append("<tr><td align = 'right' colspan = '");
                            sb.Append(dt.Columns.Count - 1);
                            sb.Append("'>Balance</td>");
                            sb.Append("<td>");
                            sb.Append(Billing_Masters.Balance);

                            sb.Append("</td>");
                            sb.Append("</tr>");
                            long number = Convert.ToInt64(Billing_Masters.Amount_paid);
                            string result = null;
                            if (number == 0)
                                result = "ZERO";
                            if (number < 0)
                                result = "minus " + ConvertNumbertoWords(Math.Abs(number));
                            string words = "";
                            if ((number / 1000000) > 0)
                            {
                                words += ConvertNumbertoWords(number / 100000) + " LAKES ";
                                number %= 1000000;
                            }
                            if ((number / 1000) > 0)
                            {
                                words += ConvertNumbertoWords(number / 1000) + " THOUSAND ";
                                number %= 1000;
                            }
                            if ((number / 100) > 0)
                            {
                                words += ConvertNumbertoWords(number / 100) + " HUNDRED ";
                                number %= 100;
                            }

                            if (number > 0)
                            {
                                if (words != "") words += "AND ";
                                var unitsMap = new[]
                                {
            "ZERO", "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINE", "TEN", "ELEVEN", "TWELVE", "THIRTEEN", "FOURTEEN", "FIFTEEN", "SIXTEEN", "SEVENTEEN", "EIGHTEEN", "NINETEEN"
        };
                                var tensMap = new[]
                                {
            "ZERO", "TEN", "TWENTY", "THIRTY", "FORTY", "FIFTY", "SIXTY", "SEVENTY", "EIGHTY", "NINETY"
        };
                                if (number < 20) words += unitsMap[number];
                                else
                                {
                                    words += tensMap[number / 10];
                                    if ((number % 10) > 0) words += " " + unitsMap[number % 10];
                                }
                                result = words;
                            }

                            sb.Append("<tr><td align = 'left' colspan = '");
                            sb.Append(dt.Columns.Count);
                            sb.Append($"'>Amount in words : {result} </td>");
                            sb.Append("<td align = 'left' colspan = '");
                            sb.Append(dt.Columns.Count);
                            sb.Append($"'>Customer Should take their tyre within 6 months </td>");
                            sb.Append("</tr>");
                          

                           

                            sb.Append("<tr><td align = 'right' colspan = '");
                            sb.Append(dt.Columns.Count);
                            sb.Append("' >For Kannan Tyres<br/><br/>Role name</td>");
                            sb.Append("</tr>");


                            sb.Append("<tr style='background-color:#F6F6F6' ><td align = 'center' style='background-color:cornflowerblue' colspan = '");
                            sb.Append(dt.Columns.Count);
                            sb.Append("'>Customer Satisfaction is our Aim</td>");
                            sb.Append("</tr>");



                            //sb.Append("<tr><td align = 'right' colspan = '");
                            //sb.Append(dt.Columns.Count - 1);
                            //sb.Append("'>Discount</td>");
                            //sb.Append("<td>");
                            //sb.Append(Billing_Masters.Total_discount);
                            //sb.Append("</td>");
                            sb.Append("</table>");

                            //Export HTML String as PDF.
                            StringReader sr = new StringReader(sb.ToString());
                            Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
                            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                            PdfWriter writer = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                            pdfDoc.Open();
                            htmlparser.Parse(sr);
                            pdfDoc.Close();
                            Response.ContentType = "application/pdf";
                            Response.AddHeader("content-disposition", "attachment;filename=Invoice_" + orderNo + ".pdf");
                            Response.Cache.SetCacheability(HttpCacheability.NoCache);
                            Response.Write(pdfDoc);
                            Response.End();
                        }
                    }
                    // ReportDocument rpt = new ReportDocument();

                    // rpt.Load(Path.Combine(HostingEnvironment.MapPath("~/Reportsdataset/AllReports"), "BillReport.rpt"));

                    // rpt.Load("http://localhost:8087/Reportsdataset/AllReports/BillReport.rpt", CrystalDecisions.Shared.OpenReportMethod.OpenReportByDefault);
                    //rpt.Database.Tables[0].SetDataSource(Billing_MastersList);
                    //rpt.Database.Tables[1].SetDataSource(Billing_Details);
                    //rpt.Database.Tables[2].SetDataSource(Customers);
                    //rpt.Database.Tables[3].SetDataSource(StockForBillingRPTlist);

                    //Stream s = rpt.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    //var response = new HttpResponseMessage(HttpStatusCode.OK);


                    //PrinterSettings printerSettings = new PrinterSettings();
                    //printerSettings.PrinterName = "Microsoft XPS Document Writer";
                    // rpt.PrintToPrinter(printerSettings, new PageSettings(), false);
                    // rpt.PrintToPrinter(1, true, 1, 1);


                    //response.Content = new StreamContent(s);
                    //response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf");
                    //string SetValueForpdf = DateTime.Now.ToString("ddMMyyhhmmtt");
                    // rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, "http://localhost:8087/Reportsdataset/AllReports" + SetValueForpdf + ".pdf");

                    //return response;
                }
                else
                {
                    var response = new HttpResponseMessage(HttpStatusCode.NotFound);
                    // return response;
                }
            }
            else
            {
                var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
                //return response;
            }
        }
        public void GetOrderListRpt(string Bllno)
        {
            List<StockForBillingRPT> StockForBillingRPTlist = new List<StockForBillingRPT>();
            StockForBillingRPT StockForBillingRPT = new StockForBillingRPT();
            if (Bllno != null)
            {
                EasyBillingEntities db = new EasyBillingEntities();
                var Billing_Masters = (from i in db.Order_Masters
                                       select i).Where(u => u.Order_Number == Bllno).Distinct().FirstOrDefault();
                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[9] {
                            new DataColumn("No", typeof(int)),
                            new DataColumn("ItemId", typeof(string)),
                            new DataColumn("Details", typeof(string)),
                             new DataColumn("Tyre no", typeof(string)),
                            new DataColumn("QTY", typeof(int)),
                            new DataColumn("Price", typeof(decimal)),
                             new DataColumn("CGST (in %)", typeof(string)),
                            new DataColumn("SGST (in %)", typeof(string)),
                          //  new DataColumn("GST cal by", typeof(string)),
                            new DataColumn("Total", typeof(decimal))});

                int count = 0;
                if (Billing_Masters != null)
                {
                    db.StockForBillingRPTs.RemoveRange(db.StockForBillingRPTs.ToList());
                    db.SaveChanges();

                    var Billing_Details = (from i in db.Order_Details
                                           select i).Where(u => u.Order_Token_number == Billing_Masters.Token_Number).Distinct().ToList();

                    foreach (var t in Billing_Details)
                    {
                        count++;
                        Products_For_Sale prdctfrsl = db.Products_For_Sales.Where(r => r.Token_Number == t.Selling_item_token).Distinct().FirstOrDefault();
                        StockForBillingRPT.Token_Number = prdctfrsl.Token_Number;
                        StockForBillingRPT.Tyre_Size = prdctfrsl.Tyre_Size;
                        StockForBillingRPT.Selling_SGST = prdctfrsl.Selling_SGST;
                        StockForBillingRPT.Selling_Price = prdctfrsl.Selling_Price;
                        StockForBillingRPT.Selling_CGST = prdctfrsl.Selling_CGST;
                        StockForBillingRPT.Product_name = prdctfrsl.Product_name;
                        StockForBillingRPT.Item_tyre_Id = prdctfrsl.Item_tyre_Id;
                        if (prdctfrsl.IsGstPercent == true)
                            dt.Rows.Add(count, t.Selling_item_id, StockForBillingRPT.Product_name + StockForBillingRPT.Tyre_Size, t.Tyre_number, t.Pieces, StockForBillingRPT.Selling_Price, StockForBillingRPT.Selling_CGST.ToString() , StockForBillingRPT.Selling_SGST.ToString() ,t.Sub_Total);
                        else if(prdctfrsl.IsGstPercent == false)
                            dt.Rows.Add(count, t.Selling_item_id, StockForBillingRPT.Product_name + StockForBillingRPT.Tyre_Size, t.Tyre_number, t.Pieces, StockForBillingRPT.Selling_Price, StockForBillingRPT.Selling_CGST.ToString() + " (in Rs.)", StockForBillingRPT.Selling_SGST.ToString() + " (in Rs.)", t.Sub_Total);
                        else
                            dt.Rows.Add(count, t.Selling_item_id, StockForBillingRPT.Product_name + StockForBillingRPT.Tyre_Size, t.Tyre_number, t.Pieces, StockForBillingRPT.Selling_Price, StockForBillingRPT.Selling_CGST.ToString() , StockForBillingRPT.Selling_SGST.ToString() , t.Sub_Total);
                        StockForBillingRPTlist.Add(StockForBillingRPT);

                    }

                    db.SaveChanges();

                    var Billing_MastersList = (from i in db.Billing_Masters
                                               select i).Where(u => u.Token_Number == Billing_Masters.Token_Number).Distinct().ToList();
                    var Customers = (from i in db.Customers
                                     select i).Where(u => u.Token_number == Billing_Masters.Customer_token_number).Distinct().ToList();
                    //offset = offset + 20;
                    //graphics.DrawString("Total amount:".PadRight(10) + Billing_Masters.Total_amount.ToString(), font, new SolidBrush(Color.Black), startx, starty + offset);
                    db.StockForBillingRPTs.AddRange(StockForBillingRPTlist);

                    //  string companyName = "Kannantyres";
                    string orderNo = Billing_Masters.Order_Number;

                    using (StringWriter sw = new StringWriter())
                    {
                        using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                        {
                            StringBuilder sb = new StringBuilder();

                            //Generate Invoice (Bill) Header.
                            sb.Append("<!DOCTYPE html><html><head></head><body>");
                            sb.Append("<br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><table  cellspacing='0'  border = '1' >");
                            //sb.Append("<tr><td style='colspan = '2' ><img width='578%' style='float:left' src = 'http://localhost:8087/1566548777665_new 033.JPG' ></img ></td></tr>");

                            sb.Append("<tr><td><b>Invoice No: </b>");
                            sb.Append(orderNo);
                            sb.Append("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" +
                                "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<b> Date: " + DateTime.Now.ToString("dd MMM yyyy hh:mm tt"));
                           
                            sb.Append(" </td></tr>");

                            sb.Append($"<tr><td align='left'><b>Customer name and address : {db.Customers.Where(z => z.Token_number == Billing_Masters.Customer_token_number).Select(z => z.Customer_Name).FirstOrDefault()} <br/>{db.Customers.Where(z => z.Token_number == Billing_Masters.Customer_token_number).Select(z => z.Address).FirstOrDefault()}<br/>Vehicle no. {db.Customers.Where(z => z.Token_number == Billing_Masters.Customer_token_number).Select(z => z.Vehicle_number).FirstOrDefault()} </b>");
                            sb.Append("</td>");

                            sb.Append("</tr>");
                            
                            sb.Append("</td></tr>");
                            sb.Append("</table>");
                            sb.Append("<br />");
                         

                            //Generate Invoice (Bill) Items Grid.
                            sb.Append("<table border = '1'>");
                            sb.Append("<tr style='font-size:10px'>");
                            foreach (DataColumn column in dt.Columns)
                            {
                                if (column.ColumnName == "No")
                                    sb.Append("<th width='4%'>");
                                else
                                    sb.Append("<th>");
                                sb.Append(column.ColumnName);
                                sb.Append("</th>");
                            }
                            sb.Append("</tr>");
                            foreach (DataRow row in dt.Rows)
                            {
                                sb.Append("<tr style='font-size:7px'>");
                                foreach (DataColumn column in dt.Columns)
                                {
                                    sb.Append("<td>");
                                    sb.Append(row[column]);
                                    sb.Append("</td>");
                                }
                                sb.Append("</tr>");
                            }
                            sb.Append("<tr><td align = 'right' colspan = '");
                            sb.Append(dt.Columns.Count - 1);
                            if (Billing_Masters.Discountper == false)
                                sb.Append($"'>Discount({Billing_Masters.Discount} calculated as Rs.)</td>");
                            else
                                sb.Append($"'>Discount({Billing_Masters.Discount} calculated as %)</td>");
                            sb.Append("<td>");
                            sb.Append(Billing_Masters.Total_discount);
                            sb.Append("</td>");
                            sb.Append("</tr>");
                            sb.Append("<tr><td align = 'right' colspan = '");
                            sb.Append(dt.Columns.Count - 1);
                            sb.Append("'>Amount Paid</td>");
                            sb.Append("<td>");
                            sb.Append(Billing_Masters.Amount_paid);
                            sb.Append("</td>");
                            sb.Append("</tr>");


                            sb.Append("<tr><td align = 'right' colspan = '");
                            sb.Append(dt.Columns.Count - 1);
                            sb.Append("'>Balance</td>");
                            sb.Append("<td>");
                            sb.Append(Billing_Masters.Balance);

                            sb.Append("</td>");
                            sb.Append("</tr>");
                            long number = Convert.ToInt64(Billing_Masters.Amount_paid);
                            string result = null;
                            if (number == 0)
                                result = "ZERO";
                            if (number < 0)
                                result = "minus " + ConvertNumbertoWords(Math.Abs(number));
                            string words = "";
                            if ((number / 1000000) > 0)
                            {
                                words += ConvertNumbertoWords(number / 100000) + " LAKES ";
                                number %= 1000000;
                            }
                            if ((number / 1000) > 0)
                            {
                                words += ConvertNumbertoWords(number / 1000) + " THOUSAND ";
                                number %= 1000;
                            }
                            if ((number / 100) > 0)
                            {
                                words += ConvertNumbertoWords(number / 100) + " HUNDRED ";
                                number %= 100;
                            }

                            if (number > 0 || !string.IsNullOrEmpty(words))
                            {
                                if (words != "") words += "AND ";
                                var unitsMap = new[]
                                {
                                "ZERO", "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINE", "TEN", "ELEVEN", "TWELVE", "THIRTEEN", "FOURTEEN", "FIFTEEN", "SIXTEEN", "SEVENTEEN", "EIGHTEEN", "NINETEEN"
                            };
                                var tensMap = new[]
                                {
                                "ZERO", "TEN", "TWENTY", "THIRTY", "FORTY", "FIFTY", "SIXTY", "SEVENTY", "EIGHTY", "NINETY"
                            };
                                if (number < 20) words += unitsMap[number];
                                else
                                {
                                    words += tensMap[number / 10];
                                    if ((number % 10) > 0) words += " " + unitsMap[number % 10];
                                }
                                result = words;
                            }
                            var deczero = decimal.Parse("0.00");
                            sb.Append("<tr>");
                            if (Billing_Masters.Amount_paid != deczero)
                            {
                                sb.Append("<td align = 'left' colspan = '");
                                sb.Append(dt.Columns.Count);
                                sb.Append($"'>Amount paid in words : {result} rupees only</td>");
                                sb.Append("<td align = 'left' colspan = '");
                                sb.Append(dt.Columns.Count);
                                sb.Append($"'>Mode of payment: {Billing_Masters.Mode_of_payment} </td>");
                            }
                            sb.Append("<td align = 'left' colspan = '");
                            sb.Append(dt.Columns.Count);
                            sb.Append($"'>Customer Should take their tyre within 6 months </td>");
                            sb.Append("</tr>");
                            //===============================================================
                            var empid = db.Order_Masters.Where(r => r.Order_Number == Bllno).Select(x => x.User_Id).Distinct().FirstOrDefault();
                            if (db.Marchent_Accounts.Where(r => r.Email_Id == empid).Any())
                            {
                                sb.Append("<tr><td align = 'right' colspan = '");
                                sb.Append(dt.Columns.Count);
                                sb.Append($"' >For Kannan Tyres<br/><br/>{db.Marchent_Accounts.Where(r => r.Email_Id == empid).Select(x => x.Marchent_name).Distinct().FirstOrDefault()} , Admin<br/>Role name</td>");
                                sb.Append("</tr>");
                            }
                            else
                            {
                                sb.Append("<tr><td align = 'right' colspan = '");
                                sb.Append(dt.Columns.Count);
                                sb.Append($"' >For Kannan Tyres<br/><br/>{db.Employees.Where(r => r.Employee_Id == empid).Select(x => x.Employee_name).Distinct().FirstOrDefault()} , {db.Employees.Where(r => r.Employee_Id == empid).Select(x => x.Designation).Distinct().FirstOrDefault()}<br/>Role name</td>");
                                sb.Append("</tr>");
                            }
                            
                            sb.Append("</table><br/><br/><br/><br/><br/><br/>");
                            sb.Append("</body></html>");
                            //Export HTML String as PDF.
                            StringReader sr = new StringReader(sb.ToString());
                            Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
                            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                            PdfWriter writer = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                            string spath = System.Web.HttpContext.Current.Server.MapPath("~/kannanInvoice");
                           
                            PdfWriter.GetInstance(pdfDoc, new FileStream($@"{spath}\Invoice_{ orderNo }.pdf", FileMode.OpenOrCreate));
                            pdfDoc.Open();
                            htmlparser.Parse(sr);
                            pdfDoc.Close();
                            Response.ContentType = "application/pdf";
                            Response.AddHeader("content-disposition", "attachment;filename=Invoice_" + orderNo + ".pdf");
                            Response.Cache.SetCacheability(HttpCacheability.NoCache);
                            Response.Write(pdfDoc);
                            Response.End();
                        }
                    }
                  
                }
                else
                {
                    var response = new HttpResponseMessage(HttpStatusCode.NotFound);
                    // return response;
                }
            }
            else
            {
                var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
                //return response;
            }
        }
        public string ConvertNumbertoWords(long number)
        {
            if (number == 0) return "ZERO";
            if (number < 0) return "minus " + ConvertNumbertoWords(Math.Abs(number));
            string words = "";
            if ((number / 1000000) > 0)
            {
                words += ConvertNumbertoWords(number / 100000) + " LAKES ";
                number %= 1000000;
            }
            if ((number / 1000) > 0)
            {
                words += ConvertNumbertoWords(number / 1000) + " THOUSAND ";
                number %= 1000;
            }
            if ((number / 100) > 0)
            {
                words += ConvertNumbertoWords(number / 100) + " HUNDRED ";
                number %= 100;
            }
            //if ((number / 10) > 0)  
            //{  
            // words += ConvertNumbertoWords(number / 10) + " RUPEES ";  
            // number %= 10;  
            //}  
            if (number > 0)
            {
                if (words != "") words += "AND ";
                var unitsMap = new[]
                {
            "ZERO", "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINE", "TEN", "ELEVEN", "TWELVE", "THIRTEEN", "FOURTEEN", "FIFTEEN", "SIXTEEN", "SEVENTEEN", "EIGHTEEN", "NINETEEN"
        };
                var tensMap = new[]
                {
            "ZERO", "TEN", "TWENTY", "THIRTY", "FORTY", "FIFTY", "SIXTY", "SEVENTY", "EIGHTY", "NINETY"
        };
                if (number < 20) words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0) words += " " + unitsMap[number % 10];
                }
            }
            return words;
        }
        public JsonResult GetItemIdForStockentry(string term)
        {

            List<string> itemids = (from tyreitms in db.Item_Tyres 
                                    where tyreitms.Item_Id.Contains(term) && (tyreitms.Tyre_make == "New" || tyreitms.Tyre_make == "Old")
                                    select tyreitms.Item_Id).Distinct().ToList();

            if (itemids.Count == 0)
            {
                itemids.Add("No item id is matched with this entry...");
            }
            return Json(itemids, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetItemTubeIdStockentry(string term)
        {

            List<string> itemid = (from tyreitms in db.Item_Tubes 
                                   where tyreitms.Item_Id.Contains(term) 
                                   select tyreitms.Item_Id).Distinct().ToList();
            
            if (itemid.Count == 0)
            {
                itemid.Add("No id is matched with this entry...");
            }
            return Json(itemid, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetProductIdwithoutservceStockentry(string term)
        {
            List<string> itemid = (from item in db.Other_Products

                                             where item.Product_id.Contains(term) && (item.Product_type != "Services")
                                             select item.Product_id).Distinct().ToList();
            
            if (itemid.Count == 0)
            {
                itemid.Add("No id is matched with this entry...");
            }
            return Json(itemid, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult GetItemtyreIdAll(string term)
        //{
        //    List<string> itemids = (from item in db.Products_For_Sales
        //                            join tyreitms in db.Item_Tyres on item.Item_tyre_Id equals tyreitms.Item_Id
        //                            where item.Item_tyre_Id.Contains(term) && item.Pieces > 0 && (tyreitms.Tyre_make == "New" || tyreitms.Tyre_make == "Old")
        //                            select item.Item_tyre_Id).Distinct().ToList();
        //    if (itemids.Count == 0)
        //    {
        //        itemids.Add("No item id is matched with this entry...");
        //    }
        //    return Json(itemids, JsonRequestBehavior.AllowGet);
        //}

        public JsonResult GetItemIdForBill(string term)
        {
            List<string> itemids = (from item in db.Products_For_Sales
                                  join tyreitms in db.Item_Tyres on item.Item_tyre_Id equals tyreitms.Item_Id
                                    where item.Item_tyre_Id.Contains(term) && item.Pieces>0 && (tyreitms.Tyre_make=="New" || tyreitms.Tyre_make=="Old")
                                             select item.Item_tyre_Id).Distinct().ToList();
            if (itemids.Count == 0)
            {
                itemids.Add("No item id is matched with this entry...");
            }
            return Json(itemids, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetItemIdForBillresole(string term)
        {

            List<string> itemids = (from item in db.Products_For_Sales
                                    join tyreitms in db.Item_Tyres on item.Item_tyre_Id equals tyreitms.Item_Id
                                    where item.Item_tyre_Id.Contains(term) && item.Pieces > 0 && (tyreitms.Tyre_make == "Resole")
                                    select item.Item_tyre_Id).Distinct().ToList();

            if (itemids.Count == 0)
            {
                itemids.Add("No item id is matched with this entry...");
            }
            return Json(itemids, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetItemTubeId(string term)
        {

            List<string> itemid = (from item in db.Products_For_Sales
                                    join tyreitms in db.Item_Tubes on item.Item_tyre_Id equals tyreitms.Item_Id
                                    where item.Item_tyre_Id.Contains(term) && item.Pieces > 0
                                    select item.Item_tyre_Id).Distinct().ToList();

        
            
            if (itemid.Count == 0)
            {
                itemid.Add("No id is matched with this entry...");
            }
            return Json(itemid, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetProductIdWithServicesWithoutRawmat(string term)
        {
            List<string> temp_Stocks = (from temp in db.Temp_Stock
                                        select temp.Item_tyre_Id).Distinct().ToList();
            IEnumerable<string> itemidchk = (from item in db.Other_Products

                                             where item.Product_id.Contains(term) && item.Product_type != "Raw Material"
                                             select item.Product_id).Distinct().ToList();
            List<string> itemid = itemidchk.Except(temp_Stocks).ToList();


            if (itemid.Count == 0)
            {
                itemid.Add("No id is matched with this entry...");
            }
            return Json(itemid, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetProductIdWithoutServicesWithRawmat(string term)
        {
            List<string> temp_Stocks = (from temp in db.Temp_Stock
                                        select temp.Item_tyre_Id).Distinct().ToList();
            IEnumerable<string> itemidchk = (from item in db.Other_Products

                                             where item.Product_id.Contains(term) && item.Product_type != "Services"
                                             select item.Product_id).Distinct().ToList();
            List<string> itemid = itemidchk.Except(temp_Stocks).ToList();


            if (itemid.Count == 0)
            {
                itemid.Add("No id is matched with this entry...");
            }
            return Json(itemid, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetProductIdWithoutServicesWithoutRawmat(string term)
        {
            List<string> temp_Stocks = (from temp in db.Temp_Stock
                                        select temp.Item_tyre_Id).Distinct().ToList();
            IEnumerable<string> itemidchk = (from item in db.Other_Products

                                             where item.Product_id.Contains(term) && item.Product_type != "Raw Material" && item.Product_type != "Services"
                                             select item.Product_id).Distinct().ToList();
            List<string> itemid = itemidchk.Except(temp_Stocks).ToList();


            if (itemid.Count == 0)
            {
                itemid.Add("No id is matched with this entry...");
            }
            return Json(itemid, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetProductId(string term)
        {
            List<string> temp_Stocks = (from temp in db.Temp_Stock
                                        select temp.Item_tyre_Id).Distinct().ToList();
            IEnumerable<string> itemidchk = (from item in db.Other_Products

                                             where item.Product_id.Contains(term) && item.Product_type != "Raw Material"
                                             select item.Product_id).Distinct().ToList();
            List<string> itemid = itemidchk.Except(temp_Stocks).ToList();


            if (itemid.Count == 0)
            {
                itemid.Add("No id is matched with this entry...");
            }
            return Json(itemid, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetProductIdwithoutservceresole(string term)
        {
            List<string> temp_Stocks = (from temp in db.Temp_Stock
                                        select temp.Item_tyre_Id).Distinct().ToList();
            IEnumerable<string> itemidchk = (from item in db.Other_Products

                                             where item.Product_id.Contains(term) && (item.Product_type != "Services" && item.Product_type != "Raw Material")
                                             select item.Product_id).Distinct().ToList();
            List<string> itemid = itemidchk.Except(temp_Stocks).ToList();


            if (itemid.Count == 0)
            {
                itemid.Add("No id is matched with this entry...");
            }
            return Json(itemid, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetProductIdwithoutservce(string term)
        {
            List<string> temp_Stocks = (from temp in db.Temp_Stock
                                        select temp.Item_tyre_Id).Distinct().ToList();
            IEnumerable<string> itemidchk = (from item in db.Other_Products

                                             where item.Product_id.Contains(term) && (item.Product_type != "Services")
                                             select item.Product_id).Distinct().ToList();
            List<string> itemid = itemidchk.Except(temp_Stocks).ToList();


            if (itemid.Count == 0)
            {
                itemid.Add("No id is matched with this entry...");
            }
            return Json(itemid, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCompNameforstock(string term)
        {
            List<string> compname = (from item in db.Products_For_Sales
                                     join tyreitms in db.Item_Tyres on item.Item_tyre_Id equals tyreitms.Item_Id
                                     where item.Product_name.Contains(term) && (tyreitms.Tyre_make == "New" || tyreitms.Tyre_make == "Old")
                                     select item.Product_name).Distinct().ToList();

            //   return Json(itemids, JsonRequestBehavior.AllowGet);
            //List<string> compname = (from item in db.Item_Tyres

            //                         where item.Company_name.Contains(term)
            //                         select item.Company_name).Distinct().ToList();

            if (compname.Count == 0)
            {
                compname.Add("No company name is matched with this entry...");
            }
            return Json(compname, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCompName(string term)
        {
            List<string> compname = (from item in db.Products_For_Sales
                                    join tyreitms in db.Item_Tyres on item.Item_tyre_Id equals tyreitms.Item_Id
                                    where item.Product_name.Contains(term) && item.Pieces > 0 && (tyreitms.Tyre_make == "New" || tyreitms.Tyre_make == "Old")
                                    select item.Product_name).Distinct().ToList();
            
         //   return Json(itemids, JsonRequestBehavior.AllowGet);
            //List<string> compname = (from item in db.Item_Tyres

            //                         where item.Company_name.Contains(term)
            //                         select item.Company_name).Distinct().ToList();

            if (compname.Count == 0)
            {
                compname.Add("No company name is matched with this entry...");
            }
            return Json(compname, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCompNameTube(string term)
        {
            List<string> compname = (from item in db.Products_For_Sales
                                     join tyreitms in db.Item_Tubes on item.Item_tyre_Id equals tyreitms.Item_Id
                                     where item.Product_name.Contains(term) && item.Pieces > 0
                                     select item.Product_name).Distinct().ToList();
            //List<string> compname = (from item in db.Item_Tubes

            //                         where item.Company_name.Contains(term)
            //                         select item.Company_name).Distinct().ToList();

            if (compname.Count == 0)
            {
                compname.Add("No company name is matched with this entry...");
            }
            return Json(compname, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetProductNameWithServicesWithoutRawmat(string term)
        {
            List<string> pronm = (from item in db.Other_Products

                                  where item.Product_name.Contains(term) && item.Product_type != "Raw Material"
                                  select item.Product_name).Distinct().ToList();

            if (pronm.Count == 0)
            {
                pronm.Add("No product name is matched with this entry...");
            }
            return Json(pronm, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetProductNameWithoutServicesWithRawmat(string term)
        {
            List<string> pronm = (from item in db.Other_Products

                                  where item.Product_name.Contains(term) && item.Product_type != "Services"
                                  select item.Product_name).Distinct().ToList();

            if (pronm.Count == 0)
            {
                pronm.Add("No product name is matched with this entry...");
            }
            return Json(pronm, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetProductNameWithoutServicesWithoutRawmat(string term)
        {
            List<string> pronm = (from item in db.Other_Products

                                  where item.Product_name.Contains(term) && item.Product_type != "Raw Material" && item.Product_type != "Services"
                                  select item.Product_name).Distinct().ToList();

            if (pronm.Count == 0)
            {
                pronm.Add("No product name is matched with this entry...");
            }
            return Json(pronm, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetProdctname(string term)
        {
            List<string> pronm = (from item in db.Other_Products

                                  where item.Product_name.Contains(term) && item.Product_type != "Raw Material"
                                  select item.Product_name).Distinct().ToList();

            if (pronm.Count == 0)
            {
                pronm.Add("No product name is matched with this entry...");
            }
            return Json(pronm, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetProdctnamewithoutservices(string term)
        {
            List<string> pronm = (from item in db.Other_Products

                                  where item.Product_name.Contains(term) && item.Product_type != "Services" && item.Product_type != "Raw Material"
                                  select item.Product_name).Distinct().ToList();

            if (pronm.Count == 0)
            {
                pronm.Add("No product name is matched with this entry...");
            }
            return Json(pronm, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetProdctnamewithoutservice(string term)
        {
            List<string> pronm = (from item in db.Other_Products

                                  where (item.Product_name.Contains(term) && item.Product_type != "Services")
                                  select item.Product_name).Distinct().ToList();

            if (pronm.Count == 0)
            {
                pronm.Add("No product name is matched with this entry...");
            }
            return Json(pronm, JsonRequestBehavior.AllowGet);
        }
       // GET: ProductsForSale
        public ActionResult Index()
        {
            var isstock = db.Products_For_Sales.Where(z => z.Approve == false).Any();
            if (isstock)
                ViewBag.isstock = "1";
            var isreqsnd= db.Products_For_Sales.Where(z => z.requestsend == true && z.Approve == false).Any();
            if (isreqsnd)
            {
                EasyBillingEntities dban = new EasyBillingEntities();
                List<Temp_Stock> temp_StockList = new List<Temp_Stock>();
                Temp_Stock temp_Stock = new Temp_Stock();
                temp_StockList = dban.Temp_Stock.ToList();
                if (temp_StockList.Count > 0)
                {
                    dban.Temp_Stock.RemoveRange(temp_StockList);
                    dban.SaveChanges();
                }
                temp_StockList = new List<Temp_Stock>();
                ViewBag.isreqsnd = "1";
                var stkall = db.Products_For_Sales.Where(z => z.requestsend == true && z.Approve == false).ToList();
                foreach (var eachstk in stkall)
                {
                    temp_Stock = new Temp_Stock();

                    temp_Stock.Token_Number = eachstk.Token_Number;

                    temp_Stock.Product_Token = eachstk.Product_Token;

                    temp_Stock.Product_name = eachstk.Product_name;
                    temp_Stock.Pieces = eachstk.Pieces;

                    temp_Stock.Amout_after_tax = eachstk.Amout_after_tax;

                    temp_Stock.Total = eachstk.Total;

                    temp_Stock.Tyre_Size = eachstk.Tyre_Size;

                    temp_Stock.Supplier_token = eachstk.Supplier_token;

                    temp_Stock.Supplier_name = eachstk.Supplier_name;

                    temp_Stock.Date = eachstk.Date;
                    temp_Stock.Purchase_Price = eachstk.Purchase_Price;

                    temp_Stock.CGST = eachstk.CGST;

                    temp_Stock.SGST = eachstk.SGST;

                    temp_Stock.Administrator_Token_number = eachstk.Administrator_Token_number;

                    temp_Stock.Administrator_name = eachstk.Administrator_name;

                    temp_Stock.Approve_date = eachstk.Approve_date;
                    temp_Stock.Approve = eachstk.Approve;

                    temp_Stock.Selling_Price = eachstk.Selling_Price;
                    temp_Stock.StockIn = eachstk.StockIn;

                    temp_Stock.Up_Selling_Price = eachstk.Up_Selling_Price;

                    temp_Stock.Up_CGST = eachstk.Up_CGST;

                    temp_Stock.Up_SGST = eachstk.Up_SGST;
                    temp_Stock.Delivery_contact_number = eachstk.Delivery_contact_number;

                    temp_Stock.Delivery_address = eachstk.Delivery_address;
                    temp_Stock.Item_tyre_Id = eachstk.Item_tyre_Id;
                    temp_Stock.Purchase_number = eachstk.Purchase_number;
                    temp_Stock.PurchaseDate = eachstk.PurchaseDate;
                    temp_Stock.Tyre_make = eachstk.Tyre_make;

                    temp_Stock.Tyre_type = eachstk.Tyre_type;

                    temp_Stock.Tyre_feel = eachstk.Tyre_feel;
                    temp_Stock.Vehicle_Token = eachstk.Vehicle_Token;

                    temp_Stock.Vehicle_type = eachstk.Vehicle_type;
                    temp_Stock.Description = eachstk.Description;
                    temp_Stock.Tyre_token = eachstk.Tyre_token;
                    temp_Stock.Item_tyre_token = eachstk.Item_tyre_token;
                    temp_Stock.Mac_id = eachstk.Mac_id;

                    temp_StockList.Add(temp_Stock);
                }
                db.Temp_Stock.AddRange(temp_StockList);
                db.SaveChanges();
                ViewBag.invno = db.Products_For_Sales.Where(z => z.requestsend == true && z.Approve == false).Select(x => x.Purchase_number).FirstOrDefault();
                return View(db.Products_For_Sales.Where(z => z.requestsend == true && z.Approve == false).FirstOrDefault());
            }
        
            //ViewBag.qty = db.Products_For_Sales.Select(x => x.Pieces).Sum();
            //ViewBag.ttlitm = db.Products_For_Sales.Select(x => x.Item_tyre_Id).ToList().Count();
            return View();
        }
        public JsonResult GetItemDetails(string id)
        {

            var itms = db.Item_Tyres.Select(x => new
            {
                x.Token_number,
                x.Company_token,
                x.Company_name,
                x.Item_Id,
                x.Tyre_feel,
                x.Tyre_make,
                x.Tyre_size,
                x.Tyre_type,
                x.Vehicle_type

            }).Where(z => z.Item_Id == id).FirstOrDefault();
            return Json(itms, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Getcheckingsuppliername(string id)
        {

            var itms = db.Dealers.Select(x => new
            {
                x.Name

            }).Where(z => z.Name == id).FirstOrDefault();
            return Json(itms, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetcheckingId(string id)
        {

            var itms = db.Products_For_Sales.Select(x => new
            {
                x.Item_tyre_Id

            }).Where(z => z.Item_tyre_Id == id.Trim()).FirstOrDefault();
            if(itms==null)
            {
              var  itmsnm = db.Products_For_Sales.Select(x => new
                {
                    x.Product_name

                }).Where(z => z.Product_name == id.Trim()).ToList();
                return Json(itmsnm, JsonRequestBehavior.AllowGet);
            }else
            return Json(itms, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetcheckingTubeId(string id)
        {
                var itmsnm = db.Products_For_Sales.Select(x => new
                {
                    x.Product_name

                }).Where(z => z.Product_name == id.Trim()).ToList();
                return Json(itmsnm, JsonRequestBehavior.AllowGet);
           
        }
        public JsonResult GetCustomerbyname(string id)
        {

            var itms = db.Customers.Select(x => new
            {
                x.Customer_Name, x.Address,x.Email,x.Phone_number,x.Vehicle_number,x.Token_number,x.Vehicle_type

            }).Where(z => z.Customer_Name == id).FirstOrDefault();
            return Json(itms, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCustomerbyNumber(string id)
        {

            var itms = db.Customers.Select(x => new
            {
                x.Token_number,
                x.Customer_Name,
                x.Address,
                x.Email,
                x.Phone_number,
                x.Vehicle_number,
                x.Vehicle_type

            }).Where(z => z.Phone_number == id).FirstOrDefault();
            return Json(itms, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCustomerbyVehcle(string id)
        {

            var itms = db.Customers.Select(x => new
            {
                x.Token_number,
                x.Customer_Name,
                x.Address,
                x.Email,
                x.Phone_number,
                x.Vehicle_number,
                x.Vehicle_type

            }).Where(z => z.Vehicle_number == id).FirstOrDefault();
            return Json(itms, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Getcheckingsupplieraddbyname(string id)
        {

            var itms = db.Dealers.Select(x => new
            {
                x.Address,
                x.Name,
                x.Pan_number,
                x.GST_number,
                x.Phone_number,
                x.Email

            }).Where(z => z.Name == id).FirstOrDefault();
            return Json(itms, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Getcheckingcustbyphn(string id)
        {

            var itms = db.Customers.Select(x => new
            {
                x.Address,
                x.Customer_Name,
               x.Phone_number,
                x.Email

            }).Where(z => z.Phone_number == id).FirstOrDefault();
            return Json(itms, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetVehicleNumber(string id)
        {
                var itms = db.Customers.Select(x => new
                {
                    x.Customer_Name,
                    x.Phone_number,
                    x.Vehicle_number,
                    x.Vehicle_type
                }).Where(z => z.Vehicle_number == id).FirstOrDefault();
                return Json(itms, JsonRequestBehavior.AllowGet);
           
        }
        public JsonResult GetCustmrname(string id)
        {
            var itmscnt = db.Customers.Select(x => new
            {
                x.Customer_Name,
                x.Phone_number,
                x.Vehicle_number,
                x.Vehicle_type
            }).Where(z => z.Customer_Name == id).Count();
            if(itmscnt>1)
            {
                var itms = db.Customers.Select(x => new
                {
                    x.Customer_Name,
                    x.Phone_number,
                  
                }).Where(z => z.Customer_Name == id).FirstOrDefault();
                return Json(itms, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var itms = db.Customers.Select(x => new
                {
                    x.Customer_Name,
                    x.Phone_number,
                    x.Vehicle_number,
                    x.Vehicle_type
                }).Where(z => z.Customer_Name == id).FirstOrDefault();
                return Json(itms, JsonRequestBehavior.AllowGet);
            }
           

        }
        public JsonResult GetCustmrno(string id)
        {
            var itmscnt = db.Customers.Select(x => new
            {
                x.Customer_Name,
                x.Phone_number,
                x.Vehicle_number,
                x.Vehicle_type
            }).Where(z => z.Phone_number == id).Count();
            if (itmscnt > 1)
            {
                var itms = db.Customers.Select(x => new
                {
                    x.Customer_Name,
                    x.Phone_number,

                }).Where(z => z.Phone_number == id).FirstOrDefault();
                return Json(itms, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var itms = db.Customers.Select(x => new
                {
                    x.Customer_Name,
                    x.Phone_number,
                    x.Vehicle_number,
                    x.Vehicle_type
                }).Where(z => z.Phone_number == id).FirstOrDefault();
                return Json(itms, JsonRequestBehavior.AllowGet);
            }


        }
        public JsonResult Getcheckingtmp(string id)
        {

            var itms = db.Temp_Stock.Where(z => z.Item_tyre_Id == id).Distinct().FirstOrDefault();
            return Json(itms, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Getitemsbyid(string id)
        {
            var itms = db.Products_For_Sales.Where(z => z.Item_tyre_Id == id).OrderByDescending(z=>z.Approve_date).Distinct().FirstOrDefault();
            return Json(itms, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetStudents(string term)
        {
            //   var dcmlempty = decimal.Parse("0.000");
            List<string> _Products_For_Sales = (from pdfs in db.Products_For_Sales
                                                join stk in db.Stocks on pdfs.Token_Number equals stk.Product_Token
                                                where (stk.Pieces != 0) /*&& pdfs.Approve == true*/
                                                && pdfs.Product_name.Contains(term)
                                                select pdfs.Product_name).Distinct().ToList();
            if (_Products_For_Sales.Count == 0)
            {
                _Products_For_Sales.Add("No tyre is matched with this name...");
            }
            return Json(_Products_For_Sales, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Getsubcat(string term)
        {
            List<string> _Products_For_Sales = (from p in db.Products_For_Sales
                                                where p.Tyre_Size.Contains(term)
                                                select p.Tyre_Size).Distinct().ToList();

            if (_Products_For_Sales.Count == 0)
            {
                _Products_For_Sales.Add("No subcategory found. You can can create new one...");
            }
            return Json(_Products_For_Sales, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult Search(string keyword)
        {
            var dcmlempty = decimal.Parse("0.000");
            if (!string.IsNullOrEmpty(keyword))
            {

                var products = (from pdfs in db.Products_For_Sales
                                join stk in db.Stocks on pdfs.Token_Number equals stk.Product_Token
                                orderby pdfs.Date descending
                                where (stk.Pieces != 0) && pdfs.Approve == true
                                && pdfs.Product_name.Contains(keyword)
                                select pdfs).Distinct().ToList();

                return PartialView("_Products", products);
            }
            else
            {

                var prdall = (from pdfs in db.Products_For_Sales
                              join stk in db.Stocks on pdfs.Token_Number equals stk.Product_Token
                              orderby pdfs.Date descending
                              where (stk.Pieces != 0) && pdfs.Approve == true

                              select pdfs).Distinct().ToList();

                return PartialView("_Products", prdall);
            }

        }

        public PartialViewResult Filter(string key, int? page, string keysub)
        {
            var dcmlempty = decimal.Parse("0.000");
            if (!string.IsNullOrEmpty(key))
            {
                if (!string.IsNullOrEmpty(keysub))
                {
                    var products = (from pdfs in db.Products_For_Sales
                                    join stk in db.Stocks on pdfs.Token_Number equals stk.Product_Token
                                    orderby pdfs.Date descending
                                    where pdfs.Approve == true
                                    && pdfs.Product_name == key && pdfs.Tyre_Size == keysub
                                    select pdfs).Distinct().ToList().ToPagedList(page ?? 1, 9);
                    return PartialView("_CategoryProducts", products);
                }
                else
                {

                    var products = (from pdfs in db.Products_For_Sales
                                    join stk in db.Stocks on pdfs.Token_Number equals stk.Product_Token
                                    orderby pdfs.Date descending
                                    where pdfs.Approve == true
                                    && pdfs.Product_name == key
                                    select pdfs).Distinct().ToList().ToPagedList(page ?? 1, 9);
                    return PartialView("_CategoryProducts", products);
                }

            }
            else
            {

                var prdall = (from pdfs in db.Products_For_Sales
                              join stk in db.Stocks on pdfs.Token_Number equals stk.Product_Token
                              orderby pdfs.Date descending
                              where pdfs.Approve == true

                              select pdfs).Distinct().ToList().ToPagedList(page ?? 1, 9);

                return PartialView("_CategoryProducts", prdall);
            }

        }
        public PartialViewResult FilterForBilling(string key, int? page, string keysub)
        {
            var dcmlempty = decimal.Parse("0.000");
            if (!string.IsNullOrEmpty(key))
            {
                if (!string.IsNullOrEmpty(keysub))
                {
                    var products = (from pdfs in db.Products_For_Sales
                                    join stk in db.Stocks on pdfs.Token_Number equals stk.Product_Token
                                    orderby pdfs.Date descending
                                    where pdfs.Approve == true
                                    && pdfs.Product_name == key && pdfs.Tyre_Size == keysub
                                    select pdfs).Distinct().ToList().ToPagedList(page ?? 1, 9);
                    return PartialView("_BillingCategoryProducts", products);
                }
                else
                {

                    var products = (from pdfs in db.Products_For_Sales
                                    join stk in db.Stocks on pdfs.Token_Number equals stk.Product_Token
                                    orderby pdfs.Date descending
                                    where pdfs.Approve == true
                                    && pdfs.Product_name == key
                                    select pdfs).Distinct().ToList().ToPagedList(page ?? 1, 9);
                    return PartialView("_BillingCategoryProducts", products);
                }

            }
            else
            {

                var prdall = (from pdfs in db.Products_For_Sales
                              join stk in db.Stocks on pdfs.Token_Number equals stk.Product_Token
                              orderby pdfs.Date descending
                              where pdfs.Approve == true

                              select pdfs).Distinct().ToList().ToPagedList(page ?? 1, 9);

                return PartialView("_BillingCategoryProducts", prdall);
            }

        }
        //[Authorize]

        //public async Task<ActionResult> Billing(int? page, string keyword)
        //{
        //    var dcmlempty = decimal.Parse("0.000");
        //    var products_For_Sales = (from pdfs in db.Products_For_Sales
        //                              join stk in db.Stocks on pdfs.Token_Number equals stk.Product_Token

        //                              where (stk.Pieces != 0) && pdfs.Approve == true

        //                              select pdfs).Distinct();
        //    ViewBag.productstks = await (from pdfs in db.Products_For_Sales
        //                                 join stk in db.Stocks on pdfs.Token_Number equals stk.Product_Token
        //                                 orderby pdfs.Date descending
        //                                 where (stk.Pieces != 0) && pdfs.Approve == true

        //                                 select stk).Distinct().ToListAsync();
        //    ViewBag.billno = null;
        //    var results = (from p in db.Products_For_Sales

        //                   group p.Tyre_Size by p.Product_name into g

        //                   select new CategoryClass { Product_name = g.Key, Subcategory = g.Distinct().ToList() }).ToList();
        //    ViewBag.ctgry = results;
        //    if (!string.IsNullOrEmpty(keyword))
        //    {
        //        return View(products_For_Sales.OrderByDescending(z => z.Date).Where(f => f.Product_name.Contains(keyword)).Distinct().ToList().ToPagedList(page ?? 1, 9));
        //    }
        //    return View(products_For_Sales.OrderByDescending(z => z.Date).Distinct().ToList().ToPagedList(page ?? 1, 9));
        //}
        [Authorize]
        public async Task<ActionResult> Billing()
        {
            ViewBag.vehicletype = db.Vehicles.Select(s => new VehicleClass()
            {
                Token_number = s.Token_number,

                Vehicle_type = s.Vehicle_type + " + " + s.Vehicle_make

            }).Distinct().ToList();
            if (db.Temp_Bill.Count() > 0)
            {
                db.Temp_Bill.RemoveRange(db.Temp_Bill.ToList());
                await db.SaveChangesAsync();
            }
            var dcmlempty = decimal.Parse("0.000");
            var products_For_Saleschk = db.Products_For_Sales.Where(z => z.Approve == false).Any();
            if (products_For_Saleschk)
                ViewBag.chk = "1";
            var billno = db.Billing_Masters.OrderByDescending(z => z.Billing_Number).Select(z => z.Billing_Number).FirstOrDefault();

            List<BillingMasterClass> BMlist = new List<BillingMasterClass>();

            BillingMasterClass BMfnal = null;
            using (var ctx = new EasyBillingEntities())
            {
                var pmexits = ctx.Billing_Masters.Select(s => new BillingMasterClass()
                {
                    Billing_Number = s.Billing_Number
                }).Any();
                var pmexitsforquot = ctx.Quotation_Masters.Select(s => new BillingMasterClass()
                {
                    Billing_Number = s.Quotation_Number
                }).Any();
                var pmexitsforord = ctx.Order_Masters.Select(s => new BillingMasterClass()
                {
                    Billing_Number = s.Order_Number
                }).Any();
                if (pmexits == true)
                {
                    BMlist.Add(ctx.Billing_Masters.Select(s => new BillingMasterClass()
                    {
                        Billing_Number = s.Billing_Number
                    }).OrderByDescending(o => o.Billing_Number).FirstOrDefault());

                }
                if (pmexitsforquot == true)
                {
                    BMlist.Add(ctx.Quotation_Masters.Select(s => new BillingMasterClass()
                    {
                        Billing_Number = s.Quotation_Number
                    }).OrderByDescending(o => o.Billing_Number).FirstOrDefault());

                }
                if (pmexitsforord == true)
                {
                    BMlist.Add(ctx.Order_Masters.Select(s => new BillingMasterClass()
                    {
                        Billing_Number = s.Order_Number
                    }).OrderByDescending(o => o.Billing_Number).FirstOrDefault());
                }
                BMfnal = BMlist.OrderByDescending(o => o.Billing_Number).FirstOrDefault();
            }
            //ViewBag.billno = BMfnal;
            if (BMfnal != null)
            {
                var fstfr = BMfnal.Billing_Number.Substring(0, 4);
                var lstfr = BMfnal.Billing_Number.Substring(BMfnal.Billing_Number.Length - 6);
                string newlstversn = (int.Parse(lstfr) + 1000001).ToString();
                string fstfr1 = (newlstversn.Substring(newlstversn.Length - 6)).ToString();
                String totalvrsn = fstfr + fstfr1;
                ViewBag.billno = totalvrsn;
                //using (var client = new HttpClient())
                //{
                //    var purchlastid = client.GetAsync("http://localhost:8087/api/Billing/GetBillinglastId");
                //    purchlastid.Wait();
                //    var lstid = purchlastid.Result;
                //    ViewBag.billno = lstid;
                //if (lstid.IsSuccessStatusCode)
                //{
                //var readTask3 = lstid.Content.ReadAsAsync<Billing_Master>();
                //readTask3.Wait();
                //var lst = readTask3.Result;
                //var text = lst.Billing_Number;
                //var fstfr = text.Substring(0, 4);
                //var lstfr = text.Substring(text.Length - 6);
                //string newlstversn = (int.Parse(lstfr) + 1000001).ToString();
                //string fstfr1 = (newlstversn.Substring(newlstversn.Length - 6)).ToString();
                //String totalvrsn = fstfr + fstfr1;
                //ViewBag.billno = totalvrsn;
                }
                else //web api sent error response 
                {
                    ViewBag.billno = "KANP000001";
                }

                //}
                //}
                //    else //web api sent error response 
                //            {
                //        ViewBag.billno = "KANP000001";
                //    }
                return View();
        }

        // GET: ProductsForSale/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products_For_Sale products_For_Sale = await db.Products_For_Sales.FindAsync(id);
            if (products_For_Sale == null)
            {
                return HttpNotFound();
            }
            var chk = await db.Stocks.Where(z => z.Product_Token == id).Select(x => new { x.Pieces }).Distinct().FirstOrDefaultAsync();
            if (chk.Pieces == int.Parse("0"))
            {
                ViewBag.stkin = "0";

            }
            else
            {
                ViewBag.stkin = chk.Pieces.ToString();

            }
            return View(products_For_Sale);
        }

        public JsonResult GetProducts(string id)
        {

            var products = db.Products.Select(x => new
            {

                x.Token_Number

            }).Where(z => z.Token_Number == id).FirstOrDefault();
            return Json(products, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult AddCustomer(Products_For_Sale products_For_Sale)
        {
            return View();
        }
        public ActionResult ProductDetails()
        {
            return View();
        }
        public ActionResult RateUpdate(string id)
        {
            string selected = id;
            
            List<Products_For_Sale> products_For_Sales = new List<Products_For_Sale>();
            List<Products_For_Sale> products_For_Salesall = new List<Products_For_Sale>();
            using (EasyBillingEntities db = new EasyBillingEntities())
            {
                ViewBag.tyresize = db.Tyre_sizes.ToList();
                if (!string.IsNullOrEmpty(selected))
                {
                    if (selected == "id=itmtyre")
                    {
                        products_For_Sales = (from x in db.Products_For_Sales
                                              where x.Approve == true && x.Item_tyre_Id.ToLower().StartsWith("ty")
                                              group x by x.Item_tyre_Id into grp
                                              select grp.OrderByDescending(y => y.Approve_date).FirstOrDefault()
                            ).ToList();
                        var newall = (from x in db.Item_Tyres
                                      where x.Tyre_make == "Resole"

                                      select x
                               ).ToList();
                        var newallforproducts = (from x in db.Other_Products
                                     
                                      select x
                             ).ToList();
                        Products_For_Sale products_For_Sale = new Products_For_Sale();
                        var User_Id = User.Identity.Name;
                        var User_name = db.Employees.Where(z => z.Employee_Id == User.Identity.Name).Select(z => z.Employee_name).FirstOrDefault();
                        foreach (var each in newall)
                        {
                            if (db.Products_For_Sales.Where(x => x.Item_tyre_Id == each.Item_Id).Any())
                            {
                                continue;
                            }
                            products_For_Sale = new Products_For_Sale();

                            products_For_Sale.Token_Number = Guid.NewGuid().ToString();
                            products_For_Sale.User_Id = User_Id;
                            products_For_Sale.User_name = User_name;

                            products_For_Sale.Product_name = each.Company_name;
                            products_For_Sale.Pieces = 1;

                            products_For_Sale.Amout_after_tax = 1;

                            products_For_Sale.Total = 1;
                            products_For_Sale.Tyre_Size = each.Tyre_size;
                            var supp = db.Dealers.Select(x => new { x.Token_number, x.Name }).FirstOrDefault();

                            products_For_Sale.Supplier_token = supp.Token_number;
                            products_For_Sale.Supplier_name = supp.Name;

                            products_For_Sale.Date = DateTime.Now;
                            products_For_Sale.Purchase_Price = 1;

                            products_For_Sale.CGST = 1;

                            products_For_Sale.SGST = 1;


                            products_For_Sale.Approve_date = DateTime.Now;
                            products_For_Sale.Approve = true;
                            products_For_Sale.Selling_Price = decimal.Parse("0.00");
                            products_For_Sale.Selling_CGST = decimal.Parse("0.00");
                            products_For_Sale.Selling_SGST = decimal.Parse("0.00");
                            products_For_Sale.Selling_net_total = decimal.Parse("0.00");
                            products_For_Sale.StockIn = 1;

                            products_For_Sale.Delivery_contact_number = 8334895299;
                            products_For_Sale.Item_tyre_Id = each.Item_Id;
                            products_For_Sale.Tyre_make = each.Tyre_make;

                            products_For_Sale.Tyre_type = each.Tyre_type;

                            products_For_Sale.Tyre_feel = each.Tyre_feel;

                            products_For_Sale.Vehicle_type = each.Vehicle_type;
                            products_For_Sale.requestsend = false;



                            products_For_Salesall.Add(products_For_Sale);
                            products_For_Sales.Add(products_For_Sale);
                        }
                        foreach (var each in newallforproducts)
                        {
                            if (db.Products_For_Sales.Where(x => x.Item_tyre_Id == each.Product_id).Any())
                            {
                                continue;
                            }
                            products_For_Sale = new Products_For_Sale();

                            products_For_Sale.Token_Number = Guid.NewGuid().ToString();
                            products_For_Sale.User_Id = User_Id;
                            products_For_Sale.User_name = User_name;

                            products_For_Sale.Product_name = each.Product_name;
                            products_For_Sale.Pieces = 1;

                            products_For_Sale.Amout_after_tax = 1;

                            products_For_Sale.Total = 1;
                            
                            var supp = db.Dealers.Select(x => new { x.Token_number, x.Name }).FirstOrDefault();

                            products_For_Sale.Supplier_token = supp.Token_number;
                            products_For_Sale.Supplier_name = supp.Name;

                            products_For_Sale.Date = DateTime.Now;
                            products_For_Sale.Purchase_Price = 1;

                            products_For_Sale.CGST = 1;

                            products_For_Sale.SGST = 1;
                            
                            products_For_Sale.Approve_date = DateTime.Now;
                            products_For_Sale.Approve = true;
                            products_For_Sale.Selling_Price = decimal.Parse("0.00");
                            products_For_Sale.Selling_CGST = decimal.Parse("0.00");
                            products_For_Sale.Selling_SGST = decimal.Parse("0.00");
                            products_For_Sale.Selling_net_total = decimal.Parse("0.00");
                            products_For_Sale.StockIn = 1;

                            products_For_Sale.Delivery_contact_number = 8334895299;
                            products_For_Sale.Item_tyre_Id = each.Product_id;
                        
                            products_For_Sale.requestsend = false;
                            
                            products_For_Salesall.Add(products_For_Sale);
                            products_For_Sales.Add(products_For_Sale);
                        }
                        if (products_For_Salesall.Count > 0)
                        {
                            db.Products_For_Sales.AddRange(products_For_Salesall);
                            db.SaveChanges();
                        }
                      
                    }
                    else if (selected == "id=itmtube")
                    {
                        products_For_Sales = (from x in db.Products_For_Sales
                                              where x.Approve == true && x.Item_tyre_Id.ToLower().StartsWith("tu")
                                              group x by x.Item_tyre_Id into grp
                                              select grp.OrderByDescending(y => y.Approve_date).FirstOrDefault()
                            ).ToList();
                    }
                    else if (selected == "id=prdct")
                    {
                        products_For_Sales = (from x in db.Products_For_Sales
                                              where x.Approve == true && x.Item_tyre_Id.ToLower().StartsWith("po")
                                              group x by x.Item_tyre_Id into grp
                                              select grp.OrderByDescending(y => y.Approve_date).FirstOrDefault()
                            ).ToList();
                    }
                    else
                    {
                        var zerodecimal = decimal.Parse("0.00");
                        products_For_Sales = (from x in db.Products_For_Sales
                                              where x.Approve == true && x.Selling_Price * x.Pieces < x.Purchase_Price
                                              group x by x.Item_tyre_Id into grp
                                              select grp.OrderByDescending(y => y.Approve_date).FirstOrDefault()
                            ).ToList();
                    }
                    return View(products_For_Sales.Where(x => x.Purchase_Price > 1).ToList());
                }
                else
                {
                    var alllist = (from x in db.Products_For_Sales
                                   where x.Approve == true
                                   group x by x.Item_tyre_Id into grp
                                   select grp.OrderByDescending(y => y.Approve_date).FirstOrDefault()
                                ).ToList();

                    var newall = (from x in db.Item_Tyres
                                  where x.Tyre_make == "Resole"

                                  select x
                                ).ToList();
                    var newallforproducts = (from x in db.Other_Products
                                             where x.Product_type != "Raw Material" && x.Product_type != "Services"
                                             select x
                            ).ToList();
                   
                    Products_For_Sale products_For_Sale = new Products_For_Sale();
               
                    // Products_For_Sale products_For_SaleForsave = new Products_For_Sale();
                    var User_Id = User.Identity.Name;
                    var User_name = db.Employees.Where(z => z.Employee_Id == User.Identity.Name).Select(z => z.Employee_name).FirstOrDefault();
                    foreach (var each in newall)
                    {
                        if (db.Products_For_Sales.Where(x => x.Item_tyre_Id == each.Item_Id).Any())
                        {
                            continue;
                        }
                        products_For_Sale = new Products_For_Sale();

                        products_For_Sale.Token_Number = Guid.NewGuid().ToString();
                        products_For_Sale.User_Id = User_Id;
                        products_For_Sale.User_name = User_name;

                        products_For_Sale.Product_name = each.Company_name;
                        products_For_Sale.Pieces = 1;

                        products_For_Sale.Amout_after_tax = 1;

                        products_For_Sale.Total = 1;
                        products_For_Sale.Tyre_Size = each.Tyre_size;
                        var supp = db.Dealers.Select(x => new { x.Token_number,x.Name}).FirstOrDefault();
                   
                        products_For_Sale.Supplier_token = supp.Token_number;
                        products_For_Sale.Supplier_name = supp.Name;

                        products_For_Sale.Date = DateTime.Now;
                        products_For_Sale.Purchase_Price = 1;

                        products_For_Sale.CGST = 1;

                        products_For_Sale.SGST = 1;
                        

                        products_For_Sale.Approve_date = DateTime.Now;
                        products_For_Sale.Approve = true;
                                products_For_Sale.Selling_Price = decimal.Parse("0.00");
                        products_For_Sale.Selling_CGST = decimal.Parse("0.00");
                        products_For_Sale.Selling_SGST = decimal.Parse("0.00");
                        products_For_Sale.Selling_net_total = decimal.Parse("0.00");
                        products_For_Sale.StockIn = 1;

                        products_For_Sale.Delivery_contact_number = 8334895299;
                        products_For_Sale.Item_tyre_Id = each.Item_Id;
                        products_For_Sale.Tyre_make = each.Tyre_make;

                        products_For_Sale.Tyre_type = each.Tyre_type;

                        products_For_Sale.Tyre_feel = each.Tyre_feel;

                        products_For_Sale.Vehicle_type = each.Vehicle_type;
                        products_For_Sale.requestsend = false;
                        
                        products_For_Salesall.Add(products_For_Sale);
                        alllist.Add(products_For_Sale);
                    }
                    foreach (var each in newallforproducts)
                    {
                        if (db.Products_For_Sales.Where(x => x.Item_tyre_Id == each.Product_id).Any())
                        {
                            continue;
                        }
                        products_For_Sale = new Products_For_Sale();

                        products_For_Sale.Token_Number = Guid.NewGuid().ToString();
                        products_For_Sale.User_Id = User_Id;
                        products_For_Sale.User_name = User_name;

                        products_For_Sale.Product_name = each.Product_name;
                        products_For_Sale.Pieces = 1;

                        products_For_Sale.Amout_after_tax = 1;

                        products_For_Sale.Total = 1;

                        var supp = db.Dealers.Select(x => new { x.Token_number, x.Name }).FirstOrDefault();

                        products_For_Sale.Supplier_token = supp.Token_number;
                        products_For_Sale.Supplier_name = supp.Name;

                        products_For_Sale.Date = DateTime.Now;
                        products_For_Sale.Purchase_Price = 1;

                        products_For_Sale.CGST = 1;

                        products_For_Sale.SGST = 1;


                        products_For_Sale.Approve_date = DateTime.Now;
                        products_For_Sale.Approve = true;
                        products_For_Sale.Selling_Price = decimal.Parse("0.00");
                        products_For_Sale.Selling_CGST = decimal.Parse("0.00");
                        products_For_Sale.Selling_SGST = decimal.Parse("0.00");
                        products_For_Sale.Selling_net_total = decimal.Parse("0.00");
                        products_For_Sale.StockIn = 1;

                        products_For_Sale.Delivery_contact_number = 8334895299;
                        products_For_Sale.Item_tyre_Id = each.Product_id;

                        products_For_Sale.requestsend = false;

                        products_For_Salesall.Add(products_For_Sale);
                        alllist.Add(products_For_Sale);
                    }
                    if (products_For_Salesall.Count > 0)
                    {
                        db.Products_For_Sales.AddRange(products_For_Salesall);
                        db.SaveChanges();
                    }
                    
                    return View(alllist.Where(x => x.Purchase_Price > 1).ToList());
                }
            }
        }
        public ActionResult RateUpdateByselection(string id)
        {
            //dynamic json = jsonData;
            string selected = id;
            List<Products_For_Sale> products_For_Sales = new List<Products_For_Sale>();
            using (EasyBillingEntities db = new EasyBillingEntities())
            {
                if (!string.IsNullOrEmpty(selected))
                {
                    if (selected == "id=itmtyre")
                    {
                        products_For_Sales = db.Products_For_Sales.Where(z => z.Approve == true && z.Item_tyre_Id.ToLower().StartsWith("ty")).ToList();
                    }
                    else if (selected == "id=itmtube")
                    {
                        products_For_Sales = db.Products_For_Sales.Where(z => z.Approve == true && z.Item_tyre_Id.ToLower().StartsWith("tu")).ToList();
                    }
                    else
                    {
                        products_For_Sales = db.Products_For_Sales.Where(z => z.Approve == true && z.Item_tyre_Id.ToLower().StartsWith("po")).ToList();
                    }
                }
                ViewBag.tyresize = db.Tyre_sizes.ToList();
                return View(products_For_Sales);
            }
        }
        [HttpPost]
        public ActionResult RateUpdate(Products_For_Sale products_For_Sale)
        {
            using (EasyBillingEntities db = new EasyBillingEntities())
            {
                ViewBag.tyresize = db.Tyre_sizes.ToList();
                return View(db.Products_For_Sales.Where(z => z.Approve == true).ToList());
            }
        }
        public ActionResult awatingapproval()
        {
            using (EasyBillingEntities db = new EasyBillingEntities())
            {
                return View(db.Products_For_Sales.Where(z => z.Approve == false && z.requestsend!=true).FirstOrDefault());
            }
        }
        public async Task<ActionResult> Approve(string id)
        {
            using (EasyBillingEntities db = new EasyBillingEntities())
            {
                Products_For_Sale products_For_Sale = db.Products_For_Sales.Where(z => z.Token_Number == id).FirstOrDefault();
                products_For_Sale.Approve = true;
                bool chk = db.Stocks.Where(a => a.Product_Token == id).Any();
                if (chk == true)
                {
                    Stock stk = db.Stocks.Where(a => a.Product_Token == products_For_Sale.Token_Number).FirstOrDefault();
                    //stk.Date = DateTime.Now.Date;
                    stk.Remodify_Date = DateTime.Now.Date;
                    products_For_Sale.Approve_date = DateTime.Now.Date;
                    stk.Pieces = stk.Pieces + (int)products_For_Sale.StockIn;
                    
                    stk.Product_Token = products_For_Sale.Token_Number;
                    
                }
                else
                {

                    Stock stk = new Stock();
                    stk.Date = DateTime.Now.Date;
                    stk.Remodify_Date = DateTime.Now.Date;
                    products_For_Sale.Approve_date = DateTime.Now.Date;
                    stk.Pieces = (int)products_For_Sale.Pieces;

                    stk.Product_Token = products_For_Sale.Token_Number;
                    stk.Remodify_Date = DateTime.Now.Date;
                    stk.Remodify_pcs = 99;
                    stk.CGST = products_For_Sale.CGST;
                    stk.SGST = products_For_Sale.SGST;
                    db.Stocks.Add(stk);
                }

                await db.SaveChangesAsync();


                return RedirectToAction("awatingapproval");
            }
        }
        public ActionResult Reject(string id)
        {
            using (EasyBillingEntities db = new EasyBillingEntities())
            {
                Products_For_Sale products_For_Sale = db.Products_For_Sales.Where(z => z.Token_Number == id).FirstOrDefault();
                products_For_Sale.Approve = false;

                db.SaveChangesAsync();


                return RedirectToAction("awatingapproval");
            }
        }
        // GET: ProductsForSale/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.Product_Token = new SelectList(db.Products, "Token_Number", "Product_Code");
            ViewBag.prdct = db.Products.ToList();
            ViewBag.tyresize = db.Tyre_sizes.ToList();
            ViewBag.dlr = db.Dealers.Distinct().ToList();
            ViewBag.itmtyres = db.Item_Tyres.Distinct().ToList();
            using (var client = new HttpClient())
            {

                var responseTask1 = client.GetAsync("http://localhost:8087/api/Dealer/GetAllDealers");
                responseTask1.Wait();

                var purchlastid = client.GetAsync("http://localhost:8087/api/Purchase/GetPurchaselastId");
                purchlastid.Wait();


                var result = responseTask1.Result;

                var lstid = purchlastid.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask1 = result.Content.ReadAsAsync<IList<Dealer>>();
                    readTask1.Wait();
                    ViewBag.dlr = readTask1.Result;

                }
                else //web api sent error response 
                {
                    ViewBag.dlr = Enumerable.Empty<Dealer>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
                if (lstid.IsSuccessStatusCode)
                {
                    var readTask3 = lstid.Content.ReadAsAsync<Purchase_Invoice>();
                    readTask3.Wait();
                    var lst = readTask3.Result;
                    var text = lst.Purchase_invoice_number;
                    var fstfr = text.Substring(0, 4);
                    var lstfr = text.Substring(text.Length - 7);
                    string newlstversn = (int.Parse(lstfr) + 10000001).ToString();
                    string fstfr1 = (newlstversn.Substring(newlstversn.Length - 7)).ToString();
                    String totalvrsn = fstfr + fstfr1;
                    ViewBag.lstid = totalvrsn;
                }
                else //web api sent error response 
                {
                    ViewBag.lstid = "PRCH0000001";
                }
            }
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> Create(Products_For_Sale products_For_Sale)
        {
            products_For_Sale.Token_Number = Guid.NewGuid().ToString();
            if (ModelState.IsValid)
            {

                products_For_Sale.Product_name = db.Products.Where(z => z.Token_Number == products_For_Sale.Product_Token).Select(z => z.Product_name).FirstOrDefault();

                products_For_Sale.Approve = false;
                products_For_Sale.Supplier_name = (from sup in db.Dealers
                                                   where sup.Token_number == products_For_Sale.Supplier_token
                                                   select sup.Name).FirstOrDefault();

                products_For_Sale.Selling_Price = 0;
                products_For_Sale.Up_Selling_Price = 0;
                products_For_Sale.Total = 0;
                products_For_Sale.Tyre_Size = (from sup in db.Tyre_sizes
                                               where sup.Token_number == products_For_Sale.Tyre_token
                                               select sup.Tyre_size1).FirstOrDefault();

                products_For_Sale.Vehicle_type = (from sup in db.Vehicles
                                                  where sup.Token_number == products_For_Sale.Vehicle_Token
                                                  select sup.Vehicle_type).FirstOrDefault();

                Purchase_Invoice purchase_Invoice = new Purchase_Invoice();

                purchase_Invoice.Token_number = Guid.NewGuid().ToString();
                purchase_Invoice.Purchase_invoice_number = products_For_Sale.Purchase_number;
                bool chkinv = db.Purchase_Invoices.Where(z => z.Purchase_invoice_number == purchase_Invoice.Purchase_invoice_number).Any();
                if (chkinv == true)
                {
                    return Json(purchase_Invoice.Purchase_invoice_number + " number already exist. Try with another.");
                }
                else
                {
                    purchase_Invoice.Date = products_For_Sale.PurchaseDate;
                    purchase_Invoice.Stock_entry_token = products_For_Sale.Token_Number;
                    // products_For_Sale.Product = db.Products.Where(x=>x.Token_Number==products_For_Sale.Product_Token).FirstOrDefault();
                    db.Products_For_Sales.Add(products_For_Sale);
                    db.Purchase_Invoices.Add(purchase_Invoice);
                    await db.SaveChangesAsync();
                    var _data = db.Products_For_Sales.Select(z => new { z.Date, z.Tyre_make, z.Product_name, z.Tyre_Size, z.Supplier_name, z.Pieces, z.Approve }).ToList();

                    //string json = JsonConvert.SerializeObject(_data.ToArray());

                    // json = "{\"data\":" + json + "}";

                    var par = Server.MapPath("~/Json/Stock.json");
                    System.IO.File.WriteAllText(par, "{\"data\":" + JsonConvert.SerializeObject(_data.ToArray(), Formatting.Indented,
new JsonSerializerSettings()
{
    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
}
) + "}");
                    return Json(products_For_Sale);
                }

            }
            else
            {
                return Json("Something went wrong. Please check your data and try again.");
            }



        }
        //public async Task<ActionResult> Create( IList<Products_For_Sale> products_For_SaleAsList, Products_For_Sale products_For_SaleAsList1, Purchase_Master purchase, HttpPostedFileBase Image)
        //{
        //    Image = Request.Files["Image"];
        //    if (purchase!=null)
        //        {
        //        using (var client = new HttpClient())
        //        {

        //            client.BaseAddress = new Uri("http://localhost:8087/api/Purchase");

        //            //HTTP POST
        //            var postTask = client.PostAsJsonAsync("http://localhost:8087/api/Purchase/PostNewMerchant?BussinessId="+User.Identity.Name, purchase);
        //            postTask.Wait();

        //            var result = postTask.Result;

        //            if (!result.IsSuccessStatusCode)
        //            {
        //                ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
        //                return new JsonResult { Data = new { status = false } };
        //            }

        //        }
        //    }
        //    int count = 0;
        //    foreach (var products_For_Sale in products_For_SaleAsList)
        //    {

        //        if (ModelState.ContainsKey("products_For_SaleAsList[" + count + "].Date"))
        //            ModelState["products_For_SaleAsList[" + count + "].Date"].Errors.Clear();
        //        count++;
        //    }
        //        foreach (var products_For_Sale in products_For_SaleAsList)
        //    {

        //        if (ModelState.IsValid)
        //        {
        //            products_For_Sale.Product_Token = products_For_Sale.Product_name;
        //            products_For_Sale.Date = DateTime.Now.Date;
        //            products_For_Sale.Token_Number = Guid.NewGuid().ToString();

        //            products_For_Sale.Administrator_Token_number = (from mrchnt in db.Marchent_Accounts
        //                                                       where mrchnt.Email_Id == User.Identity.Name
        //                                                       select mrchnt.Token_number).Distinct().FirstOrDefault();
        //            products_For_Sale.Approve = false;

        //            var productdata = (from prdct in db.Products
        //                               where prdct.Token_Number == products_For_Sale.Product_name
        //                               select new { prdct.Product_name, prdct.Description, prdct.GL_CODE, prdct.Product_Code }).Distinct().FirstOrDefault();
        //            if (productdata != null)
        //            {
        //                products_For_Sale.Product_name = productdata.Product_name;

        //                db.Products_For_Sales.Add(products_For_Sale);

        //                bool chk = db.Stocks.Where(a => a.Product_Token == products_For_Sale.Token_Number).Any();
        //                if (chk == true)
        //                {
        //                    Stock stk = db.Stocks.Where(a => a.Product_Token == products_For_Sale.Token_Number).FirstOrDefault();
        //                    stk.Date = DateTime.Now.Date;

        //                        stk.Pieces = stk.Pieces + (int)products_For_Sale.StockIn;



        //                    stk.Product_Token = products_For_Sale.Token_Number;


        //                }
        //                else
        //                {

        //                    Stock stk = new Stock();
        //                    stk.Date = DateTime.Now.Date;

        //                        stk.Pieces = (int)products_For_Sale.StockIn;


        //                    stk.Product_Token = products_For_Sale.Token_Number;
        //                    db.Stocks.Add(stk);


        //                }
        //                await db.SaveChangesAsync();

        //            }
        //            else
        //            {
        //                ViewBag.prdct = db.Products.ToList();
        //                ViewBag.Product_Token = new SelectList(db.Products, "Token_Number", "Product_Code", products_For_Sale.Product_Token);
        //                ModelState.AddModelError(string.Empty, "Product doesn't exists. Please check your product");
        //                return new JsonResult { Data = new { status = false, error = "Product doesn't exists. Please check your product" } };
        //            }
        //        }
        //        else
        //        {
        //            return new JsonResult { Data = new { status = false } };
        //        }

        //    }
        //    return new JsonResult { Data = new { status = true } };
        //}
        //public async Task<ActionResult> Create([Bind(Include = "Token_Number,Name,Product_Token,Product_Code,Product_name,Description,HSN_SAC_Code,Tax_rate,GL_CODE,Sell_On,Pieces,Weight,Unit,Amount,Amout_after_tax,Date,Discount,Total,Single_product_not_combo_multi_,Approve,Marchent_Token_number,StockIn")] Products_For_Sale products_For_Sale)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        products_For_Sale.Product_Token = products_For_Sale.Product_name;

        //        products_For_Sale.Token_Number = Guid.NewGuid().ToString();
        //        products_For_Sale.Marchent_Token_number = (from mrchnt in db.Marchent_Accounts
        //                                                   where mrchnt.Email_Id == User.Identity.Name
        //                                                   select mrchnt.Token_number).Distinct().FirstOrDefault();
        //        products_For_Sale.Approve = true;
        //        products_For_Sale.Single_product_not_combo_multi_ = true;
        //        var productdata = (from prdct in db.Products
        //                           where prdct.Token_Number == products_For_Sale.Product_name
        //                           select new { prdct.Product_name, prdct.Description, prdct.GL_CODE, prdct.Product_Code }).Distinct().FirstOrDefault();
        //        if (productdata != null)
        //        {
        //            products_For_Sale.Product_name = productdata.Product_name;
        //            products_For_Sale.Product_Code = productdata.Product_Code;
        //            if (products_For_Sale.Description == null)
        //            {
        //                products_For_Sale.Description = productdata.Description;
        //            }
        //            products_For_Sale.GL_CODE = productdata.GL_CODE;
        //            db.Products_For_Sales.Add(products_For_Sale);

        //            bool chk = db.Stocks.Where(a => a.Product_Token == products_For_Sale.Token_Number).Any();
        //            if (chk == true)
        //            {
        //                Stock stk = db.Stocks.Where(a => a.Product_Token == products_For_Sale.Token_Number).FirstOrDefault();
        //                stk.Date = DateTime.Now.Date;
        //                if(products_For_Sale.Sell_On.ToLower()=="pieces" || products_For_Sale.Sell_On.ToLower() == "plate")
        //                {
        //                    stk.Pieces = stk.Pieces + (int)products_For_Sale.StockIn;

        //                }
        //                else
        //                {
        //                    stk.Quantity = stk.Quantity + products_For_Sale.StockIn;
        //                }

        //                stk.Product_Token = products_For_Sale.Token_Number;


        //            }
        //            else
        //            {

        //                Stock stk = new Stock();
        //                stk.Date = DateTime.Now.Date;
        //                if (products_For_Sale.Sell_On.ToLower() == "pieces" || products_For_Sale.Sell_On.ToLower() == "plate")
        //                {
        //                    stk.Pieces = (int)products_For_Sale.StockIn;
        //                    stk.Quantity = decimal.Parse("0.000");
        //                }
        //                else
        //                {
        //                    stk.Quantity = products_For_Sale.StockIn;
        //                    stk.Pieces = 0;
        //                }

        //                stk.Product_Token = products_For_Sale.Token_Number;
        //                db.Stocks.Add(stk);


        //            }
        //            await db.SaveChangesAsync();
        //            return RedirectToAction("Create");
        //        }
        //        else
        //        {
        //            ViewBag.prdct = db.Products.ToList();
        //            ViewBag.Product_Token = new SelectList(db.Products, "Token_Number", "Product_Code", products_For_Sale.Product_Token);
        //            ModelState.AddModelError(string.Empty, "Product doesn't exists. Please check your product");
        //            return View(products_For_Sale);
        //        }
        //    }
        //    ViewBag.prdct = db.Products.ToList();
        //    ViewBag.Product_Token = new SelectList(db.Products, "Token_Number", "Product_Code", products_For_Sale.Product_Token);
        //    return View(products_For_Sale);
        //}

        // GET: ProductsForSale/Edit/5
        public async Task<ActionResult> Edit(string id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products_For_Sale products_For_Sale = await db.Products_For_Sales.FindAsync(id);
            if (products_For_Sale == null)
            {
                return HttpNotFound();
            }

            products_For_Sale.StockIn = db.Stocks.Where(a => a.Product_Token == products_For_Sale.Token_Number).Select(x => x.Pieces).Distinct().FirstOrDefault();
            products_For_Sale.StockIn = (int)products_For_Sale.StockIn;

            ViewBag.Product_Token = new SelectList(db.Products, "Token_Number", "Product_Code", products_For_Sale.Product_Token);
            ViewBag.prdct = db.Products.OrderByDescending(x => x.Token_Number == products_For_Sale.Product_Token).ToList();
            ViewBag.prdctselected = await (from a in db.Products
                                           where a.Token_Number == products_For_Sale.Product_Token

                                           select new { a.Token_Number, a.Product_name }).Distinct().FirstOrDefaultAsync();
            return View(products_For_Sale);
        }

        // POST: ProductsForSale/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Token_Number,Name,Product_Token,Product_Code,Product_name,Description,HSN_SAC_Code,Tax_rate,GL_CODE,Sell_On,Pieces,Weight,Unit,Amount,Amout_after_tax,Date,Discount,Total,Single_product_not_combo_multi_,Approve,Marchent_Token_number,StockIn,Subcategory,Image_path,Author_name,Publisher_name,Image")]  Products_For_Sale products_For_Sale, HttpPostedFileBase Image)
        {
            if (ModelState.IsValid)
            {
                products_For_Sale.Product_Token = products_For_Sale.Product_name;
                bool chk = db.Stocks.Where(a => a.Product_Token == products_For_Sale.Token_Number).Any();
                if (chk == true)
                {
                    Stock stk = db.Stocks.Where(a => a.Product_Token == products_For_Sale.Token_Number).FirstOrDefault();

                    stk.Date = DateTime.Now.Date;

                    stk.Pieces = (int)products_For_Sale.StockIn;

                    stk.Product_Token = products_For_Sale.Token_Number;
                }
                else
                {
                    ViewBag.Product_Token = new SelectList(db.Products, "Token_Number", "Product_Code", products_For_Sale.Product_Token);
                    return View(products_For_Sale);
                }



                products_For_Sale.Administrator_Token_number = (from mrchnt in db.Marchent_Accounts
                                                                where mrchnt.Email_Id == User.Identity.Name
                                                                select mrchnt.Token_number).Distinct().FirstOrDefault();
                products_For_Sale.Approve = true;

                var productdata = (from prdct in db.Products
                                   where prdct.Token_Number == products_For_Sale.Product_name
                                   select new { prdct.Product_name, prdct.Product_Code }).Distinct().FirstOrDefault();
                if (productdata != null)
                {
                    products_For_Sale.Product_name = productdata.Product_name;

                }
                db.Entry(products_For_Sale).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Product_Token = new SelectList(db.Products, "Token_Number", "Product_Code", products_For_Sale.Product_Token);
            return View(products_For_Sale);
        }

        // GET: ProductsForSale/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products_For_Sale products_For_Sale = await db.Products_For_Sales.FindAsync(id);
            if (products_For_Sale == null)
            {
                return HttpNotFound();
            }
            if (products_For_Sale.Approve == true)
            {
                products_For_Sale.Approve = false;
            }
            else
            {
                products_For_Sale.Approve = true;
            }
            //db.Products_For_Sales.Remove(products_For_Sale);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");

        }

        // POST: ProductsForSale/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            Products_For_Sale products_For_Sale = await db.Products_For_Sales.FindAsync(id);
            db.Products_For_Sales.Remove(products_For_Sale);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<ActionResult> Billing(FormCollection frm, int? page)
        {
            try
            {
                var results = (from p in db.Products_For_Sales

                               group p.Tyre_Size by p.Product_name into g

                               select new CategoryClass { Product_name = g.Key, Subcategory = g.Distinct().ToList() }).ToList();
                ViewBag.ctgry = results;
                Billing_Master billing = new Billing_Master();
                List<Billing_Detail> detail = new List<Billing_Detail>();

                Billing_Detail blldtl = new Billing_Detail();
                Stockout stkout = new Stockout();
                List<Stockout> stkoutlst = new List<Stockout>();

                Barcode_Master brcdmstr = new Barcode_Master();

                using (EasyBillingEntities dc = new EasyBillingEntities())
                {
                    bool ischk = dc.Billing_Masters.Any();
                    if (ischk == false)
                    {
                        billing.Billing_Number = "INV00000001";
                    }
                    else
                    {
                        var text = (from a in dc.Billing_Masters
                                    orderby a.Billing_Number descending
                                    select a.Billing_Number).FirstOrDefault();

                        var fstfr = text.Substring(0, 3);
                        var lstfr = text.Substring(text.Length - 8);
                        string newlstversn = (int.Parse(lstfr) + 100000001).ToString();
                        string fstfr1 = (newlstversn.Substring(newlstversn.Length - 8)).ToString();
                        String totalvrsn = fstfr + fstfr1;
                        billing.Billing_Number = totalvrsn;
                    }
                    var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

                    Random rd = new Random();

                    var chars1 = Enumerable.Range(0, 14)
               .Select(x => chars[rd.Next(0, chars.Length)]);
                    string barcode = new string(chars1.ToArray());

                    billing.Token_Number = (Guid.NewGuid()).ToString();
                    billing.Date = DateTime.Now.Date;
                    var mrchnttk = (from tkn in dc.Marchent_Accounts
                                    where tkn.Email_Id == User.Identity.Name
                                    select tkn.Token_number).Distinct().FirstOrDefault();
                  
                    var chkst = (from tkn in dc.Marchent_Accounts
                                 join st in dc.States on tkn.State_Code equals st.State_Code
                                 where tkn.Email_Id == User.Identity.Name
                                 select new { st.SGST, st.CGST, st.IGST, st.UTGST }).Distinct().FirstOrDefault();

                    billing.Total_tax = decimal.Parse("0.00");
                    billing.Rate_including_tax = decimal.Parse("0.00");
                   
                    billing.Total_discount = decimal.Parse("0.00");
                    billing.Total_amount = decimal.Parse("0.00");
                 

                    int b = 1;
                    int l = 0;
                    string customertoken = null;
                    foreach (var key in frm.Keys)
                    {
                        if (key.ToString() == "tokencust")
                        {
                            customertoken = frm["tokencust"];
                        }
                        else
                        {
                            if (l % 3 == 0)
                            {
                                var k = key.ToString();

                                int a = int.Parse(k.Substring(k.Length - 1, 1));
                                if (a == b)
                                { }
                                else
                                {
                                    b = a;
                                }
                                var Token = frm["token_" + a];
                                var Sellon = frm["sellon_" + a];
                                var Quant = int.Parse(frm["quantity_" + a]);

                                var stockitems = dc.Stocks.Where(x => x.Product_Token == Token).Distinct().FirstOrDefault();
                                var productforsaleitems = dc.Products_For_Sales.Where(x => x.Token_Number == Token).Distinct().FirstOrDefault();


                                if (stockitems != null)
                                {
                                    stockitems.Pieces = stockitems.Pieces - (productforsaleitems.Pieces * Quant);


                                    stockitems.Date = DateTime.Now.Date;

                                    if (stockitems.Pieces < 0)
                                    {


                                        var dcmlempty1 = decimal.Parse("0.000");
                                        var products_For_Sales1 = (from pdfs in db.Products_For_Sales
                                                                   join stk in db.Stocks on pdfs.Token_Number equals stk.Product_Token
                                                                   where (stk.Pieces != 0) && pdfs.Approve == true

                                                                   select pdfs).Distinct();
                                        ViewBag.productstks = await (from pdfs in db.Products_For_Sales
                                                                     join stk in db.Stocks on pdfs.Token_Number equals stk.Product_Token
                                                                     where (stk.Pieces != 0) && pdfs.Approve == true

                                                                     select stk).Distinct().ToListAsync();

                                        ViewBag.Error = "Please check your quantity";
                                        ModelState.AddModelError(string.Empty, "Please check your quantity");
                                        return View(products_For_Sales1.ToList().ToPagedList(page ?? 1, 9));
                                    }
                                    else
                                    {
                                        dc.SaveChanges();
                                    }
                                }

                                billing.Rate_including_tax = billing.Rate_including_tax + (productforsaleitems.Amout_after_tax * Quant);
                                billing.Total_discount = billing.Total_discount;
                                billing.Total_amount = billing.Total_amount + (productforsaleitems.Total * Quant);
                                billing.Total_tax = billing.Total_tax + ((productforsaleitems.Amout_after_tax * Quant) - (productforsaleitems.Selling_Price * Quant));
                             
                                blldtl.Billing_Token_number = billing.Token_Number;
                                blldtl.Billing_number = billing.Billing_Number;
                                blldtl.Date = billing.Date;
                               
                                blldtl.Pieces = productforsaleitems.Pieces * Quant;

                                blldtl.Amount = productforsaleitems.Selling_Price * Quant;
                             
                                blldtl.Tax = (productforsaleitems.Amout_after_tax * Quant) - (productforsaleitems.Selling_Price * Quant);
                                blldtl.Discount = decimal.Parse("0.00");
                               
                                blldtl.Sub_Total = productforsaleitems.Total * Quant;

                                stkout.Billing_Token_number = billing.Token_Number;
                                stkout.Billing_number = billing.Billing_Number;
                                stkout.Date = billing.Date;
                                stkout.Product_Token = Token;
                                stkout.Pieces = productforsaleitems.Pieces * Quant;


                                stkout.CGST = (productforsaleitems.Selling_Price * Quant) + ((productforsaleitems.CGST * Quant) / 100);
                                stkout.SGST = (productforsaleitems.Selling_Price * Quant) + ((productforsaleitems.SGST * Quant) / 100);
                                stkout.Sub_Total = productforsaleitems.Total * Quant;
                                stkout.Marchent_Token_number = mrchnttk;

                                detail.Add(blldtl);
                                stkoutlst.Add(stkout);
                            }
                        }
                        l++;

                        blldtl = new Billing_Detail();
                        stkout = new Stockout();
                    }
                   
                    brcdmstr.Barcode_Number = barcode;
                    brcdmstr.Billing_Number = billing.Billing_Number;
                    brcdmstr.Billing_Token_number = billing.Token_Number;
                    brcdmstr.Date = billing.Date;

                   
                    // image save for barcode///

                    using (MemoryStream ms = new MemoryStream())
                    {
                        //The Image is drawn based on length of Barcode text.
                        using (Bitmap bitMap = new Bitmap(barcode.Length * 30, 90))
                        {
                            //The Graphics library object is generated for the Image.
                            using (Graphics graphics = Graphics.FromImage(bitMap))
                            {
                                //The installed Barcode font.
                                System.Drawing.Font oFont = new System.Drawing.Font("IDAutomationHC39M", 17);
                                PointF point = new PointF(2f, 2f);

                                //White Brush is used to fill the Image with white color.
                                SolidBrush whiteBrush = new SolidBrush(Color.White);
                                graphics.FillRectangle(whiteBrush, 0, 0, bitMap.Width, bitMap.Height);

                                //Black Brush is used to draw the Barcode over the Image.
                                SolidBrush blackBrush = new SolidBrush(Color.Black);
                                graphics.DrawString("*" + barcode + "*", oFont, blackBrush, point);
                            }

                            //The Bitmap is saved to Memory Stream.
                            bitMap.Save(ms, ImageFormat.Png);

                            //The Image is finally converted to Base64 string.


                            byte[] imageBytes = Convert.FromBase64String(Convert.ToBase64String(ms.ToArray()));

                            MemoryStream ms1 = new MemoryStream(imageBytes, 0, imageBytes.Length);
                            ms1.Write(imageBytes, 0, imageBytes.Length);

                            System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
                            string spath = System.Web.HttpContext.Current.Server.MapPath("~/BillBarcode");
                            string path = spath + "/" + billing.Billing_Number + ".jpg";
                            image.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);

                            //p.photo = imageBytes;
                    
                            brcdmstr.Image = imageBytes;
                        }

                    }
                    billing.Billing_Details = detail;
                    billing.Stockouts = stkoutlst;
                    billing.Barcode_Master.Add(brcdmstr);

                    dc.Billing_Masters.Add(billing);
                    dc.SaveChanges();
                }

                var dcmlempty = decimal.Parse("0.000");
                var products_For_Sales = (from pdfs in db.Products_For_Sales
                                          join stk in db.Stocks on pdfs.Token_Number equals stk.Product_Token
                                          where (stk.Pieces != 0) && pdfs.Approve == true

                                          select pdfs).Distinct();
                ViewBag.productstks = await (from pdfs in db.Products_For_Sales
                                             join stk in db.Stocks on pdfs.Token_Number equals stk.Product_Token
                                             where (stk.Pieces != 0) && pdfs.Approve == true

                                             select stk).Distinct().ToListAsync();
                ViewBag.Success = "Thanks for billing...";
                ViewBag.billno = billing.Billing_Number;
                return View(products_For_Sales.ToList().ToPagedList(page ?? 1, 9));
            }
            catch
            {
                var dcmlempty = decimal.Parse("0.000");
                var products_For_Sales = (from pdfs in db.Products_For_Sales
                                          join stk in db.Stocks on pdfs.Token_Number equals stk.Product_Token
                                          where (stk.Pieces != 0) && pdfs.Approve == true

                                          select pdfs).Distinct();
                ViewBag.productstks = await (from pdfs in db.Products_For_Sales
                                             join stk in db.Stocks on pdfs.Token_Number equals stk.Product_Token
                                             where (stk.Pieces != 0) && pdfs.Approve == true

                                             select stk).Distinct().ToListAsync();
                ModelState.AddModelError(string.Empty, "Something went wrong. Please try again");
                ViewBag.Error = "Something went wrong. Please try again...";
                return View(products_For_Sales.ToList().ToPagedList(page ?? 1, 9));
            }
        }

        [HttpPost]
        public JsonResult Bill(Billing_Master Billing)
        {

            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri("http://localhost:8087/api/Billing/PostProductBill");

                //HTTP POST
                var postTask = client.PostAsJsonAsync("http://localhost:8087/api/Billing/PostProductBill", Billing);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var nmbr = result.Content.ReadAsStringAsync()
                                               .Result
                                               .Replace("\\", "")
                                               .Trim(new char[1] { '"' });

                    return new JsonResult { Data = new { status = true, nmbr } };
                }
            }

            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");


            return new JsonResult { Data = new { status = false } };
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
