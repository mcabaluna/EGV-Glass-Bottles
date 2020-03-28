using Merchandising.DTO.Models;
using Merchandising.Portal.Models;
using Merchandising.VM.Portal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Merchandising.Portal.Controllers.Api
{
    public class SequenceDocumentController : ApiController
    {
        /// <summary>
        /// AddSequenceDocument
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("api/sequencedocument")]
        public virtual StatusCodeResponseVM AddSequenceDocument([FromBody]SequenceDocument entity)
        {
            try
            {
                var obj = MerchandisingApiWrapper.Post<SequenceDocument>(typeof(SequenceDocument).Name, entity);
                var result = Content(HttpStatusCode.OK, obj);
                StatusCodeResponseVM responseVM = new StatusCodeResponseVM()
                {
                    Code = HttpStatusCode.OK.ToString(),
                    HttpStatus = (int)HttpStatusCode.OK,
                    Message = "Successfully added series document " + result.Content.DocumentName + "."
                };
                return responseVM;
            }
            catch (Exception ex)
            {
                var error = Content(HttpStatusCode.InternalServerError, ex);
                StatusCodeResponseVM responseVM = new StatusCodeResponseVM()
                {
                    Code = error.StatusCode.ToString(),
                    HttpStatus = (int)error.StatusCode,
                    Detail = "",
                    Message = error.Content.Message
                };
                return responseVM;
            }
        }

        /// <summary>
        /// GetSequentDocumentInfo
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("api/sequencedocument")]
        public virtual List<SequenceDocument> GetSequentDocumentInfo()
        {
            try
            {
                var obj = MerchandisingApiWrapper.Get<List<SequenceDocument>>(
                typeof(SequenceDocument).Name + "/getsequencedocument");
                return obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// EditSequenceDocument
        /// </summary>
        /// <returns></returns>
        [HttpPut, Route("api/sequencedocument/{id}")]
        public virtual StatusCodeResponseVM EditSequenceDocument(string id, [FromBody]SequenceDocument request)
        {
            try
            {
                var obj = MerchandisingApiWrapper.Put<SequenceDocument>(typeof(SequenceDocument).Name + $"/{id}", request);
                var result = Content(HttpStatusCode.OK, obj);
                StatusCodeResponseVM responseVM = new StatusCodeResponseVM()
                {
                    Code = HttpStatusCode.OK.ToString(),
                    HttpStatus = (int)HttpStatusCode.OK,
                    Message = "Successfully updated series document " + result.Content.DocumentName + "."
                };
                return responseVM;
            }
            catch (Exception ex)
            {
                var error = Content(HttpStatusCode.InternalServerError, ex);
                StatusCodeResponseVM responseVM = new StatusCodeResponseVM()
                {
                    Code = error.StatusCode.ToString(),
                    HttpStatus = (int)error.StatusCode,
                    Detail = "",
                    Message = error.Content.Message
                };
                return responseVM;
            }
        }
        /// <summary>
        /// DeleteSequenceDocument
        /// </summary>
        /// <returns></returns>
        [HttpPut, Route("api/sequencedocument/{id}")]
        public virtual StatusCodeResponseVM DeleteSequenceDocument(string id)
        {
            try
            {
                var obj = MerchandisingApiWrapper.Delete<SequenceDocument>(typeof(SequenceDocument).Name + $"/{id}");
                var result = Content(HttpStatusCode.OK, obj);
                StatusCodeResponseVM responseVM = new StatusCodeResponseVM()
                {
                    Code = HttpStatusCode.OK.ToString(),
                    HttpStatus = (int)HttpStatusCode.OK,
                    Message = "Successfully deleted series document " + result.Content.DocumentName + "."
                };
                return responseVM;
            }
            catch (Exception ex)
            {
                var error = Content(HttpStatusCode.InternalServerError, ex);
                StatusCodeResponseVM responseVM = new StatusCodeResponseVM()
                {
                    Code = error.StatusCode.ToString(),
                    HttpStatus = (int)error.StatusCode,
                    Detail = "",
                    Message = error.Content.Message
                };
                return responseVM;
            }
        }
    }
}
