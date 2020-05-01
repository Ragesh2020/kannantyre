using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EasyBilling.Models;
using System.IO;
using Newtonsoft.Json;
using System.ComponentModel;

namespace EasyBilling.Controllers
{
    [Authorize]
    public class ItemController : MybaseController
    {
        private EasyBillingEntities db = new EasyBillingEntities();

        // GET: Item
        public ActionResult Index()
        {
            ViewBag.company = db.Products.ToList();
            ViewBag.tyresize = db.Tyre_sizes.OrderBy(z=>z.Tyre_size1).ToList();
            ViewBag.vehicletype = db.Vehicles.Select(s => new VehicleClass()
            {
                Token_number = s.Token_number,
               
                Vehicle_type = s.Vehicle_type +" + "+ s.Vehicle_make
               
            }).Distinct().ToList<VehicleClass>();

           // ViewBag.vehicletype = db.Vehicles.Distinct().ToList();
            return View();
        }
        public JsonResult GetSize(string term)
        {
            List<string> size = (from sz in db.Tyre_sizes

                                     where sz.Tyre_size1.Contains(term)
                                     select sz.Tyre_size1).Distinct().ToList();
            if (size.Count == 0)
            {
                size.Add("No size is matched with this entry...");
            }
            return Json(size, JsonRequestBehavior.AllowGet);
        }
        public FileResult Itemexport()
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
                   // dataTable.Columns.Add(prop.Name, prop.PropertyType);
                    dataTable.Columns.Add(prop.Name, Nullable.GetUnderlyingType(
          prop.PropertyType) ?? prop.PropertyType);
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

                System.IO.File.WriteAllLines(spath + "/" + "Item export data.csv", lines);


