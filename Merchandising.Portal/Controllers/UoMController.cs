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
    public class UoMController : Controller
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
            ViewBag.Filter = new SelectList(statusData.ToList(), "Value", "Text");

            UoMVM uomVM = new UoMVM()
            {
                StatusOption = new SelectList(statusData, "Value", "Text", "1"),
                UoM = new UoM()
            };
            return View(uomVM);
        }
        public ActionResult GetUoM(string id)
        {
            var obj = MerchandisingApiWrapper.Get<UoM>(
                                  typeof(UoM).Name + $"/{id}");
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetUoMInfo()
        {
            var obj = MerchandisingApiWrapper.Get<List<UoM>>(
                               typeof(UoM).Name + "/getuominfo");
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetList(GridParams g, string search = null)
        {
            var obj = MerchandisingApiWrapper.Get<List<UoMListVM>>(
                typeof(UoM).Name + "/getuomlist" + $"?search={search}");

            //return Json(new {data = obj}, JsonRequestBehavior.AllowGet);
            return Json(new GridModelBuilder<UoMListVM>(obj.AsQueryable(), g)
            {
                KeyProp = o => o.Code,// needed for Entity Framework | nesting | tree | api
                Map = o => new
                {
                    o.Code,
                    o.Name,
                    o.Status
                }
            }.Build());
        }
        #endregion

        #region " Posting "
        [HttpPost]
        public ActionResult Save(UoM entity)
        {
            Api.UoMController uom = new Api.UoMController();
            var obj = uom.AddUoM(entity);
            return new JsonResult { Data = obj };
        }
        [HttpPut]
        public ActionResult Update(UoM entity)
        {
            Api.UoMController uom = new Api.UoMController();
            var obj = uom.EditUoM(entity.Code, entity);
            return new JsonResult { Data = obj };
        }
        [HttpDelete]
        public ActionResult Delete(string id)
        {
            Api.UoMController uom = new Api.UoMController();
            var obj = uom.DeleteUoM(id);
            return new JsonResult { Data = obj };
        }
        #endregion
    }
}