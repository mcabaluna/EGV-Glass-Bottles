using Merchandising.DTO;
using Merchandising.DTO.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;

namespace Merchandising.Api.Controllers.Portal
{
    public class RoleMenusController : ApiController
    {
        private DbContextModel db = new DbContextModel();

        /// <summary>
        /// GetRoleMenusList
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.Route("portal/api/rolemenus/getrolemenuslist")]
        public IHttpActionResult GetRoleMenusList(string search = null)
        {
            var rolemenu = new List<RoleMenus>();
            List<RoleMenus> list = new List<RoleMenus>();
            //get all roles with filter 
            rolemenu =  db.RoleMenus
                         .OrderByDescending(x => x.RoleMenuId)
                         .ToList();
            //Search fields
            if (!string.IsNullOrEmpty(search))
            {
                rolemenu = rolemenu.Where(x =>
                        x.RoleMenuId.ToLower().Contains(search.ToLower()) ||
                        x.RoleId.ToString().ToLower().Contains(search.ToLower())||
                        x.MenuName.ToString().ToLower().Contains(search.ToLower()))
                    .OrderByDescending(x => x.RoleMenuId)
                    .ToList();
            }
            if (rolemenu.Count > 0)
            {
                list = rolemenu.Select(x => new RoleMenus()
                {
                    RoleMenuId = x.RoleMenuId,
                    RoleId = x.RoleId,
                    MenuName = x.MenuName,
                    Visible = x.Visible,
                    Status = x.Status
                }).ToList();
            }
            return Ok(list);
        }
        /// <summary>
        /// GetRoleMenus
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("portal/api/rolemenus/getrolemenusinfo")]
        public IHttpActionResult GetRoleMenus()
        {
            var rolemenus = db.RoleMenus.ToList();
            return Ok(rolemenus);
        }
        /// <summary>
        /// GetRoleMenus
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.Route("portal/api/rolemenus/{id}")]
        public IHttpActionResult GetRoleMenus(string id)
        {
            RoleMenus rolemenus = db.RoleMenus.Find(id);
            if (rolemenus == null)
            {
                return NotFound();
            }

            return Ok(rolemenus);
        }
        /// <summary>
        /// EditRoles
        /// </summary>
        /// <param name="id"></param>
        /// <param name="rolemenus"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPut, System.Web.Http.Route("portal/api/rolemenus/{id}")]
        public IHttpActionResult EditRoleMenus(string id, [FromBody]List<RoleMenus> rolemenus)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            foreach(var menus in rolemenus)
            {
                if (id != menus.RoleMenuId)
                {
                    return BadRequest();
                }

                db.Entry(menus).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoleMenusExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }           

            return Ok(rolemenus);
        }
        /// <summary>
        /// AddRoles
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost, System.Web.Http.Route("portal/api/rolemenus")]
        public IHttpActionResult AddRoleMenus(List<RoleMenus> rolemenus)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                foreach(var menus in rolemenus)
                {
                    //check role menus if exists
                    var check = db.RoleMenus.Where(x => x.RoleMenuId.ToUpper().Trim() == menus.RoleMenuId.ToUpper().Trim()).Any();
                    if (check)
                    {
                        return BadRequest("Role already exists! Please create different role.");
                    }
                    var identity = (ClaimsIdentity)User.Identity;
                    menus.CreatedById = identity.Name;
                    db.RoleMenus.Add(menus);
                    db.SaveChanges();
                }
                return Ok(rolemenus);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// DeleteRoleMenus
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [System.Web.Http.HttpDelete, System.Web.Http.Route("portal/api/rolemenus/{id}")]
        public IHttpActionResult DeleteRoleMenus(string id)
        {
            RoleMenus rolemenus = db.RoleMenus.Find(id);
            if (rolemenus == null)
            {
                return NotFound();
            }

            db.RoleMenus.Remove(rolemenus);
            db.SaveChanges();

            return Ok(rolemenus);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        private bool RoleMenusExists(string id)
        {
            return db.RoleMenus.Count(e => e.RoleMenuId == id) > 0;
        }
    }
}
