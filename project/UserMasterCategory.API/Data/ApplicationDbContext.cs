using Microsoft.EntityFrameworkCore;
using UserMasterCategory.API.Models;

namespace UserMasterCategory.API.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<UserMaster> UserMasters { get; set; }  // ✅ Updated to UserMaster

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.HasDefaultSchema("UserMasterCategorySCH");
        
        modelBuilder.Entity<UserMaster>(entity =>  // ✅ Updated to UserMaster
        {
            entity.ToTable("UserMasterCategoryTBL");  // ✅ Updated table name

            entity.HasKey(e => e.id);

            entity.Property(e => e.id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.name)
                .IsRequired()
                .HasColumnName("name")
                .HasMaxLength(100);

            entity.Property(e => e.is_active)
                .HasColumnName("is_active")
                .HasDefaultValue(true);

            entity.Property(e => e.description)
                .HasColumnName("description")
                .HasMaxLength(200);

            entity.Property(e => e.phone_number)
                .IsRequired()
                .HasColumnName("phone_number")
                .HasMaxLength(15);

            entity.Property(e => e.created_at)
                .HasColumnName("created_at")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.Property(e => e.updated_at)
                .HasColumnName("updated_at")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasIndex(e => e.name)
                .HasDatabaseName("idx_UserMasterTBL_name");

            entity.HasIndex(e => e.is_active)
                .HasDatabaseName("idx_UserMasterTBL_is_active");

            entity.HasIndex(e => e.created_at)
                .HasDatabaseName("idx_UserMasterTBL_created_at");

            entity.HasIndex(e => e.phone_number)
                .HasDatabaseName("idx_UserMasterTBL_phone_number")
                .IsUnique();
        });
    }
}