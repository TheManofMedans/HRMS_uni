using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HRMS.domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Persistence.Configurations
{
    public class EmployeeDepartmentConfiguration : IEntityTypeConfiguration<EmployeeDepartment>
    {
        public void Configure(EntityTypeBuilder<EmployeeDepartment> builder)
        {
            builder.ToTable("EmployeeDepartment");
            builder.HasKey(ed => new{ ed.EmployeeID,ed.DepartmentID});
            builder.HasOne(ed => ed.Employee)
                .WithMany(e=>e.EmployeeDepartments)
                .HasForeignKey(ed => ed.EmployeeID)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(ed => ed.Department)
                .WithMany(d => d.DepartmentEmployees)
                .HasForeignKey(ed =>ed.DepartmentID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
