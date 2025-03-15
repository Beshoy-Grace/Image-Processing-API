using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using Image.API.Logging.Http.Extensions;

namespace Image.Logging
{
    public class ExceptionLogFilter : IExceptionFilter, IActionFilter
    {
        private readonly ILogger<ExceptionLogFilter> _logger;
        private string requestBodyJson;
        private string reponseBodyJson;

        public int Order => 4;

        public ExceptionLogFilter(ILogger<ExceptionLogFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            requestBodyJson = JsonConvert.SerializeObject(context.ActionArguments);
        }

        public void OnException(ExceptionContext context)
        {
            Exception exception = context.Exception;
            HttpRequest request = context.HttpContext.Request;

            string exceptionType = exception.GetType().ToString();
            string newLine = Environment.NewLine;
            string url = request.GetAbsoluteUri().ToString();
            var exceptionMessage = $"Exception: {exceptionType} {newLine}" +
                $"Message: {exception.Message} {newLine}" +
                $"URL: {url} {newLine}" +
                $"Request Body: {requestBodyJson} {newLine}";// +
                //$"StackTrace: {exception.StackTrace}";

            _logger.LogError(exception, exceptionMessage);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            try
            {
                HttpRequest request = context.HttpContext.Request;
                string newLine = Environment.NewLine;
                string url = request.GetAbsoluteUri().ToString();
                reponseBodyJson = JsonConvert.SerializeObject(context.Result);
                reponseBodyJson = reponseBodyJson.Length < 1000 ? reponseBodyJson : reponseBodyJson.Substring(0, 1000);
                var LogCallMessage = $"URL: {url} {newLine}" +
                  $"request: {requestBodyJson} {newLine}"
                  + $"Request Body: {reponseBodyJson} {newLine}";

                _logger.LogWarning("URL Call \n {0}", LogCallMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError("Trace : {0}",ex.GetBaseException().Message);
            }
        }
    }
}