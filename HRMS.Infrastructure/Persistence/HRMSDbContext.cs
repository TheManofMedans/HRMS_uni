using System;
using Microsoft.EntityFrameworkCore;
using HRMS.domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace HRMS.Infrastructure.Persistence
{
    public class HRMSDbContext : IdentityDbContext<User,IdentityRole<int>,int>
    {
        public HRMSDbContext(DbContextOptions<HRMSDbContext> options): base(options) 
        { 
        }
        public DbSet<Company> Companies => Set<Company>();
        public DbSet<UserCompany> UserCompanies => Set<UserCompany>();
        public DbSet<Department> Departments => Set<Department>();
        public DbSet<Employee> Employees => Set<Employee>();
        public DbSet<EmployeeDepartment> EmployeeDepartments => Set<EmployeeDepartment>();
        public DbSet<Shift> Shifts => Set<Shift>();
        public DbSet<Request> Requests => Set<Request>();
        public DbSet<Attendance> Attendances => Set<Attendance>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(HRMSDbContext).Assembly);
        }
    }
}
