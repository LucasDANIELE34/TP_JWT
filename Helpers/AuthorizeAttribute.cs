namespace WebApi.Helpers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApi.Models;
using System.Linq;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    private readonly string[] _roles;

    public AuthorizeAttribute(params string[] roles)
    {
        _roles = roles;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = (User)context.HttpContext.Items["User"];
        if (user == null)
        {
            // utilisateur non connecté
            context.Result = new JsonResult(new { message = "Non autorisé" }) { StatusCode = StatusCodes.Status401Unauthorized };
        }

        if (_roles.Any() && !_roles.Contains(user.Role))
        {
            // le rôle de l'utilisateur n'est pas autorisé
            context.Result = new JsonResult(new { message = "Non autorisé" }) { StatusCode = StatusCodes.Status401Unauthorized };
        }
    }
}
