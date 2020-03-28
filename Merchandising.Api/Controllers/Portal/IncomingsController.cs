using Merchandising.DTO;
using Merchandising.DTO.Models;
using Merchandising.VM.Portal;
using Merchandising.VM.Results;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;

namespace Merchandising.Api.Controllers.Portal
{
    public class IncomingsController : ApiController
    {
        private DbContextModel db = new DbContextModel();

        /// <summary>
        /// GetIncomingsList
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.Route("portal/api/incomings/getincomingslist")]
        public IHttpActionResult GetIncomingsList(string search = null)
        {
            var incoming = new List<Incomings>();
            List<IncomingsListVM> list = new List<IncomingsListVM>();
            incoming = db.Incomings.Include(b => b.Lines)
                        .OrderByDescending(x => x.DocEntry)
                        .ToList();
            if (incoming.Count > 0)
            {
                list = incoming.Select(x => new IncomingsListVM()
                {
                    BranchCode = db.Branches.FirstOrDefault(b => b.Code == x.BranchCode)?.Name,
                    CardCode = x.CardCode,
                    CardName = x.CardName,
                    Status = x.Status,
                    DocEntry = x.DocEntry,
                    DocNum = x.DocNum,
                    PaymentNo = x.PaymentNo,
                    //InvoiceNo = x.InvoiceNo,
                    //Collections = x.Collections,
                    //Balance = x.Balance,
                    InvoiceType = x.Lines.FirstOrDefault().InvType,
                    AmountPaid = x.AmountPaid,
                    DueDate = x.DueDate,
                    DatePaid = x.DatePaid,
                    Remarks = x.Remarks
                }).ToList();

                //Search fields
                if (!string.IsNullOrEmpty(search))
                {
                    list = list.Where(x =>
                            x.CardCode.ToLower().Contains(search.ToLower()) ||
                            x.CardName.ToString().ToLower().Contains(search.ToLower()))
                        .OrderByDescending(x => x.DocEntry)
                        .ToList();
                }
            }


            return Ok(list);
        }

