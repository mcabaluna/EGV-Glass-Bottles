using Merchandising.DTO.Models;
using Merchandising.VM.Portal;
using Omu.AwesomeMvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Merchandising.Portal.Controllers
{
    public class RoleMenusController : Controller
    {
        //#region " View Method"
        //// GET: Branch
        //public ActionResult Index()
        //{
        //    return View();
        //}
        //public ActionResult GetVat(string id)
        //{
        //    var obj = MerchandisingApiWrapper.Get<Vat>(
        //                          typeof(Vat).Name + $"/{id}");
        //    return Json(obj, JsonRequestBehavior.AllowGet);
        //}
        //public ActionResult GetList(GridParams g, string search = null)
        //{
        //    var obj = MerchandisingApiWrapper.Get<List<VatListVM>>(
        //        typeof(Vat).Name + "/getvatlist" + $"?search={search}");

        //    //return Json(new {data = obj}, JsonRequestBehavior.AllowGet);
        //    return Json(new GridModelBuilder<VatListVM>(obj.AsQueryable(), g)
        //    {
        //        KeyProp = o => o.Code,// needed for Entity Framework | nesting | tree | api
        //        Map = o => new
        //        {
        //            o.Code,
        //            o.Name,
        //            o.Type,
        //            o.Percentage,
        //            o.Status,
        //            EffectiveFrom = o.EffectiveFrom.ToShortDateString(),
        //            EffectiveTo = o.EffectiveTo.ToShortDateString()
        //        }
        //    }.Build());
        //}
        //#endregion

        //#region " Posting "
        //[HttpPost]
        //public ActionResult Save(List<RoleAuthorizationVM> entity)
        //{
        //    Api.VatController vat = new Api.VatController();
        //    var obj = vat.AddVat(entity);
        //    return new JsonResult { Data = obj };
        //}
        //[HttpPut]
        //public ActionResult Update(Vat entity)
        //{
        //    Api.VatController vat = new Api.VatController();
        //    var obj = vat.EditVat(entity.Code, entity);
        //    return new JsonResult { Data = obj };
        //}
        //[HttpDelete]
        //public ActionResult Delete(string id)
        //{
        //    Api.VatController vat = new Api.VatController();
        //    var obj = vat.DeleteVat(id);
        //    return new JsonResult { Data = obj };
        //}
        //#endregion
    }
}