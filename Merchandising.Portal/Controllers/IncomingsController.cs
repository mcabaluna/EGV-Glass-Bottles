using Merchandising.DTO.Models;
using Merchandising.Portal.Models;
using Merchandising.VM.Portal;
using Merchandising.VM.Results;
using Omu.AwesomeMvc;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Merchandising.Portal.Controllers
{
    public class IncomingsController : Controller
    {
        #region " Collection View Method"
        public ActionResult Collection()
        {
            //businesspartner
            var objbp = MerchandisingApiWrapper.Get<List<BusinessPartner>>(
                       typeof(BusinessPartner).Name + "/getbusinesspartnerinfo");

            var bp = objbp.Where(x => x.BpType == "C").Select(y => new { y.CardCode, y.CardName }).Distinct().ToList();
            IEnumerable<SelectListItem> bplist =
                from s in bp
                select new SelectListItem
                {
                    Text = s.CardName,
                    Value = s.CardCode
                };

            //getsequence
            var objsequence = MerchandisingApiWrapper.Get<List<SequenceTable>>(
                       typeof(SequenceTable).Name + "/getsequenceinfo");

            //var series = objsequence.Where(x => x.Document == "ORCT").Select(y => new { y.SeriesName, y.Prefix, y.NextNumber, y.Suffix }).Distinct().ToList();
            //IEnumerable<SelectListItem> serieslist =
            //    from s in series
            //    select new SelectListItem
            //    {
            //        Text = s.SeriesName,
            //        Value = s.SeriesName
            //    };
            //modeofpayment
            var objmodeofpayment = MerchandisingApiWrapper.Get<List<ModeOfPayment>>(
                       typeof(ModeOfPayment).Name + "/getmodeofpaymentinfo");

            var modeofpayment = objmodeofpayment.Select(y => new { y.Code, y.Name }).Distinct().ToList();
            IEnumerable<SelectListItem> moplist =
                from s in modeofpayment
                select new SelectListItem
                {
                    Text = s.Name,
                    Value = s.Code
                };
            IncomingsVM invoiceVM = new IncomingsVM()
            {
                CustomerOption = new SelectList(bplist, "Value", "Text"),
                //SeriesOption = new SelectList(serieslist, "Value", "Text"),
                ModeOfPaymentOption = new SelectList(moplist, "Value", "Text"),
                Incomings = new Incomings()
            };
            return View(invoiceVM);
        }
        public ActionResult GetIncomings(int id)
        {
            var obj = MerchandisingApiWrapper.Get<Incomings>(
                                  typeof(Incomings).Name + $"/{id}");
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetList(GridParams g, string invoicetype, string search = null)
        {
            var obj = MerchandisingApiWrapper.Get<List<IncomingsListVM>>(
                typeof(Incomings).Name + "/getincomingslist" + $"?search={search}");

            //return Json(new {data = obj}, JsonRequestBehavior.AllowGet);
            return Json(new GridModelBuilder<IncomingsListVM>(obj.Where(x => x.InvoiceType == invoicetype).AsQueryable(), g)
            {
                KeyProp = o => o.DocEntry,// needed for Entity Framework | nesting | tree | api
                Map = o => new
                {
                    o.DocEntry,
                    o.DocNum,
                    o.CardCode,
                    o.CardName,
                    o.PaymentNo,
                    o.BranchCode,
                    o.InvoiceNo,
                    Collections = o.Collections == 0 ? "0.00" : o.Collections.ToString("#,###.00"),
                    Balance = o.Balance == 0 ? "0.00" : o.Balance.ToString("#,###.00"),
                    AmountPaid = o.AmountPaid == 0 ? "0.00" : o.AmountPaid.ToString("#,###.00"),
                    DueDate = o.DueDate.ToString("MM-dd-yyyy"),
                    DatePaid = o.DatePaid.ToString("MM-dd-yyyy"),
                    o.Remarks,
                    o.Status,
                    IntStatus = o.Status == true ? 1 : 0
                }
            }.Build());
        }
        public JsonResult GetInvoice(string cardcode, string branch)
        {
            var obj = MerchandisingApiWrapper.Get<List<Incoming_Results>>(
              typeof(Incomings).Name + "/getinvoices" + $"?cardcode={cardcode}&branch={branch}");
            return Json(obj.ToList(), JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region " Payment View Method"
        public ActionResult Payment()
        {
            //businesspartner
            var objbp = MerchandisingApiWrapper.Get<List<BusinessPartner>>(
                       typeof(BusinessPartner).Name + "/getbusinesspartnerinfo");

            var bp = objbp.Where(x => x.BpType == "SUPPLIER").Select(y => new { y.CardCode, y.CardName }).Distinct().ToList();
            IEnumerable<SelectListItem> bplist =
                from s in bp
                select new SelectListItem
                {
                    Text = s.CardName,
                    Value = s.CardCode
                };

            //getsequence
            var objsequence = MerchandisingApiWrapper.Get<List<SequenceTable>>(
                       typeof(SequenceTable).Name + "/getsequenceinfo");

            //var series = objsequence.Where(x => x.Document == "OVPM").Select(y => new { y.SeriesName, y.Prefix, y.NextNumber, y.Suffix }).Distinct().ToList();
            //IEnumerable<SelectListItem> serieslist =
            //    from s in series
            //    select new SelectListItem
            //    {
            //        Text = s.SeriesName,
            //        Value = s.SeriesName
            //    };
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
            IncomingsVM invoiceVM = new IncomingsVM()
            {
                CustomerOption = new SelectList(bplist, "Value", "Text"),
                //SeriesOption = new SelectList(serieslist, "Value", "Text"),
                ModeOfPaymentOption = new SelectList(moplist, "Value", "Text"),
                Incomings = new Incomings()
            };
            return View(invoiceVM);
        }
        #endregion

        #region " Posting "
        [HttpPost]
        public ActionResult Save(Incomings entity)
        {
            Api.IncomingsController incoming = new Api.IncomingsController();
            var obj = incoming.AddIncomings(entity);
            return new JsonResult { Data = obj };
        }
        [HttpPut]
        public ActionResult Update(Incomings entity)
        {
            Api.IncomingsController incoming = new Api.IncomingsController();
            var obj = incoming.EditIncomings(entity.DocEntry, entity);
            return new JsonResult { Data = obj };
        }
        [HttpDelete]
        public ActionResult Cancelled(int id)
        {
            Api.IncomingsController incoming = new Api.IncomingsController();
            var obj = incoming.CancelledIncomings(id);
            return new JsonResult { Data = obj };
        }
        #endregion
    }
}