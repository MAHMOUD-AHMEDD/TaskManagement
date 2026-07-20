using Microsoft.AspNetCore.Mvc.Filters;

namespace TaskManagement.API.Filters
{
    public class LoggingActionFilter : IActionFilter
    {

        private readonly ILogger<LoggingActionFilter> _logger;

        public LoggingActionFilter(ILogger<LoggingActionFilter> logger)
        {
            _logger = logger;
        }


        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Runs BEFORE the controller action executes
            var controllerName = context.RouteData.Values["controller"];
            var actionName = context.RouteData.Values["action"];
            _logger.LogInformation("➡️ Executing {Controller}.{Action}", controllerName, actionName);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Runs AFTER the controller action executes
            var controllerName = context.RouteData.Values["controller"];
            var actionName = context.RouteData.Values["action"];
            _logger.LogInformation("⬅️ Finished {Controller}.{Action} with status {StatusCode}",
                controllerName, actionName, context.HttpContext.Response.StatusCode);
        }
    }
}
