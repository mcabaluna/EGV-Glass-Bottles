using Merchandising.DTO;
using Merchandising.DTO.Models;
using Merchandising.VM.Portal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;

namespace Merchandising.Api.Controllers.Portal
{
    public class BranchController : ApiController
    {
        private DbContextModel db = new DbContextModel();

        /// <summary>
        /// GetBranchList
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.Route("portal/api/branch/getbranchlist")]
        public IHttpActionResult GetBranchList(string search = null)
        {
            var branch = new List<Branch>();
            List<BranchListVM> list = new List<BranchListVM>();
            //get all users with filter 

            branch =  db.Branches
                        .OrderByDescending(x => x.Code)
                        .ToList();
            //Search fields
            if (!string.IsNullOrEmpty(search))
            {
                branch = branch.Where(x =>
                        x.Code.ToLower().Contains(search.ToLower()) ||
                        x.Name.ToString().ToLower().Contains(search.ToLower()) ||
                        x.ValidFrom.ToString().ToLower().Contains(search.ToLower()) ||
                        x.ValidTo.ToString().ToLower().Contains(search.ToLower()))
                    .OrderByDescending(x => x.Code)
                    .ToList();
            }
            if (branch.Count > 0)
            {
                list = branch.Select(x => new BranchListVM()
                {
                    Code = x.Code,
                    Name = x.Name,
                    Status = x.Status,
                    ValidFrom = x.ValidFrom,
                    ValidTo = x.ValidTo
                }).ToList();
            }
            return Ok(list);
        }
        /// <summary>
        /// GetBranches
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("portal/api/branch/getbranchinfo")]
        public IHttpActionResult GetBranches()
        {
            var branches = db.Branches.Where(x => x.Status.Equals(true) &&
                                                        ((DateTime.Now.Year >= x.ValidFrom.Year &&DateTime.Now.Month >= x.ValidFrom.Month && DateTime.Now.Day >= x.ValidFrom.Day) &&
                                                        (DateTime.Now.Year <= x.ValidTo.Year && DateTime.Now.Month <= x.ValidTo.Month && DateTime.Now.Day <= x.ValidTo.Day))).ToList();
            return Ok(branches);
        }

        /// <summary>
        /// GetBranch
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.Route("portal/api/branch/{id}")]
        public IHttpActionResult GetBranch(string id)
        {
            Branch branch =  db.Branches.Find(id);
            if (branch == null)
            {
                return NotFound();
            }

            return Ok(branch);
        }

        /// <summary>
        /// EditBranch
        /// </summary>
        /// <param name="id"></param>
        /// <param name="branch"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPut, System.Web.Http.Route("portal/api/branch/{id}")]
        public IHttpActionResult EditBranch(string id, [FromBody]Branch branch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != branch.Code)
            {
                return BadRequest();
            }

            db.Entry(branch).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BranchExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(branch);
        }

        /// <summary>
        /// PostBranch
        /// </summary>
        /// <param name="branch"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost, System.Web.Http.Route("portal/api/branch")]
        public IHttpActionResult AddBranch([FromBody]Branch branch)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                //check branch if exists
                var check = db.Branches.Where(x => x.Code.ToUpper().Trim() == branch.Code.ToUpper().Trim()).Any();
                if (check)
                {
                    return BadRequest("Branch already exists! Please create different branch.");
                }
                var identity = (ClaimsIdentity)User.Identity;
                branch.CreatedById = identity.Name;
                db.Branches.Add(branch);
                db.SaveChanges();
                return Ok(branch);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            //return CreatedAtRoute("DefaultApi", new { id = branch.Code }, branch);
        }

        /// <summary>
        /// DeleteBranch
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [System.Web.Http.HttpDelete, System.Web.Http.Route("portal/api/branch/{id}")]
        public IHttpActionResult DeleteBranch(string id)
        {
            Branch branch = db.Branches.Find(id);
            if (branch == null)
            {
                return NotFound();
            }

            db.Branches.Remove(branch);
            db.SaveChanges();

            return Ok(branch);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BranchExists(string id)
        {
            return db.Branches.Count(e => e.Code == id) > 0;
        }
    }
}