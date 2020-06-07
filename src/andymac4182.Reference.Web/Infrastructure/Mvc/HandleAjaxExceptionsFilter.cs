using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using andymac4182.Reference.Infrastructure.Logging;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

namespace andymac4182.Reference.Web.Infrastructure.Mvc
{
    public class HandleAjaxExceptionsFilter : ExceptionFilterAttribute
    {
        private readonly EnvironmentTypeSetting _environmentType;
        private readonly ILogger _logger;

        public HandleAjaxExceptionsFilter(EnvironmentTypeSetting environmentType, ILogger logger)
        {
            _environmentType = environmentType;
            _logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            var request = context.HttpContext.Request;
            if (!request.IsAjaxRequest()) return;
 
            var exception = context.Exception;

            switch (exception)
            {
                case ValidationException ex:
                    HandleValidationException(context, ex);
                    break;
                default:
                    HandleGenericException(context, exception);
                    break;
            }
        }

        private void HandleGenericException(ExceptionContext context, Exception exception)
        {
            var statusCode = (int)HttpStatusCode.InternalServerError;
            var includeDebuggingInformation = _environmentType.IsLocal();
            var details = includeDebuggingInformation ? exception.ToString() : null;
            var title = includeDebuggingInformation ? exception.Message : "An unhandled ajax exception was thrown";

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Type = $"https://httpstatuses.com/{statusCode.ToString()}",
                Title = title,
                Detail = details
            };

            _logger.Error(exception, "An unhandled ajax exception was thrown: {ErrorMessage}.", exception.Message);

            context.HttpContext.Response.StatusCode = statusCode;
            context.Result = new JsonResult(problemDetails);
        }

        private void HandleValidationException(ExceptionContext context, ValidationException exception)
        {
            var statusCode = (int)HttpStatusCode.BadRequest;
            var includeDebuggingInformation = _environmentType.IsLocal();
            var details = includeDebuggingInformation ? exception.ToString() : null;
            var title = exception.Message;

            var errors = exception
                .Errors
                .GroupBy(failure => failure.PropertyName)
                .ToDictionary(failures => failures.Key, failures => failures.Select(failure => failure.ErrorMessage));

            var problemDetails = new ValidationProblemDetails
            {
                Status = statusCode,
                Type = $"https://httpstatuses.com/{statusCode.ToString()}",
                Title = title,
                Detail = details,
                Errors = errors
            };

            _logger.Warning(exception, "A validation ajax exception was thrown: {ErrorMessage}.", exception.Message);

            context.HttpContext.Response.StatusCode = statusCode;
            context.Result = new JsonResult(problemDetails);
        }

        private class ValidationProblemDetails : ProblemDetails
        {
            public Dictionary<string, IEnumerable<string>> Errors { get; set; } = new Dictionary<string, IEnumerable<string>>();
        }
    }
}