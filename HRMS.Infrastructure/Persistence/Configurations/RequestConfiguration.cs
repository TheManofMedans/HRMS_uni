using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HRMS.domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Persistence.Configurations
{
    public class RequestConfiguration : IEntityTypeConfiguration<Request>
    {
        public void Configure(EntityTypeBuilder<Request> builder) 
        {
            builder.ToTable("Request");
            builder.HasKey(r => r.Id);
            builder.Property(r => r.description).HasMaxLength(1000);
            builder.Property(r => r.Type).IsRequired();
            builder.Property(r => r.Status).IsRequired();
        }
    }
}
