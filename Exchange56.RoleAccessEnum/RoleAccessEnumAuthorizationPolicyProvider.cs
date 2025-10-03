﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Exchange56.RoleAccessEnum
{
    public class RoleAccessEnumAuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider //where T : struct, System.Enum
    {
        private readonly AuthorizationOptions _options;
        private readonly IEnumerable<IRoleAccessEnumAuthorizationPolicy> _policytypes;

        private List<string> _policyPrefix { get; set; }
        public RoleAccessEnumAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options, IEnumerable<IRoleAccessEnumAuthorizationPolicy> enumPolicies)
            : base(options)
        {

            _options = options.Value;
            _policytypes = enumPolicies;
        }

        public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            var policy = await base.GetPolicyAsync(policyName);
            if (policy == null)
            {
                foreach (var t in _policytypes)
                {
                    if (policy == null)
                    {
                        policy = await t.TryGetPolicyAsync(policyName);
                    }
                }
            }

            return policy;
        }

    }



}
