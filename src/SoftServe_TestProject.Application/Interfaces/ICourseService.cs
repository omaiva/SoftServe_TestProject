using AutoMapper;
using SoftServe_TestProject.Application.Requests;
using SoftServe_TestProject.Domain.Interfaces;

namespace SoftServe_TestProject.Application.Interfaces
{
    public interface ICourseService
    {
        Task<CourseRequest?> GetCourseByIdAsync(int id);
        Task<IEnumerable<CourseRequest>> GetAllCoursesAsync();
        Task CreateCourseAsync(CourseRequest courseRequest);
        Task UpdateCourseAsync(CourseRequest courseRequest);
        Task DeleteCourseAsync(int id);
    }
}
