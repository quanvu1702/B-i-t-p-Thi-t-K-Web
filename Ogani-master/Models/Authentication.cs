using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

namespace Ogani_master.Models.Authentication
{
    public class Authentication : ActionFilterAttribute
    {
        public override void OnActionExecuting(
            ActionExecutingContext context)
        {
            if (context.HttpContext.Session
                .GetString("UserName") == null)
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