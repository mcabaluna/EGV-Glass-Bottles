using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Merchandising.DTO;
using Merchandising.DTO.Models;
using Merchandising.Helper;
using Merchandising.VM.Portal;

namespace Merchandising.Api.Controllers.Portal
{
    public class WTaxesController : ApiController
    {
        private DbContextModel db = new DbContextModel();

        /// <summary>
        /// GetWTaxList
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.Route("portal/api/wtax/getwtaxlist")]
        public IHttpActionResult GetWTaxList(string search = null)
        {
            var vat = new List<WTax>();
            List<WTaxListVM> list = new List<WTaxListVM>();
            vat = db.WTaxs
                        .OrderByDescending(x => x.Code)
                        .ToList();
            //Search fields
            if (!string.IsNullOrEmpty(search))
            {
                vat = vat.Where(x =>
                        x.Code.ToLower().Contains(search.ToLower()) ||
                        x.Name.ToString().ToLower().Contains(search.ToLower()) ||
                        x.Type.ToString().ToLower().Contains(search.ToLower()) ||
                        x.Percentage.ToString().ToLower().Contains(search.ToLower()) ||
                        x.EffectiveFrom.ToString().ToLower().Contains(search.ToLower()) ||
                        x.EffectiveTo.ToString().ToLower().Contains(search.ToLower()))
                    .OrderByDescending(x => x.Code)
                    .ToList();
            }
            if (vat.Count > 0)
            {
                list = vat.Select(x => new WTaxListVM()
                {
                    Code = x.Code,
                    Name = x.Name,
                    Type = GlobalFunctions.GetWTaxValue(x.Type),
                    Percentage = x.Percentage,
                    Status = x.Status,
                    EffectiveFrom = x.EffectiveFrom,
                    EffectiveTo = x.EffectiveTo
                }).ToList();
            }
            return Ok(list);
        }

        /// <summary>
        /// GetWTax
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("portal/api/wtax/getwtaxinfo")]
        public IHttpActionResult GetWTax()
        {
            var wtax = db.WTaxs.Where(x => x.Status.Equals(true) &&
                                                        ((DateTime.Now.Year >= x.EffectiveFrom.Year && DateTime.Now.Month >= x.EffectiveFrom.Month && DateTime.Now.Day >= x.EffectiveFrom.Day) &&
                                                        (DateTime.Now.Year <= x.EffectiveTo.Year && DateTime.Now.Month <= x.EffectiveTo.Month && DateTime.Now.Day <= x.EffectiveTo.Day))).ToList();
            return Ok(wtax);
        }

        /// <summary>
        /// GetWTax
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.Route("portal/api/wtax/{id}")]
        public IHttpActionResult GetWTax(string id)
        {
            WTax wTax =  db.WTaxs.Find(id);
            if (wTax == null)
            {
                return NotFound();
            }

            return Ok(wTax);
        }


        /// <summary>
        /// EditWTax
        /// </summary>
        /// <param name="id"></param>
        /// <param name="vat"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPut, System.Web.Http.Route("portal/api/wtax/{id}")]
        public IHttpActionResult EditWTax(string id, [FromBody]WTax wTax)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != wTax.Code)
            {
                return BadRequest();
            }

            db.Entry(wTax).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WTaxExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(wTax);
        }

        /// <summary>
        /// AddWTax
        /// </summary>
        /// <param name="vat"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost, System.Web.Http.Route("portal/api/wtax")]
        public IHttpActionResult AddWTax([FromBody]WTax wTax)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                //check branch if exists
                var check = db.WTaxs.Where(x => x.Code.ToUpper().Trim() == wTax.Code.ToUpper().Trim()).Any();
                if (check)
                {
                    return BadRequest("WTax already exists! Please create different wtax.");
                }
                var identity = (ClaimsIdentity)User.Identity;
                wTax.CreatedById = identity.Name;
                db.WTaxs.Add(wTax);
                db.SaveChanges();
                return Ok(wTax);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// DeleteWTax
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [System.Web.Http.HttpDelete, System.Web.Http.Route("portal/api/wtax/{id}")]
        public IHttpActionResult DeleteWTax(string id)
        {
            WTax wTax =  db.WTaxs.Find(id);
            if (wTax == null)
            {
                return NotFound();
            }

            db.WTaxs.Remove(wTax);
            db.SaveChanges();

            return Ok(wTax);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool WTaxExists(string id)
        {
            return db.WTaxs.Count(e => e.Code == id) > 0;
        }
    }
}