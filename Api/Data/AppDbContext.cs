using Microsoft.EntityFrameworkCore;

namespace Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    // Map your models to database tables
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<ToDoTask> ToDoTasks { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().ToTable("users");
        modelBuilder.Entity<ToDoTask>().ToTable("todo_tasks");

        modelBuilder
            .Entity<User>()
            .HasMany(u => u.ToDoTasks)
            .WithOne(t => t.User)
            .HasForeignKey(t => t.UserId)
            .IsRequired();

        base.OnModelCreating(modelBuilder);
    }
}
