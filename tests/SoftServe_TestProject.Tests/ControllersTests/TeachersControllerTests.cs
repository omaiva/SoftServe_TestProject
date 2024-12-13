using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SoftServe_TestProject.API.Controllers;
using SoftServe_TestProject.API.DTOs;
using SoftServe_TestProject.API.Responses;
using SoftServe_TestProject.Application.Interfaces;
using SoftServe_TestProject.Application.Requests;

namespace SoftServe_TestProject.Tests.ControllersTests
{
    [TestFixture]
    public class TeachersControllerTests
    {
        private TeachersController _teachersController;
        private Mock<ITeacherService> _teacherServiceMock;
        private Mock<IValidator<TeacherDTO>> _validatorMock;
        private Mock<IMapper> _mapperMock;

        [SetUp]
        public void SetUp()
        {
            _teacherServiceMock = new Mock<ITeacherService>();
            _mapperMock = new Mock<IMapper>();
            _validatorMock = new Mock<IValidator<TeacherDTO>>();

            _teachersController = new TeachersController(_teacherServiceMock.Object, _validatorMock.Object, _mapperMock.Object);
        }

        [Test]
        public async Task GetById_ReturnsOkWithTeacherResponse_WhenTeacherExisis()
        {
            var teacherId = 1;
            var request = new TeacherRequest(teacherId, "John", "Doe");
            var response = new TeacherResponse(teacherId, "John", "Doe");

            _teacherServiceMock
                .Setup(s => s.GetTeacherByIdAsync(teacherId))
                .ReturnsAsync(request);
            _mapperMock
                .Setup(m => m.Map<TeacherResponse>(request))
                .Returns(response);

            var result = await _teachersController.GetById(teacherId);

            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.EqualTo(response));
        }

        [Test]
        public async Task GetById_ReturnsNotFoundWithError_WhenTeacherDoesNotExist()
        {
            var teacherId = 1;
            TeacherRequest? request = null;

            _teacherServiceMock
                .Setup(s => s.GetTeacherByIdAsync(teacherId))
                .ReturnsAsync(request);

            var result = await _teachersController.GetById(teacherId);

            var notFoundResult = result as NotFoundObjectResult;
            Assert.NotNull(notFoundResult);
            Assert.That(notFoundResult.StatusCode, Is.EqualTo(404));

            var value = notFoundResult.Value;
            Assert.NotNull(value);

            var error = value.GetType().GetProperty("Error")?.GetValue(value)?.ToString();
            Assert.That(error, Is.EqualTo("Teacher not found."));
        }

