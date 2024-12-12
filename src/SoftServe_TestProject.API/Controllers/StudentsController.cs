using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using SoftServe_TestProject.API.DTOs;
using SoftServe_TestProject.Application.Services;
using SoftServe_TestProject.Domain.Entities;

namespace SoftServe_TestProject.API.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {
        private readonly StudentService _studentService;
        private readonly IValidator<StudentDTO> _studentValidator;

        public StudentsController(StudentService studentService, IValidator<StudentDTO> studentValidator)
        {
            _studentService = studentService;
            _studentValidator = studentValidator;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var student = await _studentService.GetStudentByIdAsync(id);
            if (student == null)
            {
                return NotFound("Student not found.");
            }

            return Ok(student);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var students = await _studentService.GetAllStudentsAsync();

            return Ok(students);
        }

        [HttpPost]
        public async Task<IActionResult> Create(StudentDTO studentDTO)
        {
            var validationResult = await _studentValidator.ValidateAsync(studentDTO);
            if (!validationResult.IsValid)
            {
                return BadRequest(new { Errors = validationResult.Errors.Select(e => e.ErrorMessage) });
            }

            var student = new Student()
            {
                FirstName = studentDTO.FirstName,
                LastName = studentDTO.LastName
            };

            await _studentService.CreateStudentAsync(student);

            return CreatedAtAction(nameof(GetById), new { Id = student.Id}, student);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, StudentDTO studentDTO)
        {
            var validationResult = await _studentValidator.ValidateAsync(studentDTO);
            if (!validationResult.IsValid)
            {
                return BadRequest(new { Errors = validationResult.Errors.Select(e => e.ErrorMessage) });
            }

            var student = new Student()
            {
                Id = id,
                FirstName = studentDTO.FirstName,
                LastName = studentDTO.LastName
            };

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
