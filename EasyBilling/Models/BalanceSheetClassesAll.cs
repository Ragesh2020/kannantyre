using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EasyBilling.Models
{
    public class BalanceSheetClassesAll
    {
        public DateTime Date { get; set; }
        public List<BalanceSheetClass> BalanceSheetClassDateWise { get; set; }
        public string CarryForwardDate { get; set; }
        public decimal CarryForward { get; set; }
        public decimal TotalBalance { get; set; }
    }
}