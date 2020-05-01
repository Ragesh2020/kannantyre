using CrystalDecisions.CrystalReports.Engine;
using EasyBilling.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Mvc;

namespace EasyBilling.Controllers.Webapi
{
    public class BillingController : ApiController
    {
        [System.Web.Http.ActionName("Delete")]
        [System.Web.Http.HttpDelete]
        public IHttpActionResult Delete(JObject jsonData)
        {
            dynamic json = jsonData;
            string token = json.Token_number;
           
            using (var ctx = new EasyBillingEntities())
            {
                var data = ctx.Temp_placedorder.Where(z => z.Token_number == token).Distinct().FirstOrDefault();
                if(data!=null)
                {
                    ctx.Temp_placedorder.Remove(data);
                        ctx.SaveChanges();
                        return Ok();
                   
                }
                else
                {
                    return BadRequest();
                }

            }
        }

        [System.Web.Http.ActionName("Postadding")]
        [System.Web.Http.HttpPost]
        public IHttpActionResult Postadding(JObject jsonData)
        {
            dynamic json = jsonData;
            string token = json.Token_number;
            int Pieces = json.Pieces;
            using (var ctx = new EasyBillingEntities())
            {
                var data = ctx.Temp_placedorder.Where(z => z.Token_number == token).Distinct().FirstOrDefault();
                var datamaxpieces = ctx.Stocks.Where(z => z.Product_Token == data.Item_token).Select(z=>z.Pieces).Distinct().FirstOrDefault();
                if (datamaxpieces >= Pieces)
                {
                    data.Pieces = Pieces;
                    if (Pieces >= 0)
                    {
                        ctx.SaveChanges();
                        return Ok();
                    }
                    else
                    {
                        return BadRequest();
                    }
                }else
                {
                    return BadRequest();
                }
               
            }
        }
                public IHttpActionResult GetBillingData(string id)
        {
            BillingMasterClass BM = null;
            using (var ctx = new EasyBillingEntities())
            {
                var pmexits = ctx.Billing_Masters.Select(s => new BillingMasterClass()
                {
                    Billing_Number = s.Billing_Number
                }).Where(a => a.Billing_Number == id).Any();

                if (pmexits == true)
                {
                    BM = ctx.Billing_Masters.Select(s => new BillingMasterClass()
                    {
                        Billing_Number = s.Billing_Number,

                      
                    }).Where(a => a.Billing_Number == id).FirstOrDefault();
                }
            }
            if (BM == null)
            {
                return NotFound();
            }
            return Ok(BM);
        }

        public IHttpActionResult GetBillinglastId()
        {
           List<BillingMasterClass> BMlist = new List<BillingMasterClass>();
           
            BillingMasterClass BMfnal = null;
            using (var ctx = new EasyBillingEntities())
            {
                var pmexits = ctx.Billing_Masters.Select(s => new BillingMasterClass()
                {
                    Billing_Number = s.Billing_Number
                }).Any();
                var pmexitsforquot = ctx.Quotation_Masters.Select(s => new BillingMasterClass()
                {
                    Billing_Number = s.Quotation_Number
                }).Any();
                var pmexitsforord = ctx.Order_Masters.Select(s => new BillingMasterClass()
                {
                    Billing_Number = s.Order_Number
                }).Any();
                if (pmexits == true)
                {
                    BMlist.Add(ctx.Billing_Masters.Select(s => new BillingMasterClass()
                    {
                        Billing_Number = s.Billing_Number
                    }).OrderByDescending(o => o.Billing_Number).FirstOrDefault());
                    
                }
                if (pmexitsforquot == true)
                {
                    BMlist.Add(ctx.Quotation_Masters.Select(s => new BillingMasterClass()
                    {
                        Billing_Number = s.Quotation_Number
                    }).OrderByDescending(o => o.Billing_Number).FirstOrDefault());
                   
                }
                if (pmexitsforord == true)
                {
                    BMlist.Add(ctx.Order_Masters.Select(s => new BillingMasterClass()
                    {
                        Billing_Number = s.Order_Number
                    }).OrderByDescending(o => o.Billing_Number).FirstOrDefault());
                }
                BMfnal = BMlist.OrderByDescending(o => o.Billing_Number).FirstOrDefault();
            }
            if (BMfnal == null)
            {
                return NotFound();
            }
            return Ok(BMfnal);
        }

