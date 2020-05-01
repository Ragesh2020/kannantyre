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
    
    public partial class Purchase_Master
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Purchase_Master()
        {
            this.Purchase_details = new HashSet<Purchase_detail>();
            this.Stocks = new HashSet<Stock>();
        }
    
        public string Token_Number { get; set; }
        public string Purchase_Number { get; set; }
        public System.DateTime Date { get; set; }
        public string Marchent_Token_number { get; set; }
        public string Dealer_Token_number { get; set; }
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
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Purchase_detail> Purchase_details { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Stock> Stocks { get; set; }
    }
}
