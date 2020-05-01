using EasyBilling.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EasyBilling.Controllers
{
    public class ManagerController : Controller
    {
        // GET: Manager
        public ActionResult Index()
        {
            using (EasyBillingEntities db = new EasyBillingEntities())
            {
                return View(db.Managers.Distinct().ToList());
            }

        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Create(Manager manager)
        {
            using (EasyBillingEntities db = new EasyBillingEntities())
            {
                manager.Token_number = Guid.NewGuid().ToString();
                if(ModelState.IsValid)
                {
                    db.Managers.Add(manager);
                   await db.SaveChangesAsync();
                    ModelState.Clear();
                    return RedirectToAction("Index");
                }else
                {
                    return View(manager);
                }
             
            }
        }
        public ActionResult Details(string id)
        {
            using (EasyBillingEntities db = new EasyBillingEntities())
            {
                return View(db.Managers.Where(z=>z.Token_number==id).Distinct().FirstOrDefault());
            }
        }
        public ActionResult Update(string id)
        {
            using (EasyBillingEntities db = new EasyBillingEntities())
            {
                return View(db.Managers.Where(z => z.Token_number == id).Distinct().FirstOrDefault());
            }
        }
        [HttpPost]
        public async Task<ActionResult> Update(Manager manager)
        {
            using (EasyBillingEntities db = new EasyBillingEntities())
            {
                Manager managerforupdate = db.Managers.Where(z => z.Token_number == manager.Token_number).Distinct().FirstOrDefault();
                if( managerforupdate!=null)
                {
                    managerforupdate.Address = manager.Address;
                    managerforupdate.Email_Id = manager.Email_Id;
                    managerforupdate.IsActive = manager.IsActive;
                    managerforupdate.Manager_name = manager.Manager_name;
                    managerforupdate.Mobile = manager.Mobile;
                    managerforupdate.Pan_Number = manager.Pan_Number;
                    managerforupdate.State_Code = manager.State_Code;
                    managerforupdate.State_Name = manager.State_Name;
                    managerforupdate.Verification_code = manager.Verification_code;

                    await db.SaveChangesAsync();
                    ModelState.Clear();
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(manager);
                }

            }
        }
        public ActionResult Delete(string id)
        {
            using (EasyBillingEntities db = new EasyBillingEntities())
            {
                return View(db.Managers.Where(z => z.Token_number == id).Distinct().FirstOrDefault());
            }
        }
        [HttpPost]
        public async Task<ActionResult> Delete(Manager manager)
        {
            using (EasyBillingEntities db = new EasyBillingEntities())
            {
                Manager managerforDeletion = db.Managers.Where(z => z.Token_number == manager.Token_number).Distinct().FirstOrDefault();
                if (managerforDeletion != null)
                {
                    db.Managers.Remove(managerforDeletion);
                    await db.SaveChangesAsync();
                    ModelState.Clear();
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(manager);
                }
            }
        }
    }
}