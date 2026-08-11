namespace HRMS.API.Authorization
{
    public interface ICompanyResolver
    {
        Task<int?> ResolveCompanyId(int resourceId);
    }
}
