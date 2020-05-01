using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EasyBilling.Models;

namespace EasyBilling.Controllers
{
  
    public class MybaseController : Controller
    {
        // GET: Mybase
        protected override void OnActionExecuting(ActionExecutingContext filterContext)

        {
            using (EasyBillingEntities db = new EasyBillingEntities())
            {
                    ViewBag.uname = db.Employees.Where(z => z.Employee_Id == User.Identity.Name).Select(z => z.Employee_name).Distinct().FirstOrDefault();
             
                ViewBag.placeorderpending = db.Placed_Orders.Where(z => z.Orderplaced == false).Distinct().ToList().Count();
                
            }
               
            base.OnActionExecuting(filterContext);
        }
    }
}