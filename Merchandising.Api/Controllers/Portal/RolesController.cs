using Merchandising.DTO;
using Merchandising.DTO.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;

namespace Merchandising.Api.Controllers.Portal
{
    public class RolesController : ApiController
    {
        private DbContextModel db = new DbContextModel();

        /// <summary>
        /// GetRolesList
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.Route("portal/api/roles/getroleslist")]
        public IHttpActionResult GetRolesList(string search = null)
        {
            var roles = new List<Roles>();
            List<Roles> list = new List<Roles>();
            //get all roles with filter 
            roles = db.Roles
                         .OrderByDescending(x => x.RoleName)
                         .ToList();
            //Search fields
            if (!string.IsNullOrEmpty(search))
            {
                roles = roles.Where(x =>
                        x.Description.ToLower().Contains(search.ToLower()) ||
                        x.RoleName.ToString().ToLower().Contains(search.ToLower()))
                    .OrderByDescending(x => x.RoleName)
                    .ToList();
            }
            if (roles.Count > 0)
            {
                list = roles.Select(x => new Roles()
                {
                    RoleId = x.RoleId,
                    RoleName = x.RoleName,
                    Description = x.Description
                }).ToList();
            }
            return Ok(list);
        }
        /// <summary>
        /// GetRoles
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("portal/api/roles/getrolesinfo")]
        public IHttpActionResult GetRoles()
        {
            var roles = db.Roles.ToList();
            return Ok(roles);
        }

        /// <summary>
        /// GetRoles
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.Route("portal/api/roles/{id}")]
        public IHttpActionResult GetRoles(int id)
        {
            Roles role =  db.Roles.Find(id);
            if (role == null)
            {
                return NotFound();
            }

            return Ok(role);
        }

        /// <summary>
        /// EditRoles
        /// </summary>
        /// <param name="id"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPut, System.Web.Http.Route("portal/api/roles/{id}")]
        public IHttpActionResult EditRoles(int id, [FromBody]Roles role)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != role.RoleId)
            {
                return BadRequest();
            }

            db.Entry(role).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(role);
        }

        /// <summary>
        /// AddRoles
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost, System.Web.Http.Route("portal/api/roles")]
        public IHttpActionResult AddRoles(Roles role)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                //check role if exists
                var check = db.Roles.Where(x => x.RoleId == role.RoleId).Any();
                if (check)
                {
                    return BadRequest("Role already exists! Please create different role.");
                }
                var identity = (ClaimsIdentity)User.Identity;
                role.CreatedById = identity.Name;
                db.Roles.Add(role);
                db.SaveChanges();
                return Ok(role);
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
        /// DeleteRoles
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [System.Web.Http.HttpDelete, System.Web.Http.Route("portal/api/roles/{id}")]
        public IHttpActionResult DeleteRoles(int id)
        {
            Roles role =  db.Roles.Find(id);
            if (role == null)
            {
                return NotFound();
            }

            db.Roles.Remove(role);
            db.SaveChanges();

            return Ok(role);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RoleExists(int id)
        {
            return db.Roles.Count(e => e.RoleId == id) > 0;
        }
    }
}
