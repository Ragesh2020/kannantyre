using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EasyBilling.Models
{
    public class PendingShipping
    {
        [Key]
        public string token { get; set; }
        public string tyreName { get; set; }
        public string Customer_name { get; set; }
        public int Total_Piece { get; set; }
        public decimal Total_Price { get; set; }

        public string paymentstatement { get; set; }

        [DisplayName("Applied date")]
        public DateTime Applied_Date { get; set; }
        public bool Isuser { get; set; }
    }
}