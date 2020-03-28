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
    public class ItemGroupsController : ApiController
    {
        private DbContextModel db = new DbContextModel();

        /// <summary>
        /// GetItemGroupList
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.Route("portal/api/itemgroup/getitemgrouplist")]
        public IHttpActionResult GetItemGroupList(string search = null)
        {
            var itemgroup = new List<ItemGroup>();
            List<ItemGroupListVM> list = new List<ItemGroupListVM>();
            itemgroup =  db.ItemGroups
                        .OrderByDescending(x => x.Code)
                        .ToList();
            //Search fields
            if (!string.IsNullOrEmpty(search))
            {
                itemgroup = itemgroup.Where(x =>
                        x.Code.ToLower().Contains(search.ToLower()) ||
                        x.Name.ToString().ToLower().Contains(search.ToLower()))
                    .OrderByDescending(x => x.Code)
                    .ToList();
            }
            if (itemgroup.Count > 0)
            {
                list = itemgroup.Select(x => new ItemGroupListVM()
                {
                    Code = x.Code,
                    Name = x.Name,
                    Status = x.Status
                }).ToList();
            }
            return Ok(list);
        }

        /// <summary>
        /// GetItemGroup
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("portal/api/itemgroup/getitemgroupinfo")]
        public IHttpActionResult GetItemGroup()
        {
            var itemgroup =  db.ItemGroups.Where(x=> x.Status.Equals(true)).ToList();
            return Ok(itemgroup);
        }

        /// <summary>
        /// GetItemGroup
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.Route("portal/api/itemgroup/{id}")]
        public IHttpActionResult GetItemGroup(string id)
        {
            ItemGroup itemgroup =  db.ItemGroups.Find(id);
            if (itemgroup == null)
            {
                return NotFound();
            }

            return Ok(itemgroup);
        }


        /// <summary>
        /// EditItemGroup
        /// </summary>
        /// <param name="id"></param>
        /// <param name="itemgroup"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPut, System.Web.Http.Route("portal/api/itemgroup/{id}")]
        public IHttpActionResult EditItemGroup(string id, ItemGroup itemgroup)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != itemgroup.Code)
            {
                return BadRequest();
            }

            db.Entry(itemgroup).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemGroupExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(itemgroup);
        }

        /// <summary>
        /// AddItemGroup
        /// </summary>
        /// <param name="itemgroup"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost, System.Web.Http.Route("portal/api/itemgroup")]
        public IHttpActionResult AddItemGroup(ItemGroup itemgroup)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                //check item group if exists
                var check = db.ItemGroups.Where(x => x.Code.ToUpper().Trim() == itemgroup.Code.ToUpper().Trim()).Any();
                if (check)
                {
                    return BadRequest("Item Group already exists! Please create different item group.");
                }
                var identity = (ClaimsIdentity)User.Identity;
                itemgroup.CreatedById = identity.Name;
                db.ItemGroups.Add(itemgroup);
                db.SaveChanges();
                return Ok(itemgroup);
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
        /// DeleteItemGroup
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [System.Web.Http.HttpDelete, System.Web.Http.Route("portal/api/itemgroup/{id}")]
        public IHttpActionResult DeleteItemGroup(string id)
        {
            ItemGroup itemgroup =  db.ItemGroups.Find(id);
            if (itemgroup  == null)
            {
                return NotFound();
            }

            db.ItemGroups.Remove(itemgroup);
            db.SaveChanges();

            return Ok(itemgroup);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ItemGroupExists(string id)
        {
            return db.ItemGroups.Count(e => e.Code == id) > 0;
        }
    }
}