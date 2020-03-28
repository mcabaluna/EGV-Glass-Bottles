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
    public class ModeOfPaymentController : Controller
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

            ModeOfPaymentVM modeOfPaymentVM = new ModeOfPaymentVM()
            {
                StatusOption = new SelectList(statusData, "Value", "Text", "1"),
                ModeOfPayment = new ModeOfPayment()
            };
            return View(modeOfPaymentVM);
        }
        public ActionResult GetModeOfPayment(string id)
        {
            var obj = MerchandisingApiWrapper.Get<ModeOfPayment>(
                                  typeof(ModeOfPayment).Name + $"/{id}");
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetList(GridParams g, string search = null)
        {
            var obj = MerchandisingApiWrapper.Get<List<ModeOfPaymentListVM>>(
                typeof(ModeOfPayment).Name + "/getmodeofpaymentlist" + $"?search={search}");

            //return Json(new {data = obj}, JsonRequestBehavior.AllowGet);
            return Json(new GridModelBuilder<ModeOfPaymentListVM>(obj.AsQueryable(), g)
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
        public ActionResult Save(ModeOfPayment entity)
        {
            Api.ModeOfPaymentController mop = new Api.ModeOfPaymentController();
            var obj = mop.AddModeOfPayment(entity);
            return new JsonResult { Data = obj };
        }
        [HttpPut]
        public ActionResult Update(ModeOfPayment entity)
        {
            Api.ModeOfPaymentController mop = new Api.ModeOfPaymentController();
            var obj = mop.EditModeOfPayment(entity.Code, entity);
            return new JsonResult { Data = obj };
        }
        [HttpDelete]
        public ActionResult Delete(string id)
        {
            Api.ModeOfPaymentController mop = new Api.ModeOfPaymentController();
            var obj = mop.DeleteModeOfPayment(id);
            return new JsonResult { Data = obj };
        }
        #endregion
    }
}