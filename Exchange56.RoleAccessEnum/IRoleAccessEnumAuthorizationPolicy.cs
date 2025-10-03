using Microsoft.AspNetCore.Authorization;

namespace Exchange56.RoleAccessEnum
{
    public interface IRoleAccessEnumAuthorizationPolicy
    {
        Task<AuthorizationPolicy?> TryGetPolicyAsync(string policyName);
        string GetPolicyPrefix();
    }



}
