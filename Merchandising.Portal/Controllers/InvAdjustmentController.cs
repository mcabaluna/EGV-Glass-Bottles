using Merchandising.DTO.Models;
using Merchandising.Enums;
using Merchandising.Portal.Models;
using Merchandising.VM.Portal;
using Omu.AwesomeMvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Merchandising.Portal.Controllers
{
    public class InvAdjustmentController : Controller
    {
        #region " View Method"
        public ActionResult Index()
        {
            //Status
            var statusData = from InvoiceStatus e in Enum.GetValues(typeof(InvoiceStatus))
                             select new SelectListItem
                             {
                                 Value = Convert.ToString((int)e),
                                 Text = e.ToString()
                             };
            //Type
            var typedata = from AdjustmentType e in Enum.GetValues(typeof(AdjustmentType))
                           select new SelectListItem
                           {
                               Value = Convert.ToString((int)e),
                               Text = e.ToString()
                           };
            //items
            var objitems = MerchandisingApiWrapper.Get<List<Items>>(
                       typeof(Items).Name + "/getitemsinfo");

            var item = objitems.Select(y => new { y.ItemCode, y.ItemName }).Distinct().ToList();
            IEnumerable<SelectListItem> itemlist =
                from s in item
                select new SelectListItem
                {
                    Text = s.ItemName,
                    Value = s.ItemCode
                };
            //branch
            var objbranch = MerchandisingApiWrapper.Get<List<Branch>>(
                       typeof(Branch).Name + "/getbranchinfo");

            var branch = objbranch.Select(y => new { y.Code, y.Name }).Distinct().ToList();
            IEnumerable<SelectListItem> branchlist =
                from s in branch
                select new SelectListItem
                {
                    Text = s.Name,
                    Value = s.Code
                };
            //uomgroup
            var objgroup = MerchandisingApiWrapper.Get<List<UoM>>(
                       typeof(UoM).Name + "/getuominfo");

            var uom = objgroup.Select(y => new { y.Code, y.Name }).Distinct().ToList();
            IEnumerable<SelectListItem> uomlist =
                from s in uom
                select new SelectListItem
                {
                    Text = s.Code,
                    Value = s.Name
                };

            //whse
            var objwhse = MerchandisingApiWrapper.Get<List<Warehouse>>(
                       typeof(Warehouse).Name + "/getwarehouseinfo");

            var whse = objwhse.Select(y => new { y.Code, y.Name }).Distinct().ToList();
            IEnumerable<SelectListItem> whselist =
                from s in whse
                select new SelectListItem
                {
                    Text = s.Code,
                    Value = s.Name
                };
            //getsequence
            //var objsequence = MerchandisingApiWrapper.Get<List<SequenceTable>>(
            //           typeof(SequenceTable).Name + "/getsequenceinfo");
            SequenceTableController seq = new SequenceTableController();
            var sequence = seq.GetSequenceSeries(7);
            IEnumerable<SelectListItem> serieslist =
            from s in sequence
            select new SelectListItem
            {
                Text = s.SeriesName,
                Value = s.Series.ToString()
            };
            //var series = objsequence.Where(x => x.Document == "INVJ").Select(y => new { y.SeriesName, y.Prefix, y.NextNumber, y.Suffix }).Distinct().ToList();
            //IEnumerable<SelectListItem> serieslist =
            //    from s in series
            //    select new SelectListItem
            //    {
            //        Text = s.SeriesName,
            //        Value = s.SeriesName
            //    };
            InvAdjustmentVM invoiceVM = new InvAdjustmentVM()
            {
                ItemOption = new SelectList(itemlist, "Value", "Text"),
                InvStatusOption = new SelectList(statusData, "Value", "Text"),
                SeriesOption = new SelectList(serieslist, "Value", "Text"),
                UoMOption = new SelectList(uomlist, "Value", "Text"),
                WhseOption = new SelectList(whselist, "Value", "Text"),
                TypeOption = new SelectList(typedata, "Value", "Text"),
                BranchOption = new SelectList(branchlist,"Value","Text"),
                InvAdjustment = new InvAdjustment()
            };
            return View(invoiceVM);
        }
        public ActionResult GetInvAdjustment(int id)
        {
            var obj = MerchandisingApiWrapper.Get<InvAdjustment>(
                                  typeof(InvAdjustment).Name + $"/{id}");
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetList(GridParams g, string search = null)
        {
            var obj = MerchandisingApiWrapper.Get<List<InvAdjustmentListVM>>(
                typeof(InvAdjustment).Name + "/getinvadjustmentlist" + $"?search={search}");

            //return Json(new {data = obj}, JsonRequestBehavior.AllowGet);
            return Json(new GridModelBuilder<InvAdjustmentListVM>(obj.AsQueryable(), g)
            {
                KeyProp = o => o.DocEntry,// needed for Entity Framework | nesting | tree | api
                Map = o => new
                {
                    o.DocEntry,
                    o.DocNum,
                    o.InvAdjustmentNo,
                    o.Type,
                    o.Reason,
                    DocTotal = o.DocTotal > 0 ? o.DocTotal.ToString("#,###.00") : "0.00",
                    Date = o.Date.ToString("MM-dd-yyyy"),
                    o.Reference,
                    o.InvoiceStatus,
                    o.Status
                }
            }.Build());
        }
        #endregion

        #region " Posting "
        [HttpPost]
        public ActionResult Save(InvAdjustment entity)
        {
            Api.InvAdjustmentController invoice = new Api.InvAdjustmentController();
            var obj = invoice.AddInvAdjustment(entity);
            return new JsonResult { Data = obj };
        }
        [HttpPut]
        public ActionResult Update(InvAdjustment entity)
        {
            Api.InvAdjustmentController invoice = new Api.InvAdjustmentController();
            var obj = invoice.EditInvAdjustment(entity.DocEntry, entity);
            return new JsonResult { Data = obj };
        }
        [HttpDelete]
        public ActionResult Cancelled(int id)
        {
            Api.InvAdjustmentController invoice = new Api.InvAdjustmentController();
            var obj = invoice.CancelledInvAdjustment(id);
            return new JsonResult { Data = obj };
        }
        #endregion
    }
}