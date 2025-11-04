using Microsoft.EntityFrameworkCore;
using UserMasterCategory.API.Data;
using UserMasterCategory.API.Data.Repositories;
using UserMasterCategory.API.Services;
using UserMasterCategory.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUserMasterCategoryRepo, UserMasterCategoryRepo>();
builder.Services.AddScoped<IUserMasterCategoryService, UserMasterCategoryService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapControllers(); 
app.LogAllEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.Run();