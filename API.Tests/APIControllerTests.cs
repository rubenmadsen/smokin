using API.Controllers;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Tests
{
    public class APIControllerTests
    {
        private IConfiguration configuration;
        private TodoAppController? controller;

        public APIControllerTests()
        {
            configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Development.json")
                .Build();
        }

        [Fact]
        public void APIController_GetToxinsData_OK()
        {
            controller = new TodoAppController(configuration, NullLogger<TodoAppController>.Instance);

            Assert.NotNull(controller);

            var result = controller!.GetToxinData() as OkObjectResult;
           
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public void APIController_GetToxinsData_DatabaseConnectionError()
        {
            var invalidConfiguration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Invalid.json")
                .Build();

            controller = new TodoAppController(invalidConfiguration, NullLogger<TodoAppController>.Instance);

            Assert.NotNull(controller);

            var result = controller!.GetToxinData() as ObjectResult;

            Assert.NotNull(result);
            Assert.Equal(500, result.StatusCode);
            Assert.Equal("An error occurred while accessing the SQLite database.", result.Value);
        }

        //[Fact]
        //public void APIController_GetToxinsData_NonExistingTable()
        //{
        //    // Simulate a non-existing table by providing an invalid query
        //    var configurationWithInvalidTable = new ConfigurationBuilder()
        //        .AddJsonFile("appsettings.WithInvalidTable.json")
        //        .Build();

        //    controller = new TodoAppController(configurationWithInvalidTable, NullLogger<TodoAppController>.Instance, dbConnection);

        //    Assert.NotNull(controller);

        //    var result = controller!.GetToxinData() as ObjectResult;

        //    Assert.NotNull(result);
        //    Assert.Equal(500, result.StatusCode);
        //    Assert.Equal("An unexpected error occurred.", result.Value);
        //}
    }

}
