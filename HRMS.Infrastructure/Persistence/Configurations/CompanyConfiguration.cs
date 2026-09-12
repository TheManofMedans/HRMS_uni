using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HRMS.domain.Entities;

namespace HRMS.Infrastructure.Persistence.Configurations
{
    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure (EntityTypeBuilder<Company> builder)
        {
            builder.ToTable("Company");
            builder.HasKey(x => x.Id);
            builder.Property(u => u.RegNum).IsRequired().HasMaxLength(100);
            builder.Property(u=>u.Name).IsRequired().HasMaxLength(100);
            builder.Property(u=>u.Address).HasMaxLength(500);
            builder.Property(c => c.OvertimeRate).HasColumnType("decimal(5,2)");
            builder.Property(c => c.LateDeductionValue).HasColumnType("decimal(18,2)");
            builder.HasIndex(u => u.RegNum).IsUnique();

            builder.HasMany(u=>u.Departments)
                .WithOne(u => u.Company)
                .HasForeignKey(u=>u.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
