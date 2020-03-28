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
    public class PricelistController : Controller
    {
        #region " View Method"
        public ActionResult Index()
        {
            var statusData = from StatusType e in Enum.GetValues(typeof(StatusType))
                             select new SelectListItem
                             {
                                 Value = Convert.ToString((int)e),
                                 Text = e.ToString()
                             };


            var obj = MerchandisingApiWrapper.Get<List<Pricelist>>(
                               typeof(Pricelist).Name + "/getpricelistinfo");

            var basepricelist = obj.Select(y => new { y.Name }).Distinct().ToList();
            IEnumerable<SelectListItem> basepricelistoption =
                from s in basepricelist
                select new SelectListItem
                {
                    Text = s.Name,
                    Value = s.Name
                };

            var objitem = MerchandisingApiWrapper.Get<List<Items>>(
                              typeof(Items).Name + "/getitemsinfo");

            var item = objitem.Select(y => new { y.ItemCode, y.ItemName }).Distinct().ToList();
            IEnumerable<SelectListItem> itemlist =
                from s in item
                select new SelectListItem
                {
                    Text = s.ItemName,
                    Value = s.ItemCode
                };
            var objuom = MerchandisingApiWrapper.Get<List<UoM>>(
                               typeof(UoM).Name + "/getuominfo");

            var uom = objuom.Select(y => new { y.Code, y.Name }).Distinct().ToList();
            IEnumerable<SelectListItem> uomlist =
                from s in uom
                select new SelectListItem
                {
                    Text = s.Name,
                    Value = s.Code
                };

            //getsequence
            //var objsequence = MerchandisingApiWrapper.Get<List<SequenceTable>>(
            //           typeof(SequenceTable).Name + "/getsequenceinfo");

            //var series = objsequence.Where(x => x.Document == "PRICELIST").Select(y => new { y.SeriesName, y.Prefix, y.NextNumber, y.Suffix }).Distinct().ToList();

            //var seriessequence = series.FirstOrDefault().Suffix != null ?
            //                    series.FirstOrDefault().Prefix + series.FirstOrDefault().NextNumber + "_" + series.FirstOrDefault().Suffix
            //                    : series.FirstOrDefault().Prefix + series.FirstOrDefault().NextNumber;
            //var seriesname = series.FirstOrDefault().SeriesName;

            SequenceTableController seq = new SequenceTableController();
            var sequence = seq.GetSequenceSeries(4);
            IEnumerable<SelectListItem> serieslist =
            from s in sequence
            select new SelectListItem
            {
                Text = s.SeriesName,
                Value = s.Series.ToString()
            };
            PricelistsVM pricelistVM = new PricelistsVM()
            {
                StatusOption = new SelectList(statusData, "Value", "Text", "1"),
                BasePricelistOption = new SelectList(basepricelistoption, "Value", "Text"),
                ItemOption = new SelectList(itemlist, "Value", "Text"),
                UoMOption = new SelectList(uomlist, "Value", "Text"),
                SeriesOption = new SelectList(serieslist, "Value", "Text"),
                //SequenceNumber = seriessequence,
                //Series = seriesname,
                Pricelist = new Pricelist()
            };
            return View(pricelistVM);
        }
        public ActionResult GetPricelist(string id)
        {
            var obj = MerchandisingApiWrapper.Get<Pricelist>(
                                  typeof(Pricelist).Name + $"/{id}");
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetList(GridParams g, string search = null)
        {
            var obj = MerchandisingApiWrapper.Get<List<PriceListVM>>(
                typeof(Pricelist).Name + "/getpricelists" + $"?search={search}");

            //return Json(new {data = obj}, JsonRequestBehavior.AllowGet);
            return Json(new GridModelBuilder<PriceListVM>(obj.AsQueryable(), g)
            {
                KeyProp = o => o.PricelistId,// needed for Entity Framework | nesting | tree | api
                Map = o => new
                {
                    o.PricelistId,
                    o.Name,
                    o.BasePricelist,
                    o.Factor,
                    o.Status
                }
            }.Build());
        }
        #endregion

        #region " Posting "
        [HttpPost]
        public ActionResult Save(Pricelist entity)
        {
            Api.PricelistController pricelist = new Api.PricelistController();
            var obj = pricelist.AddPricelist(entity);
            return new JsonResult { Data = obj };
        }
        [HttpPut]
        public ActionResult Update(Pricelist entity)
        {
            Api.PricelistController pricelist = new Api.PricelistController();
            var obj = pricelist.EditPricelist(entity.PricelistId, entity);
            return new JsonResult { Data = obj };
        }
        [HttpDelete]
        public ActionResult Delete(string id)
        {
            Api.PricelistController pricelist = new Api.PricelistController();
            var obj = pricelist.DeletePricelist(id);
            return new JsonResult { Data = obj };
        }
        #endregion
    }
}