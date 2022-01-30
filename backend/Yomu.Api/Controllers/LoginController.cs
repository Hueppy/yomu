using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Yomu.Api.Extensions;
using Yomu.Shared.Contexts;
using Yomu.Shared.Models;

namespace Yomu.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{
	private YomuContext context;
    private IPasswordHasher<Login> hasher;

	public LoginController(
        YomuContext context,
        IPasswordHasher<Login> hasher)
	{
		this.context = context;
        this.hasher = hasher;
	}

    /// <summary>
    /// Creates a new login
    /// </summary>
    /// <response code="403">Login already exists</response>
    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult> Post(string email, string password)
    {
        if (context.Logins.Any((x) => x.Email == email))
        {
            return Forbid();
        }
        
        var login = new Login()
        {
            Email = email,
            Role = Role.User
        };
        login.PasswordHash = this.hasher.HashPassword(login, password);

        await context.Logins.AddAsync(login);
        await context.SaveChangesAsync();
        
        return Ok();
    }

    [HttpPatch("{email}")]
    public async Task<ActionResult> PatchRole(string email, Role role)
    {
        return await this.WithCurrentUser(context, async (currentUser) =>
        {
            var currentLogin = await context.Logins.FindAsync(currentUser.Email);
            if (currentLogin.Role <= role)
            {
                return Forbid();
            }
            
            var login = await context.Logins.FindAsync(email);
            if (login == null)
            {
                return NotFound();
            }

            login.Role = role;

            context.Logins.Update(login);
            await context.SaveChangesAsync();
        
            return Ok();
        });
    }
}
