using System;

namespace EasyBilling.Models
{
    public class TaxGroupClass
    {
        public string Tax_Token { get; set; }
        public string Tax_Name { get; set; }
        public Nullable<double> Tax_Rate { get; set; }
        public string GL_CODE { get; set; }
        public Nullable<bool> IsActive { get; set; }
    }
}