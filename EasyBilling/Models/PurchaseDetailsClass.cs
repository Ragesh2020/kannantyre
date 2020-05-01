using System;

namespace EasyBilling.Models
{
    public class PurchaseDetailsClass
    {
        public int Purchase_details_number { get; set; }
        public string Purchase_Token_number { get; set; }
        public string Purcahse_number { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string Product_Token { get; set; }
        public Nullable<int> Pieces { get; set; }
        public Nullable<decimal> Quantity { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<decimal> Taxable_amount { get; set; }
        public string Tax_Token { get; set; }
        public Nullable<decimal> Sub_Total { get; set; }
        public Nullable<decimal> Tax { get; set; }
        public Nullable<decimal> Discount_percent { get; set; }
        public Nullable<decimal> Discount { get; set; }
        public string Product_name { get; set; }
        public virtual Purchase_Master Purchase_Master { get; set; }
    }
}