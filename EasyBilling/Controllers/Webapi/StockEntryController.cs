using EasyBilling.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Web.Http;

namespace EasyBilling.Controllers.Webapi
{
    [RoutePrefix("api/stocksitms")]
    public class StockEntryController : ApiController
    {
        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> GetAll()
        {
            IList<TempStockClass> productForSaleClasses = null;

            using (var ctx = new EasyBillingEntities())
            {

                productForSaleClasses = ctx.Temp_Stock.Select(s => new TempStockClass()
                {
                    Token_Number = s.Token_Number,
                    Date = s.Date,
                    Tyre_make = s.Tyre_make,
                    Product_name = s.Product_name,
                    Tyre_Size = s.Tyre_Size,
                    Supplier_name = s.Supplier_name,
                    Pieces = s.Pieces,
                    Tyre_type = s.Tyre_type,
                    Approve = s.Approve,
                    Item_tyre_Id = s.Item_tyre_Id,
                    requestsend = s.requestsend,
                    Purchase_number = s.Purchase_number,
                    SGST = s.SGST,
                    CGST = s.CGST,
                    Total = s.Total,
                    Purchase_Price=s.Purchase_Price

                }).OrderByDescending(x => x.Date).Distinct().ToList<TempStockClass>();

            }
            return Ok(productForSaleClasses);

        }
        [HttpGet]
        [Route("GetAllBill")]
        public async Task<IHttpActionResult> GetAllBill()
        {
            IList<TempbillClass> productForSaleClasses = null;

            using (var ctx = new EasyBillingEntities())
            {

                productForSaleClasses = ctx.Temp_Bill.Select(s => new TempbillClass()
                {
                    Token_Number = s.Token_Number,
                    Date = s.Date,
                    Product_name=s.Product_name,
                    Pieces=s.Pieces,
                    Selling_SGST=s.Selling_SGST,
                    Selling_CGST=s.Selling_CGST,
                    Item_tyre_Id = s.Item_tyre_Id,
                   
                    Selling_Price = s.Selling_Price,
                    
                    Total = s.Total,
                    Tyre_number=s.Tyre_number

                }).OrderByDescending(x => x.Date).Distinct().ToList<TempbillClass>();

            }
            return Ok(productForSaleClasses);

        }
        [HttpGet]
        [Route("GetAllNotApproved")]
        public async Task<IHttpActionResult> GetAllNotApproved()
        {
            IList<ProductForSaleClass> productForSaleClasses = null;

            using (var ctx = new EasyBillingEntities())
            {

                productForSaleClasses = ctx.Products_For_Sales.Select(s => new ProductForSaleClass()
                {
                    Token_Number = s.Token_Number,
                    Date = s.Date,
                    Tyre_make = s.Tyre_make,
                    Product_name = s.Product_name,
                    Tyre_Size = s.Tyre_Size,
                    Supplier_name = s.Supplier_name,
                    Pieces = s.Pieces,
                    Tyre_type = s.Tyre_type,
                    Approve = s.Approve,
                    Item_tyre_Id = s.Item_tyre_Id,
                    requestsend = s.requestsend,
                    Purchase_number = s.Purchase_number,
                    SGST = s.SGST,
                    CGST = s.CGST,
                    Total = s.Total


                }).Where(x =>( x.Approve==false && x.requestsend == false) || (x.Approve == false && x.requestsend == null)).Distinct().ToList<ProductForSaleClass>();

            }
            return Ok(productForSaleClasses);

        }
        [HttpGet]
        [Route("GetitemdetailsByids")]
        public IHttpActionResult GetitemdetailsByids(string id)
        {
            List<ProductForSaleClass> products_For_Sale = new List<ProductForSaleClass>();
            ProductForSaleClass products_For_Salesingle = new ProductForSaleClass();
            using (var ctx = new EasyBillingEntities())
            {
                products_For_Salesingle = (from it in ctx.Products_For_Sales

                                           select new ProductForSaleClass()
                                           {
                                               Item_tyre_Id = it.Item_tyre_Id,
                                               Approve_date = it.Approve_date,
                                               Product_name = it.Product_name,
                                               Pieces = it.Pieces,
                                               Selling_Price = it.Selling_Price,
                                               Selling_CGST = it.Selling_CGST,
                                               Selling_SGST = it.Selling_SGST,
                                               Selling_net_total=it.Selling_net_total
                                               //Total = (it.Selling_Price + (it.Selling_Price * (it.Selling_CGST + it.Selling_SGST) / 100))
                                           }).Where(z => z.Item_tyre_Id == id && z.Pieces> 0).OrderByDescending(z => z.Approve_date).FirstOrDefault();

                
                List<string> s = new List<string>(id.Split(new string[] { "-" }, StringSplitOptions.None));
                if (s[0].ToLower() == "po")
                {
                   
                    bool chkpcs = (from ord in ctx.Products_For_Sales
                                   where ord.Item_tyre_Id == id
                                   select ord.Pieces).Any();
                    if (!chkpcs)
                    {
                        products_For_Sale = new List<ProductForSaleClass>();
                        products_For_Salesingle = (from it in ctx.Other_Products
                                                   select new ProductForSaleClass()
                                                   {
                                                       Item_tyre_Id = it.Product_id,
                                                     
                                                       Product_name = it.Product_name+ " "+it.Product_type+" type",
                                                       Pieces = 0,
                                                       Selling_Price = 0,
                                                       Selling_CGST = 0,
                                                       Selling_SGST = 0,
                                                       Total = 0,
                                                       Selling_net_total=0
                                                   }).Where(z => z.Item_tyre_Id == id).FirstOrDefault();

                    }
                }
                    products_For_Sale.Add(products_For_Salesingle);
                    return Ok(products_For_Sale);
            
                //var first2 = id.Substring(id.Length - id.Length, 2);
                //if (!string.IsNullOrEmpty(first2))
                //{
                //    if (first2.ToLower() == "ty".ToLower())
                //    {
                //        products_For_Salesingle = (from it in ctx.Products_For_Sales
                //                          orderby it.Approve_date descending
                //                                   select new ProductForSaleClass()
                //                                 {
                //                                     Item_tyre_Id = it.Item_tyre_Id,
                //                                    Approve_date=it.Approve_date,
                //                                     Product_name=it.Product_name,
                //                                     Pieces=it.Pieces,
                //                                     Selling_Price=it.Selling_Price,
                //                                     Selling_CGST=it.Selling_CGST,
                //                                     Selling_SGST=it.Selling_SGST,
                //                                     Total=it.Total
                //                                 }).Where(z => z.Item_tyre_Id == id).OrderByDescending(z => z.Approve_date).Distinct().ToList().LastOrDefault();

                //       products_For_Sale.Add(products_For_Salesingle);
                       

                //        return Ok(products_For_Sale);
                //    }

                //    else if (first2.ToLower() == "tu".ToLower())
                //    {
                //        products_For_Salesingle = (from it in ctx.Products_For_Sales
                //                                   orderby it.Approve_date descending
                //                                   select new ProductForSaleClass()
                //                                   {
                //                                       Item_tyre_Id = it.Item_tyre_Id,
                //                                       Approve_date = it.Approve_date,
                //                                       Product_name = it.Product_name,
                //                                       Pieces = it.Pieces,
                //                                       Selling_Price = it.Selling_Price,
                //                                       Selling_CGST = it.Selling_CGST,
                //                                       Selling_SGST = it.Selling_SGST,
                //                                       Total = it.Total
                //                                   }).Where(z => z.Item_tyre_Id == id).OrderByDescending(z => z.Approve_date).Distinct().ToList().LastOrDefault();

                //        products_For_Sale.Add(products_For_Salesingle);

                //        return Ok(products_For_Sale);

                //    }

                //    else if (first2.ToLower() == "po".ToLower())
                //    {
                //        bool chkpcs = (from ord in ctx.Products_For_Sales
                //                       where ord.Item_tyre_Id == id
                //                       select ord.Pieces).Any();
                //        if (chkpcs)
                //        {
                //            otherProductClasses = (from it in ctx.Other_Products
                //                                       //join stk in ctx.Products_For_Sales on it.Product_id equals stk.Item_tyre_Id

                //                                   select new OtherProductClass()
                //                                   {

                //                                       Product_id = it.Product_id,
                //                                       Product_name = it.Product_name,
                //                                       Product_type = it.Product_type,

                //                                       Pieces = (from ord in ctx.Products_For_Sales
                //                                                 where ord.Item_tyre_Id == it.Product_id
                //                                                 select ord.Pieces).Sum(),

                //                                       Date = it.Date
                //                                   }).Where(z => z.Product_id == id).OrderByDescending(x => x.Date).Distinct().ToList();
                //        }
                //        else
                //        {
                //            otherProductClasses = (from it in ctx.Other_Products
                //                                       //join stk in ctx.Products_For_Sales on it.Product_id equals stk.Item_tyre_Id

                //                                   select new OtherProductClass()
                //                                   {

                //                                       Product_id = it.Product_id,
                //                                       Product_name = it.Product_name,
                //                                       Product_type = it.Product_type,

                //                                       Pieces = 0,

                //                                       Date = it.Date
                //                                   }).Where(z => z.Product_id == id).OrderByDescending(x => x.Date).Distinct().ToList();
                //        }
                //        return Ok(otherProductClasses);
                //    }
                //    else
                //    { return BadRequest("Id does not matched"); }
                //}
                //else
                //{
                //    return BadRequest("Id should not be empty");
               // }
            }
            //   

        }
        [HttpGet]
        [Route("GetitemsdetailsByComp")]
        public async Task<IHttpActionResult> GetitemsdetailsByComp(string id)
        {
            List<ProductForSaleClass> products_For_Sale = new List<ProductForSaleClass>();
            ProductForSaleClass products_For_Salesingle = new ProductForSaleClass();
            using (var ctx = new EasyBillingEntities())
            {

                products_For_Sale = (from it in ctx.Products_For_Sales
                                        
                                           select new ProductForSaleClass()
                                           {
                                               Item_tyre_Id = it.Item_tyre_Id,
                                               Approve_date = it.Approve_date,
                                               Product_name = it.Product_name,
                                               Pieces = it.Pieces,
                                               Selling_Price = it.Selling_Price,
                                               Selling_CGST = it.Selling_CGST,
                                               Selling_SGST = it.Selling_SGST,
                                               Selling_net_total=it.Selling_net_total
                                              // Total = (it.Selling_Price + (it.Selling_Price * (it.Selling_CGST + it.Selling_SGST) / 100)) 

                                           }).Where(z => z.Product_name == id && z.Pieces > 0).OrderByDescending(z => z.Approve_date).ToList();

               
                    
                    bool chkpcs = (from ord in ctx.Products_For_Sales
                                   where ord.Item_tyre_Id == id
                                   select ord.Pieces).Any();
                    if (!chkpcs)
                    {
                    products_For_Sale = new List<ProductForSaleClass>();
                    products_For_Sale = (from it in ctx.Other_Products
                                                   select new ProductForSaleClass()
                                                   {
                                                       Item_tyre_Id = it.Product_id,
                                                      Tyre_make= it.Product_type,
                                                       Product_name = it.Product_name + " " + it.Product_type + " type",
                                                       Pieces = 0,
                                                       Selling_Price = 0,
                                                       Selling_CGST = 0,
                                                       Selling_SGST = 0,
                                                       Total = 0,
                                                       Selling_net_total=0
                                                   }).Where(z => z.Product_name == id + " " + z.Tyre_make + " type").ToList();

                    }
              
               
                return Ok(products_For_Sale);
            }
        }

