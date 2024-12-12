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
    [Route("api/teachers")]
    public class TeachersController : ControllerBase
    {
        private readonly TeacherService _teacherService;
        private readonly IValidator<TeacherDTO> _teacherValidator;
        private readonly IMapper _mapper;

        public TeachersController(TeacherService teacherService, IValidator<TeacherDTO> teacherValidator, IMapper mapper)
        {
            _teacherService = teacherService;
            _teacherValidator = teacherValidator;
            _mapper = mapper;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var teacher = await _teacherService.GetTeacherByIdAsync(id);
            if (teacher == null)
            {
                return NotFound("Teacher not found");
            }

            var teacherResponse = _mapper.Map<TeacherResponse>(teacher);

            return Ok(teacherResponse);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var teachers = await _teacherService.GetAllTeachersAsync();

            var teacherResponses = _mapper.Map<IEnumerable<TeacherResponse>>(teachers);

            return Ok(teacherResponses);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TeacherDTO teacherDTO)
        {
            var validationResult = await _teacherValidator.ValidateAsync(teacherDTO);
            if (!validationResult.IsValid)
            {
                return BadRequest(new { Errors = validationResult.Errors.Select(e => e.ErrorMessage) });
            }

            var teacher = _mapper.Map<TeacherRequest>(teacherDTO);
            await _teacherService.CreateTeacherAsync(teacher);

            return NoContent();
        }

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
                return BadRequest(new { Errors = validationResult.Errors.Select(e => e.ErrorMessage) });
            }

            var teacher = _mapper.Map<TeacherRequest>(teacherDTO);
            await _teacherService.UpdateTeacherAsync(teacher);

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _teacherService.DeleteTeacherAsync(id);

            return NoContent();
        }
    }
}
