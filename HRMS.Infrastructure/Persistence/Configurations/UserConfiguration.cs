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
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {

            builder.Property(u=>u.FirstName).IsRequired().HasMaxLength(100);
            builder.Property(u=>u.LastName).IsRequired().HasMaxLength(100);
            builder.Property(u => u.SSN).IsRequired().HasMaxLength(10);
            builder.Property(u => u.PhoneNumber).IsRequired().HasMaxLength(100);
            builder.HasIndex(u => u.SSN).IsUnique();
            builder.HasIndex(u => u.PhoneNumber).IsUnique();
        }
    }
}
