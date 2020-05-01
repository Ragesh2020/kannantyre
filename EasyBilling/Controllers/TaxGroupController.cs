using EasyBilling.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace EasyBilling.Controllers
{
    public class TaxGroupController : MybaseController
    {
        // GET: TaxGroup
        public ActionResult Index()
        {
            IEnumerable<Tax_Group> taxgroup = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:8087/api/");
                //HTTP GET
                var responseTask = client.GetAsync("http://localhost:8087/api/TaxGroup/GetAllTaxGroups");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<Tax_Group>>();
                    readTask.Wait();

                    taxgroup = readTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    taxgroup = Enumerable.Empty<Tax_Group>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            return View(taxgroup);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult create(Tax_Group taxgroup)
        {
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri("http://localhost:8087/api/TaxGroup");

                //HTTP POST
                var postTask = client.PostAsJsonAsync("http://localhost:8087/api/TaxGroup/PostNewTaxGroup", taxgroup);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }

            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

            return View(taxgroup);
        }
        public ActionResult Edit(string id)
        {
            Tax_Group taxgroup = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:8087/api/GetMerchant");
                //HTTP GET
                var responseTask = client.GetAsync("http://localhost:8087/api/TaxGroup/GetAllTaxGroup?id=" + id.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Tax_Group>();
                    readTask.Wait();

                    taxgroup = readTask.Result;
                }
            }

            return View(taxgroup);
        }
        [HttpPost]
        public ActionResult Edit(Tax_Group taxgroup)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:8087/api/product");

                //HTTP POST
                var putTask = client.PutAsJsonAsync<Tax_Group>("http://localhost:8087/api/TaxGroup/PutTaxGroup", taxgroup);
                putTask.Wait();


                var result = putTask.Result;
                if (result.IsSuccessStatusCode)
                {

                    return RedirectToAction("Index");
                }
            }
            return View(taxgroup);
        }
        public ActionResult Delete(string id)
        {
            Tax_Group taxgroup = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:8087/api/GetMerchant");
                //HTTP GET
                var responseTask = client.GetAsync("http://localhost:8087/api/TaxGroup/GetAllTaxGroup?id=" + id.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Tax_Group>();
                    readTask.Wait();

                    taxgroup = readTask.Result;
                }
            }

            return View(taxgroup);
        }
        [HttpPost]
        public ActionResult Delete(string id, Tax_Group taxgroup)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:8087/api/");

                //HTTP DELETE
                var deleteTask = client.DeleteAsync("http://localhost:8087/api/TaxGroup/Delete?id=" + id.ToString());
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
            Tax_Group taxgroup = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:8087/api/GetMerchant");
                //HTTP GET
                var responseTask = client.GetAsync("http://localhost:8087/api/TaxGroup/GetAllTaxGroup?id=" + id.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Tax_Group>();
                    readTask.Wait();

                    taxgroup = readTask.Result;
                }
            }

            return View(taxgroup);
        }

    }

}