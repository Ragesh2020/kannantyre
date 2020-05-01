using System.Collections.Generic;

namespace EasyBilling.Models
{
    public class PurchaseAll
    {
        public Purchase_Master Purchasemast { get; set; }
        public List<Purchase_detail> PurchaseDet { get; set; }
        public List<Stock> stck { get; set; }

    }
}