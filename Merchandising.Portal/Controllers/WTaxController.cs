using Merchandising.DTO.Models;
using Merchandising.Enums;
using Merchandising.Portal.Models;
using Merchandising.VM.Portal;
using Omu.AwesomeMvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Merchandising.Portal.Controllers
{
    public class WTaxController : Controller
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
            var typeData = from WTaxType e in Enum.GetValues(typeof(WTaxType))
                           select new SelectListItem
                           {
                               Value = Convert.ToString((int)e),
                               Text = e.ToString()
                           };
            ViewBag.Filter = new SelectList(statusData.ToList(), "Value", "Text");

            WTaxVM wtaxVM = new WTaxVM()
            {
                StatusOption = new SelectList(statusData, "Value", "Text", "1"),
                WTaxOption = new SelectList(typeData, "Value", "Text"),
                WTax = new WTax()
            };
            return View(wtaxVM);
        }
        public ActionResult GetWTax(string id)
        {
            var obj = MerchandisingApiWrapper.Get<WTax>(
                                  typeof(WTax).Name + $"/{id}");
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetWTaxInfo()
        {
            var obj = MerchandisingApiWrapper.Get<List<WTax>>(
                               typeof(WTax).Name + "/getwtaxinfo");
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetList(GridParams g, string search = null)
        {
            var obj = MerchandisingApiWrapper.Get<List<WTaxListVM>>(
                typeof(WTax).Name + "/getwtaxlist" + $"?search={search}");

            //return Json(new {data = obj}, JsonRequestBehavior.AllowGet);
            return Json(new GridModelBuilder<WTaxListVM>(obj.AsQueryable(), g)
            {
                KeyProp = o => o.Code,// needed for Entity Framework | nesting | tree | api
                Map = o => new
                {
                    o.Code,
                    o.Name,
                    o.Type,
                    Percentage = o.Percentage > 0 ? o.Percentage.ToString("#,###.00") : "0.00",
                    o.Status,
                    EffectiveFrom = o.EffectiveFrom.ToShortDateString(),
                    EffectiveTo = o.EffectiveTo.ToShortDateString()
                }
            }.Build());
        }
        #endregion

        #region " Posting "
        [HttpPost]
        public ActionResult Save(WTax entity)
        {
            Api.WTaxController wtax = new Api.WTaxController();
            var obj = wtax.AddWTax(entity);
            return new JsonResult { Data = obj };
        }
        [HttpPut]
        public ActionResult Update(WTax entity)
        {
            Api.WTaxController wtax = new Api.WTaxController();
            var obj = wtax.EditWTax(entity.Code, entity);
            return new JsonResult { Data = obj };
        }
        [HttpDelete]
        public ActionResult Delete(string id)
        {
            Api.WTaxController wtax = new Api.WTaxController();
            var obj = wtax.DeleteWTax(id);
            return new JsonResult { Data = obj };
        }
        #endregion
    }
}