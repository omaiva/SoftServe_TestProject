using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using SoftServe_TestProject.API.DTOs;
using SoftServe_TestProject.API.Validators;
using SoftServe_TestProject.Application.Services;
using SoftServe_TestProject.Domain.Entities;

namespace SoftServe_TestProject.API.Controllers
{
    [ApiController]
    [Route("api/courses")]
    public class CoursesController : ControllerBase
    {
        private readonly CourseService _courseService;
        private readonly IValidator<CourseDTO> _courseValidator;

        public CoursesController(CourseService courseService, IValidator<CourseDTO> courseValidator)
        {
            _courseService = courseService;
            _courseValidator = courseValidator;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var course = await _courseService.GetCourseByIdAsync(id);
            if (course == null)
            {
                return NotFound("Course not found.");
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
            var validationResult = await _courseValidator.ValidateAsync(courseDTO);
            if (!validationResult.IsValid)
            {
                return BadRequest(new { Errors = validationResult.Errors.Select(e => e.ErrorMessage) });
            }

            var course = new Course()
            {
                Title = courseDTO.Title,
                Description = courseDTO.Description,
                TeacherId = courseDTO.TeacherId 
            };

            await _courseService.CreateCourseAsync(course);

            return CreatedAtAction(nameof(GetById), new { Id = course.Id }, course);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, CourseDTO courseDTO)
        {
            var validationResult = await _courseValidator.ValidateAsync(courseDTO);
            if (!validationResult.IsValid)
            {
                return BadRequest(new { Errors = validationResult.Errors.Select(e => e.ErrorMessage) });
            }

            var course = new Course()
            {
                Title = courseDTO.Title,
                Description = courseDTO.Description,
                TeacherId = courseDTO.TeacherId
            };

            await _courseService.UpdateCourseAsync(course);

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _courseService.DeleteCourseAsync(id);

                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Course not found.");
            }
            catch (Exception)
            {
                return BadRequest("Something went wrong.");
            }
        }
    }
}
