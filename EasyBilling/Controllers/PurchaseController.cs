using EasyBilling.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Mvc;

namespace EasyBilling.Controllers
{
    public class PurchaseController : MybaseController
    {
        // GET: Purchase
        public ActionResult Index()
        {
            List<PurchaseAll> allOrder = new List<PurchaseAll>();
            IList<Purchase_Master> purchMast = null;
            IList<Purchase_detail> purchdet = null;
          
            IList<Product> prdct = null;
            using (var db = new EasyBillingEntities())
            {
                using (var client = new HttpClient())
                {
                    var responseTaskMaster = client.GetAsync("http://localhost:8087/api/Purchase/GetPurchaseMasterList");
                    responseTaskMaster.Wait();
                    var responseTaskDetails = client.GetAsync("http://localhost:8087/api/Purchase/GetPurchaseDetailsList");
                    responseTaskDetails.Wait();
                    var responseTaskProduct = client.GetAsync("http://localhost:8087/api/Product/GetAllProducts");
                    responseTaskProduct.Wait();
                 
                    var resultMaster = responseTaskMaster.Result;
                    var resultDetails = responseTaskDetails.Result;
                    var resultProduct = responseTaskProduct.Result;
                  
                    if (resultMaster.IsSuccessStatusCode)
                    {
                        var readTaskMast = resultMaster.Content.ReadAsAsync<IList<Purchase_Master>>();
                        readTaskMast.Wait();

                        purchMast = readTaskMast.Result;

                    }
                    else //web api sent error response 
                    {
                        purchMast = null;

                        ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    }

                    if (resultDetails.IsSuccessStatusCode)
                    {
                        var readTaskStock = resultDetails.Content.ReadAsAsync<IList<Purchase_detail>>();
                        readTaskStock.Wait();

                        purchdet = readTaskStock.Result;

                    }
                    else //web api sent error response 
                    {
                        purchdet = null;

                        ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    }

                    if (resultProduct.IsSuccessStatusCode)
                    {
                        var readTaskPrdct = resultProduct.Content.ReadAsAsync<IList<Product>>();
                        readTaskPrdct.Wait();

                        prdct = readTaskPrdct.Result;

                    }
                    else //web api sent error response 
                    {
                        prdct = null;

                        ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    }

                    foreach (var i in purchMast)
                    {
                        //foreach(var od in purchStck )
                        //{
                        //    if (od.Purchase_Token_number == i.Token_Number)
                        //    {
                        var od = purchdet.Where(a => a.Purchase_Token_number.Equals(i.Token_Number)).ToList();
                        foreach (var k in od)
                        {

                            foreach (var l in prdct)
                            {
                                if (k.Product_Token == l.Token_Number)
                                {
                                    k.Product_name = l.Product_name;
                                }
                            }

                        }
                   
                        

                        allOrder.Add(new PurchaseAll { Purchasemast = i, PurchaseDet = od });

                    }

                    return View(allOrder);
                }
            }
        }
      
        public ActionResult Purchase_Entry()
        {
            IEnumerable<Tax_Group> txgrp = null;
            IEnumerable<Dealer> dlr = null;
            IEnumerable<Product> prdct = null;
            
            using (var client = new HttpClient())
            {
                var responseTask = client.GetAsync("http://localhost:8087/api/TaxGroup/GetAllUserTaxGroups");
                responseTask.Wait();
                var responseTask1 = client.GetAsync("http://localhost:8087/api/Dealer/GetAllDealers");
                responseTask1.Wait();
                var responseTask2 = client.GetAsync("http://localhost:8087/api/Product/GetAllProducts");
                responseTask2.Wait();
                var purchlastid = client.GetAsync("http://localhost:8087/api/Purchase/GetPurchaselastId");
                purchlastid.Wait();

                var result = responseTask.Result;
                var result1 = responseTask1.Result;
                var result2 = responseTask2.Result;
                var lstid = purchlastid.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<Tax_Group>>();
                    readTask.Wait();

                    txgrp = readTask.Result;
                    ViewBag.txgrp = txgrp;
                }
                else //web api sent error response 
                {
                    txgrp = Enumerable.Empty<Tax_Group>();
                    ViewBag.txgrp = txgrp;
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
                if (result1.IsSuccessStatusCode)
                {
                    var readTask1 = result1.Content.ReadAsAsync<IList<Dealer>>();
                    readTask1.Wait();
                    dlr = readTask1.Result;
                    ViewBag.dlr = dlr;
                }
                else //web api sent error response 
                {
                    dlr = Enumerable.Empty<Dealer>();
                    ViewBag.dlr = dlr;
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
                if (result2.IsSuccessStatusCode)
                {
                    var readTask2 = result2.Content.ReadAsAsync<IList<Product>>();
                    readTask2.Wait();
                    prdct = readTask2.Result;
                    ViewBag.prdct = prdct;
                }
                else //web api sent error response 
                {
                    prdct = Enumerable.Empty<Product>();
                    ViewBag.prdct = prdct;
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }

                if (lstid.IsSuccessStatusCode)
                {
                    var readTask3 = lstid.Content.ReadAsAsync<Purchase_Master>();
                    readTask3.Wait();
                    var lst = readTask3.Result;                  
                    var text = lst.Purchase_Number;
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
        
        [HttpPost]
        public JsonResult save(Purchase_Master purchase)
        {
           
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri("http://localhost:8087/api/Purchase");

                //HTTP POST
                var postTask = client.PostAsJsonAsync("http://localhost:8087/api/Purchase/PostNewMerchant", purchase);
                postTask.Wait();

                var result = postTask.Result;
               
                if (result.IsSuccessStatusCode)
                {
                    return new JsonResult { Data = new { status = true } };
                }
            }
            
            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

          
            return new JsonResult { Data = new { status = false } };
        }
      
    }
}