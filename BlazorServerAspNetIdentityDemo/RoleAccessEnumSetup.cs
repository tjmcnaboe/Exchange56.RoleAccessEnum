using BlazorServerAspNetIdentityDemo.Components.Account;
using BlazorServerAspNetIdentityDemo.Data;
using Exchange56.RoleAccessEnum;
using Exchange56.RoleAccessEnum.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace BlazorServerAspNetIdentityDemo
{
    public class AppRolePermissionProvider : IRolePermissionProvider<DemoAppRoleRequirement>//IPermissionRoleProvider<Permissions>
    {
        public List<IRolePermission<DemoAppRoleRequirement>> GetRoles()
        {
            var roles = new List<IRolePermission<DemoAppRoleRequirement>>();

            //RootPermissions globalAdminPermissions = RootPermissions.All;
            roles.Add(new AppRole(RoleConstants.GlobalAdmin, DemoAppRoleRequirement.All));

            //RootPermissions ownerPermissions = RootPermissions.Owner;
            roles.Add(new AppRole(RoleConstants.Owner, DemoAppRoleRequirement.Owner));


            //RootPermissions adminPermissions = RootPermissions.Admin;
            roles.Add(new AppRole(RoleConstants.Admin, DemoAppRoleRequirement.Admin));

            roles.Add(new AppRole(RoleConstants.AccountManager, DemoAppRoleRequirement.AccountManager));
            roles.Add(new AppRole(RoleConstants.ProjectManager, DemoAppRoleRequirement.AccountManager));
            roles.Add(new AppRole(RoleConstants.ProjectManager, DemoAppRoleRequirement.IsDeveloper));

            roles.Add(new AppRole(RoleConstants.Developer, DemoAppRoleRequirement.Dev));

            roles.Add(new AppRole(RoleConstants.Member, DemoAppRoleRequirement.IsMember));
            return roles;
        }


    }

    public class AppRole : IRolePermission<DemoAppRoleRequirement>
    {
        public AppRole(string name, DemoAppRoleRequirement role)
        {
            Name = name;
            Role = role;
        }
        public string Name { get; set; }
        public DemoAppRoleRequirement Role { get; set; }
    }


    [Flags]
    public enum DemoAppRoleRequirement
    {
        None = 0,
        IsAdmin = 1,
        IsDeveloper = 2,
        IsAccountManager = 4,
        IsOwner = 8,
        IsGlobalAdmin = 16,
        IsMember = 32,
        Forecast = 64,
        ViewAccessControl = 128,
        Dev = IsDeveloper + Forecast,
        AccountManager = IsAccountManager,
        Admin = IsAdmin + IsDeveloper + IsAccountManager,
        Owner = IsAdmin + IsDeveloper + IsAccountManager + IsOwner,
        All = ~None
    }

    public class RoleConstants
    {
        public const string GlobalAdmin = "globaladmin";
        public const string Owner = "owner";
        public const string Admin = "admin";
        public const string Developer = "developer";
        public const string AccountManager = "accountmanager";
        public const string Member = "member";
        public const string ProjectManager = "projectmanager";
        public static List<string> Roles = new List<string>() { GlobalAdmin, Owner, Admin, Developer, AccountManager, Member };
    }

    public class AppAccessPolicy : IRoleAccessEnumAuthorizationPolicy
    {
        public string GetPolicyPrefix()
        {
            throw new NotImplementedException();
        }

        public async Task<AuthorizationPolicy?> TryGetPolicyAsync(string policyName)
        {
            string _policyPrefix = typeof(DemoAppRoleRequirement).Name;

            if (PolicyNameProvider<DemoAppRoleRequirement>.IsValidPolicyName(policyName, _policyPrefix))
            {
                var permissions = PolicyNameProvider<DemoAppRoleRequirement>.GetPermissionsFrom(policyName, _policyPrefix);

                var policy = new AuthorizationPolicyBuilder()
                    .AddRequirements(new RoleAccessEnumAuthorizationRequirement<DemoAppRoleRequirement>(permissions))
                    .Build();

                return policy;
            }

            return null;
        }
    }



    /// <summary>
    ///this is a stub for testing in aspnetcore this could inject the user manager and get roles from the user
    /// </summary>
    public class TestRoleProvider : IUserRoleProvider
    {
        public async Task<List<string>> GetRoles()
        {
            //return new List<string>();
            return new List<string>() { RoleConstants.Owner };

            //return new List<string>() { RoleConstants.Developer };
        }
    }

    public class AspNetIdentityRoleProvider : IUserRoleProvider
    {
        private UserManager<ApplicationUser> _userManager;
        private IHttpContextAccessor httpAccesor;

        public AspNetIdentityRoleProvider(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpAccessor)
        {
            _userManager = userManager;
            httpAccesor = httpAccessor;
        }


        public async Task<List<string>> GetRoles()
        {
            if (httpAccesor.HttpContext.User.Identity.IsAuthenticated)
            {
                var user = await _userManager.FindByNameAsync(httpAccesor.HttpContext.User.Identity.Name);
                if (user != null)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    return roles.ToList();
                }
            }
            return new List<string>();
        }
    }

    public class SubscriptionLevelRoleProvider : IUserRoleProvider
    {
        private UserManager<ApplicationUser> _userManager;
        private IHttpContextAccessor httpAccesor;
        public SubscriptionLevelRoleProvider(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpAccessor)
        {
            _userManager = userManager;
            httpAccesor = httpAccessor;
        }

        public async Task<List<string>> GetRoles()
        {
            var roles = new List<string>();
            if (httpAccesor.HttpContext.User.Identity.IsAuthenticated)
            {
                var user = await _userManager.FindByNameAsync(httpAccesor.HttpContext.User.Identity.Name);
                if (user != null)
                {
                    roles.Add(RoleConstants.Member);
                    if (user.SubscriptionLevel == SubscriptionLevel.Free)
                    {
                        roles.Add(RoleConstants.Developer);
                    }
                    else if (user.SubscriptionLevel == SubscriptionLevel.Paid)
                    {
                        roles.Add(RoleConstants.AccountManager);
                        roles.Add(RoleConstants.ProjectManager);
                    }
                }

            }
                
            return roles;
        }
    }
}
