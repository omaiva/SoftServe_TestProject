using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SoftServe_TestProject.Domain.Entities;

namespace SoftServe_TestProject.Data.Configurations
{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Title)
                .HasMaxLength(20)
                .IsRequired();

            builder.HasIndex(c => c.Title)
                .IsUnique();

            builder.Property(c => c.Description)
                .HasMaxLength(50);

            builder.HasOne(c => c.Teacher)
                .WithMany(c => c.Courses)
                .HasForeignKey(c => c.TeacherId);

            builder.HasMany(c => c.Students)
                .WithMany(c => c.Courses);
        }
    }
}
