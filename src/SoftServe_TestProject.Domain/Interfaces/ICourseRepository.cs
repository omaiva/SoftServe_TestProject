using SoftServe_TestProject.Domain.Entities;

namespace SoftServe_TestProject.Domain.Interfaces
{
    public interface ICourseRepository
    {
        Task<IEnumerable<Course>> GetAllAsync();
        Task<Course?> GetByIdAsync(int id);
        Task AddAsync(Course course);
        void Update(Course course);
        void Delete(Course course);
    }
}
