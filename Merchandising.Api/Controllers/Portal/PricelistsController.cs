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
using System.Threading.Tasks;
using System.Web.Http;

namespace Merchandising.Api.Controllers.Portal
{
    public class PricelistsController : ApiController
    {
        private DbContextModel db = new DbContextModel();

        /// <summary>
        /// GetPricelist
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.Route("portal/api/pricelist/getpricelists")]
        public IHttpActionResult GetPricelists(string search = null)
        {
            var pricelist = new List<Pricelist>();
            List<PriceListVM> list = new List<PriceListVM>();
            pricelist =  db.Pricelists
                        .OrderByDescending(x => x.PricelistId)
                        .ToList();
            //Search fields
            if (!string.IsNullOrEmpty(search))
            {
                pricelist = pricelist.Where(x =>
                        x.PricelistId.ToLower().Contains(search.ToLower()) ||
                        x.Name.ToString().ToLower().Contains(search.ToLower()) ||
                        x.BasePricelist.ToString().ToLower().Contains(search.ToLower()) ||
                        x.Factor.ToString().ToLower().Contains(search.ToLower()))
                    .OrderByDescending(x => x.PricelistId)
                    .ToList();
            }
            if (pricelist.Count > 0)
            {
                list = pricelist.Select(x => new PriceListVM()
                {
                    PricelistId = x.PricelistId,
                    Name = x.Name,
                    BasePricelist = x.BasePricelist,
                    Factor = x.Factor,
                    Status = x.Status
                }).ToList();
            }
            return Ok(list);
        }

        /// <summary>
        /// GetPricelist
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("portal/api/pricelist/getpricelistinfo")]
        public IHttpActionResult GetPricelist()
        {
            var pricelist =  db.Pricelists.Where(x=> x.Status.Equals(true)).ToList();
            return Ok(pricelist);
        }

        /// <summary>
        /// GetPricelist
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.Route("portal/api/pricelist/{id}")]
        public IHttpActionResult GetPricelist(string id)
        {
            Pricelist pricelist = db.Pricelists.Include(x => x.Lines)
                           .Include(x => x.UoMs).SingleOrDefault(b => b.PricelistId == id);
            if (pricelist == null)
            {
                return NotFound();
            }

            return Ok(pricelist);
        }

        /// <summary>
        /// EditPricelist
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pricelist"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPut, System.Web.Http.Route("portal/api/pricelist/{id}")]
        public IHttpActionResult EditPricelist(string id, [FromBody]Pricelist pricelist)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != pricelist.PricelistId)
            {
                return BadRequest();
            }
            //itemlines
            var itemlist = db.PricelistLines.Where(x => x.PricelistId == pricelist.PricelistId).ToList();
            if (itemlist != null)
            {
                foreach (PricelistLines item in itemlist)
                {
                    db.PricelistLines.Remove(item);
                }
                foreach (PricelistLines item in pricelist.Lines)
                {
                    db.PricelistLines.Add(item);
                }
            }
            //itemuom
            var uomlist = db.PricelistUoM.Where(x => x.PricelistId == pricelist.PricelistId).ToList();
            if (uomlist != null)
            {
                foreach (PricelistUoM uom in uomlist)
                {
                    db.PricelistUoM.Remove(uom);
                }
                foreach (PricelistUoM uom in pricelist.UoMs)
                {
                    db.PricelistUoM.Add(uom);
                }
            }
            db.Entry(pricelist).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PricelistExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(pricelist);
        }

        /// <summary>
        /// AddPricelist
        /// </summary>
        /// <param name="pricelist"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost, System.Web.Http.Route("portal/api/pricelist")]
        public IHttpActionResult AddPricelist([FromBody]Pricelist pricelist)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                //check branch if exists
                var check = db.Pricelists.Where(x => x.PricelistId.ToUpper().Trim() == pricelist.PricelistId.ToUpper().Trim()).Any();
                if (check)
                {
                    return BadRequest("Pricelist is already exists! Please create different pricelist.");
                }
                var identity = (ClaimsIdentity)User.Identity;
                pricelist.CreatedById = identity.Name;

                //Validate Sequence
                //var numbering = (from i in db.SequenceTables.Where(o => o.Document == "PRICELIST")
                //                 select new { i.SeriesName, i.Prefix, i.NextNumber, i.Suffix }).ToList();

                //pricelist.PricelistId = numbering.FirstOrDefault().Suffix != null ?
                //                numbering.FirstOrDefault().Prefix + numbering.FirstOrDefault().NextNumber + "_" + numbering.FirstOrDefault().Suffix :
                //                numbering.FirstOrDefault().Prefix + numbering.FirstOrDefault().NextNumber;
                db.Pricelists.Add(pricelist);
                db.SaveChanges();

                //Update Sequence Table
                //string series = numbering.FirstOrDefault().SeriesName;
                //SequenceTable seq = db.SequenceTables.FirstOrDefault(i => i.SeriesName == series && i.Document == "PRICELIST");
                ////seq.NextNumber = numbering.FirstOrDefault().NextNumber + 1;
                //seq.NextNumber = string.Format("{0:00}", (int.Parse(numbering.FirstOrDefault().NextNumber) + 1));
                //seq.LastNumber = numbering.FirstOrDefault().NextNumber;

                //Update Sequence Table
                var series = Convert.ToInt32(pricelist.Series);
                var numbering = db.SequenceTableLines.SingleOrDefault(x => x.Series == series);
                if (numbering != null)
                {
                    numbering.LastNum = numbering.NextNumber;
                    numbering.NextNumber = numbering.NextNumber + 1;
                    db.SaveChanges();
                }
                //>>end
                //>>end
                return Ok(pricelist);
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
        /// DeletePricelist
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [System.Web.Http.HttpDelete, System.Web.Http.Route("portal/api/pricelist/{id}")]
        public IHttpActionResult DeletePricelist(string id)
        {
            Pricelist pricelist =  db.Pricelists.Find(id);
            if (pricelist == null)
            {
                return NotFound();
            }

            db.Pricelists.Remove(pricelist);
            db.SaveChanges();

            return Ok(pricelist);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PricelistExists(string id)
        {
            return db.Pricelists.Count(e => e.PricelistId == id) > 0;
        }
    }
}