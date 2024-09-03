using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.Data.Sqlite;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Globalization;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using API.Manager;
namespace API.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class TodoAppController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<TodoAppController> _logger;
        private DatabaseGenerator databaseManager;

        public TodoAppController(IConfiguration configuration, ILogger<TodoAppController> logger)
        {
            _configuration = configuration;
            _logger = logger;
            this.databaseManager = new DatabaseGenerator(_configuration.GetConnectionString("DBcon"));
        }

        

        [SwaggerOperation(Summary = "Returns data on toxins derived from smoking.")]
        [HttpGet]
        [Route("GetToxinData")]
        public IActionResult GetToxinData()
        {
            try
            {
                string query = "select * from toxins";
                DataTable table = new DataTable();
                string sqlDatasource = _configuration.GetConnectionString("DBcon");

                using (var myCon = new SqliteConnection(sqlDatasource))
                {
                    myCon.Open();

                    using (var myCommand = new SqliteCommand(query, myCon))
                    {
                        using (var myReader = myCommand.ExecuteReader())
                        {
                            table.Load(myReader);
                        }
                    }
                }

                _logger.LogInformation("GetToxinData");
                return Ok(new JsonResult(table));
            }
            catch (SqliteException ex)
            {
                _logger.LogCritical(ex, "SQLite error occurred");
                return StatusCode(500, "An error occurred while accessing the SQLite database.");
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "An unexpected error occurred");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [SwaggerOperation(Summary = "Returns descriptions for individual toxins, detailing their effects on the body.")]
        [HttpGet]
        [Route("GetToxinsDescriptions")]
        public IActionResult GetToxinsDescriptions()
        {
            // Implementation should provide descriptions of toxins and their effects on the body.
            throw new NotImplementedException("Endpoint not implemented yet");
        }

        [HttpPost]
        [Route("AddUser")]
        public IActionResult AddUser([FromForm] int numberOfDays, [FromForm] string type, [FromForm] int quantity, [FromForm] int totalSpent, [FromForm] int totalIfInvested)
        {
            // Implementation should add a new user, with the specified parameters, to a table in the sqlite database.
            throw new NotImplementedException("Endpoint not implemented yet");
        }

        [HttpGet]
        [Route("GetAllUsers")]
        public IActionResult GetAllUsers()
        {
            // Implementation will retrieve all users from the database.
            throw new NotImplementedException("Endpoint not implemented yet");
        }

        [HttpGet]
        [Route("GetAllUsersCalculatedData")]
        public IActionResult GetAllUsersCalculatedData()
        {
            // Implementation will retrieve and summarize data from all the users in the database. 
            throw new NotImplementedException("Endpoint not implemented yet");
        }

        [HttpGet]
        [Route("GetAllUsersCalculatedDataByType/{type}")]
        public IActionResult GetAllUsersCalculatedDataByType(string type)
        {
            // Implementation will retrieve and summarize data from all the users in the database, type should either be 'Tobacco' or 'Electronic'. 
            throw new NotImplementedException("Endpoint not implemented yet");
        }

        [HttpGet]
        [Route("GetUserToxinsData{numberOfDays}/{type}/{quantity}/{priceToday}/{futureDays}")]
        public IActionResult GetUserToxinsData(int numberOfDays, string type, int quantity, int priceToday, int futureDays)
        {
            // Implementation will calculate and return the amount of toxins a user has consumed based on their input. 
            throw new NotImplementedException("Endpoint not implemented yet");
        }


        [SwaggerOperation(Summary = "Generate the database from data files.")]
        [HttpGet]
        [Route("GenerateDatabase")]
        public IActionResult GenerateDatabase()
        {
            int result = this.databaseManager.GenerateDatabase();
            if (result == 0)
                return StatusCode(200, "All good in the hood.");
            else
                return StatusCode(500, "Grande problemas");
        }

    }
}