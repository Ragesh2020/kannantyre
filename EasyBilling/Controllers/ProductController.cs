using EasyBilling.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EasyBilling.Controllers
{
    [Authorize]
    public class ProductController : MybaseController
    {
        // GET: Product
        public ActionResult Index()
        {
            return View();
        }
       
        public FileResult Companyexport()
        {
            EasyBillingEntities db = new EasyBillingEntities();
            List<Product> data = db.Products.ToList();
            string spath = System.Web.HttpContext.Current.Server.MapPath("~/Export");
            if (data.Count > 0)
            {
                DataTable dataTable = new DataTable();
                PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(Product));

                for (int i = 0; i < props.Count; i++)
                {
                    PropertyDescriptor prop = props[i];
                   // dataTable.Columns.Add(prop.Name, prop.PropertyType);
                    dataTable.Columns.Add(prop.Name, Nullable.GetUnderlyingType(
          prop.PropertyType) ?? prop.PropertyType);
                }
                object[] values = new object[props.Count];
                foreach (Product item in data)
                {
                    for (int i = 0; i < values.Length; i++)
                    {
                        values[i] = props[i].GetValue(item);
                    }
                    dataTable.Rows.Add(values);
                }
                var lines = new List<string>();

                string[] columnNames = dataTable.Columns.Cast<DataColumn>().
                                                  Select(column => column.ColumnName).
                                                  ToArray();

                var header = string.Join(",", columnNames);
                lines.Add(header);

                var valueLines = dataTable.AsEnumerable()
                                   .Select(row => string.Join(",", row.ItemArray));
                lines.AddRange(valueLines);

                System.IO.File.WriteAllLines(spath + "/" + "Company export data.csv", lines);


                //   Process.Start(spath + "/" + "Export data.csv");
            }
            byte[] fileBytes = System.IO.File.ReadAllBytes(spath + "/" + "Company export data.csv");
            string fileName = "Company export data.csv";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);


        }
        public FileResult Productexport()
        {
            EasyBillingEntities db = new EasyBillingEntities();
            List<Other_Product> data = db.Other_Products.ToList();
            string spath = System.Web.HttpContext.Current.Server.MapPath("~/Export");
            if (data.Count > 0)
            {
                DataTable dataTable = new DataTable();
                PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(Other_Product));

                for (int i = 0; i < props.Count; i++)
                {
                    PropertyDescriptor prop = props[i];
                   // dataTable.Columns.Add(prop.Name, prop.PropertyType);
                    dataTable.Columns.Add(prop.Name, Nullable.GetUnderlyingType(
          prop.PropertyType) ?? prop.PropertyType);
                }
                object[] values = new object[props.Count];
                foreach (Other_Product item in data)
                {
                    for (int i = 0; i < values.Length; i++)
                    {
                        values[i] = props[i].GetValue(item);
                    }
                    dataTable.Rows.Add(values);
                }
                var lines = new List<string>();

                string[] columnNames = dataTable.Columns.Cast<DataColumn>().
                                                  Select(column => column.ColumnName).
                                                  ToArray();

                var header = string.Join(",", columnNames);
                lines.Add(header);

                var valueLines = dataTable.AsEnumerable()
                                   .Select(row => string.Join(",", row.ItemArray));
                lines.AddRange(valueLines);

                System.IO.File.WriteAllLines(spath + "/" + "Product export data.csv", lines);


                //   Process.Start(spath + "/" + "Export data.csv");
            }
            byte[] fileBytes = System.IO.File.ReadAllBytes(spath + "/" + "Product export data.csv");
            string fileName = "Product export data.csv";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);


        }
        public ActionResult Create()
        {
            IEnumerable<Tax_Group> txgrp = null;
            using (var client = new HttpClient())
            {
                var responseTask = client.GetAsync("http://localhost:8087//api/TaxGroup/GetAllUserTaxGroups");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<Tax_Group>>();
                    readTask.Wait();

                    txgrp = readTask.Result;
                    ViewBag.states = txgrp;
                }
                else //web api sent error response 
                {


                    txgrp = Enumerable.Empty<Tax_Group>();
                    ViewBag.states = txgrp;
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            using (EasyBillingEntities db = new EasyBillingEntities())
            {
                var _data = db.Products.ToList();
                // string json = JsonConvert.SerializeObject(_data.ToArray());
                string json = JsonConvert.SerializeObject(_data.ToArray(), Formatting.Indented,
    new JsonSerializerSettings()
    {
        ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
    }
    );
                json = "{\"data\":" + json + "}";
                //write string to file
                var par = Server.MapPath("~/Json/Company.json");
                System.IO.File.WriteAllText(par, json);
            }
            return View();
        }
        [HttpPost]
        public async Task<JsonResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {

                using (EasyBillingEntities db = new EasyBillingEntities())
            {
                    if (!string.IsNullOrEmpty(product.Product_name))
                    {
                        bool chk = db.Products.Where(z => z.Product_name.ToLower().Equals(product.Product_name.ToLower())).Any();
                        if (chk != true)
                        {

                            product.Token_Number = Guid.NewGuid().ToString();

                            db.Products.Add(product);
                            await db.SaveChangesAsync();
                            var _data = db.Products.ToList();
                            // string json = JsonConvert.SerializeObject(_data.ToArray());
                            string json = JsonConvert.SerializeObject(_data.ToArray(), Formatting.Indented,
        new JsonSerializerSettings()
        {
            ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
        }
        );
                            json = "{\"data\":" + json + "}";
                            //write string to file
                            var par = Server.MapPath("~/Json/Company.json");
                            System.IO.File.WriteAllText(par, json);
                            return Json(product);

                        }
                        else
                        {
                            return Json(product.Product_name + " already exists. Please try with another.");
                        }
                    }else
                    {
                        return Json("Company name cannot be blank.");
                    }
            }
            }
            
                return Json("");
           
        }

        public ActionResult Edit(string id)
        {
            Product product = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:8087//api/GetMerchant");
                //HTTP GET
                var responseTask = client.GetAsync("http://localhost:8087//api/Product/GetAllProducts?id=" + id.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Product>();
                    readTask.Wait();

                    product = readTask.Result;
                }
            }
            //IList<Tax_Group> taxexist = null;

            //using (var client = new HttpClient())
            //{
            //    client.BaseAddress = new Uri("http://localhost:8087//api/GetMerchant");
                //HTTP GET
                //var responseTask = client.GetAsync("http://localhost:8087//api/TaxGroup/GetAllTaxGroupforedit?id=" + (product.GL_CODE).ToString());
                //responseTask.Wait();

                //var result = responseTask.Result;
                //if (result.IsSuccessStatusCode)
                //{
                //    var readTask = result.Content.ReadAsAsync<IList<Tax_Group>>();
                //    readTask.Wait();

                //    taxexist = readTask.Result;


                //    ViewBag.states = taxexist;

                //}
            //}
            return View(product);
        }
        [HttpPost]
        public ActionResult Edit(Product product)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:8087//api/product");

                //HTTP POST
                var putTask = client.PutAsJsonAsync<Product>("http://localhost:8087//api/Product/PutProducts", product);
                putTask.Wait();


                var result = putTask.Result;
                if (result.IsSuccessStatusCode)
                {

                    return RedirectToAction("Index");
                }
            }
            return View(product);
        }
        public ActionResult Delete(string id)
        {
            Product product = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:8087//api/GetMerchant");
                //HTTP GET
                var responseTask = client.GetAsync("http://localhost:8087//api/Product/GetAllProducts?id=" + id.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Product>();
                    readTask.Wait();

                    product = readTask.Result;
                }
            }

            return View(product);
        }
        [HttpPost]
        public ActionResult Delete(string id, Product product)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:8087//api/");

                //HTTP DELETE
                var deleteTask = client.DeleteAsync("http://localhost:8087//api/Product/Delete?id=" + id.ToString());
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
            Product product = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:8087//api/GetMerchant");
                //HTTP GET
                var responseTask = client.GetAsync("http://localhost:8087//api/Product/GetAllProducts?id=" + id.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Product>();
                    readTask.Wait();

                    product = readTask.Result;
                }
            }

            return View(product);
        }
        [Authorize]
        public ActionResult ProductList()
        {
                return View();
          
        }
        [Authorize]
        public ActionResult CreateProduct()
        {
            return View();
        }
        [HttpPost]
        public async Task<JsonResult> CreateProduct(Other_Product product)
        {
            using (EasyBillingEntities db = new EasyBillingEntities())
            {
                if (ModelState.IsValid)
                {
                    if (!string.IsNullOrEmpty(product.Product_name) || !string.IsNullOrEmpty(product.Product_type))
                    {
                        bool chk = db.Other_Products.Where(z => z.Product_name.ToLower() == product.Product_name.ToLower() && z.Product_type.ToLower() == product.Product_type.ToLower()).Any();
                        if (chk == true)
                        {
                            return Json(product.Product_name + " along with " + product.Product_type + " is alreday exist. Please try with another");
                        }
                        else
                        {
                            product.Token_number = Guid.NewGuid().ToString();
                            db.Other_Products.Add(product);
                            await db.SaveChangesAsync();
                            var _data = db.Other_Products.ToList();
                            string json = JsonConvert.SerializeObject(_data.ToArray());

                            json = "{\"data\":" + json + "}";
                            //write string to file
                            var par = Server.MapPath("~/Json/products.json");
                            System.IO.File.WriteAllText(par, json);
                            return Json(product);
                        }
                    }else
                    {
                        return Json("Name nad type cannot be blank.");
                    }
                }else
                    return Json("Something went wrong. Please check your entry and try again");
            }
        }

    }

}