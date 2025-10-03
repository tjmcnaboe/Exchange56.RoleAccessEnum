using Exchange56.RoleAccessEnum.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace Exchange56.RoleAccessEnum
{

    public partial class RoleAccessEnumAuthorizationHandler<T> : AuthorizationHandler<RoleAccessEnumAuthorizationRequirement<T>> where T : System.Enum
    {
        private IUserRoleProvider _RequestContextRoles;
        private IRolePermissionProvider<T> _applicationRoleProvider;

        public RoleAccessEnumAuthorizationHandler(IUserRoleProvider rc, IRolePermissionProvider<T> permissionRoleProvider)
        {
            _RequestContextRoles = rc;
            _applicationRoleProvider = permissionRoleProvider;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleAccessEnumAuthorizationRequirement<T> requirement)
        {
            var roles = await _RequestContextRoles.GetRoles();
            var userRoles = _applicationRoleProvider.GetRoles().Where(r => roles.Contains(r.Name)).ToList();

            var userPermissions = default(T);


            foreach (var role in userRoles)
            {
                //userPermissions = role.Permissions;
                if (role.Role.HasFlag(requirement.Requirement))
                //if(requirement.Permissions.(role.Permissions))
                {
                    context.Succeed(requirement);
                    return;
                }
            }


            return;
        }

    }



}
