using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EasyBilling.Models
{
    public class Employee_salary_expenseClass
    {
        public string Token_number { get; set; }
        public decimal Advance_collected { get; set; }
        public decimal salary_to_be_paid { get; set; }
        public string Employee_token_number { get; set; }
        public string Employee_name { get; set; }

        public System.DateTime Date { get; set; }
        public string ExpenseId { get; set; }
        public string Mac_id { get; set; }
        public string User_Id { get; set; }
        public string User_name { get; set; }
        public decimal Monthly_salary { get; set; }
        public decimal Salary_paid { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public string monthYear { get; set; }
        public System.DateTime Salary_updated_date { get; set; }
        public decimal Updated_salary { get; set; }
        public System.DateTime Salary_paid_date { get; set; }
    }
}