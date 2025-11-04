using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Models;
using TaskManager.Enums;

namespace TaskManager.Data.Configurations
{
    public class TaskItemConfiguration : IEntityTypeConfiguration<TaskItem>
    {
        public void Configure(EntityTypeBuilder<TaskItem> builder)
        {
            // Table configuration
            builder.ToTable("Tasks", "taskmanager");
            
            // Primary Key
            builder.HasKey(t => t.Id);
            
            // Properties configuration
            builder.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnType("varchar(100)");

            builder.Property(t => t.Description)
                .HasColumnType("text");

            builder.Property(t => t.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20)
                .HasColumnType("varchar(20)")
                .HasDefaultValue(TaskManager.Enums.TaskStatus.Pending);

            builder.Property(t => t.Priority)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20)
                .HasColumnType("varchar(20)")
                .HasDefaultValue(PriorityLevel.Medium);

            builder.Property(t => t.Category)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnType("varchar(50)")
                .HasDefaultValue("General");

            builder.Property(t => t.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(t => t.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(t => t.AssignedTo)
                .HasMaxLength(100)
                .HasColumnType("varchar(100)");

            builder.Property(t => t.Tags)
                .HasMaxLength(200)
                .HasColumnType("varchar(200)");

            // Indexes for better performance
            builder.HasIndex(t => t.Status)
                .HasDatabaseName("IX_Tasks_Status");

            builder.HasIndex(t => t.Priority)
                .HasDatabaseName("IX_Tasks_Priority");

            builder.HasIndex(t => t.Category)
                .HasDatabaseName("IX_Tasks_Category");

            builder.HasIndex(t => t.DueDate)
                .HasDatabaseName("IX_Tasks_DueDate");

            builder.HasIndex(t => t.CreatedAt)
                .HasDatabaseName("IX_Tasks_CreatedAt");

            builder.HasIndex(t => new { t.Status, t.Priority })
                .HasDatabaseName("IX_Tasks_Status_Priority");

            builder.HasIndex(t => new { t.Category, t.Status })
                .HasDatabaseName("IX_Tasks_Category_Status");

            // Self-referencing relationship for sub-tasks
            builder.HasOne(t => t.ParentTask)
                .WithMany(t => t.SubTasks)
                .HasForeignKey(t => t.ParentTaskId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Tasks_ParentTask");

            // Check constraints
            builder.HasCheckConstraint("CK_Tasks_DueDate", 
                @"""DueDate"" IS NULL OR ""DueDate"" >= ""CreatedAt""");

            builder.HasCheckConstraint("CK_Tasks_CompletedAt", 
                @"""CompletedAt"" IS NULL OR (""Status"" = 'Completed' AND ""CompletedAt"" >= ""CreatedAt"")");

            // Seed data
            builder.HasData(
                new TaskItem 
                { 
                    Id = 1,
                    Title = "Design Database Schema",
                    Description = "Design and implement PostgreSQL database schema for Task Manager",
                    Status = TaskManager.Enums.TaskStatus.Completed,
                    Priority = PriorityLevel.High,
                    Category = "Database",
                    CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    CompletedAt = new DateTime(2024, 1, 2, 0, 0, 0, DateTimeKind.Utc),
                    EstimatedEffort = TimeSpan.FromHours(4),
                    Tags = "database,design,postgresql"
                },
                new TaskItem 
                { 
                    Id = 2,
                    Title = "Implement REST API",
                    Description = "Create ASP.NET Core WebAPI with CRUD operations",
                    Status = TaskManager.Enums.TaskStatus.InProgress,
                    Priority = PriorityLevel.High,
                    Category = "Backend",
                    CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    DueDate = new DateTime(2024, 12, 31, 0, 0, 0, DateTimeKind.Utc),
                    EstimatedEffort = TimeSpan.FromHours(8),
                    Tags = "api,rest,aspnetcore"
                },
                new TaskItem 
                { 
                    Id = 3,
                    Title = "Add Authentication",
                    Description = "Implement JWT authentication and authorization",
                    Status = TaskManager.Enums.TaskStatus.Pending,
                    Priority = PriorityLevel.Medium,
                    Category = "Security",
                    CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    DueDate = new DateTime(2024, 12, 15, 0, 0, 0, DateTimeKind.Utc),
                    EstimatedEffort = TimeSpan.FromHours(6),
                    Tags = "auth,jwt,security"
                }
            );
        }
    }
}