        public IHttpActionResult PostNewMerchant([FromBody]Billing_Master billing)
        {
            bool status = false;
            Barcode_Master brcdmstr = new Barcode_Master();
            if (!ModelState.IsValid)
                return BadRequest("Sorry there is some problem. Please check and try again");

            using (EasyBillingEntities dc = new EasyBillingEntities())
            {
                var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

                Random rd = new Random();

                var chars1 = Enumerable.Range(0, 14)
           .Select(x => chars[rd.Next(0, chars.Length)]);
                string barcode = new string(chars1.ToArray());

                billing.Token_Number = (Guid.NewGuid()).ToString();

                var dtls = billing.Billing_Details.Distinct().ToList();
                //foreach (var echdtls in dtls)
                //{
                //    bool chk = dc.Stocks.Where(a => a.Product_Token == echdtls.Product_Token).Any();
                //    if (chk == true)
                //    {
                //        Stock stk = dc.Stocks.Where(a => a.Product_Token == echdtls.Product_Token).FirstOrDefault();
                //        stk.Date = echdtls.Date;
                //        stk.Pieces = stk.Pieces - echdtls.Pieces;
                      
                //        stk.Product_Token = echdtls.Product_Token;
                       
                //        if (stk.Pieces < 0 )
                //        {
                //            return BadRequest("Sorry there is some problem. Please check and try again");
                //        }
                //        dc.SaveChanges();

                //    }
                //    else
                //    {
                //        return NotFound();
                //    }
                //}

                brcdmstr.Barcode_Number = barcode;
                brcdmstr.Billing_Number = billing.Billing_Number;
                brcdmstr.Billing_Token_number = billing.Token_Number;
                brcdmstr.Date = billing.Date;

             

                // image save for barcode///

                using (MemoryStream ms = new MemoryStream())
                {
                    //The Image is drawn based on length of Barcode text.
                    using (Bitmap bitMap = new Bitmap(barcode.Length * 30, 90))
                    {
                        //The Graphics library object is generated for the Image.
                        using (Graphics graphics = Graphics.FromImage(bitMap))
                        {
                            //The installed Barcode font.
                            Font oFont = new Font("IDAutomationHC39M", 17);
                            PointF point = new PointF(2f, 2f);

                            //White Brush is used to fill the Image with white color.
                            SolidBrush whiteBrush = new SolidBrush(Color.White);
                            graphics.FillRectangle(whiteBrush, 0, 0, bitMap.Width, bitMap.Height);

                            //Black Brush is used to draw the Barcode over the Image.
                            SolidBrush blackBrush = new SolidBrush(Color.Black);
                            graphics.DrawString("*" + barcode + "*", oFont, blackBrush, point);
                        }

                        //The Bitmap is saved to Memory Stream.
                        bitMap.Save(ms, ImageFormat.Png);

                        //The Image is finally converted to Base64 string.


                        byte[] imageBytes = Convert.FromBase64String(Convert.ToBase64String(ms.ToArray()));

                        MemoryStream ms1 = new MemoryStream(imageBytes, 0, imageBytes.Length);
                        ms1.Write(imageBytes, 0, imageBytes.Length);

                        System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
                        string spath = System.Web.HttpContext.Current.Server.MapPath("~/BillBarcode");
                        string path = spath + "/" + billing.Billing_Number + ".jpg";
                        image.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);

                        //p.photo = imageBytes;
                       
                        brcdmstr.Image = imageBytes;
                    }

                }

                billing.Barcode_Master.Add(brcdmstr);

                dc.Billing_Masters.Add(billing);
                dc.SaveChanges();
                status = true;
            }

            return Ok();
        }

