using Merchandising.DTO;
using Merchandising.DTO.Models;
using Merchandising.VM.Portal;
using Merchandising.VM.Results;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;

namespace Merchandising.Api.Controllers.Portal
{
    public class ItemsController : ApiController
    {
        private DbContextModel db = new DbContextModel();

        /// <summary>
        /// GetItemList
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.Route("portal/api/items/getitemlist")]
        public IHttpActionResult GetItemList(string search = null)
        {
            var items = new List<Items>();
            List<ItemsListVM> list = new List<ItemsListVM>();
            //get all roles with filter 
            items = db.Items
                         .OrderByDescending(x => x.CreatedOn)
                         .ToList();
            //
            if (items.Count > 0)
            {
                list = items.Select(x => new ItemsListVM()
                {
                    ItemCode = x.ItemCode,
                    ItemName = x.ItemName,
                    GroupName = db.ItemGroups.FirstOrDefault(b => b.Code == x.GroupCode)?.Name,
                    WTaxName = db.WTaxs.FirstOrDefault(b => b.Code == x.WtaxId)?.Name,
                    Status = x.Status
                }).ToList();

                //Search fields
                if (!string.IsNullOrEmpty(search))
                {
                    list = list.Where(x =>
                            x.ItemCode.ToLower().Contains(search.ToLower()) ||
                            x.ItemName.ToString().ToLower().Contains(search.ToLower()) ||
                            x.GroupName.ToString().ToLower().Contains(search.ToLower()) ||
                            x.WTaxName.ToString().ToLower().Contains(search.ToLower()))
                        .OrderByDescending(x => x.ItemCode)
                        .ToList();
                }
            }
            return Ok(list);
        }
        /// <summary>
        /// GetItems
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("portal/api/items/getitemsinfo")]
        public IHttpActionResult GetItems()
        {
            var items = db.Items.Include(b => b.ItemUoM).Include(b => b.ItemOnHandPerWhse).Where(x => x.Status.Equals(true)).ToList();
            return Ok(items);
        }
        /// <summary>
        /// GetItems
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.Route("portal/api/items/{id}")]
        public IHttpActionResult GetItems(string id)
        {
            try
            {
                //Items items = await db.Items.FindAsync(id);
                //Items items = db.Items.Include(x => x.ItemUoM).SingleOrDefault(x => x.ItemCode == id);
                Items items = db.Items.Include(x => x.ItemUoM).Include(b => b.ItemOnHandPerWhse).SingleOrDefault(x => x.ItemCode == id);
                if (items == null)
                {
                    return NotFound();
                }

                return Ok(items);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// CheckItemPrice
        /// </summary>
        /// <param name="pricelistid"></param>
        /// <param name="itemcode"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.Route("portal/api/items/checkitemprice")]
        public IHttpActionResult CheckItemPrice(string pricelistid, string itemcode)
        {
            try
            {
                var result = (from n in db.PricelistLines.Where(x => x.PricelistId == pricelistid && x.ItemId == itemcode).ToList()
                              from b in db.Items.Where(x => x.ItemCode == itemcode).DefaultIfEmpty()
                              select new PricelistItem_Results()
                              {
                                  PricelistId = n.PricelistId,
                                  ItemCode = n.ItemId,
                                  ItemName = n.ItemName,
                                  WholeSaleQty = b.WholeSaleQty,
                                  RetailPrice = n.RetailPrice,
                                  WholeSalePrice = n.WholesalePrice
                              }).DefaultIfEmpty().FirstOrDefault();
                             

                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// EditItems
        /// </summary>
        /// <param name="id"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPut, System.Web.Http.Route("portal/api/items/{id}")]
        public IHttpActionResult EditItems(string id, [FromBody]Items items)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != items.ItemCode)
            {
                return BadRequest();
            }
            //Uom
            var uomlist = db.ItemUoM.Where(p => p.ItemCode == items.ItemCode);
            if (items.ItemUoM != null)
            {
                foreach (ItemUoM uom in uomlist)
                {
                    db.ItemUoM.Remove(uom);
                }
                foreach (ItemUoM uom in items.ItemUoM)
                {
                    db.ItemUoM.Add(uom);
                    db.SaveChanges();
                }
            }

            //itemPerWarehouse
            //var whselist = db.ItemOnHandPerWhse.Where(p => p.ItemCode == items.ItemCode);
            //if (items.ItemOnHandPerWhse != null)
            //{
            //    foreach (ItemOnHandPerWhse whse in whselist)
            //    {
            //        db.ItemOnHandPerWhse.Remove(whse);
            //    }
            //    foreach (ItemOnHandPerWhse whs in items.ItemOnHandPerWhse)
            //    {
            //        db.ItemOnHandPerWhse.Add(whs);
            //        db.SaveChanges();
            //    }
            //}
            db.Entry(items).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(items);
        }
        /// <summary>
        /// AddItems
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost, System.Web.Http.Route("portal/api/items")]
        public IHttpActionResult AddItems(Items items)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                //check items if exists
                var check = db.Items.Where(x => x.ItemCode.ToUpper().Trim() == items.ItemCode.ToUpper().Trim()).Any();
                if (check)
                {
                    return BadRequest("Item already exists! Please create different item.");
                }
                var identity = (ClaimsIdentity)User.Identity;
                items.CreatedById = identity.Name;

                //Validate Sequence
                //var numbering = (from i in db.SequenceTables.Where(o => o.Document == "OITM" && o.SeriesName == items.Series)
                //                 select new { i.SeriesName, i.Prefix, i.NextNumber, i.Suffix }).ToList();

                //items.ItemCode = numbering.FirstOrDefault().Suffix != null ?
                //                numbering.FirstOrDefault().Prefix + numbering.FirstOrDefault().NextNumber + "_" + numbering.FirstOrDefault().Suffix :
                //                numbering.FirstOrDefault().Prefix + numbering.FirstOrDefault().NextNumber;
                db.Items.Add(items);
                db.SaveChanges();

                //Update Sequence Table
                //string series = numbering.FirstOrDefault().SeriesName;
                //SequenceTable seq = db.SequenceTables.FirstOrDefault(i => i.SeriesName == series && i.Document == "OITM");
                ////seq.NextNumber = numbering.FirstOrDefault().NextNumber + 1;
                //seq.NextNumber = string.Format("{0:00}", (int.Parse(numbering.FirstOrDefault().NextNumber) + 1));
                //seq.LastNumber = numbering.FirstOrDefault().NextNumber;
                //Update Sequence Table
                var series = Convert.ToInt32(items.Series);
                var numbering = db.SequenceTableLines.SingleOrDefault(x => x.Series == series);
                if (numbering != null)
                {
                    numbering.LastNum = numbering.NextNumber;
                    numbering.NextNumber = numbering.NextNumber + 1;
                    db.SaveChanges();
                }
                //>>end
                //>>end

                return Ok(items);
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
        /// DeleteItems
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [System.Web.Http.HttpDelete, System.Web.Http.Route("portal/api/items/{id}")]
        public IHttpActionResult DeleteItems(string id)
        {
            Items items = db.Items.Find(id);
            if (items == null)
            {
                return NotFound();
            }

            db.Items.Remove(items);
            db.SaveChanges();

            return Ok(items);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        private bool ItemsExists(string id)
        {
            return db.Items.Count(e => e.ItemCode == id) > 0;
        }
    }
}