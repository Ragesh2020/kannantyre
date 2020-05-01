using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace EasyBilling.Models
{
    public class PurchaseMasterclass
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PurchaseMasterclass()
        {
            this.Purchase_details = new HashSet<PurchaseDetailsClass>();
            this.Stocks = new HashSet<StockClass>();
        }

        public string Token_Number { get; set; }
        public string Purchase_Number { get; set; }
        public System.DateTime Date { get; set; }
        public string Marchent_Token_number { get; set; }
        public string Dealer_Token_number { get; set; }
        public string Tax_token { get; set; }
        public decimal Total_tax { get; set; }
        public decimal Discount_percent { get; set; }
        public decimal Total_discount { get; set; }
        public decimal Total_amount { get; set; }
        public decimal CGST { get; set; }
        public decimal SGST { get; set; }
        public decimal IGST { get; set; }
        public string Narration { get; set; }
        public decimal Rate_including_tax { get; set; }
        public decimal UTGST { get; set; }
        [DisplayName("Tax group")]
        public string txgrp { get; set; }

        [DisplayName("Dealer")]
        public string dlr { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PurchaseDetailsClass> Purchase_details { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StockClass> Stocks { get; set; }

    }
}