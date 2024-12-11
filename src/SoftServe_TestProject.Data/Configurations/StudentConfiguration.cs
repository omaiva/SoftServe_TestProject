using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SoftServe_TestProject.Domain.Entities;

namespace SoftServe_TestProject.Data.Configurations
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.FirstName)
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(s => s.LastName)
                .HasMaxLength(20)
                .IsRequired();

            builder.HasMany(s => s.Courses)
                .WithMany(s => s.Students);

            builder.HasMany(s => s.Teachers)
                .WithMany(s => s.Students);
        }
    }
}
