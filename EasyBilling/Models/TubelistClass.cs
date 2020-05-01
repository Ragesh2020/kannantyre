using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EasyBilling.Models
{
    public class TubelistClass
    {
     
        public string Token_number { get; set; }
       
        public string Item_Id { get; set; }

        public int Pieces { get; set; }
        public string Tube_size { get; set; }
        public string Tyre_feel { get; set; }
        public string Tyre_make { get; set; }
        public string Company_name { get; set; }
      
        public string Vehicle_type { get; set; }
        public string Company_token { get; set; }

        public string Description { get; set; }
        public string Size_token { get; set; }
        public System.DateTime Date { get; set; }
        public string Mac_id { get; set; }
        public virtual Product Product { get; set; }


        public virtual Tyre_size Tyre_size { get; set; }
    }
}