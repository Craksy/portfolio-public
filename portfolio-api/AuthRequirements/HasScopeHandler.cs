using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace auth_api.AuthRequirements
{
    /// <summary>
    /// An Auth handler using <see cref="HasScopeRequirement"/> to check if the user has the required role
    /// </summary>
    public class HasScopeHandler : AuthorizationHandler<HasScopeRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HasScopeRequirement requirement)
        {
            if(!context.User.HasClaim(c => c.Type == "scope" && c.Issuer == requirement.Issuer)){
                return Task.CompletedTask;
            }

            var scopes = context.User.FindFirst(c => c.Type == "scope" && c.Issuer == requirement.Issuer)?.Value.Split(' ');

            if(scopes != null && scopes.Any(s => s==requirement.Scope)){
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}