using Microsoft.AspNetCore.Mvc;
using SoftServe_TestProject.API.DTOs;
using SoftServe_TestProject.Application.Services;
using SoftServe_TestProject.Domain.Entities;

namespace SoftServe_TestProject.API.Controllers
{
    [ApiController]
    [Route("api/teachers")]
    public class TeachersController : ControllerBase
    {
        private readonly TeacherService _teacherService;

        public TeachersController(TeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var teacher = await _teacherService.GetTeacherByIdAsync(id);
            if (teacher == null)
            {
                return NotFound();
            }

            return Ok(teacher);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var teachers = await _teacherService.GetAllTeachersAsync();

            return Ok(teachers);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TeacherDTO teacherDTO)
        {
            var teacher = new Teacher()
            {
                FirstName = teacherDTO.FirstName,
                LastName = teacherDTO.LastName
            };

            await _teacherService.CreateTeacherAsync(teacher);

            return CreatedAtAction(nameof(GetById), new { Id = teacher.Id }, teacher);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, TeacherDTO teacherDTO)
        {
            var teacher = new Teacher()
            {
                FirstName = teacherDTO.FirstName,
                LastName = teacherDTO.LastName
            };

            await _teacherService.UpdateTeacherAsync(teacher);

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _teacherService.DeleteTeacherAsync(id);

                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Teacher not found.");
            }
            catch (Exception)
            {
                return BadRequest("Something went wrong.");
            }
        }
    }
}
