using Merchandising.Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Merchandising.Portal.Controllers.Api
{
    public abstract class GenericController<BO> : ApiController
    {
        public virtual IHttpActionResult Get(long id)
        {
            try
            {
                var obj = MerchandisingApiWrapper.Get<BO>(typeof(BO).Name.RemoveLastChars(2) + "?id=" + id.ToString());

                return Content(HttpStatusCode.OK, obj);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, ex);
            }
        }

        public virtual IHttpActionResult Get()
        {
            try
            {
                var obj = MerchandisingApiWrapper.Get<ICollection<BO>>(typeof(BO).Name.RemoveLastChars(2) + "");

                return Content(HttpStatusCode.OK, obj);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, ex);
            }
        }

        public virtual IHttpActionResult Put(long id, [FromBody] BO entity)
        {
            try
            {
                var obj = MerchandisingApiWrapper.Put<BO>(typeof(BO).Name.RemoveLastChars(2) + "?id=" + id.ToString(), entity);

                return Content(HttpStatusCode.OK, obj);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, ex);
            }
        }

        public virtual IHttpActionResult Post([FromBody] BO entity)
        {
            try
            {
                var obj = MerchandisingApiWrapper.Post<BO>(typeof(BO).Name.RemoveLastChars(2), entity);

                return Content(HttpStatusCode.OK, obj);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, ex);
            }
        }

        public virtual IHttpActionResult Delete(long id)
        {
            try
            {
                var obj = MerchandisingApiWrapper.Delete<bool>(typeof(BO).Name.RemoveLastChars(2) + "?id=" + id.ToString());

                return Content(HttpStatusCode.OK, obj);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, ex);
            }
        }
    }
}
