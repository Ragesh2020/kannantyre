using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EasyBilling.Models
{
    public class OtherExpensesClass
    {
        public string Token_number { get; set; }
        public string Other_expense_type { get; set; }
        public decimal Other_expense_amount { get; set; }
    }
}