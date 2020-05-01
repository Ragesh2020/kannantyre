using System;
using System.ComponentModel;

namespace EasyBilling.Models
{
    public class MarchentAccount
    {
        public string Token_number { get; set; }
        public string Marchent_name { get; set; }
        public string Email_Id { get; set; }
        public string Address { get; set; }
        public string Mobile { get; set; }
        public string Telephone_No { get; set; }
        public string GSTIN_Number { get; set; }
        public string CIN_Number { get; set; }
        public string UIN_Number { get; set; }
        public Nullable<int> State_Code { get; set; }
        public string Pan_Number { get; set; }
        public bool IsActive { get; set; }
        public string Verification_code { get; set; }
        public string State_Name { get; set; }
        [DisplayName("State name")]
        public string stcd { get; set; }

        public virtual StateData State { get; set; }
    }
}