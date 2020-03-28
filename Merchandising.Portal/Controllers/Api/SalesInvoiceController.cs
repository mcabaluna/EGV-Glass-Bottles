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
    public class SalesInvoiceController : ApiController
    {
        /// <summary>
        /// AddSalesInvoice
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("api/salesinvoice")]
        public virtual StatusCodeResponseVM AddSalesInvoice([FromBody]SalesInvoice entity)
        {
            try
            {
                var obj = MerchandisingApiWrapper.Post<SalesInvoice>(typeof(SalesInvoice).Name, entity);
                var result = Content(HttpStatusCode.OK, obj);
                StatusCodeResponseVM responseVM = new StatusCodeResponseVM()
                {
                    Code = HttpStatusCode.OK.ToString(),
                    HttpStatus = (int)HttpStatusCode.OK,
                    Message = "Successfully added salesinvoice " + result.Content.SInvoice + "."
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
        /// EditSalesInvoice
        /// </summary>
        /// <returns></returns>
        [HttpPut, Route("api/salesinvoice/{id}")]
        public virtual StatusCodeResponseVM EditSalesInvoice(int id, [FromBody]SalesInvoice request)
        {
            try
            {
                var obj = MerchandisingApiWrapper.Put<SalesInvoice>(typeof(SalesInvoice).Name + $"/{id}", request);
                var result = Content(HttpStatusCode.OK, obj);
                StatusCodeResponseVM responseVM = new StatusCodeResponseVM()
                {
                    Code = HttpStatusCode.OK.ToString(),
                    HttpStatus = (int)HttpStatusCode.OK,
                    Message = "Successfully updated salesinvoice " + result.Content.SInvoice + " !."
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
        ///// DeleteSalesInvoice
        ///// </summary>
        ///// <returns></returns>
        //[HttpPut, Route("api/salesinvoice/{id}")]
        //public virtual StatusCodeResponseVM DeleteSalesInvoice(string id)
        //{
        //    try
        //    {
        //        var obj = MerchandisingApiWrapper.Delete<SalesInvoice>(typeof(SalesInvoice).Name + $"/{id}");
        //        var result = Content(HttpStatusCode.OK, obj);
        //        StatusCodeResponseVM responseVM = new StatusCodeResponseVM()
        //        {
        //            Code = HttpStatusCode.OK.ToString(),
        //            HttpStatus = (int)HttpStatusCode.OK,
        //            Message = "Successfully deleted salesinvoice " + result.Content.SInvoice + " !."
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
        /// CancelledSalesInvoice
        /// </summary>
        /// <returns></returns>
        [HttpPut, Route("api/salesinvoice/{id}")]
        public virtual StatusCodeResponseVM CancelledSalesInvoice(int id)
        {
            try
            {
                var obj = MerchandisingApiWrapper.Delete<SalesInvoice>(typeof(SalesInvoice).Name + $"/{id}");
                var result = Content(HttpStatusCode.OK, obj);
                StatusCodeResponseVM responseVM = new StatusCodeResponseVM()
                {
                    Code = HttpStatusCode.OK.ToString(),
                    HttpStatus = (int)HttpStatusCode.OK,
                    Message = "Successfully cancelled salesinvoice " + result.Content.SInvoice + " !."
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
