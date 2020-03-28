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
    public class PaymentTermsController : ApiController
    {
        /// <summary>
        /// AddPaymentTerms
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("api/paymentterms")]
        public virtual StatusCodeResponseVM AddPaymentTerms([FromBody]PaymentTerms entity)
        {
            try
            {
                var obj = MerchandisingApiWrapper.Post<PaymentTerms>(typeof(PaymentTerms).Name, entity);
                var result = Content(HttpStatusCode.OK, obj);
                StatusCodeResponseVM responseVM = new StatusCodeResponseVM()
                {
                    Code = HttpStatusCode.OK.ToString(),
                    HttpStatus = (int)HttpStatusCode.OK,
                    Message = "Successfully added payment term " + result.Content.Name + "."
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
        /// EditPaymentTerms
        /// </summary>
        /// <returns></returns>
        [HttpPut, Route("api/paymentterms/{id}")]
        public virtual StatusCodeResponseVM EditPaymentTerms(string id, [FromBody]PaymentTerms request)
        {
            try
            {
                var obj = MerchandisingApiWrapper.Put<PaymentTerms>(typeof(PaymentTerms).Name + $"/{id}", request);
                var result = Content(HttpStatusCode.OK, obj);
                StatusCodeResponseVM responseVM = new StatusCodeResponseVM()
                {
                    Code = HttpStatusCode.OK.ToString(),
                    HttpStatus = (int)HttpStatusCode.OK,
                    Message = "Successfully updated payment term " + result.Content.TermId + " !."
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
        /// DeletePaymentTerms
        /// </summary>
        /// <returns></returns>
        [HttpPut, Route("api/paymentterms/{id}")]
        public virtual StatusCodeResponseVM DeletePaymentTerms(string id)
        {
            try
            {
                var obj = MerchandisingApiWrapper.Delete<PaymentTerms>(typeof(PaymentTerms).Name + $"/{id}");
                var result = Content(HttpStatusCode.OK, obj);
                StatusCodeResponseVM responseVM = new StatusCodeResponseVM()
                {
                    Code = HttpStatusCode.OK.ToString(),
                    HttpStatus = (int)HttpStatusCode.OK,
                    Message = "Successfully deleted payment term " + result.Content.Name + " !."
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
