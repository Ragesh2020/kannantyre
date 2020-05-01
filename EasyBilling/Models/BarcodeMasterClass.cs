using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EasyBilling.Models
{
    public class BarcodeMasterClass
    {
        public string Barcode_Number { get; set; }
        public string Billing_Number { get; set; }
        public string Billing_Token_number { get; set; }
        public Nullable<System.DateTime> Date { get; set; }

        public virtual Billing_Master Billing_Master { get; set; }
    }
}