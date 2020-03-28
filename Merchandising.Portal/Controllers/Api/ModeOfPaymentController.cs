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
    public class ModeOfPaymentController : ApiController
    { 
        /// <summary>
      /// AddModeOfPayment
      /// </summary>
      /// <returns></returns>
        [HttpPost, Route("api/modeofpayment")]
        public virtual StatusCodeResponseVM AddModeOfPayment([FromBody]ModeOfPayment entity)
        {
            try
            {
                var obj = MerchandisingApiWrapper.Post<ModeOfPayment>(typeof(ModeOfPayment).Name, entity);
                var result = Content(HttpStatusCode.OK, obj);
                StatusCodeResponseVM responseVM = new StatusCodeResponseVM()
                {
                    Code = HttpStatusCode.OK.ToString(),
                    HttpStatus = (int)HttpStatusCode.OK,
                    Message = "Successfully added mode of payment " + result.Content.Name + "."
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
        /// EditModeOfPayment
        /// </summary>
        /// <returns></returns>
        [HttpPut, Route("api/modeofpayment/{id}")]
        public virtual StatusCodeResponseVM EditModeOfPayment(string id, [FromBody]ModeOfPayment request)
        {
            try
            {
                var obj = MerchandisingApiWrapper.Put<ModeOfPayment>(typeof(ModeOfPayment).Name + $"/{id}", request);
                var result = Content(HttpStatusCode.OK, obj);
                StatusCodeResponseVM responseVM = new StatusCodeResponseVM()
                {
                    Code = HttpStatusCode.OK.ToString(),
                    HttpStatus = (int)HttpStatusCode.OK,
                    Message = "Successfully updated mode of payment " + result.Content.Code + " !."
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
        /// DeletModeOfPayment
        /// </summary>
        /// <returns></returns>
        [HttpPut, Route("api/modeofpayment/{id}")]
        public virtual StatusCodeResponseVM DeleteModeOfPayment(string id)
        {
            try
            {
                var obj = MerchandisingApiWrapper.Delete<ModeOfPayment>(typeof(ModeOfPayment).Name + $"/{id}");
                var result = Content(HttpStatusCode.OK, obj);
                StatusCodeResponseVM responseVM = new StatusCodeResponseVM()
                {
                    Code = HttpStatusCode.OK.ToString(),
                    HttpStatus = (int)HttpStatusCode.OK,
                    Message = "Successfully deleted mode of payment " + result.Content.Name + " !."
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
