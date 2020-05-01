using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EasyBilling.Models
{
    public class TempbillClass
    {
        public string Token_Number { get; set; }
        public string Product_Token { get; set; }
        public string Product_name { get; set; }
        public string Tyre_token { get; set; }
        public string Tyre_Size { get; set; }
        public string Supplier_token { get; set; }
        public string Supplier_name { get; set; }
        public System.DateTime Date { get; set; }
        public decimal Purchase_Price { get; set; }
        public decimal CGST { get; set; }
        public decimal SGST { get; set; }
        public int Pieces { get; set; }
        public decimal Selling_Price { get; set; }
        public decimal Amout_after_tax { get; set; }
        public decimal Total { get; set; }
        public System.DateTime Approve_date { get; set; }
        public bool Approve { get; set; }
        public string Administrator_Token_number { get; set; }
        public string Administrator_name { get; set; }
        public long Delivery_contact_number { get; set; }
        public string Delivery_address { get; set; }
        public string Item_tyre_token { get; set; }
        public string Item_tyre_Id { get; set; }
        public string Purchase_number { get; set; }
        public string Vehicle_Token { get; set; }
        public string Vehicle_type { get; set; }
        public string Description { get; set; }
        public string Tyre_make { get; set; }
        public string Tyre_feel { get; set; }
        public string Tyre_type { get; set; }
        public string Mac_id { get; set; }
        public Nullable<bool> requestsend { get; set; }
        public decimal Selling_CGST { get; set; }
        public decimal Selling_SGST { get; set; }
        public Nullable<bool> CGST_SGST_CHECK { get; set; }
        public string User_Id { get; set; }
        public string User_name { get; set; }
        public string Approve_user_id { get; set; }
        public string Approve_user_name { get; set; }
        public string Rate_update_user_id { get; set; }
        public string Rate_update_user_name { get; set; }
        public int itemscount { get; set; }
        public string Tyre_number { get; set; }
    }
}