using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using EasyBilling.Models;
using PagedList;

namespace EasyBilling.Controllers
{
    public class TestController : MybaseController
    {
        private EasyBillingEntities db = new EasyBillingEntities();
        // GET: Test
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetItemDetails(string id)
        {

            var itms = db.Item_Tyres.Select(x => new
            {
                x.Token_number,
                x.Company_token,
                x.Item_Id,
                x.Tyre_feel,
                x.Tyre_make,
                x.Tyre_size,
                x.Tyre_type,
                x.Vehicle_type

            }).Where(z => z.Token_number == id).FirstOrDefault();
            return Json(itms, JsonRequestBehavior.AllowGet);
        }


    }
}