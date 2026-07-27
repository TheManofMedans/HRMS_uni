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
    public class UserCompanyConfiguartion : IEntityTypeConfiguration<UserCompany>
    {
        public void Configure (EntityTypeBuilder<UserCompany> builder)
        {
            builder.ToTable("UserCompanies");
            builder.HasKey(uc => new {uc.UserId, uc.CompanyId});
            builder.HasOne(u=>u.User)
                .WithMany(u=>u.UserCompanies)
                .HasForeignKey(u=>u.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(uc => uc.Company)
                .WithMany(c => c.UserCompanies)
                .HasForeignKey(uc => uc.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Property(uc=>uc.Role).IsRequired();
        }
    }
}
