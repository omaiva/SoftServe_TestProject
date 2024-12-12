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

        public StudentsController(StudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var student = await _studentService.GetStudentByIdAsync(id);
            if (student == null)
            {
                return NotFound();
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
            if (ModelState.IsValid)
            {
                var student = new Student()
                {
                    FirstName = studentDTO.FirstName,
                    LastName = studentDTO.LastName
                };

                await _studentService.CreateStudentAsync(student);

                return Ok(student);
            }

            return BadRequest();
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, StudentDTO studentDTO)
        {
            if (ModelState.IsValid)
            {
                var student = new Student()
                {
                    Id = id,
                    FirstName = studentDTO.FirstName,
                    LastName = studentDTO.LastName
                };

                await _studentService.UpdateStudentAsync(student);

                return Ok(student);
            }

            return BadRequest();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _studentService.DeleteStudentAsync(id);

            return Ok();
        }
    }
}
