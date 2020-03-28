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
    public class ItemsController : ApiController
    {
        /// <summary>
        /// AddItems
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("api/items")]
        public virtual StatusCodeResponseVM AddItems([FromBody]Items entity)
        {
            try
            {
                var obj = MerchandisingApiWrapper.Post<Items>(typeof(Items).Name, entity);
                var result = Content(HttpStatusCode.OK, obj);
                StatusCodeResponseVM responseVM = new StatusCodeResponseVM()
                {
                    Code = HttpStatusCode.OK.ToString(),
                    HttpStatus = (int)HttpStatusCode.OK,
                    Message = "Successfully added item " + result.Content.ItemName + "."
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
        /// EditItems
        /// </summary>
        /// <returns></returns>
        [HttpPut, Route("api/items/{id}")]
        public virtual StatusCodeResponseVM EditItems(string id, [FromBody]Items request)
        {
            try
            {
                var obj = MerchandisingApiWrapper.Put<Items>(typeof(Items).Name + $"/{id}", request);
                var result = Content(HttpStatusCode.OK, obj);
                StatusCodeResponseVM responseVM = new StatusCodeResponseVM()
                {
                    Code = HttpStatusCode.OK.ToString(),
                    HttpStatus = (int)HttpStatusCode.OK,
                    Message = "Successfully updated item " + result.Content.ItemName + " !."
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
        /// DeleteItems
        /// </summary>
        /// <returns></returns>
        [HttpPut, Route("api/items/{id}")]
        public virtual StatusCodeResponseVM DeleteItems(string id)
        {
            try
            {
                var obj = MerchandisingApiWrapper.Delete<Items>(typeof(Items).Name + $"/{id}");
                var result = Content(HttpStatusCode.OK, obj);
                StatusCodeResponseVM responseVM = new StatusCodeResponseVM()
                {
                    Code = HttpStatusCode.OK.ToString(),
                    HttpStatus = (int)HttpStatusCode.OK,
                    Message = "Successfully deleted item " + result.Content.ItemName+ " !."
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
