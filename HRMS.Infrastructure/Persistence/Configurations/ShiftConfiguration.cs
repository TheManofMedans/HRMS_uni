using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HRMS.domain.Entities;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Persistence.Configurations
{
    public class ShiftConfiguration : IEntityTypeConfiguration<Shift>
    {
        public void Configure(EntityTypeBuilder<Shift> builder)
        {
            builder.ToTable("Shifts");
            builder.HasKey(s => s.Id);

            builder.Property(s => s.ShiftName).IsRequired().HasMaxLength(100);

            builder.HasOne(s => s.Company)
                .WithMany(c => c.Shifts)
                .HasForeignKey(s=> s.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.HasMany(s => s.Attendances)
                   .WithOne(a => a.shift)
                   .HasForeignKey(a => a.ShiftId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
