using EasyBilling.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace EasyBilling.Controllers
{
    [Authorize]
    public class CustomerController : MybaseController
    {
        // GET: Customer

        public ActionResult Index(int? page, string keyword)
        {
            IEnumerable<Customer> customer = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:8087//api/Customer/GetAllCustomers");
                //HTTP GET
                var responseTask = client.GetAsync("http://localhost:8087//api/Customer/GetAllCustomers");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<Customer>>();
                    readTask.Wait();

                    customer = readTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..
                   
                    customer = Enumerable.Empty<Customer>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
                if (!string.IsNullOrEmpty(keyword))
                {
                    return View(customer.Where(f => f.Token_number.Contains(keyword)).Distinct().ToList().ToPagedList(page ?? 1, 4));
                }
            }
            return View(customer.ToPagedList(page ?? 1, 4));
        }
        public PartialViewResult Filter(string key, int? page)
        {
            var dcmlempty = decimal.Parse("0.000");
            List<Customer> customer = new List<Customer>();
            if (string.IsNullOrEmpty(key) || key.ToLower()=="all")
            {
               
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:8087//api/Customer/GetAllCustomers");
                    //HTTP GET
                    var responseTask = client.GetAsync("http://localhost:8087//api/Customer/GetAllCustomers");
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<List<Customer>>();
                        readTask.Wait();

                        customer = readTask.Result;
                    }

                }

                return PartialView("_CustomerPartial", customer.ToPagedList(page ?? 1, 4));
            }
            else
            {
              

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:8087//api/Customer/GetFilterCustomers");
                    //HTTP GET
                    var responseTask = client.GetAsync("http://localhost:8087//api/Customer/GetFilterCustomers?key=" + key);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<List<Customer>>();
                        readTask.Wait();

                        customer = readTask.Result;
                    }

                }

                return PartialView("_CustomerPartial", customer.ToPagedList(page ?? 1, 4));

            }

        }
        //public JsonResult GetInstitutes(string term)
        //{
        //    using (EasyBillingEntities db = new EasyBillingEntities())
        //    {
        //        var dcmlempty = decimal.Parse("0.000");
        //        List<string> _Products_For_Sales = (from cst in db.Customers
        //                                            where cst.IsActive == true
        //                                            && cst.Token_number.Contains(term)
        //                                            select cst.Token_number).Distinct().ToList();
        //        if(_Products_For_Sales.Count==0)
        //            {
        //            _Products_For_Sales.Add("No customer exists with this key...");
        //        }
        //        return Json(_Products_For_Sales, JsonRequestBehavior.AllowGet);
        //    }
        //}

        public JsonResult Getcustdetails(string id)
        {
            using (EasyBillingEntities db = new EasyBillingEntities())
            {
                var customer = db.Customers.Select(x => new
                {
                    x.Customer_Name,
                    x.Email,
                    x.Token_number,
                   // x.Pan_number,
                    x.Address,x.Phone_number,
                    //x.State_Name

                }).Where(z => z.Token_number == id).Distinct().FirstOrDefault();
                return Json(customer, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Create()
        {
            IEnumerable<State> state = null;
            using (var client = new HttpClient())
            {
                var responseTask = client.GetAsync("http://localhost:8087//api/State/GetAllStates");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<State>>();
                    readTask.Wait();

                    state = readTask.Result;
                    ViewBag.states = state;
                }
                else //web api sent error response 
                {


                    state = Enumerable.Empty<State>();
                    ViewBag.states = state;
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            return View();
        }
        [HttpPost]
        public ActionResult create(Customer customer)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:8087//api/Customer");

                //HTTP POST
                var postTask = client.PostAsJsonAsync("http://localhost:8087//api/Customer/PostNewCustomer", customer);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }else
                {
                    IEnumerable<State> state = null;
                    
                        var responseTask = client.GetAsync("http://localhost:8087//api/State/GetAllStates");
                        responseTask.Wait();

                        var result1 = responseTask.Result;
                        if (result1.IsSuccessStatusCode)
                        {
                            var readTask = result1.Content.ReadAsAsync<IList<State>>();
                            readTask.Wait();

                            state = readTask.Result;
                            ViewBag.states = state;
                        }
                        else //web api sent error response 
                        {


                            state = Enumerable.Empty<State>();
                            ViewBag.states = state;
                            ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                        }
                        if (result.StatusCode==System.Net.HttpStatusCode.Found)
                    {
                        ModelState.AddModelError(string.Empty, "Email already exists.");
                    }else
                    {
                        ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
                    }
                }
            }

            

            return View(customer);
        }
        public ActionResult Edit(string id)
        {
            Customer customer = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:8087//api/GetMerchant");
                //HTTP GET
                var responseTask = client.GetAsync("http://localhost:8087//api/Customer/GetAllCustomers?id=" + id.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Customer>();
                    readTask.Wait();

                    customer = readTask.Result;
                }
            }
           // IList<State> stateexist = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:8087//api/GetMerchant");
                //HTTP GET
                //var responseTask = client.GetAsync("http://localhost:8087//api/State/GetAllStatesforedit?id=" + (customer.State).ToString());
                //responseTask.Wait();

                //var result = responseTask.Result;
                //if (result.IsSuccessStatusCode)
                //{
                //    var readTask = result.Content.ReadAsAsync<IList<State>>();
                //    readTask.Wait();

                //    stateexist = readTask.Result;


                //    ViewBag.states = stateexist;

                //}
            }
            return View(customer);
        }
        [HttpPost]
        public ActionResult Edit(Customer customer)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:8087//api/Dealer");

                //HTTP POST
                var putTask = client.PutAsJsonAsync<Customer>("http://localhost:8087//api/Customer/PutCustomers", customer);
                putTask.Wait();


                var result = putTask.Result;
                if (result.IsSuccessStatusCode)
                {

                    return RedirectToAction("Index");
                }
            }

            return View(customer);
        }
        public ActionResult Delete(string id)
        {
            Customer customer = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:8087//api/GetMerchant");
                //HTTP GET
                var responseTask = client.GetAsync("http://localhost:8087//api/Customer/GetAllCustomers?id=" + id.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Customer>();
                    readTask.Wait();

                    customer = readTask.Result;
                }
            }

            return View(customer);
        }
        [HttpPost]
        public ActionResult Delete(string id, Customer customer)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:8087//api/");

                //HTTP DELETE
                var deleteTask = client.DeleteAsync("http://localhost:8087//api/Customer/Delete?id=" + id.ToString());
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
            Customer customer = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:8087//api/GetMerchant");
                //HTTP GET
                var responseTask = client.GetAsync("http://localhost:8087//api/Customer/GetAllCustomers?id=" + id.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Customer>();
                    readTask.Wait();

                    customer = readTask.Result;
                }
            }

            return View(customer);
        }
    }
}