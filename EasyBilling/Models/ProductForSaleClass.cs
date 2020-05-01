using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EasyBilling.Models
{
    public class ProductForSaleClass
    {
        public string Token_Number { get; set; }

        public string Product_Token { get; set; }

        public string Product_name { get; set; }

        public int Pieces { get; set; }

        public decimal Amout_after_tax { get; set; }
        public decimal Total { get; set; }

        public string Tyre_Size { get; set; }

        public string Supplier_token { get; set; }

        public string Supplier_name { get; set; }

        public System.DateTime Date { get; set; }

        public decimal Purchase_Price { get; set; }

        public decimal CGST { get; set; }

        public decimal SGST { get; set; }

        public string Administrator_Token_number { get; set; }

        public string Administrator_name { get; set; }

        public System.DateTime Approve_date { get; set; }
        public bool Approve { get; set; }
 
        public decimal Selling_Price { get; set; }
        public int StockIn { get; set; }

        public decimal Up_Selling_Price { get; set; }

        public decimal Up_CGST { get; set; }

        public decimal Up_SGST { get; set; }

        public long Delivery_contact_number { get; set; }
    
        public string Delivery_address { get; set; }
       
        public string Item_tyre_Id { get; set; }
        
        public string Purchase_number { get; set; }
       
        public System.DateTime PurchaseDate { get; set; }
       
        public string Tyre_make { get; set; }
   

        public string Tyre_type { get; set; }
       

        public string Tyre_feel { get; set; }
        public string Vehicle_Token { get; set; }
   
        public string Vehicle_type { get; set; }
        public string Description { get; set; }
        public string Tyre_token { get; set; }
        public string Item_tyre_token { get; set; }

        public string Mac_id { get; set; }

        public Nullable<bool> requestsend { get; set; }
        public string rateperunit { get; set; }
        public bool Totalamount { get; set; }
        public string percent { get; set; }
        public bool amnt { get; set; }

        public decimal Selling_CGST { get; set; }
        public decimal Selling_SGST { get; set; }
        public Nullable<bool> CGST_SGST_CHECK { get; set; }


        public string User_Id { get; set; }
        public string User_name { get; set; }
        public string Approve_user_id { get; set; }
        public string Approve_user_name { get; set; }
        public string Rate_update_user_id { get; set; }
        public string Rate_update_user_name { get; set; }
        public Nullable<bool> IsGstPercent { get; set; }
        public virtual Dealer Dealer { get; set; }
   
        public decimal Total_discount { get; set; }
        public decimal Total_amount { get; set; }
        public decimal Amount_paid { get; set; }
        public decimal Balance { get; set; }
        public string Mode_of_payment { get; set; }
        public string Narration { get; set; }
        public string CustToken_number { get; set; }
        public string Customer_Name { get; set; }
        public string Email { get; set; }
        public string Phone_number { get; set; }
        public string Address { get; set; }
        public string Vehicle_number { get; set; }
        public string Billno { get; set; }
        public string selectionForExecution { get; set; }
        public Nullable<decimal> Selling_net_total { get; set; }
        public System.DateTime SDate { get; set; }
        public System.DateTime EDate { get; set; }
        public Nullable<bool> CalculationByRatePerUnit { get; set; }

    }
}