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
    [Route("api/courses")]
    public class CoursesController : ControllerBase
    {
        private readonly CourseService _courseService;
        private readonly IValidator<CourseDTO> _courseValidator;
        private readonly IMapper _mapper;

        public CoursesController(CourseService courseService, IValidator<CourseDTO> courseValidator, IMapper mapper)
        {
            _courseService = courseService;
            _courseValidator = courseValidator;
            _mapper = mapper;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var course = await _courseService.GetCourseByIdAsync(id);
            if (course == null)
            {
                return NotFound("Course not found.");
            }

            var courseResponse = _mapper.Map<CourseResponse>(course);

            return Ok(courseResponse);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var courses = await _courseService.GetAllCoursesAsync();

            var courseResponses = _mapper.Map<IEnumerable<CourseResponse>>(courses);

            return Ok(courseResponses);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CourseDTO courseDTO)
        {
            var validationResult = await _courseValidator.ValidateAsync(courseDTO);
            if (!validationResult.IsValid)
            {
                return BadRequest(new { Errors = validationResult.Errors.Select(e => e.ErrorMessage) });
            }

            var course = _mapper.Map<CourseRequest>(courseDTO);
            await _courseService.CreateCourseAsync(course);

            return NoContent();
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, CourseDTO courseDTO)
        {
            if (id != courseDTO.Id)
            {
                return BadRequest(new { Error = "Id and course Id are different." });
            }

            var validationResult = await _courseValidator.ValidateAsync(courseDTO);
            if (!validationResult.IsValid)
            {
                return BadRequest(new { Errors = validationResult.Errors.Select(e => e.ErrorMessage) });
            }

            var course = _mapper.Map<CourseRequest>(courseDTO);
            await _courseService.UpdateCourseAsync(course);

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _courseService.DeleteCourseAsync(id);

            return NoContent();
        }
    }
}
