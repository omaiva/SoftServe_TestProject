using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using SoftServe_TestProject.API.DTOs;
using SoftServe_TestProject.API.Responses;
using SoftServe_TestProject.Application.Requests;
using SoftServe_TestProject.Application.Services;

namespace SoftServe_TestProject.API.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {
        private readonly StudentService _studentService;
        private readonly IValidator<StudentDTO> _studentValidator;
        private readonly IMapper _mapper;

        public StudentsController(StudentService studentService, IValidator<StudentDTO> studentValidator, IMapper mapper)
        {
            _studentService = studentService;
            _studentValidator = studentValidator;
            _mapper = mapper;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var student = await _studentService.GetStudentByIdAsync(id);
            if (student == null)
            {
                return NotFound("Student not found.");
            }

            var studentResponse = _mapper.Map<StudentResponse>(student);

            return Ok(studentResponse);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var students = await _studentService.GetAllStudentsAsync();

            var studentsResponses = _mapper.Map<IEnumerable<StudentResponse>>(students);

            return Ok(studentsResponses);
        }

        [HttpPost]
        public async Task<IActionResult> Create(StudentDTO studentDTO)
        {
            var validationResult = await _studentValidator.ValidateAsync(studentDTO);
            if (!validationResult.IsValid)
            {
                return BadRequest(new { Errors = validationResult.Errors.Select(e => e.ErrorMessage) });
            }

            var student = _mapper.Map<StudentRequest>(studentDTO);
            await _studentService.CreateStudentAsync(student);

            return NoContent();
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, StudentDTO studentDTO)
        {
            if (id != studentDTO.Id)
            {
                return BadRequest(new { Error = "Id and student Id are different." });
            }

            var validationResult = await _studentValidator.ValidateAsync(studentDTO);
            if (!validationResult.IsValid)
            {
                return BadRequest(new { Errors = validationResult.Errors.Select(e => e.ErrorMessage) });
            }

            var student = _mapper.Map<StudentRequest>(studentDTO);

            await _studentService.UpdateStudentAsync(student);

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _studentService.DeleteStudentAsync(id);

            return NoContent();
        }
    }
}
