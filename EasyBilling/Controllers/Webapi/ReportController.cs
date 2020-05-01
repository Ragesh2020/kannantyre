using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace EasyBilling.Controllers.Webapi
{
    public class ReportController : ApiController
    {
        //public JsonResult GetStudents(string term)
        //{
        //    using (EasyBilling.Models.EasyBillingEntities db = new Models.EasyBillingEntities())
        //    {
        //        var dcmlempty = decimal.Parse("0.000");
        //        List<string> _Products_For_Sales = (from pdfs in db.Products_For_Sales
        //                                            join stk in db.Stocks on pdfs.Token_Number equals stk.Product_Token
        //                                            where (stk.Pieces != 0 || stk.Quantity != dcmlempty) && pdfs.IsActive == true
        //                                            && pdfs.Name.Contains(term)
        //                                            select pdfs.Name).Distinct().ToList();

        //        return Json(JsonConvert.DeserializeObject<List<string>>(_Products_For_Sales));
        //    }
        //}
    }
}
