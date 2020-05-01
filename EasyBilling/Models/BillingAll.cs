using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EasyBilling.Models
{
    public class BillingAll
    {
        public Billing_Master Billingmast { get; set; }

        public List<Billing_Detail> BillingDet { get; set; }
        public List<Stock> stck { get; set; }
    }
}