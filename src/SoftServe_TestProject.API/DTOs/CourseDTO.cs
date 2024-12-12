using System.ComponentModel.DataAnnotations;

namespace SoftServe_TestProject.API.DTOs
{
    public record CourseDTO(string Title, string Description, int TeacherId) { }
}
