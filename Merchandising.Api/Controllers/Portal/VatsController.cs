using Merchandising.DTO;
using Merchandising.DTO.Models;
using Merchandising.Helper;
using Merchandising.VM.Portal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;

namespace Merchandising.Api.Controllers.Portal
{
    public class VatsController : ApiController
    {
        private DbContextModel db = new DbContextModel();

        /// <summary>
        /// GetVatList
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.Route("portal/api/vat/getvatlist")]
        public IHttpActionResult GetVatList(string search = null)
        {
            var vat = new List<Vat>();
            List<VatListVM> list = new List<VatListVM>();
            //get all users with filter 
            vat =  db.Vat
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
                list = vat.Select(x => new VatListVM()
                {
                    Code = x.Code,
                    Name = x.Name,
                    Type = GlobalFunctions.GetVatValue(x.Type),
                    Percentage = x.Percentage,
                    Status = x.Status,
                    EffectiveFrom = x.EffectiveFrom,
                    EffectiveTo = x.EffectiveTo
                }).ToList();
            }
            return Ok(list);
        }
        /// <summary>
        /// GetVat
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("portal/api/vat/getvatinfo")]
        public IHttpActionResult GetVat()
        {
            var vat =  db.Vat.Where(x => x.Status.Equals(true) &&
                                                        ((DateTime.Now.Year >= x.EffectiveFrom.Year && DateTime.Now.Month >= x.EffectiveFrom.Month && DateTime.Now.Day >= x.EffectiveFrom.Day) &&
                                                        (DateTime.Now.Year <= x.EffectiveTo.Year && DateTime.Now.Month <= x.EffectiveTo.Month && DateTime.Now.Day <= x.EffectiveTo.Day))).ToList();
            return Ok(vat);
        }

        /// <summary>
        /// GetVat
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.Route("portal/api/vat/{id}")]
        public IHttpActionResult GetVat(string id)
        {
            Vat vat = db.Vat.Find(id);
            if (vat == null)
            {
                return NotFound();
            }

            return Ok(vat);
        }

        /// <summary>
        /// EditVat
        /// </summary>
        /// <param name="id"></param>
        /// <param name="vat"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPut, System.Web.Http.Route("portal/api/vat/{id}")]
        public IHttpActionResult EditVat(string id, [FromBody]Vat vat)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != vat.Code)
            {
                return BadRequest();
            }

            db.Entry(vat).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VatExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(vat);
        }

        /// <summary>
        /// AddVat
        /// </summary>
        /// <param name="vat"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost, System.Web.Http.Route("portal/api/vat")]
        public IHttpActionResult AddVat(Vat vat)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                //check branch if exists
                var check = db.Vat.Where(x => x.Code.ToUpper().Trim() == vat.Code.ToUpper().Trim()).Any();
                if (check)
                {
                    return BadRequest("Vat already exists! Please create different vat.");
                }
                var identity = (ClaimsIdentity)User.Identity;
                vat.CreatedById = identity.Name;
                db.Vat.Add(vat);
                db.SaveChanges();
                return Ok(vat);
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
        /// DeleteVat
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [System.Web.Http.HttpDelete, System.Web.Http.Route("portal/api/vat/{id}")]
        public IHttpActionResult DeleteVat(string id)
        {
            Vat vat =  db.Vat.Find(id);
            if (vat == null)
            {
                return NotFound();
            }

            db.Vat.Remove(vat);
            db.SaveChanges();

            return Ok(vat);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool VatExists(string id)
        {
            return db.Vat.Count(e => e.Code == id) > 0;
        }
    }
}