using Merchandising.DTO;
using Merchandising.DTO.Models;
using Merchandising.Helper;
using Merchandising.VM.Portal;
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
    public class BpGroupsController : ApiController
    {
        private DbContextModel db = new DbContextModel();

        /// <summary>
        /// GetBpGrouplist
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.Route("portal/api/bpgroup/getbpgrouplist")]
        public IHttpActionResult GetBpGrouplist(string search = null)
        {
            var bpgroup = new List<BpGroup>();
            List<BPGroupListVM> list = new List<BPGroupListVM>();
            bpgroup =  db.BpGroups.OrderByDescending(x => x.Code).ToList();
            //Search fields
            if (!string.IsNullOrEmpty(search))
            {
                bpgroup = bpgroup.Where(x =>
                        x.Code.ToLower().Contains(search.ToLower()) ||
                        x.Name.ToString().ToLower().Contains(search.ToLower()) ||
                        x.BpType.ToString().ToLower().Contains(search.ToLower()))
                    .OrderByDescending(x => x.Code)
                    .ToList();
            }
            if (bpgroup.Count > 0)
            {
                list = bpgroup.Select(x => new BPGroupListVM()
                {
                    Code = x.Code,
                    Name = x.Name,
                    BPType = GlobalFunctions.GetBPGroupValue(x.BpType),
                    Status = x.Status
                }).ToList();
            }
            return Ok(list);
        }

        /// <summary>
        /// GetBpGroup
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("portal/api/bpgroup/getbpgroupinfo")]
        public IHttpActionResult GetBpGroup()
        {
            var bpgroup = db.BpGroups.Where(x=> x.Status.Equals(true)).ToList();
            return Ok(bpgroup);
        }

        /// <summary>
        /// GetBpGroup
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.Route("portal/api/bpgroup/{id}")]
        public IHttpActionResult GetBpGroup(string id)
        {
            BpGroup bpgroup =  db.BpGroups.Find(id);
            if (bpgroup == null)
            {
                return NotFound();
            }

            return Ok(bpgroup);
        }

        /// <summary>
        /// EditBpGroup
        /// </summary>
        /// <param name="id"></param>
        /// <param name="bpgroup"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPut, System.Web.Http.Route("portal/api/bpgroup/{id}")]
        public IHttpActionResult EditBpGroup(string id, [FromBody]BpGroup bpgroup)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != bpgroup.Code)
            {
                return BadRequest();
            }

            db.Entry(bpgroup).State = EntityState.Modified;

            try
            {
               db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BpGroupExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok(bpgroup);
        }

        /// <summary>
        /// AddBpGroup
        /// </summary>
        /// <param name="bpgroup"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost, System.Web.Http.Route("portal/api/bpgroup")]
        public IHttpActionResult AddBpGroup([FromBody]BpGroup bpgroup)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                //check bpgroup if exists
                var check = db.BpGroups.Where(x => x.Code.ToUpper().Trim() == bpgroup.Code.ToUpper().Trim()).Any();
                if (check)
                {
                    return BadRequest("BP Group already exists! Please create different bpgroup.");
                }
                var identity = (ClaimsIdentity)User.Identity;
                bpgroup.CreatedById = identity.Name;
                db.BpGroups.Add(bpgroup);
                db.SaveChanges();
                return Ok(bpgroup);
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
        /// DeleteBpGroup
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [System.Web.Http.HttpDelete, System.Web.Http.Route("portal/api/bpgroup/{id}")]
        public IHttpActionResult DeleteBpGroup(string id)
        {
            BpGroup bpgroup = db.BpGroups.Find(id);
            if (bpgroup == null)
            {
                return NotFound();
            }

            db.BpGroups.Remove(bpgroup);
            db.SaveChanges();

            return Ok(bpgroup);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BpGroupExists(string id)
        {
            return db.BpGroups.Count(e => e.Code == id) > 0;
        }
    }
}
