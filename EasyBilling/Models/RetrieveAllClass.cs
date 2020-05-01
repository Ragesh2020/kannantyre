using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EasyBilling.Models
{
    public class RetrieveAllClass
    {
        public string Tokennumber { get; set; }
        public string billtype { get; set; }
        public string orderNumber { get; set; }
        public string orderDate { get; set; }
        public string VehicleNumber { get; set; }
        public string customerName { get; set; }
        public string Amount { get; set; }
        public string AmountOnQuotation { get; set; }
        public string Balance { get; set; }
        public string Status { get; set; }
    }
}