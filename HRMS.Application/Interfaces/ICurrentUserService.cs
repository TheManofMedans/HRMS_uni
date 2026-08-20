using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Interfaces
{
    public interface ICurrentUserService
    {
        bool IsAuthenticated { get; }
        bool IsSuperAdmin {  get; } 
        int? EmployeeId { get; }
        IReadOnlyDictionary<int, string> CompanyRoles { get; }
    }
}
