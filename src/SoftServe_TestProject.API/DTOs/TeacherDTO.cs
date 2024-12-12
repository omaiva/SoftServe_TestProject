using System.ComponentModel.DataAnnotations;

namespace SoftServe_TestProject.API.DTOs
{
    public record TeacherDTO()
    {
        [Required]
        [MaxLength(20)]
        public string FirstName { get; init; }

        [Required]
        [MaxLength(20)]
        public string LastName { get; init; }
    }
}
