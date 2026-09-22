using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

namespace Ogani_master.Filters
{
    public class SessionLoginAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(
            ActionExecutingContext context)
        {
            var username = context.HttpContext.Session
                .GetString("Username");

            if (string.IsNullOrEmpty(username))
            {
                context.Result = new RedirectToRouteResult(
                    new RouteValueDictionary
                    {
                        ["area"] = "",
                        ["controller"] = "Access",
                        ["action"] = "Login"
                    }
                );
            }
        }
    }
}