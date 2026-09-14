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
    public class MailMessageConfiguration : IEntityTypeConfiguration<MailMessage>
    {
        public void Configure(EntityTypeBuilder<MailMessage> builder)
        {
            builder.ToTable("MailMessages");
            builder.HasKey(mm => mm.Id);
            builder.Property(mm => mm.Body).IsRequired().HasMaxLength(4000);
            builder.HasOne(mm => mm.Thread)
                .WithMany(t => t.Messages)
                .HasForeignKey(mm => mm.ThreadId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(mm => mm.Sender)
                .WithMany()
                .HasForeignKey(mm => mm.SenderUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
