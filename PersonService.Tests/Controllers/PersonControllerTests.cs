using Microsoft.AspNetCore.Mvc;
using Moq;
using PersonService.Controllers;
using PersonService.Dtos.Person;
using PersonService.Dtos;
using PersonService.Exceptions;
using PersonService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting.Server;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PersonService.Tests.Controllers
{
    [TestClass]
    public class PersonControllerTests
    {
        private Mock<IPersonService>? personServiceMock;
        private Mock<ILogger<PersonController>>? loggerMock;
        private PersonController? controller;

        [TestInitialize]
        public void TestInitialize()
        {
            personServiceMock = new Mock<IPersonService>();
            loggerMock = new Mock<ILogger<PersonController>>();
            controller = new PersonController(personServiceMock.Object, loggerMock.Object);
        }

        [TestMethod]
        public async Task GetPerson_ReturnsOkResult_WhenPersonIsFound()
        {
            var personCode = "123";
            var personDto = new PersonDto { PersonalCode = personCode };
            personServiceMock!.Setup(service => service.GetPerson(personCode))
                              .ReturnsAsync(personDto);

            var result = await controller!.GetPerson(personCode, new XroadHeaders());

            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(personDto, okResult.Value);
        }

        [TestMethod]
        public async Task GetPerson_ReturnsNotFound_WhenPersonIsNotFound()
        {
            var personCode = "123";
            personServiceMock!.Setup(service => service.GetPerson(personCode))
                              .ThrowsAsync(new EntityNotFoundException("Not found"));

            var result = await controller!.GetPerson(personCode, new XroadHeaders());

            var notFoundResult = result.Result as NotFoundResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
        }

        [TestMethod]
        public async Task GetPerson_ThrowsException()
        {
            var personCode = "123";
            var personDto = new PersonDto { PersonalCode = personCode };
            personServiceMock!.Setup(service => service.GetPerson(personCode))
                              .Throws<Exception>();
            
            var response = await controller!.GetPerson(personCode, new XroadHeaders());
            
            var result = response.Result as ObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(500, result.StatusCode);
            Assert.AreEqual("Internal server error", result.Value);
        }

        [TestMethod]
        public async Task CreatePerson_ReturnsCreatedResult_WhenPersonIsCreated()
        {
            var personDto = new PersonDto { PersonalCode = "123" };
            personServiceMock!.Setup(service => service.CreatePerson(personDto))
                              .ReturnsAsync(personDto);

            var result = await controller!.CreatePerson(personDto, new XroadHeaders());

            var createdResult = result.Result as CreatedResult;
            Assert.IsNotNull(createdResult);
            Assert.AreEqual(201, createdResult.StatusCode);
            Assert.AreEqual($"api/Person", createdResult.Location);
            Assert.AreEqual(personDto, createdResult.Value);
        }

        [TestMethod]
        public async Task CreatePerson_ThrowsException()
        {
            var personCode = "123";
            var personDto = new PersonDto { PersonalCode = personCode };
            personServiceMock!.Setup(service => service.CreatePerson(personDto))
                              .Throws<Exception>();

            var response = await controller!.CreatePerson(personDto, new XroadHeaders());

            var result = response.Result as ObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(500, result.StatusCode);
            Assert.AreEqual("Internal server error", result.Value);
        }

        [TestMethod]
        public async Task UpdatePerson_ReturnsOkResult_WhenPersonIsUpdated()
        {
            var personDto = new PersonDto { PersonalCode = "123" };
            personServiceMock!.Setup(service => service.UpdatePerson(personDto))
                              .ReturnsAsync(personDto);

            var result = await controller!.UpdatePerson(personDto, new XroadHeaders());

            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(personDto, okResult.Value);
        }

        [TestMethod]
        public async Task UpdatePerson_ReturnsNotFound_WhenPersonIsNotFound()
        {
            var personDto = new PersonDto { PersonalCode = "123" };
            personServiceMock!.Setup(service => service.UpdatePerson(personDto))
                              .ThrowsAsync(new EntityNotFoundException("Not found"));

            var result = await controller!.UpdatePerson(personDto, new XroadHeaders());

            var notFoundResult = result.Result as NotFoundResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
        }

        [TestMethod]
        public async Task UpdatePerson_ThrowsException()
        {
            var personCode = "123";
            var personDto = new PersonDto { PersonalCode = personCode };
            personServiceMock!.Setup(service => service.UpdatePerson(personDto))
                              .Throws<Exception>();

            var response = await controller!.UpdatePerson(personDto, new XroadHeaders());

            var result = response.Result as ObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(500, result.StatusCode);
            Assert.AreEqual("Internal server error", result.Value);
        }

        [TestMethod]
        public async Task DeletePerson_ReturnsOkResult_WhenPersonIsDeleted()
        {
            var personDto = new PersonDto { PersonalCode = "123" };
            personServiceMock!.Setup(service => service.DeletePerson(personDto))
                              .ReturnsAsync(true);

            var result = await controller!.DeletePerson(personDto, new XroadHeaders());

            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(true, okResult.Value);
        }

        [TestMethod]
        public async Task DeletePerson_ThrowsException()
        {
            var personCode = "123";
            var personDto = new PersonDto { PersonalCode = personCode };
            personServiceMock!.Setup(service => service.DeletePerson(personDto))
                              .Throws<Exception>();

            var response = await controller!.DeletePerson(personDto, new XroadHeaders());

            var result = response.Result as ObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(500, result.StatusCode);
            Assert.AreEqual("Internal server error", result.Value);
        }

        [TestMethod]
        public async Task GetPersonChanges_ReturnsOkResult_WithPersonChanges()
        {
            var requestDto = new PersonChangesRequestDto { };
            var responseDto = new PersonChangesResponseDto { };
            personServiceMock!.Setup(service => service.GetPersonChanges(requestDto))
                              .ReturnsAsync(responseDto);

            var result = await controller!.GetPersonChanges(requestDto);

            var okResult = result.Result as ObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(responseDto, okResult.Value);
        }

        [TestMethod]
        public async Task GetPersonChanges_ReturnsInternalServerError_OnException()
        {
            var requestDto = new PersonChangesRequestDto { };
            personServiceMock!.Setup(service => service.GetPersonChanges(requestDto))
                              .ThrowsAsync(new System.Exception());

            var result = await controller!.GetPersonChanges(requestDto);

            var statusCodeResult = result.Result as ObjectResult;
            Assert.IsNotNull(statusCodeResult);
            Assert.AreEqual(500, statusCodeResult.StatusCode);
            Assert.AreEqual("Internal server error", statusCodeResult.Value);
        }
    }
}
