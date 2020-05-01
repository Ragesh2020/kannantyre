using EasyBilling.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web.Mvc;

namespace EasyBilling.Controllers
{
    public class BillingController : MybaseController
    {
        // GET: Billing
        public ActionResult Index()
        {
            List<BillingAll> allOrder = new List<BillingAll>();
            IList<Billing_Master> purchMast = null;
            IList<Billing_Detail> purchdet = null;

            IList<Product> prdct = null;
            
            using (var db = new EasyBillingEntities())
            {
                using (var client = new HttpClient())
                {
                    var responseTaskMaster = client.GetAsync("http://localhost:8087//api/Billing/GetBillingMasterList");
                    responseTaskMaster.Wait();
                    var responseTaskDetails = client.GetAsync("http://localhost:8087//api/Billing/GetBillingDetailsList");
                    responseTaskDetails.Wait();
                    var responseTaskProduct = client.GetAsync("http://localhost:8087//api/Product/GetAllProducts");
                    responseTaskProduct.Wait();

                    var resultMaster = responseTaskMaster.Result;
                    var resultDetails = responseTaskDetails.Result;
                    var resultProduct = responseTaskProduct.Result;

                    if (resultMaster.IsSuccessStatusCode)
                    {
                        var readTaskMast = resultMaster.Content.ReadAsAsync<IList<Billing_Master>>();
                        readTaskMast.Wait();

                        purchMast = readTaskMast.Result;
                        

                    }
                    else //web api sent error response 
                    {
                        purchMast = null;

                        ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    }

                    if (resultDetails.IsSuccessStatusCode)
                    {
                        var readTaskStock = resultDetails.Content.ReadAsAsync<IList<Billing_Detail>>();
                        readTaskStock.Wait();

                        purchdet = readTaskStock.Result;

                    }
                    else //web api sent error response 
                    {
                        purchdet = null;

                        ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    }

                    if (resultProduct.IsSuccessStatusCode)
                    {
                        var readTaskPrdct = resultProduct.Content.ReadAsAsync<IList<Product>>();
                        readTaskPrdct.Wait();

                        prdct = readTaskPrdct.Result;

                    }
                    else //web api sent error response 
                    {
                        prdct = null;

                        ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    }

                    foreach (var i in purchMast)
                    {
                        var od = purchdet.Where(a => a.Billing_Token_number.Equals(i.Token_Number)).ToList();
                        foreach (var k in od)
                        {

                            //foreach (var l in prdct)
                            //{
                            //    if (k.Product_Token == l.Token_Number)
                            //    {
                            //        k.Product_name = l.Product_name;
                            //    }
                            //}

                        }
                        
                        allOrder.Add(new BillingAll { Billingmast = i,BillingDet = od });

                    }


                    return View(allOrder);
                }
            }
        }

