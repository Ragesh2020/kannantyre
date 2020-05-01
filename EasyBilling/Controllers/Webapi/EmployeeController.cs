using EasyBilling.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace EasyBilling.Controllers.Webapi
{
    [RoutePrefix("api/employee")]
    public class EmployeeController : ApiController
    {
        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> GetAll()
        {
            IList<EmployeeClass> emp = null;

            using (var ctx = new EasyBillingEntities())
            {

                emp = ctx.Employees.Select(s => new EmployeeClass()
                {
                    Token_number = s.Token_number,
                    Employee_Id = s.Employee_Id,
                    Employee_name = s.Employee_name,
                    Designation = s.Designation,
                    Contact_number = s.Contact_number,
                    Email_id = s.Email_id,
                    Salary = s.Salary,
                    login_required = s.login_required,
                    Joining_date = s.Joining_date,
                    Leaving_date = s.Leaving_date,
                    Date = s.Date


                }).OrderByDescending(x => x.Date).Distinct().ToList();


            }
            return Ok(emp);

        }
        [HttpPost]
        [Route("Deletesel")]
        public async Task<IHttpActionResult> Deletesel(List<string> list)
        {

            using (var ctx = new EasyBillingEntities())
            {
                ctx.Employees.RemoveRange(ctx.Employees.Where(x => list.Contains(x.Token_number)).ToList());
                ctx.SaveChanges();

                return Ok();

            }


        }
        [HttpPost]
        [Route("PostSave")]
        public async Task<IHttpActionResult> PostSave([FromBody] Employee employee)
        {
            using (EasyBillingEntities db = new EasyBillingEntities())
            {
               // employee.Date = DateTime.Parse(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));
                employee.Leaving_date = DateTime.Parse("01-01-1999");
                Random rd = new Random();
                var id = "KAN" + rd.Next(0000, 9999);
                bool empidchk = db.Employees.Where(z => z.Employee_Id == id).Any();
                if (empidchk)
                    id = "KAN" + rd.Next(0000, 9999);
                employee.Employee_Id = id;
                if (ModelState.ContainsKey("employee.login_required"))
                    ModelState["employee.login_required"].Errors.Clear();
                if (ModelState.IsValid)
                {
                    if (!string.IsNullOrEmpty(employee.Employee_Id))
                    {
                        bool Employeecheck = db.Employees.Where(z => z.Employee_Id == employee.Employee_Id).Any();
                        bool Employeecontctcheck = db.Employees.Where(z => z.Contact_number == employee.Contact_number).Any();
                        bool Employeeemailcheck = db.Employees.Where(z => z.Email_id == employee.Email_id).Any();
                        if (Employeecontctcheck == true)
                        {
                            return BadRequest("Contact number exists");
                        }
                        if (Employeeemailcheck == true)
                        {
                            return BadRequest("Email exists");
                        }
                        if (Employeecheck == true)
                        {
                            return BadRequest("Employee id should not duplicate");
                        }
                        else
                        {
                            employee.Token_number = Guid.NewGuid().ToString();
                            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
                            String sMacAddress = string.Empty;
                            foreach (NetworkInterface adapter in nics)
                            {
                                IPInterfaceProperties properties = adapter.GetIPProperties();
                                sMacAddress = adapter.GetPhysicalAddress().ToString();
                                if (!string.IsNullOrEmpty(sMacAddress))// only return MAC Address from first card  
                                    break;
                            }
                            employee.Mac_id = sMacAddress;
                            employee.Date = DateTime.Now;
                            StringBuilder builder = new StringBuilder();
                            builder.Append(RandomString(4));
                            builder.Append(RandomNumber(1000, 9999));
                            builder.Append(RandomString(2));
                            employee.Password= builder.ToString();
                            employee.User_Id = User.Identity.Name;
                           var userdet = db.Employees.Where(z => z.Employee_Id == User.Identity.Name).Select(z => new { z.Employee_name , z.Designation }).FirstOrDefault();
                            employee.User_name = userdet.Employee_name;
                            db.Employees.Add(employee);
                            await db.SaveChangesAsync();
                            //MailMessage mailMessage = new MailMessage("kannantyresgonikoppal@gmail.com", employee.Email_id);
                            //mailMessage.Subject = "New Kannantyres user account and details";
                            //mailMessage.IsBodyHtml = true;
                            //mailMessage.Body = "User Id: " + employee.Employee_Id + "<br/>Password: " + employee.Password;
                            //SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                            //smtp.Credentials = new NetworkCredential("kannantyresgonikoppal@gmail.com", "Kan@12345");
                            //smtp.Host = "smtp.gmail.com";
                            //smtp.EnableSsl = true;
                            //smtp.Send(mailMessage);
                            // mailMessage = new MailMessage(employee.Email_id,"kannantyresgonikoppal@gmail.com");
                            //mailMessage.Subject = "New Kannantyres user account and details";
                            //mailMessage.IsBodyHtml = true;
                            //mailMessage.Body = "New user account has been created<br/>User name: " + employee.Employee_name + "<br/>User Id: " + employee.Employee_Id + "<br/>Password: " + employee.Password + "<br/>Created by: " + userdet.Employee_name +"("+ userdet.Designation+")";
                            // smtp = new SmtpClient("smtp.gmail.com", 587);
                            //smtp.Credentials = new NetworkCredential("kannantyresgonikoppal@gmail.com", "Kan@12345");
                            //smtp.Host = "smtp.gmail.com";
                            //smtp.EnableSsl = true;
                            //smtp.Send(mailMessage);
                            return Created("", employee);
                        }
                    }
                    else
                    {
                        return BadRequest("Employee Id should not empty");
                    }
                }
                else
                {
                    var snglerrr = string.Empty;
                    //snglerrr = "-----------Error------------------";
                    foreach (var errr in ModelState.Values)
                    {

                        for (int i = 0; i < errr.Errors.Count; i++)
                        {
                            int j = i + 1;

                            if (string.IsNullOrEmpty(snglerrr))
                                snglerrr = j + ". " + errr.Errors[i].ErrorMessage;
                            else
                                snglerrr = snglerrr + "\n" + j + ". " + errr.Errors[i].ErrorMessage;
                            //errors.Add(snglerrr);
                        }
                    }

                    return BadRequest(snglerrr);
                }
            }
        }

        private int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        private string RandomString(int size)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            
            return builder.ToString();
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IHttpActionResult> GetById([FromUri] string id)
        {
            using (EasyBillingEntities db = new EasyBillingEntities())
            {
                return await Task.FromResult(Ok(db.Employees.Where(z => z.Token_number == id).Distinct().FirstOrDefault()));
            }
        }


        [HttpDelete]
        [Route("{id}")]
        public async Task<IHttpActionResult> Delete([FromUri]string id)
        {
            using (EasyBillingEntities db = new EasyBillingEntities())
            {
                Employee employee = db.Employees.Where(z => z.Token_number == id).Distinct().FirstOrDefault();

                if (employee != null)
                {
                    db.Employees.Remove(employee);
                    await db.SaveChangesAsync();
                    ModelState.Clear();
                    return Ok();
                }
                return BadRequest($"Invalid token number {id}");

            }
        }

    }
}
