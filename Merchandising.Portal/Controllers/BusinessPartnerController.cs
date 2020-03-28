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
    public class BusinessPartnerController : Controller
    {
        #region " View Method"
        // GET: BusinessPartner
        public ActionResult Index()
        {
            //bptype
            List<SelectListItem> bptypelist = new List<SelectListItem>();
            bptypelist.Add(new SelectListItem() { Text = "CUSTOMER", Value = "C_1" });
            bptypelist.Add(new SelectListItem() { Text = "VENDOR", Value = "S_2" });

            //bpgroup
            var objbpgroup = MerchandisingApiWrapper.Get<List<BpGroup>>(
                       typeof(BpGroup).Name + "/getbpgroupinfo");

            var bpgroup = objbpgroup.Select(y => new { y.Code, y.Name }).Distinct().ToList();
            IEnumerable<SelectListItem> bpgrouplist =
                from s in bpgroup
                select new SelectListItem
                {
                    Text = s.Code + " - " + s.Name,
                    Value = s.Code
                };

            //vat
            var objvat = MerchandisingApiWrapper.Get<List<Vat>>(
                       typeof(Vat).Name + "/getvatinfo");

            var vat = objvat.Select(y => new { y.Code, y.Name }).Distinct().ToList();
            IEnumerable<SelectListItem> vatlist =
                from s in vat
                select new SelectListItem
                {
                    Text = s.Name,
                    Value = s.Code
                };

            //wtax
            var objwtax = MerchandisingApiWrapper.Get<List<WTax>>(
                       typeof(WTax).Name + "/getwtaxinfo");

            var wtax = objwtax.Distinct().ToList();
            IEnumerable<SelectListItem> wtaxlist =
                from s in wtax
                select new SelectListItem
                {
                    Text = s.Code + " - " + s.Name,
                    Value = s.Code
                };

            //pricelist
            var objpricelist = MerchandisingApiWrapper.Get<List<Pricelist>>(
                       typeof(Pricelist).Name + "/getpricelistinfo");

            var pricelist = objpricelist.Select(y => new { y.PricelistId, y.Name }).Distinct().ToList();
            IEnumerable<SelectListItem> priceList =
                from s in pricelist
                select new SelectListItem
                {
                    Text = s.Name,
                    Value = s.PricelistId
                };
            //paymentterms
            var objpaymentterms = MerchandisingApiWrapper.Get<List<PaymentTerms>>(
                       typeof(PaymentTerms).Name + "/getpaymenttermsinfo");

            var paymentterms = objpaymentterms.Select(y => new { y.TermId, y.Name }).Distinct().ToList();
            IEnumerable<SelectListItem> paymenttermslist =
                from s in paymentterms
                select new SelectListItem
                {
                    Text = s.TermId + " - " + s.Name,
                    Value = s.TermId
                };
            //getsequence
            //var objsequence = MerchandisingApiWrapper.Get<List<SequenceTable>>(
            //           typeof(SequenceTable).Name + "/getsequenceinfo");

            //var series = objsequence.Where(x => x.Document == "BPMD").Select(y => new { y.SeriesName, y.Prefix, y.NextNumber, y.Suffix }).Distinct().ToList();
            //IEnumerable<SelectListItem> serieslist =
            //    from s in series
            //    select new SelectListItem
            //    {
            //        Text = s.SeriesName,
            //        Value = s.SeriesName
            //    };
            //SequenceTableController seq = new SequenceTableController();
            //var sequence = seq.GetSequenceSeries(1);

            //provinces
            var objprovinces = MerchandisingApiWrapper.Get<List<Provinces>>(
                   typeof(Provinces).Name + "/getprovincesinfo");

            var provinces = objprovinces.Select(y => new { y.ProvCode, y.ProvName }).Distinct().ToList();
            IEnumerable<SelectListItem> provinceslist =
                from s in objprovinces
                select new SelectListItem
                {
                    Text = s.ProvCode + " - " + s.ProvName,
                    Value = s.ProvCode
                };
            BusinessPartnerVM bpVM = new BusinessPartnerVM()
            {
                BPTypeOption = new SelectList(bptypelist, "Value", "Text", "C"),
                BPGroupOption = new SelectList(bpgrouplist, "Value", "Text"),
                PaymentTermsOption = new SelectList(paymenttermslist, "Value", "Text"),
                WTaxOption = new SelectList(wtaxlist, "Value", "Text"),
                //SeriesOption = new SelectList(sequence, "Value", "Text"),
                VATOption = new SelectList(vatlist, "Value", "Text"),
                ProvincesOption = new SelectList(provinceslist, "Value", "Text"),
                PriceListOption = new SelectList(priceList, "Value", "Text"),
                BusinessPartner = new BusinessPartner(),
                BpAddress = new BpAddress(),
                WTax = wtax,
                BpWTax = new BpWTax()
            };
            return View(bpVM);
        }
        public ActionResult GetCities(string provcode)
        {
            //cities
            var objcities = MerchandisingApiWrapper.Get<List<Cities>>(
                     typeof(Cities).Name + "/getcitiesinfo");

            var cities = objcities.Where(x => x.ProvCode == provcode).ToList();
            return Json(cities, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetBusinessPartner(string id)
        {
            var obj = MerchandisingApiWrapper.Get<BusinessPartner>(
                                  typeof(BusinessPartner).Name + $"/{id}");
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetBPBalanceDetails(string id)
        {
            var obj = MerchandisingApiWrapper.Get<List<BPBalance_Results>>(
                                typeof(BusinessPartner).Name + "/getbpbalancedetails" + $"/{id}");
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetList(GridParams g, string search = null)
        {
            var obj = MerchandisingApiWrapper.Get<List<BusinessPartnerListVM>>(
                typeof(BusinessPartner).Name + "/getbusinesspartnerlist" + $"?search={search}");


            //return Json(new {data = obj}, JsonRequestBehavior.AllowGet);
            return Json(new GridModelBuilder<BusinessPartnerListVM>(obj.AsQueryable(), g)
            {
                KeyProp = o => o.CardCode,// needed for Entity Framework | nesting | tree | api
                Map = o => new
                {
                    o.CardCode,
                    o.CardName,
                    o.BpType,
                    o.BpGroupCode,
                    o.Address,
                    o.ContactNumber,
                    o.Email,
                    Balance = o.Balance.Equals(0) ? "0.00" : o.Balance.ToString("#,###.00"),
                    o.Status
                }
            }.Build());
        }
        #endregion

        #region " Posting "
        [HttpPost]
        public ActionResult Save(BusinessPartner entity)
        {
            Api.BusinessPartnerController bp = new Api.BusinessPartnerController();
            var obj = bp.AddBusinessPartner(entity);
            return new JsonResult { Data = obj };
        }
        [HttpPut]
        public ActionResult Update(BusinessPartner entity)
        {
            Api.BusinessPartnerController bp = new Api.BusinessPartnerController();
            var obj = bp.EditBusinessPartner(entity.CardCode, entity);
            return new JsonResult { Data = obj };
        }
        [HttpDelete]
        public ActionResult Delete(string id)
        {
            Api.BusinessPartnerController bp = new Api.BusinessPartnerController();
            var obj = bp.DeleteBusinessPartner(id);
            return new JsonResult { Data = obj };
        }
        #endregion
    }
}