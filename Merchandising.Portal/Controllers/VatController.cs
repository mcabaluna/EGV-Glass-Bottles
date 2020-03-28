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
    public class VatController : Controller
    {
        #region " View Method"
        // GET: Branch
        public ActionResult Index()
        {
            var statusData = from StatusType e in Enum.GetValues(typeof(StatusType))
                             select new SelectListItem
                             {
                                 Value = Convert.ToString((int)e),
                                 Text = e.ToString()
                             };
            var typeData = from VatType e in Enum.GetValues(typeof(VatType))
                           select new SelectListItem
                           {
                               Value = Convert.ToString((int)e),
                               Text = e.ToString()
                           };
            ViewBag.Filter = new SelectList(statusData.ToList(), "Value", "Text");

            VatVM vatVM = new VatVM()
            {
                StatusOption = new SelectList(statusData, "Value", "Text", "1"),
                VatOption = new SelectList(typeData, "Value", "Text"),
                Vat = new Vat()
            };
            return View(vatVM);
        }
        public ActionResult GetVat(string id)
        {
            var obj = MerchandisingApiWrapper.Get<Vat>(
                                  typeof(Vat).Name + $"/{id}");
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetVatInfo()
        {
            var obj = MerchandisingApiWrapper.Get<List<Vat>>(
                               typeof(Vat).Name + "/getvatinfo");
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetList(GridParams g, string search = null)
        {
            var obj = MerchandisingApiWrapper.Get<List<VatListVM>>(
                typeof(Vat).Name + "/getvatlist" + $"?search={search}");

            //return Json(new {data = obj}, JsonRequestBehavior.AllowGet);
            return Json(new GridModelBuilder<VatListVM>(obj.AsQueryable(), g)
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
        public ActionResult Save(Vat entity)
        {
            Api.VatController vat = new Api.VatController();
            var obj = vat.AddVat(entity);
            return new JsonResult { Data = obj };
        }
        [HttpPut]
        public ActionResult Update(Vat entity)
        {
            Api.VatController vat = new Api.VatController();
            var obj = vat.EditVat(entity.Code, entity);
            return new JsonResult { Data = obj };
        }
        [HttpDelete]
        public ActionResult Delete(string id)
        {
            Api.VatController vat = new Api.VatController();
            var obj = vat.DeleteVat(id);
            return new JsonResult { Data = obj };
        }
        #endregion
    }
}