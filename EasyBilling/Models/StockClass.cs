using System;

namespace EasyBilling.Models
{
    public class StockClass
    {
        public int Stock_Id { get; set; }
        public string Purchase_Token_number { get; set; }
        public string Purcahse_number { get; set; }
        public System.DateTime Date { get; set; }
        public string Product_Token { get; set; }
        public int Pieces { get; set; }
        public string Product_name { get; set; }
        public string Marchent_Token_number { get; set; }
        public System.DateTime Remodify_Date { get; set; }
        public int Remodify_pcs { get; set; }
        public decimal CGST { get; set; }
        public decimal SGST { get; set; }
        public virtual Purchase_Master Purchase_Master { get; set; }
    }
}