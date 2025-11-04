using Microsoft.EntityFrameworkCore;  // ✅ ADD THIS
using TaskManager.Data;
using TaskManager.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database Configuration
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))); // ✅ NOW THIS WILL WORK

// Service Registration
builder.Services.AddScoped<ITaskService, TaskService>();

var app = builder.Build();

// Configure the HTTP pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Database Migration
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
    Console.WriteLine("✅ PostgreSQL Database created with professional schema!");
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

Console.WriteLine(" Task Manager API Started!");
Console.WriteLine(" Swagger Documentation: /swagger");
Console.WriteLine("Database: PostgreSQL with advanced schema");

app.Run();