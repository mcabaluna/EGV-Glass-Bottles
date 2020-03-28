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
    public class InvAdjustmentController : ApiController
    {
        /// <summary>
        /// AddInvAdjustment
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("api/invadjustment")]
        public virtual StatusCodeResponseVM AddInvAdjustment([FromBody]InvAdjustment entity)
        {
            try
            {
                var obj = MerchandisingApiWrapper.Post<InvAdjustment>(typeof(InvAdjustment).Name, entity);
                var result = Content(HttpStatusCode.OK, obj);
                StatusCodeResponseVM responseVM = new StatusCodeResponseVM()
                {
                    Code = HttpStatusCode.OK.ToString(),
                    HttpStatus = (int)HttpStatusCode.OK,
                    Message = "Successfully added invadjustment " + result.Content.InvAdjustmentNo + "."
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
        /// EditInvAdjustment
        /// </summary>
        /// <returns></returns>
        [HttpPut, Route("api/invadjustment/{id}")]
        public virtual StatusCodeResponseVM EditInvAdjustment(int id, [FromBody]InvAdjustment request)
        {
            try
            {
                var obj = MerchandisingApiWrapper.Put<InvAdjustment>(typeof(InvAdjustment).Name + $"/{id}", request);
                var result = Content(HttpStatusCode.OK, obj);
                StatusCodeResponseVM responseVM = new StatusCodeResponseVM()
                {
                    Code = HttpStatusCode.OK.ToString(),
                    HttpStatus = (int)HttpStatusCode.OK,
                    Message = "Successfully updated invadjustment " + result.Content.InvAdjustmentNo + " !."
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
        /// CancelledInvAdjustment
        /// </summary>
        /// <returns></returns>
        [HttpPut, Route("api/invadjustment/{id}")]
        public virtual StatusCodeResponseVM CancelledInvAdjustment(int id)
        {
            try
            {
                var obj = MerchandisingApiWrapper.Delete<InvAdjustment>(typeof(InvAdjustment).Name + $"/{id}");
                var result = Content(HttpStatusCode.OK, obj);
                StatusCodeResponseVM responseVM = new StatusCodeResponseVM()
                {
                    Code = HttpStatusCode.OK.ToString(),
                    HttpStatus = (int)HttpStatusCode.OK,
                    Message = "Successfully cancelled invadjustment " + result.Content.InvAdjustmentNo + " !."
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
