using Merchandising.DTO;
using Merchandising.DTO.Models;
using Merchandising.Helper;
using Merchandising.VM.Portal;
using Merchandising.VM.Results;
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
    public class BusinessPartnerController : ApiController
    {
        private DbContextModel db = new DbContextModel();

        /// <summary>
        /// GetBusinessPartnerList
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.Route("portal/api/businesspartner/getbusinesspartnerlist")]
        public IHttpActionResult GetBusinessPartnerList(string search = null)
        {
            var bp = new List<BusinessPartner>();
            List<BusinessPartnerListVM> list = new List<BusinessPartnerListVM>();
            //get all bp with filter 

            bp = db.BusinessPartners
                        .OrderByDescending(x => x.CardCode)
                        .ToList();
            //Search fields
            if (!string.IsNullOrEmpty(search))
            {
                bp = bp.Where(x =>
                        x.CardCode.ToLower().Contains(search.ToLower()) ||
                        x.CardName.ToString().ToLower().Contains(search.ToLower()) ||
                        x.BpType.ToString().ToLower().Contains(search.ToLower()) ||
                        x.BpCode.ToString().ToLower().Contains(search.ToLower()) ||
                        x.Address.ToString().ToLower().Contains(search.ToLower()) ||
                        x.ContactNumber.ToString().ToLower().Contains(search.ToLower()) ||
                        x.Email.ToString().ToLower().Contains(search.ToLower()) ||
                        x.Balance.ToString().ToLower().Contains(search.ToLower()) ||
                         x.Remarks.ToString().ToLower().Contains(search.ToLower()))
                    .OrderByDescending(x => x.CardCode)
                    .ToList();
            }
            if (bp.Count > 0)
            {
                list = bp.Select(x => new BusinessPartnerListVM()
                {
                    CardCode = x.CardCode,
                    CardName = x.CardName,
                    BpType = x.BpType == "C" ? "CUSTOMER" : "SUPPLIER",
                    BpGroupCode = x.BpCode,
                    Address = x.Address,
                    ContactNumber = x.ContactNumber,
                    WithWTax = x.WithWTax,
                    Email = x.Email,
                    TermId = x.TermId,
                    VatCode = x.VatCode,
                    Tin = x.Tin,
                    PricelistId = x.PricelistId,
                    Balance = x.Balance,
                    Series = x.Series,
                    Remarks = x.Remarks,
                    Status = x.Status
                }).ToList();
            }
            return Ok(list);
        }
        /// <summary>
        /// GetBusinessPartner
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("portal/api/businesspartner/getbusinesspartnerinfo")]
        public IHttpActionResult GetBusinessPartner()
        {
            var bp = db.BusinessPartners.ToList();
            return Ok(bp);
        }

        /// <summary>
        /// GetBusinessPartner
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.Route("portal/api/businesspartner/{id}")]
        public IHttpActionResult GetBusinessPartner(string id)
        {
            BusinessPartner bp = db.BusinessPartners.Include(x => x.BpAddresses)
                                    .Include(x => x.BpWTax).SingleOrDefault(b => b.CardCode == id);
            if (bp == null)
            {
                return NotFound();
            }

            return Ok(bp);
        }
        /// <summary>
        /// GetBPBalanceDetails
        /// </summary>
        /// <param name="cardcode"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.Route("portal/api/businesspartner/getbpbalancedetails/{id}")]
        public IHttpActionResult GetBPBalanceDetails(string id)
        {
            //SalesInvoice
            var invoices = db.SalesInvoices.Where(x => x.CardCode == id &&
                                            (x.Status == Enums.InvoiceType.PARTIALLY_PAID ||
                                            x.Status == Enums.InvoiceType.UNPAID)).ToList()
                                            .Select(b => new BPBalance_Results()
                                            {
                                                DocEntry = b.DocEntry,
                                                DocNum = b.DocNum,
                                                DocType = "SI",
                                                SINo = b.SInvoice,
                                                Total = b.DocTotal,
                                                GrossTotal = b.GrossTotal,
                                                TransactionDate = b.Date,
                                                DueDate = b.DueDate,
                                                Reference = b.Reference ?? "",
                                                Status = GlobalFunctions.GetTransStatusValue((int)b.Status)
                                            }).ToList()
            //purchaseinvoice
            .Union(db.PurchaseInvoices.Where(x => x.CardCode == id &&
                                            (x.Status == Enums.InvoiceType.PARTIALLY_PAID ||
                                            x.Status == Enums.InvoiceType.UNPAID)).ToList()
                                            .Select(b => new BPBalance_Results()
                                            {
                                                DocEntry = b.DocEntry,
                                                DocNum = b.DocNum,
                                                DocType = "PI",
                                                SINo = b.PInvoice,
                                                Total = b.DocTotal,
                                                GrossTotal = b.GrossTotal,
                                                TransactionDate = b.Date,
                                                DueDate = b.DueDate,
                                                Reference = b.Reference ?? "",
                                                Status = GlobalFunctions.GetTransStatusValue((int)b.Status)
                                            }).ToList()).ToList();

            return Ok(invoices);

        }

        /// <summary>
        /// EditBusinessPartner
        /// </summary>
        /// <param name="id"></param>
        /// <param name="bp"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPut, System.Web.Http.Route("portal/api/businesspartner/{id}")]
        public IHttpActionResult EditBusinessPartner(string id, [FromBody]BusinessPartner bp)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != bp.CardCode)
            {
                return BadRequest();
            }
            //bpaddress
            var bpaddress = db.BpAddresses.Where(x => x.CardCode == bp.CardCode).ToList();
            if (bp.BpAddresses != null)
            {
                foreach (BpAddress address in bpaddress)
                {
                    db.BpAddresses.Remove(address);
                }
                foreach (BpAddress address in bp.BpAddresses)
                {
                    db.BpAddresses.Add(address);
                }
            }
            //bpwtax
            var bpwtax = db.BpWTax.Where(x => x.CardCode == bp.CardCode).ToList();
            if (bp.BpWTax != null)
            {
                foreach (BpWTax wtax in bpwtax)
                {
                    db.BpWTax.Remove(wtax);
                }
                foreach (BpWTax wtax in bp.BpWTax)
                {
                    db.BpWTax.Add(wtax);
                }
            }
            db.Entry(bp).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BusinessPartnerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(bp);
        }

        /// <summary>
        /// AddBusinessPartner
        /// </summary>
        /// <param name="bp"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost, System.Web.Http.Route("portal/api/businesspartner")]
        public IHttpActionResult AddBusinessPartner([FromBody]BusinessPartner bp)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                //check branch if exists
                var check = db.BusinessPartners.Where(x => x.CardCode.ToUpper().Trim() == bp.CardCode.ToUpper().Trim()).Any();
                if (check)
                {
                    return BadRequest("Business Partner already exists! Please create different business partner.");
                }
                var identity = (ClaimsIdentity)User.Identity;
                bp.CreatedById = identity.Name;

                db.BusinessPartners.Add(bp);
                db.SaveChanges();

                //Update Sequence Table
                var series = Convert.ToInt32(bp.Series);
                var numbering = db.SequenceTableLines.SingleOrDefault(x => x.Series == series);
                if (numbering != null)
                {
                    numbering.LastNum = numbering.NextNumber;
                    numbering.NextNumber = numbering.NextNumber + 1;
                    db.SaveChanges();
                }
                //>>end

                return Ok(bp);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            //return CreatedAtRoute("DefaultApi", new { id = branch.Code }, branch);
        }

        /// <summary>
        /// DeleteBussinessPartner 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [System.Web.Http.HttpDelete, System.Web.Http.Route("portal/api/businesspartner/{id}")]
        public IHttpActionResult DeleteBussinessPartner(string id)
        {
            BusinessPartner bp = db.BusinessPartners.Find(id);
            if (bp == null)
            {
                return NotFound();
            }

            db.BusinessPartners.Remove(bp);
            db.SaveChanges();

            return Ok(bp);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BusinessPartnerExists(string id)
        {
            return db.BusinessPartners.Count(e => e.CardCode == id) > 0;
        }
    }
}