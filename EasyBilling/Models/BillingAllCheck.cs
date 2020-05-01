using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EasyBilling.Models
{
    public class BillingAllCheck
    {
        public Purchase_Master Purchasemast { get; set; }
        public List<Stock> stck { get; set; }
    }
}