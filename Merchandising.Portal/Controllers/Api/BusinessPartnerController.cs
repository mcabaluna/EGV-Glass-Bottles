using Merchandising.DTO.Models;
using Merchandising.Portal.Models;
using Merchandising.VM.Portal;
using System;
using System.Net;
using System.Web.Http;

namespace Merchandising.Portal.Controllers.Api
{
    public class BusinessPartnerController : ApiController
    {
        /// <summary>
        /// AddBusinessPartner
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("api/businesspartner")]
        public virtual StatusCodeResponseVM AddBusinessPartner([FromBody]BusinessPartner entity)
        {
            try
            {
                var obj = MerchandisingApiWrapper.Post<BusinessPartner>(typeof(BusinessPartner).Name, entity);
                var result = Content(HttpStatusCode.OK, obj);
                StatusCodeResponseVM responseVM = new StatusCodeResponseVM()
                {
                    Code = HttpStatusCode.OK.ToString(),
                    HttpStatus = (int)HttpStatusCode.OK,
                    Message = "Successfully added businesspartner " + result.Content.CardName + "."
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
        /// EditBusinessPartner
        /// </summary>
        /// <returns></returns>
        [HttpPut, Route("api/businesspartner/{id}")]
        public virtual StatusCodeResponseVM EditBusinessPartner(string id, [FromBody]BusinessPartner request)
        {
            try
            {
                var obj = MerchandisingApiWrapper.Put<BusinessPartner>(typeof(BusinessPartner).Name + $"/{id}", request);
                var result = Content(HttpStatusCode.OK, obj);
                StatusCodeResponseVM responseVM = new StatusCodeResponseVM()
                {
                    Code = HttpStatusCode.OK.ToString(),
                    HttpStatus = (int)HttpStatusCode.OK,
                    Message = "Successfully updated businesspartner " + result.Content.CardName + " !."
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
        /// DeleteBusinessPartner
        /// </summary>
        /// <returns></returns>
        [HttpPut, Route("api/businesspartner/{id}")]
        public virtual StatusCodeResponseVM DeleteBusinessPartner(string id)
        {
            try
            {
                var obj = MerchandisingApiWrapper.Delete<BusinessPartner>(typeof(BusinessPartner).Name + $"/{id}");
                var result = Content(HttpStatusCode.OK, obj);
                StatusCodeResponseVM responseVM = new StatusCodeResponseVM()
                {
                    Code = HttpStatusCode.OK.ToString(),
                    HttpStatus = (int)HttpStatusCode.OK,
                    Message = "Successfully deleted businesspartner " + result.Content.CardName + " !."
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
