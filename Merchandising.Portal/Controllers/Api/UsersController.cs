using Merchandising.DTO.Models;
using Merchandising.Portal.Models;
using Merchandising.VM.Portal;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;

namespace Merchandising.Portal.Controllers.Api
{
    public class UsersController : GenericController<Users>
    {
        /// <summary>
        /// GetUserList
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpGet, Route("api/users/getuserlist")]
        public virtual IHttpActionResult GetUserList(string filter = "all", string search = null)
        {
            try
            {
                var obj = MerchandisingApiWrapper.Get<List<UserListVM>>(
                    typeof(Users).Name + "/getuserlist" + $"?filter={filter}&search={search}");
                return Content(HttpStatusCode.OK, obj);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// GetUserInfo
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("api/users/getuserinfo")]
        public virtual IHttpActionResult GetUserInfo()
        {
            try
            {
                var obj = MerchandisingApiWrapper.Get<List<UserVM>>(typeof(Users).Name + "/getuserinfo");
                return Content(HttpStatusCode.OK, obj);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// AddUsers
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("api/users")]
        public virtual StatusCodeResponseVM AddUser([FromBody]Users entity)
        {
            try
            {
                var obj = MerchandisingApiWrapper.Post<Users>(typeof(Users).Name, entity);
                var result = Content(HttpStatusCode.OK, obj);
                StatusCodeResponseVM responseVM = new StatusCodeResponseVM()
                {
                    Code = HttpStatusCode.OK.ToString(),
                    HttpStatus = (int)HttpStatusCode.OK,
                    Message = "Successfully added user " + result.Content.UserName + "."
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
        /// AddUsers
        /// </summary>
        /// <returns></returns>
        [HttpPut, Route("api/users/{id}")]
        public virtual StatusCodeResponseVM EditUser(string id, [FromBody]Users request)
        {
            try
            {
                var obj = MerchandisingApiWrapper.Put<Users>(typeof(Users).Name + $"/{id}", request);
                var result = Content(HttpStatusCode.OK, obj);
                StatusCodeResponseVM responseVM = new StatusCodeResponseVM()
                {
                    Code = HttpStatusCode.OK.ToString(),
                    HttpStatus = (int)HttpStatusCode.OK,
                    Message = "Successfully updated user " + result.Content.UserName + " !."
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
        /// AuthenticateUser
        /// </summary>
        /// <returns></returns>
        [HttpPut, Route("api/users//authenticateuser/{id}")]
        public virtual StatusCodeResponseVM AuthenticateUser(string id, [FromBody]Users request)
        {
            try
            {
                var obj = MerchandisingApiWrapper.Put<Users>(typeof(Users).Name + $"/{id}", request);
                var result = Content(HttpStatusCode.OK, obj);
                StatusCodeResponseVM responseVM = new StatusCodeResponseVM()
                {
                    Code = HttpStatusCode.OK.ToString(),
                    HttpStatus = (int)HttpStatusCode.OK,
                    Message = "Successfully updated user " + result.Content.UserName + " !."
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
        /// DeleteUser
        /// </summary>
        /// <returns></returns>
        [HttpPut, Route("api/users/{id}")]
        public virtual StatusCodeResponseVM DeleteUser(string id)
        {
            try
            {
                var obj = MerchandisingApiWrapper.Delete<Users>(typeof(Users).Name + $"/{id}");
                var result = Content(HttpStatusCode.OK, obj);
                StatusCodeResponseVM responseVM = new StatusCodeResponseVM()
                {
                    Code = HttpStatusCode.OK.ToString(),
                    HttpStatus = (int)HttpStatusCode.OK,
                    Message = "Successfully deleted user " + result.Content.UserName + " !."
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
