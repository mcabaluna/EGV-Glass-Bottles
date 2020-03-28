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
    public class BranchController : Controller
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
            ViewBag.Filter = new SelectList(statusData.ToList(), "Value", "Text");
            ViewBag.Status = new SelectList(statusData.ToList(), "Value", "Text", 1);
            return View();
        }
        public ActionResult Create()
        {
            return View();
        }
        public ActionResult GetBranch(string id)
        {
            var obj = MerchandisingApiWrapper.Get<Branch>(
                                  typeof(Branch).Name + $"/{id}");
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetBranchInfo()
        {
            var obj = MerchandisingApiWrapper.Get<List<Branch>>(
                               typeof(Branch).Name + "/getbranchinfo");
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetList(GridParams g, string search = null)
        {
            var obj = MerchandisingApiWrapper.Get<List<BranchListVM>>(
                typeof(Branch).Name + "/getbranchlist" + $"?search={search}");

            //return Json(new {data = obj}, JsonRequestBehavior.AllowGet);
            return Json(new GridModelBuilder<BranchListVM>(obj.AsQueryable(), g)
            {
                KeyProp = o => o.Code,// needed for Entity Framework | nesting | tree | api
                Map = o => new
                {
                    o.Code,
                    o.Name,
                    o.Status,
                    ValidFrom = o.ValidFrom.ToShortDateString(),
                    ValidTo = o.ValidTo.ToShortDateString()
                }
            }.Build());
        }
        #endregion

        #region " Posting "
        [HttpPost]
        public ActionResult Save(Branch entity)
        {
            Api.BranchController branch = new Api.BranchController();
            var obj = branch.AddBranch(entity);
            return new JsonResult { Data = obj };
        }
        [HttpPut]
        public ActionResult Update(Branch entity)
        {
            Api.BranchController branch = new Api.BranchController();
            var obj = branch.EditBranch(entity.Code, entity);
            return new JsonResult { Data = obj };
        }
        [HttpDelete]
        public ActionResult Delete(string id)
        {
            Api.BranchController branch = new Api.BranchController();
            var obj = branch.DeleteBranch(id);
            return new JsonResult { Data = obj };
        }
        #endregion
    }
}