using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SoftServe_TestProject.Domain.Entities;

namespace SoftServe_TestProject.Data.Configurations
{
    public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
    {
        public void Configure(EntityTypeBuilder<Teacher> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.FirstName)
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(t => t.LastName)
                .HasMaxLength(20)
                .IsRequired();

            builder.HasMany(t => t.Students)
                .WithMany(t => t.Teachers);

            builder.HasMany(t => t.Courses)
                .WithOne(t => t.Teacher)
                .HasForeignKey(t => t.TeacherId);
        }
    }
}
