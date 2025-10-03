namespace Exchange56.RoleAccessEnum.Interfaces
{
    public interface IRolePermission<T> where T : System.Enum
    {
        public string Name { get; set; }
        public T Role { get; set; }
    }
}