        public ActionResult BillSpecific(string INVid)
        {
            using (var db = new EasyBillingEntities())
             {
                Billing_Master purchMast = new Billing_Master();
                using (var client = new HttpClient())
                {
                    var responseTask = client.GetAsync("http://localhost:8087//api/Billing/GetBillingData?id=" + INVid);
                    responseTask.Wait();


                    var resultMaster = responseTask.Result;

                    if (resultMaster.IsSuccessStatusCode)
                    {
                        var readTaskMast = resultMaster.Content.ReadAsAsync<Billing_Master>();
                        readTaskMast.Wait();

                        purchMast = readTaskMast.Result;
                        using (MemoryStream ms = new MemoryStream())
                        {
                            // string barcode = purchMast.Barcode_Number;
                            //The Image is drawn based on length of Barcode text.
                            //   using (Bitmap bitMap = new Bitmap(barcode.Length * 40, 80))
                            //    {
                            //        //The Graphics library object is generated for the Image.
                            //        using (Graphics graphics = Graphics.FromImage(bitMap))
                            //        {
                            //            //The installed Barcode font.
                            //            Font oFont = new Font("IDAutomationHC39M", 16);
                            //            PointF point = new PointF(2f, 2f);

                            //            //White Brush is used to fill the Image with white color.
                            //            SolidBrush whiteBrush = new SolidBrush(Color.White);
                            //            graphics.FillRectangle(whiteBrush, 0, 0, bitMap.Width, bitMap.Height);

                            //            //Black Brush is used to draw the Barcode over the Image.
                            //            SolidBrush blackBrush = new SolidBrush(Color.Black);
                            //            graphics.DrawString("*" + barcode + "*", oFont, blackBrush, point);
                            //        }

                            //        //The Bitmap is saved to Memory Stream.
                            //        bitMap.Save(ms, ImageFormat.Png);

                            //        //The Image is finally converted to Base64 string.
                            //        ViewBag.BarcodeImage = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
                            //    }
                            //}
                            ViewBag.billingnum = purchMast.Billing_Number;
                        }
                    }
                    else //web api sent error response 
                    {
                        purchMast = null;

                        ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    }
                }
            
                return View(purchMast);
            }
        }
        public ActionResult Billing_Entry()
        {
            IEnumerable<Tax_Group> txgrp = null;
            IEnumerable<Customer> cstmr = null;
            IEnumerable<Product> prdct = null;

            using (var client = new HttpClient())
            {
                var responseTask = client.GetAsync("http://localhost:8087//api/TaxGroup/GetAllUserTaxGroups");
                responseTask.Wait();
                var responseTask1 = client.GetAsync("http://localhost:8087//api/Customer/GetAllCustomers");
                responseTask1.Wait();
                var responseTask2 = client.GetAsync("http://localhost:8087//api/Product/GetAllProducts");
                responseTask2.Wait();
                var purchlastid = client.GetAsync("http://localhost:8087//api/Billing/GetBillinglastId");
                purchlastid.Wait();

                var result = responseTask.Result;
                var result1 = responseTask1.Result;
                var result2 = responseTask2.Result;
                var lstid = purchlastid.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<Tax_Group>>();
                    readTask.Wait();

                    txgrp = readTask.Result;
                    ViewBag.txgrp = txgrp;
                }
                else //web api sent error response 
                {
                    txgrp = Enumerable.Empty<Tax_Group>();
                    ViewBag.txgrp = txgrp;
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
                if (result1.IsSuccessStatusCode)
                {
                    var readTask1 = result1.Content.ReadAsAsync<IList<Customer>>();
                    readTask1.Wait();
                    cstmr = readTask1.Result;
                    ViewBag.dlr = cstmr;
                }
                else //web api sent error response 
                {
                    cstmr = Enumerable.Empty<Customer>();
                    ViewBag.dlr = cstmr;
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
                if (result2.IsSuccessStatusCode)
                {
                    var readTask2 = result2.Content.ReadAsAsync<IList<Product>>();
                    readTask2.Wait();
                    prdct = readTask2.Result;
                    ViewBag.prdct = prdct;
                }
                else //web api sent error response 
                {
                    prdct = Enumerable.Empty<Product>();
                    ViewBag.prdct = prdct;
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }

                if (lstid.IsSuccessStatusCode)
                {
                    var readTask3 = lstid.Content.ReadAsAsync<Billing_Master>();
                    readTask3.Wait();
                    var lst = readTask3.Result;
                    var text = lst.Billing_Number;
                    var fstfr = text.Substring(0, 3);
                    var lstfr = text.Substring(text.Length - 8);
                    string newlstversn = (int.Parse(lstfr) + 100000001).ToString();
                    string fstfr1 = (newlstversn.Substring(newlstversn.Length - 8)).ToString();
                    String totalvrsn = fstfr + fstfr1;
                    ViewBag.lstid = totalvrsn;
                }
                else //web api sent error response 
                {
                    ViewBag.lstid = "INV00000001";
                }
            }
            return View();
        }

        [HttpPost]
        public JsonResult save(Billing_Master Billing)
        {
           
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri("http://localhost:8087//api/Billing/PostNewMerchant");

                //HTTP POST
                var postTask = client.PostAsJsonAsync("http://localhost:8087//api/Billing/PostNewMerchant", Billing);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return new JsonResult { Data = new { status = true } };
                }
            }

            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");


