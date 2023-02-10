using Domain;
using Microsoft.AspNetCore.Authorization;
using UserService.Interface;

namespace Daya.Jwt
{
    public record IsActiveRequirement : IAuthorizationRequirement;
    public class JustActiveAuth 
        : AuthorizationHandler<IsActiveRequirement>
    {
        private readonly IUserRepo rp;

        public JustActiveAuth(IUserRepo rp)
        {
            this.rp = rp;
        }

        protected override Task HandleRequirementAsync
            (AuthorizationHandlerContext context, IsActiveRequirement requirement)
        {
            if (rp.GetUserActiveState(Guid.Parse(context.User.Identity.Name)))
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
