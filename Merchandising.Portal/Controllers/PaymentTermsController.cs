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
    public class PaymentTermsController : Controller
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

            PaymentTermsVM termsVM = new PaymentTermsVM()
            {
                StatusOption = new SelectList(statusData, "Value", "Text", "1"),
                Terms = new PaymentTerms()
            };
            return View(termsVM);
        }
        public ActionResult GetPaymentTerms(string id)
        {
            var obj = MerchandisingApiWrapper.Get<PaymentTerms>(
                                  typeof(PaymentTerms).Name + $"/{id}");
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetList(GridParams g, string search = null)
        {
            var obj = MerchandisingApiWrapper.Get<List<PaymentTermsListVM>>(
                typeof(PaymentTerms).Name + "/getpaymenttermslist" + $"?search={search}");

            //return Json(new {data = obj}, JsonRequestBehavior.AllowGet);
            return Json(new GridModelBuilder<PaymentTermsListVM>(obj.AsQueryable(), g)
            {
                KeyProp = o => o.TermId,// needed for Entity Framework | nesting | tree | api
                Map = o => new
                {
                    o.TermId,
                    o.Name,
                    o.NoOfDays,
                    o.Status
                }
            }.Build());
        }
        #endregion

        #region " Posting "
        [HttpPost]
        public ActionResult Save(PaymentTerms entity)
        {
            Api.PaymentTermsController terms = new Api.PaymentTermsController();
            var obj = terms.AddPaymentTerms(entity);
            return new JsonResult { Data = obj };
        }
        [HttpPut]
        public ActionResult Update(PaymentTerms entity)
        {
            Api.PaymentTermsController terms = new Api.PaymentTermsController();
            var obj = terms.EditPaymentTerms(entity.TermId, entity);
            return new JsonResult { Data = obj };
        }
        [HttpDelete]
        public ActionResult Delete(string id)
        {
            Api.PaymentTermsController terms = new Api.PaymentTermsController();
            var obj = terms.DeletePaymentTerms(id);
            return new JsonResult { Data = obj };
        }
        #endregion
    }
}