        [HttpGet]
        [Route("Getitemsdetails")]
        public async Task<IHttpActionResult> Getitemsdetails(string id)
        {
            IList<TyrelistClass> productForSaleClasses = null;
            IList<TubelistClass> tubelistClasses = null;
            IList<OtherProductClass> otherProductClasses = null;

            using (var ctx = new EasyBillingEntities())
            {
                var first2 = id.Substring(id.Length - id.Length, 2);
                if (!string.IsNullOrEmpty(first2))
                {
                    if (first2.ToLower() == "ty".ToLower())
                    {
                       bool chkpcs= (from ord in ctx.Products_For_Sales
                         where ord.Item_tyre_Id == id
                         select ord.Pieces).Any();
                        if (chkpcs)
                        {
                            productForSaleClasses = (from it in ctx.Item_Tyres
                                                     select new TyrelistClass()
                                                     {

                                                         Item_Id = it.Item_Id,
                                                         Company_name = it.Company_name,
                                                         Tyre_size = it.Tyre_size,
                                                         Tyre_feel = it.Tyre_feel,
                                                         Tyre_make = it.Tyre_make,
                                                         Vehicle_type = it.Vehicle_type,
                                                         
                                                         Pieces = (from ord in ctx.Products_For_Sales
                                                                   where ord.Item_tyre_Id == id
                                                                   select ord.Pieces).Sum(),
                                                         Date = it.Date
                                                     }).Where(z => z.Item_Id == id).OrderByDescending(x => x.Date).Distinct().ToList();
                        }
                        else
                        {
                            productForSaleClasses = (from it in ctx.Item_Tyres
                                                     select new TyrelistClass()
                                                     {
                                                         Item_Id = it.Item_Id,
                                                         Company_name = it.Company_name,
                                                         Tyre_size = it.Tyre_size,
                                                         Tyre_feel = it.Tyre_feel,
                                                         Tyre_make = it.Tyre_make,
                                                         Vehicle_type = it.Vehicle_type,
                                                         Pieces = 0,
                                                         Date = it.Date
                                                     }).Where(z => z.Item_Id == id).OrderByDescending(x => x.Date).Distinct().ToList();
                        }



                        //ctx.Item_Tyres.Select(s => new TyrelistClass()
                        //{
                        //    Item_Id = s.Item_Id,
                        //    Company_name = s.Company_name,
                        //    Tyre_size = s.Tyre_size,
                        //    Tyre_feel = s.Tyre_feel,
                        //    Tyre_make = s.Tyre_make,
                        //    Vehicle_type = s.Vehicle_type,

                        //    Date = s.Date
                        //}).Where(z => z.Item_Id == id).OrderByDescending(x => x.Date).Distinct().ToList();
                        return Ok(productForSaleClasses);
                    }

                    else if (first2.ToLower() == "tu".ToLower())
                    {
                        bool chkpcs = (from ord in ctx.Products_For_Sales
                                       where ord.Item_tyre_Id == id
                                       select ord.Pieces).Any();
                        if (chkpcs)
                        {
                            tubelistClasses = (from it in ctx.Item_Tubes
                                                   //join stk in ctx.Products_For_Sales on it.Item_Id equals stk.Item_tyre_Id
                                                   // group stk by stk.Item_tyre_Id into g
                                               select new TubelistClass()
                                               {

                                                   Item_Id = it.Item_Id,
                                                   Company_name = it.Company_name,
                                                   Tube_size = it.Tube_size,

                                                   Pieces = (from ord in ctx.Products_For_Sales
                                                             where ord.Item_tyre_Id == id
                                                             select ord.Pieces).Sum(),

                                                   Date = it.Date
                                               }).Where(z => z.Item_Id == id).OrderByDescending(x => x.Date).Distinct().ToList();
                        }else
                        {
                            tubelistClasses = (from it in ctx.Item_Tubes
                                                   //join stk in ctx.Products_For_Sales on it.Item_Id equals stk.Item_tyre_Id
                                                   // group stk by stk.Item_tyre_Id into g
                                               select new TubelistClass()
                                               {

                                                   Item_Id = it.Item_Id,
                                                   Company_name = it.Company_name,
                                                   Tube_size = it.Tube_size,

                                                   Pieces = 0,

                                                   Date = it.Date
                                               }).Where(z => z.Item_Id == id).OrderByDescending(x => x.Date).Distinct().ToList();
                        }

                        return Ok(tubelistClasses);

                    }

                    else if (first2.ToLower() == "po".ToLower())
                    {
                        bool chkpcs = (from ord in ctx.Products_For_Sales
                                       where ord.Item_tyre_Id == id
                                       select ord.Pieces).Any();
                        if (chkpcs)
                        {
                            otherProductClasses = (from it in ctx.Other_Products
                                                       //join stk in ctx.Products_For_Sales on it.Product_id equals stk.Item_tyre_Id

                                                   select new OtherProductClass()
                                                   {

                                                       Product_id = it.Product_id,
                                                       Product_name = it.Product_name,
                                                       Product_type = it.Product_type,

                                                       Pieces = (from ord in ctx.Products_For_Sales
                                                                 where ord.Item_tyre_Id == it.Product_id
                                                                 select ord.Pieces).Sum(),

                                                       Date = it.Date
                                                   }).Where(z => z.Product_id == id).OrderByDescending(x => x.Date).Distinct().ToList();
                        }
                        else
                        {
                            otherProductClasses = (from it in ctx.Other_Products
                                                       //join stk in ctx.Products_For_Sales on it.Product_id equals stk.Item_tyre_Id

                                                   select new OtherProductClass()
                                                   {

                                                       Product_id = it.Product_id,
                                                       Product_name = it.Product_name,
                                                       Product_type = it.Product_type,

                                                       Pieces = 0,

                                                       Date = it.Date
                                                   }).Where(z => z.Product_id == id).OrderByDescending(x => x.Date).Distinct().ToList();
                        }
                        return Ok(otherProductClasses);
                    }
                    else
                    { return BadRequest("Id does not matched"); }
                }else
                {
                    return BadRequest("Id should not be empty");
                }
            }
         //   

        }

        [HttpGet]
        [Route("GetitemsdetailsByCompname")]
        public async Task<IHttpActionResult> GetitemsdetailsByCompname(string id)
        {
            IList<TyrelistClass> productForSaleClasses = null;
           
            using (var ctx = new EasyBillingEntities())
            {
               // List<string> temp = ctx.Temp_Stock.Select(z=>z.Item_tyre_Id).ToList();
                bool chkpcs = (from ord in ctx.Products_For_Sales
                               where ord.Product_name == id
                               select ord.Pieces).Any();
                if (chkpcs)
                {
                    try
                    {
                        productForSaleClasses = (from it in ctx.Item_Tyres

                                                 select new TyrelistClass()
                                                 {

                                                     Item_Id = it.Item_Id,
                                                     Company_name = it.Company_name,
                                                     Tyre_size = it.Tyre_size,
                                                     Tyre_feel = it.Tyre_feel,
                                                     Tyre_make = it.Tyre_make,
                                                     Vehicle_type = it.Vehicle_type,

                                                     Pieces = (from ord in ctx.Products_For_Sales
                                                               where ord.Item_tyre_Id == it.Item_Id
                                                               select ord.Pieces).Sum(),
                                                     Date = it.Date
                                                 }).Where(z => z.Company_name == id && z.Tyre_make != "Resole").OrderByDescending(x => x.Date).Distinct().ToList();

                    }catch
                    {
                        productForSaleClasses = (from it in ctx.Item_Tyres

                                                 select new TyrelistClass()
                                                 {

                                                     Item_Id = it.Item_Id,
                                                     Company_name = it.Company_name,
                                                     Tyre_size = it.Tyre_size,
                                                     Tyre_feel = it.Tyre_feel,
                                                     Tyre_make = it.Tyre_make,
                                                     Vehicle_type = it.Vehicle_type,

                                                     Pieces = (from ord in ctx.Products_For_Sales
                                                               where ord.Item_tyre_Id == it.Item_Id
                                                               select ord.Pieces).FirstOrDefault(),
                                                     Date = it.Date
                                                 }).Where(z => z.Company_name == id && z.Tyre_make != "Resole").OrderByDescending(x => x.Date).Distinct().ToList();


                    }


                }
                else
                {
                    productForSaleClasses = (from it in ctx.Item_Tyres
                                             select new TyrelistClass()
                                             {
                                                 Item_Id = it.Item_Id,
                                                 Company_name = it.Company_name,
                                                 Tyre_size = it.Tyre_size,
                                                 Tyre_feel = it.Tyre_feel,
                                                 Tyre_make = it.Tyre_make,
                                                 Vehicle_type = it.Vehicle_type,
                                                 Pieces = 0,
                                                 Date = it.Date
                                             }).Where(z => z.Company_name == id && !ctx.Temp_Stock.Any(pv => pv.Item_tyre_Id == z.Item_Id)).OrderByDescending(x => x.Date).Distinct().ToList();
                }

           
                        return Ok(productForSaleClasses);
            }
        }

        [HttpGet]
        [Route("Gettubecomp")]
        public async Task<IHttpActionResult> Gettubecomp(string id)
        {
            IList<TubelistClass> productForSaleClasses = null;
            using (var ctx = new EasyBillingEntities())
            {
                bool chkpcs = (from ord in ctx.Products_For_Sales
                               where ord.Product_name == id
                               select ord.Pieces).Any();
                if (chkpcs)
                {
                    productForSaleClasses = (from it in ctx.Item_Tubes
                                             select new TubelistClass()
                                             {

                                                 Item_Id =it.Item_Id,
                                                 Company_name = it.Company_name,
                                                 Tube_size = it.Tube_size,
                                                 

                                                 Pieces = (from ord in ctx.Products_For_Sales
                                                           where ord.Product_name == id
                                                           select ord.Pieces).Sum(),
                                                 Date = it.Date
                                             }).Where(z => z.Company_name == id && !ctx.Temp_Stock.Any(pv => pv.Item_tyre_Id == z.Item_Id)).OrderByDescending(x => x.Date).Distinct().ToList();
                }
                else
                {
                    productForSaleClasses = (from it in ctx.Item_Tubes
                                             select new TubelistClass()
                                             {
                                                 Item_Id = it.Item_Id,
                                                 Company_name = it.Company_name,
                                                 Tube_size = it.Tube_size,

                                                 Pieces = 0,
                                                 Date = it.Date
                                             }).Where(z => z.Company_name == id && !ctx.Temp_Stock.Any(pv => pv.Item_tyre_Id == z.Item_Id)).OrderByDescending(x => x.Date).Distinct().ToList();
                }


                return Ok(productForSaleClasses);
            }
        }

        [HttpGet]
        [Route("GetProdctDetls")]
        public async Task<IHttpActionResult> GetProdctDetls(string id)
        {
            IList<OtherProductClass> productForSaleClasses = null;
            using (var ctx = new EasyBillingEntities())
            {
                bool chkpcs = (from ord in ctx.Products_For_Sales
                               where ord.Product_name == id
                               select ord.Pieces).Any();
                if (chkpcs)
                {
                    productForSaleClasses = (from it in ctx.Other_Products
                                             select new OtherProductClass()
                                             {

                                                 Product_id =it.Product_id,
                                                 Product_name = it.Product_name,
                                                 Product_type = it.Product_type,


                                                 Pieces = (from ord in ctx.Products_For_Sales
                                                           where ord.Product_name == id
                                                           select ord.Pieces).Sum(),
                                                 Date = it.Date
                                             }).Where(z => z.Product_name == id && !ctx.Temp_Stock.Any(pv => pv.Item_tyre_Id == z.Product_id)).OrderByDescending(x => x.Date).Distinct().ToList();
                }
                else
                {
                    productForSaleClasses = (from it in ctx.Other_Products
                                             select new OtherProductClass()
                                             {
                                                 Product_id = it.Product_id,
                                                 Product_name = it.Product_name,
                                                 Product_type = it.Product_type,
                                                 
                                                 Pieces = 0,
                                                 Date = it.Date
                                             }).Where(z => z.Product_name == id && !ctx.Temp_Stock.Any(pv => pv.Item_tyre_Id == z.Product_id)).OrderByDescending(x => x.Date).Distinct().ToList();
                }


                return Ok(productForSaleClasses);
            }
        }

