using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using HMS.WebClient.Services;
using HMS.Shared.Enums;

namespace HMS.WebClient.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly UserRole[] _allowedRoles;

        public AuthorizeAttribute(params UserRole[] roles)
        {
            _allowedRoles = roles;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var authService = context.HttpContext.RequestServices.GetService<AuthService>();
            var user = authService?.GetCurrentUser();

            // not logged in
            if (user == null)
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
                return;
            }

            // no specific roles required or user has a required role
            if (_allowedRoles.Length == 0 || _allowedRoles.Contains(user.Role))
            {
                return;
            }

            // user doesn't have any of the required roles
            context.Result = new RedirectToActionResult("AccessDenied", "Account", null);
        }
    }
}