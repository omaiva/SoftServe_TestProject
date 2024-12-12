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
            if (ModelState.IsValid)
            {
                var teacher = new Teacher()
                {
                    FirstName = teacherDTO.FirstName,
                    LastName = teacherDTO.LastName
                };

                await _teacherService.CreateTeacherAsync(teacher);

                return Ok(teacher);
            }

            return BadRequest();
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, TeacherDTO teacherDTO)
        {
            if (ModelState.IsValid)
            {
                var teacher = new Teacher()
                {
                    FirstName = teacherDTO.FirstName,
                    LastName = teacherDTO.LastName
                };

                await _teacherService.UpdateTeacherAsync(teacher);

                return Ok(teacher);
            }

            return BadRequest();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _teacherService.DeleteTeacherAsync(id);

            return Ok();
        }
    }
}
