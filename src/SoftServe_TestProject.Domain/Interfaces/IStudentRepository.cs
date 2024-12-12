using SoftServe_TestProject.Domain.Entities;

namespace SoftServe_TestProject.Domain.Interfaces
{
    public interface IStudentRepository
    {
        Task<IEnumerable<Student>> GetAllAsync();
        Task<Student?> GetByIdAsync(int id);
        Task AddAsync(Student student);
        void Update(Student student);
        void Delete(Student student);
    }
}
