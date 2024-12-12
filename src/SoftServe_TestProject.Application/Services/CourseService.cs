using AutoMapper;
using SoftServe_TestProject.Application.Requests;
using SoftServe_TestProject.Domain.Entities;
using SoftServe_TestProject.Domain.Interfaces;

namespace SoftServe_TestProject.Application.Services
{
    public class CourseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CourseService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CourseRequest?> GetCourseByIdAsync(int id)
        {
            var course = await _unitOfWork.Courses.GetByIdAsync(id);

            return course == null ? null : _mapper.Map<CourseRequest>(course);
        }

        public async Task<IEnumerable<CourseRequest>> GetAllCoursesAsync()
        {
            var courses =  await _unitOfWork.Courses.GetAllAsync();
            return _mapper.Map<IEnumerable<CourseRequest>>(courses);
        }

        public async Task CreateCourseAsync(CourseRequest courseRequest)
        {
            var teacher = await _unitOfWork.Teachers
                .GetByIdAsync(courseRequest.TeacherId);
            if (teacher == null)
            {
                throw new ArgumentException("Invalid TeacherId");
            }

            var course = _mapper.Map<Course>(courseRequest);

            await _unitOfWork.Courses.AddAsync(course);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateCourseAsync(CourseRequest courseRequest)
        {
            var existingCourse = await _unitOfWork.Courses.GetByIdAsync(courseRequest.Id);
            if (existingCourse == null)
            {
                throw new KeyNotFoundException("Course not found.");
            }

            var teacher = await _unitOfWork.Teachers
                .GetByIdAsync(courseRequest.TeacherId);
            if (teacher == null)
            {
                throw new ArgumentException("Invalid TeacherId");
            }

            var course = _mapper.Map<Course>(courseRequest);

            await _unitOfWork.Courses.UpdateAsync(course);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteCourseAsync(int id)
        {
            var course = await _unitOfWork.Courses.GetByIdAsync(id);
            if (course == null)
            {
                throw new KeyNotFoundException("Course not found.");
            }

            await _unitOfWork.Courses.DeleteAsync(course);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
