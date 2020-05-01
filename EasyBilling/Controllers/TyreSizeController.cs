using EasyBilling.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EasyBilling.Controllers
{
    [Authorize]
    public class TyreSizeController : MybaseController
    {
        // GET: TyreSize
        public ActionResult Index()
        {
            
                return View();
           
        }
        public FileResult Sizeexport()
        {
            EasyBillingEntities db = new EasyBillingEntities();
            List<Tyre_size> data = db.Tyre_sizes.ToList();
            string spath = System.Web.HttpContext.Current.Server.MapPath("~/Export");
            if (data.Count > 0)
            {
                DataTable dataTable = new DataTable();
                PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(Tyre_size));

                for (int i = 0; i < props.Count; i++)
                {
                    PropertyDescriptor prop = props[i];
                 //   dataTable.Columns.Add(prop.Name, prop.PropertyType);
                    dataTable.Columns.Add(prop.Name, Nullable.GetUnderlyingType(
          prop.PropertyType) ?? prop.PropertyType);
                }
                object[] values = new object[props.Count];
                foreach (Tyre_size item in data)
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

                System.IO.File.WriteAllLines(spath + "/" + "Size export data.csv", lines);


                //   Process.Start(spath + "/" + "Export data.csv");
            }
            byte[] fileBytes = System.IO.File.ReadAllBytes(spath + "/" + "Size export data.csv");
            string fileName = "Size export data.csv";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);


        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<JsonResult> Create(Tyre_size tyre_Size)
        {
            using (EasyBillingEntities db = new EasyBillingEntities())
            {
                if (!string.IsNullOrEmpty(tyre_Size.Tyre_size1))
                {
                    bool chk = db.Tyre_sizes.Where(z => z.Tyre_size1.ToLower().Equals(tyre_Size.Tyre_size1.ToLower())).Any();
                    if (chk != true)
                    {
                        tyre_Size.Token_number = Guid.NewGuid().ToString();
                        if (ModelState.IsValid)
                        {
                            db.Tyre_sizes.Add(tyre_Size);
                            await db.SaveChangesAsync();
                            ModelState.Clear();

                            var _data = db.Tyre_sizes.ToList();
                            string json = JsonConvert.SerializeObject(_data.ToArray());

                            json = "{\"data\":" + json + "}";
                            //write string to file
                            var par = Server.MapPath("~/Json/TyreSize.json");
                            System.IO.File.WriteAllText(par, json);
                            return Json(tyre_Size);

                        }
                        else
                        {
                            return Json("Something went wrong. Please check your data and try again.");
                        }
                    }
                    else
                    {
                        return Json(tyre_Size.Tyre_size1 + " already exists. Please try with another.");
                    }
                }else
                {
                    return Json("Tyre size cannot be blank.");
                }
            }
        }
        public ActionResult Details(string id)
        {
            using (EasyBillingEntities db = new EasyBillingEntities())
            {
                return View(db.Tyre_sizes.Where(z => z.Token_number == id).Distinct().FirstOrDefault());
            }
        }
        public ActionResult Update(string id)
        {
            using (EasyBillingEntities db = new EasyBillingEntities())
            {
                return View(db.Tyre_sizes.Where(z => z.Token_number == id).Distinct().FirstOrDefault());
            }
        }
        [HttpPost]
        public async Task<ActionResult> Update(Tyre_size tyre_Size)
        {
            using (EasyBillingEntities db = new EasyBillingEntities())
            {
                Tyre_size tyresizesforupdate = db.Tyre_sizes.Where(z => z.Token_number == tyre_Size.Token_number).Distinct().FirstOrDefault();
                if (tyresizesforupdate != null)
                {
                    tyresizesforupdate.Tyre_size1 = tyre_Size.Tyre_size1;
                    tyresizesforupdate.With_tube = tyre_Size.With_tube;

                     await db.SaveChangesAsync();
                    ModelState.Clear();
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(tyre_Size);
                }

            }
        }
        public ActionResult Delete(string id)
        {
            using (EasyBillingEntities db = new EasyBillingEntities())
            {
                return View(db.Tyre_sizes.Where(z => z.Token_number == id).Distinct().FirstOrDefault());
            }
        }
        [HttpPost]
        public async Task<ActionResult> Delete(Tyre_size tyre_Size)
        {
            using (EasyBillingEntities db = new EasyBillingEntities())
            {
                Tyre_size tyresizeforDeletion = db.Tyre_sizes.Where(z => z.Token_number == tyre_Size.Token_number).Distinct().FirstOrDefault();
                if (tyresizeforDeletion == null )
                {
                    return View(tyre_Size);
                   
                }
                else
                {
                    db.Tyre_sizes.Remove(tyresizeforDeletion);
                    await db.SaveChangesAsync();
                    ModelState.Clear();
                    return RedirectToAction("Index");
                }
            }
        }
    }
}