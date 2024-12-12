using Microsoft.EntityFrameworkCore;
using SoftServe_TestProject.Domain.Entities;

namespace SoftServe_TestProject.Data
{
    public class ProjectContext : DbContext
    {
        public ProjectContext(DbContextOptions<ProjectContext> options) : base(options) { }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProjectContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
