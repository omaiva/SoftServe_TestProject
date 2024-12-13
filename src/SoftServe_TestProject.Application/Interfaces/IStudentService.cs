using SoftServe_TestProject.Application.Requests;

namespace SoftServe_TestProject.Application.Interfaces
{
    public interface IStudentService
    {
        Task<StudentRequest?> GetStudentByIdAsync(int id);
        Task<IEnumerable<StudentRequest>> GetAllStudentsAsync();
        Task CreateStudentAsync(StudentRequest studentRequest);
        Task UpdateStudentAsync(StudentRequest studentRequest);
        Task DeleteStudentAsync(int id);
    }
}