            return new JsonResult { Data = new { status = false } };
        }

        [HttpPost]
        public ActionResult save1(FormCollection f)
        {

            foreach (var key in f.Keys)
            {
                var value = f[key.ToString()];
                var a= f[key.ToString()].Substring(f[key.ToString()].Length - 1, 1);
            }

            return new JsonResult { Data = new { status = false } };
        }

        [Authorize]
        public ActionResult CartList(string v)
        {
            List<Temp_placedorder> tmplist = new List<Temp_placedorder>();
            

            using (EasyBillingEntities db = new EasyBillingEntities())
            {
                
                var customerToken = db.Customers.Where(z => z.Email == User.Identity.Name).Select(z => z.Token_number).Distinct().FirstOrDefault();
            var usr = "0";
            if (customerToken == null)
            {
                customerToken = db.Users.Where(z => z.Email == User.Identity.Name).Select(z => z.Token_number).Distinct().FirstOrDefault();
                usr = "1";
            }
            if (v!=null && v!="")
            {
               
                    
                    bool chk = db.Temp_placedorder.Where(z => z.Item_token == v && z.Customer_token == customerToken).Any();
                    if (chk == true)
                    {
                        Temp_placedorder temp= db.Temp_placedorder.Where(z => z.Item_token == v && z.Customer_token == customerToken).Distinct().FirstOrDefault();
                        var pcs = db.Products_For_Sales.Where(z => z.Token_Number == v).Select(z => z.Pieces).Distinct().FirstOrDefault();
                        temp.Pieces = temp.Pieces + pcs;
                        db.SaveChanges();
                    }
                    else
                    {
                        Temp_placedorder temp_Placedorder = new Temp_placedorder();
                        temp_Placedorder.Token_number = Guid.NewGuid().ToString();
                        temp_Placedorder.Item_token = v;
                        var pcs = db.Products_For_Sales.Where(z => z.Token_Number == v).Select(z => z.Pieces).Distinct().FirstOrDefault();
                        temp_Placedorder.Pieces = pcs;
                        temp_Placedorder.Customer_token = customerToken;
                        if (usr == "1")
                        {
                            temp_Placedorder.IsUser = true;
                        }
                        else
                        {
                            temp_Placedorder.IsUser = false;
                        }
                        temp_Placedorder.Order_Date = DateTime.Now.Date;

                        db.Temp_placedorder.Add(temp_Placedorder);
                        db.SaveChanges();
                    }
                   var tmpdata = db.Temp_placedorder.Where(z => z.Customer_token == customerToken).Distinct().ToList();
                    foreach(var data in tmpdata)
                    {
                        Temp_placedorder temp_Placedorder = new Temp_placedorder();
                        var productsdetails = db.Products_For_Sales.Where(z => z.Token_Number == data.Item_token).Distinct().FirstOrDefault();
                        temp_Placedorder.Token_number = data.Token_number;
                        temp_Placedorder.Productname = productsdetails.Product_name;
                        temp_Placedorder.Pieces = data.Pieces;
                        temp_Placedorder.Primaryprice = productsdetails.Total;
                        temp_Placedorder.price = productsdetails.Total * (int)data.Pieces;
                        tmplist.Add(temp_Placedorder);
                    }
                    return View(tmplist);
                  
                }
                else
                {
                    var tmpdata = db.Temp_placedorder.Where(z => z.Customer_token == customerToken).Distinct().ToList();
                    foreach (var data in tmpdata)
                    {
                        Temp_placedorder temp_Placedorder = new Temp_placedorder();
                        var productsdetails = db.Products_For_Sales.Where(z => z.Token_Number == data.Item_token).Distinct().FirstOrDefault();
                        temp_Placedorder.Token_number = data.Token_number;
                        temp_Placedorder.Productname = productsdetails.Product_name;
                        temp_Placedorder.Pieces = data.Pieces;
                        temp_Placedorder.Primaryprice = productsdetails.Total;
                        temp_Placedorder.price = productsdetails.Total * (int)data.Pieces;
                        tmplist.Add(temp_Placedorder);
                    }
                    return View(tmplist);
                }
            }
           
        }
        [Authorize]
        public ActionResult DeliveryAddress(string v)
        {
            Customer_shipping_address customer_Shipping_Address = new Customer_shipping_address();
          
                ViewBag.tkn = v;
                using (EasyBillingEntities db = new EasyBillingEntities())
                {
                if (v != null && v != "")
                {
                    ViewBag.item = db.Products_For_Sales.Where(z => z.Token_Number == v).Distinct().FirstOrDefault();
                }
                
                    var customerToken = db.Customers.Where(z => z.Email == User.Identity.Name).Select(z=>z.Token_number).Distinct().FirstOrDefault();
                    if(customerToken==null)
                    {
                        customerToken = db.Users.Where(z => z.Email == User.Identity.Name).Select(z => z.Token_number).Distinct().FirstOrDefault();
                    }
                if (v == null || v == "")
                {
                    List<cartlist> cartlists = new List<cartlist>();
                   
                    var itmcrt = db.Temp_placedorder.Where(z => z.Customer_token == customerToken).Distinct().ToList();
                    if(itmcrt.Count==0)
                    {
                        return RedirectToAction("CartList");
                    }
                    foreach(var eachitm in itmcrt)
                    {
                        cartlist cartlist = new cartlist();
                        var lst= db.Products_For_Sales.Where(z => z.Token_Number == eachitm.Item_token).Select(z=> z.Total).Distinct().FirstOrDefault();
                     
                        cartlist.Price = lst;
                        cartlists.Add(cartlist);
                    }
                    var total = decimal.Parse("0.00");
                  foreach (var crtlsts in cartlists)
                    {
                        total = total+crtlsts.Price;
                    }
                    
                    ViewBag.itemcart = total;
                }
                bool chktkn = db.Customer_shipping_addresses.Where(z => z.Customer_token_number == customerToken).Any();
                    if(chktkn!=false)
                    {
                        customer_Shipping_Address = db.Customer_shipping_addresses.Where(z => z.Customer_token_number == customerToken).Distinct().FirstOrDefault();
                    }
            
            }
            if(customer_Shipping_Address!=null)
            {
                return View(customer_Shipping_Address);
            }
            else
            {
                return View();
            }
            
        }
        public PartialViewResult Payment(Customer_shipping_address customer_Shipping_Address,string tkn)
        {
            using (EasyBillingEntities db = new EasyBillingEntities())
            {
                var customerToken = db.Customers.Where(z => z.Email == User.Identity.Name).Select(z => z.Token_number).Distinct().FirstOrDefault();
                int a = 0;
                if(customerToken==null)
                {
                    customerToken = db.Users.Where(z => z.Email == User.Identity.Name).Select(z => z.Token_number).Distinct().FirstOrDefault();
                    a = 2;
                }
                bool chktkn = db.Customer_shipping_addresses.Where(z => z.Customer_token_number == customerToken).Any();
                if (chktkn == false)
                {
                    customer_Shipping_Address.Customer_token_number = customerToken;
                    if(a==2)
                    {
                        customer_Shipping_Address.IsUser = true;
                    }else
                    {
                        customer_Shipping_Address.IsUser = false;
                    }
                    
                    db.Customer_shipping_addresses.Add(customer_Shipping_Address);
                    
                }
                else
                {
                    Customer_shipping_address customer_Shipping_Address1 = db.Customer_shipping_addresses.Where(z => z.Customer_token_number == customerToken).Distinct().FirstOrDefault();
                    customer_Shipping_Address1.First_name = customer_Shipping_Address.First_name;
                    customer_Shipping_Address1.Last_name = customer_Shipping_Address.Last_name;
                    customer_Shipping_Address1.Address_Line1 = customer_Shipping_Address.Address_Line1;
                    customer_Shipping_Address1.Address_Line2 = customer_Shipping_Address.Address_Line2;
                    customer_Shipping_Address1.Town_City = customer_Shipping_Address.Town_City;
                    customer_Shipping_Address1.State = customer_Shipping_Address.State;
                    customer_Shipping_Address1.Pin_code = customer_Shipping_Address.Pin_code;
                    customer_Shipping_Address1.Phone_number = customer_Shipping_Address.Phone_number;
                    customer_Shipping_Address1.Email = customer_Shipping_Address.Email;
                    if (a == 2)
                    {
                        customer_Shipping_Address1.IsUser = true;
                    }
                    else
                    {
                        customer_Shipping_Address1.IsUser = false;
                    }

                }


                db.SaveChanges();
            }
            ViewBag.tkn = tkn;
                return PartialView("_payment");
          
        }
        [Authorize]
        public ActionResult PlaceOrder(string tkn, string rd,string error)
        {
            ViewBag.tkn = tkn;
            if (rd.ToUpper()=="PAYPAL")
            {
                ViewBag.payment = "Pay with paypal";
            }
            else
            {
                ViewBag.payment = "Pay on delivery";
                ViewBag.pod = "1";
            }
            
            Customer_shipping_address customer_Shipping_Address = new Customer_shipping_address();
            using (EasyBillingEntities db = new EasyBillingEntities())
            {
                if(tkn == null && tkn == "")
                        {
                    ViewBag.item = db.Products_For_Sales.Where(z => z.Token_Number == tkn).Distinct().FirstOrDefault();
                }
                var customerToken = db.Customers.Where(z => z.Email == User.Identity.Name).Select(z => z.Token_number).Distinct().FirstOrDefault();
                var  isuser = "0";
                if (customerToken==null)
                {
                    customerToken = db.Users.Where(z => z.Email == User.Identity.Name).Select(z => z.Token_number).Distinct().FirstOrDefault();
                    isuser = "1";
                }
                ViewBag.isuser = isuser;
                bool chktkn = db.Customer_shipping_addresses.Where(z => z.Customer_token_number == customerToken).Any();
                if (chktkn != false)
                {
                  customer_Shipping_Address = db.Customer_shipping_addresses.Where(z => z.Customer_token_number == customerToken).Distinct().FirstOrDefault();
                }
                if (tkn == null || tkn == "")
                {
                    ViewBag.tkn = null;
                    List<cartlist> cartlists = new List<cartlist>();

                    var itmcrt = db.Temp_placedorder.Where(z => z.Customer_token == customerToken).Distinct().ToList();
                    foreach (var eachitm in itmcrt)
                    {
                        cartlist cartlist = new cartlist();
                        var lst = db.Products_For_Sales.Where(z => z.Token_Number == eachitm.Item_token).Select(z => z.Total).Distinct().FirstOrDefault();

                        cartlist.Price = lst;
                        cartlists.Add(cartlist);
                    }
                    var total = decimal.Parse("0.00");
                    foreach (var crtlsts in cartlists)
                    {
                        total = total + crtlsts.Price;
                    }

                    ViewBag.itemcart = total;
                }
                ViewBag.error = error;
                return View(customer_Shipping_Address);
            }
        }