        public IHttpActionResult PostProductForBill1([FromBody]FormCollection frm, [FromUri] FormDataCollection frm1)
        {
            bool status = false;

            Billing_Master billing = new Billing_Master();
            List<Billing_Detail> detail = new List<Billing_Detail>();
            Billing_Detail blldtl = new Billing_Detail();
            Stockout stkout = new Stockout();
            List<Stockout> stkoutlst = new List<Stockout>();

            Barcode_Master brcdmstr = new Barcode_Master();
            if (!ModelState.IsValid)
                return BadRequest("Sorry there is some problem. Please check and try again");

            using (EasyBillingEntities dc = new EasyBillingEntities())
            {
                var text = dc.Billing_Masters.OrderByDescending(x => x.Billing_Number).Select(x => x.Billing_Number).Distinct().FirstOrDefault();
                var fstfr = text.Substring(0, 3);
                var lstfr = text.Substring(text.Length - 8);
                string newlstversn = (int.Parse(lstfr) + 100000001).ToString();
                string fstfr1 = (newlstversn.Substring(newlstversn.Length - 8)).ToString();
                String totalvrsn = fstfr + fstfr1;
                billing.Billing_Number = totalvrsn;

                var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

                Random rd = new Random();

                var chars1 = Enumerable.Range(0, 14)
           .Select(x => chars[rd.Next(0, chars.Length)]);
                string barcode = new string(chars1.ToArray());

                billing.Token_Number = (Guid.NewGuid()).ToString();
                billing.Date = DateTime.Now.Date;
                var mrchnttk = (from tkn in dc.Marchent_Accounts
                                where tkn.Email_Id == User.Identity.Name
                                select tkn.Token_number).Distinct().FirstOrDefault();
               // billing.Marchent_Token_number = mrchnttk;
                var chkst = (from tkn in dc.Marchent_Accounts
                             join st in dc.States on tkn.State_Code equals st.State_Code
                             where tkn.Email_Id == User.Identity.Name
                             select new { st.SGST, st.CGST, st.IGST, st.UTGST }).Distinct().FirstOrDefault();

                billing.Total_tax = decimal.Parse("0.00");
                billing.Rate_including_tax = decimal.Parse("0.00");
               // billing.Discount_percent = decimal.Parse("0.00");
                billing.Total_discount = decimal.Parse("0.00");
                billing.Total_amount = decimal.Parse("0.00");
               // billing.CGST = decimal.Parse("0.00");
               // billing.SGST = decimal.Parse("0.00");
               
                int b = 1;
                int l = 0;

                foreach (var key in frm.Keys)
                {
                    if (l % 2 == 0)
                    {
                        var k = key.ToString();

                        int a = int.Parse(k.Substring(k.Length - 1, 1));
                        if (a == b)
                        { }
                        else
                        {
                            b = a;
                        }
                        var Token = frm[a - 1];
                        var Sellon = frm[a];
                        var Quant = int.Parse(frm[a + 1]);

                        var stockitems = dc.Stocks.Where(x => x.Product_Token == Token).Distinct().FirstOrDefault();
                        var productforsaleitems = dc.Products_For_Sales.Where(x => x.Token_Number == Token).Distinct().FirstOrDefault();


                        if (stockitems != null)
                        {
                            
                                stockitems.Pieces = stockitems.Pieces - (productforsaleitems.Pieces * Quant);
                           
                            stockitems.Date = DateTime.Now.Date;

                            if (stockitems.Pieces < 0 )
                            {
                                ModelState.AddModelError(string.Empty, "Please check your quantity");
                            }
                            else
                            {
                                dc.SaveChanges();
                            }
                        }

                        billing.Rate_including_tax = billing.Rate_including_tax + productforsaleitems.Amout_after_tax;
                        billing.Total_discount = billing.Total_discount ;
                        billing.Total_amount = billing.Total_amount + productforsaleitems.Total;
                        billing.Total_tax = billing.Total_tax + (productforsaleitems.Amout_after_tax - productforsaleitems.Selling_Price);

                        blldtl.Billing_Token_number = billing.Token_Number;
                        blldtl.Billing_number = billing.Billing_Number;
                        blldtl.Date = billing.Date;
                  //      blldtl.Product_Token = Token;
                        blldtl.Pieces = productforsaleitems.Pieces * Quant;
                       
                        blldtl.Amount = productforsaleitems.Selling_Price;
                    //    blldtl.Taxable_amount = productforsaleitems.Pieces * productforsaleitems.Selling_Price;
                        blldtl.Tax = productforsaleitems.Amout_after_tax - productforsaleitems.Selling_Price;
                        blldtl.Discount = decimal.Parse("0.00");
                      //  blldtl.Discount_percent = ((productforsaleitems.Amout_after_tax - productforsaleitems.Total) * 100) / productforsaleitems.Amout_after_tax;
                        blldtl.Sub_Total = productforsaleitems.Total;

                        stkout.Billing_Token_number = billing.Token_Number;
                        stkout.Billing_number = billing.Billing_Number;
                        stkout.Date = billing.Date;
                        stkout.Product_Token = Token;
                        stkout.Pieces = productforsaleitems.Pieces * Quant;
                   
                        stkout.CGST = (productforsaleitems.Selling_Price * Quant) + ((productforsaleitems.CGST * Quant) / 100);
                        stkout.SGST = (productforsaleitems.Selling_Price * Quant) + ((productforsaleitems.SGST * Quant) / 100);
                        stkout.Sub_Total = productforsaleitems.Total;
                        stkout.Marchent_Token_number = mrchnttk;

                        detail.Add(blldtl);
                        stkoutlst.Add(stkout);

                    }
                    l++;
                }
                //if (chkst.SGST == true && chkst.CGST == true)
                //{
                //    billing.CGST = billing.Total_tax / 2;
                //    billing.SGST = billing.CGST;
                //}
                //else if (chkst.CGST == true && chkst.IGST == true)
                //{
                //    billing.CGST = billing.Total_tax / 2;
                   
                //}
                //else if (chkst.CGST == true && chkst.UTGST == true)
                //{
                //    billing.CGST = billing.Total_tax / 2;
                    
                //}
                //else if (chkst.SGST == true && chkst.IGST == true)
                //{
                //    billing.SGST = billing.Total_tax / 2;
                 
                //}
                //else if (chkst.SGST == true && chkst.UTGST == true)
                //{
                //    billing.SGST = billing.Total_tax / 2;
                  
                //}
               
                //else
                //{ }

                brcdmstr.Barcode_Number = barcode;
                brcdmstr.Billing_Number = billing.Billing_Number;
                brcdmstr.Billing_Token_number = billing.Token_Number;
                brcdmstr.Date = billing.Date;

                //billing.Barcode_Number = barcode;

                // image save for barcode///

                using (MemoryStream ms = new MemoryStream())
                {
                    //The Image is drawn based on length of Barcode text.
                    using (Bitmap bitMap = new Bitmap(barcode.Length * 30, 90))
                    {
                        //The Graphics library object is generated for the Image.
                        using (Graphics graphics = Graphics.FromImage(bitMap))
                        {
                            //The installed Barcode font.
                            Font oFont = new Font("IDAutomationHC39M", 17);
                            PointF point = new PointF(2f, 2f);

                            //White Brush is used to fill the Image with white color.
                            SolidBrush whiteBrush = new SolidBrush(Color.White);
                            graphics.FillRectangle(whiteBrush, 0, 0, bitMap.Width, bitMap.Height);

                            //Black Brush is used to draw the Barcode over the Image.
                            SolidBrush blackBrush = new SolidBrush(Color.Black);
                            graphics.DrawString("*" + barcode + "*", oFont, blackBrush, point);
                        }

                        //The Bitmap is saved to Memory Stream.
                        bitMap.Save(ms, ImageFormat.Png);

                        //The Image is finally converted to Base64 string.


                        byte[] imageBytes = Convert.FromBase64String(Convert.ToBase64String(ms.ToArray()));

                        MemoryStream ms1 = new MemoryStream(imageBytes, 0, imageBytes.Length);
                        ms1.Write(imageBytes, 0, imageBytes.Length);

                        System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
                        string spath = System.Web.HttpContext.Current.Server.MapPath("~/BillBarcode");
                        string path = spath + "/" + billing.Billing_Number + ".jpg";
                        image.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);

                        //p.photo = imageBytes;
                  //      billing.Barcode_photo = imageBytes;
                        brcdmstr.Image = imageBytes;
                    }

                }
                billing.Billing_Details = detail;
                billing.Stockouts = stkoutlst;
                billing.Barcode_Master.Add(brcdmstr);

                dc.Billing_Masters.Add(billing);
                dc.SaveChanges();
                status = true;
            }

