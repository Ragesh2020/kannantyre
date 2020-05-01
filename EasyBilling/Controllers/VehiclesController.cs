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
using Newtonsoft.Json;
using System.ComponentModel;

namespace EasyBilling.Controllers
{
    [Authorize]
    public class VehiclesController : MybaseController
    {
        private EasyBillingEntities db = new EasyBillingEntities();

        // GET: Vehicles
        public ActionResult Index()
        {
            return View();
        }

        public FileResult Vehiclesexport()
        {
            EasyBillingEntities db = new EasyBillingEntities();
            List<Vehicle> data = db.Vehicles.ToList();
            string spath = System.Web.HttpContext.Current.Server.MapPath("~/Export");
            if (data.Count > 0)
            {
                DataTable dataTable = new DataTable();
                PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(Vehicle));

                for (int i = 0; i < props.Count; i++)
                {
                    PropertyDescriptor prop = props[i];
                   // dataTable.Columns.Add(prop.Name, prop.PropertyType);
                    dataTable.Columns.Add(prop.Name, Nullable.GetUnderlyingType(
          prop.PropertyType) ?? prop.PropertyType);
                }
                object[] values = new object[props.Count];
                foreach (Vehicle item in data)
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

                System.IO.File.WriteAllLines(spath + "/" + "Vehicle export data.csv", lines);


                //   Process.Start(spath + "/" + "Export data.csv");
            }
            byte[] fileBytes = System.IO.File.ReadAllBytes(spath + "/" + "Vehicle export data.csv");
            string fileName = "Vehicle export data.csv";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);


        }
        // GET: Vehicles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Vehicles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
       
        public async Task<JsonResult> Create([Bind(Include = "Token_number,Vehicle_type,Vehicle_make")] Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(vehicle.Vehicle_type) || !string.IsNullOrEmpty(vehicle.Vehicle_make))
                {
                    bool chk = db.Vehicles.Where(z => z.Vehicle_make.ToLower() == vehicle.Vehicle_make.ToLower() && z.Vehicle_type.ToLower() == vehicle.Vehicle_type.ToLower()).Any();

                    if (chk != true)
                    {
                        vehicle.Token_number = Guid.NewGuid().ToString();
                        db.Vehicles.Add(vehicle);
                        await db.SaveChangesAsync();
                        var _data = db.Vehicles.ToList();
                        string json = JsonConvert.SerializeObject(_data.ToArray());

                        json = "{\"data\":" + json + "}";
                        //write string to file
                        var par = Server.MapPath("~/Json/Vehicle.json");
                        System.IO.File.WriteAllText(par, json);
                        ModelState.Clear();
                        return Json(vehicle);
                    }
                    else
                    {
                        return Json(vehicle.Vehicle_type + " along with " + vehicle.Vehicle_make + " is already exist. Please try again with another. Thank you.");
                    }
                }
                else
                {
                    return Json("Please check your entry and try again. Thank you...");
                }
            }else
            {
                return Json("Type and make cannot be blank. Please check your entry and try again. Thank you...");
            }

           
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
