using Merchandising.DTO.Models;
using Merchandising.Portal.Models;
using Merchandising.VM.Portal;
using System;
using System.Net;
using System.Web.Http;

namespace Merchandising.Portal.Controllers.Api
{
    public class SequenceTableController : ApiController
    {
        /// <summary>
        /// AddSequenceTable
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("api/sequencetable")]
        public virtual StatusCodeResponseVM AddSequenceTable([FromBody]SequenceTable entity)
        {
            try
            {
                var obj = MerchandisingApiWrapper.Post<SequenceTable>(typeof(SequenceTable).Name, entity);
                var result = Content(HttpStatusCode.OK, obj);
                StatusCodeResponseVM responseVM = new StatusCodeResponseVM()
                {
                    Code = HttpStatusCode.OK.ToString(),
                    HttpStatus = (int)HttpStatusCode.OK,
                    Message = "Successfully added document numbering!."
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
        /// EditSequenceTable
        /// </summary>
        /// <returns></returns>
        [HttpPut, Route("api/sequencetable/{id}")]
        public virtual StatusCodeResponseVM EditSequenceTable(int id, [FromBody]SequenceTable request)
        {
            try
            {
                var obj = MerchandisingApiWrapper.Put<SequenceTable>(typeof(SequenceTable).Name + $"/{id}", request);
                var result = Content(HttpStatusCode.OK, obj);
                StatusCodeResponseVM responseVM = new StatusCodeResponseVM()
                {
                    Code = HttpStatusCode.OK.ToString(),
                    HttpStatus = (int)HttpStatusCode.OK,
                    Message = "Successfully updated document numbering!."
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
        /// DeleteSequenceTable
        /// </summary>
        /// <returns></returns>
        [HttpPut, Route("api/sequencetable/{id}")]
        public virtual StatusCodeResponseVM DeleteSequenceTable(int id)
        {
            try
            {
                var obj = MerchandisingApiWrapper.Delete<SequenceTable>(typeof(SequenceTable).Name + $"/{id}");
                var result = Content(HttpStatusCode.OK, obj);
                StatusCodeResponseVM responseVM = new StatusCodeResponseVM()
                {
                    Code = HttpStatusCode.OK.ToString(),
                    HttpStatus = (int)HttpStatusCode.OK,
                    Message = "Successfully deleted document numbering!."
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