        [Test]
        public async Task GetAll_ReturnsOkWithListOfTeachers_WhenTeachersExist()
        {
            var request = new List<TeacherRequest>
            {
                new TeacherRequest(1, "John", "Doe"),
                new TeacherRequest(2, "Jane", "Doe")
            };
            var response = new List<TeacherResponse>
            {
                new TeacherResponse(1, "John", "Doe"),
                new TeacherResponse(2, "Jane", "Doe")
            };

            _teacherServiceMock
                .Setup(s => s.GetAllTeachersAsync())
                .ReturnsAsync(request);
            _mapperMock
                .Setup(m => m.Map<IEnumerable<TeacherResponse>>(request))
                .Returns(response);

            var result = await _teachersController.GetAll();

            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.EqualTo(response));
        }

        [Test]
        public async Task GetAll_ReturnsOkWithNull_WhenTeachersDoNotExist()
        {
            List<TeacherRequest> request = [];
            List<TeacherResponse> response = [];

            _teacherServiceMock
                .Setup(s => s.GetAllTeachersAsync())
                .ReturnsAsync(request);
            _mapperMock
                .Setup(m => m.Map<IEnumerable<TeacherResponse>>(request))
                .Returns(response);

            var result = await _teachersController.GetAll();

            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.EqualTo(response));
        }

        [Test]
        public async Task Create_ReturnsNoContent_WhenTeacherCreated()
        {
            var teacherId = 1;
            var DTO = new TeacherDTO(teacherId, "John", "Doe");
            var request = new TeacherRequest(teacherId, "John", "Doe");

            _teacherServiceMock
                .Setup(s => s.CreateTeacherAsync(request))
                .Returns(Task.CompletedTask);
            _validatorMock
                .Setup(v => v.ValidateAsync(DTO, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());
            _mapperMock
                .Setup(m => m.Map<TeacherRequest>(DTO))
                .Returns(request);

            var result = await _teachersController.Create(DTO);

            var noContentResult = result as NoContentResult;
            Assert.NotNull(noContentResult);
            Assert.That(noContentResult.StatusCode, Is.EqualTo(204));
        }

        [Test]
        public async Task Create_ReturnsBadRequestWithError_WhenEntityIsNotValid()
        {
            var DTO = new TeacherDTO(1, "That parameter is longer than 20 symbols.", "Doe");
            var validationErrors = new List<FluentValidation.Results.ValidationFailure>()
            {
                new FluentValidation.Results.ValidationFailure("FirstName", "That parameter is longer than 20 symbols.")
            };

            _validatorMock
                .Setup(v => v.ValidateAsync(DTO, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult(validationErrors));

            var result = await _teachersController.Create(DTO);

            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.That(badRequestResult.StatusCode, Is.EqualTo(400));

            var value = badRequestResult.Value;
            Assert.NotNull(value);

            var errors = value.GetType().GetProperty("Error")?.GetValue(value);
            Assert.That(errors, Is.EqualTo(validationErrors.Select(e => e.ErrorMessage).ToList()));
        }

        [Test]
        public async Task Update_ReturnsNoContent_WhenTeacherIsUpdated()
        {
            var teacherId = 1;
            var DTO = new TeacherDTO(teacherId, "John", "Doe");
            var request = new TeacherRequest(teacherId, "John", "Doe");

            _teacherServiceMock
                .Setup(s => s.UpdateTeacherAsync(request))
                .Returns(Task.CompletedTask);
            _validatorMock
                .Setup(v => v.ValidateAsync(DTO, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());
            _mapperMock
                .Setup(m => m.Map<TeacherRequest>(DTO))
                .Returns(request);

            var result = await _teachersController.Update(teacherId, DTO);

            var noContentResult = result as NoContentResult;
            Assert.NotNull(noContentResult);
            Assert.That(noContentResult.StatusCode, Is.EqualTo(204));
        }

        [Test]
        public async Task Update_ReturnsBadRequestWithError_WhenEntityIsNotValid()
        {
            var teacherId = 1;
            var DTO = new TeacherDTO(teacherId, "That parameter is longer than 20 symbols.", "Doe");
            var validationErrors = new List<FluentValidation.Results.ValidationFailure>()
            {
                new FluentValidation.Results.ValidationFailure("FirstName", "That parameter is longer than 20 symbols.")
            };

            _validatorMock
                .Setup(v => v.ValidateAsync(DTO, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult(validationErrors));

            var result = await _teachersController.Update(teacherId, DTO);

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
            var teacherId = 2;
            var DTO = new TeacherDTO(1, "John.", "Doe");

            var result = await _teachersController.Update(teacherId, DTO);

            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.That(badRequestResult.StatusCode, Is.EqualTo(400));

            var value = badRequestResult.Value;
            Assert.NotNull(value);

            var error = value.GetType().GetProperty("Error")?.GetValue(value)?.ToString();
            Assert.That(error, Is.EqualTo("Id and teacher Id are different."));
        }

        [Test]
        public async Task Delete_ReturnsNoContent_WhenTeacherIsDeleted()
        {
            var teacherId = 1;
            var request = new TeacherRequest(teacherId, "John", "Doe");

            _teacherServiceMock
                .Setup(s => s.GetTeacherByIdAsync(teacherId))
                .ReturnsAsync(request);
            _teacherServiceMock
                .Setup(s => s.DeleteTeacherAsync(teacherId))
                .Returns(Task.CompletedTask);

            var result = await _teachersController.Delete(teacherId);

            var noContentResult = result as NoContentResult;
            Assert.NotNull(noContentResult);
            Assert.That(noContentResult.StatusCode, Is.EqualTo(204));
        }

        [Test]
        public async Task Delete_ReturnsNotFoundWithError_WhenTeacherDoesNotExist()
        {
            var studentId = 1;
            TeacherRequest? request = null;

            _teacherServiceMock
                .Setup(s => s.GetTeacherByIdAsync(studentId))
                .ReturnsAsync(request);

            var result = await _teachersController.Delete(studentId);

            var notFoundResult = result as NotFoundObjectResult;
            Assert.NotNull(notFoundResult);
            Assert.That(notFoundResult.StatusCode, Is.EqualTo(404));

            var value = notFoundResult.Value;
            Assert.NotNull(value);

            var error = value.GetType().GetProperty("Error")?.GetValue(value)?.ToString();
            Assert.That(error, Is.EqualTo("Teacher not found."));
        }
    }
}
