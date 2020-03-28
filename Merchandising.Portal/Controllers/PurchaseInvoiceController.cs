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
    public class PurchaseInvoiceController : Controller
    {
        #region " View Method"
        public ActionResult Index()
        {
            //Status
            var statusData = from InvoiceType e in Enum.GetValues(typeof(InvoiceType))
                             select new SelectListItem
                             {
                                 Value = Convert.ToString((int)e),
                                 Text = e.ToString()
                             };
            //businesspartner
            var objbp = MerchandisingApiWrapper.Get<List<BusinessPartner>>(
                       typeof(BusinessPartner).Name + "/getbusinesspartnerinfo");

            var bp = objbp.Where(x => x.BpType == "S").Select(y => new { y.CardCode, y.CardName }).Distinct().ToList();
            IEnumerable<SelectListItem> bplist =
                from s in bp
                select new SelectListItem
                {
                    Text = s.CardName,
                    Value = s.CardCode
                };
            //items
            var objitems = MerchandisingApiWrapper.Get<List<Items>>(
                       typeof(Items).Name + "/getitemsinfo");

            var item = objitems.Where(x=> x.isPurchaseItem == true).Select(y => new { y.ItemCode, y.ItemName }).Distinct().ToList();
            IEnumerable<SelectListItem> itemlist =
                from s in item
                select new SelectListItem
                {
                    Text = s.ItemName,
                    Value = s.ItemCode
                };
            //uomgroup
            var objgroup = MerchandisingApiWrapper.Get<List<UoM>>(
                       typeof(UoM).Name + "/getuominfo");

            var uom = objgroup.Select(y => new { y.Code, y.Name }).Distinct().ToList();
            IEnumerable<SelectListItem> uomlist =
                from s in uom
                select new SelectListItem
                {
                    Text = s.Name,
                    Value = s.Code
                };

            //vat
            var objvat = MerchandisingApiWrapper.Get<List<Vat>>(
                       typeof(Vat).Name + "/getvatinfo");

            var vat = objvat.Where(x => x.Type == 1).Select(y => new { y.Code, y.Percentage, y.Name }).Distinct().ToList();
            IEnumerable<SelectListItem> vatlist =
                from s in vat
                select new SelectListItem
                {
                    Text = s.Code + " - " + (s.Percentage / 100) + " %",
                    Value = (s.Percentage / 100).ToString()
                };

            //whse
            var objwhse = MerchandisingApiWrapper.Get<List<Warehouse>>(
                       typeof(Warehouse).Name + "/getwarehouseinfo");

            var whse = objwhse.Select(y => new { y.Code, y.Name }).Distinct().ToList();
            IEnumerable<SelectListItem> whselist =
                from s in whse
                select new SelectListItem
                {
                    Text = s.Name,
                    Value = s.Code
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
            //payment terms
            var objpaymentterms = MerchandisingApiWrapper.Get<List<PaymentTerms>>(
                       typeof(PaymentTerms).Name + "/getpaymenttermsinfo");

            var paymenterms = objpaymentterms.Select(y => new { y.TermId, y.Name }).Distinct().ToList();
            IEnumerable<SelectListItem> paymenttermslist =
                from s in paymenterms
                select new SelectListItem
                {
                    Text = s.Name,
                    Value = s.TermId
                };
            //getsequence
            //var objsequence = MerchandisingApiWrapper.Get<List<SequenceTable>>(
            //           typeof(SequenceTable).Name + "/getsequenceinfo");

            //var series = objsequence.Where(x => x.Document == "OPCH").Select(y => new { y.SeriesName, y.Prefix, y.NextNumber, y.Suffix }).Distinct().ToList();
            //IEnumerable<SelectListItem> serieslist =
            //    from s in series
            //    select new SelectListItem
            //    {
            //        Text = s.SeriesName,
            //        Value = s.SeriesName
            //    };
            SequenceTableController seq = new SequenceTableController();
            var sequence = seq.GetSequenceSeries(6);
            IEnumerable<SelectListItem> serieslist =
            from s in sequence
            select new SelectListItem
            {
                Text = s.SeriesName,
                Value = s.Series.ToString()
            };
            //modeofpayment
            var objmodeofpayment = MerchandisingApiWrapper.Get<List<ModeOfPayment>>(
                       typeof(ModeOfPayment).Name + "/getmodeofpaymentinfo");

            var modeofpayment = objmodeofpayment.Select(y => new { y.Code, y.Name }).Distinct().ToList();
            IEnumerable<SelectListItem> moplist =
                from s in modeofpayment
                select new SelectListItem
                {
                    Text = s.Code,
                    Value = s.Name
                };
            PurchaseInvoiceVM invoiceVM = new PurchaseInvoiceVM()
            {
                CustomerOption = new SelectList(bplist, "Value", "Text"),
                ItemOption = new SelectList(itemlist, "Value", "Text"),
                StatusOption = new SelectList(statusData, "Value", "Text"),
                SeriesOption = new SelectList(serieslist, "Value", "Text"),
                UoMOption = new SelectList(uomlist, "Value", "Text"),
                VatOption = new SelectList(vatlist, "Value", "Text"),
                ModeOfPaymentOption = new SelectList(moplist, "Value", "Text"),
                PaymentTermsOption = new SelectList(paymenttermslist, "Value", "Text"),
                WhseOption = new SelectList(whselist, "Value", "Text"),
                BranchOption = new SelectList(branchlist, "Value", "Text"),
                PurchaseInvoice = new PurchaseInvoice()
            };
            return View(invoiceVM);
        }
        public ActionResult GetPurchaseInvoice(string id)
        {
            var obj = MerchandisingApiWrapper.Get<PurchaseInvoice>(
                                  typeof(PurchaseInvoice).Name + $"/{id}");
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetList(GridParams g, string search = null)
        {
            var obj = MerchandisingApiWrapper.Get<List<PurchaseInvoiceListVM>>(
                typeof(PurchaseInvoice).Name + "/getpurchaseinvoicelist" + $"?search={search}");

            //return Json(new {data = obj}, JsonRequestBehavior.AllowGet);
            return Json(new GridModelBuilder<PurchaseInvoiceListVM>(obj.AsQueryable(), g)
            {
                KeyProp = o => o.DocEntry,// needed for Entity Framework | nesting | tree | api
                Map = o => new
                {
                    o.DocEntry,
                    o.DocNum,
                    o.PInvoice,
                    o.CardCode,
                    o.CardName,
                    DocTotal = o.DocTotal.ToString("#,###.00"),
                    GrossTotal = o.GrossTotal.ToString("#,###.00"),
                    DueDate = o.DueDate.ToString("MM-dd-yyyy"),
                    Date = o.Date.ToString("MM-dd-yyyy"),
                    o.Reference,
                    o.Status
                }
            }.Build());
        }
        #endregion

        #region " Posting "
        [HttpPost]
        public ActionResult Save(PurchaseInvoice entity)
        {
            Api.PurchaseInvoiceController invoice = new Api.PurchaseInvoiceController();
            var obj = invoice.AddPurchaseInvoice(entity);
            return new JsonResult { Data = obj };
        }
        [HttpPut]
        public ActionResult Update(PurchaseInvoice entity)
        {
            Api.PurchaseInvoiceController invoice = new Api.PurchaseInvoiceController();
            var obj = invoice.EditPurchaseInvoice(entity.DocEntry, entity);
            return new JsonResult { Data = obj };
        }
        [HttpDelete]
        public ActionResult Cancelled(int id)
        {
            Api.PurchaseInvoiceController invoice = new Api.PurchaseInvoiceController();
            var obj = invoice.CancelledPurchaseInvoice(id);
            return new JsonResult { Data = obj };
        }
        #endregion
    }
}