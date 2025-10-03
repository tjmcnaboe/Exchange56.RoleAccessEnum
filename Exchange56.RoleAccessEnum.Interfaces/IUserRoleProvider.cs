namespace Exchange56.RoleAccessEnum.Interfaces
{

    public interface IUserRoleProvider
    {
        public Task <List<string>> GetRoles();
    }
}
