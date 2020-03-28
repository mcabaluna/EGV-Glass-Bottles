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
    public class IncomingsController : ApiController
    { /// <summary>
      /// AddIncomings
      /// </summary>
      /// <returns></returns>
        [HttpPost, Route("api/incomings")]
        public virtual StatusCodeResponseVM AddIncomings([FromBody]Incomings entity)
        {
            try
            {
                var obj = MerchandisingApiWrapper.Post<Incomings>(typeof(Incomings).Name, entity);
                var result = Content(HttpStatusCode.OK, obj);
                StatusCodeResponseVM responseVM = new StatusCodeResponseVM()
                {
                    Code = HttpStatusCode.OK.ToString(),
                    HttpStatus = (int)HttpStatusCode.OK,
                    Message = "Successfully added incoming " + result.Content.PaymentNo + "."
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
        /// EditIncomings
        /// </summary>
        /// <returns></returns>
        [HttpPut, Route("api/incomings/{id}")]
        public virtual StatusCodeResponseVM EditIncomings(int id, [FromBody]Incomings request)
        {
            try
            {
                var obj = MerchandisingApiWrapper.Put<Incomings>(typeof(Incomings).Name + $"/{id}", request);
                var result = Content(HttpStatusCode.OK, obj);
                StatusCodeResponseVM responseVM = new StatusCodeResponseVM()
                {
                    Code = HttpStatusCode.OK.ToString(),
                    HttpStatus = (int)HttpStatusCode.OK,
                    Message = "Successfully updated incoming " + result.Content.PaymentNo + " !."
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
        /// CancelledIncomings
        /// </summary>
        /// <returns></returns>
        [HttpPut, Route("api/incomings/{id}")]
        public virtual StatusCodeResponseVM CancelledIncomings(int id)
        {
            try
            {
                var obj = MerchandisingApiWrapper.Delete<Incomings>(typeof(Incomings).Name + $"/{id}");
                var result = Content(HttpStatusCode.OK, obj);
                StatusCodeResponseVM responseVM = new StatusCodeResponseVM()
                {
                    Code = HttpStatusCode.OK.ToString(),
                    HttpStatus = (int)HttpStatusCode.OK,
                    Message = "Successfully cancelled incoming " + result.Content.PaymentNo + " !."
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
    