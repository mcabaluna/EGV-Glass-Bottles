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
    public class BpGroupController : Controller
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
            var typeData = from BpType e in Enum.GetValues(typeof(BpType))
                           select new SelectListItem
                           {
                               Value = Convert.ToString((int)e),
                               Text = e.ToString()
                           };
            ViewBag.Filter = new SelectList(statusData.ToList(), "Value", "Text");

            BPGroupVM bpgroupVM = new BPGroupVM()
            {
                StatusOption = new SelectList(statusData, "Value", "Text", "1"),
                BPTypeOption = new SelectList(typeData, "Value", "Text"),
                BpGroup = new BpGroup()
            };
            return View(bpgroupVM);
        }
        public ActionResult GetBpGroup(string id)
        {
            var obj = MerchandisingApiWrapper.Get<BpGroup>(
                                  typeof(BpGroup).Name + $"/{id}");
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetList(GridParams g, string search = null)
        {
            var obj = MerchandisingApiWrapper.Get<List<BPGroupListVM>>(
                typeof(BpGroup).Name + "/getbpgrouplist" + $"?search={search}");

            //return Json(new {data = obj}, JsonRequestBehavior.AllowGet);
            return Json(new GridModelBuilder<BPGroupListVM>(obj.AsQueryable(), g)
            {
                KeyProp = o => o.Code,// needed for Entity Framework | nesting | tree | api
                Map = o => new
                {
                    o.Code,
                    o.Name,
                    o.BPType,
                    o.Status
                }
            }.Build());
        }
        #endregion

        #region " Posting "
        [HttpPost]
        public ActionResult Save(BpGroup entity)
        {
            Api.BpGroupController bpgroup = new Api.BpGroupController();
            var obj = bpgroup.AddBpGroup(entity);
            return new JsonResult { Data = obj };
        }
        [HttpPut]
        public ActionResult Update(BpGroup entity)
        {
            Api.BpGroupController bpgroup = new Api.BpGroupController();
            var obj = bpgroup.EditBpGroup(entity.Code, entity);
            return new JsonResult { Data = obj };
        }
        [HttpDelete]
        public ActionResult Delete(string id)
        {
            Api.BpGroupController bpgroup = new Api.BpGroupController();
            var obj = bpgroup.DeleteBpGroup(id);
            return new JsonResult { Data = obj };
        }
        #endregion
    }
}