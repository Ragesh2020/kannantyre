using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using EasyBilling.Models;

namespace EasyBilling.Controllers
{
    public class ProprietorController : Controller
    {
        // GET: Proprietor
        public ActionResult Index()
        {
            using (EasyBillingEntities db = new EasyBillingEntities())
            {
                return View(db.Proprietors.Distinct().ToList());
            }

        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Create(Proprietor proprietor)
        {
            using (EasyBillingEntities db = new EasyBillingEntities())
            {
                proprietor.Token_number = Guid.NewGuid().ToString();
                if (ModelState.IsValid)
                {
                    proprietor.State_Code = 19;
                    proprietor.State_Name = "Karnataka";
                    db.Proprietors.Add(proprietor);
                    await db.SaveChangesAsync();
                    ModelState.Clear();
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(proprietor);
                }
            }
        }
        public ActionResult Details(string id)
        {
            using (EasyBillingEntities db = new EasyBillingEntities())
            {
                return View(db.Proprietors.Where(z => z.Token_number == id).Distinct().FirstOrDefault());
            }
        }
        public ActionResult Update(string id)
        {
            using (EasyBillingEntities db = new EasyBillingEntities())
            {
                return View(db.Proprietors.Where(z => z.Token_number == id).Distinct().FirstOrDefault());
            }
        }
        [HttpPost]
        public async Task<ActionResult> Update(Proprietor proprietor)
        {
            using (EasyBillingEntities db = new EasyBillingEntities())
            {
                Proprietor proprietorforupdate = db.Proprietors.Where(z => z.Token_number == proprietor.Token_number).Distinct().FirstOrDefault();
                if (proprietorforupdate != null)
                {
                    proprietorforupdate.Address = proprietor.Address;
                    proprietorforupdate.Email_Id = proprietor.Email_Id;
                    proprietorforupdate.IsActive = proprietor.IsActive;
                    proprietorforupdate.Proprietor_name = proprietor.Proprietor_name;
                    proprietorforupdate.Mobile = proprietor.Mobile;
                    proprietorforupdate.Pan_Number = proprietor.Pan_Number;
                    proprietorforupdate.State_Code = proprietor.State_Code;
                    proprietorforupdate.State_Name = proprietor.State_Name;
                    proprietorforupdate.Verification_code = proprietor.Verification_code;

                    await db.SaveChangesAsync();
                    ModelState.Clear();
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(proprietor);
                }

            }
        }
        public ActionResult Delete(string id)
        {
            using (EasyBillingEntities db = new EasyBillingEntities())
            {
                return View(db.Proprietors.Where(z => z.Token_number == id).Distinct().FirstOrDefault());
            }
        }
        [HttpPost]
        public async Task<ActionResult> Delete(Proprietor proprietor)
        {
            using (EasyBillingEntities db = new EasyBillingEntities())
            {
                Proprietor propietorforDeletion = db.Proprietors.Where(z => z.Token_number == proprietor.Token_number).Distinct().FirstOrDefault();
                if (propietorforDeletion != null)
                {
                    db.Proprietors.Remove(propietorforDeletion);
                    await db.SaveChangesAsync();
                    ModelState.Clear();
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(proprietor);
                }
            }
        }
    }
}