        /// <summary>
        /// GetIncomings
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("portal/api/incomings/getincomingsinfo")]
        public IHttpActionResult GetIncomings()
        {
            var incoming = db.Incomings.ToList();
            return Ok(incoming);
        }
        /// <summary>
        /// GetInvoices
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("portal/api/incomings/getinvoices")]
        public IHttpActionResult GetInvoices(string cardcode, string branch)
        {
            var list = db.SalesInvoices.Where(x => x.CardCode == cardcode.Trim() && (x.Status == Enums.InvoiceType.PARTIALLY_PAID ||
                      x.Status == Enums.InvoiceType.UNPAID) && x.BranchCode == branch).
                      Select(b => new Incoming_Results()
                      {
                          DocEntry = b.DocEntry,
                          InvoiceNo = b.SInvoice,
                          DocNum = b.DocNum,
                          DueDate = b.DueDate,
                          CardCode = b.CardCode,
                          CardName = b.CardName,
                          GrossTotal = b.GrossTotal,
                          Collection = b.PaidToDate,
                          Balance = b.GrossTotal - b.PaidToDate
                      })
                      .Union(db.PurchaseInvoices.Where(x => x.CardCode == cardcode.Trim() && (x.Status == Enums.InvoiceType.PARTIALLY_PAID ||
                      x.Status == Enums.InvoiceType.UNPAID) && x.BranchCode == branch).
                      Select(b => new Incoming_Results()
                      {
                          DocEntry = b.DocEntry,
                          InvoiceNo = b.PInvoice,
                          DocNum = b.DocNum,
                          DueDate = b.DueDate,
                          CardCode = b.CardCode,
                          CardName = b.CardName,
                          GrossTotal = b.GrossTotal,
                          Collection = b.PaidToDate,
                          Balance = b.GrossTotal - b.PaidToDate
                      })).ToList();

            return Ok(list);
        }

        /// <summary>
        /// GetIncomings
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.Route("portal/api/incomings/{id}")]
        public IHttpActionResult GetIncomings(int id)
        {
            Incomings incoming = db.Incomings.Include(x => x.Lines).FirstOrDefault(x => x.DocEntry == id);
            if (incoming == null)
            {
                return NotFound();
            }

            return Ok(incoming);
        }

        /// <summary>
        /// EditIncomings
        /// </summary>
        /// <param name="id"></param>
        /// <param name="incoming"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPut, System.Web.Http.Route("portal/api/incomings/{id}")]
        public IHttpActionResult EditIncomings(int id, [FromBody]Incomings incoming)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != incoming.DocEntry)
            {
                return BadRequest();
            }

            db.Entry(incoming).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IncomingsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }


            return Ok(incoming);
        }

        /// <summary>
        /// AddIncomings
        /// </summary>
        /// <param name="incoming"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost, System.Web.Http.Route("portal/api/incomings")]
        public IHttpActionResult AddIncomings([FromBody]Incomings incoming)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                //check branch if exists
                var check = db.Incomings.Where(x => x.DocEntry == incoming.DocEntry).Any();
                if (check)
                {
                    return BadRequest("Incoming already exists! Please create different incoming.");
                }
                var identity = (ClaimsIdentity)User.Identity;
                incoming.CreatedById = identity.Name;

                //Validate Sequence
                //var numbering = (from i in db.SequenceTables.Where(o => o.Document == "ORCT" && o.SeriesName == incoming.Series)
                //                 select new { i.SeriesName, i.Prefix, i.NextNumber, i.Suffix }).ToList();

                //incoming.PaymentNo = numbering.FirstOrDefault().Suffix != null ?
                //                numbering.FirstOrDefault().Prefix + numbering.FirstOrDefault().NextNumber + "_" + numbering.FirstOrDefault().Suffix :
                //                numbering.FirstOrDefault().Prefix + numbering.FirstOrDefault().NextNumber;
                //incoming.DocNum = Convert.ToInt32(numbering.FirstOrDefault().NextNumber);
                int objecttype = Convert.ToInt32(incoming.ObjectType);
                int series = 0;
                if (incoming.Series == null || incoming.Series == "")
                {
                    series = db.SequenceTables.FirstOrDefault(x => x.ObjectCode == objecttype).DefaultSeries;
                    incoming.Series = series.ToString();
                }
                else
                {
                    series = Convert.ToInt32(incoming.Series);
                }
                var newres = db.SequenceTableLines.SingleOrDefault(x => x.ObjectCode == objecttype && x.Series == series);
                if (newres != null)
                {
                    if (newres.NumSize > 0)
                    {
                        var nextnumber = newres.NextNumber.ToString();
                        var numsize = newres.NumSize;
                        var concatnum = nextnumber.PadLeft(nextnumber.Length + numsize - nextnumber.Length, '0');
                        //Master Data
                        incoming.PaymentNo = newres.LastStr != null ? newres.BeginStr + concatnum + "_" + newres.LastStr :
                                          newres.BeginStr + concatnum;
                        incoming.DocNum = newres.NextNumber;
                    }
                    else
                    {
                        //Transactional

                        incoming.PaymentNo = newres.LastStr != null ? newres.BeginStr + newres.NextNumber + "_" + newres.LastStr :
                                      newres.BeginStr + newres.NextNumber;
                        incoming.DocNum = newres.NextNumber;
                    }
                }


                db.Incomings.Add(incoming);
                db.SaveChanges();

                //Update Sequence Table
                //string series = numbering.FirstOrDefault().SeriesName;
                //SequenceTable seq = db.SequenceTables.FirstOrDefault(i => i.SeriesName == series && i.Document == "ORCT");
                ////seq.NextNumber = numbering.FirstOrDefault().NextNumber + 1;
                //seq.NextNumber = string.Format("{0:00}", (int.Parse(numbering.FirstOrDefault().NextNumber) + 1));
                //seq.LastNumber = numbering.FirstOrDefault().NextNumber;

                //Update Sequence Table
                var numbering = db.SequenceTableLines.SingleOrDefault(x => x.Series == series);
                if (numbering != null)
                {
                    numbering.LastNum = numbering.NextNumber;
                    numbering.NextNumber = numbering.NextNumber + 1;
                    db.SaveChanges();
                }
                //>>end

                //Add Collection Paid To Date in Invoice : 2020-02-09
                foreach (var inv in incoming.Lines)
                {
                    if (inv.InvType.Equals("SI"))
                    {
                        SalesInvoice si = db.SalesInvoices.SingleOrDefault(x => x.SInvoice == inv.InvoiceNo);
                        if (si.GrossTotal == si.PaidToDate + inv.SumApplied)
                        {
                            si.PaidToDate += inv.SumApplied;
                            si.Status = Enums.InvoiceType.FULLYPAID;
                        }
                        else if (si.GrossTotal > si.PaidToDate + inv.SumApplied)
                        {
                            si.PaidToDate += inv.SumApplied;
                            si.Status = Enums.InvoiceType.PARTIALLY_PAID;
                        }

                        db.SaveChanges();
                    }
                    else
                    {
                        PurchaseInvoice pi = db.PurchaseInvoices.SingleOrDefault(x => x.PInvoice == inv.InvoiceNo);

                        if (pi.GrossTotal == pi.PaidToDate + inv.SumApplied)
                        {
                            pi.PaidToDate += inv.SumApplied;
                            pi.Status = Enums.InvoiceType.FULLYPAID;
                        }
                        else if (pi.GrossTotal > pi.PaidToDate + inv.SumApplied)
                        {
                            pi.PaidToDate += inv.SumApplied;
                            pi.Status = Enums.InvoiceType.PARTIALLY_PAID;
                        }
                        db.SaveChanges();
                    }

                    //deduct BP Balance
                    BusinessPartner bp = db.BusinessPartners.SingleOrDefault(x => x.CardCode == incoming.CardCode);
                    bp.Balance = bp.Balance - inv.SumApplied;
                    db.SaveChanges();
                }
                //>end

                return Ok(incoming);
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
        /// DeleteIncomings
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //[System.Web.Http.HttpDelete, System.Web.Http.Route("portal/api/incomings/{id}")]
        //public IHttpActionResult DeleteIncomings(int id)
        //{
        //    Incomings incoming = db.Incomings.Find(id);
        //    if (incoming == null)
        //    {
        //        return NotFound();
        //    }

        //    db.Incomings.Remove(incoming);
        //    db.SaveChanges();

        //    return Ok(incoming);
        //}

        [System.Web.Http.HttpDelete, System.Web.Http.Route("portal/api/incomings/{id}")]
        public IHttpActionResult CancelledIncomings(int id)
        {
            Incomings incomings = db.Incomings.Include(b => b.Lines).SingleOrDefault(x => x.DocEntry == id);
            if (incomings == null)
            {
                return NotFound();
            }

            //Update Incoming Status to InActive
            incomings.Status = false;
            db.Entry(incomings).State = EntityState.Modified;
            db.SaveChanges();

            //Add Collection Paid To Date in Invoice : 2020-02-09
            foreach (var inv in incomings.Lines)
            {
                if (inv.InvType.Equals("SI"))
                {
                    SalesInvoice si = db.SalesInvoices.SingleOrDefault(x => x.SInvoice == inv.InvoiceNo);
                    if (si.GrossTotal == si.PaidToDate - inv.SumApplied)
                    {
                        si.PaidToDate = si.PaidToDate - inv.SumApplied;
                        si.Status = Enums.InvoiceType.FULLYPAID;
                    }
                    else if (si.PaidToDate - inv.SumApplied != 0)
                    {
                        si.PaidToDate = si.PaidToDate - inv.SumApplied;
                        si.Status = Enums.InvoiceType.PARTIALLY_PAID;
                    }
                    else if (si.PaidToDate - inv.SumApplied == 0)
                    {
                        si.PaidToDate = si.PaidToDate - inv.SumApplied;
                        si.Status = Enums.InvoiceType.UNPAID;
                    }
                    db.SaveChanges();
                }
                else
                {
                    PurchaseInvoice pi = db.PurchaseInvoices.SingleOrDefault(x => x.PInvoice == inv.InvoiceNo);

                    if (pi.GrossTotal == pi.PaidToDate - inv.SumApplied)
                    {
                        pi.PaidToDate = pi.PaidToDate - inv.SumApplied;
                        pi.Status = Enums.InvoiceType.FULLYPAID;
                    }
                    else if (pi.PaidToDate - inv.SumApplied != 0)
                    {
                        pi.PaidToDate = pi.PaidToDate - inv.SumApplied;
                        pi.Status = Enums.InvoiceType.PARTIALLY_PAID;
                    }
                    else if (pi.PaidToDate - inv.SumApplied == 0)
                    {
                        pi.PaidToDate = pi.PaidToDate - inv.SumApplied;
                        pi.Status = Enums.InvoiceType.UNPAID;
                    }
                    db.SaveChanges();
                }

                //deduct BP Balance
                BusinessPartner bp = db.BusinessPartners.SingleOrDefault(x => x.CardCode == incomings.CardCode);
                bp.Balance = bp.Balance - inv.SumApplied;
                db.SaveChanges();
            }
            //>end


            return Ok(incomings);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool IncomingsExists(int id)
        {
            return db.Incomings.Count(e => e.DocEntry == id) > 0;
        }

        #region"LOGIN"

        #endregion
    }
}
