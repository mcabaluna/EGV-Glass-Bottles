using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Merchandising.DTO;
using Merchandising.DTO.Models;
using Merchandising.VM.Portal;

namespace Merchandising.Api.Controllers.Portal
{
    public class WarehousesController : ApiController
    {
        private DbContextModel db = new DbContextModel();

        /// <summary>
        /// GetWarehouseList
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.Route("portal/api/warehouse/getwarehouselist")]
        public IHttpActionResult GetWarehouseList(string search = null)
        {
            var whse = new List<Warehouse>();
            List<WarehouseListVM> list = new List<WarehouseListVM>();
            whse =  db.Warehouses
                        .OrderByDescending(x => x.Code)
                        .ToList();
            //Search fields
            if (!string.IsNullOrEmpty(search))
            {
                whse = whse.Where(x =>
                        x.Code.ToLower().Contains(search.ToLower()) ||
                        x.Name.ToString().ToLower().Contains(search.ToLower()) ||
                        x.BranchCode.ToString().ToLower().Contains(search.ToLower()))
                    .OrderByDescending(x => x.Code)
                    .ToList();
            }
            if (whse.Count > 0)
            {
                list = whse.Select(x => new WarehouseListVM()
                {
                    Code = x.Code,
                    Name = x.Name,
                    Status = x.Status,
                    BranchCode = x.BranchCode
                }).ToList();
            }
            return Ok(list);
        }

        /// <summary>
        /// GetWarehouse
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("portal/api/warehouse/getwarehouseinfo")]
        public IHttpActionResult GetWarehouse()
        {
            var whse = db.Warehouses.Where(x => x.Status.Equals(true)).ToList();
            return Ok(whse);
        }

        /// <summary>
        /// GetWarehouse
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.Route("portal/api/warehouse/{id}")]
        public IHttpActionResult GetWarehouse(string id)
        {
            Warehouse whse =  db.Warehouses.Find(id);
            if (whse == null)
            {
                return NotFound();
            }

            return Ok(whse);
        }


        /// <summary>
        /// EditWarehouse
        /// </summary>
        /// <param name="id"></param>
        /// <param name="whse"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPut, System.Web.Http.Route("portal/api/warehouse/{id}")]
        public IHttpActionResult EditWarehouse(string id, [FromBody]Warehouse whse)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != whse.Code)
            {
                return BadRequest();
            }

            db.Entry(whse).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WarehouseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(whse);
        }

        /// <summary>
        /// AddWarehouse
        /// </summary>
        /// <param name="whse"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost, System.Web.Http.Route("portal/api/warehouse")]
        public IHttpActionResult AddWarehouse([FromBody]Warehouse whse)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                //check warehouse if exists
                var check = db.Warehouses.Where(x => x.Code.ToUpper().Trim() == whse.Code.ToUpper().Trim()).Any();
                if (check)
                {
                    return BadRequest("Warehouse already exists! Please create different warehouse.");
                }
                var identity = (ClaimsIdentity)User.Identity;
                whse.CreatedById = identity.Name;
                db.Warehouses.Add(whse);
                db.SaveChanges();
                return Ok(whse);
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
        /// DeleteWarehouse
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [System.Web.Http.HttpDelete, System.Web.Http.Route("portal/api/warehouse/{id}")]
        public IHttpActionResult DeleteWarehouse(string id)
        {
            Warehouse whse =  db.Warehouses.Find(id);
            if (whse == null)
            {
                return NotFound();
            }

            db.Warehouses.Remove(whse);
            db.SaveChanges();

            return Ok(whse);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool WarehouseExists(string id)
        {
            return db.Warehouses.Count(e => e.Code == id) > 0;
        }
    }
}