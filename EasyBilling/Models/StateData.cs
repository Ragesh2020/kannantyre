using System;
using System.Collections.Generic;

namespace EasyBilling.Models
{
    public class StateData
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public StateData()
        {
            this.Dealers = new HashSet<DealerAccount>();
            this.Marchent_Account = new HashSet<MarchentAccount>();
            this.Customers = new HashSet<CustomerClass>();
        }
        public int State_Code { get; set; }
        public string Name { get; set; }
        public string State_Identity { get; set; }
        public Nullable<bool> CGST { get; set; }
        public Nullable<bool> SGST { get; set; }
        public Nullable<bool> UTGST { get; set; }
        public Nullable<bool> IGST { get; set; }
        public string State1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DealerAccount> Dealers { get; set; }
  
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CustomerClass> Customers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<MarchentAccount> Marchent_Account { get; set; }

    }
}