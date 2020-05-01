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
    public class EmployeeController : MybaseController
    {
        // GET: Employee
        public ActionResult Index()
        {
                return View();
        }
        public FileResult Employeeexport()
        {
            EasyBillingEntities db = new EasyBillingEntities();
            List<Employee> data = db.Employees.ToList();
            string spath = System.Web.HttpContext.Current.Server.MapPath("~/Export");
            if (data.Count > 0)
            {
                DataTable dataTable = new DataTable();
                PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(Employee));

                for (int i = 0; i < props.Count; i++)
                {
                    PropertyDescriptor prop = props[i];
                  //  dataTable.Columns.Add(prop.Name, prop.PropertyType);
                    dataTable.Columns.Add(prop.Name, Nullable.GetUnderlyingType(
            prop.PropertyType) ?? prop.PropertyType);
                }
                object[] values = new object[props.Count];
                foreach (Employee item in data)
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

                System.IO.File.WriteAllLines(spath + "/" + "Employee export data.csv", lines);


                //   Process.Start(spath + "/" + "Export data.csv");
            }
            byte[] fileBytes = System.IO.File.ReadAllBytes(spath + "/" + "Employee export data.csv");
            string fileName = "Employee export data.csv";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);


        }
        public JsonResult GetDesignation(string term)
        {
            List<Employee> ObjList = new List<Employee>()
            {

                new Employee {Designation="Latur" },
                new Employee {Designation="Mumbai" },
                new Employee {Designation="Pune" },
                new Employee {Designation="Delhi" },
                new Employee {Designation="Dehradun" },
                new Employee {Designation="Noida" },
             

        };
          //  string[] lstofdesig = { "Propritor", "Manager", "Asst manager", "Sales Man", "Foreman", "Employee" };
            List<string> descchk = (from item in ObjList

                                    where item.Designation.Contains(term)
                                             select item.Designation).Distinct().ToList();
         
            if (descchk.Count == 0)
            {
                descchk.Add("No employee designation is matched with this entry...");
            }
            return Json(descchk, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<JsonResult> Create(Employee employee)
        {
            using (EasyBillingEntities db = new EasyBillingEntities())
            {
                employee.Token_number = Guid.NewGuid().ToString();
                if (ModelState.IsValid)
                {
                    bool check = db.Employees.Where(z => z.Employee_Id.Equals(employee.Employee_Id)).Any();
                    if (check == true)
                        return Json("Employee id exists");
                    else
                    {
                        db.Employees.Add(employee);
                        await db.SaveChangesAsync();
                        ModelState.Clear();
                        var _data = db.Employees.ToList();
                        string json = JsonConvert.SerializeObject(_data.ToArray());

                        json = "{\"data\":" + json + "}";
                        //write string to file
                        var par = Server.MapPath("~/Json/Employee.json");
                        System.IO.File.WriteAllText(par, json);
                        return Json(employee);
                    }
                }
                else
                {
                    return Json("Please check your credentials. please try again.");
                }

            }
        }
        public ActionResult Details(string id)
        {
            using (EasyBillingEntities db = new EasyBillingEntities())
            {
                return View(db.Employees.Where(z => z.Token_number == id).Distinct().FirstOrDefault());
            }
        }
        public ActionResult Update(string id)
        {
            using (EasyBillingEntities db = new EasyBillingEntities())
            {
                return View(db.Employees.Where(z => z.Token_number == id).Distinct().FirstOrDefault());
            }
        }
        [HttpPost]
        public async Task<ActionResult> Update(Employee employee)
        {
            using (EasyBillingEntities db = new EasyBillingEntities())
            {
                Employee Employeeforupdate = db.Employees.Where(z => z.Token_number == employee.Token_number).Distinct().FirstOrDefault();
                if (Employeeforupdate != null)
                {
                    Employeeforupdate.Contact_number = employee.Contact_number;
                    Employeeforupdate.Designation = employee.Designation;
                    Employeeforupdate.Email_id = employee.Email_id;
                    Employeeforupdate.Employee_Id = employee.Employee_Id;
                    Employeeforupdate.Employee_name = employee.Employee_name;
                    Employeeforupdate.Joining_date = employee.Joining_date;
                    Employeeforupdate.Leaving_date = employee.Leaving_date;
                    Employeeforupdate.login_required = employee.login_required;
                    Employeeforupdate.Salary = employee.Salary;
                  

                    await db.SaveChangesAsync();
                    ModelState.Clear();
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(employee);
                }

            }
        }
        public ActionResult Delete(string id)
        {
            using (EasyBillingEntities db = new EasyBillingEntities())
            {
                return View(db.Employees.Where(z => z.Token_number == id).Distinct().FirstOrDefault());
            }
        }
        [HttpPost]
        public async Task<ActionResult> Delete(Employee employee)
        {
            using (EasyBillingEntities db = new EasyBillingEntities())
            {
                Employee EmpforDeletion = db.Employees.Where(z => z.Token_number == employee.Token_number).Distinct().FirstOrDefault();
                if (EmpforDeletion != null)
                {
                    db.Employees.Remove(EmpforDeletion);
                    await db.SaveChangesAsync();
                    ModelState.Clear();
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(employee);
                }
            }
        }
    }
}