using FluentValidation;
using SoftServe_TestProject.API.DTOs;

namespace SoftServe_TestProject.API.Validators
{
    public class TeacherDTOValidator : AbstractValidator<TeacherDTO>
    {
        public TeacherDTOValidator()
        {
            RuleFor(s => s.FirstName)
                .NotEmpty()
                .MaximumLength(20);

            RuleFor(s => s.LastName)
                .NotEmpty()
                .MaximumLength(20);
        }
    }
}
