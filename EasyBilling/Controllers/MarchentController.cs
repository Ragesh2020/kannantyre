using EasyBilling.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Mvc;

namespace EasyBilling.Controllers
{
    public class MarchentController : MybaseController
    {
        // GET: Marchent
        public ActionResult Index()
        {
            IEnumerable<Marchent_Account> marchent = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:8087//api/");
                //HTTP GET
                var responseTask = client.GetAsync("http://localhost:8087//api/Marchent/GetAllMarchents");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<Marchent_Account>>();
                    readTask.Wait();

                    marchent = readTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    marchent = Enumerable.Empty<Marchent_Account>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            return View(marchent);
        }
        //public ActionResult Create()
        //{
        //    IEnumerable<State> state = null;
        //    using (var client = new HttpClient())
        //    {
        //        var responseTask = client.GetAsync("http://localhost:8087//api/State/GetAllStates");
        //        responseTask.Wait();

        //        var result = responseTask.Result;
        //        if (result.IsSuccessStatusCode)
        //        {
        //            var readTask = result.Content.ReadAsAsync<IList<State>>();
        //            readTask.Wait();

        //            state = readTask.Result;
        //            ViewBag.states = state;
        //        }
        //        else 
        //        {


        //            state = Enumerable.Empty<State>();
        //            ViewBag.states = state;
        //            ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
        //        }
        //    }
        //    return View();
        //}
        //[HttpPost]
        //public ActionResult create(Marchent_Account marchent)
        //{
        //    using (var client = new HttpClient())
        //    {
                
        //        client.BaseAddress = new Uri("http://localhost:8087//api/Marchent");
               
        //        //HTTP POST
        //        var postTask = client.PostAsJsonAsync("http://localhost:8087//api/Marchent/PostNewMerchant", marchent);
        //        postTask.Wait();

        //        var result = postTask.Result;
        //        if (result.IsSuccessStatusCode)
        //        {
        //            return RedirectToAction("Index");
        //        }
        //    }

        //    ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

        //    return View(marchent);
        //}

        public ActionResult Edit(string id)
        {
            Marchent_Account merchant = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:8087//api/GetMerchant");
                //HTTP GET
                var responseTask = client.GetAsync("http://localhost:8087//api/Marchent/GetAllMarchents?id=" + id.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Marchent_Account>();
                    readTask.Wait();

                    merchant = readTask.Result;
                }
            }
            
            IList<State> stateexist = null;
           
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:8087//api/GetMerchant");
                //HTTP GET
                var responseTask = client.GetAsync("http://localhost:8087//api/State/GetAllStatesforedit?id=" + (merchant.State_Code).ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<State>>();
                    readTask.Wait();
                    
                    stateexist = readTask.Result;
                   
              
                    ViewBag.states = stateexist;

                }
            }
                return View(merchant);
        }
        [HttpPost]
        public ActionResult Edit(Marchent_Account marchent)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:8087//api/Marchent");

                //HTTP POST
                var putTask = client.PutAsJsonAsync<Marchent_Account>("http://localhost:8087//api/Marchent/PutMerchant", marchent);
                putTask.Wait();


                var result = putTask.Result;
                if (result.IsSuccessStatusCode)
                {

                    return RedirectToAction("Index");
                }
            }
            return View(marchent);
        }
        public ActionResult Delete(string id)
        {
            Marchent_Account merchant = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:8087//api/GetMerchant");
                //HTTP GET
                var responseTask = client.GetAsync("http://localhost:8087//api/Marchent/GetAllMarchents?id=" + id.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Marchent_Account>();
                    readTask.Wait();

                    merchant = readTask.Result;
                }
            }

            return View(merchant);
        }
        [HttpPost]
        public ActionResult Delete(string id,Marchent_Account marchent)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:8087//api/");

                //HTTP DELETE
                var deleteTask = client.DeleteAsync("http://localhost:8087//api/Marchent/Delete?id=" + id.ToString());
                deleteTask.Wait();

                var result = deleteTask.Result;
                if (result.IsSuccessStatusCode)
                {

                    return RedirectToAction("Index");
                }
            }

            return RedirectToAction("Index");
        }
        public ActionResult Details(string id)
        {
            Marchent_Account merchant = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:8087//api/GetMerchant");
                //HTTP GET
                var responseTask = client.GetAsync("http://localhost:8087//api/Marchent/GetAllMarchents?id=" + id.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Marchent_Account>();
                    readTask.Wait();

                    merchant = readTask.Result;
                }
            }

            return View(merchant);
        }
        //public ActionResult abc()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public ActionResult abc(bool includeAddress = false)
        //{
        //    using (var client = new HttpClient())
        //    {
        //        using (var ctx = new EasyBillingEntities())
        //        {
        //            IList<MarchentAccount> marchent = null;
        //            marchent = ctx.Marchent_Accounts.Include("State").Select(s => new MarchentAccount()
        //            {
        //                Marchent_name = s.Marchent_name,
        //                Email_Id = s.Email_Id,
        //                Mobile = s.Mobile,
        //                Telephone_No = s.Telephone_No,
        //                Address = s.Address,
        //                GSTIN_Number = s.GSTIN_Number,
        //                Pan_Number = s.Pan_Number,
        //                IsActive = s.IsActive,
        //                State_Code = s.State_Code,
        //                State = s.State == null || includeAddress == false ? null : new StateData()
        //                {
        //                    State_Code = s.State_Code.Value,

        //                }
        //            }).ToList<MarchentAccount>();




        //            //client.BaseAddress = new Uri("http://localhost:8087//api/Marchent");

        //            //HTTP POST
        //            var postTask = client.PostAsJsonAsync("http://localhost:8087//api/Marchent/PostNewStudent", marchent);
        //            postTask.Wait();

        //            var result = postTask.Result;
        //        }
        //    }

        //    ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
        //    return View();
        //}


    }

}