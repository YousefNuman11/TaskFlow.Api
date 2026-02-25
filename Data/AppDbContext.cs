using Microsoft.EntityFrameworkCore;
using TaskFlow.Api.Models;


namespace TaskFlow.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<TaskItem> Tasks { get; set; }

    public DbSet<User> Users { get; set;}


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasMany(u => u.Tasks)
            .WithOne()
            .HasForeignKey("UserId");
            
        base.OnModelCreating(modelBuilder);
    }




}
