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
    public class ItemGroupController : Controller
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

            ItemGroupVM itemGroupVM = new ItemGroupVM()
            {
                StatusOption = new SelectList(statusData, "Value", "Text", "1"),
                ItemGroup = new ItemGroup()
            };
            return View(itemGroupVM);
        }
        public ActionResult GetItemGroup(string id)
        {
            var obj = MerchandisingApiWrapper.Get<ItemGroup>(
                                  typeof(ItemGroup).Name + $"/{id}");
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetList(GridParams g, string search = null)
        {
            var obj = MerchandisingApiWrapper.Get<List<ItemGroupListVM>>(
                typeof(ItemGroup).Name + "/getitemgrouplist" + $"?search={search}");

            //return Json(new {data = obj}, JsonRequestBehavior.AllowGet);
            return Json(new GridModelBuilder<ItemGroupListVM>(obj.AsQueryable(), g)
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
        public ActionResult Save(ItemGroup entity)
        {
            Api.ItemGroupController itemgroup = new Api.ItemGroupController();
            var obj = itemgroup.AddItemGroup(entity);
            return new JsonResult { Data = obj };
        }
        [HttpPut]
        public ActionResult Update(ItemGroup entity)
        {
            Api.ItemGroupController itemgroup = new Api.ItemGroupController();
            var obj = itemgroup.EditModeOfPayment(entity.Code, entity);
            return new JsonResult { Data = obj };
        }
        [HttpDelete]
        public ActionResult Delete(string id)
        {
            Api.ItemGroupController itemgroup = new Api.ItemGroupController();
            var obj = itemgroup.DeleteModeOfPayment(id);
            return new JsonResult { Data = obj };
        }
        #endregion
    }
}