namespace HRMS.API.Authorization
{
    public interface IResourceOwnerResolver
    {
        Task<int?> ResolveEmployeeId(int resourceId);
    }
}
