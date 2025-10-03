namespace Exchange56.RoleAccessEnum
{
    public class RoleAccessEnumAuthorizeAttribute<T> : Microsoft.AspNetCore.Authorization.AuthorizeAttribute where T : struct, Enum
    {
        public RoleAccessEnumAuthorizeAttribute() { }

        public RoleAccessEnumAuthorizeAttribute(string policy) : base(policy) { }

        public RoleAccessEnumAuthorizeAttribute(T member)
        {
            _policyPrefix = typeof(T).Name;
            Permissions = member;
        }

        public string _policyPrefix { get; }

        //public abstract 
        public T Permissions
        {
            get
            {
                return !string.IsNullOrEmpty(Policy)
                    ? PolicyNameProvider<T>.GetPermissionsFrom(Policy, _policyPrefix)
                    : default(T);
            }
            set
            {
                var name = PolicyNameProvider<T>.GeneratePolicyNameFor(value, _policyPrefix);
                if (name == null)
                { Policy = string.Empty; }
                else
                { Policy = name; }
            }
        }
    }



}
