using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EasyBilling.Models
{
    public class EmployeeClass
    {
        public string Token_number { get; set; }

        public string Employee_Id { get; set; }

        public Nullable<System.DateTime> Leaving_date { get; set; }


        public string Image_path { get; set; }

        public string Employee_name { get; set; }
        public string Designation { get; set; }

        public System.DateTime Joining_date { get; set; }

        public string Contact_number { get; set; }

        public string Email_id { get; set; }
        public decimal Salary { get; set; }



        public bool login_required { get; set; }
        public System.DateTime Date { get; set; }
        public string Mac_id { get; set; }
    }
}