using Merchandising.DTO;
using Merchandising.DTO.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;

namespace Merchandising.Api.Controllers.Portal
{
    public class RolePageController : ApiController
    {
        private DbContextModel db = new DbContextModel();

        /// <summary>
        /// GetRolePageList
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.Route("portal/api/rolepage/getrolepagelist")]
        public IHttpActionResult GetRolePageList(string search = null)
        {
            var rolepage = new List<RolePage>();
            List<RolePage> list = new List<RolePage>();
            //get all roles with filter 
            rolepage =  db.RolePage
                         .OrderByDescending(x => x.RolePageId)
                         .ToList();
            //Search fields
            if (!string.IsNullOrEmpty(search))
            {
                rolepage = rolepage.Where(x =>
                        x.RolePageId.ToLower().Contains(search.ToLower()) ||
                        x.RoleId.ToString().ToLower().Contains(search.ToLower()) ||
                        x.Module.ToString().ToLower().Contains(search.ToLower()) ||
                        x.Page.ToString().ToLower().Contains(search.ToLower()))
                    .OrderByDescending(x => x.RolePageId)
                    .ToList();
            }
            if (rolepage.Count > 0)
            {
                list = rolepage.Select(x => new RolePage()
                {
                    RolePageId = x.RolePageId,
                    RoleId = x.RoleId,
                    Module = x.Module,
                    Page = x.Page,
                    Button = x.Button,
                    Visible = x.Visible,
                    Status = x.Status
                }).ToList();
            }
            return Ok(list);
        }
        /// <summary>
        /// GetRolePage
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("portal/api/roles/getrolepageinfo")]
        public IHttpActionResult GetRolePage()
        {
            var rolepage =db.RolePage.ToList();
            return Ok(rolepage);
        }

        /// <summary>
        /// GetRolePage
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.Route("portal/api/rolepage/{id}")]
        public IHttpActionResult GetRolePage(string id)
        {
            RolePage rolepage = db.RolePage.Find(id);
            if (rolepage == null)
            {
                return NotFound();
            }

            return Ok(rolepage);
        }

        /// <summary>
        /// EditRolePage
        /// </summary>
        /// <param name="id"></param>
        /// <param name="rolepage"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPut, System.Web.Http.Route("portal/api/rolepage/{id}")]
        public IHttpActionResult EditRolePage(string id, [FromBody]RolePage rolepage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != rolepage.RolePageId)
            {
                return BadRequest();
            }

            db.Entry(rolepage).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RolePageExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(rolepage);
        }

        /// <summary>
        /// AddRoles
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost, System.Web.Http.Route("portal/api/rolepage")]
        public IHttpActionResult AddRolePage(List<RolePage> rolepage)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                foreach (var page in rolepage)
                {
                    //check rolepage if exists
                    //var check = db.RolePage.Where(x => x.RolePageId.ToUpper().Trim() == page.RolePageId.ToUpper().Trim()).Any();
                    //if (check)
                    //{
                    //    return BadRequest("Role Page already exists! Please create different role.");
                    //}
                    var identity = (ClaimsIdentity)User.Identity;
                    page.CreatedById = identity.Name;
                    db.RolePage.Add(page);
                    db.SaveChanges();
                }

                return Ok(rolepage);
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
        /// DeleteRolePage
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [System.Web.Http.HttpDelete, System.Web.Http.Route("portal/api/rolepage/{id}")]
        public IHttpActionResult DeleteRolePage(string id)
        {
            RolePage rolepage = db.RolePage.Find(id);
            if (rolepage == null)
            {
                return NotFound();
            }

            db.RolePage.Remove(rolepage);
            db.SaveChanges();

            return Ok(rolepage);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        private bool RolePageExists(string id)
        {
            return db.RolePage.Count(e => e.RolePageId == id) > 0;
        }
    }
}
