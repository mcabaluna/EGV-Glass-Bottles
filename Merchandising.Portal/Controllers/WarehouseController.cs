using Merchandising.DTO.Models;
using Merchandising.Enums;
using Merchandising.Portal.Models;
using Merchandising.VM.Portal;
using Omu.AwesomeMvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Merchandising.Portal.Controllers
{
    public class WarehouseController : Controller
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
            try
            {
                var obj = MerchandisingApiWrapper.Get<List<Branch>>(
                                typeof(Branch).Name + "/getbranchinfo");

                var branch = obj.Select(y => new { y.Code, y.Name }).Distinct().ToList();
                IEnumerable<SelectListItem> selectList =
                    from s in branch
                    select new SelectListItem
                    {
                        Text = s.Code + " - " + s.Name,
                        Value = s.Code
                    };
                WarehouseVM warehouseVM = new WarehouseVM()
                {
                    StatusOption = new SelectList(statusData, "Value", "Text", "1"),
                    BranchOption = new SelectList(selectList, "Value", "Text"),
                    Warehouse = new Warehouse()
                };
                return View(warehouseVM);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        public ActionResult GetWarehouse(string id)
        {
            var obj = MerchandisingApiWrapper.Get<Warehouse>(
                                  typeof(Warehouse).Name + $"/{id}");
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetWarehouseInfo()
        {
            var obj = MerchandisingApiWrapper.Get<List<Warehouse>>(
                               typeof(Warehouse).Name + "/getwarehouseinfo");
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetList(GridParams g, string search = null)
        {
            var obj = MerchandisingApiWrapper.Get<List<WarehouseListVM>>(
                typeof(Warehouse).Name + "/getwarehouselist" + $"?search={search}");

            //return Json(new {data = obj}, JsonRequestBehavior.AllowGet);
            return Json(new GridModelBuilder<WarehouseListVM>(obj.AsQueryable(), g)
            {
                KeyProp = o => o.Code,// needed for Entity Framework | nesting | tree | api
                Map = o => new
                {
                    o.Code,
                    o.Name,
                    o.Status,
                    o.BranchCode
                }
            }.Build());
        }
        #endregion

        #region " Posting "
        [HttpPost]
        public ActionResult Save(Warehouse entity)
        {
            Api.WarehouseController warehouse = new Api.WarehouseController();
            var obj = warehouse.AddWarehouse(entity);
            return new JsonResult { Data = obj };
        }
        [HttpPut]
        public ActionResult Update(Warehouse entity)
        {
            Api.WarehouseController warehouse = new Api.WarehouseController();
            var obj = warehouse.EditWarehouse(entity.Code, entity);
            return new JsonResult { Data = obj };
        }
        [HttpDelete]
        public ActionResult Delete(string id)
        {
            Api.WarehouseController warehouse = new Api.WarehouseController();
            var obj = warehouse.DeleteWarehouse(id);
            return new JsonResult { Data = obj };
        }
        #endregion
    }
}