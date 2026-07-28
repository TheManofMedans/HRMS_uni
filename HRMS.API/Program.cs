using Microsoft.EntityFrameworkCore;
using HRMS.Infrastructure.Persistence;
using HRMS.API.Middleware;
using HRMS.API.Extensions;
using FluentValidation;
using FluentValidation.AspNetCore;
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

app.UseAuthorization();

app.MapControllers();

app.Run();