            return Ok();
        }
        public IHttpActionResult PostProductBill([FromBody]Billing_Master billing)
        {
            bool status = false;
            Barcode_Master brcdmstr = new Barcode_Master();
            
            using (EasyBillingEntities dc = new EasyBillingEntities())
            {
                var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

                Random rd = new Random();

                var chars1 = Enumerable.Range(0, 14)
           .Select(x => chars[rd.Next(0, chars.Length)]);
                string barcode = new string(chars1.ToArray());

                billing.Token_Number = (Guid.NewGuid()).ToString();
                bool ischk = dc.Billing_Masters.Any();
                if (ischk == false)
                {
                    billing.Billing_Number = "INV00000001";
                }
                else
                {
                    var text = (from a in dc.Billing_Masters
                                orderby a.Billing_Number descending
                                select a.Billing_Number).FirstOrDefault();

                    var fstfr = text.Substring(0, 3);
                    var lstfr = text.Substring(text.Length - 8);
                    string newlstversn = (int.Parse(lstfr) + 100000001).ToString();
                    string fstfr1 = (newlstversn.Substring(newlstversn.Length - 8)).ToString();
                    String totalvrsn = fstfr + fstfr1;
                    billing.Billing_Number = totalvrsn;
                }
                billing.Date = DateTime.Now.Date;
                var mrchnttk = (from tkn in dc.Marchent_Accounts
                                where tkn.Email_Id == "souravganguly707@gmail.com" ||tkn.Email_Id== "kannantyres@kannantyres.com"
                                select tkn.Token_number).Distinct().FirstOrDefault();
                //billing.Marchent_Token_number = mrchnttk;
                var chkst = (from tkn in dc.Marchent_Accounts
                             join st in dc.States on tkn.State_Code equals st.State_Code
                             where tkn.Email_Id == "souravganguly707@gmail.com" || tkn.Email_Id == "kannantyres@kannantyres.com"
                             select new { st.SGST, st.CGST, st.IGST, st.UTGST }).Distinct().FirstOrDefault();
                //if (chkst.SGST == true && chkst.CGST == true)
                //{
                //    billing.CGST = billing.Total_tax / 2;
                //    billing.SGST = billing.CGST;
                //}
                //else if (chkst.CGST == true && chkst.IGST == true)
                //{
                //    billing.CGST = billing.Total_tax / 2;
                  
                //}
                //else if (chkst.CGST == true && chkst.UTGST == true)
                //{
                //    billing.CGST = billing.Total_tax / 2;
               
                //}
                //else if (chkst.SGST == true && chkst.IGST == true)
                //{
                //    billing.SGST = billing.Total_tax / 2;
                  
                //}
                //else if (chkst.SGST == true && chkst.UTGST == true)
                //{
                //    billing.SGST = billing.Total_tax / 2;
                   
                //}
              
                //else
                //{ }
                billing.Narration = "Thank you for purchase. Please visit our shop again...";

                var dtls = billing.Billing_Details.Distinct().ToList();
                var stkouts = billing.Stockouts.Distinct().ToList();
                
                //foreach (var echdtls in dtls)
                //{
                //    Products_For_Sale prdct = dc.Products_For_Sales.Where(a => a.Token_Number == echdtls.Product_Token).Distinct().FirstOrDefault();
                   
                //        if(prdct != null)
                //        {
                            
                //            echdtls.Billing_Token_number = billing.Token_Number;
                //            echdtls.Billing_Token_number = billing.Billing_Number;
                //            echdtls.Date = DateTime.Now.Date;
                //          //  echdtls.Product_Token = prdct.Token_Number;
                //            if(echdtls.Pieces==prdct.Pieces)
                //            {
                //                billing.Rate_including_tax = billing.Rate_including_tax + prdct.Amout_after_tax;
                //                echdtls.Amount = prdct.Selling_Price;
                //            //    echdtls.Discount_percent = (((prdct.Amout_after_tax) - (prdct.Total)) * 100) / (prdct.Amout_after_tax);
                               
                //            }
                //            else
                //            {
                //                int pcsincrs = echdtls.Pieces / prdct.Pieces;
                //                billing.Rate_including_tax = billing.Rate_including_tax = billing.Rate_including_tax + (prdct.Amout_after_tax * pcsincrs);
                //                echdtls.Amount = prdct.Selling_Price*pcsincrs;
                //              //  echdtls.Discount_percent = (((prdct.Amout_after_tax * pcsincrs) - (prdct.Total * pcsincrs)) * 100) / (prdct.Amout_after_tax * pcsincrs);
                //            }
                            
                //        }
                   
                //    //bool chk = dc.Stocks.Where(a => a.Product_Token == echdtls.Product_Token).Any();
                //    //if (chk == true)
                //    //{
                //    //    Stock stk = dc.Stocks.Where(a => a.Product_Token == echdtls.Product_Token).FirstOrDefault();
                //    //    stk.Date = echdtls.Date;
                //    //    stk.Pieces = stk.Pieces - echdtls.Pieces;
                      
                //    //    stk.Product_Token = echdtls.Product_Token;

                //    //    if ((stk.Pieces < 0 && stk.Pieces == 0))
                //    //    {
                //    //        return BadRequest("Sorry there is some problem. Please check and try again");
                //    //    }
                //    //    dc.SaveChanges();

                //    //}
                //    //else
                //    //{
                //    //    return NotFound();
                //    //}
                //}
                foreach(var stks in stkouts)
                {
                    stks.Billing_Token_number = billing.Token_Number;
                    stks.Date = DateTime.Now.Date;
                    stks.Billing_number = billing.Billing_Number;
                  //  stks.Marchent_Token_number = billing.Marchent_Token_number;
                }
                brcdmstr.Barcode_Number = barcode;
                brcdmstr.Billing_Number = billing.Billing_Number;
                brcdmstr.Billing_Token_number = billing.Token_Number;
                brcdmstr.Date = billing.Date;

               // billing.Barcode_Number = barcode;

                // image save for barcode///

                using (MemoryStream ms = new MemoryStream())
                {
                    //The Image is drawn based on length of Barcode text.
                    using (Bitmap bitMap = new Bitmap(barcode.Length * 30, 90))
                    {
                        //The Graphics library object is generated for the Image.
                        using (Graphics graphics = Graphics.FromImage(bitMap))
                        {
                            //The installed Barcode font.
                            Font oFont = new Font("IDAutomationHC39M", 17);
                            PointF point = new PointF(2f, 2f);

                            //White Brush is used to fill the Image with white color.
                            SolidBrush whiteBrush = new SolidBrush(Color.White);
                            graphics.FillRectangle(whiteBrush, 0, 0, bitMap.Width, bitMap.Height);

                            //Black Brush is used to draw the Barcode over the Image.
                            SolidBrush blackBrush = new SolidBrush(Color.Black);
                            graphics.DrawString("*" + barcode + "*", oFont, blackBrush, point);
                        }

                        //The Bitmap is saved to Memory Stream.
                        bitMap.Save(ms, ImageFormat.Png);

                        //The Image is finally converted to Base64 string.


                        byte[] imageBytes = Convert.FromBase64String(Convert.ToBase64String(ms.ToArray()));

                        MemoryStream ms1 = new MemoryStream(imageBytes, 0, imageBytes.Length);
                        ms1.Write(imageBytes, 0, imageBytes.Length);

                        System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
                        string spath = System.Web.HttpContext.Current.Server.MapPath("~/BillBarcode");
                        string path = spath + "/" + billing.Billing_Number + ".jpg";
                        image.Save(path, ImageFormat.Jpeg);

                        //p.photo = imageBytes;
                      //  billing.Barcode_photo = imageBytes;
                        brcdmstr.Image = imageBytes;
                    }

                }

                billing.Barcode_Master.Add(brcdmstr);

                dc.Billing_Masters.Add(billing);
                dc.SaveChanges();
                status = true;
            }

            return Ok(billing.Billing_Number);
        }

        public IHttpActionResult PostShippingBill([FromBody]Billing_Master billing)
        {
            bool status = false;
            Barcode_Master brcdmstr = new Barcode_Master();

            using (EasyBillingEntities dc = new EasyBillingEntities())
            {
                var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

                Random rd = new Random();

                var chars1 = Enumerable.Range(0, 14)
           .Select(x => chars[rd.Next(0, chars.Length)]);
                string barcode = new string(chars1.ToArray());

                billing.Token_Number = (Guid.NewGuid()).ToString();
                bool ischk = dc.Billing_Masters.Any();
                if (ischk == false)
                {
                    billing.Billing_Number = "INV00000001";
                }
                else
                {
                    var text = (from a in dc.Billing_Masters
                                orderby a.Billing_Number descending
                                select a.Billing_Number).FirstOrDefault();

                    var fstfr = text.Substring(0, 3);
                    var lstfr = text.Substring(text.Length - 8);
                    string newlstversn = (int.Parse(lstfr) + 100000001).ToString();
                    string fstfr1 = (newlstversn.Substring(newlstversn.Length - 8)).ToString();
                    String totalvrsn = fstfr + fstfr1;
                    billing.Billing_Number = totalvrsn;
                }
                billing.Date = DateTime.Now.Date;
                var mrchnttk = (from tkn in dc.Marchent_Accounts
                                where tkn.Email_Id == "souravganguly707@gmail.com" || tkn.Email_Id == "kannantyres@kannantyres.com"
                                select tkn.Token_number).Distinct().FirstOrDefault();
               // billing.Marchent_Token_number = mrchnttk;
                var chkst = (from tkn in dc.Marchent_Accounts
                             join st in dc.States on tkn.State_Code equals st.State_Code
                             where tkn.Email_Id == "souravganguly707@gmail.com" || tkn.Email_Id == "kannantyres@kannantyres.com"
                             select new { st.SGST, st.CGST, st.IGST, st.UTGST }).Distinct().FirstOrDefault();
                //if (chkst.SGST == true && chkst.CGST == true)
                //{
                //    billing.CGST = billing.Total_tax / 2;
                //    billing.SGST = billing.CGST;
                //}
                //else if (chkst.CGST == true && chkst.IGST == true)
                //{
                //    billing.CGST = billing.Total_tax / 2;
                   
                //}
                //else if (chkst.CGST == true && chkst.UTGST == true)
                //{
                //    billing.CGST = billing.Total_tax / 2;
                   
                //}
                //else if (chkst.SGST == true && chkst.IGST == true)
                //{
                //    billing.SGST = billing.Total_tax / 2;
                   
                //}
                //else if (chkst.SGST == true && chkst.UTGST == true)
                //{
                //    billing.SGST = billing.Total_tax / 2;
                   
                //}
                
                //else
                //{ }
                billing.Narration = "Thank you for purchase. Please visit our shop again...";

                var dtls = billing.Billing_Details.Distinct().ToList();
                var stkouts = billing.Stockouts.Distinct().ToList();

                //foreach (var echdtls in dtls)
                //{
                //    Products_For_Sale prdct = dc.Products_For_Sales.Where(a => a.Token_Number == echdtls.Product_Token).Distinct().FirstOrDefault();

                //    if (prdct != null)
                //    {

                //        echdtls.Billing_Token_number = billing.Token_Number;
                //        echdtls.Billing_number = billing.Billing_Number;
                //        echdtls.Date = DateTime.Now.Date;
                //        echdtls.Product_Token = prdct.Token_Number;
                //        if (echdtls.Pieces == prdct.Pieces)
                //        {
                //            billing.Rate_including_tax = billing.Rate_including_tax + prdct.Amout_after_tax;
                //            echdtls.Amount = prdct.Selling_Price;
                //            echdtls.Discount_percent = (((prdct.Amout_after_tax) - (prdct.Total)) * 100) / (prdct.Amout_after_tax);

                //        }
                //        else
                //        {
                //            int pcsincrs = echdtls.Pieces / prdct.Pieces;
                //            billing.Rate_including_tax = billing.Rate_including_tax = billing.Rate_including_tax + (prdct.Amout_after_tax * pcsincrs);
                //            echdtls.Amount = prdct.Selling_Price * pcsincrs;
                //            echdtls.Discount_percent = (((prdct.Amout_after_tax * pcsincrs) - (prdct.Total * pcsincrs)) * 100) / (prdct.Amout_after_tax * pcsincrs);
                //        }

                //    }

                   
                //}
                foreach (var stks in stkouts)
                {
                    stks.Billing_Token_number = billing.Token_Number;
                    stks.Date = DateTime.Now.Date;
                    stks.Billing_number = billing.Billing_Number;
                  //  stks.Marchent_Token_number = billing.Marchent_Token_number;
                }
                brcdmstr.Barcode_Number = barcode;
                brcdmstr.Billing_Number = billing.Billing_Number;
                brcdmstr.Billing_Token_number = billing.Token_Number;
                brcdmstr.Date = billing.Date;

               // billing.Barcode_Number = barcode;

                // image save for barcode///

                using (MemoryStream ms = new MemoryStream())
                {
                    //The Image is drawn based on length of Barcode text.
                    using (Bitmap bitMap = new Bitmap(barcode.Length * 30, 90))
                    {
                        //The Graphics library object is generated for the Image.
                        using (Graphics graphics = Graphics.FromImage(bitMap))
                        {
                            //The installed Barcode font.
                            Font oFont = new Font("IDAutomationHC39M", 17);
                            PointF point = new PointF(2f, 2f);

                            //White Brush is used to fill the Image with white color.
                            SolidBrush whiteBrush = new SolidBrush(Color.White);
                            graphics.FillRectangle(whiteBrush, 0, 0, bitMap.Width, bitMap.Height);

                            //Black Brush is used to draw the Barcode over the Image.
                            SolidBrush blackBrush = new SolidBrush(Color.Black);
                            graphics.DrawString("*" + barcode + "*", oFont, blackBrush, point);
                        }

                        //The Bitmap is saved to Memory Stream.
                        bitMap.Save(ms, ImageFormat.Png);

                        //The Image is finally converted to Base64 string.


                        byte[] imageBytes = Convert.FromBase64String(Convert.ToBase64String(ms.ToArray()));

                        MemoryStream ms1 = new MemoryStream(imageBytes, 0, imageBytes.Length);
                        ms1.Write(imageBytes, 0, imageBytes.Length);

                        System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
                        string spath = System.Web.HttpContext.Current.Server.MapPath("~/BillBarcode");
                        string path = spath + "/" + billing.Billing_Number + ".jpg";
                        image.Save(path, ImageFormat.Jpeg);

                        //p.photo = imageBytes;
                       // billing.Barcode_photo = imageBytes;
                        brcdmstr.Image = imageBytes;
                    }

                }

                billing.Barcode_Master.Add(brcdmstr);

                dc.Billing_Masters.Add(billing);
                dc.SaveChanges();
                status = true;
            }

            return Ok(billing.Billing_Number);
        }
        public IHttpActionResult GetBillingMasterList()
        {
            IList<BillingMasterClass> BMC = null;
            using (var ctx = new EasyBillingEntities())
            {
                BMC = ctx.Billing_Masters.Select(s => new BillingMasterClass()
                {
                    Token_Number = s.Token_Number,
                    //CGST = s.CGST,

                    //Customer_Token_number = s.Customer_Token_number,
                    Date = s.Date,
                  //  Discount_percent = s.Discount_percent,
                    Billing_Number = s.Billing_Number,

                 
                  //  Marchent_Token_number = s.Marchent_Token_number,
                    Narration = s.Narration,
                    Rate_including_tax = s.Rate_including_tax,
                    //SGST = s.SGST,
                    //Tax_token = s.Tax_token,
                    Total_amount = s.Total_amount,
                    Total_discount = s.Total_discount,
                    Total_tax = s.Total_tax,
                


                }).ToList();



            }
            if (BMC.Count == 0)
            {
                return NotFound();
            }


            return Ok(BMC);

        }

        public IHttpActionResult GetBillingDetailsList()
        {

            IList<BillingDetailsClass> BillingDet = null;

            using (var ctx = new EasyBillingEntities())
            {

                BillingDet = ctx.Billing_Details.Select(l => new BillingDetailsClass()
                {
                    Pieces = l.Pieces,

                 //   Product_Token = l.Product_Token,
                    Billing_number = l.Billing_number,
                    Date = l.Date,

                    Billing_Token_number = l.Billing_Token_number,
                
                    Amount = l.Amount,
                    Discount = l.Discount,
                    Billing_details_number = l.Billing_details_number,
                    //Discount_percent = l.Discount_percent,
                    //Taxable_amount = l.Taxable_amount,
                    Sub_Total = l.Sub_Total,
                    Tax = l.Tax,
                   

                }).Distinct().ToList();

            }

            if (BillingDet.Count == 0)
            {
                return NotFound();
            }

            return Ok(BillingDet);
        }
      
        public HttpResponseMessage GetBillingListReport(string Bllno)
        {
            List<Product> pp = new List<Product>();
            List<Products_For_Sale> pps = new List<Products_For_Sale>();
            if (Bllno != null)
            {
                EasyBillingEntities db = new EasyBillingEntities();
                var stks = (from i in db.Billing_Masters
                            select new
                            {
                                Token_Number = i.Token_Number,
                                Billing_Number = i.Billing_Number,
                                Date = i.Date,
                                //Customer_Token_number = i.Customer_Token_number,
                                //Marchent_Token_number = i.Marchent_Token_number


                            }).Where(u => u.Billing_Number == Bllno).Distinct().FirstOrDefault();
                if (stks != null)
                {
                    //var mrchnt = (from i in db.Marchent_Accounts
                    //              select new
                    //              {
                    //                  Token_number = i.Token_number,
                    //                  GSTIN_Number = i.GSTIN_Number,
                    //                  State_Name = i.State_Name

                    //              }).Where(u => u.Token_number == stks.Marchent_Token_number).Distinct().ToList();

                    var stks1 = (from i in db.Billing_Details
                                 select new
                                 {
                                     Billing_Token_number = i.Billing_Token_number,
                                     Billing_number = i.Billing_number,
                                     Date = i.Date,
                                     Billing_details_number = i.Billing_details_number,
                                     Pieces = i.Pieces,
                                    
                                     Amount = i.Amount,
                                     Tax = i.Tax,
                                    // Product_Token = i.Product_Token,
                                     Discount = i.Discount,
                                     Sub_Total = i.Sub_Total
                                     //Product_name = (from a in db.Products
                                     //                where a.Token_Number == i.Product_Token
                                     //                select a.Product_name).Distinct().FirstOrDefault()
                                 }).Where(u => u.Billing_Token_number == stks.Token_Number).Distinct().ToList();


                    //foreach (var t in stks1)
                    //{
                    //    Products_For_Sale prdctfrsl = db.Products_For_Sales.Where(r => r.Token_Number == t.Product_Token).Distinct().FirstOrDefault();
                    //    pps.Add(prdctfrsl);
                        
                    //}

                    var stks2 = (from i in db.Billing_Masters
                                 select new
                                 {
                                     Token_Number = i.Token_Number,
                                     Billing_Number = i.Billing_Number,
                                     Date = i.Date,
                                     Narration = i.Narration,
                                     Total_discount = i.Total_discount,
                                     Total_tax = i.Total_tax,
                                     Total_amount = i.Total_amount,
                                     //CGST = i.CGST,
                                     //SGST = i.SGST,
                                   
                                     //i.Barcode_photo,
                                     //i.Rate_including_tax,
                                     //i.Payment_Statement
                                     
                                 }).Where(u => u.Token_Number == stks.Token_Number).Distinct().ToList();
                    //var dlr = (from i in db.Customers
                    //           select new
                    //           {
                    //               Address = i.Address,
                    //               Customer_Name = i.Customer_Name,
                    //              // State_Name = i.State_Name,
                    //               Phone_number = i.Phone_number,
                    //               Token_number = i.Token_number


                    //           }).Where(u => u.Token_number == stks.Customer_Token_number).Distinct().ToList();
                    ReportDocument rpt = new ReportDocument();
                    // rpt.Refresh();
                    rpt.Load(Path.Combine(HostingEnvironment.MapPath("~/Reportsdataset/AllReports"), "BillReport.rpt"));
                    //rpt.Load(Path.Combine(HostingEnvironment.MapPath("~/"), "BillReport1.rpt"));
                   // rpt.Load("BillReport.rpt",CrystalDecisions.Shared.OpenReportMethod.OpenReportByDefault);
                    rpt.Database.Tables[0].SetDataSource(stks2);
                    rpt.Database.Tables[1].SetDataSource(stks1);
                    //rpt.Database.Tables[2].SetDataSource(dlr);
                    //rpt.Database.Tables[3].SetDataSource(mrchnt);
                    rpt.Database.Tables[4].SetDataSource(pps);
                    Stream s = rpt.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    var response = new HttpResponseMessage(HttpStatusCode.OK);


                    //PrinterSettings printerSettings = new PrinterSettings();
                    //printerSettings.PrinterName = "Microsoft XPS Document Writer";
                    // rpt.PrintToPrinter(printerSettings, new PageSettings(), false);
                    // rpt.PrintToPrinter(1, true, 1, 1);


                    response.Content = new StreamContent(s);
                    response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf");

                    return response;
                }
                else
                {
                    var response = new HttpResponseMessage(HttpStatusCode.NotFound);
                   return response;
                }
            }
            else
            {
                var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
                return response;
            }
        }
        public HttpResponseMessage GetBillingListRpt(string Bllno)
        {
           // List<StockForBillingRPT> StockForBillingRPTlist = new List<StockForBillingRPT>();
           //StockForBillingRPT StockForBillingRPT = new StockForBillingRPT();
           // if (Bllno != null)
           // {
           //     EasyBillingEntities db = new EasyBillingEntities();
           //     var Billing_Masters = (from i in db.Billing_Masters
           //                 select i).Where(u => u.Billing_Number == Bllno).Distinct().FirstOrDefault();
           //     if (Billing_Masters != null)
           //     {
           //         var Billing_Details = (from i in db.Billing_Details
           //                      select i).Where(u => u.Billing_Token_number == Billing_Masters.Token_Number).Distinct().ToList();
                  
           //         foreach (var t in Billing_Details)
           //         {
           //             Products_For_Sale prdctfrsl = db.Products_For_Sales.Where(r => r.Token_Number == t.Selling_item_token).Distinct().FirstOrDefault();
           //             StockForBillingRPT.Token_Number = prdctfrsl.Token_Number;
           //             StockForBillingRPT.Tyre_Size = prdctfrsl.Tyre_Size;
           //             StockForBillingRPT.Selling_SGST = prdctfrsl.Selling_SGST;
           //             StockForBillingRPT.Selling_Price = prdctfrsl.Selling_Price;
           //             StockForBillingRPT.Selling_CGST = prdctfrsl.Selling_CGST;
           //             StockForBillingRPT.Product_name = prdctfrsl.Product_name;
           //             StockForBillingRPT.Item_tyre_Id = prdctfrsl.Item_tyre_Id;

           //             StockForBillingRPTlist.Add(StockForBillingRPT);
                      
           //         }
                   
           //             db.StockForBillingRPTs.AddRange(StockForBillingRPTlist);
           //             db.SaveChanges();
                    
           //         var Billing_MastersList = (from i in db.Billing_Masters
           //                      select i).Where(u => u.Token_Number == Billing_Masters.Token_Number).Distinct().ToList();
           //         var Customers = (from i in db.Customers
           //                    select i).Where(u => u.Token_number == Billing_Masters.Customer_token_number).Distinct().ToList();
                
                    ReportDocument rpt = new ReportDocument();
                    //  rprt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, @"G:\NiceSoftware\Nice-Billing" + SetValueForpdf + ".pdf");
                   rpt.Load(Path.Combine(HostingEnvironment.MapPath("~/Reportsdataset/AllReports"), "CrystalReportTest.rpt"));
                    //rpt.Load(Path.Combine(HostingEnvironment.MapPath("~/"), "BillReport1.rpt"));
                    //rpt.Load("http://localhost:8087/Reportsdataset/AllReports/BillReport.rpt", CrystalDecisions.Shared.OpenReportMethod.OpenReportByDefault);
                    //rpt.Database.Tables[0].SetDataSource(Billing_MastersList);
                    //rpt.Database.Tables[1].SetDataSource(Billing_Details);
                    //rpt.Database.Tables[2].SetDataSource(Customers);
                    //rpt.Database.Tables[3].SetDataSource(StockForBillingRPTlist);
                 
                    Stream s = rpt.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    var response = new HttpResponseMessage(HttpStatusCode.OK);


                    //PrinterSettings printerSettings = new PrinterSettings();
                    //printerSettings.PrinterName = "Microsoft XPS Document Writer";
                    // rpt.PrintToPrinter(printerSettings, new PageSettings(), false);
                    // rpt.PrintToPrinter(1, true, 1, 1);


                    response.Content = new StreamContent(s);
                    response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf");
                   //string SetValueForpdf = DateTime.Now.ToString("ddMMyyhhmmtt");
                   // rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, "http://localhost:8087/Reportsdataset/AllReports" + SetValueForpdf + ".pdf");
                    
            //db.StockForBillingRPTs.RemoveRange(StockForBillingRPTlist);
                    //    db.SaveChanges();
                    
                    return response;
        //        }
        //        else
        //        {
        //            var response = new HttpResponseMessage(HttpStatusCode.NotFound);
        //            return response;
        //        }
        //    }
        //    else
        //    {
        //        var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
        //        return response;
        //    }
        }
    }
}
