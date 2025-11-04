using Microsoft.EntityFrameworkCore;
using MasterCategoryAPI.Data;
using MasterCategoryAPI.Repositories;
using MasterCategoryAPI.Services;
using UserMasterCategory.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

var app = builder.Build();

app.UseAuthorization();
app.MapControllers();

app.Run();