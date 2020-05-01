using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EasyBilling.Models
{
    public class TyreSizes
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TyreSizes()
        {
            this.Item_Tyre = new HashSet<Item_Tyre>();
           
            this.Item_Tube = new HashSet<Item_Tube>();
        }

     
        public string Token_number { get; set; }

        public System.DateTime Date { get; set; }
        public string Mac_id { get; set; }
        public string Tyre_size1 { get; set; }
        
        public bool With_tube { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Item_Tyre> Item_Tyre { get; set; }
      
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Item_Tube> Item_Tube { get; set; }
    }
}