                //   Process.Start(spath + "/" + "Export data.csv");
            }
            byte[] fileBytes = System.IO.File.ReadAllBytes(spath + "/" + "Item export data.csv");
            string fileName = "Item export data.csv";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);


        }
        public FileResult ItemTubeexport()
        {
            EasyBillingEntities db = new EasyBillingEntities();
            List<Item_Tube> data = db.Item_Tubes.ToList();
            string spath = System.Web.HttpContext.Current.Server.MapPath("~/Export");
            if (data.Count > 0)
            {
                DataTable dataTable = new DataTable();
                PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(Item_Tube));

                for (int i = 0; i < props.Count; i++)
                {
                    PropertyDescriptor prop = props[i];
                   // dataTable.Columns.Add(prop.Name, prop.PropertyType);
                    dataTable.Columns.Add(prop.Name, Nullable.GetUnderlyingType(
          prop.PropertyType) ?? prop.PropertyType);
                }
                object[] values = new object[props.Count];
                foreach (Item_Tube item in data)
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

                System.IO.File.WriteAllLines(spath + "/" + "Item Tube export data.csv", lines);


                //   Process.Start(spath + "/" + "Export data.csv");
            }
            byte[] fileBytes = System.IO.File.ReadAllBytes(spath + "/" + "Item Tube export data.csv");
            string fileName = "Item Tube export data.csv";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);


        }
        // GET: Item/Create
        public ActionResult Create()
        {
            Random rd = new Random();
            int id = rd.Next(1, 99999999);
            ViewBag.itemId = id.ToString();
            return View();
        }

        // POST: Item/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public async Task<JsonResult> Create(Item_Tyre item_Tyre)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(item_Tyre.Company_token) || !string.IsNullOrEmpty(item_Tyre.Tyre_token) || !string.IsNullOrEmpty(item_Tyre.Tyre_feel) || !string.IsNullOrEmpty(item_Tyre.Tyre_make) || !string.IsNullOrEmpty(item_Tyre.Tyre_type))

                {
                    item_Tyre.Token_number = Guid.NewGuid().ToString();
                    item_Tyre.Company_name = db.Products.Where(z => z.Token_Number == item_Tyre.Company_token).Select(z => z.Product_name).Distinct().FirstOrDefault();
                    item_Tyre.Tyre_size = db.Tyre_sizes.Where(z => z.Token_number == item_Tyre.Tyre_token).Select(z => z.Tyre_size1).Distinct().FirstOrDefault();

                    item_Tyre.Vehicle_type = db.Vehicles.Where(z => z.Token_number == item_Tyre.Vehicle_token).Select(z => z.Vehicle_type).Distinct().FirstOrDefault();
                    var lastId = (from a in db.Item_Tyres
                                  orderby a.Item_Id descending
                                  select a.Item_Id).Distinct().FirstOrDefault();
                    if (!string.IsNullOrEmpty(lastId))
                    {
                        var text = lastId;
                        var fstfr = text.Substring(0, 9);
                        var lstfr = text.Substring(text.Length - 8);
                        string newlstversn = (int.Parse(lstfr) + 100000001).ToString();
                        string fstfr1 = (newlstversn.Substring(newlstversn.Length - 8)).ToString();
                        if (item_Tyre.Tyre_type.ToLower().Equals("tubeless"))
                            item_Tyre.Item_Id = "TY" + item_Tyre.Company_name.Substring(0, 3) + item_Tyre.Tyre_size + "TUL-" + fstfr1;
                        else
                            item_Tyre.Item_Id = "TY" + item_Tyre.Company_name.Substring(0, 3) + item_Tyre.Tyre_size + "TUB-" + fstfr1;
                    }
                    else
                    {
                        if (item_Tyre.Tyre_type.ToLower().Equals("tubeless"))
                            item_Tyre.Item_Id = "TY" + item_Tyre.Company_name.Remove(3) + item_Tyre.Tyre_size + "TUL-00000001";
                        else
                            item_Tyre.Item_Id = "TY" + item_Tyre.Company_name.Remove(3) + item_Tyre.Tyre_size + "TUB-00000001";
                    }

                    if (!string.IsNullOrEmpty(item_Tyre.Item_Id))
                    {
                        bool chk = db.Item_Tyres.Where(z => z.Company_name.ToLower() == item_Tyre.Company_name.ToLower()
                        && z.Tyre_type.ToLower() == item_Tyre.Tyre_type.ToLower()
                        && z.Tyre_feel.ToLower() == item_Tyre.Tyre_feel.ToLower()
                        && z.Tyre_make.ToLower() == item_Tyre.Tyre_make.ToLower()
                        && z.Tyre_size.ToLower() == item_Tyre.Tyre_size.ToLower()
                        && z.Vehicle_type.ToLower() == item_Tyre.Vehicle_type.ToLower()).Any();
                        if (chk == true)
                        {
                            return Json("Your entry already exist. Please try again another...");
                        }
                        else
                        {
                            db.Item_Tyres.Add(item_Tyre);
                            await db.SaveChangesAsync();
                            var _data = await db.Item_Tyres.ToListAsync();
                            string json = JsonConvert.SerializeObject(_data.ToArray(), Formatting.Indented,
        new JsonSerializerSettings()
        {
            ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
        }
        );
                            json = "{\"data\":" + json + "}";
                            //write string to file
                            var par = Server.MapPath("~/Json/sampledata.json");
                            System.IO.File.WriteAllText(par, json);
                            return Json(item_Tyre);
                        }
                    }
                    else
                    {
                        return Json("Something went wrong to generate Item id. Please refresh the page and try again. Thank you.");
                    }
                }
                else
                {
                    return Json("Company name, tyre type, tyre feel, tyre make, tyre size must be selected. ");
                }

            }
            else
            {
                return Json("Please check your entry and try again. Thank you. [Note: all fields mandatory without vehicle and description]");
            }
        }

        public ActionResult ItemTubeList()
        {
            ViewBag.Company = db.Products.Distinct().ToList();
            ViewBag.size = db.Tyre_sizes.OrderByDescending(z => z.Tyre_size1).Distinct().ToList();
            return View();
        }


        // GET: Item/Create
        public ActionResult CreateItemTube()
        {
            Random rd = new Random();
            int id = rd.Next(1, 99999999);
            ViewBag.itemId = id.ToString();
            return View();
        }

        // POST: Item/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public async Task<JsonResult> CreateItemTube(Item_Tube item_Tube)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(item_Tube.Company_token) || !string.IsNullOrEmpty(item_Tube.Size_token))
                {
                    item_Tube.Token_number = Guid.NewGuid().ToString();
                    item_Tube.Company_name = db.Products.Where(z => z.Token_Number == item_Tube.Company_token).Select(z => z.Product_name).FirstOrDefault();
                    item_Tube.Tube_size = db.Tyre_sizes.Where(z => z.Token_number == item_Tube.Size_token).Select(z => z.Tyre_size1).FirstOrDefault();
                    var lastId = (from a in db.Item_Tubes
                                  orderby a.Item_Id descending
                                  select a.Item_Id).Distinct().FirstOrDefault();
                    if (!string.IsNullOrEmpty(lastId))
                    {
                        var text = lastId;
                        var fstfr = text.Substring(0, 9);
                        var lstfr = text.Substring(text.Length - 8);
                        string newlstversn = (int.Parse(lstfr) + 100000001).ToString();
                        string fstfr1 = (newlstversn.Substring(newlstversn.Length - 8)).ToString();
                        item_Tube.Item_Id = "TU" + item_Tube.Company_name.Substring(0, 3) + item_Tube.Tube_size + "-" + fstfr1;
                    }
                    else
                    {
                        item_Tube.Item_Id = "TU" + item_Tube.Company_name.Substring(0, 3) + item_Tube.Tube_size + "-00000001";
                    }

                    if (!string.IsNullOrEmpty(item_Tube.Item_Id))
                    {

                        bool chk = db.Item_Tubes.Where(z => z.Company_name.ToLower() == item_Tube.Company_name.ToLower() && z.Tube_size.ToLower() == item_Tube.Tube_size.ToLower()).Any();
                        if (chk == false)
                        {
                            db.Item_Tubes.Add(item_Tube);
                            await db.SaveChangesAsync();
                            var _data = db.Item_Tubes.ToList();
                            string json = JsonConvert.SerializeObject(_data.ToArray(), Formatting.Indented,
    new JsonSerializerSettings()
    {
        ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
    }
    );
                            json = "{\"data\":" + json + "}";
                            //write string to file
                            var par = Server.MapPath("~/Json/ItemTube.json");
                            System.IO.File.WriteAllText(par, json);
                            return Json(item_Tube);
                        }
                        else
                        {
                            return Json(item_Tube.Company_name + "along with " + item_Tube.Tube_size + " is already exist. Please try with another.");
                        }
                    }
                    else
                    {
                        return Json("Item Id cannot be blank. Please try again.");
                    }
                }
                else
                {
                    return Json("Company name and size must be selected.");
                }
            }
            else

                return Json("Please check your entry and try again. Thank you.");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
