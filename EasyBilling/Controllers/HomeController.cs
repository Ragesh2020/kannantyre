using EasyBilling.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.Reporting.WebForms;
using System.IO;

namespace EasyBilling.Controllers
{
   
    public class HomeController : MybaseController
    {
        private EasyBillingEntities db = new EasyBillingEntities();
        //[Authorize]
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
           

            //string FileName = "File_" + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
            //string extension;
            //string encoding;
            //string mimeType;
            //string[] streams;
            //Warning[] warnings;

            //LocalReport report = new LocalReport();
            //report.ReportPath = @"D:\mychecking\EasyBilling\EasyBilling\Report1.rdlc";
           
            //Byte[] mybytes = report.Render("PDF", null,
            //                out extension, out encoding,
            //                out mimeType, out streams, out warnings); //for exporting to PDF  

            //using (FileStream fs = System.IO.File.Create(@"C:\Users\sourav.ganguly\source\repos\TestforRDLC\TestforRDLC\download\" + FileName))
            //{
            //    fs.Write(mybytes, 0, mybytes.Length);
            //}

            //Response.ClearHeaders();
            //Response.ClearContent();
            //Response.Buffer = true;
            //Response.Clear();
            //Response.ContentType = "application/pdf";
            //Response.AddHeader("Content-Disposition", "attachment; filename=" + FileName);
            //Response.WriteFile(@"C:\Users\sourav.ganguly\source\repos\TestforRDLC\TestforRDLC\download\" + FileName);
            //Response.Flush();
            //Response.Close();
            //Response.End();
            return View();
        }
        [HttpGet]
        public JsonResult AjaxGetCall()
        {
            Temp_Stock temp_Stock = new Temp_Stock();
            temp_Stock.Total = db.Temp_Stock.Select(z => z.Total).Sum();
            temp_Stock.Pieces = db.Temp_Stock.Select(z => z.Pieces).Sum();
            temp_Stock.ttlitms = db.Temp_Stock.Select(z => z.Item_tyre_Id).Count();
            return Json(temp_Stock, JsonRequestBehavior.AllowGet);
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
        public JsonResult GetItemTubeId(string term)
        {
            List<string> temp_Stocks = (from temp in db.Temp_Stock
                                        select temp.Item_tyre_Id).Distinct().ToList();
            IEnumerable<string> itemidchk = (from item in db.Item_Tubes

                                             where item.Item_Id.Contains(term)
                                             select item.Item_Id).Distinct().ToList();
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

                                             where item.Product_id.Contains(term)
                                             select item.Product_id).Distinct().ToList();
            List<string> itemid = itemidchk.Except(temp_Stocks).ToList();


            if (itemid.Count == 0)
            {
                itemid.Add("No id is matched with this entry...");
            }
            return Json(itemid, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCompName(string term)
        {
            List<string> compname = (from item in db.Item_Tyres

                                     where item.Company_name.Contains(term)
                                     select item.Company_name).Distinct().ToList();

            if (compname.Count == 0)
            {
                compname.Add("No company name is matched with this entry...");
            }
            return Json(compname, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCompNameTube(string term)
        {
            List<string> compname = (from item in db.Item_Tubes

                                     where item.Company_name.Contains(term)
                                     select item.Company_name).Distinct().ToList();

            if (compname.Count == 0)
            {
                compname.Add("No company name is matched with this entry...");
            }
            return Json(compname, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Getcheckingsuppliername(string id)
        {

            var itms = db.Dealers.Select(x => new
            {
                x.Name

            }).Where(z => z.Name == id).FirstOrDefault();
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
        public JsonResult Getcheckingtmp(string id)
        {

            var itms = db.Temp_Stock.Where(z => z.Item_tyre_Id == id).Distinct().FirstOrDefault();
            return Json(itms, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetProdctname(string term)
        {
            List<string> pronm = (from item in db.Other_Products

                                  where item.Product_name.Contains(term)
                                  select item.Product_name).Distinct().ToList();

            if (pronm.Count == 0)
            {
                pronm.Add("No product name is matched with this entry...");
            }
            return Json(pronm, JsonRequestBehavior.AllowGet);
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
        public FileResult export()
        {
            EasyBillingEntities db = new EasyBillingEntities();
            List<Item_Tyre> data = db.Item_Tyres.ToList();
           string spath = System.Web.HttpContext.Current.Server.MapPath("~/Export");
            if (data.Count > 0)
            {
                DataTable dataTable = new DataTable();
                PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(Item_Tyre));

                for (int i = 0; i < props.Count; i++)
                {
                    PropertyDescriptor prop = props[i];
                    dataTable.Columns.Add(prop.Name, prop.PropertyType);
                }
                object[] values = new object[props.Count];
                foreach (Item_Tyre item in data)
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
                 
                System.IO.File.WriteAllLines(spath+"/"+"Export data.csv", lines);

         
                //   Process.Start(spath + "/" + "Export data.csv");
            }
                byte[] fileBytes = System.IO.File.ReadAllBytes(spath + "/" + "Export data.csv");
                string fileName = "Export data.csv";
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
          
            
        }
        [Authorize]
        public ActionResult Dashboard()
        {
           
                ViewBag.Title = "Dashboard";

            return View();
        }
        public ActionResult test()
        {
            return View();
        }
      [Authorize]
        public ActionResult About()
        {
            return View();
        }
        [Authorize]
        public ActionResult Contact()
        {
            return View();

        }
        public ActionResult SignIn()
        {
            FormsAuthentication.SignOut();
            ViewBag.Title = "Sign In";

            return View();
        }
        [HttpPost]
        public ActionResult SignIn(Marchent_Account marchent, string ReturnUrl)
        {
            using (var ctx = new EasyBillingEntities())
            {
                if (marchent.User_Id != null && marchent.Password != null)
                {
                    bool chck = false;
                   
                   chck = ctx.Marchent_Accounts.Where(a => a.Email_Id == marchent.User_Id && a.Verification_code == marchent.Password).Any();
                    if (chck)
                    {
                        var mrchnt = new Marchent_Account();

                        mrchnt = ctx.Marchent_Accounts.Where(a => a.Email_Id == marchent.User_Id && a.Verification_code == marchent.Password).FirstOrDefault();
                    }
                    else
                    {
                        chck = ctx.Employees.Where(a => a.Employee_Id == marchent.User_Id && a.Password == marchent.Password).Any();
                        if (chck)
                        {
                            var mrchnt = new Employee();
                            mrchnt = ctx.Employees.Where(a => a.Employee_Id == marchent.User_Id && a.Password == marchent.Password).FirstOrDefault();
                        }
                    }
                    if (chck == true)
                    {
                        int timeout = marchent.rememberme ? 60 : 5;
                        var ticket = new FormsAuthenticationTicket(marchent.User_Id, marchent.rememberme, timeout);
                        string encrypted = FormsAuthentication.Encrypt(ticket);
                        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                        cookie.Expires = DateTime.Now.AddMinutes(timeout);
                        cookie.HttpOnly = true;

                        Response.Cookies.Add(cookie);
                        FormsAuthentication.SetAuthCookie(marchent.User_Id, marchent.rememberme);
                        var t = User.Identity.IsAuthenticated;

                        return RedirectToAction("Dashboard");

                    }
                    else
                    {
                        ViewBag.Title = "Sign In";
                        ModelState.AddModelError(string.Empty, "Please check credentials with your e-mail.");
                        return View(marchent);
                    }
                }
                else
                {
                    ViewBag.Title = "Sign In";
                    ModelState.AddModelError(string.Empty, "Please check credentials with your e-mail.");
                    return View(marchent);
                }
            }
        }

        //[HttpPost]
        //public ActionResult SignIn(Marchent_Account marchent, string ReturnUrl)
        //{
        //    using ( var client = new HttpClient())
        //    {
        //        if (marchent.Email_Id == "souravganguly707@gmail.com" || marchent.Email_Id== "kannantyres@kannantyres.com")
        //        {
        //            client.BaseAddress = new Uri("http://localhost:8087//api/Marchent");

        //            //HTTP POST
        //            var postTask = client.PostAsJsonAsync("http://localhost:8087//api/Marchent/PostMerchantLogin", marchent);
        //            postTask.Wait();

        //            var result = postTask.Result;
        //            if (result.IsSuccessStatusCode)
        //            {
        //                var readTask1 = result.Content.ReadAsAsync<Marchent_Account>();
        //                readTask1.Wait();

        //                var mrchnt = readTask1.Result;
        //                //============================================== for license blocking for Kannan tyres======================================================================================
        //                //NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
        //                //String sMacAddress = string.Empty;
        //                //foreach (NetworkInterface adapter in nics)
        //                //{
        //                //    if (sMacAddress != "28F10E3842DB")// only return MAC Address from first card  
        //                //    {
        //                //        IPInterfaceProperties properties = adapter.GetIPProperties();
        //                //        sMacAddress = adapter.GetPhysicalAddress().ToString();
        //                //    }
        //                //}

        //                //string HostName = Dns.GetHostName();

        //                //var license = "KADAMBARI-LC-" + sMacAddress + "-" + HostName;
        //                //if (license != mrchnt.License)
        //                //{

        //                //    ModelState.AddModelError(string.Empty, "You have no license to use. Please contact to develper. Thank you");
        //                //    return View(marchent);
        //                //}
        //                //else
        //                //{
        //                //============================================== // for license======================================================================================
        //                int timeout = mrchnt.rememberme ? 60 : 5;
        //                    var ticket = new FormsAuthenticationTicket(mrchnt.Email_Id, mrchnt.rememberme, timeout);
        //                    string encrypted = FormsAuthentication.Encrypt(ticket);
        //                    var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
        //                    cookie.Expires = DateTime.Now.AddMinutes(timeout);
        //                    cookie.HttpOnly = true;

        //                    Response.Cookies.Add(cookie);
        //                    FormsAuthentication.SetAuthCookie(mrchnt.Email_Id, mrchnt.rememberme);
        //                    var t = User.Identity.IsAuthenticated;
        //                    //if (Url.IsLocalUrl(ReturnUrl))
        //                    //{
        //                    //    return Redirect(ReturnUrl);
        //                    //}
        //                    //else
        //                    //{
        //                        return RedirectToAction("Dashboard");

        //                    //}

        //                //}
        //            }
        //            else
        //            {
        //                ViewBag.Title = "Sign In";
        //                ModelState.AddModelError(string.Empty, "Please check credentials with your e-mail.");
        //                return View(marchent);
        //            }
        //        }else
        //        {
        //            HttpResponseMessage result = null;

        //                client.BaseAddress = new Uri("http://localhost:8087//api/Customer");

        //                //HTTP POST
        //                var postTask = client.PostAsJsonAsync("http://localhost:8087//api/Customer/PostCustomerLogin", marchent);
        //                postTask.Wait();

        //                result = postTask.Result;

        //            if (result.IsSuccessStatusCode)
        //            {

        //                    int timeout = marchent.rememberme ? 60 : 5;
        //                    var ticket = new FormsAuthenticationTicket(marchent.Email_Id, marchent.rememberme, timeout);
        //                    string encrypted = FormsAuthentication.Encrypt(ticket);
        //                    var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
        //                    cookie.Expires = DateTime.Now.AddMinutes(timeout);
        //                    cookie.HttpOnly = true;

        //                    Response.Cookies.Add(cookie);
        //                    FormsAuthentication.SetAuthCookie(marchent.Email_Id, marchent.rememberme);
        //                    var t = User.Identity.IsAuthenticated;
        //                    if (Url.IsLocalUrl(ReturnUrl))
        //                    {
        //                        return Redirect(ReturnUrl);
        //                    }
        //                    else
        //                    {
        //                        return RedirectToAction("Index");

        //                    }

        //            }
        //            else
        //            {
        //                ViewBag.Title = "Sign In";
        //                ModelState.AddModelError(string.Empty, "Please check credentials with your e-mail.");
        //                return View(marchent);
        //            }
        //        }
        //    }
        //}

        public ActionResult SignUp(string id)
        {
            if(id=="1")
            {
                ViewBag.succeed = "1";
            }
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
        public ActionResult SignUp(Customer customer)
        {
            using (var client = new HttpClient())
            {
                //if (customer.New_customer == true)
                //{
                //    client.BaseAddress = new Uri("http://localhost:8087//api/Customer");

                //    //HTTP POST
                //    var postTask = client.PostAsJsonAsync("http://localhost:8087//api/Customer/PostNewCustomer", customer);
                //    postTask.Wait();

                //    var result = postTask.Result;
                //    if (result.IsSuccessStatusCode)
                //    {

                //        return RedirectToAction("SignUp", new { id = "1" });
                //    }
                //    else
                //    {
                //        if (result.StatusCode == HttpStatusCode.Found)
                //        {
                //            ModelState.AddModelError(string.Empty, "Email id already exists.");
                //        }else
                //        {
                //            ModelState.AddModelError(string.Empty, "something went wrong. Please try again.");
                //        }
                //    }
                //}
                //else
                //{

                    client.BaseAddress = new Uri("http://localhost:8087//api/Customer");

                    //HTTP POST
                    var postTask = client.PostAsJsonAsync("http://localhost:8087//api/Customer/PostNewUser", customer);
                    postTask.Wait();

                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction("SignIn");
                    }else
                    {
                        if(result.StatusCode==HttpStatusCode.Found)
                        {
                            ModelState.AddModelError(string.Empty, "Email id already exists.");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "something went wrong. Please try again.");
                        }
                    }
               // }
               
            }

           

            return View(customer);
        }

        public ActionResult CustomerSignUp()
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
        public ActionResult CustomerSignUp(Marchent_Account marchent)
        {
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri("http://localhost:8087//api/Marchent");

                //HTTP POST
                var postTask = client.PostAsJsonAsync("http://localhost:8087//api/Marchent/PostNewMerchant", marchent);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("SignIn");
                }
                else
                {
                    IEnumerable<State> state = null;

                    var responseTask1 = client.GetAsync("http://localhost:8087//api/State/GetAllStates");
                    responseTask1.Wait();

                    var result1 = responseTask1.Result;
                    if (result1.IsSuccessStatusCode)
                    {
                        var readTask1 = result1.Content.ReadAsAsync<IList<State>>();
                        readTask1.Wait();

                        state = readTask1.Result;
                        ViewBag.states = state;
                    }
                    else //web api sent error response 
                    {
                        state = Enumerable.Empty<State>();
                        ViewBag.states = state;
                        ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    }
                }
                ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
                return View(marchent);
            }
        }

        [HttpPost]
        public ActionResult Signout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("SignIn");
        }

    }
}

