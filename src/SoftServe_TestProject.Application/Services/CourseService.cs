using SoftServe_TestProject.Domain.Entities;
using SoftServe_TestProject.Domain.Interfaces;

namespace SoftServe_TestProject.Application.Services
{
    public class CourseService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CourseService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Course?> GetCourseByIdAsync(int id)
        {
            return await _unitOfWork.Courses.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Course>> GetAllCoursesAsync()
        {
            return await _unitOfWork.Courses.GetAllAsync();
        }

        public async Task CreateCourseAsync(Course course)
        {
            var teacher = await _unitOfWork.Teachers
                .GetByIdAsync(course.TeacherId);
            if (teacher == null)
            {
                throw new ArgumentException("Invalid TeacherId");
            }

            await _unitOfWork.Courses.AddAsync(course);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateCourseAsync(Course course)
        {
            var teacher = await _unitOfWork.Teachers
                .GetByIdAsync(course.TeacherId);
            if (teacher == null)
            {
                throw new ArgumentException("Invalid TeacherId");
            }

            _unitOfWork.Courses.Update(course);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteCourseAsync(int id)
        {
            var course = await _unitOfWork.Courses.GetByIdAsync(id);
            if (course == null)
            {
                throw new KeyNotFoundException("Course not found.");
            }

            _unitOfWork.Courses.Delete(course);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
