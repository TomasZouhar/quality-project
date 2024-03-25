using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace QualityProject.API.Swagger;

public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.ContainsKey("Authorization"))
        {
            return Task.FromResult(AuthenticateResult.Fail("Missing Authorization Header"));
        }

        string authorizationHeader = Request.Headers["Authorization"];
        if (string.IsNullOrEmpty(authorizationHeader))
        {
            return Task.FromResult(AuthenticateResult.Fail("Missing Authorization Header"));
        }

        if (!authorizationHeader.StartsWith("basic", StringComparison.OrdinalIgnoreCase))
        {
            return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header"));
        }

        string encodedUsernamePassword = authorizationHeader.Substring("Basic ".Length).Trim();
        Encoding encoding = Encoding.GetEncoding("iso-8859-1");
        string usernamePassword = encoding.GetString(Convert.FromBase64String(encodedUsernamePassword));

        int seperatorIndex = usernamePassword.IndexOf(':');
        string username = usernamePassword.Substring(0, seperatorIndex);
        string password = usernamePassword.Substring(seperatorIndex + 1);

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            return Task.FromResult(AuthenticateResult.Fail("Invalid Username or Password"));
        }

        if (username == "admin" && password == "admin")
        {
            var claims = new[] { new Claim(ClaimTypes.Name, username) };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
        else
        {
            return Task.FromResult(AuthenticateResult.Fail("Invalid Username or Password"));
        }
    }
}