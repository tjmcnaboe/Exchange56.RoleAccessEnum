using Microsoft.AspNetCore.Authorization;

namespace Exchange56.RoleAccessEnum
{
    public class RoleAccessEnumAuthorizationRequirement<T> : IAuthorizationRequirement where T : System.Enum
    {
        public RoleAccessEnumAuthorizationRequirement(T requirement)
        {
            Requirement = requirement;
        }

        public T Requirement { get; }
    }



}
