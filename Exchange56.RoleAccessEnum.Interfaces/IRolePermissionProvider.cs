namespace Exchange56.RoleAccessEnum.Interfaces
{
    //Assosciates Application Permissions with Roles
    public interface IRolePermissionProvider<T> where T : System.Enum
    {
        List<IRolePermission<T>> GetRoles();
    }
}
