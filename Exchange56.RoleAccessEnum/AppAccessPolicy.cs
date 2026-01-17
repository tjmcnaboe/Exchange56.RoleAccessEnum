using Microsoft.AspNetCore.Authorization;

namespace Exchange56.RoleAccessEnum
{
    public class AppAccessPolicy<T> : IRoleAccessEnumAuthorizationPolicy where T : struct, System.Enum
    {

        public string GetPolicyPrefix()
        {
            throw new NotImplementedException();
        }

        public async Task<AuthorizationPolicy?> TryGetPolicyAsync(string policyName)
        {
            string _policyPrefix = typeof(T).Name;

            if (PolicyNameProvider<T>.IsValidPolicyName(policyName, _policyPrefix))
            {
                var permissions = PolicyNameProvider<T>.GetPermissionsFrom(policyName, _policyPrefix);

                var policy = new AuthorizationPolicyBuilder()
                    .AddRequirements(new RoleAccessEnumAuthorizationRequirement<T>(permissions))
                    .Build();

                return policy;
            }

            return null;
        }
    }



}
