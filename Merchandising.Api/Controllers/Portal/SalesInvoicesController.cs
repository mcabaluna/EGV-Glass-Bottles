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
using System.Web.Http;

namespace Merchandising.Api.Controllers.Portal
{
    public class SalesInvoicesController : ApiController
    {
        private DbContextModel db = new DbContextModel();

        /// <summary>
        /// GetSalesInvoiceList
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.Route("portal/api/salesinvoice/getsalesinvoicelist")]
        public IHttpActionResult GetSalesInvoiceList(string search = null) 
        {
            var salesinvoice = new List<SalesInvoice>();
            List<SalesInvoiceListVM> list = new List<SalesInvoiceListVM>();
            //get all roles with filter 
            salesinvoice = db.SalesInvoices
                         .OrderByDescending(x => x.DocEntry)
                         .ToList();
            //
            if (salesinvoice.Count > 0)
            {
                list = salesinvoice.Select(x => new SalesInvoiceListVM()
                {
                    DocEntry = x.DocEntry,
                    DocNum = x.DocNum,
                    SInvoice = x.SInvoice,
                    CardCode = x.CardCode,
                    CardName = x.CardName,
                    BranchCode = db.Branches.FirstOrDefault(b => b.Code == x.BranchCode)?.Name,
                    DocTotal = x.DocTotal,
                    GrossTotal = x.GrossTotal,
                    DueDate = x.DueDate,
                    Date = x.Date,
                    Reference = x.Reference,
                    Status = GlobalFunctions.GetTransStatusValue((int)x.Status)
                }).ToList();

                //Search fields
                if (!string.IsNullOrEmpty(search))
                {
                    list = list.Where(x =>
                            x.SInvoice.ToLower().Contains(search.ToLower()) ||
                            x.CardCode.ToString().ToLower().Contains(search.ToLower()) ||
                            x.CardName.ToString().ToLower().Contains(search.ToLower()) ||
                            x.Status.ToString().ToLower().Contains(search.ToLower()) ||
                            x.Reference.ToString().ToLower().Contains(search.ToLower()))
                        .OrderByDescending(x => x.DocEntry)
                        .ToList();
                }
            }
            return Ok(list);
        }
        /// <summary>
        /// GetSalesInvoice
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("portal/api/salesinvoice/getsalesinvoiceinfo")]
        public IHttpActionResult GetSalesInvoice()
        {
            var invoice = db.SalesInvoices.Include(x => x.Lines).ToList();
            return Ok(invoice);
        }
        /// <summary>
        /// GetSalesOpens
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("portal/api/salesinvoice/getsalesopen")]
        public IHttpActionResult GetSalesOpens(string code = "")
        {
            var invoice = db.SalesInvoices.Where(b => b.CardCode == code).Include(x => x.Lines).ToList();
            return Ok(invoice);
        }


        /// <summary>
        /// GetSalesInvoice
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.Route("portal/api/salesinvoice/{id}")]
        public IHttpActionResult GetSalesInvoice(int id)
        {
            try
            {
                SalesInvoice invoice = db.SalesInvoices.Include(x => x.Lines).SingleOrDefault(x => x.DocEntry == id);
                if (invoice == null)
                {
                    return NotFound();
                }

                return Ok(invoice);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// EditSalesInvoice
        /// </summary>
        /// <param name="id"></param>
        /// <param name="invoice"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPut, System.Web.Http.Route("portal/api/salesinvoice/{id}")]
        public IHttpActionResult EditSalesInvoice(int id, [FromBody]SalesInvoice invoice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != invoice.DocEntry)
            {
                return BadRequest();
            }
            //lines
            var lines = db.SalesInvoiceLines.Where(p => p.DocEntry == invoice.DocEntry);
            if (invoice.Lines != null)
            {
                foreach (SalesInvoiceLines item in lines)
                {
                    db.SalesInvoiceLines.Remove(item);
                }
                foreach (SalesInvoiceLines item in invoice.Lines)
                {
                    db.SalesInvoiceLines.Add(item);
                    db.SaveChanges();
                }
            }
            db.Entry(invoice).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SalesInvoiceExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(invoice);
        }
        /// <summary>
        /// AddSalesInvoice
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost, System.Web.Http.Route("portal/api/salesinvoice")]
        public IHttpActionResult AddSalesInvoice(SalesInvoice invoice)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                //check salesinvoice if exists
                var check = db.SalesInvoices.Where(x => x.DocEntry == invoice.DocEntry).Any();
                if (check)
                {
                    return BadRequest("Sales Invoice already exists! Please create different salesinvoice.");
                }
                var identity = (ClaimsIdentity)User.Identity;
                invoice.CreatedById = identity.Name;

                //Validate Sequence
                //var numbering = (from i in db.SequenceTables.Where(o => o.Document == "OINV" && o.SeriesName == invoice.Series)
                //                 select new { i.SeriesName, i.Prefix, i.NextNumber, i.Suffix }).ToList();

                //invoice.SInvoice = numbering.FirstOrDefault().Suffix != null ?
                //                numbering.FirstOrDefault().Prefix + numbering.FirstOrDefault().NextNumber + "_" + numbering.FirstOrDefault().Suffix :
                //                numbering.FirstOrDefault().Prefix + numbering.FirstOrDefault().NextNumber;
                //invoice.DocNum = Convert.ToInt32(numbering.FirstOrDefault().NextNumber);
                db.SalesInvoices.Add(invoice);
                db.SaveChanges();

                //Update Sequence Table
                //string series = numbering.FirstOrDefault().SeriesName;
                //SequenceTable seq = db.SequenceTables.FirstOrDefault(i => i.SeriesName == series && i.Document == "OINV");
                ////seq.NextNumber = numbering.FirstOrDefault().NextNumber + 1;
                //seq.NextNumber = string.Format("{0:00}", (int.Parse(numbering.FirstOrDefault().NextNumber) + 1));
                //seq.LastNumber = numbering.FirstOrDefault().NextNumber;

                //Update Sequence Table
                var series = Convert.ToInt32(invoice.Series);
                var numbering = db.SequenceTableLines.SingleOrDefault(x => x.Series == series);
                if (numbering != null)
                {
                    numbering.LastNum = numbering.NextNumber;
                    numbering.NextNumber = numbering.NextNumber + 1;
                    db.SaveChanges();
                }
                //>>end
                //>>end

                //Update Balance of Business Partner : 2020-02-08
                BusinessPartner bp = db.BusinessPartners.FirstOrDefault(x => x.CardCode == invoice.CardCode);
                bp.Balance += invoice.GrossTotal;
                db.SaveChanges();
                //>>end
                return Ok(invoice);
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
        ///// <summary>
        ///// DeleteSalesInvoice
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //[System.Web.Http.HttpDelete, System.Web.Http.Route("portal/api/salesinvoice/{id}")]
        //public IHttpActionResult DeleteSalesInvoice(int id)
        //{
        //    SalesInvoice invoice = await db.SalesInvoices.FindAsync(id);
        //    if (invoice == null)
        //    {
        //        return NotFound();
        //    }

        //    db.SalesInvoices.Remove(invoice);
        //    db.SaveChanges();

        //    return Ok(invoice);
        //}
        /// <summary>
        /// CancelledSalesInvoice
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [System.Web.Http.HttpDelete, System.Web.Http.Route("portal/api/salesinvoice/{id}")]
        public IHttpActionResult CancelledSalesInvoice(int id)
        {
            SalesInvoice invoice = db.SalesInvoices.Find(id);
            if (invoice == null)
            {
                return NotFound();
            }
            invoice.Status = Enums.InvoiceType.CANCELED;
            db.Entry(invoice).State = EntityState.Modified;
            db.SaveChanges();
            return Ok(invoice);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        private bool SalesInvoiceExists(int id)
        {
            return db.SalesInvoices.Count(e => e.DocEntry == id) > 0;
        }
    }
}