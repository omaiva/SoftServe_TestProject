using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SoftServe_TestProject.API.Controllers;
using SoftServe_TestProject.API.DTOs;
using SoftServe_TestProject.API.Responses;
using SoftServe_TestProject.API.Validators;
using SoftServe_TestProject.Application.Interfaces;
using SoftServe_TestProject.Application.Requests;
using SoftServe_TestProject.Domain.Entities;

namespace SoftServe_TestProject.Tests.ControllersTests
{
    [TestFixture]
    public class StudentsControllerTests
    {
        private StudentsController _studentsController;
        private Mock<IStudentService> _studentServiceMock;
        private Mock<IValidator<StudentDTO>> _validatorMock;
        private Mock<IMapper> _mapperMock;

        [SetUp]
        public void SetUp()
        {
            _studentServiceMock = new Mock<IStudentService>();
            _mapperMock = new Mock<IMapper>();
            _validatorMock = new Mock<IValidator<StudentDTO>>();

            _studentsController = new StudentsController(_studentServiceMock.Object, _validatorMock.Object, _mapperMock.Object);
        }

        [Test]
        public async Task GetById_ReturnsOkWithStudentResponse_WhenStudentExisis()
        {
            var studentId = 1;
            var request = new StudentRequest(studentId, "John", "Doe");
            var response = new StudentResponse(studentId, "John", "Doe");

            _studentServiceMock
                .Setup(s => s.GetStudentByIdAsync(studentId))
                .ReturnsAsync(request);
            _mapperMock
                .Setup(m => m.Map<StudentResponse>(request))
                .Returns(response);

            var result = await _studentsController.GetById(studentId);

            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.EqualTo(response));
        }

        [Test]
        public async Task GetById_ReturnsNotFoundWithError_WhenStudentDoesNotExist()
        {
            var studentId = 1;
            StudentRequest? request = null;

            _studentServiceMock
                .Setup(s => s.GetStudentByIdAsync(studentId))
                .ReturnsAsync(request);

            var result = await _studentsController.GetById(studentId);

            var notFoundResult = result as NotFoundObjectResult;
            Assert.NotNull(notFoundResult);
            Assert.That(notFoundResult.StatusCode, Is.EqualTo(404));

            var value = notFoundResult.Value;
            Assert.NotNull(value);

            var error = value.GetType().GetProperty("Error")?.GetValue(value)?.ToString();
            Assert.That(error, Is.EqualTo("Student not found."));
        }

