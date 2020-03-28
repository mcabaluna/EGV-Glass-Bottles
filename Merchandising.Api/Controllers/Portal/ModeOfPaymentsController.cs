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
using Merchandising.VM.Portal;

namespace Merchandising.Api.Controllers.Portal
{
    public class ModeOfPaymentsController : ApiController
    {
        private DbContextModel db = new DbContextModel();

        /// <summary>
        /// GetModeOfPaymentList
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.Route("portal/api/modeofpayment/getmodeofpaymentlist")]
        public IHttpActionResult GetModeOfPaymentList(string search = null)
        {
            var mop = new List<ModeOfPayment>();
            List<ModeOfPaymentListVM> list = new List<ModeOfPaymentListVM>();
            mop =  db.ModeOfPayments
                        .OrderByDescending(x => x.Code)
                        .ToList();
            //Search fields
            if (!string.IsNullOrEmpty(search))
            {
                mop = mop.Where(x =>
                        x.Code.ToLower().Contains(search.ToLower()) ||
                        x.Name.ToString().ToLower().Contains(search.ToLower()))
                    .OrderByDescending(x => x.Code)
                    .ToList();
            }
            if (mop.Count > 0)
            {
                list = mop.Select(x => new ModeOfPaymentListVM()
                {
                    Code = x.Code,
                    Name = x.Name,
                    Status = x.Active
                }).ToList();
            }
            return Ok(list);
        }

        /// <summary>
        /// GetModeOfPayment
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("portal/api/modeofpayment/getmodeofpaymentinfo")]
        public IHttpActionResult GetModeOfPayment()
        {
            var wtax =  db.ModeOfPayments.Where(x=> x.Active.Equals(true)).ToList();
            return Ok(wtax);
        }

        /// <summary>
        /// GetModeOfPayment
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.Route("portal/api/modeofpayment/{id}")]
        public IHttpActionResult GetModeOfPayment(string id)
        {
            ModeOfPayment mop =  db.ModeOfPayments.Find(id);
            if (mop == null)
            {
                return NotFound();
            }

            return Ok(mop);
        }


        /// <summary>
        /// EditModeOfPayment
        /// </summary>
        /// <param name="id"></param>
        /// <param name="vat"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPut, System.Web.Http.Route("portal/api/modeofpayment/{id}")]
        public IHttpActionResult EditModeOfPayment(string id, [FromBody]ModeOfPayment mop)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != mop.Code)
            {
                return BadRequest();
            }

            db.Entry(mop).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ModeOfPaymentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(mop);
        }

        /// <summary>
        /// AddModeOfPayment
        /// </summary>
        /// <param name="vat"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost, System.Web.Http.Route("portal/api/modeofpayment")]
        public IHttpActionResult AddModeOfPayment([FromBody]ModeOfPayment mop)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                //check branch if exists
                var check = db.ModeOfPayments.Where(x => x.Code.ToUpper().Trim() == mop.Code.ToUpper().Trim()).Any();
                if (check)
                {
                    return BadRequest("Mode of Payment already exists! Please create different mode of payment.");
                }
                var identity = (ClaimsIdentity)User.Identity;
                mop.CreatedById = identity.Name;
                db.ModeOfPayments.Add(mop);
                db.SaveChanges();
                return Ok(mop);
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
        /// DeleteModeOfPayment
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [System.Web.Http.HttpDelete, System.Web.Http.Route("portal/api/modeofpayment/{id}")]
        public IHttpActionResult DeleteModeOfPayment(string id)
        {
            ModeOfPayment mop =  db.ModeOfPayments.Find(id);
            if (mop == null)
            {
                return NotFound();
            }

            db.ModeOfPayments.Remove(mop);
            db.SaveChanges();

            return Ok(mop);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ModeOfPaymentExists(string id)
        {
            return db.ModeOfPayments.Count(e => e.Code == id) > 0;
        }
    }
}