using EasyBilling.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Mail;
using System.Web.Http;
using Zender.Mail;

namespace EasyBilling.Controllers.Webapi
{
    public class MarchentController : ApiController
    {
        public IHttpActionResult GetAllMarchents()
        {
            bool includeAddress = false;
            IList<MarchentAccount> marchent = null;

            using (var ctx = new EasyBillingEntities())
            {

                marchent = ctx.Marchent_Accounts.Include("State").Select(s => new MarchentAccount()
                {
                    Token_number = s.Token_number,
                    Marchent_name = s.Marchent_name,
                    Email_Id = s.Email_Id,
                    Mobile = s.Mobile,
                    Telephone_No = s.Telephone_No,
                    Address = s.Address,
                    GSTIN_Number = s.GSTIN_Number,
                    Pan_Number = s.Pan_Number,
                    IsActive = s.IsActive,
                    State_Code = s.State_Code,
                    State_Name = s.State_Name,
                    State = s.State == null || includeAddress == false ? null : new StateData()
                    {
                        State_Code = s.State_Code.Value,

                    },
                    stcd = (from a in ctx.States
                            where a.State_Code == s.State_Code
                            select a.Name).Distinct().FirstOrDefault(),

                }).ToList<MarchentAccount>();


            }


            if (marchent.Count == 0)
            {
                return NotFound();
            }

            return Ok(marchent);
        }
        public IHttpActionResult PostNewMerchant([FromBody]Marchent_Account marchent)
        {
            if (!ModelState.IsValid)
                return BadRequest("Sorry there is some problem. Please check and try again");

            using (var ctx = new EasyBillingEntities())
            {
                if (marchent.Email_Id != null && marchent.GSTIN_Number != null && marchent.Marchent_name != null)
                {
                    //var stcode = int.Parse(marchent.stcd);
                    bool chkmndtrycredentials = false;
                    bool chkcredentialsPAN = false;
                    bool chkcredentialsUIN = false;
                    bool chkcredentialsCIN = false;
                    var chkstate = ctx.States.Select(a => new { a.State_Code, a.Name }).Where(x => x.State_Code == marchent.State_Code).FirstOrDefault();
                    chkmndtrycredentials = ctx.Marchent_Accounts.Where(x => x.Email_Id == marchent.Email_Id || x.GSTIN_Number == marchent.GSTIN_Number).Any();

                    if (marchent.UIN_Number != null)
                    {
                        chkcredentialsUIN = ctx.Marchent_Accounts.Where(x => x.UIN_Number == marchent.UIN_Number).Any();
                    }
                    if (marchent.CIN_Number != null)
                    {
                        chkcredentialsCIN = ctx.Marchent_Accounts.Where(x => x.CIN_Number == marchent.CIN_Number).Any();
                    }
                    if (marchent.Pan_Number != null)
                    {
                        chkcredentialsPAN = ctx.Marchent_Accounts.Where(x => x.Pan_Number == marchent.Pan_Number).Any();
                    }

                    if (chkcredentialsUIN == true || chkcredentialsCIN == true || chkcredentialsPAN == true || chkmndtrycredentials == true)
                    {
                        return BadRequest("Require fields mandotory.");
                    }
                    else
                    {
                        if (chkstate != null)
                        {
                            marchent.IsActive = true;
                            marchent.State_Name = chkstate.Name;
                            marchent.State_Code = marchent.State_Code;
                            marchent.Token_number = (Guid.NewGuid()).ToString();
                            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789,.\"';:[] }{|=+-_!@#$%^&*()/";

                            Random rd = new Random();

                            var chars1 = Enumerable.Range(0, 10)
                       .Select(x => chars[rd.Next(0, chars.Length)]);
                            marchent.Verification_code = new string(chars1.ToArray());

                            ctx.Marchent_Accounts.Add(marchent);
                            ctx.SaveChanges();
                            SendVerificationPassToEmail(marchent.Email_Id, marchent.Verification_code);
                        }
                        else
                        {
                            return BadRequest("State does not exists");
                        }
                    }



                }
                else
                {
                    return BadRequest("Require fields mandotory.");
                }
            }

            return Ok();
        }

        public IHttpActionResult PostMerchantLogin([FromBody]Marchent_Account marchent)
        {
            if (!ModelState.IsValid)
                return BadRequest("Sorry there is some problem. Please check and try again");

            using (var ctx = new EasyBillingEntities())
            {
                if (marchent.Email_Id != null && marchent.Verification_code != null)
                {
                    bool chck = ctx.Marchent_Accounts.Where(a => a.Email_Id == marchent.Email_Id && a.Verification_code == marchent.Verification_code).Any();
                    var mrchnt = ctx.Marchent_Accounts.Where(a => a.Email_Id == marchent.Email_Id && a.Verification_code == marchent.Verification_code).FirstOrDefault();
                    if (chck == true && mrchnt.IsActive == true)
                    {
                        marchent.License = mrchnt.License;
                        return Ok(marchent);

                    }else
                    {
                        return BadRequest("Email id or passord does not exist.");
                    }
                }
                else
                {
                    return BadRequest("Require fields mandotory.");
                }
            }

            return Ok();
        }

