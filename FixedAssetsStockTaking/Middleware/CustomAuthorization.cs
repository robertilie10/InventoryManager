using Microsoft.AspNetCore.Authorization;
using FixedAssetsStockTaking.Data;
using FixedAssetsStockTaking.Helpers;
using System.Security.Claims;


namespace RolesWithouFixedAssetsStockTakingtIdentity.Middleware
{
    public class CustomAuthorization : AuthorizationHandler<RoleRequirement>
    {
        private readonly FixedAssetsStockTakingContext _context;

        public CustomAuthorization(FixedAssetsStockTakingContext context)
        {
            _context = context;    

        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleRequirement requirement)
        {
            if (!context.User.HasClaim(c => c.Type == ClaimTypes.Name && c.Issuer == "AD AUTHORITY"))
            {
                return Task.FromResult(0);
            }

            var username = context.User.FindFirst(c => c.Type == ClaimTypes.Name && c.Issuer == "AD AUTHORITY").Value.Replace("MMRMAKITA\\", "");
            var roles = requirement.RoleNames;
            var RoleName = "Guest";
            var userId = _context.Users.FirstOrDefault(r => r.Username == username).Username;

            if (userId != null)
            {

                foreach (var role in roles)
                {   

                    RoleName = _context.Roles.FirstOrDefault(r => r.Name == role).Name;
                    
                    if(RoleName != null)
                    {
                        if (_context.Users.FirstOrDefault(u => u.Username == userId).RoleID == RoleName)
                        {
                            context.Succeed(requirement);
                            return Task.FromResult(0);
                        }
                    }
                }
            }


            return Task.FromResult(0);
        }
    }
}
