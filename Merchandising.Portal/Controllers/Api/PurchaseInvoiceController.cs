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
    public class PurchaseInvoiceController : ApiController
    {
        /// <summary>
        /// AddPurchaseInvoice
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("api/purchaseinvoice")]
        public virtual StatusCodeResponseVM AddPurchaseInvoice([FromBody]PurchaseInvoice entity)
        {
            try
            {
                var obj = MerchandisingApiWrapper.Post<PurchaseInvoice>(typeof(PurchaseInvoice).Name, entity);
                var result = Content(HttpStatusCode.OK, obj);
                StatusCodeResponseVM responseVM = new StatusCodeResponseVM()
                {
                    Code = HttpStatusCode.OK.ToString(),
                    HttpStatus = (int)HttpStatusCode.OK,
                    Message = "Successfully added purchase invoice " + result.Content.PInvoice + "."
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
        /// EditPurchaseInvoice
        /// </summary>
        /// <returns></returns>
        [HttpPut, Route("api/purchaseinvoice/{id}")]
        public virtual StatusCodeResponseVM EditPurchaseInvoice(int id, [FromBody]PurchaseInvoice request)
        {
            try
            {
                var obj = MerchandisingApiWrapper.Put<PurchaseInvoice>(typeof(PurchaseInvoice).Name + $"/{id}", request);
                var result = Content(HttpStatusCode.OK, obj);
                StatusCodeResponseVM responseVM = new StatusCodeResponseVM()
                {
                    Code = HttpStatusCode.OK.ToString(),
                    HttpStatus = (int)HttpStatusCode.OK,
                    Message = "Successfully updated purchase invoice " + result.Content.PInvoice + " !."
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
        ///// <summary>
        ///// DeletePurchaseInvoice
        ///// </summary>
        ///// <returns></returns>
        //[HttpPut, Route("api/PurchaseInvoice/{id}")]
        //public virtual StatusCodeResponseVM DeletePurchaseInvoice(string id)
        //{
        //    try
        //    {
        //        var obj = MerchandisingApiWrapper.Delete<PurchaseInvoice>(typeof(PurchaseInvoice).Name + $"/{id}");
        //        var result = Content(HttpStatusCode.OK, obj);
        //        StatusCodeResponseVM responseVM = new StatusCodeResponseVM()
        //        {
        //            Code = HttpStatusCode.OK.ToString(),
        //            HttpStatus = (int)HttpStatusCode.OK,
        //            Message = "Successfully deleted PurchaseInvoice " + result.Content.SInvoice + " !."
        //        };
        //        return responseVM;
        //    }
        //    catch (Exception ex)
        //    {
        //        var error = Content(HttpStatusCode.InternalServerError, ex);
        //        StatusCodeResponseVM responseVM = new StatusCodeResponseVM()
        //        {
        //            Code = error.StatusCode.ToString(),
        //            HttpStatus = (int)error.StatusCode,
        //            Detail = "",
        //            Message = error.Content.Message
        //        };
        //        return responseVM;
        //    }
        //}

        /// <summary>
        /// CancelledPurchaseInvoice
        /// </summary>
        /// <returns></returns>
        [HttpPut, Route("api/purchaseinvoice/{id}")]
        public virtual StatusCodeResponseVM CancelledPurchaseInvoice(int id)
        {
            try
            {
                var obj = MerchandisingApiWrapper.Delete<PurchaseInvoice>(typeof(PurchaseInvoice).Name + $"/{id}");
                var result = Content(HttpStatusCode.OK, obj);
                StatusCodeResponseVM responseVM = new StatusCodeResponseVM()
                {
                    Code = HttpStatusCode.OK.ToString(),
                    HttpStatus = (int)HttpStatusCode.OK,
                    Message = "Successfully cancelled purchase invoice " + result.Content.PInvoice + " !."
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
