using EasyBilling.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EasyBilling.Controllers.Webapi
{
    public class TaxGroupController : ApiController
    {
        public IHttpActionResult GetAllTaxGroups(bool includeAddress = false)
        {
            IList<TaxGroupClass> taxgroup = null;

            using (var ctx = new EasyBillingEntities())
            {

                taxgroup = ctx.Tax_Groups.Select(s => new TaxGroupClass()
                {
                    Tax_Token = s.Tax_Token,
                    Tax_Name = s.Tax_Name,
                    Tax_Rate = s.Tax_Rate,
                    GL_CODE = s.GL_CODE,
                    
                    IsActive = s.IsActive,
                   
                }).ToList<TaxGroupClass>();


            }


            if (taxgroup.Count == 0)
            {
                return NotFound();
            }

            return Ok(taxgroup);
        }
        public IHttpActionResult GetAllUserTaxGroups(bool includeAddress = false)
        {
            IList<TaxGroupClass> taxgroup = null;

            using (var ctx = new EasyBillingEntities())
            {

                taxgroup = ctx.Tax_Groups.Where(s=>s.IsActive==true).Select(s => new TaxGroupClass()
                {
                    Tax_Token = s.Tax_Token,
                    Tax_Name = s.Tax_Name,
                    Tax_Rate = s.Tax_Rate,
                    GL_CODE = s.GL_CODE,

                    IsActive = s.IsActive,

                }).ToList<TaxGroupClass>();


            }


            if (taxgroup.Count == 0)
            {
                return NotFound();
            }

            return Ok(taxgroup);
        }
        public IHttpActionResult PostNewTaxGroup([FromBody]Tax_Group taxgroup)
        {
            if (!ModelState.IsValid)
                return BadRequest("Sorry there is some problem. Please check and try again");

            using (var ctx = new EasyBillingEntities())
            {

         
                if (taxgroup.GL_CODE != null)
                {
                    taxgroup.IsActive = true;
                   
                    taxgroup.Tax_Token = (Guid.NewGuid()).ToString();
                    ctx.Tax_Groups.Add(taxgroup);
                    ctx.SaveChanges();
                }
                else
                {
                    return BadRequest("State does not exists");
                }
            }

            return Ok();
        }

        public IHttpActionResult GetAllTaxGroup(string id)
        {

            TaxGroupClass taxGroup = null;

            using (var ctx = new EasyBillingEntities())
            {

                taxGroup = ctx.Tax_Groups.Where(x => x.Tax_Token == id).Select(s => new TaxGroupClass()
                {
                    Tax_Token = s.Tax_Token,
                    Tax_Name = s.Tax_Name,
                    Tax_Rate = s.Tax_Rate,
                    GL_CODE = s.GL_CODE,
                    
                    IsActive = s.IsActive,

                }).FirstOrDefault();


            }


            if (taxGroup == null)
            {
                return NotFound();
            }

            return Ok(taxGroup);
        }

        public IHttpActionResult GetAllTaxGroupforedit(string id)
        {

            IList<TaxGroupClass> taxGroup = null;

            using (var ctx = new EasyBillingEntities())
            {

                taxGroup = ctx.Tax_Groups.Where(s=>s.IsActive==true).Select(s => new TaxGroupClass()
                {
                    Tax_Token = s.Tax_Token,
                    Tax_Name = s.Tax_Name,
                    Tax_Rate = s.Tax_Rate,
                    GL_CODE = s.GL_CODE,

                    IsActive = s.IsActive,

                }).OrderByDescending(x => x.GL_CODE == id).ToList();


            }


            if (taxGroup == null)
            {
                return NotFound();
            }

            return Ok(taxGroup);
        }

        public IHttpActionResult PutTaxGroup([FromBody]Tax_Group taxGroup)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid data");

            using (var ctx = new EasyBillingEntities())
            {
                var existingTaxGroup = ctx.Tax_Groups.Where(s => s.Tax_Token == taxGroup.Tax_Token).FirstOrDefault();

                if (existingTaxGroup != null)
                {
                    existingTaxGroup.Tax_Name = taxGroup.Tax_Name;
                   
                    existingTaxGroup.Tax_Rate = taxGroup.Tax_Rate;
                    existingTaxGroup.IsActive = true;
                    

                    ctx.SaveChanges();
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
                var taxGroup = ctx.Tax_Groups
                    .Where(s => s.Tax_Token == id)
                    .FirstOrDefault();

                ctx.Entry(taxGroup).State = EntityState.Deleted;
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
                var existingTaxGroup = ctx.Tax_Groups.Where(s => s.Tax_Token == token).FirstOrDefault();

                if (existingTaxGroup != null)
                {
                    var chkkactive = (from a in ctx.Tax_Groups
                                      where a.Tax_Token == token
                                      select a.IsActive).Distinct().FirstOrDefault();
                    if (chkkactive == true)
                    {
                        existingTaxGroup.IsActive = false;
                    }
                    else if (chkkactive == false)
                    {
                        existingTaxGroup.IsActive = true;
                    }
                    else
                    {
                        existingTaxGroup.IsActive = false;
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

    }
}