using EasyBilling.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Web.Http;

namespace EasyBilling.Controllers.Webapi
{
    [RoutePrefix("api/expenses")]
    public class ExpensesController : ApiController,MacAdd
    {
        public string macAdd()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            String sMacAddress = string.Empty;
            foreach (NetworkInterface adapter in nics)
            {
                IPInterfaceProperties properties = adapter.GetIPProperties();
                sMacAddress = adapter.GetPhysicalAddress().ToString();
                if (!string.IsNullOrEmpty(sMacAddress))// only return MAC Address from first card  
                    break;
            }
           return sMacAddress;
        }
        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> GetAll()
        {
            IList<Employee_salary_expenseClass> empsallist = null;
          
            using (var ctx = new EasyBillingEntities())
            {

                empsallist = ctx.Employee_salary_expenses.Select(s => new Employee_salary_expenseClass()
                {
                    Token_number = s.Token_number,
                    Advance_collected = s.Advance_collected,
                    Employee_name = s.Employee_name,
                    salary_to_be_paid = s.salary_to_be_paid,
                    Date=s.Date,
                    Monthly_salary=s.Monthly_salary,
                    Salary_paid=s.Salary_paid,
                    //monthYear= CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(s.Month) +s.Year,
                }).Distinct().ToList();


            }
            return Ok(empsallist);

        }
        [HttpGet]
        [Route("GetAllOtherExpenses")]
        public async Task<IHttpActionResult> GetAllOtherExpenses()
        {
            IList<OtherExpensesClass> otherExpensesClasses = null;

            using (var ctx = new EasyBillingEntities())
            {

                otherExpensesClasses = ctx.Other_expenses.Select(s => new OtherExpensesClass()
                {
                    Token_number = s.Token_number,
                    Other_expense_type = s.Other_expense_type,
                    Other_expense_amount = s.Other_expense_amount,

                }).Distinct().ToList();


            }
            return Ok(otherExpensesClasses);

        }
        [HttpPost]
        [Route("PostOtherExpensesSave")]
        public async Task<IHttpActionResult> PostOtherExpensesSave([FromBody] Other_expense other_Expense)
        {
            using (EasyBillingEntities db = new EasyBillingEntities())
            {
                if (ModelState.IsValid)
                {
                    other_Expense.Token_number = Guid.NewGuid().ToString();
                    var pmexits = db.Other_expenses.Any();
                    string text = null;
                    String totalvrsn = null;
                    if (pmexits == true)
                    {
                        text = (from a in db.Other_expenses
                                orderby a.ExpenseId descending
                                select a.ExpenseId).FirstOrDefault();

                        var fstfr = text.Substring(0, 4);
                        var lstfr = text.Substring(text.Length - 7);
                        string newlstversn = (int.Parse(lstfr) + 10000001).ToString();
                        string fstfr1 = (newlstversn.Substring(newlstversn.Length - 7)).ToString();
                        totalvrsn = fstfr + fstfr1;
                    }
                    else
                        totalvrsn = "EOTH0000001";
                    other_Expense.ExpenseId = totalvrsn;
                    other_Expense.Date = DateTime.Now.Date;
                    other_Expense.Mac_id = macAdd();
                    var userdet = db.Employees.Where(z => z.Employee_Id == User.Identity.Name).Select(z => new { z.Employee_name, z.Employee_Id }).FirstOrDefault();
                    if (userdet == null)
                    {
                        var merchantdet = db.Marchent_Accounts.Where(z => z.Email_Id == User.Identity.Name).Select(z => new { z.Marchent_name, z.Email_Id }).FirstOrDefault();
                        other_Expense.User_Id = merchantdet.Email_Id;
                        other_Expense.User_name = merchantdet.Marchent_name;
                    }
                    else
                    {
                        other_Expense.User_Id = userdet.Employee_Id;
                        other_Expense.User_name = userdet.Employee_name;
                    }
                    db.Other_expenses.Add(other_Expense);
                        await db.SaveChangesAsync();
                        return Created("", other_Expense);
                    
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
        [HttpGet]
        [Route("GetAllProductExpenses")]
        public async Task<IHttpActionResult> GetAllProductExpenses()
        {
            IList<productExpensesClass> productExpenseslist = null;

            using (var ctx = new EasyBillingEntities())
            {

                productExpenseslist = ctx.Product_expenses.Select(s => new productExpensesClass()
                {
                    Token_number = s.Token_number,
                    Product_name = s.Product_name,
                    Amount_for_expense = s.Amount_for_expense,
                  
                }).Distinct().ToList();


            }
            return Ok(productExpenseslist);

        }
        [HttpPost]
        [Route("PostProductexpenseSave")]
        public async Task<IHttpActionResult> PostProductexpenseSave([FromBody] Product_expense product_Expense)
        {
            using (EasyBillingEntities db = new EasyBillingEntities())
            {

                if (ModelState.IsValid)
                {
                    if (!string.IsNullOrEmpty(product_Expense.Product_token_number))
                    {
                        product_Expense.Token_number = Guid.NewGuid().ToString();
                        var pmexits = db.Product_expenses.Any();
                        string text = null;
                        String totalvrsn = null;
                        if (pmexits == true)
                        {
                            text = (from a in db.Product_expenses
                                    orderby a.ExpenseId descending
                                    select a.ExpenseId).FirstOrDefault();

                            var fstfr = text.Substring(0, 4);
                            var lstfr = text.Substring(text.Length - 7);
                            string newlstversn = (int.Parse(lstfr) + 10000001).ToString();
                            string fstfr1 = (newlstversn.Substring(newlstversn.Length - 7)).ToString();
                            totalvrsn = fstfr + fstfr1;
                        }
                        else
                            totalvrsn = "EPRO0000001";
                        product_Expense.ExpenseId = totalvrsn;
                        product_Expense.Date = DateTime.Now.Date;
                        product_Expense.Mac_id = macAdd();
                        var userdet = db.Employees.Where(z => z.Employee_Id == User.Identity.Name).Select(z => new { z.Employee_name, z.Employee_Id }).FirstOrDefault();
                        if (userdet == null)
                        {
                            var merchantdet = db.Marchent_Accounts.Where(z => z.Email_Id == User.Identity.Name).Select(z => new { z.Marchent_name, z.Email_Id }).FirstOrDefault();
                            product_Expense.User_Id = merchantdet.Email_Id;
                            product_Expense.User_name = merchantdet.Marchent_name;
                        }
                        else
                        {
                            product_Expense.User_Id = userdet.Employee_Id;
                            product_Expense.User_name = userdet.Employee_name;
                        }
                        product_Expense.Product_name = db.Other_Products.Where(z => z.Token_number == product_Expense.Product_token_number).Select(z => z.Product_name).FirstOrDefault();
                        db.Product_expenses.Add(product_Expense);
                        await db.SaveChangesAsync();
                        return Created("", product_Expense);
                    }
                    else
                    {
                        return BadRequest("At least one product must be selected.");
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
        [HttpPost]
        [Route("PostEmployee_salary_expenseSave")]
        public async Task<IHttpActionResult> PostEmployee_salary_expenseSave([FromBody] Employee_salary_expense employee_Salary_Expense)
        {
            //employee_Salary_Expense.Advance_collected = employee_Salary_Expense.Advance_collected == null ? 0 : employee_Salary_Expense.Advance_collected;
            //employee_Salary_Expense.salary_to_be_paid = employee_Salary_Expense.salary_to_be_paid == null ? 0 : employee_Salary_Expense.salary_to_be_paid;
            using (EasyBillingEntities db = new EasyBillingEntities())
            {

                if (ModelState.IsValid)
                {
                    if (!string.IsNullOrEmpty(employee_Salary_Expense.Employee_token_number))
                    {
                        employee_Salary_Expense.Token_number = Guid.NewGuid().ToString();
                         var pmexits = db.Employee_salary_expenses.Any();
                        string text = null;
                        String totalvrsn = null;
                        var userdet = db.Employees.OrderByDescending(z => z.Date).Where(z => z.Token_number ==employee_Salary_Expense.Employee_token_number).Select(z => new { z.Employee_name, z.Employee_Id, z.Salary, z.Date }).FirstOrDefault();

                        var userdetforuserifname = db.Employees.Where(z => z.Employee_Id == User.Identity.Name).Select(z => new { z.Employee_name, z.Employee_Id }).FirstOrDefault();

                        if (pmexits == true)
                        {
                            text = (from a in db.Employee_salary_expenses
                                    orderby a.ExpenseId descending
                                    select a.ExpenseId).FirstOrDefault();

                            var fstfr = text.Substring(0, 4);
                            var lstfr = text.Substring(text.Length - 7);
                            string newlstversn = (int.Parse(lstfr) + 10000001).ToString();
                            string fstfr1 = (newlstversn.Substring(newlstversn.Length - 7)).ToString();
                            totalvrsn = fstfr + fstfr1;
                            employee_Salary_Expense.Monthly_salary = db.Employee_salary_expenses.OrderByDescending(z => z.Date).Where(z => z.Employee_token_number == employee_Salary_Expense.Employee_token_number).Select(x => x.Monthly_salary).FirstOrDefault();

                            employee_Salary_Expense.Salary_updated_date = userdet.Date;
                            employee_Salary_Expense.Updated_salary = userdet.Salary;
                            if (employee_Salary_Expense.Monthly_salary != userdet.Salary)
                                employee_Salary_Expense.Monthly_salary = userdet.Salary;
                        }
                        else
                        {
                            totalvrsn = "ESAL0000001";
                            employee_Salary_Expense.Monthly_salary = userdet.Salary;
                            employee_Salary_Expense.Salary_updated_date = userdet.Date;
                            employee_Salary_Expense.Updated_salary = userdet.Salary;
                        }
                        employee_Salary_Expense.ExpenseId = totalvrsn;
                        //employee_Salary_Expense.Date = DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                        employee_Salary_Expense.Date =DateTime.Now;
                        employee_Salary_Expense.Mac_id = macAdd();
                         if(userdet==null)
                        {
                            var merchantdet = db.Marchent_Accounts.Where(z => z.Email_Id == User.Identity.Name).Select(z => new { z.Marchent_name, z.Email_Id }).FirstOrDefault();
                            employee_Salary_Expense.User_Id = merchantdet.Email_Id;
                            employee_Salary_Expense.User_name = merchantdet.Marchent_name;
                        }
                       else
                        {
                            employee_Salary_Expense.User_Id = userdetforuserifname.Employee_Id;
                            employee_Salary_Expense.User_name = userdetforuserifname.Employee_name;
                        }
                        employee_Salary_Expense.Month = DateTime.Now.Month;
                        employee_Salary_Expense.Year = DateTime.Now.Year;
                        employee_Salary_Expense.Salary_paid_date = DateTime.Now;
                    
                        employee_Salary_Expense.Employee_name = db.Employees.Where(z=>z.Token_number== employee_Salary_Expense.Employee_token_number).Select(z=>z.Employee_name).FirstOrDefault();
                        db.Employee_salary_expenses.Add(employee_Salary_Expense);
                        await db.SaveChangesAsync();
                        return Created("", employee_Salary_Expense);
                    }
                    else
                    {
                        return BadRequest("At least one employee must be selected.");
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
        [HttpGet]
        [Route("{id}")]
        public async Task<IHttpActionResult> GetById([FromUri] string id)
        {
            using (EasyBillingEntities db = new EasyBillingEntities())
            {
                return await Task.FromResult(Ok(db.Item_Tubes.Where(z => z.Token_number == id).Distinct().FirstOrDefault()));
            }
        }

      
    }
}
