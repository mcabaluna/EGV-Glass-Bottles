using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Merchandising.DTO;
using Merchandising.DTO.Models;

namespace Merchandising.Api.Controllers.Portal
{
    public class ProvincesController : ApiController
    {
        private DbContextModel db = new DbContextModel();

        /// <summary>
        /// GetProvinces
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("portal/api/provinces/getprovincesinfo")]
        public IHttpActionResult GetProvinces()
        {
            var provinces =  db.Provinces.ToList();
            return Ok(provinces);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}