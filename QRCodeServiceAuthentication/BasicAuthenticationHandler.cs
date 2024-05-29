using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace QRCodeService.Authentication
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly AuthenticationSettings _authSettings;

        public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, 
            ILoggerFactory logger, 
            UrlEncoder encoder, 
            ISystemClock clock,
            IOptions<AuthenticationSettings> authSettings) : base(options, logger, encoder, clock)
        {
            _authSettings = authSettings.Value;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return Task.FromResult(AuthenticateResult.Fail("Not authorized"));
            }

            var authorizationHeader = Request.Headers["Authorization"].ToString();

            if (!authorizationHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
            {
                return Task.FromResult(AuthenticateResult.Fail("Not authorized"));
            }

            var authBase64Decoded = Encoding.UTF8.GetString(
                    Convert.FromBase64String(
                        authorizationHeader.Replace("Basic ", "", StringComparison.OrdinalIgnoreCase))
                );

            var credentials = authBase64Decoded.Split(new[] { ':' }, 2);

            if (credentials.Length != 2)
            {
                return Task.FromResult(AuthenticateResult.Fail("Not authorized"));
            }

            var username = credentials[0];
            var password = credentials[1];

            if (Encrypt(username) != _authSettings.Username ||
                Encrypt(password) != _authSettings.Password)
            {
                return Task.FromResult(AuthenticateResult.Fail("Not authorized"));
            }

            var client = new BasicAuthenticationClient
            {
                AuthenticationType = BasicAuthenticationDefaults.AuthenticationScheme,
                IsAuthenticated = true,
                Name = username
            };

            var claimPrincipal = new ClaimsPrincipal(new ClaimsIdentity(client,
                    new [] { new Claim(ClaimTypes.Name, username) } ));

            var authenticationTicket = new AuthenticationTicket(claimPrincipal, Scheme.Name);
            
            return Task.FromResult(AuthenticateResult.Success(authenticationTicket));
        }

        private string Encrypt(string value)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(value);
            SHA256Managed hashstring = new();
            byte[] hash = hashstring.ComputeHash(bytes);
            string hashString = string.Empty;
            foreach (byte x in hash)
            {
                hashString += string.Format("{0:x2}", x);
            }
            return hashString;
        }
    }
}
