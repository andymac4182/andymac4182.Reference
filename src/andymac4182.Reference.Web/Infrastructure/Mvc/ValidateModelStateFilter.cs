using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Filters;

namespace andymac4182.Reference.Web.Infrastructure.Mvc
{
    public class ValidateModelStateFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid) return;

            var modelStateErrors = context.ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.Exception?.Message ?? e.ErrorMessage)
                .Join(Environment.NewLine);

            throw new Exception($"Model binding failed: {Environment.NewLine}{modelStateErrors}");
        }
    }
}