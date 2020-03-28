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
    public class PaymentTermsController : ApiController
    {
        private DbContextModel db = new DbContextModel();

        /// <summary>
        /// GetPaymentTermsList
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.Route("portal/api/paymentterms/getpaymenttermslist")]
        public IHttpActionResult GetPaymentTermsList(string search = null)
        {
            var terms = new List<PaymentTerms>();
            List<PaymentTermsListVM> list = new List<PaymentTermsListVM>();
            terms =  db.PaymentTerms
                        .OrderByDescending(x => x.TermId)
                        .ToList();
            //Search fields
            if (!string.IsNullOrEmpty(search))
            {
                terms = terms.Where(x =>
                        x.TermId.ToLower().Contains(search.ToLower()) ||
                        x.Name.ToString().ToLower().Contains(search.ToLower()) ||
                        x.NoOfDays.ToString().ToLower().Contains(search.ToLower()))
                    .OrderByDescending(x => x.TermId)
                    .ToList();
            }
            if (terms.Count > 0)
            {
                list = terms.Select(x => new PaymentTermsListVM()
                {
                    TermId = x.TermId,
                    Name = x.Name,
                    NoOfDays = x.NoOfDays,
                    Status = x.Status
                }).ToList();
            }
            return Ok(list);
        }

        /// <summary>
        /// GetPaymentTerms
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("portal/api/paymentterms/getpaymenttermsinfo")]
        public IHttpActionResult GetPaymentTerms()
        {
            var terms =  db.PaymentTerms.Where(x=>x.Status.Equals(true)).ToList();
            return Ok(terms);
        }

        /// <summary>
        /// GetPaymentTerms
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.Route("portal/api/paymentterms/{id}")]
        public IHttpActionResult GetPaymentTerms(string id)
        {
            PaymentTerms terms =  db.PaymentTerms.Find(id);
            if (terms == null)
            {
                return NotFound();
            }

            return Ok(terms);
        }


        /// <summary>
        /// EditPaymentTerms
        /// </summary>
        /// <param name="id"></param>
        /// <param name="terms"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPut, System.Web.Http.Route("portal/api/paymentterms/{id}")]
        public IHttpActionResult EditPaymentTerms(string id, [FromBody]PaymentTerms terms)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != terms.TermId)
            {
                return BadRequest();
            }

            db.Entry(terms).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaymentTermsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(terms);
        }

        /// <summary>
        /// AddPaymentTerms
        /// </summary>
        /// <param name="terms"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost, System.Web.Http.Route("portal/api/paymentterms")]
        public IHttpActionResult AddPaymentTerms([FromBody]PaymentTerms terms)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                //check payment terms if exists
                var check = db.PaymentTerms.Where(x => x.TermId.ToUpper().Trim() == terms.TermId.ToUpper().Trim()).Any();
                if (check)
                {
                    return BadRequest("Payment Term already exists! Please create different payment term.");
                }
                var identity = (ClaimsIdentity)User.Identity;
                terms.CreatedById = identity.Name;
                db.PaymentTerms.Add(terms);
                db.SaveChanges();
                return Ok(terms);
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
        /// DeletePaymentTerms
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [System.Web.Http.HttpDelete, System.Web.Http.Route("portal/api/paymentterms/{id}")]
        public IHttpActionResult DeletePaymentTerms(string id)
        {
            PaymentTerms terms =  db.PaymentTerms.Find(id);
            if (terms == null)
            {
                return NotFound();
            }

            db.PaymentTerms.Remove(terms);
            db.SaveChanges();

            return Ok(terms);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PaymentTermsExists(string id)
        {
            return db.PaymentTerms.Count(e => e.TermId == id) > 0;
        }
    }
}