        public IHttpActionResult GetAllMarchents(string id)
        {
            bool includeAddress = false;
            MarchentAccount marchent = null;

            using (var ctx = new EasyBillingEntities())
            {

                marchent = ctx.Marchent_Accounts.Include("State").Where(x => x.Token_number == id).Select(s => new MarchentAccount()
                {
                    Token_number = s.Token_number,
                    Marchent_name = s.Marchent_name,
                    Email_Id = s.Email_Id,
                    Mobile = s.Mobile,
                    Telephone_No = s.Telephone_No,
                    Address = s.Address,
                    GSTIN_Number = s.GSTIN_Number,
                    CIN_Number = s.CIN_Number,
                    UIN_Number = s.UIN_Number,
                    Pan_Number = s.Pan_Number,
                    IsActive = s.IsActive,
                    State_Name = s.State_Name,
                    stcd = (from a in ctx.States
                            where a.State_Code == s.State_Code
                            select a.Name).Distinct().FirstOrDefault(),
                    State = s.State == null || includeAddress == false ? null : new StateData()
                    {
                        State1 = s.State.Name,

                    },
                    State_Code = s.State_Code,
                }).FirstOrDefault();


            }


            if (marchent == null)
            {
                return NotFound();
            }

            return Ok(marchent);
        }
        public IHttpActionResult PutMerchant([FromBody]Marchent_Account marchent)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid data");

            using (var ctx = new EasyBillingEntities())
            {
                var existingMarchent = ctx.Marchent_Accounts.Where(s => s.Token_number == marchent.Token_number).FirstOrDefault();

                if (existingMarchent != null)
                {
                    var stcode = int.Parse(marchent.stcd);
                    var chkstate = ctx.States.Select(a => new { a.State_Code, a.Name }).Where(x => x.State_Code == stcode).FirstOrDefault();
                    if (chkstate != null)
                    {
                        existingMarchent.Marchent_name = marchent.Marchent_name;
                        existingMarchent.Email_Id = marchent.Email_Id;
                        existingMarchent.Address = marchent.Address;
                        existingMarchent.Mobile = marchent.Mobile;
                        existingMarchent.Telephone_No = marchent.Telephone_No;
                        existingMarchent.GSTIN_Number = marchent.GSTIN_Number;
                        existingMarchent.CIN_Number = marchent.CIN_Number;
                        existingMarchent.UIN_Number = marchent.UIN_Number;
                        existingMarchent.State_Name = chkstate.Name;
                        existingMarchent.State_Code = stcode;
                        existingMarchent.Pan_Number = marchent.Pan_Number;
                        ctx.SaveChanges();
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    return NotFound();
                }
            }

            return Ok();
        }

        public IHttpActionResult Delete(string id)
        {
            if (id == null)
                return BadRequest("Not a valid student id");

            using (var ctx = new EasyBillingEntities())
            {
                var merchant = ctx.Marchent_Accounts
                    .Where(s => s.Token_number == id)
                    .FirstOrDefault();

                ctx.Entry(merchant).State = EntityState.Deleted;
                ctx.SaveChanges();
            }

            return Ok();
        }
        [System.Web.Http.ActionName("PostActivate")]
        [System.Web.Http.HttpPost]
        public IHttpActionResult PostActivate(JObject jsonData)
        {
            dynamic json = jsonData;
            string token = json.Token_number;
            using (var ctx = new EasyBillingEntities())
            {
                var existingMarchent = ctx.Marchent_Accounts.Where(s => s.Token_number == token).FirstOrDefault();

                if (existingMarchent != null)
                {
                    var chkkactive = (from a in ctx.Marchent_Accounts
                                      where a.Token_number == token
                                      select a.IsActive).Distinct().FirstOrDefault();
                    if (chkkactive == true)
                    {
                        existingMarchent.IsActive = false;
                    }
                    else if (chkkactive == false)
                    {
                        existingMarchent.IsActive = true;
                    }
                    else
                    {
                        existingMarchent.IsActive = false;
                    }
                    ctx.SaveChanges();
                }
                else
                {
                    return NotFound();
                }
            }

            return Ok();
        }

        [System.Web.Http.NonAction]
        public void SendVerificationPassToEmail(String Toemail, string merchantpassword)
        {


            ZenderMessage message = new ZenderMessage("eaf7f170-f8e8-4391-8e5b-eb1919fca608");

            MailAddress from = new MailAddress("etreetraining@gmail.com");

            MailAddress to = new MailAddress(Toemail);



            message.From = from;

            message.To.Add(to);

            message.Subject = "Your Marchent account successfully created";

            message.Body = "Thank you for for your patience." +
                "<br />Your Easy Billing merchant account have been successfully created. " +

                 "<br /><b> Your email id: </b><b>" + Toemail + "</b>" +
                " <br /><b> Password: </b><b>" + merchantpassword + "</b>" +
                " <br />If you facing any problem, Please mail at hr@elephanttreetech.com or info@elephanttreetech.com .We will be always available for you and give support happily";

            message.IsBodyHtml = true;

            message.SendMailAsync();

        }

        
    }
}
