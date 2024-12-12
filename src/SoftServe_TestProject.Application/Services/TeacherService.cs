using SoftServe_TestProject.Domain.Entities;
using SoftServe_TestProject.Domain.Interfaces;

namespace SoftServe_TestProject.Application.Services
{
    public class TeacherService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TeacherService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Teacher> GetTeacherByIdAsync(int id)
        {
            return await _unitOfWork.Teachers.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Teacher>> GetAllTeachersAsync()
        {
            return await _unitOfWork.Teachers.GetAllAsync();
        }

        public async Task CreateTeacherAsync(Teacher teacher)
        {
            await _unitOfWork.Teachers.AddAsync(teacher);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateTeacherAsync(Teacher teacher)
        {
            _unitOfWork.Teachers.Update(teacher);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteTeacherAsync(int id)
        {
            var teacher = await _unitOfWork.Teachers.GetByIdAsync(id);
            if (teacher == null)
            {
                throw new KeyNotFoundException("Teacher not found.");
            }

            _unitOfWork.Teachers.Delete(teacher);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
