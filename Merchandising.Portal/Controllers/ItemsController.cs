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
    public class ItemsController : Controller
    {
        #region " View Method"
        public ActionResult Index()
        {
            //itemgroup
            var objitemgroup = MerchandisingApiWrapper.Get<List<ItemGroup>>(
                       typeof(ItemGroup).Name + "/getitemgroupinfo");

            var itemgroup = objitemgroup.Select(y => new { y.Code, y.Name }).Distinct().ToList();
            IEnumerable<SelectListItem> itemgrouplist =
                from s in itemgroup
                select new SelectListItem
                {
                    Text = s.Name,
                    Value = s.Code
                };
            //uomgroup
            var objuomgroup = MerchandisingApiWrapper.Get<List<UoM>>(
                       typeof(UoM).Name + "/getuominfo");

            var uom = objuomgroup.Select(y => new { y.Code, y.Name }).Distinct().ToList();
            IEnumerable<SelectListItem> uomlist =
                from s in uom
                select new SelectListItem
                {
                    Text = s.Name,
                    Value = s.Code
                };

            //wtax
            var objwtax = MerchandisingApiWrapper.Get<List<WTax>>(
                       typeof(WTax).Name + "/getwtaxinfo");

            var wtax = objwtax.Select(y => new { y.Code, y.Name }).Distinct().ToList();
            IEnumerable<SelectListItem> wtaxlist =
                from s in wtax
                select new SelectListItem
                {
                    Text = s.Name,
                    Value = s.Code
                };

            //getsequence
            //var objsequence = MerchandisingApiWrapper.Get<List<SequenceTable>>(
            //           typeof(SequenceTable).Name + "/getsequenceinfo");

            //var series = objsequence.Where(x => x.ObjectCode == "OITM").Select(y => new { y.Prefix, y.SeriesName, y.NextNumber, y.Suffix }).Distinct().ToList();
            //IEnumerable<SelectListItem> serieslist =
            //    from s in series
            //    select new SelectListItem
            //    {
            //        Text = s.SeriesName,
            //        Value = s.SeriesName
            //    };
            SequenceTableController seq = new SequenceTableController();
            var sequence = seq.GetSequenceSeries(3);
            IEnumerable<SelectListItem> serieslist =
            from s in sequence
            select new SelectListItem
            {
                Text = s.SeriesName,
                Value = s.Series.ToString()
            };


            ItemsVM itemsVM = new ItemsVM()
            {
                ItemGroupOption = new SelectList(itemgrouplist, "Value", "Text"),
                UomGroupOption = new SelectList(uomlist, "Value", "Text"),
                WTaxOption = new SelectList(wtaxlist, "Value", "Text"),
                SeriesOption = new SelectList(serieslist, "Value", "Text"),
                Items = new Items()
            };
            return View(itemsVM);
        }
        public ActionResult GetItems(string id)
        {
            var obj = MerchandisingApiWrapper.Get<Items>(
                                  typeof(Items).Name + $"/{id}");
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetItemsInfo()
        {
            var objitem = MerchandisingApiWrapper.Get<List<Items>>(
                         typeof(Items).Name + "/getitemsinfo");
            return Json(objitem, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CheckItemPrice(string pricelistid, string itemcode)
        {
            var objitem = MerchandisingApiWrapper.Get<PricelistItem_Results>(
                         typeof(Items).Name + "/checkitemprice" + $"?pricelistid={pricelistid}&itemcode={itemcode}");
            return Json(objitem, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAvailStock(string itemcode, string warehouse)
        {
            var obj = MerchandisingApiWrapper.Get<Items>(
                                         typeof(Items).Name + $"/{itemcode}");
            decimal availstock = 0m;
            if (warehouse != null && warehouse != "")
            {
                var objperwhse = obj.ItemOnHandPerWhse.SingleOrDefault(x => x.WhseId == warehouse);
                if (objperwhse != null)
                    availstock = objperwhse.OnHand - objperwhse.Commited - objperwhse.Ordered;
            }
            return Json(availstock, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetList(GridParams g, string search = null)
        {
            //var obj = MerchandisingApiWrapper.Get<List<ItemsListVM>>(
            //    typeof(Items).Name + "/getitemlist" + $"?search={search}");

            var objitem = MerchandisingApiWrapper.Get<List<Items>>(
                       typeof(Items).Name + "/getitemsinfo");

            var objitemgroup = MerchandisingApiWrapper.Get<List<ItemGroup>>(
                       typeof(ItemGroup).Name + "/getitemgroupinfo");

            var objwtax = MerchandisingApiWrapper.Get<List<WTax>>(
                       typeof(WTax).Name + "/getwtaxinfo");

            var items = new List<Items>();
            List<ItemsListVM> list = new List<ItemsListVM>();
            //get all roles with filter 
            items = objitem
                         .OrderByDescending(x => x.CreatedOn)
                         .ToList();
            //
            if (items.Count > 0)
            {
                list = items.Select(x => new ItemsListVM()
                {
                    ItemCode = x.ItemCode,
                    ItemName = x.ItemName,
                    GroupName = objitemgroup.FirstOrDefault(b => b.Code == x.GroupCode)?.Name,
                    WTaxName = objwtax.FirstOrDefault(b => b.Code == x.WtaxId)?.Name,
                    Status = x.Status
                }).ToList();

                //Search fields
                if (!string.IsNullOrEmpty(search))
                {
                    list = list.Where(x =>
                            x.ItemCode.ToLower().Contains(search.ToLower()) ||
                            x.ItemName.ToString().ToLower().Contains(search.ToLower()) ||
                            x.GroupName.ToString().ToLower().Contains(search.ToLower()) ||
                            x.WTaxName.ToString().ToLower().Contains(search.ToLower()))
                        .OrderByDescending(x => x.ItemCode)
                        .ToList();
                }
            }
            //return Json(new {data = obj}, JsonRequestBehavior.AllowGet);
            return Json(new GridModelBuilder<ItemsListVM>(list.AsQueryable(), g)
            {
                KeyProp = o => o.ItemCode,// needed for Entity Framework | nesting | tree | api
                Map = o => new
                {
                    o.ItemCode,
                    o.ItemName,
                    o.GroupName,
                    o.WTaxName,
                    o.Status
                }
            }.Build());
        }
        #endregion

        #region " Posting "
        [HttpPost]
        public ActionResult Save(Items entity)
        {
            Api.ItemsController items = new Api.ItemsController();
            var obj = items.AddItems(entity);
            return new JsonResult { Data = obj };
        }
        [HttpPut]
        public ActionResult Update(Items entity)
        {
            Api.ItemsController items = new Api.ItemsController();
            var obj = items.EditItems(entity.ItemCode, entity);
            return new JsonResult { Data = obj };
        }
        [HttpDelete]
        public ActionResult Delete(string id)
        {
            Api.ItemsController items = new Api.ItemsController();
            var obj = items.DeleteItems(id);
            return new JsonResult { Data = obj };
        }
        #endregion
    }
}