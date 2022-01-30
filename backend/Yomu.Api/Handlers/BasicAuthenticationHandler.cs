using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Yomu.Api.Services;

namespace Yomu.Api.Handlers;

public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
	private readonly IAuthenticator authenticator;
	
    public BasicAuthenticationHandler(
		IOptionsMonitor<AuthenticationSchemeOptions> options,
		ILoggerFactory logger,
		UrlEncoder encoder,
		ISystemClock clock,
		IAuthenticator authenticator) : base(options, logger, encoder, clock)
    {
		this.authenticator = authenticator;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
		List<Claim> claims = new List<Claim>();
		var header = Request.Headers.Authorization;
		if (!StringValues.IsNullOrEmpty(header)) {
			var authHeader = AuthenticationHeaderValue.Parse(header);
			var credentials = Encoding.UTF8.GetString(
				Convert.FromBase64String(authHeader.Parameter)
			).Split(new[] {':'}, 2);
		
			var email = credentials[0];
			var password = credentials[1];

			var login = await this.authenticator.Authenticate(email, password);
            if (login == null) {
                return AuthenticateResult.Fail("User could not be authenticated");
            }

			claims.Add(new Claim(ClaimTypes.Email, login.Email));
            claims.Add(new Claim(ClaimTypes.Role, login.Role.ToString()));
		}
		var identity = new ClaimsIdentity(claims);
		var principal = new ClaimsPrincipal(identity);
		var ticket = new AuthenticationTicket(principal, Scheme.Name);
		
		return AuthenticateResult.Success(ticket);
    }

	protected override Task HandleChallengeAsync(AuthenticationProperties properties)
	{
        if (!Request.IsHttps)
        {
    		const string insecureProtocolMessage = "Request is HTTP, Basic Authentication will not respond.";
            Logger.LogInformation(insecureProtocolMessage);
            // 421 Misdirected Request
            // The request was directed at a server that is not able to produce a response.
            // This can be sent by a server that is not configured to produce responses for the combination of scheme and authority that are included in the request URI.
            Response.StatusCode = StatusCodes.Status421MisdirectedRequest;
        }
        else
        {
            Response.StatusCode = 401;
            /*if (!Options.SuppressWWWAuthenticateHeader)
            {
            	var headerValue = _Scheme + $" realm=\"{Options.Realm}\"";
                Response.Headers.Append(HeaderNames.WWWAuthenticate, headerValue);
        	}*/
		}
		
		return Task.CompletedTask;
	}
}
