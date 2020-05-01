using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EasyBilling.Models
{
    public class StockoutClass
    {
        public int Stock_out_Id { get; set; }
        public string Billing_Token_number { get; set; }
        public string Billing_number { get; set; }
        public System.DateTime Date { get; set; }
        public string Product_Token { get; set; }
        public int Pieces { get; set; }
        public decimal Quantity { get; set; }
        public string Tax_Token { get; set; }
        public decimal Tax { get; set; }
        public decimal Sub_Total { get; set; }

        public virtual Billing_Master Billing_Master { get; set; }
   

    }
}