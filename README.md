**A flexible, enum-based authorization framework for ASP.NET Core applications**

Exchange56.RoleAccessEnum allows developers to define application permissions as enums, map user roles to those permissions, and create strongly-typed authorization attributes. This framework enables multiple frontends or backends to share a common set of permissions while retaining full flexibility in role-to-permission mappings.
## Features

- **Enum-based permissions**: Define your app’s permissions as strongly-typed enums.
- **Flexible role mapping**: Map user roles to enum permissions in any way your application requires.
- **Multiple user role sources**: Fetch roles from ASP.NET Identity, subscription levels, external services, or any custom provider.
- **Strongly-typed authorization attributes**: Apply permissions safely in controllers, Razor pages, or APIs using generic attributes.
- **Dynamic policies**: Automatically generate `AuthorizationPolicy` objects from enum values and prefixes.
- **Composite permissions**: Combine flags for complex roles and permissions.

---

## Installation

```bash
dotnet add package Exchange56.RoleAccessEnum




## Getting Started

### 1. Define your permission enum

```csharp
[Flags]
public enum AppPermissions
{
    None = 0,
    Read = 1,
    Write = 2,
    Delete = 4,
    Admin = Read | Write | Delete,
    All = ~None
}
```

---

### 2. Map roles to permissions

Implement `IRolePermissionProvider<T>` to map your app’s roles to enum permissions:

```csharp
public class AppRolePermissionProvider : IRolePermissionProvider<AppPermissions>
{
    public List<IRolePermission<AppPermissions>> GetRoles()
    {
        return new List<IRolePermission<AppPermissions>>
        {
            new AppRole("Admin", AppPermissions.Admin),
            new AppRole("User", AppPermissions.Read)
        };
    }
}
```

---

### 3. Provide user roles

Implement `IUserRoleProvider` to fetch user roles from your preferred source:

```csharp
public class MyRoleProvider : IUserRoleProvider
{
    public async Task<List<string>> GetRoles()
    {
        // Example: fetch roles from ASP.NET Identity or database
        return new List<string> { "Admin" };
    }
}
```

---

### 4. Apply strongly-typed authorization

Use `RoleAccessEnumAuthorizeAttribute<T>` on controllers, actions, or pages:

```csharp
[RoleAccessEnumAuthorize<AppPermissions>(AppPermissions.Write)]
public IActionResult EditItem()
{
    return View();
}
```

---

### 5. Register services

In `Startup.cs` or `Program.cs`:

```csharp
services.AddSingleton<IRolePermissionProvider<AppPermissions>, AppRolePermissionProvider>();
services.AddSingleton<IUserRoleProvider, MyRoleProvider>();
services.AddSingleton<IAuthorizationPolicyProvider, RoleAccessEnumAuthorizationPolicyProvider>();
services.AddSingleton<IAuthorizationHandler, RoleAccessEnumAuthorizationHandler<AppPermissions>>();
```

---

## How It Works

1. Developers define **enum-based permissions** for the application.
2. **Roles are mapped to permissions** via `IRolePermissionProvider<T>`.
3. **User roles are fetched** from any source implementing `IUserRoleProvider`.
4. `RoleAccessEnumAuthorizationHandler<T>` checks whether the user’s roles satisfy the permission requirement.
5. `RoleAccessEnumAuthorizeAttribute<T>` provides **strongly-typed, declarative authorization** in your code.

This allows multiple frontends or applications to **share a consistent set of permissions**, while each backend can **map roles differently** to suit its needs.

---

## Example

```csharp
[Flags]
public enum DemoAppRoleRequirement
{
    None = 0,
    IsAdmin = 1,
    IsDeveloper = 2,
    AccountManager = 4,
    All = ~None
}

[RoleAccessEnumAuthorize<DemoAppRoleRequirement>(DemoAppRoleRequirement.IsAdmin)]
public IActionResult AdminDashboard()
{
    return View();
}
```

---

## License

This project is licensed under the MIT License — see the [LICENSE](LICENSE) file for details.


