using DemoApplication.Services.Abstracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DemoApplication.Attributs
{
    public class IsUserAdress : ActionFilterAttribute, IActionFilter
    {
        private readonly IUserService _userService;

        public IsUserAdress(IUserService userService)
        {
            _userService = userService;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (_userService.CurrentUser.Adress is null)
            {
                filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary
                {
                    { "controller", "account" },
                    { "action", "AdressAdd" }
                });
            }
          
              
        } 
    }
}