        [HttpGet]
        [Route("GetApprovestock")]
        public async Task<IHttpActionResult> GetApprovestock()
        {
            using (EasyBillingEntities db = new EasyBillingEntities())
            {
                try
                {
                    List<Products_For_Sale> products_For_Salelist = db.Products_For_Sales.Where(z => z.Approve == false).Distinct().ToList();
                    if (products_For_Salelist.Count > 0)
                    {
                        List<Stock> stklst = new List<Stock>();
                        foreach (var products_For_Sale in products_For_Salelist)
                        {
                            products_For_Sale.Approve = true;
                            products_For_Sale.requestsend = false;
                            products_For_Sale.Approve_user_id = User.Identity.Name;
                            products_For_Sale.Approve_user_name = db.Employees.Where(z => z.Employee_Id == User.Identity.Name).Select(z => z.Employee_name).FirstOrDefault();
                          
                            bool chk = db.Stocks.Where(a => a.Product_Token == products_For_Sale.Token_Number).Any();
                            if (chk == true)
                            {
                                Stock stk = db.Stocks.Where(a => a.Product_Token == products_For_Sale.Token_Number).FirstOrDefault();
                                //stk.Date = DateTime.Now.Date;
                                stk.Remodify_Date = DateTime.Now.Date;
                                products_For_Sale.Approve_date = DateTime.Now.Date;
                                stk.Pieces = stk.Pieces + (int)products_For_Sale.StockIn;

                                stk.Product_Token = products_For_Sale.Token_Number;

                            }
                            else
                            {
                                Stock stk = new Stock();
                                stk.Date = DateTime.Now.Date;
                                stk.Remodify_Date = DateTime.Now.Date;
                                products_For_Sale.Approve_date = DateTime.Now.Date;
                                stk.Pieces = (int)products_For_Sale.Pieces;

                                stk.Product_Token = products_For_Sale.Token_Number;
                                stk.Remodify_Date = DateTime.Now.Date;
                                stk.Remodify_pcs = 99;
                                stk.CGST = products_For_Sale.CGST;
                                stk.SGST = products_For_Sale.SGST;
                                stklst.Add(stk);
                            }
                        }
                        db.Stocks.AddRange(stklst);
                        await db.SaveChangesAsync();
                        return Ok();
                    }else
                    {
                        return BadRequest("Already approved.");
                    }
                }
                catch(Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
        [HttpGet]
        [Route("Getretstock")]
        public async Task<IHttpActionResult> Getretstock()
        {
            using (EasyBillingEntities db = new EasyBillingEntities())
            {
                try
                {
                    List<Products_For_Sale> products_For_Salelist = db.Products_For_Sales.Where(z => z.Approve == false).Distinct().ToList();
                    if (products_For_Salelist.Count > 0)
                    {
                        bool chkret = db.Products_For_Sales.Where(z => z.requestsend == true && z.Approve == false).Distinct().Any();
                        if(chkret)
                            return BadRequest("Already returned.");
                      
                        foreach (var products_For_Sale in products_For_Salelist)
                        {
                            products_For_Sale.requestsend = true;
                            
                        }
                      
                        await db.SaveChangesAsync();
                        return Ok();
                    }
                    else
                    {
                        return BadRequest("Already approved.");
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
        [HttpPost]
        [Route("Postallstock")]
        public async Task<IHttpActionResult> Postallstock([FromBody]  Products_For_Sale products_For_Sale)
        {
            try
            {
                if (products_For_Sale.Date == null || products_For_Sale.Date == DateTime.Parse("1/1/0001 12:00:00 AM"))
                    return BadRequest("Date should not be empty");
                else
                {
                    using (EasyBillingEntities db = new EasyBillingEntities())
                    {
                        int chk = 0;
                        EasyBillingEntities dban = new EasyBillingEntities();
                        List<Products_For_Sale> temp_StockList = new List<Products_For_Sale>();

                        temp_StockList = dban.Products_For_Sales.Where(z => z.Approve == false && z.requestsend == true).ToList();
                        if (temp_StockList.Count > 0)
                        {
                            chk = 1;
                            dban.Products_For_Sales.RemoveRange(temp_StockList);
                            dban.SaveChanges();
                        }
                        List<Products_For_Sale> products_For_SalesList = new List<Products_For_Sale>();
                        Products_For_Sale products_For_Sales = new Products_For_Sale();
                        var allstocks = db.Temp_Stock.ToList();

                      var User_Id = User.Identity.Name;
                      var User_name = db.Employees.Where(z => z.Employee_Id == User.Identity.Name).Select(z => z.Employee_name).FirstOrDefault();
                        foreach (var stk in allstocks)
                        {
                            products_For_Sales = new Products_For_Sale();

                            products_For_Sales.Token_Number = stk.Token_Number;
                            products_For_Sales.User_Id = User_Id;
                            products_For_Sales.User_name = User_name;
                            products_For_Sales.Product_Token = stk.Product_Token;

                            products_For_Sales.Product_name = stk.Product_name;
                            products_For_Sales.Pieces = stk.Pieces;
                            products_For_Sales.IsGstPercent = stk.IsGstPercent;
                            products_For_Sales.Amout_after_tax = stk.Amout_after_tax;

                            products_For_Sales.Total = stk.Total;

                            products_For_Sales.Tyre_Size = stk.Tyre_Size;

                            products_For_Sales.Supplier_token = stk.Supplier_token;

                            products_For_Sales.Supplier_name = stk.Supplier_name;

                            products_For_Sales.Date = products_For_Sale.Date;
                            products_For_Sales.Purchase_Price = stk.Purchase_Price;

                            products_For_Sales.CGST = stk.CGST;

                            products_For_Sales.SGST = stk.SGST;


                            products_For_Sales.Administrator_Token_number = stk.Administrator_Token_number;

                            products_For_Sales.Administrator_name = stk.Administrator_name;
                           
                                products_For_Sales.CalculationByRatePerUnit = stk.CalculationByRatePerUnit;
                            
                            products_For_Sales.Approve_date = stk.Approve_date;
                            products_For_Sales.Approve = stk.Approve;
                            bool chkselling = (from x in db.Products_For_Sales
                                               where x.Approve == true && x.Item_tyre_Id == stk.Item_tyre_Id
                                               group x by x.Item_tyre_Id into grp
                                               select grp.OrderByDescending(y => y.Approve_date).FirstOrDefault()).Any();
                            if (chkselling)
                            {
                                var sellingexisting = (from x in db.Products_For_Sales
                                                       where x.Approve == true && x.Item_tyre_Id == stk.Item_tyre_Id
                                                       group x by x.Item_tyre_Id into grp
                                                       select grp.OrderByDescending(y => y.Approve_date).FirstOrDefault()).FirstOrDefault();
                                if (sellingexisting != null)
                                {
                                    products_For_Sales.Selling_Price = sellingexisting.Selling_Price;

                                    products_For_Sales.Selling_CGST = sellingexisting.Selling_CGST;
                                    products_For_Sales.Selling_SGST = sellingexisting.Selling_SGST;
                                }
                            }
                            else
                            {
                                products_For_Sales.Selling_Price = stk.Selling_Price;

                                products_For_Sales.Up_Selling_Price = stk.Up_Selling_Price;
                                products_For_Sales.Selling_CGST = 0;
                                products_For_Sales.Selling_SGST = 0;
                                products_For_Sales.Up_CGST = stk.Up_CGST;

                                products_For_Sales.Up_SGST = stk.Up_SGST;
                                products_For_Sales.Selling_net_total = 0;
                            }

                            products_For_Sales.StockIn = stk.StockIn;

                            products_For_Sales.Delivery_contact_number = stk.Delivery_contact_number;

                            products_For_Sales.Delivery_address = stk.Delivery_address;
                            products_For_Sales.Item_tyre_Id = stk.Item_tyre_Id;
                            products_For_Sales.Purchase_number = stk.Purchase_number;
                            products_For_Sales.PurchaseDate = stk.PurchaseDate;
                            products_For_Sales.Tyre_make = stk.Tyre_make;

                            products_For_Sales.Tyre_type = stk.Tyre_type;

                            products_For_Sales.Tyre_feel = stk.Tyre_feel;
                            products_For_Sales.Vehicle_Token = stk.Vehicle_Token;

                            products_For_Sales.Vehicle_type = stk.Vehicle_type;
                            products_For_Sales.Description = stk.Description;
                            products_For_Sales.Tyre_token = stk.Tyre_token;
                            products_For_Sales.Item_tyre_token = stk.Item_tyre_token;
                            products_For_Sales.Mac_id = stk.Mac_id;
                            products_For_Sales.requestsend = false;
                            products_For_SalesList.Add(products_For_Sales);
                        }
                        if (chk == 0)
                        {
                            Purchase_Invoice purchase_Invoice = new Purchase_Invoice();

                            purchase_Invoice.Token_number = Guid.NewGuid().ToString();
                            purchase_Invoice.Purchase_invoice_number = products_For_Sale.Purchase_number;
                            bool chkinv = db.Purchase_Invoices.Where(z => z.Purchase_invoice_number == purchase_Invoice.Purchase_invoice_number).Any();
                            if (chkinv == true)
                            {
                                return BadRequest(purchase_Invoice.Purchase_invoice_number + " number already exist. Try with another.");
                            }
                            else
                            {
                                if (products_For_Sale.PurchaseDate == DateTime.Parse("01-01-0001 00:00:00"))
                                {
                                    products_For_Sale.PurchaseDate = DateTime.Parse("01-01-2000 00:00:00");
                                    purchase_Invoice.Date = DateTime.Parse("01-01-2000 00:00:00");
                                }else
                                purchase_Invoice.Date = products_For_Sale.PurchaseDate;
                                purchase_Invoice.Stock_entry_token = products_For_Sale.Token_Number;
                                db.Products_For_Sales.AddRange(products_For_SalesList);

                                db.Purchase_Invoices.Add(purchase_Invoice);

                                await db.SaveChangesAsync();
                            }
                        }
                        else
                        {
                            db.Products_For_Sales.AddRange(products_For_SalesList);
                            await db.SaveChangesAsync();
                        }
                        return Ok();
                    }
                }
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Route("PostBillSave")]
        public async Task<IHttpActionResult> PostBillSave([FromBody] Products_For_Sale products_For_Sale)
        {
            Other_Product product = new Other_Product();
            using (EasyBillingEntities db = new EasyBillingEntities())
            {
                try
                {
                    if (products_For_Sale.selectionForExecution == "Purchase")
                    {
                        Billing_Master billing_Master = new Billing_Master();
                        Billing_Detail billing_Detail = new Billing_Detail();
                        List<TempbillClass> products_For_Saleclss = (from it in db.Temp_Bill
                                                                     orderby it.Approve_date descending
                                                                     select new TempbillClass()
                                                                     {
                                                                         Token_Number = it.Token_Number,
                                                                         Item_tyre_Id = it.Item_tyre_Id,
                                                                         Approve_date = it.Approve_date,
                                                                         Product_name = it.Product_name,
                                                                         Pieces = it.Pieces,
                                                                         Selling_Price = it.Selling_Price,
                                                                         Selling_CGST = it.Selling_CGST,
                                                                         Selling_SGST = it.Selling_SGST,
                                                                         Total = it.Total
                                                                     }).Distinct().ToList();


                        Temp_Bill temp_Stock = new Temp_Bill();
                        temp_Stock.Selling_CGST = db.Temp_Bill.Select(z => z.Selling_CGST).Sum();
                        temp_Stock.Selling_SGST = db.Temp_Bill.Select(z => z.Selling_SGST).Sum();
                        temp_Stock.Total = db.Temp_Bill.Select(z => z.Total).Sum();

                        billing_Master.Token_Number = Guid.NewGuid().ToString();
                        billing_Master.Billing_Number = products_For_Sale.Billno;
                        billing_Master.Date = DateTime.Now;
                        billing_Master.Total_tax = temp_Stock.Selling_CGST + temp_Stock.Selling_SGST;
                        billing_Master.Rate_including_tax = temp_Stock.Total;
                        billing_Master.Discount = products_For_Sale.Discount;
                        billing_Master.Total_discount = products_For_Sale.Total_discount;
                        billing_Master.Total_amount = temp_Stock.Total;
                        billing_Master.IsGstPercent = products_For_Sale.IsGstPercent;
                        if (products_For_Sale.percent == "1")
                            billing_Master.Discountper = true;
                        else
                            billing_Master.Discountper = false;
                        billing_Master.Narration = products_For_Sale.Narration;
                        billing_Master.Customer_token_number = products_For_Sale.CustToken_number;
                        billing_Master.Amount_paid = products_For_Sale.Amount_paid;
                        billing_Master.Balance = products_For_Sale.Balance;
                        billing_Master.Mode_of_payment = products_For_Sale.Mode_of_payment;
                        billing_Master.Transaction_Id = products_For_Sale.Narration;
                        NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
                        String sMacAddress = string.Empty;
                        foreach (NetworkInterface adapter in nics)
                        {
                            IPInterfaceProperties properties = adapter.GetIPProperties();
                            sMacAddress = adapter.GetPhysicalAddress().ToString();
                            if (!string.IsNullOrEmpty(sMacAddress))// only return MAC Address from first card  
                                break;
                        }

                        billing_Master.Mac_id = sMacAddress;
                        billing_Master.User_Id = User.Identity.Name;

                        db.Billing_Masters.Add(billing_Master);
                        foreach (var ech in products_For_Saleclss)
                        {
                            billing_Detail = new Billing_Detail();
                            billing_Detail.Billing_Token_number = billing_Master.Token_Number;
                            billing_Detail.Billing_number = billing_Master.Billing_Number;
                            billing_Detail.Date = billing_Master.Date;
                            ProductForSaleClass products_For_Salesingle = (from it in db.Products_For_Sales

                                                                           select new ProductForSaleClass()
                                                                           {
                                                                               Token_Number = it.Token_Number,
                                                                               Item_tyre_Id = it.Item_tyre_Id,
                                                                               Approve_date = it.Approve_date,
                                                                               IsGstPercent = it.IsGstPercent,
                                                                               Product_name = it.Product_name,
                                                                               Pieces = it.Pieces,
                                                                               Selling_Price = it.Selling_Price,
                                                                               Selling_CGST = it.Selling_CGST,
                                                                               Selling_SGST = it.Selling_SGST,
                                                                               Total = it.Total,
                                                                           }).Where(z => z.Item_tyre_Id == ech.Item_tyre_Id && z.Pieces > 0).OrderByDescending(z => z.Approve_date).FirstOrDefault();
                            if (products_For_Salesingle == null)
                            {
                                product = db.Other_Products.Where(z => z.Product_id == ech.Item_tyre_Id).FirstOrDefault();
                                billing_Detail.Selling_item_token = product.Token_number;
                                billing_Detail.IsGstPercent = true;
                            }
                            else
                            {
                                billing_Detail.Selling_item_token = products_For_Salesingle.Token_Number;
                                billing_Detail.IsGstPercent = products_For_Salesingle.IsGstPercent;
                            }
                            
                            billing_Detail.Pieces = ech.Pieces;
                            billing_Detail.Amount = ech.Total;
                            billing_Detail.Tax = ech.Selling_CGST + ech.Selling_SGST;
                            billing_Detail.Discount = decimal.Parse("0");
                            billing_Detail.Sub_Total = ech.Total;
                            billing_Detail.Selling_item_id = ech.Item_tyre_Id;

                            db.Billing_Details.Add(billing_Detail);
                            if (products_For_Salesingle != null)
                            {
                                var restPieces = products_For_Salesingle.Pieces - ech.Pieces;

                                if (restPieces < 0)
                                    return BadRequest("Quantity cannot be less than stok quantity");
                                Products_For_Sale updatepro = db.Products_For_Sales.Where(z => z.Token_Number == products_For_Salesingle.Token_Number).FirstOrDefault();
                                updatepro.Pieces = restPieces;
                            }
                            db.SaveChanges();
                        }
                        db.Temp_Bill.RemoveRange(db.Temp_Bill.ToList());
                        await db.SaveChangesAsync();

                        return Ok();
                    }
                    else if (products_For_Sale.selectionForExecution == "Order")
                    {
                        
                            Order_Master billing_Master = new Order_Master();
                        Order_Detail billing_Detail = new Order_Detail();
                        List<TempbillClass> products_For_Saleclss = (from it in db.Temp_Bill
                                                                     orderby it.Approve_date descending
                                                                     select new TempbillClass()
                                                                     {
                                                                         Token_Number = it.Token_Number,
                                                                         Item_tyre_Id = it.Item_tyre_Id,
                                                                         Approve_date = it.Approve_date,
                                                                         Product_name = it.Product_name,
                                                                         Pieces = it.Pieces,
                                                                         Selling_Price = it.Selling_Price,
                                                                         Selling_CGST = it.Selling_CGST,
                                                                         Selling_SGST = it.Selling_SGST,
                                                                         Total = it.Total,
                                                                         Tyre_number=it.Tyre_number
                                                                     }).Distinct().ToList();

                        //foreach (var ech1 in products_For_Saleclss)
                        //{
                        //    Order_Detail order_Detail = new Order_Detail();
                        //    order_Detail.Order_Token_number = billing_Master.Token_Number;
                        //    order_Detail.Order_number = billing_Master.Order_Number;
                        //    order_Detail.Date = billing_Master.Date;
                        //    var products_For_Salesingle = db.Products_For_Sales
                        //                                  .Where(z => z.Item_tyre_Id == ech1.Item_tyre_Id && z.Pieces > 0)
                        //                                  .Select(z=>z).OrderByDescending(z => z.Approve_date).FirstOrDefault();

                            //order_Detail.Selling_item_token = products_For_Salesingle.Token_Number;
                            //order_Detail.IsGstPercent = products_For_Salesingle.IsGstPercent;
                            //order_Detail.Pieces = ech.Pieces;
                            //order_Detail.Amount = ech.Total;
                            //order_Detail.Tax = ech.Selling_CGST + ech.Selling_SGST;
                            //order_Detail.Discount = decimal.Parse("0");
                            //order_Detail.Sub_Total = ech.Total;
                            //order_Detail.Selling_item_id = ech.Item_tyre_Id;

                            //db.Order_Details.Add(order_Detail);
                        //}
                        Temp_Bill temp_Stock = new Temp_Bill();
                      
                        temp_Stock.Selling_CGST = db.Temp_Bill.Select(z => z.Selling_CGST).Sum();
                        temp_Stock.Selling_SGST = db.Temp_Bill.Select(z => z.Selling_SGST).Sum();
                        temp_Stock.Total = db.Temp_Bill.Select(z => z.Total).Sum();

                        billing_Master.Token_Number = Guid.NewGuid().ToString();
                        
                        billing_Master.Order_Number = products_For_Sale.Billno;
                        billing_Master.Date = DateTime.Now;
                        billing_Master.Total_tax = temp_Stock.Selling_CGST + temp_Stock.Selling_SGST;
                        billing_Master.Rate_including_tax = temp_Stock.Total;
                        billing_Master.Discount = products_For_Sale.Discount;
                        billing_Master.Total_discount = products_For_Sale.Total_discount;
                        billing_Master.Total_amount = temp_Stock.Total;
                        billing_Master.IsGstPercent = products_For_Sale.IsGstPercent;
                        if (products_For_Sale.percent == "1")
                            billing_Master.Discountper = true;
                        else
                            billing_Master.Discountper = false;
                        billing_Master.Narration = products_For_Sale.Narration;
                        billing_Master.Customer_token_number = products_For_Sale.CustToken_number;
                        billing_Master.Amount_paid = products_For_Sale.Amount_paid;
                        billing_Master.Balance = products_For_Sale.Balance;
                        billing_Master.Mode_of_payment = products_For_Sale.Mode_of_payment;
                        billing_Master.Transaction_Id = products_For_Sale.Narration;
                        NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
                        String sMacAddress = string.Empty;
                        foreach (NetworkInterface adapter in nics)
                        {
                            IPInterfaceProperties properties = adapter.GetIPProperties();
                            sMacAddress = adapter.GetPhysicalAddress().ToString();
                            if (!string.IsNullOrEmpty(sMacAddress))// only return MAC Address from first card  
                                break;
                        }

                        billing_Master.Mac_id = sMacAddress;
                        billing_Master.User_Id = User.Identity.Name;

                        db.Order_Masters.Add(billing_Master);
                        foreach (var ech1 in products_For_Saleclss)
                        {
                            billing_Detail = new Order_Detail();
                            billing_Detail.Order_Token_number = billing_Master.Token_Number;
                            billing_Detail.Order_number = billing_Master.Order_Number;
                            billing_Detail.Date = billing_Master.Date;
                            billing_Detail.Tyre_number = ech1.Tyre_number;
                            billing_Detail.Order_details_number = db.Order_Details.OrderByDescending(x => x.Order_details_number).Select(x => x.Order_details_number).FirstOrDefault() + 1;
                            ProductForSaleClass products_For_Salesingle = (from it in db.Products_For_Sales

                                                                           select new ProductForSaleClass()
                                                                           {
                                                                               Token_Number = it.Token_Number,
                                                                               Item_tyre_Id = it.Item_tyre_Id,
                                                                               Approve_date = it.Approve_date,
                                                                               IsGstPercent = it.IsGstPercent,
                                                                               Product_name = it.Product_name,
                                                                               Pieces = it.Pieces,
                                                                               Selling_Price = it.Selling_Price,
                                                                               Selling_CGST = it.Selling_CGST,
                                                                               Selling_SGST = it.Selling_SGST,
                                                                               Total = it.Total,
                                                                           }).Where(z => z.Item_tyre_Id == ech1.Item_tyre_Id && z.Pieces > 0).OrderByDescending(z => z.Approve_date).FirstOrDefault();

                            billing_Detail.Selling_item_token = products_For_Salesingle.Token_Number;
                            billing_Detail.IsGstPercent = products_For_Salesingle.IsGstPercent;
                            billing_Detail.Pieces = ech1.Pieces;
                            billing_Detail.Amount = ech1.Total;
                            billing_Detail.Tax = ech1.Selling_CGST + ech1.Selling_SGST;
                            billing_Detail.Discount = decimal.Parse("0");
                            billing_Detail.Sub_Total = ech1.Total;
                            billing_Detail.Selling_item_id = ech1.Item_tyre_Id;

                            db.Order_Details.Add(billing_Detail);

                            //var restPieces = products_For_Salesingle.Pieces - ech1.Pieces;

                            //if (restPieces < 0)
                            //    return BadRequest("Quantity cannot be less than stok quantity");
                            //Products_For_Sale updatepro = db.Products_For_Sales.Where(z => z.Token_Number == billing_Detail.Selling_item_token).FirstOrDefault();
                            //updatepro.Pieces = restPieces;
                            db.SaveChanges();
                        }
                        db.Temp_Bill.RemoveRange(db.Temp_Bill.ToList());
                        await db.SaveChangesAsync();

                        return Ok();
                    }
                    else
                    {
                        Quotation_Master billing_Master = new Quotation_Master();
                        Quotation_Detail billing_Detail = new Quotation_Detail();
                        List<TempbillClass> products_For_Saleclss = (from it in db.Temp_Bill
                                                                     orderby it.Approve_date descending
                                                                     select new TempbillClass()
                                                                     {
                                                                         Token_Number = it.Token_Number,
                                                                         Item_tyre_Id = it.Item_tyre_Id,
                                                                         Approve_date = it.Approve_date,
                                                                         Product_name = it.Product_name,
                                                                         Pieces = it.Pieces,
                                                                         Selling_Price = it.Selling_Price,
                                                                         Selling_CGST = it.Selling_CGST,
                                                                         Selling_SGST = it.Selling_SGST,
                                                                         Total = it.Total
                                                                     }).Distinct().ToList();


                        Temp_Bill temp_Stock = new Temp_Bill();
                        temp_Stock.Selling_CGST = db.Temp_Bill.Select(z => z.Selling_CGST).Sum();
                        temp_Stock.Selling_SGST = db.Temp_Bill.Select(z => z.Selling_SGST).Sum();
                        temp_Stock.Total = db.Temp_Bill.Select(z => z.Total).Sum();

                        billing_Master.Token_Number = Guid.NewGuid().ToString();
                        billing_Master.Quotation_Number = products_For_Sale.Billno;
                        billing_Master.Date = DateTime.Now;
                        billing_Master.Total_tax = temp_Stock.Selling_CGST + temp_Stock.Selling_SGST;
                        billing_Master.Rate_including_tax = temp_Stock.Total;
                        billing_Master.Discount = products_For_Sale.Discount;
                        billing_Master.Total_discount = products_For_Sale.Total_discount;
                        billing_Master.Total_amount = temp_Stock.Total;
                        billing_Master.IsGstPercent = products_For_Sale.IsGstPercent;
                        if (products_For_Sale.percent == "1")
                            billing_Master.Discountper = true;
                        else
                            billing_Master.Discountper = false;
                        billing_Master.Narration = products_For_Sale.Narration;
                        billing_Master.Customer_token_number = products_For_Sale.CustToken_number;
                       
                        billing_Master.Transaction_Id = products_For_Sale.Narration;
                        NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
                        String sMacAddress = string.Empty;
                        foreach (NetworkInterface adapter in nics)
                        {
                            IPInterfaceProperties properties = adapter.GetIPProperties();
                            sMacAddress = adapter.GetPhysicalAddress().ToString();
                            if (!string.IsNullOrEmpty(sMacAddress))// only return MAC Address from first card  
                                break;
                        }

                        billing_Master.Mac_id = sMacAddress;
                        billing_Master.User_Id = User.Identity.Name;

                        db.Quotation_Masters.Add(billing_Master);
                        foreach (var ech in products_For_Saleclss)
                        {
                            billing_Detail = new Quotation_Detail();
                            billing_Detail.Quotation_Token_number = billing_Master.Token_Number;
                            billing_Detail.Quotation_number = billing_Master.Quotation_Number;
                            billing_Detail.Date = billing_Master.Date;
                            billing_Detail.Quotation_details_number = db.Quotation_Details.OrderByDescending(x => x.Quotation_details_number).Select(x => x.Quotation_details_number).FirstOrDefault() + 1;
                            ProductForSaleClass products_For_Salesingle = (from it in db.Products_For_Sales

                                                                           select new ProductForSaleClass()
                                                                           {
                                                                               Token_Number = it.Token_Number,
                                                                               Item_tyre_Id = it.Item_tyre_Id,
                                                                               Approve_date = it.Approve_date,
                                                                               IsGstPercent = it.IsGstPercent,
                                                                               Product_name = it.Product_name,
                                                                               Pieces = it.Pieces,
                                                                               Selling_Price = it.Selling_Price,
                                                                               Selling_CGST = it.Selling_CGST,
                                                                               Selling_SGST = it.Selling_SGST,
                                                                               Total = it.Total,
                                                                           }).Where(z => z.Item_tyre_Id == ech.Item_tyre_Id && z.Pieces > 0).OrderByDescending(z => z.Approve_date).FirstOrDefault();

                            billing_Detail.Selling_item_token = products_For_Salesingle.Token_Number;
                            billing_Detail.IsGstPercent = products_For_Salesingle.IsGstPercent;
                            billing_Detail.Pieces = ech.Pieces;
                            billing_Detail.Amount = ech.Total;
                            billing_Detail.Tax = ech.Selling_CGST + ech.Selling_SGST;
                            billing_Detail.Discount = decimal.Parse("0");
                            billing_Detail.Sub_Total = ech.Total;
                            billing_Detail.Selling_item_id = ech.Item_tyre_Id;

                            db.Quotation_Details.Add(billing_Detail);

                            var restPieces = products_For_Salesingle.Pieces - ech.Pieces;

                            if (restPieces < 0)
                                return BadRequest("Quantity cannot be less than stok quantity");
                            Products_For_Sale updatepro = db.Products_For_Sales.Where(z => z.Token_Number == products_For_Salesingle.Token_Number).FirstOrDefault();
                            updatepro.Pieces = restPieces;
                            db.SaveChanges();
                        }
                        db.Temp_Bill.RemoveRange(db.Temp_Bill.ToList());
                        await db.SaveChangesAsync();

                        return Ok();
                    }
                        
                }
                catch(Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
 
        [HttpPost]
        [Route("PostSave")]
        public async Task<IHttpActionResult> PostSave([FromBody] Temp_Stock products_For_Sale)
        {
            using (EasyBillingEntities db = new EasyBillingEntities())
            {
                bool isexist = false;
                if (!string.IsNullOrEmpty(products_For_Sale.Token_Number))
                    isexist = db.Temp_Stock.Where(z => z.Token_Number == products_For_Sale.Token_Number).Any();
                if (isexist)
                {
                    Temp_Stock products_For_Saletmp = db.Temp_Stock.Where(z => z.Token_Number == products_For_Sale.Token_Number).FirstOrDefault();

                    if (products_For_Sale.Purchase_Price <= 0)
                    {
                        return BadRequest("Purchase Price should not be less than or equal 0.");
                    }
                    if (products_For_Sale.Total <= 0)
                    {
                        return BadRequest("Total should not be less than or equal 0.");
                    }
                    if (products_For_Sale.Pieces <= 0)
                    {
                        return BadRequest("Stock should not be less than or equal 0.");
                    }

                    else
                    {
                        products_For_Saletmp.Approve = false;
                        products_For_Saletmp.Pieces = products_For_Sale.Pieces;
                        products_For_Saletmp.Purchase_Price = products_For_Sale.Purchase_Price;
                        products_For_Saletmp.CGST = products_For_Sale.CGST;
                        products_For_Saletmp.SGST = products_For_Sale.SGST;
                        products_For_Saletmp.Total = products_For_Sale.Total;

                        NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
                        String sMacAddress = string.Empty;
                        foreach (NetworkInterface adapter in nics)
                        {
                            IPInterfaceProperties properties = adapter.GetIPProperties();
                            sMacAddress = adapter.GetPhysicalAddress().ToString();
                            if (!string.IsNullOrEmpty(sMacAddress))// only return MAC Address from first card  
                                break;
                        }
                        products_For_Saletmp.Mac_id = sMacAddress;
                        // products_For_Sale.Product = db.Products.Where(x=>x.Token_Number==products_For_Sale.Product_Token).FirstOrDefault();


                        await db.SaveChangesAsync();

                        return Created("", products_For_Saletmp);

                    }

                }
                else
                {
                    // products_For_Sale.Date= DateTime.Now;
                    products_For_Sale.Token_Number = Guid.NewGuid().ToString();
                    if (ModelState.ContainsKey("Date"))
                        ModelState["Date"].Errors.Clear();
                    if (products_For_Sale.Pieces <= 0)
                    {
                        return BadRequest("Stock should not be less than or equal 0.");
                    }
                    if (products_For_Sale.Purchase_Price <= 0)
                    {
                        return BadRequest("Purchase Price should not be less than or equal 0.");
                    }
                    if (products_For_Sale.Total <= 0)
                    {
                        return BadRequest("Total should not be less than or equal 0.");
                    }
                    if (ModelState.IsValid)
                    {


                        if (!string.IsNullOrEmpty(products_For_Sale.Item_tyre_Id))
                        {
                            var first2 = products_For_Sale.Item_tyre_Id.Substring(products_For_Sale.Item_tyre_Id.Length - products_For_Sale.Item_tyre_Id.Length, 2);
                            if (!string.IsNullOrEmpty(first2))
                            {
                                if (first2.ToLower() == "ty".ToLower())
                                {
                                    var values = db.Item_Tyres.Where(z => z.Item_Id == products_For_Sale.Item_tyre_Id).Select(z => new { z.Company_name, z.Tyre_type, z.Tyre_feel, z.Tyre_make, z.Tyre_size, z.Vehicle_type }).Distinct().FirstOrDefault();
                                    products_For_Sale.Product_name = values.Company_name;
                                    products_For_Sale.Tyre_feel = values.Tyre_feel;
                                    products_For_Sale.Tyre_Size = values.Tyre_size;
                                    products_For_Sale.Tyre_make = values.Tyre_make;
                                    products_For_Sale.Tyre_type = values.Tyre_type;
                                    products_For_Sale.Vehicle_type = values.Vehicle_type;

                                    products_For_Sale.Tyre_token = db.Tyre_sizes.Where(z => z.Tyre_size1 == products_For_Sale.Tyre_Size).Select(z => z.Token_number).Distinct().FirstOrDefault();
                                    products_For_Sale.Vehicle_Token = db.Vehicles.Where(z => z.Vehicle_type == products_For_Sale.Vehicle_type).Select(z => z.Token_number).Distinct().FirstOrDefault();

                                    products_For_Sale.Item_tyre_token = db.Item_Tyres.Where(z => z.Item_Id == products_For_Sale.Item_tyre_Id).Select(z => z.Token_number).Distinct().FirstOrDefault();
                                    if (string.IsNullOrEmpty(products_For_Sale.Product_Token))
                                    {
                                        products_For_Sale.Product_Token = db.Products.Where(z => z.Product_name == products_For_Sale.Product_name).Select(z => z.Token_Number).FirstOrDefault();
                                    }

                                    if (string.IsNullOrEmpty(products_For_Sale.Item_tyre_token))
                                    {
                                        return BadRequest("Product Id should be seleted.");
                                    }
                                }
                                else if (first2.ToLower() == "tu".ToLower())
                                {
                                    var values = db.Item_Tubes.Where(z => z.Item_Id == products_For_Sale.Item_tyre_Id).Select(z => new { z.Company_name, z.Tube_size }).Distinct().FirstOrDefault();
                                    products_For_Sale.Product_name = values.Company_name;
                                    if (string.IsNullOrEmpty(products_For_Sale.Product_Token))
                                    {
                                        products_For_Sale.Product_Token = db.Products.Where(z => z.Product_name == products_For_Sale.Product_name).Select(z => z.Token_Number).FirstOrDefault();
                                    }
                                    products_For_Sale.Tyre_Size = values.Tube_size;
                                    products_For_Sale.Tyre_token = db.Tyre_sizes.Where(z => z.Tyre_size1 == products_For_Sale.Tyre_Size).Select(z => z.Token_number).Distinct().FirstOrDefault();
                                    // products_For_Sale.Vehicle_Token = db.Vehicles.Where(z => z.Vehicle_type == products_For_Sale.Vehicle_type).Select(z => z.Token_number).Distinct().FirstOrDefault();

                                    products_For_Sale.Item_tyre_token = db.Item_Tubes.Where(z => z.Item_Id == products_For_Sale.Item_tyre_Id).Select(z => z.Token_number).Distinct().FirstOrDefault();
                                    //if (string.IsNullOrEmpty(products_For_Sale.Product_Token))
                                    //{
                                    //    products_For_Sale.Product_Token = db.Other_Products.Where(z => z.Product_name == products_For_Sale.Product_name).Select(z => z.Token_number).FirstOrDefault();
                                    //}

                                    if (string.IsNullOrEmpty(products_For_Sale.Item_tyre_token))
                                    {
                                        return BadRequest("Item tube Id should be seleted.");
                                    }
                                }
                                else if (first2.ToLower() == "po".ToLower())
                                {
                                    var values = db.Other_Products.Where(z => z.Product_id == products_For_Sale.Item_tyre_Id).Select(z => new { z.Product_name, z.Product_type }).Distinct().FirstOrDefault();
                                    //products_For_Sale.Product_name = values.Product_name;
                                    //products_For_Sale.Product_Token = db.Products.Select(z => z.Token_Number).FirstOrDefault();
                                    //products_For_Sale.Tyre_token = db.Tyre_sizes.Where(z => z.Tyre_size1 == products_For_Sale.Tyre_Size).Select(z => z.Token_number).Distinct().FirstOrDefault();
                                    // products_For_Sale.Vehicle_Token = db.Vehicles.Where(z => z.Vehicle_type == products_For_Sale.Vehicle_type).Select(z => z.Token_number).Distinct().FirstOrDefault();

                                    products_For_Sale.Item_tyre_token = db.Other_Products.Where(z => z.Product_id == products_For_Sale.Item_tyre_Id).Select(z => z.Token_number).Distinct().FirstOrDefault();
                                    //if (string.IsNullOrEmpty(products_For_Sale.Product_Token))
                                    //{
                                    //    products_For_Sale.Product_Token = db.Other_Products.Where(z => z.Product_name == products_For_Sale.Product_name).Select(z => z.Token_number).FirstOrDefault();
                                    //}

                                    if (string.IsNullOrEmpty(products_For_Sale.Item_tyre_token))
                                    {
                                        return BadRequest("Item tyre Id should be seleted.");
                                    }
                                }
                            }

                        }
                        else
                        {
                            return BadRequest("Id or name should not be blank.");
                        }

                        if (string.IsNullOrEmpty(products_For_Sale.Purchase_number))
                        {
                            return BadRequest("Purchase number should not be empty.");
                        }
                        else
                        {
                            products_For_Sale.Approve = false;
                            products_For_Sale.Supplier_token = (from sup in db.Dealers
                                                                where sup.Name == products_For_Sale.Supplier_name
                                                                select sup.Token_number).FirstOrDefault();
                            if (products_For_Sale.Date == DateTime.Parse("1/1/0001 12:00:00 AM"))
                                products_For_Sale.Date = DateTime.Now;

                            if (products_For_Sale.percent == "3")
                                products_For_Sale.IsGstPercent = true;
                            else
                                products_For_Sale.IsGstPercent = false;
                            if (products_For_Sale.rateperunit == "1")
                                products_For_Sale.CalculationByRatePerUnit = true;
                            else
                                products_For_Sale.CalculationByRatePerUnit = false;
                            products_For_Sale.Selling_Price = 0;
                            products_For_Sale.Up_Selling_Price = 0;

                            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
                            String sMacAddress = string.Empty;
                            foreach (NetworkInterface adapter in nics)
                            {
                                IPInterfaceProperties properties = adapter.GetIPProperties();
                                sMacAddress = adapter.GetPhysicalAddress().ToString();
                                if (!string.IsNullOrEmpty(sMacAddress))// only return MAC Address from first card  
                                    break;
                            }
                            products_For_Sale.Mac_id = sMacAddress;
                            products_For_Sale.Approve_date = DateTime.Parse("01-01-2000 00:00:00");
                            // products_For_Sale.Product = db.Products.Where(x=>x.Token_Number==products_For_Sale.Product_Token).FirstOrDefault();
                            db.Temp_Stock.Add(products_For_Sale);

                            await db.SaveChangesAsync();

                            return Created("", products_For_Sale);

                        }
                    }
                    else
                    {
                        var snglerrr = string.Empty;
                        //snglerrr = "-----------Error------------------";
                        foreach (var errr in ModelState.Values)
                        {

                            for (int i = 0; i < errr.Errors.Count; i++)
                            {
                                int j = i + 1;
                                if (string.IsNullOrEmpty(errr.Errors[i].ErrorMessage))
                                {
                                    if (string.IsNullOrEmpty(snglerrr))
                                        snglerrr = "Please select date.";

                                }
                                else
                                {
                                    if (string.IsNullOrEmpty(snglerrr))
                                        snglerrr = j + ". " + errr.Errors[i].ErrorMessage;
                                    else
                                        snglerrr = snglerrr + "\n" + j + ". " + errr.Errors[i].ErrorMessage;
                                }
                                //errors.Add(snglerrr);
                            }
                        }

                        return BadRequest(snglerrr);
                    }
                }
            }
        }
        [HttpPost]
        [Route("PostSaveBill")]
        public async Task<IHttpActionResult> PostSaveBill([FromBody] Temp_Bill temp_Bill)
        {
            try
            {
                if (temp_Bill.Amout_after_tax == 11 || temp_Bill.Amout_after_tax == 12 || temp_Bill.Amout_after_tax == 13)
                {
                    Other_Product product = new Other_Product();

                    using (EasyBillingEntities db = new EasyBillingEntities())
                    {
                        bool isexist = false;
                        if (!string.IsNullOrEmpty(temp_Bill.Item_tyre_Id))
                            isexist = db.Temp_Bill.Where(z => z.Item_tyre_Id == temp_Bill.Item_tyre_Id).Any();
                        ProductForSaleClass products_For_Sale = (from it in db.Products_For_Sales

                                                                 select new ProductForSaleClass()
                                                                 {
                                                                     Product_Token = it.Product_Token,
                                                                     Product_name = it.Product_name,
                                                                     Tyre_token = it.Tyre_token,
                                                                     Tyre_Size = it.Tyre_Size,
                                                                     Supplier_token = it.Supplier_token,
                                                                     Supplier_name = it.Supplier_name,
                                                                     Date = DateTime.Now,
                                                                     Purchase_Price = it.Purchase_Price,
                                                                     CGST = it.CGST,
                                                                     SGST = it.SGST,
                                                                     Pieces = it.Pieces,
                                                                     Selling_Price = it.Selling_Price,
                                                                     Amout_after_tax = it.Amout_after_tax,
                                                                     Total = it.Total,
                                                                     Approve_date = it.Approve_date,
                                                                     Approve = it.Approve,
                                                                     Administrator_Token_number = it.Administrator_Token_number,
                                                                     Administrator_name = it.Administrator_name,
                                                                     Delivery_contact_number = it.Delivery_contact_number,
                                                                     Delivery_address = it.Delivery_address,
                                                                     Item_tyre_token = it.Item_tyre_token,
                                                                     Item_tyre_Id = it.Item_tyre_Id,
                                                                     Purchase_number = it.Purchase_number,
                                                                     Vehicle_Token = it.Vehicle_Token,
                                                                     Vehicle_type = it.Vehicle_type,
                                                                     Description = it.Description,
                                                                     Tyre_make = it.Tyre_make,
                                                                     Tyre_feel = it.Tyre_feel,
                                                                     Tyre_type = it.Tyre_type,
                                                                     Mac_id = it.Mac_id,
                                                                     requestsend = it.requestsend,
                                                                     Selling_CGST = it.Selling_CGST,
                                                                     Selling_SGST = it.Selling_SGST,
                                                                     CGST_SGST_CHECK = it.CGST_SGST_CHECK,
                                                                     User_Id = it.User_Id,
                                                                     User_name = it.User_name,
                                                                     Approve_user_id = it.Approve_user_id,
                                                                     Approve_user_name = it.Approve_user_name,
                                                                     Rate_update_user_id = it.Rate_update_user_id,
                                                                     Rate_update_user_name = it.Rate_update_user_name

                                                                 }).Where(z => z.Item_tyre_Id == temp_Bill.Item_tyre_Id && z.Pieces > 0).OrderByDescending(z => z.Approve_date).FirstOrDefault();

                        if(products_For_Sale==null)
                        {
                            product = db.Other_Products.Where(z => z.Product_id == temp_Bill.Item_tyre_Id).FirstOrDefault();
                        }
                        if (isexist)
                        {

                            Temp_Bill products_For_Saletmp = db.Temp_Bill.Where(z => z.Item_tyre_Id == temp_Bill.Item_tyre_Id).FirstOrDefault();
                            products_For_Saletmp.Tyre_number = temp_Bill.Tyre_number;
                            if (temp_Bill.Amout_after_tax == 11 || temp_Bill.Amout_after_tax == 12)
                            {
                                if (temp_Bill.Amout_after_tax == 11 && temp_Bill.Amout_after_tax != 12)
                                {
                                    if (temp_Bill.Pieces <= 0 && temp_Bill.Total <= 0)
                                    {
                                        return BadRequest("Quantity or amount should not be less than or equal 0.");
                                    }
                                    else
                                    {
                                        products_For_Saletmp.Pieces = temp_Bill.Pieces;
                                        products_For_Saletmp.Total = temp_Bill.Total * temp_Bill.Pieces;
                                        products_For_Saletmp.Selling_Price = temp_Bill.Total;
                                        await db.SaveChangesAsync();
                                        return Created("", products_For_Saletmp);
                                    }
                                }
                                else
                                {
                                    if (temp_Bill.Pieces <= 0 && temp_Bill.Total <= 0)
                                    {
                                        return BadRequest("Quantity or amount should not be less than or equal 0.");
                                    }
                                    else
                                    {
                                        products_For_Saletmp.Pieces = temp_Bill.Pieces;
                                        products_For_Saletmp.Total = temp_Bill.Pieces * (temp_Bill.Total + (temp_Bill.Total * (temp_Bill.CGST + temp_Bill.SGST) / 100));
                                        products_For_Saletmp.Selling_CGST = temp_Bill.CGST;
                                        products_For_Saletmp.Selling_SGST = temp_Bill.SGST;
                                        products_For_Saletmp.Selling_Price = temp_Bill.Total;
                                        await db.SaveChangesAsync();
                                        return Created("", products_For_Saletmp);
                                    }
                                }
                            }
                            else
                            {
                                if (temp_Bill.Total <= 0)
                                {
                                    return BadRequest("Amount should not be less than or equal 0.");
                                }
                                else
                                {
                                    products_For_Saletmp.Pieces = 1;
                                    products_For_Saletmp.Selling_Price = temp_Bill.Total;
                                    products_For_Saletmp.Total = temp_Bill.Total;
                                    products_For_Saletmp.Selling_Price = temp_Bill.Total;
                                    await db.SaveChangesAsync();
                                    return Created("", products_For_Saletmp);
                                }
                            }
                        }
                        else
                        {
                            temp_Bill.Token_Number = Guid.NewGuid().ToString();
                            if (ModelState.ContainsKey("Date"))
                                ModelState["Date"].Errors.Clear();
                            if (temp_Bill.Amout_after_tax == 11 || temp_Bill.Amout_after_tax == 12)
                            {
                                if (temp_Bill.Pieces <= 0 && temp_Bill.Total <= 0)
                                {
                                    return BadRequest("Quantity or amount should not be less than or equal 0.");
                                }
                            }
                            else
                            {
                                if (temp_Bill.Total <= 0)
                                {
                                    return BadRequest("Amount should not be less than or equal 0.");
                                }
                            }
                            if (ModelState.IsValid)
                            {
                                if (!string.IsNullOrEmpty(temp_Bill.Item_tyre_Id))
                                {
                                    temp_Bill.Product_Token = product.Token_number;
                                    temp_Bill.Product_name = product.Product_name;
                                    temp_Bill.Date = DateTime.Now;
                                    temp_Bill.Purchase_Price = decimal.Zero;
                                    temp_Bill.Supplier_name = User.Identity.Name;
                                    temp_Bill.Selling_Price = temp_Bill.Total;

                                    if (temp_Bill.Amout_after_tax == 11 && temp_Bill.Amout_after_tax != 12)
                                    {
                                        temp_Bill.Pieces = temp_Bill.Pieces;
                                        temp_Bill.Selling_Price = temp_Bill.Selling_Price;
                                        temp_Bill.Total = temp_Bill.Total * temp_Bill.Pieces;
                                    }
                                    else if (temp_Bill.Amout_after_tax != 11 && temp_Bill.Amout_after_tax == 12)
                                    {
                                        temp_Bill.Selling_CGST = temp_Bill.CGST;
                                        temp_Bill.Selling_SGST = temp_Bill.SGST;
                                        temp_Bill.Pieces = temp_Bill.Pieces;
                                        temp_Bill.Selling_Price = temp_Bill.Total;
                                        temp_Bill.Total = temp_Bill.Pieces * (temp_Bill.Total + (temp_Bill.Total * (temp_Bill.CGST + temp_Bill.SGST) / 100));
                                    }
                                    else if (temp_Bill.Amout_after_tax == 13)
                                    {
                                        temp_Bill.Pieces = 1;
                                        temp_Bill.Selling_Price = temp_Bill.Total;
                                        temp_Bill.Total = temp_Bill.Total;
                                        
                                    }
                                    else
                                    {
                                        temp_Bill.Pieces = temp_Bill.Pieces;
                                        temp_Bill.Selling_Price = products_For_Sale.Selling_Price;
                                        temp_Bill.Total = products_For_Sale.Selling_Price + (products_For_Sale.Selling_Price * (products_For_Sale.Selling_CGST + products_For_Sale.Selling_SGST) / 100);
                                        temp_Bill.CGST = products_For_Sale.CGST;
                                        temp_Bill.SGST = products_For_Sale.SGST;
                                        temp_Bill.Selling_CGST = products_For_Sale.Selling_CGST;
                                        temp_Bill.Selling_SGST = products_For_Sale.Selling_SGST;
                                    }
                                    temp_Bill.Amout_after_tax = decimal.Zero;

                                    temp_Bill.Approve_date = DateTime.Now;
                                    temp_Bill.Approve = true;
                                    temp_Bill.Delivery_contact_number = 0;
                                    
                                    temp_Bill.User_Id = User.Identity.Name;
                                    
                                    db.Temp_Bill.Add(temp_Bill);

                                    await db.SaveChangesAsync();

                                    return Created("", temp_Bill);
                                }
                                else
                                {
                                    return BadRequest("Id or name should not be blank.");
                                }



                            }
                            else
                            {
                                var snglerrr = string.Empty;
                                //snglerrr = "-----------Error------------------";
                                foreach (var errr in ModelState.Values)
                                {

                                    for (int i = 0; i < errr.Errors.Count; i++)
                                    {
                                        int j = i + 1;
                                        if (string.IsNullOrEmpty(errr.Errors[i].ErrorMessage) && string.IsNullOrEmpty(errr.Errors[i].Exception.Message))
                                        {
                                            if (string.IsNullOrEmpty(snglerrr))
                                                snglerrr = "Something went wrong. Please try again";

                                        }
                                        else
                                        {
                                            if (string.IsNullOrEmpty(snglerrr))
                                            {
                                                if (!string.IsNullOrEmpty(errr.Errors[i].ErrorMessage))
                                                    snglerrr = j + ". " + errr.Errors[i].ErrorMessage;
                                                else if (!string.IsNullOrEmpty(errr.Errors[i].Exception.Message))
                                                    snglerrr = j + ". " + errr.Errors[i].Exception.Message;
                                            }
                                            else
                                                snglerrr = snglerrr + "\n" + j + ". " + errr.Errors[i].ErrorMessage;
                                        }
                                        //errors.Add(snglerrr);
                                    }
                                }

                                return BadRequest(snglerrr);
                            }
                        }
                    }
                }
                else
                {
                    using (EasyBillingEntities db = new EasyBillingEntities())
                    {
                        bool isexist = false;
                        if (!string.IsNullOrEmpty(temp_Bill.Item_tyre_Id))
                            isexist = db.Temp_Bill.Where(z => z.Item_tyre_Id == temp_Bill.Item_tyre_Id).Any();
                        ProductForSaleClass products_For_Sale = (from it in db.Products_For_Sales

                                                                 select new ProductForSaleClass()
                                                                 {

                                                                     Product_Token = it.Product_Token,
                                                                     Product_name = it.Product_name,
                                                                     Tyre_token = it.Tyre_token,
                                                                     Tyre_Size = it.Tyre_Size,
                                                                     Supplier_token = it.Supplier_token,
                                                                     Supplier_name = it.Supplier_name,
                                                                     Date = DateTime.Now,
                                                                     Purchase_Price = it.Purchase_Price,
                                                                     CGST = it.CGST,
                                                                     SGST = it.SGST,
                                                                     Pieces = it.Pieces,
                                                                     Selling_Price = it.Selling_Price,
                                                                     Amout_after_tax = it.Amout_after_tax,
                                                                     Total = it.Total,
                                                                     Approve_date = it.Approve_date,
                                                                     Approve = it.Approve,
                                                                     Administrator_Token_number = it.Administrator_Token_number,
                                                                     Administrator_name = it.Administrator_name,
                                                                     Delivery_contact_number = it.Delivery_contact_number,
                                                                     Delivery_address = it.Delivery_address,
                                                                     Item_tyre_token = it.Item_tyre_token,
                                                                     Item_tyre_Id = it.Item_tyre_Id,
                                                                     Purchase_number = it.Purchase_number,
                                                                     Vehicle_Token = it.Vehicle_Token,
                                                                     Vehicle_type = it.Vehicle_type,
                                                                     Description = it.Description,
                                                                     Tyre_make = it.Tyre_make,
                                                                     Tyre_feel = it.Tyre_feel,
                                                                     Tyre_type = it.Tyre_type,
                                                                     Mac_id = it.Mac_id,
                                                                     requestsend = it.requestsend,
                                                                     Selling_CGST = it.Selling_CGST,
                                                                     Selling_SGST = it.Selling_SGST,
                                                                     CGST_SGST_CHECK = it.CGST_SGST_CHECK,
                                                                     User_Id = it.User_Id,
                                                                     User_name = it.User_name,
                                                                     Approve_user_id = it.Approve_user_id,
                                                                     Approve_user_name = it.Approve_user_name,
                                                                     Rate_update_user_id = it.Rate_update_user_id,
                                                                     Rate_update_user_name = it.Rate_update_user_name

                                                                 }).Where(z => z.Item_tyre_Id == temp_Bill.Item_tyre_Id && z.Pieces > 0).OrderByDescending(z => z.Approve_date).FirstOrDefault();

                        if (isexist)
                        {

                            Temp_Bill products_For_Saletmp = db.Temp_Bill.Where(z => z.Item_tyre_Id == temp_Bill.Item_tyre_Id).FirstOrDefault();
                            products_For_Saletmp.Tyre_number = temp_Bill.Tyre_number;
                            if (temp_Bill.Pieces <= 0)
                            {
                                return BadRequest("Quantity should not be less than or equal 0.");
                            }
                            else
                            {

                                products_For_Saletmp.Pieces = temp_Bill.Pieces;
                                products_For_Saletmp.Total = (products_For_Sale.Selling_Price + (products_For_Sale.Selling_Price * (products_For_Sale.Selling_CGST + products_For_Sale.Selling_SGST) / 100)) * temp_Bill.Pieces;
                                await db.SaveChangesAsync();
                                return Created("", products_For_Saletmp);
                            }
                        }
                        else
                        {
                            temp_Bill.Token_Number = Guid.NewGuid().ToString();

                            if (ModelState.ContainsKey("Date"))
                                ModelState["Date"].Errors.Clear();
                            if (temp_Bill.Pieces <= 0)
                            {
                                return BadRequest("Quantity should not be less than or equal 0.");
                            }

                            if (ModelState.IsValid)
                            {
                                if (!string.IsNullOrEmpty(temp_Bill.Item_tyre_Id))
                                {


                                    temp_Bill.Product_Token = products_For_Sale.Product_Token;
                                    temp_Bill.Product_name = products_For_Sale.Product_name;
                                    temp_Bill.Tyre_token = products_For_Sale.Tyre_token;
                                    temp_Bill.Tyre_Size = products_For_Sale.Tyre_Size;
                                    temp_Bill.Supplier_token = products_For_Sale.Supplier_token;
                                    temp_Bill.Supplier_name = products_For_Sale.Supplier_name;
                                    temp_Bill.Date = DateTime.Now;
                                    temp_Bill.Purchase_Price = products_For_Sale.Purchase_Price;
                                    temp_Bill.CGST = products_For_Sale.CGST;
                                    temp_Bill.SGST = products_For_Sale.SGST;
                                    temp_Bill.Pieces = temp_Bill.Pieces;
                                    temp_Bill.Selling_Price = products_For_Sale.Selling_Price;
                                    temp_Bill.Amout_after_tax = products_For_Sale.Amout_after_tax;
                                    temp_Bill.Total = (products_For_Sale.Selling_Price + (products_For_Sale.Selling_Price * (products_For_Sale.Selling_CGST + products_For_Sale.Selling_SGST) / 100)) * temp_Bill.Pieces;
                                    temp_Bill.Approve_date = products_For_Sale.Approve_date;
                                    temp_Bill.Approve = products_For_Sale.Approve;
                                    temp_Bill.Administrator_Token_number = products_For_Sale.Administrator_Token_number;
                                    temp_Bill.Administrator_name = products_For_Sale.Administrator_name;
                                    temp_Bill.Delivery_contact_number = products_For_Sale.Delivery_contact_number;
                                    temp_Bill.Delivery_address = products_For_Sale.Delivery_address;
                                    temp_Bill.Item_tyre_token = products_For_Sale.Item_tyre_token;
                                    temp_Bill.Item_tyre_Id = products_For_Sale.Item_tyre_Id;
                                    temp_Bill.Purchase_number = products_For_Sale.Purchase_number;
                                    temp_Bill.Vehicle_Token = products_For_Sale.Vehicle_Token;
                                    temp_Bill.Vehicle_type = products_For_Sale.Vehicle_type;
                                    temp_Bill.Description = products_For_Sale.Description;
                                    temp_Bill.Tyre_make = products_For_Sale.Tyre_make;
                                    temp_Bill.Tyre_feel = products_For_Sale.Tyre_feel;
                                    temp_Bill.Tyre_type = products_For_Sale.Tyre_type;
                                    temp_Bill.Mac_id = products_For_Sale.Mac_id;
                                    temp_Bill.requestsend = products_For_Sale.requestsend;
                                    temp_Bill.Selling_CGST = products_For_Sale.Selling_CGST;
                                    temp_Bill.Selling_SGST = products_For_Sale.Selling_SGST;
                                    temp_Bill.CGST_SGST_CHECK = products_For_Sale.CGST_SGST_CHECK;
                                    temp_Bill.User_Id = products_For_Sale.User_Id;
                                    temp_Bill.User_name = products_For_Sale.User_name;
                                    temp_Bill.Approve_user_id = products_For_Sale.Approve_user_id;
                                    temp_Bill.Approve_user_name = products_For_Sale.Approve_user_name;
                                    temp_Bill.Rate_update_user_id = products_For_Sale.Rate_update_user_id;
                                    temp_Bill.Rate_update_user_name = products_For_Sale.Rate_update_user_name;

                                    db.Temp_Bill.Add(temp_Bill);

                                    await db.SaveChangesAsync();

                                    return Created("", temp_Bill);
                                }
                                else
                                {
                                    return BadRequest("Id or name should not be blank.");
                                }



                            }
                            else
                            {
                                var snglerrr = string.Empty;
                                //snglerrr = "-----------Error------------------";
                                foreach (var errr in ModelState.Values)
                                {

                                    for (int i = 0; i < errr.Errors.Count; i++)
                                    {
                                        int j = i + 1;
                                        if (string.IsNullOrEmpty(errr.Errors[i].ErrorMessage))
                                        {
                                            if (string.IsNullOrEmpty(snglerrr))
                                                snglerrr = "Please select date.";

                                        }
                                        else
                                        {
                                            if (string.IsNullOrEmpty(snglerrr))
                                                snglerrr = j + ". " + errr.Errors[i].ErrorMessage;
                                            else
                                                snglerrr = snglerrr + "\n" + j + ". " + errr.Errors[i].ErrorMessage;
                                        }
                                        //errors.Add(snglerrr);
                                    }
                                }

                                return BadRequest(snglerrr);
                            }

                        }
                    }
                }
            }catch(Exception ex)
                        {

                return BadRequest(ex.Message);
                        }
}
        //[HttpPut]
        //[Route("")]
        //public async Task<IHttpActionResult> Save([FromBody] Products_For_Sale products_For_Sale)
        //{
        //    using (EasyBillingEntities db = new EasyBillingEntities())
        //    {
        //        products_For_Sale.Token_Number = Guid.NewGuid().ToString();
        //        if (ModelState.IsValid)
        //        {
        //            if(products_For_Sale.Pieces<=0 )
        //            {
        //                return BadRequest("Stock should not be less than or equal 0.");
        //            }

        //            if (!string.IsNullOrEmpty(products_For_Sale.Item_tyre_Id))
        //            {
        //                var values = db.Item_Tyres.Where(z => z.Item_Id == products_For_Sale.Item_tyre_Id).Select(z => new { z.Company_name, z.Tyre_type, z.Tyre_feel, z.Tyre_make, z.Tyre_size, z.Vehicle_type }).Distinct().FirstOrDefault();
        //                products_For_Sale.Product_name = values.Company_name;
        //                products_For_Sale.Tyre_feel = values.Tyre_feel;
        //                products_For_Sale.Tyre_Size = values.Tyre_size;
        //                products_For_Sale.Tyre_make = values.Tyre_make;
        //                products_For_Sale.Tyre_type = values.Tyre_type;
        //                products_For_Sale.Vehicle_type = values.Vehicle_type;
        //            }else
        //            {
        //                return BadRequest("Item id or company name should not be blank.");
        //            }
        //            products_For_Sale.Tyre_token = db.Tyre_sizes.Where(z => z.Tyre_size1 == products_For_Sale.Tyre_Size).Select(z => z.Token_number).Distinct().FirstOrDefault();
        //            products_For_Sale.Vehicle_Token = db.Vehicles.Where(z => z.Vehicle_type == products_For_Sale.Vehicle_type).Select(z => z.Token_number).Distinct().FirstOrDefault();

        //            products_For_Sale.Item_tyre_token = db.Item_Tyres.Where(z => z.Item_Id == products_For_Sale.Item_tyre_Id).Select(z => z.Token_number).Distinct().FirstOrDefault();
        //            if (string.IsNullOrEmpty(products_For_Sale.Product_Token))
        //            {
        //                products_For_Sale.Product_Token = db.Products.Where(z => z.Product_name == products_For_Sale.Product_name).Select(z => z.Token_Number).FirstOrDefault();
        //            }

        //            //bool checkitemid = db.Products_For_Sales.Where(z => z.Item_tyre_Id == products_For_Sale.Item_tyre_Id).Any();
        //           // products_For_Sale.Item_tyre_Id = db.Item_Tyres.Where(z => z.Token_number == products_For_Sale.Item_tyre_token).Select(z => z.Item_Id).FirstOrDefault();
        //            if (string.IsNullOrEmpty(products_For_Sale.Item_tyre_token))
        //            {
        //                return BadRequest("Item tyre Id should be seleted.");
        //            }
        //            else if (string.IsNullOrEmpty(products_For_Sale.Purchase_number))
        //            {
        //                return BadRequest("Purchase number should not be empty.");
        //            }
        //            else
        //            {
        //                products_For_Sale.Approve = false;
        //                products_For_Sale.Supplier_token = (from sup in db.Dealers
        //                                                   where sup.Name == products_For_Sale.Supplier_name
        //                                                   select sup.Token_number).FirstOrDefault();
        //                if(products_For_Sale.Date!=null)
        //                products_For_Sale.Date = DateTime.Parse(products_For_Sale.Date.ToString("dd-MM-yyyy HH:mm:ss"));
        //                else
        //                    products_For_Sale.Date = DateTime.Parse(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));
        //                products_For_Sale.Selling_Price = 0;
        //                products_For_Sale.Up_Selling_Price = 0;
        //               // products_For_Sale.Total = 0;
        //                //products_For_Sale.Tyre_Size = (from sup in db.Tyre_sizes
        //                //                               where sup.Token_number == products_For_Sale.Tyre_token
        //                //                               select sup.Tyre_size1).FirstOrDefault();

        //                //products_For_Sale.Vehicle_type = (from sup in db.Vehicles
        //                //                                  where sup.Token_number == products_For_Sale.Vehicle_Token
        //                //                                  select sup.Vehicle_type).FirstOrDefault();

        //                Purchase_Invoice purchase_Invoice = new Purchase_Invoice();

        //                purchase_Invoice.Token_number = Guid.NewGuid().ToString();
        //                purchase_Invoice.Purchase_invoice_number = products_For_Sale.Purchase_number;
        //                bool chkinv = db.Purchase_Invoices.Where(z => z.Purchase_invoice_number == purchase_Invoice.Purchase_invoice_number).Any();
        //                if (chkinv == true)
        //                {
        //                    return BadRequest(purchase_Invoice.Purchase_invoice_number + " number already exist. Try with another.");
        //                }
        //                else
        //                {

        //                    purchase_Invoice.Date = products_For_Sale.PurchaseDate;
        //                    purchase_Invoice.Stock_entry_token = products_For_Sale.Token_Number;
        //                    NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
        //                    String sMacAddress = string.Empty;
        //                    foreach (NetworkInterface adapter in nics)
        //                    {
        //                        IPInterfaceProperties properties = adapter.GetIPProperties();
        //                        sMacAddress = adapter.GetPhysicalAddress().ToString();
        //                        if (!string.IsNullOrEmpty(sMacAddress))// only return MAC Address from first card  
        //                            break;
        //                    }
        //                    products_For_Sale.Mac_id = sMacAddress;
        //                    // products_For_Sale.Product = db.Products.Where(x=>x.Token_Number==products_For_Sale.Product_Token).FirstOrDefault();
        //                    db.Products_For_Sales.Add(products_For_Sale);
        //                    db.Purchase_Invoices.Add(purchase_Invoice);
        //                    await db.SaveChangesAsync();

        //                    return Created("", products_For_Sale);
        //                }
        //            }
        //        }
        //        else
        //        {
        //            var snglerrr = string.Empty;
        //            //snglerrr = "-----------Error------------------";
        //            foreach (var errr in ModelState.Values)
        //            {

        //                for (int i = 0; i < errr.Errors.Count; i++)
        //                {
        //                    int j = i + 1;
        //                    if (string.IsNullOrEmpty(errr.Errors[i].ErrorMessage))
        //                    {
        //                        if (string.IsNullOrEmpty(snglerrr))
        //                            snglerrr = "Something went wrong. Please check your data and try again.";

        //                    }
        //                    else
        //                    {
        //                        if (string.IsNullOrEmpty(snglerrr))
        //                            snglerrr = j + ". " + errr.Errors[i].ErrorMessage;
        //                        else
        //                            snglerrr = snglerrr + "\n" + j + ". " + errr.Errors[i].ErrorMessage;
        //                    }
        //                    //errors.Add(snglerrr);
        //                }
        //            }

        //            return BadRequest(snglerrr);
        //        }
        //    }
        //}

        [HttpGet]
        [Route("{id}")]
        public async Task<IHttpActionResult> GetById([FromUri] string id)
        {
            using (EasyBillingEntities db = new EasyBillingEntities())
            {
                return await Task.FromResult(Ok(db.Products_For_Sales.Where(z => z.Token_Number == id).Distinct().FirstOrDefault()));
            }
        }


        [HttpPost]
        [Route("{id}")]
        public async Task<IHttpActionResult> Delete([FromUri]string id)
        {
            using (EasyBillingEntities db = new EasyBillingEntities())
            {
                Temp_Stock products_For_Sale = db.Temp_Stock.Where(z => z.Token_Number == id).Distinct().FirstOrDefault();
               // Purchase_Invoice purchase_Invoice = db.Purchase_Invoices.Where(z => z.Purchase_invoice_number == products_For_Sale.Purchase_number).Distinct().FirstOrDefault();
                if (products_For_Sale != null)
                {
                    db.Temp_Stock.Remove(products_For_Sale);
                   // db.Purchase_Invoices.Remove(purchase_Invoice);
                    await db.SaveChangesAsync();
                    ModelState.Clear();
                    return Ok();
                }
              
                return BadRequest($"Invalid token number {id}");
            }
        }
        [HttpPost]
        [Route("DeleteBill")]
        public async Task<IHttpActionResult> DeleteBill([FromBody]Temp_Bill temp_Bill)
        {
            using (EasyBillingEntities db = new EasyBillingEntities())
            {
                Temp_Bill products_For_Sale = db.Temp_Bill.Where(z => z.Token_Number == temp_Bill.Token_Number).Distinct().FirstOrDefault();
                // Purchase_Invoice purchase_Invoice = db.Purchase_Invoices.Where(z => z.Purchase_invoice_number == products_For_Sale.Purchase_number).Distinct().FirstOrDefault();
                if (products_For_Sale != null)
                {
                    db.Temp_Bill.Remove(products_For_Sale);
                    // db.Purchase_Invoices.Remove(purchase_Invoice);
                    await db.SaveChangesAsync();
                    ModelState.Clear();
                    return Ok();
                }

                return BadRequest($"Invalid token number {temp_Bill.Token_Number}");
            }
        }
        [HttpPost]
        [Route("Deletesel")]
        public IHttpActionResult Deletesel(List<string> list)
        {

            using (var ctx = new EasyBillingEntities())
            {
                ctx.Temp_Stock.RemoveRange(ctx.Temp_Stock.Where(x => list.Contains(x.Token_Number)).ToList());
                ctx.SaveChanges();
            
                return Ok();

            }


        }
    }
}
