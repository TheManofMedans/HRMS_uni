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
    public class PaySlipConfiguration :IEntityTypeConfiguration<PaySlip>
    {
        public void Configure(EntityTypeBuilder<PaySlip> builder)
        {
            builder.ToTable("PaySlip");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.WeekStart).IsRequired();
            builder.Property(p => p.WeekEnd).IsRequired();
            builder.Property(p => p.BasePay).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(p => p.OvertimePay).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(p => p.NetPay).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(p => p.AbsenceDeduction).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(p => p.UnpaidLeaveDeduction).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(p => p.LateDeduction).IsRequired().HasColumnType("decimal(18,2)");
            builder.HasIndex(p => new { p.EmployeeId, p.DepartmentId, p.WeekStart, p.WeekEnd }).IsUnique();
            builder.HasOne<Employee>(p => p.Employee)
                .WithMany()
                .HasForeignKey(p => p.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<Department>(p => p.Department)
                .WithMany()
                .HasForeignKey(p => p.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
