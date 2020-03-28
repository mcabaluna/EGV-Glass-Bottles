using Merchandising.DTO.Models;
using Merchandising.Portal.Models;
using Merchandising.VM.Portal;
using Omu.AwesomeMvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace Merchandising.Portal.Controllers
{
    public class UsersController : Controller
    {
        #region " View Method "
        /// <summary>
        /// Index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //var roleData = from AccessRoles e in Enum.GetValues(typeof(AccessRoles))
            //               select new SelectListItem
            //               {
            //                   Value = Convert.ToString((int)e),
            //                   Text = e.ToString()
            //               };
            var objroles = MerchandisingApiWrapper.Get<List<Merchandising.DTO.Models.Roles>>(
                            typeof(Merchandising.DTO.Models.Roles).Name + "/getrolesinfo");

            var roles = objroles.Select(y => new { y.RoleId, y.RoleName }).Distinct().ToList();
            IEnumerable<SelectListItem> rolelist =
                from s in roles
                select new SelectListItem
                {
                    Text = s.RoleName,
                    Value = s.RoleId.ToString()
                };
            var objbranch = MerchandisingApiWrapper.Get<List<Branch>>(
                                 typeof(Branch).Name + "/getbranchinfo");

            var branch = objbranch.Select(y => new { y.Code, y.Name }).Distinct().ToList();
            IEnumerable<SelectListItem> branchlist =
                from s in branch
                select new SelectListItem
                {
                    Text = s.Code + " - " + s.Name,
                    Value = s.Code
                };

            UserVM userVM = new UserVM()
            {
                AccessRoleOption = new SelectList(rolelist, "Value", "Text"),
                BranchOption = new SelectList(branchlist, "Value", "Text"),
                Users = new Users()
            };
            return View(userVM);
        }
        public ActionResult GetUser(string id)
        {
            var obj = MerchandisingApiWrapper.Get<Users>(
                                  typeof(Users).Name + $"/{id}");
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Login", "Users");
        }
        public ActionResult Login(Users user)
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("ErrorPage", "Users");
            }
            if ((Session.Count > 0))// && Session["Token"] != null)
            {
                return RedirectToAction("Home_Index", "Home");
            }
            if (user.UserId != null && user.Password != null)
            {
                try
                {
                    var obj = MerchandisingApiWrapper.Put<Users>(typeof(Users).Name
                   + "/authenticateuser/" + $"{user.UserId}", user);

                    var objroles = MerchandisingApiWrapper.Get<Merchandising.DTO.Models.Roles>(
                                 typeof(Merchandising.DTO.Models.Roles).Name + $"/{obj.Role}");

                    //HttpContext.Session.SetString("email", _admin.Email);
                    //HttpContext.Session.SetInt32("id", _admin.Id);
                    //HttpContext.Session.SetInt32("role_id", (int)_admin.RolesId);
                    //HttpContext.Session.SetString("name", _admin.FullName);

                    FormsAuthentication.SetAuthCookie(user.UserId, false);
                    Session["Username"] = user.UserId;
                    Session["Role"] = objroles.RoleName;
                    return RedirectToAction("Home_Index", "Home");
                }
                catch (Exception ex)
                {
                    if (ex.Message == "Not Found")
                        ModelState.AddModelError("", "User not found!");
                    else
                        ModelState.AddModelError("", ex.Message);
                }
            }
            else
            {
                //ModelState.AddModelError("", "Error, Check data");
            }
            return View(user);
        }
        /// <summary>
        /// GetList
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public ActionResult GetList(GridParams g, string search = null)
        {
            var obj = MerchandisingApiWrapper.Get<List<UserListVM>>(
                typeof(Users).Name + "/getuserlist" + $"?search={search}");

            //return Json(new {data = obj}, JsonRequestBehavior.AllowGet);
            return Json(new GridModelBuilder<UserListVM>(obj.AsQueryable(), g)
            {
                KeyProp = o => o.UserId,// needed for Entity Framework | nesting | tree | api
                Map = o => new
                {

                    o.UserId,
                    o.UserName,
                    o.RoleName,
                    o.BranchName,
                    o.ContactNo,
                    o.Email,
                    o.Status,
                    LastAccess = o.LastAccess.HasValue == true ? o.LastAccess.Value.ToShortDateString() + " " + o.LastAccess.Value.ToShortTimeString() : string.Empty
                }
            }.Build());
        }

        #endregion
        #region " API Posting "
        [HttpPost]
        public ActionResult Save(Users entity)
        {
            Api.UsersController users = new Api.UsersController();
            var obj = users.AddUser(entity);
            return new JsonResult { Data = obj };
        }
        [HttpPut]
        public ActionResult Update(Users entity)
        {
            Api.UsersController users = new Api.UsersController();
            var obj = users.EditUser(entity.UserId, entity);
            return new JsonResult { Data = obj };
        }
        [HttpDelete]
        public ActionResult Delete(string id)
        {
            Api.UsersController users = new Api.UsersController();
            var obj = users.DeleteUser(id);
            return new JsonResult { Data = obj };
        }
        #endregion

    }
}