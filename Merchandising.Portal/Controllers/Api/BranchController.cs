using Merchandising.DTO.Models;
using Merchandising.Portal.Models;
using Merchandising.VM.Portal;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;

namespace Merchandising.Portal.Controllers.Api
{
    public class BranchController : GenericController<Branch>
    {
        /// <summary>
        /// GetBranches
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("api/branch")]
        public virtual IHttpActionResult GetBranches()
        {
            try
            {
                var obj = MerchandisingApiWrapper.Get<List<Branch>>(
                    typeof(Branch).Name);
                return Content(HttpStatusCode.OK, obj);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, ex);
            }
        }
        /// <summary>
        /// GetBranch
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("api/branch/{id}")]
        public virtual IHttpActionResult GetBranch(string id)
        {
            try
            {
                var obj = MerchandisingApiWrapper.Get<Branch>(
                    typeof(Branch).Name + $"/{id}");
                return Content(HttpStatusCode.OK, obj);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, ex);
            }
        }
        /// <summary>
        /// AddBranch
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("api/branch")]
        public virtual StatusCodeResponseVM AddBranch([FromBody]Branch entity)
        {
            try
            {
                var obj = MerchandisingApiWrapper.Post<Branch>(typeof(Branch).Name, entity);
                var result = Content(HttpStatusCode.OK, obj);
                StatusCodeResponseVM responseVM = new StatusCodeResponseVM()
                {
                    Code = HttpStatusCode.OK.ToString(),
                    HttpStatus = (int)HttpStatusCode.OK,
                    Message = "Successfully added branch " + result.Content.Name + "."
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
        /// EditBranch
        /// </summary>
        /// <returns></returns>
        [HttpPut, Route("api/branch/{id}")]
        public virtual StatusCodeResponseVM EditBranch(string id, [FromBody]Branch request)
        {
            try
            {
                var obj = MerchandisingApiWrapper.Put<Branch>(typeof(Branch).Name + $"/{id}", request);
                var result = Content(HttpStatusCode.OK, obj);
                StatusCodeResponseVM responseVM = new StatusCodeResponseVM()
                {
                    Code = HttpStatusCode.OK.ToString(),
                    HttpStatus = (int)HttpStatusCode.OK,
                    Message = "Successfully updated branch " + result.Content.Code + " !."
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
        /// DeletBranch
        /// </summary>
        /// <returns></returns>
        [HttpPut, Route("api/branch/{id}")]
        public virtual StatusCodeResponseVM DeleteBranch(string id)
        {
            try
            {
                var obj = MerchandisingApiWrapper.Delete<Branch>(typeof(Branch).Name + $"/{id}");
                var result = Content(HttpStatusCode.OK, obj);
                StatusCodeResponseVM responseVM = new StatusCodeResponseVM()
                {
                    Code = HttpStatusCode.OK.ToString(),
                    HttpStatus = (int)HttpStatusCode.OK,
                    Message = "Successfully deleted branch " + result.Content.Name + " !."
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