        public ActionResult PlaceOrderSuccess(string v,string pymth,string usr)
        {
            if (pymth.ToUpper() == "PAYPAL")
            {
                ViewBag.payment = "Pay with paypal";
            }
            else
            {
                ViewBag.payment = "Pay on delivery";
            }
            Customer_shipping_address customer_Shipping_Address = new Customer_shipping_address();
            Placed_Order placed_Order = new Placed_Order();
            if (v != null && v != "")
            {
               
                using (EasyBillingEntities db = new EasyBillingEntities())
                {
                    if (v.ToLower() != "crt")
                    {
                        ViewBag.item = db.Products_For_Sales.Where(z => z.Token_Number == v).Distinct().FirstOrDefault();
                        ViewBag.tkn = v;
                    }
                    string customerToken = null;
                    if (usr=="1")
                    {
                         customerToken = db.Users.Where(z => z.Email == User.Identity.Name).Select(z => z.Token_number).Distinct().FirstOrDefault();
                    }
                    else
                    {
                         customerToken = db.Customers.Where(z => z.Email == User.Identity.Name).Select(z => z.Token_number).Distinct().FirstOrDefault();
                    }
                    if (v.ToLower() == "crt")
                    {
                        ViewBag.tkn = null;
                        List<cartlist> cartlists = new List<cartlist>();

                        var itmcrt = db.Temp_placedorder.Where(z => z.Customer_token == customerToken).Distinct().ToList();
                        foreach (var eachitm in itmcrt)
                        {
                            cartlist cartlist = new cartlist();
                            var lst = db.Products_For_Sales.Where(z => z.Token_Number == eachitm.Item_token).Select(z => z.Total).Distinct().FirstOrDefault();

                            cartlist.Price = lst;
                            cartlists.Add(cartlist);
                        }
                        var total = decimal.Parse("0.00");
                        foreach (var crtlsts in cartlists)
                        {
                            total = total + crtlsts.Price;
                        }

                        ViewBag.itemcart = total;
                    }
                    bool chktkn = db.Customer_shipping_addresses.Where(z => z.Customer_token_number == customerToken).Any();
                    if (chktkn != false)
                    {
                        if (v.ToLower() != "crt")
                        {
                            placed_Order.Token_number = Guid.NewGuid().ToString();
                            placed_Order.Item_token = v;
                            placed_Order.Orderplaced = false;
                            placed_Order.Pieces = 1;
                            placed_Order.Customer_token = customerToken;
                            if (usr == "1")
                            {
                                placed_Order.IsUser = true;
                            }
                            else
                            {
                                placed_Order.IsUser = false;
                            }
                            placed_Order.Order_Date = DateTime.Now.Date;
                            placed_Order.Approve_Date = DateTime.Parse("12/12/12");
                            if (pymth.ToUpper() == "PAYPAL")
                            {
                                placed_Order.Ispaid = true;
                            }
                            else
                            {
                                placed_Order.Ispaid = false;
                            }
                            db.Placed_Orders.Add(placed_Order);

                            Stock stock = db.Stocks.Where(z => z.Product_Token == v).Distinct().FirstOrDefault();
                            Products_For_Sale products_For_Sale = db.Products_For_Sales.Where(z => z.Token_Number == v).Distinct().FirstOrDefault();
                            stock.Pieces = stock.Pieces - products_For_Sale.Pieces;
                            if (stock.Pieces < 0)
                            {
                                return RedirectToAction("PlaceOrder", new { error = "Stock cannot be negetive. Please check." });
                            }
                            else
                            {
                                db.SaveChanges();
                            }

                          
                        }else
                        {
                            var tmlchk = db.Temp_placedorder.Where(z => z.Customer_token == customerToken).Distinct().ToList();
                            if(tmlchk.Count()>0)
                            {
                                foreach(var eachtmpdata in tmlchk)
                                {
                                    placed_Order.Token_number = Guid.NewGuid().ToString();
                                    placed_Order.Item_token = eachtmpdata.Item_token;
                                    placed_Order.Orderplaced = false;
                                    placed_Order.Pieces = (int)eachtmpdata.Pieces;
                                    placed_Order.Customer_token = customerToken;
                                    if (usr == "1")
                                    {
                                        placed_Order.IsUser = true;
                                    }
                                    else
                                    {
                                        placed_Order.IsUser = false;
                                    }
                                    placed_Order.Order_Date = DateTime.Now.Date;
                                    placed_Order.Approve_Date = DateTime.Parse("12/12/12");
                                    if (pymth.ToUpper() == "PAYPAL")
                                    {
                                        placed_Order.Ispaid = true;
                                    }
                                    else
                                    {
                                        placed_Order.Ispaid = false;
                                    }
                                    db.Placed_Orders.Add(placed_Order);

                                    Stock stock = db.Stocks.Where(z => z.Product_Token == eachtmpdata.Item_token).Distinct().FirstOrDefault();
                                    Products_For_Sale products_For_Sale = db.Products_For_Sales.Where(z => z.Token_Number == eachtmpdata.Item_token).Distinct().FirstOrDefault();
                                    stock.Pieces = stock.Pieces - (products_For_Sale.Pieces*((int)eachtmpdata.Pieces));
                                    
                                    if(stock.Pieces<0)
                                    {
                                        return RedirectToAction("PlaceOrder",new { error="Stock cannot be negetive. Please check."});
                                    }else
                                    {
                                        db.SaveChanges();
                                    }

                                   
                                }
                            }
                     
                            db.Temp_placedorder.RemoveRange(tmlchk);
                            db.SaveChanges();
                        }
                        customer_Shipping_Address = db.Customer_shipping_addresses.Where(z => z.Customer_token_number == customerToken).Distinct().FirstOrDefault();
                    }
                }
            }
            if (customer_Shipping_Address != null)
            {
                return View(customer_Shipping_Address);
            }
            else
            {
                return View();
            }

        }
        [Authorize]
        public ActionResult PendingShipping(int? page,string id,string sccess, string bllno)
        {
           PendingShipping pendingShipping = new PendingShipping();
           List<PendingShipping> pendingShippingList = new List<PendingShipping>();

            using (EasyBillingEntities db = new EasyBillingEntities())
            {
                List<Placed_Order> placed_Order = db.Placed_Orders.Where(z => z.Orderplaced == false).Distinct().ToList();
                if (id!=null)
                {
                    Placed_Order plcdorder = db.Placed_Orders.Where(z => z.Token_number == id).Distinct().FirstOrDefault();
                    if(plcdorder.Orderplaced==false)
                    {
                        plcdorder.Orderplaced = true;
                        plcdorder.Approve_Date = DateTime.Now.Date;
                        db.SaveChanges();
                        Billing_Master billing = new Billing_Master();
                        Billing_Detail billing_Detail = new Billing_Detail();
                        Stockout stockout = new Stockout();

                        Products_For_Sale products_For_Sale = db.Products_For_Sales.Where(z => z.Token_Number == plcdorder.Item_token).FirstOrDefault();
                        //===============bill master entry=========================//
                       // billing.Customer_Token_number = plcdorder.Customer_token;
                        billing.Total_tax = (products_For_Sale.Amout_after_tax - products_For_Sale.Selling_Price) * plcdorder.Pieces;
                        billing.Rate_including_tax = (products_For_Sale.Amout_after_tax)*plcdorder.Pieces;
                        billing.Total_discount = decimal.Parse("0.00") *plcdorder.Pieces;
                        billing.Total_amount = products_For_Sale.Total *plcdorder.Pieces;
                        //if (plcdorder.Ispaid == true)
                        //{
                        //    billing.Payment_Statement = "*** Paid by Paypal ***";
                        //}else
                        //{
                        //    billing.Payment_Statement = "*** Pay on delivery ***";
                        //}
                        //===============bill details entry=========================//

                      //  billing_Detail.Product_Token = plcdorder.Item_token;
                        billing_Detail.Pieces = products_For_Sale.Pieces *plcdorder.Pieces;
                      
                        billing_Detail.Tax = (products_For_Sale.Amout_after_tax - products_For_Sale.Selling_Price) * plcdorder.Pieces;
                        billing_Detail.Discount=decimal.Parse("0.00");
                        billing_Detail.Sub_Total = products_For_Sale.Total*plcdorder.Pieces;


                        //===============stockout entry=========================//

                        stockout.Product_Token = plcdorder.Item_token;
                        stockout.Pieces = products_For_Sale.Pieces *plcdorder.Pieces;
                  
                        stockout.CGST= (products_For_Sale.Selling_Price * plcdorder.Pieces)+ ((products_For_Sale.CGST*plcdorder.Pieces)/100);
                        stockout.SGST = (products_For_Sale.Selling_Price * plcdorder.Pieces) + ((products_For_Sale.SGST * plcdorder.Pieces) / 100);
                        stockout.Sub_Total = products_For_Sale.Total*plcdorder.Pieces;

                        //===========================================================//
                        billing.Billing_Details.Add(billing_Detail);
                        billing.Stockouts.Add(stockout);

                        using (var client = new HttpClient())
                        {

                            client.BaseAddress = new Uri("http://localhost:8087//api/Billing/PostShippingBill");

                            //HTTP POST
                            var postTask = client.PostAsJsonAsync("http://localhost:8087//api/Billing/PostShippingBill", billing);
                            postTask.Wait();

                            var result = postTask.Result;
                            if (result.IsSuccessStatusCode)
                            {
                                var nmbr = result.Content.ReadAsStringAsync()
                                                           .Result
                                                           .Replace("\\", "")
                                                           .Trim(new char[1] { '"' });
                                ViewBag.billno = nmbr;
                                ViewBag.success = "Successfully approved";
                            }
                            else
                            {
                                ViewBag.error = "Something went wrong. Please try again.";
                            }
                        }

                        
                    }
                    else
                    {
                        ViewBag.error = "Already approved";
                    }
                }
                placed_Order = db.Placed_Orders.Where(z => z.Orderplaced == false).Distinct().ToList();
                foreach (var plcdordrpending in placed_Order)
                {
                    
                    pendingShipping.token = plcdordrpending.Token_number;
                    if (plcdordrpending.IsUser == true)
                    {
                        pendingShipping.Customer_name = db.Users.Where(z => z.Token_number == plcdordrpending.Customer_token).Select(z => z.Name).Distinct().FirstOrDefault();
                    }
                    else
                    {
                        pendingShipping.Customer_name = db.Customers.Where(z => z.Token_number == plcdordrpending.Customer_token).Select(z => z.Customer_Name).Distinct().FirstOrDefault();
                    }
                    pendingShipping.tyreName = db.Products_For_Sales.Where(z => z.Token_Number == plcdordrpending.Item_token).Select(z => z.Product_name).Distinct().FirstOrDefault();
                    pendingShipping.Total_Piece = plcdordrpending.Pieces;
                   pendingShipping.Total_Price = (db.Products_For_Sales.Where(z => z.Token_Number == plcdordrpending.Item_token).Select(z => z.Total).Distinct().FirstOrDefault())* plcdordrpending.Pieces;
                    pendingShipping.Applied_Date = plcdordrpending.Order_Date;
                    pendingShipping.Isuser = plcdordrpending.IsUser;
                    if (plcdordrpending.Ispaid==true)
                    {
                        pendingShipping.paymentstatement = "Paid by Paypal";
                    }
                    else
                    {
                        pendingShipping.paymentstatement = "Pay on delivery";
                    }
                    pendingShippingList.Add(pendingShipping);
                    pendingShipping = new PendingShipping();
                }
            }
            if (ViewBag.error != null)
            {

                return RedirectToAction("PendingShipping", new { sccess = ViewBag.error });
            }
            else if (ViewBag.success != null)
            {
              
                return RedirectToAction("PendingShipping", new { sccess = ViewBag.success, bllno = ViewBag.billno,id="" });
            }
            else
            {
                ViewBag.success = sccess;
                ViewBag.billno = bllno;
                ViewBag.error = sccess;
                return View(pendingShippingList.ToPagedList(page ?? 1, 5));
            }
               
        }

       
    }
}