using EasyBilling.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace EasyBilling.Controllers
{
    [Authorize]
    public class StateController : MybaseController
    {
        // GET: State
        public ActionResult Index()
        {
            IEnumerable<State> state = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:8087/api/");
                //HTTP GET
                var responseTask = client.GetAsync("http://localhost:8087/api/State/GetAllStates");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<State>>();
                    readTask.Wait();

                    state = readTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    state = Enumerable.Empty<State>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            return View(state);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult create(State state)
        {
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri("http://localhost:8087/api/State");

                //HTTP POST
                var postTask = client.PostAsJsonAsync("http://localhost:8087/api/State/PostNewState", state);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }

            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

            return View(state);
        }
        public ActionResult Edit(string id)
        {
            State state = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:8087/api/GetMerchant");
                //HTTP GET
                var responseTask = client.GetAsync("http://localhost:8087/api/State/GetAllStates?id=" + id.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<State>();
                    readTask.Wait();

                    state = readTask.Result;
                }
            }

            return View(state);
        }
        [HttpPost]
        public ActionResult Edit(State state)
        {
           
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:8087/api/State/PutTaxGroup");

                //HTTP POST
                var putTask = client.PutAsJsonAsync("http://localhost:8087/api/State/PutTaxGroup", state);
                putTask.Wait();


                var result = putTask.Result;
                
                if (result.IsSuccessStatusCode)
                {

                    return RedirectToAction("Index");
                }
                if (!ModelState.IsValid)
                    ModelState.AddModelError(string.Empty, "model not valoid");
                ModelState.AddModelError(string.Empty,result.StatusCode.ToString());
            }
           // ModelState.AddModelError(string.Empty, "Please check your data and try again...");
           
            return View(state);
        }
        public ActionResult Delete(string id)
        {
            State state = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:8087/api/GetMerchant");
                //HTTP GET
                var responseTask = client.GetAsync("http://localhost:8087/api/State/GetAllStates?id=" + id.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<State>();
                    readTask.Wait();

                    state = readTask.Result;
                }
            }

            return View(state);
        }
        [HttpPost]
        public ActionResult Delete(string id, State state)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:8087/api/");

                //HTTP DELETE
                var deleteTask = client.DeleteAsync("http://localhost:8087/api/State/Delete?id=" + id.ToString());
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
            State state = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:8087/api/GetMerchant");
                //HTTP GET
                var responseTask = client.GetAsync("http://localhost:8087/api/State/GetAllStates?id=" + id.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<State>();
                    readTask.Wait();

                    state = readTask.Result;
                }
            }

            return View(state);
        }

    }

}