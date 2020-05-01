using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace EasyBilling.Models
{
    public class CustomerClass
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CustomerClass()
        {
            this.Billing_Master = new HashSet<BillingMasterClass>();
        }

        public string Token_number { get; set; }
        public string Customer_Name { get; set; }
        public string Email { get; set; }
        public string Phone_number { get; set; }
        public string Alternate_phone_number { get; set; }
        public Nullable<int> State { get; set; }
        public string Address { get; set; }
        public string Second_Address { get; set; }
        public string Pan_number { get; set; }
        public string Credit_Card_number { get; set; }
        public double Credit_limit { get; set; }
        public Nullable<bool> IsActive { get; set; }
        [DisplayName("State name")]
        public string stcd { get; set; }
        public StateData Statecd { get; set; }
        public string Marchent_Token_number { get; set; }

        public string Vehicle_type { get; set; }
        public string Password { get; set; }
        public System.DateTime Applied_date { get; set; }
        public System.DateTime Approve_date { get; set; }
        public bool New_customer { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BillingMasterClass> Billing_Master { get; set; }
    }
}