        [Test]
        public async Task GetAll_ReturnsOkWithListOfStudents_WhenStudentsExist()
        {
            var request = new List<StudentRequest>
            {
                new StudentRequest(1, "John", "Doe"),
                new StudentRequest(2, "Jane", "Doe")
            };
            var response = new List<StudentResponse>
            {
                new StudentResponse(1, "John", "Doe"),
                new StudentResponse(2, "Jane", "Doe")
            };

            _studentServiceMock
                .Setup(s => s.GetAllStudentsAsync())
                .ReturnsAsync(request);
            _mapperMock
                .Setup(m => m.Map<IEnumerable<StudentResponse>>(request))
                .Returns(response);

            var result = await _studentsController.GetAll();

            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.EqualTo(response));
        }

        [Test]
        public async Task GetAll_ReturnsOkWithNull_WhenStudentsDoNotExist()
        {
            List<StudentRequest> request = [];
            List<StudentResponse> response = [];

            _studentServiceMock
                .Setup(s => s.GetAllStudentsAsync())
                .ReturnsAsync(request);
            _mapperMock
                .Setup(m => m.Map<IEnumerable<StudentResponse>>(request))
                .Returns(response);

            var result = await _studentsController.GetAll();

            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.EqualTo(response));
        }

        [Test]
        public async Task Create_ReturnsNoContent_WhenStudentCreated()
        {
            var studentId = 1;
            var DTO = new StudentDTO(studentId, "John", "Doe");
            var request = new StudentRequest(studentId, "John", "Doe");

            _studentServiceMock
                .Setup(s => s.CreateStudentAsync(request))
                .Returns(Task.CompletedTask);
            _validatorMock
                .Setup(v => v.ValidateAsync(DTO, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());
            _mapperMock
                .Setup(m => m.Map<StudentRequest>(DTO))
                .Returns(request);

            var result = await _studentsController.Create(DTO);

            var noContentResult = result as NoContentResult;
            Assert.NotNull(noContentResult);
            Assert.That(noContentResult.StatusCode, Is.EqualTo(204));
        }

        [Test]
        public async Task Create_ReturnsBadRequestWithError_WhenEntityIsNotValid()
        {
            var DTO = new StudentDTO(1, "That parameter is longer than 20 symbols.", "Doe");
            var validationErrors = new List<FluentValidation.Results.ValidationFailure>()
            {
                new FluentValidation.Results.ValidationFailure("FirstName", "That parameter is longer than 20 symbols.")
            };

            _validatorMock
                .Setup(v => v.ValidateAsync(DTO, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult(validationErrors));

            var result = await _studentsController.Create(DTO);

            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.That(badRequestResult.StatusCode, Is.EqualTo(400));

            var value = badRequestResult.Value;
            Assert.NotNull(value);
            
            var errors = value.GetType().GetProperty("Error")?.GetValue(value);
            Assert.That(errors, Is.EqualTo(validationErrors.Select(e => e.ErrorMessage).ToList()));
        }

        [Test]
        public async Task Update_ReturnsNoContent_WhenStudentIsUpdated()
        {
            var studentId = 1;
            var DTO = new StudentDTO(studentId, "John", "Doe");
            var request = new StudentRequest(studentId, "John", "Doe");

            _studentServiceMock
                .Setup(s => s.UpdateStudentAsync(request))
                .Returns(Task.CompletedTask);
            _validatorMock
                .Setup(v => v.ValidateAsync(DTO, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());
            _mapperMock
                .Setup(m => m.Map<StudentRequest>(DTO))
                .Returns(request);

            var result = await _studentsController.Update(studentId, DTO);

            var noContentResult = result as NoContentResult;
            Assert.NotNull(noContentResult);
            Assert.That(noContentResult.StatusCode, Is.EqualTo(204));
        }

        [Test]
        public async Task Update_ReturnsBadRequestWithError_WhenEntityIsNotValid()
        {
            var studentId = 1;
            var DTO = new StudentDTO(studentId, "That parameter is longer than 20 symbols.", "Doe");
            var validationErrors = new List<FluentValidation.Results.ValidationFailure>()
            {
                new FluentValidation.Results.ValidationFailure("FirstName", "That parameter is longer than 20 symbols.")
            };

            _validatorMock
                .Setup(v => v.ValidateAsync(DTO, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult(validationErrors));

            var result = await _studentsController.Update(studentId, DTO);

            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.That(badRequestResult.StatusCode, Is.EqualTo(400));

            var value = badRequestResult.Value;
            Assert.NotNull(value);

            var errors = value.GetType().GetProperty("Error")?.GetValue(value);
            Assert.That(errors, Is.EqualTo(validationErrors.Select(e => e.ErrorMessage).ToList()));
        }

        [Test]
        public async Task Update_ReturnsBadRequestWithError_WhenIdsAreEqual()
        {
            var studentId = 2;
            var DTO = new StudentDTO(1, "John.", "Doe");

            var result = await _studentsController.Update(studentId, DTO);

            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.That(badRequestResult.StatusCode, Is.EqualTo(400));

            var value = badRequestResult.Value;
            Assert.NotNull(value);

            var error = value.GetType().GetProperty("Error")?.GetValue(value)?.ToString();
            Assert.That(error, Is.EqualTo("Id and student Id are different."));
        }

        [Test]
        public async Task Delete_ReturnsNoContent_WhenStudentIsDeleted()
        {
            var studentId = 1;
            var request = new StudentRequest(studentId, "John", "Doe");

            _studentServiceMock
                .Setup(s => s.GetStudentByIdAsync(studentId))
                .ReturnsAsync(request);
            _studentServiceMock
                .Setup(s => s.DeleteStudentAsync(studentId))
                .Returns(Task.CompletedTask);

            var result = await _studentsController.Delete(studentId);

            var noContentResult = result as NoContentResult;
            Assert.NotNull(noContentResult);
            Assert.That(noContentResult.StatusCode, Is.EqualTo(204));
        }

        [Test]
        public async Task Delete_ReturnsNotFoundWithError_WhenStudentDoesNotExist()
        {
            var studentId = 1;
            StudentRequest? request = null;

            _studentServiceMock
                .Setup(s => s.GetStudentByIdAsync(studentId))
                .ReturnsAsync(request);

            var result = await _studentsController.Delete(studentId);

            var notFoundResult = result as NotFoundObjectResult;
            Assert.NotNull(notFoundResult);
            Assert.That(notFoundResult.StatusCode, Is.EqualTo(404));

            var value = notFoundResult.Value;
            Assert.NotNull(value);

            var error = value.GetType().GetProperty("Error")?.GetValue(value)?.ToString();
            Assert.That(error, Is.EqualTo("Student not found."));
        }
    }
}
