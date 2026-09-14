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
    public class MailThreadConfiguration : IEntityTypeConfiguration<MailThread>
    {
        public void Configure(EntityTypeBuilder<MailThread> builder)
        {
            builder.ToTable("MailThreads");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Subject).IsRequired().HasMaxLength(200);
            builder.HasOne(mt => mt.Company)
                .WithMany()
                .HasForeignKey(mt => mt.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(mt => mt.CreatedByUser)
                .WithMany()
                .HasForeignKey(mt => mt.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(mt => mt.RelatedRequest)
                .WithMany()
                .HasForeignKey(mt => mt.RelatedRequestId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
