using Microsoft.AspNetCore.Identity;
using Yomu.Shared.Contexts;
using Yomu.Shared.Models;

namespace Yomu.Api.Services;

public class Authenticator : IAuthenticator
{
	private readonly YomuContext context;
	private readonly IPasswordHasher<Login> hasher;
	
    public Authenticator(
		YomuContext context,
		IPasswordHasher<Login> hasher)
    {
		this.context = context;
		this.hasher = hasher;
    }
	
	public async Task<Login> Authenticate(string email, string password)
	{
		var login = await this.context.Logins.FindAsync(email);
        
        if (login == null)
		{
			return null;
		}
			
		var result = this.hasher.VerifyHashedPassword(login, login.PasswordHash, password);
		switch (result)
		{
			case PasswordVerificationResult.Success:
				return login;
			case PasswordVerificationResult.SuccessRehashNeeded:
				login.PasswordHash = hasher.HashPassword(login, password);
				context.Update(login);
				context.SaveChanges();
				return login;
			default:
				return null;
		}
	}
}
