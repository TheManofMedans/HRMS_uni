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
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable("Notification");
            builder.HasKey(n => n.Id);
            builder.Property(n => n.Type).IsRequired();
            builder.Property(n => n.Message).HasMaxLength(256);
            builder.Property(n => n.CreatedAt).IsRequired();

            builder.HasOne(n => n.User)
                .WithMany()
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(n => n.Attendance)
                .WithMany()
                .HasForeignKey(n => n.AttendanceId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(n => n.Request)
                .WithMany()
                .HasForeignKey(n => n.RequestId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasIndex(n => new {n.UserId,n.isRead});
        }
    }
}
