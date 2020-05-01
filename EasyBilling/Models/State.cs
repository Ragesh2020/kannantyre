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
    
    public partial class State
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public State()
        {
            this.Dealers = new HashSet<Dealer>();
            this.Marchent_Account = new HashSet<Marchent_Account>();
        }
    
        public int State_Code { get; set; }
        public string Name { get; set; }
        public string State_Identity { get; set; }
        public bool CGST { get; set; }
        public bool SGST { get; set; }
        public bool UTGST { get; set; }
        public bool IGST { get; set; }
        public string Marchent_Token_number { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Dealer> Dealers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Marchent_Account> Marchent_Account { get; set; }
    }
}
