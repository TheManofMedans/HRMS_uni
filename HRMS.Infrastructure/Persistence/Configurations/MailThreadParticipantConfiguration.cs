using HRMS.domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Persistence.Configurations
{
    public class MailThreadParticipantConfiguration : IEntityTypeConfiguration<MailThreadParticipant>
    {
        public void Configure(EntityTypeBuilder<MailThreadParticipant> builder)
        {
            builder.ToTable("MailThreadParticipants");
            builder.HasKey(mtp => new { mtp.ThreadId, mtp.UserId });
            builder.HasOne(mtp => mtp.Thread)
                .WithMany(mt => mt.Participants)
                .HasForeignKey(mt => mt.ThreadId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(mtp => mtp.User)
                .WithMany()
                .HasForeignKey(mt => mt.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
