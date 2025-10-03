namespace Exchange56.RoleAccessEnum
{
    public static class PolicyNameProvider<T> where T : struct, Enum
    {

        public const string DefaultPrefix = "Permissions";

        public static bool IsValidPolicyName(string? policyName, string prefix = "")
        {
            if (string.IsNullOrEmpty(prefix)) { prefix = DefaultPrefix; }
            return policyName != null && policyName.StartsWith(prefix, StringComparison.OrdinalIgnoreCase);
        }

        public static string GeneratePolicyNameFor(T permissions, string prefix = "")
        {
            if (string.IsNullOrEmpty(prefix)) { prefix = DefaultPrefix; }
            var permissionsInt = EnumsNET.Enums.ToInt32(permissions);
            //int something = (int)permissions;
            // permissions.AsString();
            return $"{prefix}{permissionsInt}";
        }


        public static T GetPermissionsFrom(string policyName, string prefix = "")
        {
            if (string.IsNullOrEmpty(prefix)) { prefix = DefaultPrefix; }
            var permissionsValue = int.Parse(policyName[prefix.Length..]!);
            T val = ((T[])Enum.GetValues(typeof(T)))[0];

            foreach (T enumValue in (T[])Enum.GetValues(typeof(T)))
            {
                if (Convert.ToInt32(enumValue).Equals(permissionsValue))
                {
                    val = enumValue;
                    break;
                }
            }
            return val;
        }



    }



}
