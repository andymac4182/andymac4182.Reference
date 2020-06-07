using System;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace andymac4182.Reference.Web.Features.Error
{
    public class Error : Controller
    {
        [AllowAnonymous]
        [Route("[controller]/{statusCode:int?}")]
        public IActionResult Index(int? statusCode = (int) HttpStatusCode.InternalServerError)
        {
            var httpStatusCode = statusCode != null && Enum.IsDefined(typeof(HttpStatusCode), statusCode)
                ? (HttpStatusCode)statusCode
                : HttpStatusCode.InternalServerError;

            HttpContext.Response.StatusCode = (int)httpStatusCode;
            return View(httpStatusCode);
        }
    }
}