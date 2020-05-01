//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EasyBilling.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Billing_Master
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Billing_Master()
        {
            this.Barcode_Master = new HashSet<Barcode_Master>();
            this.Billing_Details = new HashSet<Billing_Detail>();
            this.Stockouts = new HashSet<Stockout>();
        }
    
        public string Token_Number { get; set; }
        public string Billing_Number { get; set; }
        public System.DateTime Date { get; set; }
        public decimal Total_tax { get; set; }
        public decimal Rate_including_tax { get; set; }
        public decimal Total_discount { get; set; }
        public decimal Total_amount { get; set; }
        public bool Discountper { get; set; }
        public string Narration { get; set; }
        public string Mac_id { get; set; }
        public string User_Id { get; set; }
        public string User_name { get; set; }
        public decimal Amount_paid { get; set; }
        public decimal Balance { get; set; }
        public string Mode_of_payment { get; set; }
        public string Transaction_Id { get; set; }
        public string Customer_token_number { get; set; }
        public Nullable<decimal> Discount { get; set; }
        public Nullable<bool> IsGstPercent { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Barcode_Master> Barcode_Master { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Billing_Detail> Billing_Details { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Stockout> Stockouts { get; set; }
    }
}
