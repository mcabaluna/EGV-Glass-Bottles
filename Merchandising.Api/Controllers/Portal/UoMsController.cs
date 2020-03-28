using Merchandising.DTO;
using Merchandising.DTO.Models;
using Merchandising.VM.Portal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;

namespace Merchandising.Api.Controllers.Portal
{
    public class UoMsController : ApiController
    {
        private DbContextModel db = new DbContextModel();

        /// <summary>
        /// GetUoMList
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.Route("portal/api/uom/getuomlist")]
        public IHttpActionResult GetUoMList(string search = null)
        {
            var uom = new List<UoM>();
            List<UoMListVM> list = new List<UoMListVM>();
            uom = db.UoM
                        .OrderByDescending(x => x.Code)
                        .ToList();
            //Search fields
            if (!string.IsNullOrEmpty(search))
            {
                uom = uom.Where(x =>
                        x.Code.ToLower().Contains(search.ToLower()) ||
                        x.Name.ToString().ToLower().Contains(search.ToLower()))
                    .OrderByDescending(x => x.Code)
                    .ToList();
            }
            if (uom.Count > 0)
            {
                list = uom.Select(x => new UoMListVM()
                {
                    Code = x.Code,
                    Name = x.Name,
                    Status = x.Status
                }).ToList();
            }
            return Ok(list);
        }

        /// <summary>
        /// GetUoM
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("portal/api/uom/getuominfo")]
        public IHttpActionResult GetUoM()
        {
            var uom = db.UoM.Where(x => x.Status.Equals(true)).ToList();
            return Ok(uom);
        }

        /// <summary>
        /// GetUoM
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.Route("portal/api/uom/{id}")]
        public IHttpActionResult GetUoM(string id)
        {
            UoM uom =  db.UoM.Find(id);
            if (uom == null)
            {
                return NotFound();
            }

            return Ok(uom);
        }


        /// <summary>
        /// EditUoM
        /// </summary>
        /// <param name="id"></param>
        /// <param name="uom"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPut, System.Web.Http.Route("portal/api/uom/{id}")]
        public IHttpActionResult EditUoM(string id, [FromBody]UoM uom)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != uom.Code)
            {
                return BadRequest();
            }

            db.Entry(uom).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UoMExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(uom);
        }

        /// <summary>
        /// AddUoM
        /// </summary>
        /// <param name="uom"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost, System.Web.Http.Route("portal/api/uom")]
        public IHttpActionResult AddUoM([FromBody]UoM uom)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                //check uom if exists
                var check = db.UoM.Where(x => x.Code.ToUpper().Trim() == uom.Code.ToUpper().Trim()).Any();
                if (check)
                {
                    return BadRequest("UoM already exists! Please create different uom.");
                }
                var identity = (ClaimsIdentity)User.Identity;
                uom.CreatedById = identity.Name;
                db.UoM.Add(uom);
                db.SaveChanges();
                return Ok(uom);
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
        /// DeleteUoM
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [System.Web.Http.HttpDelete, System.Web.Http.Route("portal/api/uom/{id}")]
        public IHttpActionResult DeleteUoM(string id)
        {
            UoM uom = db.UoM.Find(id);
            if (uom == null)
            {
                return NotFound();
            }

            db.UoM.Remove(uom);
            db.SaveChanges();

            return Ok(uom);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UoMExists(string id)
        {
            return db.UoM.Count(e => e.Code == id) > 0;
        }
    }
}