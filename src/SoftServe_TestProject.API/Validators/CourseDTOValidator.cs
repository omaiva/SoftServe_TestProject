using FluentValidation;
using SoftServe_TestProject.API.DTOs;

namespace SoftServe_TestProject.API.Validators
{
    public class CourseDTOValidator : AbstractValidator<CourseDTO>
    {
        public CourseDTOValidator()
        {
            RuleFor(c => c.Title)
                .NotEmpty()
                .MaximumLength(20);

            RuleFor(c => c.Description)
                .MaximumLength(50);

            RuleFor(c => c.TeacherId)
                .NotEmpty();
        }
    }
}
