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
    [Route("api/teachers")]
    public class TeachersController : ControllerBase
    {
        private readonly ITeacherService _teacherService;
        private readonly IValidator<TeacherDTO> _teacherValidator;
        private readonly IMapper _mapper;

        public TeachersController(ITeacherService teacherService, IValidator<TeacherDTO> teacherValidator, IMapper mapper)
        {
            _teacherService = teacherService;
            _teacherValidator = teacherValidator;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets the teacher by id
        /// </summary>
        /// <returns>The teacher entity</returns>
        /// <response code="200">Returns the teacher entity</response>
        /// <response code="404">The entity was not found</response>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var teacher = await _teacherService.GetTeacherByIdAsync(id);
            if (teacher == null)
            {
                return NotFound(new { Error = "Teacher not found" } );
            }

            var teacherResponse = _mapper.Map<TeacherResponse>(teacher);

            return Ok(teacherResponse);
        }

        /// <summary>
        /// Gets the list of all teachers
        /// </summary>
        /// <returns>The list of teachers</returns>
        /// <response code="200">Returns the list of teachers</response>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var teachers = await _teacherService.GetAllTeachersAsync();

            var teacherResponses = _mapper.Map<IEnumerable<TeacherResponse>>(teachers);

            return Ok(teacherResponses);
        }

        /// <summary>
        /// Creates the new teacher
        /// </summary>
        /// <returns>No content</returns>
        /// <response code="204">The teacher was successfully created</response>
        /// <response code="400">The entity was not valid</response>
        [HttpPost]
        public async Task<IActionResult> Create(TeacherDTO teacherDTO)
        {
            var validationResult = await _teacherValidator.ValidateAsync(teacherDTO);
            if (!validationResult.IsValid)
            {
                return BadRequest(new { Error = validationResult.Errors.Select(e => e.ErrorMessage) });
            }

            var teacher = _mapper.Map<TeacherRequest>(teacherDTO);
            await _teacherService.CreateTeacherAsync(teacher);

            return NoContent();
        }

        /// <summary>
        /// Updates the existing teacher
        /// </summary>
        /// <returns>No content</returns>
        /// <response code="204">The teacher was successfully updated</response>
        /// <response code="400">The teacher was not valid</response>
        /// <response code="400">Id and teacher's id were different</response>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, TeacherDTO teacherDTO)
        {
            if (id != teacherDTO.Id)
            {
                return BadRequest(new { Error = "Id and teacher Id are different." });
            }

            var validationResult = await _teacherValidator.ValidateAsync(teacherDTO);
            if (!validationResult.IsValid)
            {
                return BadRequest(new { Error = validationResult.Errors.Select(e => e.ErrorMessage) });
            }

            var teacher = _mapper.Map<TeacherRequest>(teacherDTO);
            await _teacherService.UpdateTeacherAsync(teacher);

            return NoContent();
        }

        /// <summary>
        /// Deletes the student by id
        /// </summary>
        /// <returns>No content</returns>
        /// <response code="204">The student was successfully deleted</response>
        /// <response code="404">The entity was not found</response>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var teacher = await _teacherService.GetTeacherByIdAsync(id);
            if (teacher == null)
            {
                return NotFound(new { Error = "Teacher not found" } );
            }

            await _teacherService.DeleteTeacherAsync(id);

            return NoContent();
        }
    }
}
