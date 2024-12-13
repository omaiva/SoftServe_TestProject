using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using SoftServe_TestProject.API.DTOs;
using SoftServe_TestProject.API.Responses;
using SoftServe_TestProject.Application.Interfaces;
using SoftServe_TestProject.Application.Requests;

namespace SoftServe_TestProject.API.Controllers
{
    [ApiController]
    [Route("api/courses")]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _courseService;
        private readonly IValidator<CourseDTO> _courseValidator;
        private readonly IMapper _mapper;

        public CoursesController(ICourseService courseService, IValidator<CourseDTO> courseValidator, IMapper mapper)
        {
            _courseService = courseService;
            _courseValidator = courseValidator;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets the course by id
        /// </summary>
        /// <returns>The course entity</returns>
        /// <response code="200">Returns the course entity</response>
        /// <response code="404">The entity was not found</response>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var course = await _courseService.GetCourseByIdAsync(id);
            if (course == null)
            {
                return NotFound(new { Error = "Course not found." } );
            }

            var courseResponse = _mapper.Map<CourseResponse>(course);

            return Ok(courseResponse);
        }

        /// <summary>
        /// Gets the list of all courses
        /// </summary>
        /// <returns>The list of courses</returns>
        /// <response code="200">Returns the list of courses</response>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var courses = await _courseService.GetAllCoursesAsync();

            var courseResponses = _mapper.Map<IEnumerable<CourseResponse>>(courses);

            return Ok(courseResponses);
        }

        /// <summary>
        /// Creates the new course
        /// </summary>
        /// <returns>No content</returns>
        /// <response code="204">The course was successfully created</response>
        /// <response code="400">The entity was not valid</response>
        [HttpPost]
        public async Task<IActionResult> Create(CourseDTO courseDTO)
        {
            var validationResult = await _courseValidator.ValidateAsync(courseDTO);
            if (!validationResult.IsValid)
            {
                return BadRequest(new { Error = validationResult.Errors.Select(e => e.ErrorMessage) });
            }

            var course = _mapper.Map<CourseRequest>(courseDTO);
            await _courseService.CreateCourseAsync(course);

            return NoContent();
        }

        /// <summary>
        /// Updates the existing course
        /// </summary>
        /// <returns>No content</returns>
        /// <response code="204">The course was successfully updated</response>
        /// <response code="400">The course was not valid</response>
        /// <response code="400">Id and course's id were different</response>
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
                return BadRequest(new { Error = validationResult.Errors.Select(e => e.ErrorMessage) });
            }

            var course = _mapper.Map<CourseRequest>(courseDTO);
            await _courseService.UpdateCourseAsync(course);

            return NoContent();
        }

        /// <summary>
        /// Deletes the course by id
        /// </summary>
        /// <returns>No content</returns>
        /// <response code="204">The course was successfully deleted</response>
        /// <response code="404">The entity was not found</response>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var course = await _courseService.GetCourseByIdAsync(id);
            if (course == null)
            {
                return NotFound(new { Error = "Course not found." } );
            }

            await _courseService.DeleteCourseAsync(id);

            return NoContent();
        }
    }
}
