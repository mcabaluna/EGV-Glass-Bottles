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
    public class InvAdjustmentsController : ApiController
    {
        private DbContextModel db = new DbContextModel();


        /// <summary>
        /// GetInvAdjustmentList
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.Route("portal/api/invadjustment/getinvadjustmentlist")]
        public IHttpActionResult GetInvAdjustmentList(string search = null)
        {
            var invadjustment = new List<InvAdjustment>();
            List<InvAdjustmentListVM> list = new List<InvAdjustmentListVM>();
            //get all roles with filter 
            invadjustment = db.InvAdjustments
                         .OrderByDescending(x => x.DocEntry)
                         .ToList();
            //
            if (invadjustment.Count > 0)
            {
                list = invadjustment.Select(x => new InvAdjustmentListVM()
                {
                    DocEntry = x.DocEntry,
                    DocNum = x.DocNum,
                    InvAdjustmentNo =x.InvAdjustmentNo,
                    BranchCode = db.Branches.FirstOrDefault(b => b.Code == x.BranchCode)?.Name,
                    DocTotal = x.DocTotal,
                    Type = GlobalFunctions.GetAdjustmentTypeValue((int)x.Type),
                    Date = x.Date,
                    Reference = x.Reference,
                    Reason = x.Reason,
                    Status = GlobalFunctions.GetStatusValue((int)x.Status),
                    InvoiceStatus = GlobalFunctions.GetInvoiceStatusValue((int)x.InvoiceStatus)
                }).ToList();

                //Search fields
                if (!string.IsNullOrEmpty(search))
                {
                    list = list.Where(x =>
                            x.Type.ToLower().Contains(search.ToLower()) ||
                            x.Reason.ToString().ToLower().Contains(search.ToLower()) ||
                            x.Reference.ToString().ToLower().Contains(search.ToLower()) ||
                            x.BranchCode.ToString().ToLower().Contains(search.ToLower()) ||
                            x.Status.ToString().ToLower().Contains(search.ToLower()) ||
                            x.Reference.ToString().ToLower().Contains(search.ToLower()))
                        .OrderByDescending(x => x.DocEntry)
                        .ToList();
                }
            }
            return Ok(list);
        }
        /// <summary>
        /// GetInvAdjustment
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("portal/api/invadjustment/getinvadjustmentinfo")]
        public IHttpActionResult GetInvAdjustment()
        {
            var invadjust = db.InvAdjustments.Include(x => x.Lines).ToList();
            return Ok(invadjust);
        }
        /// <summary>
        /// GetInvAdjustment
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.Route("portal/api/invadjustment/{id}")]
        public IHttpActionResult GetInvAdjustment(int id)
        {
            try
            {
                InvAdjustment invadjust = db.InvAdjustments.Include(x => x.Lines).SingleOrDefault(x => x.DocEntry == id);
                if (invadjust == null)
                {
                    return NotFound();
                }

                return Ok(invadjust);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// EditInvAdjustment
        /// </summary>
        /// <param name="id"></param>
        /// <param name="invoice"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPut, System.Web.Http.Route("portal/api/invadjustment/{id}")]
        public IHttpActionResult EditInvAdjustment(int id, [FromBody]InvAdjustment invadjustment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != invadjustment.DocEntry)
            {
                return BadRequest();
            }
            //lines
            var lines = db.InvAdjustmentLines.Where(p => p.DocEntry == invadjustment.DocEntry);
            if (invadjustment.Lines != null)
            {
                foreach (InvAdjustmentLines item in lines)
                {
                    db.InvAdjustmentLines.Remove(item);
                }
                foreach (InvAdjustmentLines item in invadjustment.Lines)
                {
                    db.InvAdjustmentLines.Add(item);
                    db.SaveChanges();
                }
            }
            db.Entry(invadjustment).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InvAdjustmentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(invadjustment);
        }
        /// <summary>
        /// AddInvAdjustment
        /// </summary>
        /// <param name="invadjustment"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost, System.Web.Http.Route("portal/api/invadjustment")]
        public IHttpActionResult AddInvAdjustment(InvAdjustment invadjustment)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                //check invadjustment if exists
                var check = db.InvAdjustments.Where(x => x.DocEntry == invadjustment.DocEntry).Any();
                if (check)
                {
                    return BadRequest("Inv. Adjustment already exists! Please create different inv. adjustment.");
                }
                var identity = (ClaimsIdentity)User.Identity;
                invadjustment.CreatedById = identity.Name;

                //Validate Sequence
                //var numbering = (from i in db.SequenceTables.Where(o => o.Document == "INVJ" && o.SeriesName == invadjustment.Series)
                //                 select new { i.SeriesName, i.Prefix, i.NextNumber, i.Suffix }).ToList();

                //invadjustment.InvAdjustmentNo = numbering.FirstOrDefault().Suffix != null ?
                //                numbering.FirstOrDefault().Prefix + numbering.FirstOrDefault().NextNumber + "_" + numbering.FirstOrDefault().Suffix :
                //                numbering.FirstOrDefault().Prefix + numbering.FirstOrDefault().NextNumber;
                //invadjustment.DocNum = Convert.ToInt32(numbering.FirstOrDefault().NextNumber);
                db.InvAdjustments.Add(invadjustment);
                db.SaveChanges();

                //Update Sequence Table
                //string series = numbering.FirstOrDefault().SeriesName;
                //SequenceTable seq = db.SequenceTables.FirstOrDefault(i => i.SeriesName == series && i.Document == "INVJ");
                ////seq.NextNumber = numbering.FirstOrDefault().NextNumber + 1;
                //seq.NextNumber = string.Format("{0:00}", (int.Parse(numbering.FirstOrDefault().NextNumber) + 1));
                //seq.LastNumber = numbering.FirstOrDefault().NextNumber;

                //Update Sequence Table
                var series = Convert.ToInt32(invadjustment.Series);
                var numbering = db.SequenceTableLines.SingleOrDefault(x => x.Series == series);
                if (numbering != null)
                {
                    numbering.LastNum = numbering.NextNumber;
                    numbering.NextNumber = numbering.NextNumber + 1;
                    db.SaveChanges();
                }
                //>>end
                //>>end

                //Insert On Hand per Item : 2020-02-08
                foreach (var lines in invadjustment.Lines)
                {
                    var item = db.Items.Include(x => x.ItemOnHandPerWhse).FirstOrDefault(x => x.ItemCode == lines.ItemCode);
                    if (invadjustment.Type == Enums.AdjustmentType.STOCK_IN)
                    {
                        //add stock
                        if (item.ItemOnHandPerWhse.Count == 0 || !item.ItemOnHandPerWhse.Any(x => x.ItemCode == lines.ItemCode && x.WhseId == lines.Whse))
                        {
                            ItemOnHandPerWhse itemOnHandPerWhse = new ItemOnHandPerWhse()
                            {
                                ItemCode = lines.ItemCode,
                                WhseId = lines.Whse,
                                OnHand = lines.Quantity,
                                ItemCost = lines.UnitPrice
                            };
                            db.ItemOnHandPerWhse.Add(itemOnHandPerWhse);
                            db.SaveChanges();
                        }
                        else
                        {
                            //Deduct stock
                            foreach (var WhseOnhand in item.ItemOnHandPerWhse.Where(x => x.ItemCode == lines.ItemCode && x.WhseId == lines.Whse))
                            {
                                WhseOnhand.OnHand += lines.Quantity;
                                WhseOnhand.ItemCost = lines.UnitPrice;
                                db.SaveChanges();
                            }
                        }
                    }
                    else
                    {
                        foreach (var WhseOnhand in item.ItemOnHandPerWhse.Where(x => x.ItemCode == lines.ItemCode && x.WhseId == lines.Whse))
                        {
                            WhseOnhand.OnHand -= lines.Quantity;
                            WhseOnhand.ItemCost = lines.UnitPrice;
                            db.SaveChanges();
                        }
                    }
                }
                //>> end

                return Ok(invadjustment);
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
        /// CancelledInvAdjustment
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [System.Web.Http.HttpDelete, System.Web.Http.Route("portal/api/invadjustment/{id}")]
        public IHttpActionResult CancelledInvAdjustment(int id)
        {
            InvAdjustment invadjustment = db.InvAdjustments.Find(id);
            if (invadjustment == null)
            {
                return NotFound();
            }
            invadjustment.InvoiceStatus = Enums.InvoiceStatus.CANCELLED;
            invadjustment.Status = Enums.StatusType.IN_ACTIVE;
            db.Entry(invadjustment).State = EntityState.Modified;
            db.SaveChanges();
            return Ok(invadjustment);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool InvAdjustmentExists(int id)
        {
            return db.InvAdjustments.Count(e => e.DocEntry == id) > 0;
        }
    }
}