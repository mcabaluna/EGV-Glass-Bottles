using Merchandising.DTO;
using Merchandising.DTO.Models;
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
    public class SequenceTablesController : ApiController
    {
        private DbContextModel db = new DbContextModel();

        /// <summary>
        /// GetSequenceList
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.Route("portal/api/sequencetable/getsequencelist")]
        public IHttpActionResult GetSequenceList(string search = null)
        {
            List<SequenceListVM> list = new List<SequenceListVM>();

            //list = (from n in db.SequenceDocument.ToList().DefaultIfEmpty()
            //        join a in db.SequenceTables.ToList().DefaultIfEmpty() on n.ObjectCode equals a.ObjectCode
            //        join b in db.SequenceTableLines.ToList().DefaultIfEmpty() on a.ObjectCode equals b.ObjectCode
            //        select new SequenceListVM()
            //        {
            //            Document = n.DocumentName,
            //            DefaultSeries = b.SeriesName,
            //            InitialNum = b.InitialNum.ToString(),
            //            NextNumber = b.NextNumber.ToString(),
            //            LastNum = b.LastNum.ToString()
            //        }).ToList();
            list = (from n in db.SequenceDocument.DefaultIfEmpty()
                    from a in db.SequenceTables.Where(o => o.ObjectCode == n.ObjectCode)
                    from b in db.SequenceTableLines.Where(o => o.Id == a.Id && o.Indicator == true).Distinct().DefaultIfEmpty()
                    select new SequenceListVM()
                    {
                        //Id = a.Id == null ? 0 : a.Id,
                        Id = a.Id,
                        ObjectCode = n.ObjectCode,
                        Document = n.DocumentName,
                        DocumentType = n.DocType,
                        DocSubType = n.DocSubType,
                        Series = a.DefaultSeries,
                        DefaultSeries = a.DefaultSeries > 0 ? b.SeriesName : "",
                        InitialNum = a.DefaultSeries > 0 ? b.InitialNum.ToString() : "",
                        NextNumber = a.DefaultSeries > 0 ? b.NextNumber.ToString() : "",
                        LastNum = a.DefaultSeries > 0 ? b.LastNum.ToString() : ""

                    }).Distinct().ToList();

            ////Search fields
            //if (!string.IsNullOrEmpty(search))
            //{
            //    sequence = sequence.Where(x =>
            //            x.Document.ToLower().Contains(search.ToLower()) ||
            //            x.DocType.ToString().ToLower().Contains(search.ToLower()) ||
            //            x.Prefix.ToString().ToLower().Contains(search.ToLower()) ||
            //            x.SeriesName.ToString().ToLower().Contains(search.ToLower()) ||
            //            x.Suffix.ToString().ToLower().Contains(search.ToLower()) ||
            //             x.InitialNum.ToString().ToLower().Contains(search.ToLower()) ||
            //              x.NextNumber.ToString().ToLower().Contains(search.ToLower()) ||
            //            x.LastNumber.ToString().ToLower().Contains(search.ToLower()))
            //        .OrderByDescending(x => x.Document)
            //        .ToList();
            //}
            //if (sequence.Count > 0)
            //{
            //    list = sequence.Select(x => new SequenceListVM()
            //    {
            //        Id = x.Id,
            //        Document = x.Document,
            //        DocumentType =x.DocType,
            //        Prefix = x.Prefix,
            //        Suffix= x.Suffix,
            //        InitialNum = x.InitialNum,
            //        NextNumber = x.NextNumber,
            //        LastNumber = x.LastNumber,
            //        BranchCode = x.BranchCode
            //    }).ToList();
            //}
            return Ok(list);
        }

        /// <summary>
        /// GetSequenceInfo
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("portal/api/sequencetable/getsequenceinfo")]
        public IHttpActionResult GetSequenceInfo()
        {
            var sequence = db.SequenceTables.Include(x => x.Lines).ToList();
            return Ok(sequence);
        }

        /// <summary>
        /// GetSequence
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.Route("portal/api/sequencetable/{id}")]
        public IHttpActionResult GetSequence(int id)
        {
            SequenceTable sequence = db.SequenceTables.Include(x => x.Lines)?.SingleOrDefault(x => x.Id == id);
            if (sequence == null)
            {
                return NotFound();
            }

            return Ok(sequence);
        }

        //[System.Web.Http.HttpGet, System.Web.Http.Route("portal/api/sequencetable/checktransactions")]
        //public IHttpActionResult CheckTransactions(string type)
        //{
        //    CheckTransaction_Results results = new CheckTransaction_Results();
        //    List<SubDocument_Results> subresults = new List<SubDocument_Results>();
        //    switch (type)
        //    {
        //        case "ITEM":
        //            results.Document = type.ToUpper().Trim();
        //            //Sales Invoice
        //            //var salestrans = db.SalesInvoices.Select(x => new {  x.BranchCode }).Distinct().ToList();
        //            var salestrans = db.SalesInvoiceLines.Select(x => x.ItemCode).Distinct().ToList();

        //            foreach (var sales in salestrans)
        //            {
        //                if (subresults.Any(x => x.BranchCode != sales.BranchCode && x.Series != sales.Series))
        //                {
        //                    SubDocument_Results sub = new SubDocument_Results();
        //                    sub.BranchCode = sales.BranchCode;
        //                    sub.Series = sales.Series;
        //                    subresults.Add(sub);
        //                }
        //            }
        //            //Purchase Invoice
        //            var purchtrans = db.PurchaseInvoices.Select(x => new { x.Series, x.BranchCode }).Distinct().ToList();
        //            foreach (var purchase in purchtrans)
        //            {
        //                if (subresults.Any(x => x.BranchCode != purchase.BranchCode && x.Series != purchase.Series))
        //                {
        //                    SubDocument_Results sub = new SubDocument_Results();
        //                    sub.BranchCode = purchase.BranchCode;
        //                    sub.Series = purchase.Series;
        //                    subresults.Add(sub);
        //                }
        //            }
        //            //Inventory Adjustment  
        //            var invtrans = db.InvAdjustments.Select(x => new { x.BranchCode, x.Series }).Distinct().ToList();
        //            foreach (var inv in invtrans)
        //            {
        //                if (subresults.Any(x => x.BranchCode != inv.BranchCode && x.Series != inv.Series))
        //                {
        //                    SubDocument_Results sub = new SubDocument_Results();
        //                    sub.BranchCode = inv.BranchCode;
        //                    sub.Series = inv.Series;
        //                    subresults.Add(sub);
        //                }
        //            }
        //            break;
        //        case "BUSINESS PARTNER - CUSTOMER":
        //            results.Document = type.ToUpper().Trim();
        //            var custtrans = db.SalesInvoices.Select(x => new { x.BranchCode, x.Series }).Distinct().ToList();
        //            foreach (var cust in custtrans)
        //            {
        //                if (subresults.Any(x => x.BranchCode != cust.BranchCode && x.Series != cust.Series))
        //                {
        //                    SubDocument_Results sub = new SubDocument_Results();
        //                    sub.BranchCode = cust.BranchCode;
        //                    sub.Series = cust.Series;
        //                    subresults.Add(sub);
        //                }
        //            }
        //            break;
        //        case "BUSINESS PARTNER - VENDOR":
        //            results.Document = type.ToUpper().Trim();
        //            var ventrans = db.PurchaseInvoices.Select(x => new { x.BranchCode, x.Series }).Distinct().ToList();
        //            foreach (var vendor in ventrans)
        //            {
        //                if (subresults.Any(x => x.BranchCode != vendor.BranchCode && x.Series != vendor.Series))
        //                {
        //                    SubDocument_Results sub = new SubDocument_Results();
        //                    sub.BranchCode = vendor.BranchCode;
        //                    sub.Series = vendor.Series;
        //                    subresults.Add(sub);
        //                }
        //            }
        //            break;
        //        case "PRICELIST":
        //            results.Document = type.ToUpper().Trim();
        //            var plisttrans = db.BusinessPartners.Select(x => new { x.PricelistId }).Distinct().ToList();
        //            foreach (var ven in plisttrans)
        //            {
        //                if (subresults.Any(x => x.Code != ven))
        //                {
        //                    SubDocument_Results sub = new SubDocument_Results();
        //                    sub.Code = ven;
        //                    subresults.Add(sub);
        //                }
        //            }
        //            break;
        //        case "SALES INVOICE":
        //            break;
        //        case "PURCHASE INVOICE":
        //            break;
        //        case "COLLECTION":
        //            break;
        //        case "PAYMENTS":
        //            break;
        //        case "INVENTORY ADJUSTMENT":
        //            break;
        //    }
        //}

        /// <summary>
        /// EditSequence
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sequence"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPut, System.Web.Http.Route("portal/api/sequencetable/{id}")]
        public IHttpActionResult EditSequence(int id, [FromBody]SequenceTable sequence)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != sequence.Id)
            {
                return BadRequest();
            }

            try
            {
                if (sequence.Lines != null)
                {
                    //Update if existing Sequence Lines
                    foreach (var lines in sequence.Lines)
                    {
                        if (db.SequenceTableLines.Any(x => x.Series == lines.Series))
                        {
                            //update sequence lines
                            db.Entry(lines).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        else
                        {
                            //add sequence lines
                            db.SequenceTableLines.Add(lines);
                            db.SaveChanges();
                        }
                        //Update default series
                        if (lines.Indicator)
                        {
                            var seqlines = db.SequenceTableLines.Find(lines.Series);

                            if (seqlines != null)
                            {
                                sequence.DefaultSeries = seqlines.Series;
                                db.SaveChanges();
                            }
                        }
                    }
                }
                else
                {
                    var seq = db.SequenceTables.Include(x => x.Lines).SingleOrDefault(b => b.Id == sequence.Id);
                    if (seq != null)
                    {
                        seq.DefaultSeries = sequence.DefaultSeries;
                        //Update if existing Sequence Lines
                        foreach (var lines in seq.Lines)
                        {
                            if (lines.Series == seq.DefaultSeries)
                            {
                                lines.Indicator = true;
                            }
                            else
                            {
                                lines.Indicator = false;
                            }
                            db.SaveChanges();
                        }
                        db.SaveChanges();
                    }
                }

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SequenceTableExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(sequence);
        }

        /// <summary>
        /// AddSequence
        /// </summary>
        /// <param name="sequence"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost, System.Web.Http.Route("portal/api/sequencetable")]
        public IHttpActionResult AddSequence([FromBody]SequenceTable sequence)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                //check branch if exists
                var check = db.SequenceTables.Where(x => x.Id == sequence.Id).Any();
                if (check)
                {
                    return BadRequest("Series already exists! Please create different series.");
                }
                var identity = (ClaimsIdentity)User.Identity;
                sequence.CreatedById = identity.Name;
                db.SequenceTables.Add(sequence);
                db.SaveChanges();
                return Ok(sequence);
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
        /// DeleteSequence
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [System.Web.Http.HttpDelete, System.Web.Http.Route("portal/api/sequencetable/{id}")]
        public IHttpActionResult DeleteSequence(int id)
        {
            SequenceTable sequence = db.SequenceTables.Find(id);
            if (sequence == null)
            {
                return NotFound();
            }

            db.SequenceTables.Remove(sequence);
            db.SaveChanges();

            return Ok(sequence);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SequenceTableExists(int id)
        {
            return db.SequenceTables.Count(e => e.Id == id) > 0;
        }
    }
}