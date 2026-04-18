using Microsoft.EntityFrameworkCore;

namespace Solster.AspNetCore.ActivityLog.EntityFrameworkCore;

public class ActivityDbContext(DbContextOptions<ActivityDbContext> options) : DbContext(options)
{
    public DbSet<ActivityLogEntry> ActivityLogs => Set<ActivityLogEntry>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ActivityLogEntry>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Actor).IsRequired().HasMaxLength(255);
            entity.Property(e => e.ActorIp).HasMaxLength(45);
            entity.Property(e => e.UserAgent).HasMaxLength(500);
            entity.Property(e => e.ResourceType).HasMaxLength(100);
            entity.Property(e => e.ResourceId).HasMaxLength(100);
            entity.Property(e => e.ResourceLabel).HasMaxLength(255);

            entity.HasIndex(e => new { e.ResourceType, e.ResourceId, e.Timestamp });
            entity.HasIndex(e => new { e.Actor, e.Timestamp });
            entity.HasIndex(e => e.Timestamp);
        });
    }
}

