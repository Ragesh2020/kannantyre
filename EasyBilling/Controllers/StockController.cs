using CrystalDecisions.CrystalReports.Engine;
using EasyBilling.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace EasyBilling.Controllers
{
    [Authorize]
    public class StockController : MybaseController
    {
        // GET: Stock

        public JsonResult Getstockpernm(string id)
        {
            using (EasyBillingEntities db = new EasyBillingEntities())
            {
                var stktkn = db.Products_For_Sales.Where(z => z.Product_name.Contains(id)).Select(x => x.Token_Number).FirstOrDefault();
                var stkpcs = db.Stocks.Where(z => z.Product_Token == stktkn).Select(x => x.Pieces).Distinct().FirstOrDefault();
                return Json(stkpcs, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Index()
        {
            IEnumerable<Stock> Stk = null;
            
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:8087/api/Stock/GetAllStockData");
                //HTTP GET
                var responseTask = client.GetAsync("http://localhost:8087/api/Stock/GetAllStockData");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<Stock>>();
                    readTask.Wait();

                    Stk = readTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    Stk = Enumerable.Empty<Stock>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            return View(Stk);
        }
    }
}