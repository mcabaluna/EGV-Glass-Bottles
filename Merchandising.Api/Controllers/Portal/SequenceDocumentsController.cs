using Merchandising.DTO;
using Merchandising.DTO.Models;
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
    public class SequenceDocumentsController : ApiController
    {
        private DbContextModel db = new DbContextModel();


        /// <summary>
        /// GetSequentInfo
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("portal/api/sequencedocument/getsequencedocument")]
        public IHttpActionResult GetSequentDocumentInfo()
        {
            var seqdocument = db.SequenceDocument.ToList();
            return Ok(seqdocument);
        }

        /// <summary>
        /// GetSequenceDocument
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.Route("portal/api/sequencedocument/{id}")]
        public IHttpActionResult GetSequenceDocument(int id)
        {
            SequenceDocument seqdocument =  db.SequenceDocument.Find(id);
            if (seqdocument == null)
            {
                return NotFound();
            }

            return Ok(seqdocument);
        }

        /// <summary>
        /// EditSequenceDocument
        /// </summary>
        /// <param name="id"></param>
        /// <param name="seqdocument"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPut, System.Web.Http.Route("portal/api/sequencedocument/{id}")]
        public IHttpActionResult EditSequenceDocument(int id, [FromBody]SequenceDocument seqdocument)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //if (id != seqdocument.Document)
            //{
            //    return BadRequest();
            //}

            db.Entry(seqdocument).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SequenceDocumentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(seqdocument);
        }

        /// <summary>
        /// AddSequenceDocument
        /// </summary>
        /// <param name="seqdocument"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost, System.Web.Http.Route("portal/api/sequencedocument")]
        public IHttpActionResult AddSequenceDocument([FromBody]SequenceDocument seqdocument)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                //check sequence document if exists
                //var check = db.SequenceDocument.Where(x => x.Document == seqdocument.Document).Any();
                //if (check)
                //{
                //    return BadRequest("Sequence Document already exists! Please create different sequence document.");
                //}
                var identity = (ClaimsIdentity)User.Identity;
                seqdocument.CreatedById = identity.Name;
                db.SequenceDocument.Add(seqdocument);
                db.SaveChanges();
                return Ok(seqdocument);
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
        /// DeleteSequenceDocument
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [System.Web.Http.HttpDelete, System.Web.Http.Route("portal/api/sequencedocument/{id}")]
        public IHttpActionResult DeleteSequenceDocument(string id)
        {
            SequenceTable sequence = db.SequenceTables.Find(id);
            if (sequence == null)
            {
                return NotFound();
            }

            db.SequenceTables.Remove(sequence);
            db.SaveChanges();

            return Ok(sequence);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SequenceDocumentExists(int id)
        {
            return db.SequenceDocument.Count(e => e.ObjectCode == id) > 0;
        }
    }
}
