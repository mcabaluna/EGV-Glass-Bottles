using System;
using System.Linq;
using Microsoft.Owin.Security.OAuth;
using System.Security.Claims;
using System.Threading.Tasks;
using Merchandising.DTO;

namespace Merchandising.Api
{
    public class AuthorizationServerProvider: OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            using (DbContextModel db = new DbContextModel())
            {
                var user = db.Users.FirstOrDefault(x =>
                    x.UserName.Equals(context.UserName, StringComparison.OrdinalIgnoreCase)
                    && x.Password == context.Password); 
                if (user == null)
                {
                    context.SetError("invalid_grant", "Provided username and password is incorrect");
                    return;
                }
                var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                identity.AddClaim(new Claim(ClaimTypes.Role, user.Role.ToString()));
                identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
                context.Validated(identity);
            }
        }
    }
}