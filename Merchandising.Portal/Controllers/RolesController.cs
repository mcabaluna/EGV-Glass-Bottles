using Merchandising.DTO.Models;
using Merchandising.Portal.Models;
using Omu.AwesomeMvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Merchandising.Portal.Controllers
{
    public class RolesController : Controller
    {
        #region " View Method"
        // GET: Branch
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetRoles(string id)
        {
            var obj = MerchandisingApiWrapper.Get<Roles>(
                                  typeof(Roles).Name + $"/{id}");
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetList(GridParams g, string search = null)
        {
            var obj = MerchandisingApiWrapper.Get<List<Roles>>(
                typeof(Roles).Name + "/getroleslist" + $"?search={search}");

            //return Json(new {data = obj}, JsonRequestBehavior.AllowGet);
            return Json(new GridModelBuilder<Roles>(obj.AsQueryable(), g)
            {
                KeyProp = o => o.RoleId,// needed for Entity Framework | nesting | tree | api
                Map = o => new
                {
                    o.RoleId,
                    o.RoleName,
                    o.Description
                }
            }.Build());
        }
        #endregion

        #region " Posting "
        [HttpPost]
        public ActionResult Save(Roles entity)
        {
            Api.RolesController role = new Api.RolesController();
            var obj = role.AddRoles(entity);
            return new JsonResult { Data = obj };
        }
        [HttpPut]
        public ActionResult Update(Roles entity)
        {
            Api.RolesController role = new Api.RolesController();
            var obj = role.EditRoles(entity.RoleId, entity);
            return new JsonResult { Data = obj };
        }
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Api.RolesController role = new Api.RolesController();
            var obj = role.DeleteRoles(id);
            return new JsonResult { Data = obj };
        }
        #endregion
    }
}