using Microsoft.EntityFrameworkCore;
using TeamTasks.Domain.Entities;

namespace TeamTasks.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Developer> Developers => Set<Developer>();
        public DbSet<Task> Tasks => Set<Task>();
        public DbSet<TaskItem> TaskItems => Set<TaskItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Developer>().ToTable("developers");
            modelBuilder.Entity<Project>().ToTable("projects");
            modelBuilder.Entity<TaskItem>().ToTable("tasks");
        }
    }
}
