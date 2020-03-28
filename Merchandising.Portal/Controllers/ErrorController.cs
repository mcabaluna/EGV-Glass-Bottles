using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Merchandising.Portal.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        /// <summary>
        /// NotFound
        /// </summary>
        /// <returns></returns>
        public ActionResult NotFound()
        {
            Response.StatusCode = 404;  //you may want to set this to 200
            return View("NotFound");
        }
        /// <summary>
        /// NotFoundError
        /// </summary>
        /// <returns></returns>
        public ActionResult NotFoundError()
        {
            Response.StatusCode = 404;  //you may want to set this to 200
            return View("NotFoundError");
        }
        /// <summary>
        /// Unauthorize
        /// </summary>
        /// <returns></returns>
        public ActionResult Unauthorize()
        {
            Response.StatusCode = 404;  //you may want to set this to 200
            return View("Unauthorize");
        }
    }
}