using Microsoft.AspNetCore.Mvc;
using SoftServe_TestProject.API.DTOs;
using SoftServe_TestProject.Application.Services;
using SoftServe_TestProject.Domain.Entities;

namespace SoftServe_TestProject.API.Controllers
{
    [ApiController]
    [Route("api/courses")]
    public class CoursesController : ControllerBase
    {
        private readonly CourseService _courseService;
        private readonly TeacherService _teacherService;

        public CoursesController(CourseService courseService, TeacherService teacherService)
        {
            _courseService = courseService;
            _teacherService = teacherService;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var course = await _courseService.GetCourseByIdAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            return Ok(course);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var courses = await _courseService.GetAllCoursesAsync();

            return Ok(courses);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CourseDTO courseDTO)
        {
            if (ModelState.IsValid)
            {
                var teacher = await _teacherService.GetTeacherByIdAsync(courseDTO.TeacherId);
                if (teacher == null)
                {
                    return BadRequest();
                }

                var course = new Course()
                {
                    Title = courseDTO.Title,
                    Description = courseDTO.Description,
                    TeacherId = courseDTO.TeacherId
                };

                await _courseService.CreateCourseAsync(course);

                return Ok(course);
            }

            return BadRequest();
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, CourseDTO courseDTO)
        {
            if (ModelState.IsValid)
            {
                var teacher = await _teacherService.GetTeacherByIdAsync(courseDTO.TeacherId);
                if (teacher == null)
                {
                    return BadRequest();
                }

                var course = new Course()
                {
                    Title = courseDTO.Title,
                    Description = courseDTO.Description,
                    TeacherId = courseDTO.TeacherId
                };

                await _courseService.UpdateCourseAsync(course);

                return Ok(course);
            }

            return BadRequest();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _courseService.DeleteCourseAsync(id);

            return Ok();
        }
    }
}
