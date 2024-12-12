using AutoMapper;
using SoftServe_TestProject.Application.Requests;
using SoftServe_TestProject.Domain.Entities;
using SoftServe_TestProject.Domain.Interfaces;

namespace SoftServe_TestProject.Application.Services
{
    public class TeacherService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TeacherService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<TeacherRequest?> GetTeacherByIdAsync(int id)
        {
            var teacher = await _unitOfWork.Teachers.GetByIdAsync(id);

            return teacher == null ? null : _mapper.Map<TeacherRequest>(teacher);
        }

        public async Task<IEnumerable<TeacherRequest>> GetAllTeachersAsync()
        {
            var teachers = await _unitOfWork.Teachers.GetAllAsync();

            return _mapper.Map<IEnumerable<TeacherRequest>>(teachers);
        }

        public async Task CreateTeacherAsync(TeacherRequest teacherRequest)
        {
            var teacher = _mapper.Map<Teacher>(teacherRequest);

            await _unitOfWork.Teachers.AddAsync(teacher);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateTeacherAsync(TeacherRequest teacherRequest)
        {
            var existingTeacher = await _unitOfWork.Teachers.GetByIdAsync(teacherRequest.Id);
            if (existingTeacher == null)
            {
                throw new KeyNotFoundException("Teacher not found.");
            }

            var teacher = _mapper.Map<Teacher>(teacherRequest);

            await _unitOfWork.Teachers.UpdateAsync(teacher);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteTeacherAsync(int id)
        {
            var teacher = await _unitOfWork.Teachers.GetByIdAsync(id);
            if (teacher == null)
            {
                throw new KeyNotFoundException("Teacher not found.");
            }

            await _unitOfWork.Teachers.DeleteAsync(teacher);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
