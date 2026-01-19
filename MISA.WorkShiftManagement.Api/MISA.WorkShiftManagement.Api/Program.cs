using MISA.WorkShiftManagement.Core.Entities;
using MISA.WorkShiftManagement.Core.Interfaces.Repositories;
using MISA.WorkShiftManagement.Core.Interfaces.Services;
using MISA.WorkShiftManagement.Core.Middleware;
using MISA.WorkShiftManagement.Core.Services;
using MISA.WorkShiftManagement.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// AutoMapper use Dapper
Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

// Register DI
builder.Services.AddScoped<IWorkShiftRepository, WorkShiftRepository>();
builder.Services.AddScoped<IWorkShiftService, WorkShiftService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy
                .WithOrigins("http://localhost:5173")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseAuthorization();

// Customer Middleware
app.UseMiddleware<MISAErrorExceptionMiddleware>();

app.MapControllers();

app.Run();