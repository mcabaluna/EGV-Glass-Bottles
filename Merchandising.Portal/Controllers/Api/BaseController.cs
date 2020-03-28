using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;

namespace Merchandising.Portal.Controllers.Api
{
    public abstract class BaseController : ApiController
    {
        public new NegotiatedContentResult<T> Content<T>(HttpStatusCode statusCode, T value)
        {
            //try
            //{
            //    //if (statusCode != HttpStatusCode.OK)
            //        //Logger.Log(JsonConvert.SerializeObject(value), LoggingLevel.Error);
            //}
            //catch {; }

            if (value == null)
                return base.Content(statusCode, value);

            var str = JsonConvert.SerializeObject(value, Formatting.None, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            if (typeof(HttpException) == value.GetType())
            {
                var httpException = JsonConvert.DeserializeObject<HttpException>(str);
                return base.Content((HttpStatusCode)httpException.GetHttpCode(), value);
            }
            else if (typeof(AggregateException) == value.GetType())
            {
                return base.Content(HttpStatusCode.ServiceUnavailable, default(T));
            }

            var result = JsonConvert.DeserializeObject<T>(str);
            return base.Content(statusCode, result);

        }

        private Exception GetInnerException(Exception ex)
        {
            return ex.InnerException != null ? ex.InnerException : ex;
        }
    }
}
