using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace EasyBilling.Models
{
    public class ProductClass
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ProductClass()
        {
            this.Item_Tube = new HashSet<Item_Tube>();
            this.Item_Tyre = new HashSet<Item_Tyre>();
  
        }

        public string Token_Number { get; set; }
        public string Product_Code { get; set; }
        public string Product_name { get; set; }
        public System.DateTime Date { get; set; }
        public string Mac_id { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Item_Tube> Item_Tube { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Item_Tyre> Item_Tyre { get; set; }
   
    }
}