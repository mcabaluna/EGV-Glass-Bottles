using Merchandising.DTO;
using Merchandising.DTO.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace Merchandising.Api.Controllers.Portal
{
    /// <summary>
    /// AuditTrailLogsController
    /// </summary>
    public class AuditTrailLogsController : ApiController
    {
        private DbContextModel db = new DbContextModel();

        /// <summary>
        /// Get all Audit Trail Logs
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpGet, Route("portal/api/audittraillogs")]
        public  IHttpActionResult GetAuditTrailLogsList(string filter = "all", string search = null)
        {
            //get all audit trail logs with filter in mode field
            var audittrail = db.AuditTrailLogs.Where(x => x.Mode.ToLower() == filter.ToLower())
                                         .OrderByDescending(x => x.AuditId)
                                         .ToList();

            //Search fields
            if (!string.IsNullOrEmpty(search))
            {
                audittrail = audittrail.Where(x => x.Document.ToLower().Contains(search.ToLower()) || x.Branch.ToLower().Contains(search.ToLower()) ||
                              x.ComputerName.ToLower().Contains(search.ToLower()) || x.IpAddress.ToLower().Contains(search.ToLower()) ||
                              x.UpdatedBy.ToLower().Contains(search.ToLower()) || x.UpdatedTime.ToString(CultureInfo.InvariantCulture).ToLower().Contains(search.ToLower()))
                              .OrderByDescending(x => x.AuditId)
                              .ToList();
            }
            return Ok(audittrail);
        }

        /// <summary>
        /// GetAuditTrailLogs
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet, Route("portal/api/audittraillogs/{id}")]
        public IHttpActionResult GetAuditTrailLogs(int id)
        {

            AuditTrailLogs auditTrailLogs =  db.AuditTrailLogs.Find(id);
            if (auditTrailLogs == null)
            {
                return NotFound();
            }

            return Ok(auditTrailLogs);
        }

        /// <summary>
        /// EditAuditTrailLogs
        /// </summary>
        /// <param name="id"></param>
        /// <param name="auditTrailLogs"></param>
        /// <returns></returns>
        [HttpPut, Route("portal/api/audittraillogs/{id}")]
        public IHttpActionResult EditAuditTrailLogs(int id, [FromBody] AuditTrailLogs auditTrailLogs)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != auditTrailLogs.AuditId)
            {
                return BadRequest();
            }

            db.Entry(auditTrailLogs).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuditTrailLogsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok();
        }

        /// <summary>
        /// AddAuditTrailLogs
        /// </summary>
        /// <param name="auditTrailLogs"></param>
        /// <returns></returns>
        [HttpPost,Route("portal/api/audittraillogs")]
        public IHttpActionResult AddAuditTrailLogs([FromBody]AuditTrailLogs auditTrailLogs)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                db.AuditTrailLogs.Add(auditTrailLogs);
                db.SaveChanges();

                return Ok(auditTrailLogs);
            }
            catch ( System.Exception ex)
            {
                return BadRequest(ex.Message);
            }

            
        }

        /// <summary>
        /// DeleteAuditTrailLogs
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete,Route("portal/api/audittraillogs/{id}")]
        public IHttpActionResult DeleteAuditTrailLogs(int id)
        {
            try
            {
                AuditTrailLogs auditTrailLogs =  db.AuditTrailLogs.Find(id);
                if (auditTrailLogs == null)
                {
                    return NotFound();
                }

                db.AuditTrailLogs.Remove(auditTrailLogs);
                db.SaveChanges();

                return Ok();

            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AuditTrailLogsExists(int id)
        {
            return db.AuditTrailLogs.Count(e => e.AuditId == id) > 0;
        }
    }
}