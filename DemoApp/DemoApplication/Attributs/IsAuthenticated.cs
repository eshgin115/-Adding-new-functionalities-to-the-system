using DemoApplication.Services.Abstracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DemoApplication.Attributs
{
    public class IsAuthenticated : ActionFilterAttribute, IActionFilter
    {
        private readonly IUserService _userService;

        public IsAuthenticated( IUserService userService)
        {
            _userService = userService;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (_userService.IsAuthenticated)
            {
                filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary
                {
                    { "controller", "account" },
                    { "action", "dashboard" }
                });
            }
        }
    }
}