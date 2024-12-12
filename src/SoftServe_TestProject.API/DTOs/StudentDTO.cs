using System.ComponentModel.DataAnnotations;

namespace SoftServe_TestProject.API.DTOs
{
    public record StudentDTO()
    {
        [Required]
        [MaxLength(20)]
        public string FirstName { get; init; }

        [Required]
        [MaxLength(20)]
        public string LastName { get; init; }
    }
}
