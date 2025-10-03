using Microsoft.AspNetCore.Identity;

namespace BlazorServerAspNetIdentityDemo.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public SubscriptionLevel SubscriptionLevel { get; set; } = SubscriptionLevel.Bronze;
    }

    public enum SubscriptionLevel
    {
        Platinum,
        Gold,
        Silver,
        Bronze,
        Free,
        Paid,
        Tester
    }

}
