using AutoMapper;
using SoftServe_TestProject.Application.Interfaces;
using SoftServe_TestProject.Application.Requests;
using SoftServe_TestProject.Domain.Entities;
using SoftServe_TestProject.Domain.Interfaces;

namespace SoftServe_TestProject.Application.Services
{
    public class StudentService : IStudentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public StudentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<StudentRequest?> GetStudentByIdAsync(int id)
        {
            var student = await _unitOfWork.Students.GetByIdAsync(id);

            return student == null ? null : _mapper.Map<StudentRequest>(student);
        }

        public async Task<IEnumerable<StudentRequest>> GetAllStudentsAsync()
        {
            var students = await _unitOfWork.Students.GetAllAsync();

            return _mapper.Map<IEnumerable<StudentRequest>>(students);
        }

        public async Task CreateStudentAsync(StudentRequest studentRequest)
        {
            var student = _mapper.Map<Student>(studentRequest);

            await _unitOfWork.Students.AddAsync(student);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateStudentAsync(StudentRequest studentRequest)
        {
            var existingStudent = await _unitOfWork.Students.GetByIdAsync(studentRequest.Id);
            if (existingStudent == null)
            {
                throw new KeyNotFoundException("Student not found.");
            }

            var student = _mapper.Map<Student>(studentRequest);

            await _unitOfWork.Students.UpdateAsync(student);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteStudentAsync(int id)
        {
            var student = await _unitOfWork.Students.GetByIdAsync(id);
            if (student == null)
            {
                throw new KeyNotFoundException("Student not found.");
            }

            await _unitOfWork.Students.DeleteAsync(student);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
