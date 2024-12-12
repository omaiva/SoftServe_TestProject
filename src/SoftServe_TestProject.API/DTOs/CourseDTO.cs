using System.ComponentModel.DataAnnotations;

namespace SoftServe_TestProject.API.DTOs
{
    public record CourseDTO()
    {
        [Required]
        [MaxLength(20)]
        public string Title { get; init; }
        [MaxLength(50)]
        public string Description { get; init; }
        [Required]
        public int TeacherId { get; init; }
    }
}
