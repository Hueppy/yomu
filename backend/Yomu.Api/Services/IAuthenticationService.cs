using Yomu.Shared.Models;

namespace Yomu.Api.Services;

public interface IAuthenticator
{
	Task<Login> Authenticate(string email, string password);	
}
