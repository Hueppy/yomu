using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Yomu.Shared.Contexts;
using Yomu.Shared.Models;

namespace Yomu.Api.Extensions;

public static class ControllerBaseExtension
{
    public static async Task<ActionResult> WithCurrentUser(
        this ControllerBase controller,
        YomuContext context,
        Func<User, Task<ActionResult>> action)
    {
        var email = controller.HttpContext.User.FindFirst(ClaimTypes.Email);
        if (email == null)
        {
            return controller.Forbid();
        }
        var user = context.Users.FirstOrDefault(x => x.Email == email.Value);
        if (user == null)
        {
            return controller.Forbid();
        }

        return await action(user);
    }

    public static bool IsAdministrator(this ControllerBase controller)
    {
        var roleClaim = controller.HttpContext.User.FindFirst(ClaimTypes.Role);
        return roleClaim != null
            && Role.TryParse<Role>(roleClaim.Value, out Role role)
            && role == Role.Administrator;
    }
}
