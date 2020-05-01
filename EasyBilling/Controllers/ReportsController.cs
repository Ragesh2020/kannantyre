using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EasyBilling.Models;

namespace EasyBilling.Controllers
{
    public class ReportsController : MybaseController
    {
        private EasyBillingEntities db = new EasyBillingEntities();
        // GET: Reports
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Index_backup()
        {
            return View();
        }
        public JsonResult GetSuppliers(string term)
        {
            List<string> suppliers = (from pdfs in db.Dealers
                                                
                                                where pdfs.Token_number.Contains(term)
                                                select pdfs.Token_number).Distinct().ToList();
            if (suppliers.Count == 0)
            {
                suppliers.Add("No supplier token is matched with this key...");
            }
            return Json(suppliers, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetInstitutes(string term)
        {
            List<string> institutes = (from pdfs in db.Customers

                                      where pdfs.Token_number.Contains(term)
                                      select pdfs.Token_number).Distinct().ToList();
            if (institutes.Count == 0)
            {
                institutes.Add("No Customers key is matched with this key...");
            }
            return Json(institutes, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetStocks(string term)
        {
            List<string> stcks = new List<string>();
            var stckid = int.Parse(term);
            var stkid = (from pdfs in db.Stocks

                                       where pdfs.Stock_Id== stckid
                                       select pdfs.Stock_Id).Distinct().ToList();
            if(stkid.Count==0)
            {
                stcks.Add("No stock Id is matched with this Id...");
            }else
            {
                stcks.Add("Stock Id is matched. You may proceed...");
            }
            //if (institutes.Count == 0)
            //{
            //    institutes.Add(stckid);
                
            //}
            return Json(stcks, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetPurchase(string term)
        {
            List<string> PurchaseToken = (from pdfs in db.Purchase_Masters

                                       where pdfs.Purchase_Number.Contains(term)
                                       select pdfs.Purchase_Number).Distinct().ToList();
            if (PurchaseToken.Count == 0)
            {
                PurchaseToken.Add("No purchase number is matched with this key...");
            }
            return Json(PurchaseToken, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetInvoice(string term)
        {
            List<string> invoice = (from pdfs in db.Billing_Masters

                                       where pdfs.Billing_Number.Contains(term)
                                       select pdfs.Billing_Number).Distinct().ToList();
            if (invoice.Count == 0)
            {
                invoice.Add("No bill number is matched with this key...");
            }
            return Json(invoice, JsonRequestBehavior.AllowGet);
        }

    }
}