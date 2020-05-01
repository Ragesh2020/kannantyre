using EasyBilling.Models;
using Newtonsoft.Json;
using PagedList;
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
    public class DealerController : MybaseController
    {
        
        // GET: Dealer
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetSize(string term)
        {
           EasyBillingEntities db = new EasyBillingEntities();
        List<string> size = (from sz in db.Tyre_sizes

                                 where sz.Tyre_size1.Contains(term)
                                 select sz.Tyre_size1).Distinct().ToList();
            if (size.Count == 0)
            {
                size.Add("No size is matched with this entry...");
            }
            return Json(size, JsonRequestBehavior.AllowGet);
        }
        public FileResult Suppliereexport()
        {
            EasyBillingEntities db = new EasyBillingEntities();
            List<Dealer> data = db.Dealers.ToList();
            string spath = System.Web.HttpContext.Current.Server.MapPath("~/Export");
            if (data.Count > 0)
            {
                DataTable dataTable = new DataTable();
                PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(Dealer));

                for (int i = 0; i < props.Count; i++)
                {
                    PropertyDescriptor prop = props[i];
                    //dataTable.Columns.Add(prop.Name, prop.PropertyType);
                    dataTable.Columns.Add(prop.Name, Nullable.GetUnderlyingType(
          prop.PropertyType) ?? prop.PropertyType);
                }
                object[] values = new object[props.Count];
                foreach (Dealer item in data)
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

                System.IO.File.WriteAllLines(spath + "/" + "Supplier export data.csv", lines);


                //   Process.Start(spath + "/" + "Export data.csv");
            }
            byte[] fileBytes = System.IO.File.ReadAllBytes(spath + "/" + "Supplier export data.csv");
            string fileName = "Supplier export data.csv";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);


        }
        public JsonResult GetDealers(string term)
        {
            using (EasyBillingEntities db = new EasyBillingEntities())
            {
                var dcmlempty = decimal.Parse("0.000");
                List<string> _Products_For_Sales = (from pdfs in db.Dealers
                                               
                                                    where pdfs.Dealer_code.Contains(term)
                                                    select pdfs.Dealer_code).Distinct().ToList();
                if (_Products_For_Sales.Count == 0)
                {
                    _Products_For_Sales.Add("No Supplier is matched with this Code...");
                }
                return Json(_Products_For_Sales, JsonRequestBehavior.AllowGet);
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
        public async Task<JsonResult> Create(Dealer dealer)
        {
            using (EasyBillingEntities db = new EasyBillingEntities())
            {
                if (!string.IsNullOrEmpty(dealer.Name) || !string.IsNullOrEmpty(dealer.Phone_number))
                {
                    bool chk = db.Dealers.Where(z => z.Name.ToLower() == dealer.Name.ToLower()).Any();
                    bool chkphn = db.Dealers.Where(z => z.Phone_number.ToLower().Equals(dealer.Phone_number.ToLower())).Any();
                    if (chk != true && chkphn != true)
                    {
                        if (ModelState.IsValid)
                        {
                            dealer.Token_number = Guid.NewGuid().ToString();
                            dealer.Isactive = true;
                            db.Dealers.Add(dealer);
                            await db.SaveChangesAsync();
                            var _data = db.Dealers.ToList();
                            string json = JsonConvert.SerializeObject(_data.ToArray());

                            json = "{\"data\":" + json + "}";
                            //write string to file
                            var par = Server.MapPath("~/Json/Supplier.json");
                            System.IO.File.WriteAllText(par, json);
                            return Json(dealer);
                        }
                        else
                        {
                            return Json("Please check your entry and try again. Name and phone number is mandatory.");
                        }
                    }
                    else if (chk == true && chkphn == true)
                    {
                        return Json(dealer.Name + " and " + dealer.Phone_number + " already exists. Please try with another.");
                    }
                    else if (chk == true && chkphn == false)
                    {
                        return Json(dealer.Name + " already exists. Please try with another.");
                    }
                    else if (chk == false && chkphn == true)
                    {
                        return Json(dealer.Phone_number + " already exists. Please try with another.");
                    }
                    else
                    {
                        return Json("Please check if your data exist already. Please try with another.");
                    }
                }else
                {
                    return Json("Name and phone number cannot be blank.");
                }
            }
              
        }

        public ActionResult Edit(string id)
        {
            Dealer dealer = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:8087//api/GetMerchant");
                //HTTP GET
                var responseTask = client.GetAsync("http://localhost:8087//api/Dealer/GetAllDealers?id=" + id.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Dealer>();
                    readTask.Wait();

                    dealer = readTask.Result;
                }
            }
            IList<State> stateexist = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:8087//api/GetMerchant");
                //HTTP GET
                var responseTask = client.GetAsync("http://localhost:8087//api/State/GetAllStatesforedit?id=" + (dealer.State).ToString());
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
            return View(dealer);
        }
        [HttpPost]
        public ActionResult Edit(Dealer dealer)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:8087//api/Dealer");

                //HTTP POST
                var putTask = client.PutAsJsonAsync<Dealer>("http://localhost:8087//api/Dealer/PutDealers", dealer);
                putTask.Wait();


                var result = putTask.Result;
                if (result.IsSuccessStatusCode)
                {

                    return RedirectToAction("Index");
                }
            }
            return View(dealer);
        }
        public ActionResult Delete(string id)
        {
            Dealer dealer = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:8087//api/GetMerchant");
                //HTTP GET
                var responseTask = client.GetAsync("http://localhost:8087//api/Dealer/GetAllDealers?id=" + id.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Dealer>();
                    readTask.Wait();

                    dealer = readTask.Result;
                }
            }

            return View(dealer);
        }
        [HttpPost]
        public ActionResult Delete(string id, Dealer Dealer)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:8087//api/");

                //HTTP DELETE
                var deleteTask = client.DeleteAsync("http://localhost:8087//api/Dealer/Delete?id=" + id.ToString());
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
            Dealer dealer = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:8087//api/GetMerchant");
                //HTTP GET
                var responseTask = client.GetAsync("http://localhost:8087//api/Dealer/GetAllDealers?id=" + id.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Dealer>();
                    readTask.Wait();

                    dealer = readTask.Result;
                }
            }

            return View(dealer);
        }
    }
}