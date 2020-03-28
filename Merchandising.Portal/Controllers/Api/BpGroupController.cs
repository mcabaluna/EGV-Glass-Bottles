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
    public class BpGroupController : ApiController
    {
        /// <summary>
        /// AddBpGroup
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("api/bpgroup")]
        public virtual StatusCodeResponseVM AddBpGroup([FromBody]BpGroup entity)
        {
            try
            {
                var obj = MerchandisingApiWrapper.Post<BpGroup>(typeof(BpGroup).Name, entity);
                var result = Content(HttpStatusCode.OK, obj);
                StatusCodeResponseVM responseVM = new StatusCodeResponseVM()
                {
                    Code = HttpStatusCode.OK.ToString(),
                    HttpStatus = (int)HttpStatusCode.OK,
                    Message = "Successfully added bpgroup " + result.Content.Name + "."
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
        /// EditBpGroup
        /// </summary>
        /// <returns></returns>
        [HttpPut, Route("api/bpgroup/{id}")]
        public virtual StatusCodeResponseVM EditBpGroup(string id, [FromBody]BpGroup request)
        {
            try
            {
                var obj = MerchandisingApiWrapper.Put<BpGroup>(typeof(BpGroup).Name + $"/{id}", request);
                var result = Content(HttpStatusCode.OK, obj);
                StatusCodeResponseVM responseVM = new StatusCodeResponseVM()
                {
                    Code = HttpStatusCode.OK.ToString(),
                    HttpStatus = (int)HttpStatusCode.OK,
                    Message = "Successfully updated bpgroup " + result.Content.Code + " !."
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
        /// DeleteBpGroup
        /// </summary>
        /// <returns></returns>
        [HttpPut, Route("api/bpgroup/{id}")]
        public virtual StatusCodeResponseVM DeleteBpGroup(string id)
        {
            try
            {
                var obj = MerchandisingApiWrapper.Delete<BpGroup>(typeof(BpGroup).Name + $"/{id}");
                var result = Content(HttpStatusCode.OK, obj);
                StatusCodeResponseVM responseVM = new StatusCodeResponseVM()
                {
                    Code = HttpStatusCode.OK.ToString(),
                    HttpStatus = (int)HttpStatusCode.OK,
                    Message = "Successfully deleted bpgroup " + result.Content.Name + " !."
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
