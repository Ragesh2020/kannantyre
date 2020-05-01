using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EasyBilling.Models
{
    public class VehicleClass
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public VehicleClass()
        {
            this.Item_Tyre = new HashSet<Item_Tyre>();
           
        }
       
        public string Token_number { get; set; }
        
        public string Vehicle_type { get; set; }
     
        public string Vehicle_make { get; set; }
        public System.DateTime Date { get; set; }
        public string Mac_id { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Item_Tyre> Item_Tyre { get; set; }
    
    }
}