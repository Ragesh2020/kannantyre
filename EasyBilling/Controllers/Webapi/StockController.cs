using CrystalDecisions.CrystalReports.Engine;
using EasyBilling.Models;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Hosting;
using System.Web.Http;


namespace EasyBilling.Controllers.Webapi
{
    public class StockController : ApiController
    {
        public IHttpActionResult GetAllStockData()
        {
            IList<StockClass> stock = null;

            using (var ctx = new EasyBillingEntities())
            {
                var dcml =decimal.Parse("0.000");

                stock = ctx.Stocks.Select(s => new StockClass()
                {
                    Date = s.Date,
                    Pieces = s.Pieces,
                    Product_name = (from a in ctx.Products
                                    where a.Token_Number == s.Product_Token
                                    select a.Product_name).Distinct().FirstOrDefault(),

                    Product_Token = s.Product_Token,
                    Purcahse_number = s.Purcahse_number,
                    Purchase_Token_number = s.Purchase_Token_number,
                  
                    Stock_Id = s.Stock_Id,
                  
                    CGST = s.CGST,
                    SGST=s.SGST
                   

                }).Where(x=>x.Pieces!=0 ).ToList();


            }


            if (stock.Count == 0)
            {
                return NotFound();
            }
            return Ok(stock);
        }

        public HttpResponseMessage GetStockInhands(int stckid)
        {
            try
            {
                List<Products_For_Sale> prd = new List<Products_For_Sale>();
                EasyBillingEntities db = new EasyBillingEntities();


                if (stckid != 0)
                {


                    var stks = db.Stocks.Select(a => new { a.Pieces,  a.Product_Token, a.Stock_Id })
                               .Where(p => p.Stock_Id == stckid).ToList();
                    foreach (var i in stks)
                    {
                        var prdcts = db.Products_For_Sales.Where(x => x.Token_Number == i.Product_Token).Distinct().FirstOrDefault();
                        prd.Add(prdcts);
                    }
                    var mrch = db.Marchent_Accounts.Select(a => new { a.CIN_Number, a.GSTIN_Number, a.Marchent_name, a.Pan_Number, a.State_Name }).ToList();
                    ReportDocument rpt = new ReportDocument();

                    rpt.Load(Path.Combine(HostingEnvironment.MapPath("~/Reportsdataset/AllReports"), "StockReport.rpt"));
                    rpt.Database.Tables[0].SetDataSource(stks);
                  
                    rpt.Database.Tables[1].SetDataSource(mrch);
                    rpt.Database.Tables[2].SetDataSource(prd);
                    Stream s = rpt.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    var response = new HttpResponseMessage(HttpStatusCode.OK);

                    response.Content = new StreamContent(s);
                    response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf");

                    return response;
                }
                else
                {
                    var stks = db.Stocks.Select(a => new { a.Pieces, a.Product_Token })
                                .ToList();

                    foreach (var i in stks)
                    {
                        var prdcts = db.Products_For_Sales.Where(x => x.Token_Number == i.Product_Token).Distinct().FirstOrDefault();
                        prd.Add(prdcts);
                    }
                    var mrch = db.Marchent_Accounts.Select(a=>new { a.CIN_Number,a.GSTIN_Number,a.Marchent_name,a.Pan_Number,a.State_Name }).ToList();
                    ReportDocument rpt = new ReportDocument();

                    rpt.Load(Path.Combine(HostingEnvironment.MapPath("~/Reportsdataset/AllReports"), "StockReport.rpt"));
                    rpt.Database.Tables[0].SetDataSource(stks);
                  
                    rpt.Database.Tables[1].SetDataSource(mrch);
                    rpt.Database.Tables[2].SetDataSource(prd);
                    Stream s = rpt.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    var response = new HttpResponseMessage(HttpStatusCode.OK);

                    response.Content = new StreamContent(s);
                    response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf");

                    return response;
                }



                // rpt.Database.Tables[1].SetDataSource(dsCustomers.Tables["Table1"]);

                //rpt.SetDataSource(rpt);


            }
            catch
            {
                var response = new HttpResponseMessage(HttpStatusCode.NotFound);
                return response;
            }

        }
    }
}
