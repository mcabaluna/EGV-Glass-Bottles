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
    public class WarehouseController : ApiController
    {
        /// <summary>
        /// AddWarehouse
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("api/warehouse")]
        public virtual StatusCodeResponseVM AddWarehouse([FromBody]Warehouse entity)
        {
            try
            {
                var obj = MerchandisingApiWrapper.Post<Warehouse>(typeof(Warehouse).Name, entity);
                var result = Content(HttpStatusCode.OK, obj);
                StatusCodeResponseVM responseVM = new StatusCodeResponseVM()
                {
                    Code = HttpStatusCode.OK.ToString(),
                    HttpStatus = (int)HttpStatusCode.OK,
                    Message = "Successfully added warehouse " + result.Content.Name + "."
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
        /// EditWarehouse
        /// </summary>
        /// <returns></returns>
        [HttpPut, Route("api/warehouse/{id}")]
        public virtual StatusCodeResponseVM EditWarehouse(string id, [FromBody]Warehouse request)
        {
            try
            {
                var obj = MerchandisingApiWrapper.Put<Warehouse>(typeof(Warehouse).Name + $"/{id}", request);
                var result = Content(HttpStatusCode.OK, obj);
                StatusCodeResponseVM responseVM = new StatusCodeResponseVM()
                {
                    Code = HttpStatusCode.OK.ToString(),
                    HttpStatus = (int)HttpStatusCode.OK,
                    Message = "Successfully updated warehouse " + result.Content.Code + " !."
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
        /// DeleteWarehouse
        /// </summary>
        /// <returns></returns>
        [HttpDelete, Route("api/warehouse/{id}")]
        public virtual StatusCodeResponseVM DeleteWarehouse(string id)
        {
            try
            {
                var obj = MerchandisingApiWrapper.Delete<Warehouse>(typeof(Warehouse).Name + $"/{id}");
                var result = Content(HttpStatusCode.OK, obj);
                StatusCodeResponseVM responseVM = new StatusCodeResponseVM()
                {
                    Code = HttpStatusCode.OK.ToString(),
                    HttpStatus = (int)HttpStatusCode.OK,
                    Message = "Successfully deleted warehouse " + result.Content.Name + " !."
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
