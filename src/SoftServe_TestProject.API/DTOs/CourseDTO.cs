using System.ComponentModel.DataAnnotations;

namespace SoftServe_TestProject.API.DTOs
{
    public record CourseDTO(int Id, string Title, string Description, int TeacherId) { }
}
