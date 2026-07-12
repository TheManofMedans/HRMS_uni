using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.domain.Entities
{
    public class Company
    {
        public int Id { get; set; }
        public string RegNum { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;    
        public string Address {  get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<UserCompany> UserCompanies { get; set; } = new List<UserCompany>();
        public ICollection<Department> Departments { get; set; } = new List<Department>();

    }
}
