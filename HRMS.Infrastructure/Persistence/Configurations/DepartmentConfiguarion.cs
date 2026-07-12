using HRMS.domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Persistence.Configurations
{
    public class DepartmentConfiguarion :IEntityTypeConfiguration<Department>
    {
        public void Configure (EntityTypeBuilder<Department> builder)
        {
            builder.ToTable("Departments");
            builder.HasKey(d => d.Id);
            builder.Property(d => d.Name).HasMaxLength(200).IsRequired();
            builder.Property(d => d.Description).HasMaxLength(300);

            builder.HasMany(d => d.Attendances)
                .WithOne(a=> a.department)
                .HasForeignKey(a =>a.departmentId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
