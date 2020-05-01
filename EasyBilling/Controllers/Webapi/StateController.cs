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
    public class StateController : ApiController
    {
        public IHttpActionResult GetAllStates(bool includeAddress = false)
        {
            IList<StateData> state = null;

            using (var ctx = new EasyBillingEntities())
            {

                state = ctx.States.Select(s => new StateData()
                {
                    State_Code = s.State_Code,
                    Name = s.Name,
                   
                    State_Identity = s.State_Identity,

                    CGST = s.CGST,
                    SGST = s.SGST,
                    UTGST = s.UTGST,
                    IGST=s.IGST,
                    
                }).ToList<StateData>();


            }


            if (state.Count == 0)
            {
                return NotFound();
            }

            return Ok(state);
        }
        public IHttpActionResult PostNewState([FromBody]State state)
        {
            if (!ModelState.IsValid)
                return BadRequest("Sorry there is some problem. Please check and try again");

            using (var ctx = new EasyBillingEntities())
            {
                if (state.State_Code != 0)
                {
                    if (state.CGST == null)
                    {
                        state.CGST = false;
                    }
                    
                    if (state.SGST == null)
                    {
                        state.SGST = false;
                    }
                    
                    if (state.IGST == null)
                    {
                        state.IGST = false;
                    }
                    
                    if (state.UTGST == null)
                    {
                        state.UTGST = false;
                    }
                    
                    ctx.States.Add(state);
                    ctx.SaveChanges();
                }
                else
                {
                    return BadRequest("State code required");
                }
            }

            return Ok();
        }
        public IHttpActionResult GetAllStates(string id)
        {

            StateData state = null;

            using (var ctx = new EasyBillingEntities())
            {
                var statecd = int.Parse(id);
                state = ctx.States.Where(x => x.State_Code == statecd).Select(s => new StateData()
                {
                    State_Code=s.State_Code,
                    Name=s.Name,
                    State_Identity=s.State_Identity,
                    CGST=s.CGST,
                    SGST=s.SGST,
                    IGST=s.IGST,
                    UTGST=s.UTGST,
                    

                }).FirstOrDefault();


            }


            if (state == null)
            {
                return NotFound();
            }

            return Ok(state);
        }

        public IHttpActionResult GetAllStatesforedit(string id="19")
        {

            IList<StateData> state = null;

            using (var ctx = new EasyBillingEntities())
            {
                var statecd = int.Parse(id);
                state = ctx.States.Select(s => new StateData()
                {
                    State_Code = s.State_Code,
                    Name = s.Name,
                    State_Identity = s.State_Identity,
                    CGST = s.CGST,
                    SGST = s.SGST,
                    IGST = s.IGST,
                    UTGST = s.UTGST,


                }).OrderByDescending(x => x.State_Code == statecd).ToList();


            }


            if (state == null)
            {
                return NotFound();
            }

            return Ok(state);
        }
        public IHttpActionResult PutTaxGroup([FromBody]State state)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid data");

            using (var ctx = new EasyBillingEntities())
            {
                var existingState = ctx.States.Where(s => s.State_Code == state.State_Code).FirstOrDefault();

                if (existingState != null)
                {
                    if (state.CGST == true && state.SGST == true && state.IGST == true && state.UTGST == true)
                    {
                        return BadRequest("All tax sections cannot be true");
                    }
                    else if (state.CGST == true && state.SGST == true && state.IGST == true)
                    {
                        return BadRequest("Please check your tax sections");
                    }
                    else if (state.CGST == true && state.SGST == true && state.UTGST == true)
                    {
                        return BadRequest("Please check your tax sections");
                    }
                    else if (state.CGST == true && state.IGST == true && state.UTGST == true)
                    {
                        return BadRequest("Please check your tax sections");
                    }
                    else if (state.CGST == true && state.IGST == true)
                    {
                        return BadRequest("Please check your tax sections");
                    }
                    else if (state.SGST == true && state.IGST == true && state.UTGST == true)
                    {
                        return BadRequest("Please check your tax sections");
                    }
                    else if (state.SGST == true && state.IGST == true)
                    {
                        return BadRequest("Please check your tax sections");
                    }
                    else if (state.SGST == true && state.UTGST == true)
                    {
                        return BadRequest("Please check your tax sections");
                    }
                    else if (state.UTGST == true && state.IGST == true)
                    {
                        return BadRequest("Please check your tax sections");
                    }
                    else
                    {
                        existingState.Name = state.Name;

                        existingState.State_Identity = state.State_Identity;
                        if (state.CGST == null)
                        {
                            existingState.CGST =false;
                        }
                        else
                        {
                            existingState.CGST = state.CGST;
                        }
                        if (state.SGST == null)
                        {
                            existingState.SGST = false;
                        }
                        else
                        {
                            existingState.SGST = state.SGST;
                        }
                        if (state.IGST == null)
                        {
                            existingState.IGST = false;
                        }
                        else
                        {
                            existingState.IGST = state.IGST;
                        }
                        if (state.UTGST == null)
                        {
                            existingState.UTGST = false;
                        }
                        else
                        {
                            existingState.UTGST = state.UTGST;
                         
                        }
                       
                        ctx.SaveChanges();
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
                var statecd = int.Parse(id);
                var state = ctx.States
                    .Where(s => s.State_Code == statecd)
                    .FirstOrDefault();

                ctx.Entry(state).State = EntityState.Deleted;
                ctx.SaveChanges();
            }

            return Ok();
        }
        [System.Web.Http.ActionName("PostActivate")]
        [System.Web.Http.HttpPost]
        public IHttpActionResult PostActivate(JObject jsonData)
        {
            dynamic json = jsonData;
            int token = json.Token_number;
            using (var ctx = new EasyBillingEntities())
            {
                var existingState = ctx.States.Where(s => s.State_Code == token).FirstOrDefault();
                
                //if (existingState != null)
                //{
                //    if (state.CGST == true && state.SGST == true && state.IGST == true && state.UTGST == true)
                //    {
                //        return BadRequest("All tax sections cannot be true");
                //    }
                //    else if (state.CGST == true && state.SGST == true && state.IGST == true)
                //    {
                //        return BadRequest("Please check your tax sections");
                //    }
                //    else if (state.CGST == true && state.SGST == true && state.UTGST == true)
                //    {
                //        return BadRequest("Please check your tax sections");
                //    }
                //    else if (state.CGST == true && state.IGST == true && state.UTGST == true)
                //    {
                //        return BadRequest("Please check your tax sections");
                //    }
                //    else if (state.CGST == true && state.IGST == true)
                //    {
                //        return BadRequest("Please check your tax sections");
                //    }
                //    else if (state.SGST == true && state.IGST == true && state.UTGST == true)
                //    {
                //        return BadRequest("Please check your tax sections");
                //    }
                //    else if (state.SGST == true && state.IGST == true)
                //    {
                //        return BadRequest("Please check your tax sections");
                //    }
                //    else if (state.SGST == true && state.UTGST == true)
                //    {
                //        return BadRequest("Please check your tax sections");
                //    }
                //    else if (state.UTGST == true && state.IGST == true)
                //    {
                //        return BadRequest("Please check your tax sections");
                //    }
                //    else
                //    {
                //        existingState.Name = state.Name;

                //        existingState.State_Identity = state.State_Identity;
                //        existingState.CGST = state.CGST;
                //        existingState.SGST = state.SGST;
                //        existingState.UTGST = state.UTGST;
                //        existingState.IGST = state.IGST;
                //        ctx.SaveChanges();
                //    }
                //}
                //else
                //{
                //    return NotFound();
                //}
            }

            return Ok();
        }

    }
}