using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SoftServe_TestProject.API.Controllers;
using SoftServe_TestProject.API.DTOs;
using SoftServe_TestProject.API.Responses;
using SoftServe_TestProject.Application.Interfaces;
using SoftServe_TestProject.Application.Requests;
using SoftServe_TestProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftServe_TestProject.Tests.ControllersTests
{
    [TestFixture]
    public class CoursesControllerTests
    {
        private CoursesController _coursesController;
        private Mock<ICourseService> _courseServiceMock;
        private Mock<IValidator<CourseDTO>> _validatorMock;
        private Mock<IMapper> _mapperMock;

        [SetUp]
        public void SetUp()
        {
            _courseServiceMock = new Mock<ICourseService>();
            _mapperMock = new Mock<IMapper>();
            _validatorMock = new Mock<IValidator<CourseDTO>>();

            _coursesController = new CoursesController(_courseServiceMock.Object, _validatorMock.Object, _mapperMock.Object);
        }

        [Test]
        public async Task GetById_ReturnsOkWithCourseResponse_WhenCourseExisis()
        {
            var courseId = 1;
            var teacherId = 1;
            var request = new CourseRequest(courseId, "Math", "Math course", teacherId);
            var response = new CourseResponse(courseId, "Math", "Math course", teacherId);

            _courseServiceMock
                .Setup(s => s.GetCourseByIdAsync(courseId))
                .ReturnsAsync(request);
            _mapperMock
                .Setup(m => m.Map<CourseResponse>(request))
                .Returns(response);

            var result = await _coursesController.GetById(courseId);

            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.EqualTo(response));
        }

        [Test]
        public async Task GetById_ReturnsNotFoundWithError_WhenCourseDoesNotExist()
        {
            var courseId = 1;
            CourseRequest? request = null;

            _courseServiceMock
                .Setup(s => s.GetCourseByIdAsync(courseId))
                .ReturnsAsync(request);

            var result = await _coursesController.GetById(courseId);

            var notFoundResult = result as NotFoundObjectResult;
            Assert.NotNull(notFoundResult);
            Assert.That(notFoundResult.StatusCode, Is.EqualTo(404));

            var value = notFoundResult.Value;
            Assert.NotNull(value);

            var error = value.GetType().GetProperty("Error")?.GetValue(value)?.ToString();
            Assert.That(error, Is.EqualTo("Course not found."));
        }

        [Test]
        public async Task GetAll_ReturnsOkWithListOfCourses_WhenCoursesExist()
        {
            var request = new List<CourseRequest>
            {
                new CourseRequest(1, "Math", "Math course", 1),
                new CourseRequest(2, "English", "English course", 2)
            };
            var response = new List<CourseResponse>
            {
                new CourseResponse(1, "Math", "Math course", 1),
                new CourseResponse(2, "English", "English course", 2)
            };

            _courseServiceMock
                .Setup(s => s.GetAllCoursesAsync())
                .ReturnsAsync(request);
            _mapperMock
                .Setup(m => m.Map<IEnumerable<CourseResponse>>(request))
                .Returns(response);

            var result = await _coursesController.GetAll();

            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.EqualTo(response));
        }

        [Test]
        public async Task GetAll_ReturnsOkWithNull_WhenCoursesDoNotExist()
        {
            List<CourseRequest> request = [];
            List<CourseResponse> response = [];

            _courseServiceMock
                .Setup(s => s.GetAllCoursesAsync())
                .ReturnsAsync(request);
            _mapperMock
                .Setup(m => m.Map<IEnumerable<CourseResponse>>(request))
                .Returns(response);

            var result = await _coursesController.GetAll();

            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.EqualTo(response));
        }

        [Test]
        public async Task Create_ReturnsNoContent_WhenCourseCreated()
        {
            var courseId = 1;
            var teacherId = 1;
            var DTO = new CourseDTO(courseId, "Math", "Math course", teacherId);
            var request = new CourseRequest(courseId, "Math", "Math course", teacherId);

            _courseServiceMock
                .Setup(s => s.CreateCourseAsync(request))
                .Returns(Task.CompletedTask);
            _validatorMock
                .Setup(v => v.ValidateAsync(DTO, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());
            _mapperMock
                .Setup(m => m.Map<CourseRequest>(DTO))
                .Returns(request);

            var result = await _coursesController.Create(DTO);

            var noContentResult = result as NoContentResult;
            Assert.NotNull(noContentResult);
            Assert.That(noContentResult.StatusCode, Is.EqualTo(204));
        }

        [Test]
        public async Task Create_ReturnsBadRequestWithError_WhenEntityIsNotValid()
        {
            var DTO = new CourseDTO(1, "That parameter is longer than 20 symbols.", "Math course", 1);
            var validationErrors = new List<FluentValidation.Results.ValidationFailure>()
            {
                new FluentValidation.Results.ValidationFailure("Title", "That parameter is longer than 20 symbols.")
            };

            _validatorMock
                .Setup(v => v.ValidateAsync(DTO, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult(validationErrors));

            var result = await _coursesController.Create(DTO);

            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.That(badRequestResult.StatusCode, Is.EqualTo(400));

            var value = badRequestResult.Value;
            Assert.NotNull(value);

            var errors = value.GetType().GetProperty("Error")?.GetValue(value);
            Assert.That(errors, Is.EqualTo(validationErrors.Select(e => e.ErrorMessage).ToList()));
        }

        [Test]
        public async Task Update_ReturnsNoContent_WhenCourseIsUpdated()
        {
            var courseId = 1;
            var teacherId = 1;
            var DTO = new CourseDTO(courseId, "Math", "Math course", teacherId);
            var request = new CourseRequest(courseId, "Math", "Math course", teacherId);

            _courseServiceMock
                .Setup(s => s.UpdateCourseAsync(request))
                .Returns(Task.CompletedTask);
            _validatorMock
                .Setup(v => v.ValidateAsync(DTO, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());
            _mapperMock
                .Setup(m => m.Map<CourseRequest>(DTO))
                .Returns(request);

            var result = await _coursesController.Update(courseId, DTO);

            var noContentResult = result as NoContentResult;
            Assert.NotNull(noContentResult);
            Assert.That(noContentResult.StatusCode, Is.EqualTo(204));
        }

        [Test]
        public async Task Update_ReturnsBadRequestWithError_WhenEntityIsNotValid()
        {
            var courseId = 1;
            var teacherId = 1;
            var DTO = new CourseDTO(courseId, "That parameter is longer than 20 symbols.", "Math course", teacherId);
            var validationErrors = new List<FluentValidation.Results.ValidationFailure>()
            {
                new FluentValidation.Results.ValidationFailure("FirstName", "That parameter is longer than 20 symbols.")
            };

            _validatorMock
                .Setup(v => v.ValidateAsync(DTO, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult(validationErrors));

            var result = await _coursesController.Update(courseId, DTO);

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
            var courseId = 2;
            var DTO = new CourseDTO(1, "Math", "Math course", 1);

            var result = await _coursesController.Update(courseId, DTO);

            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.That(badRequestResult.StatusCode, Is.EqualTo(400));

            var value = badRequestResult.Value;
            Assert.NotNull(value);

            var error = value.GetType().GetProperty("Error")?.GetValue(value)?.ToString();
            Assert.That(error, Is.EqualTo("Id and course Id are different."));
        }

        [Test]
        public async Task Delete_ReturnsNoContent_WhenCourseIsDeleted()
        {
            var courseId = 1;
            var teacherId = 1;
            var request = new CourseRequest(courseId, "Math", "Math course", teacherId);

            _courseServiceMock
                .Setup(s => s.GetCourseByIdAsync(courseId))
                .ReturnsAsync(request);
            _courseServiceMock
                .Setup(s => s.DeleteCourseAsync(courseId))
                .Returns(Task.CompletedTask);

            var result = await _coursesController.Delete(courseId);

            var noContentResult = result as NoContentResult;
            Assert.NotNull(noContentResult);
            Assert.That(noContentResult.StatusCode, Is.EqualTo(204));
        }

        [Test]
        public async Task Delete_ReturnsNotFoundWithError_WhenCourseDoesNotExist()
        {
            var studentId = 1;
            CourseRequest? request = null;

            _courseServiceMock
                .Setup(s => s.GetCourseByIdAsync(studentId))
                .ReturnsAsync(request);

            var result = await _coursesController.Delete(studentId);

            var notFoundResult = result as NotFoundObjectResult;
            Assert.NotNull(notFoundResult);
            Assert.That(notFoundResult.StatusCode, Is.EqualTo(404));

            var value = notFoundResult.Value;
            Assert.NotNull(value);

            var error = value.GetType().GetProperty("Error")?.GetValue(value)?.ToString();
            Assert.That(error, Is.EqualTo("Course not found."));
        }
    }
}
