using Microsoft.EntityFrameworkCore;
using HRMS.Infrastructure.Persistence;
using HRMS.API.Middleware;
using HRMS.API.Extensions;
using HRMS.domain.Entities;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using HRMS.API.Authentication;
using HRMS.domain.Enums;
using HRMS.API.Authorization.Resolvers;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<HRMSDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();
builder.Services.AddTransient<ExceptionHandlingMiddleware>();
builder.Services.AddApplicationServices();
builder.Services.AddInfrustructureServices();
builder.Services.AddAutoMapper(cfg => { },typeof(HRMS.Application.Mappings.AttendanceMappingProfile).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(HRMS.Application.Validators.CreateAttendanceDtoValidator).Assembly);
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddIdentity<User, IdentityRole<int>>(options =>
{
    options.Password.RequiredLength = 8;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.User.RequireUniqueEmail = true;
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
})
    .AddEntityFrameworkStores<HRMSDbContext>()
    .AddDefaultTokenProviders();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<CompanyCompanyResolver>();
builder.Services.AddScoped<DepartmentCompanyResolver>();
builder.Services.AddScoped<ShiftCompanyResolver>();
builder.Services.AddScoped<AttendanceCompanyResolver>();
builder.Services.AddScoped<RequestCompanyResolver>();
builder.Services.AddScoped<IAuthorizationHandler, CompanyRoleAuthorizationHandler>();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Company_RequireCEO", policy =>
        policy.Requirements.Add(new CompanyRoleRequirement(CompanyRole.CEO, typeof(CompanyCompanyResolver), "companyId")));
    options.AddPolicy("Company_RequireHRManager", policy =>
        policy.Requirements.Add(new CompanyRoleRequirement(CompanyRole.HRManager, typeof(CompanyCompanyResolver), "companyId")));

    options.AddPolicy("Department_RequireHRManager", policy =>
        policy.Requirements.Add(new CompanyRoleRequirement(CompanyRole.HRManager, typeof(DepartmentCompanyResolver), "departmentId")));

    options.AddPolicy("Shift_RequireHRManager", policy =>
        policy.Requirements.Add(new CompanyRoleRequirement(CompanyRole.HRManager, typeof(ShiftCompanyResolver), "shiftId")));

    options.AddPolicy("Attendance_RequireHRManager", policy =>
        policy.Requirements.Add(new CompanyRoleRequirement(CompanyRole.HRManager, typeof(AttendanceCompanyResolver), "id")));

    options.AddPolicy("Request_RequireHRManager", policy =>
        policy.Requirements.Add(new CompanyRoleRequirement(CompanyRole.HRManager, typeof(RequestCompanyResolver), "id")));
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
