using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EasyBilling.Models
{
    public class productExpensesClass
    {
        public string Token_number { get; set; }
        public decimal Amount_for_expense { get; set; }
        public string Product_token_number { get; set; }
        public string Product_name { get; set; }
    }
}