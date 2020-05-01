using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace EasyBilling.Models
{
    public class DealerAccount
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DealerAccount()
        {
            this.Products_For_Sale = new HashSet<Products_For_Sale>();
        }

        public string Token_number { get; set; }
       
        public string Dealer_code { get; set; }
       
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

        public System.DateTime Date { get; set; }
        public string Mac_id { get; set; }
        public string Phone_number { get; set; }
        public Nullable<int> State { get; set; }
      
        public string State_Name { get; set; }
     
        public bool Isactive { get; set; }
        public string Marchent_Token_number { get; set; }
        public string Office_number { get; set; }
      
        public string GST_number { get; set; }
      
        public string Pan_number { get; set; }

        public virtual State State1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Products_For_Sale> Products_For_Sale { get; set; }
    }
}