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
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly IValidator<StudentDTO> _studentValidator;
        private readonly IMapper _mapper;

        public StudentsController(IStudentService studentService, IValidator<StudentDTO> studentValidator, IMapper mapper)
        {
            _studentService = studentService;
            _studentValidator = studentValidator;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets the student by id
        /// </summary>
        /// <returns>The student entity</returns>
        /// <response code="200">Returns the student entity</response>
        /// <response code="404">The entity was not found</response>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var student = await _studentService.GetStudentByIdAsync(id);
            if (student == null)
            {
                return NotFound(new { Error = "Student not found." } );
            }

            var studentResponse = _mapper.Map<StudentResponse>(student);

            return Ok(studentResponse);
        }

        /// <summary>
        /// Gets the list of all students
        /// </summary>
        /// <returns>The list of students</returns>
        /// <response code="200">Returns the list of students</response>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var students = await _studentService.GetAllStudentsAsync();

            var studentsResponses = _mapper.Map<IEnumerable<StudentResponse>>(students);

            return Ok(studentsResponses);
        }

        /// <summary>
        /// Creates the new student
        /// </summary>
        /// <returns>No content</returns>
        /// <response code="204">The student was successfully created</response>
        /// <response code="400">The entity was not valid</response>
        [HttpPost]
        public async Task<IActionResult> Create(StudentDTO studentDTO)
        {
            var validationResult = await _studentValidator.ValidateAsync(studentDTO);
            if (!validationResult.IsValid)
            {
                return BadRequest(new { Error = validationResult.Errors.Select(e => e.ErrorMessage) });
            }

            var student = _mapper.Map<StudentRequest>(studentDTO);
            await _studentService.CreateStudentAsync(student);

            return NoContent();
        }

        /// <summary>
        /// Updates the existing student
        /// </summary>
        /// <returns>No content</returns>
        /// <response code="204">The student was successfully updated</response>
        /// <response code="400">The student was not valid</response>
        /// <response code="400">Id and student's id were different</response>
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
                return BadRequest(new { Error = validationResult.Errors.Select(e => e.ErrorMessage) });
            }

            var student = _mapper.Map<StudentRequest>(studentDTO);

            await _studentService.UpdateStudentAsync(student);

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
            var student = await _studentService.GetStudentByIdAsync(id);
            if (student == null)
            {
                return NotFound(new { Error = "Student not found." } );
            }

            await _studentService.DeleteStudentAsync(id);

            return NoContent();
        }
    }
}
