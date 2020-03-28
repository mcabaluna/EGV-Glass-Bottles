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
    public class CitiesController : ApiController
    {
        private DbContextModel db = new DbContextModel();

        /// <summary>
        /// GetCities
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("portal/api/cities/getcitiesinfo")]
        public IHttpActionResult GetCities()
        {
            var cities =  db.Cities.ToList();
            return Ok(cities);
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