using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace EasyBilling.Models
{
    public class BillingMasterClass
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BillingMasterClass()
        {
            this.Billing_Details = new HashSet<BillingDetailsClass>();
            this.Stockouts = new HashSet<StockoutClass>();
            this.Barcode_Master = new HashSet<BarcodeMasterClass>();
        }

        public string Token_Number { get; set; }
        public string Billing_Number { get; set; }
        public System.DateTime Date { get; set; }
        public string Marchent_Token_number { get; set; }
        public string Customer_Token_number { get; set; }
        public string Tax_token { get; set; }
        public decimal Total_tax { get; set; }
        public decimal Rate_including_tax { get; set; }
        public decimal Discount_percent { get; set; }
        public decimal Total_discount { get; set; }
        public decimal Total_amount { get; set; }
        public decimal CGST { get; set; }
        public decimal SGST { get; set; }
        public decimal IGST { get; set; }
        public decimal UTGST { get; set; }
        public string Narration { get; set; }

        [DisplayName("Tax group")]
        public string txgrp { get; set; }

        [DisplayName("Dealer")]
        public string dlr { get; set; }


        public string Barcode_Number { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BillingDetailsClass> Billing_Details { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Tax_Group Tax_Group { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StockoutClass> Stockouts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BarcodeMasterClass> Barcode_Master { get; set; }
        
    }
}