using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using MiniTwit.Core.Error;
using MiniTwit.Core.Responses;

namespace MiniTwit.Server.Authentication;

public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock) { }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        // Convert 401 to 403 and attach APIError
        SetupResponseConverter();

        // Get auth header
        var authHeader = Request.Headers[HeaderNames.Authorization].ToString();

        if (authHeader != null && authHeader.StartsWith("basic", StringComparison.OrdinalIgnoreCase))
        {
            if (authHeader == "Basic c2ltdWxhdG9yOnN1cGVyX3NhZmUh")
            {
                var claims = new[] { new Claim("name", "simulator"), new Claim(ClaimTypes.Role, "Simulator") };
                var identity = new ClaimsIdentity(claims, "Basic");
                var claimsPrincipal = new ClaimsPrincipal(identity);

                return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, Scheme.Name)));
            }
        }

        return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header"));
    }

    private void SetupResponseConverter()
    {
        Context.Response.OnStarting(async (state) =>
       {
           if (Response.StatusCode == StatusCodes.Status401Unauthorized)
           {
               Response.StatusCode = StatusCodes.Status403Forbidden;
               await Response.WriteAsJsonAsync<APIError>(ErrorType.UNAUTHORIZED_CREDENTIALS.ToAPIError(HTTPResponse.Forbidden));
           }
       }, null!);
    }
}
