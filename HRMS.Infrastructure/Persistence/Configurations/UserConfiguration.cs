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
            builder.ToTable("Users");
            builder.HasKey(u => u.Id);
            builder.Property(u=>u.FirstName).IsRequired().HasMaxLength(100);
            builder.Property(u=>u.LastName).IsRequired().HasMaxLength(100);
            builder.Property(u=>u.Email).IsRequired().HasMaxLength(256);
            builder.Property(u => u.PhoneNumber).IsRequired().HasMaxLength(50);
            builder.HasIndex(u => u.Email).IsUnique();
            builder.HasIndex(u=>u.PhoneNumber).IsUnique();
            builder.HasIndex(u => u.SSN).IsUnique();
        }
    }
}
