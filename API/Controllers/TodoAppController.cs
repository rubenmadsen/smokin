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
using API.Models;
using Microsoft.AspNetCore.JsonPatch.Internal;

namespace API.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class TodoAppController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<TodoAppController> _logger;

        public TodoAppController(IConfiguration configuration, ILogger<TodoAppController> logger)
        {
            _configuration = configuration;
            _logger = logger;
           
        }


        // Endpoint: GP-26
        [SwaggerOperation(Summary = "Returns all substances and the corresponding concentrations of Cigarette / E-Cigarette")]
        [HttpGet]
        [Route("GetSubstancesAndConcentrations/{cigarette_type}")]
        public IActionResult GetSubstancesAndConcentrations(string cigarette_type)
        {
            try
            {
                string query = "SELECT toxinName, Amount FROM Consumables WHERE nameConsumable = @cigarette_type AND Amount <> '-'";
                DataTable table = new DataTable();
                string sqlDatasource = _configuration.GetConnectionString("DBcon");

                using (var myCon = new SqliteConnection(sqlDatasource))
                {
                    myCon.Open();

                    using (var myCommand = new SqliteCommand(query, myCon))
                    {
                        myCommand.Parameters.AddWithValue("@cigarette_type", cigarette_type);
                        using (var myReader = myCommand.ExecuteReader())
                        {
                            table.Load(myReader);
                        }
                    }
                }

                _logger.LogInformation("GetSubstancesAndConcentrations");
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


        // Endpoint: GP-29
        [SwaggerOperation(Summary = "Returns all substances, their category, and their explanation")]
        [HttpGet]
        [Route("GetSubstanceCategoryAndDescription")]
        public IActionResult GetSubstanceCategoryAndDescription()
        {
            try
            {
                string query = "SELECT * FROM Toxins";
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

                _logger.LogInformation("GetSubstanceCategoryAndDescription");
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


        // Endpoint: GP-27
        [SwaggerOperation(Summary = "Add a row with userName, date, amount in the Table Users")]
        [HttpPost]
        [Route("PostNewUserTrackingData")]
        public IActionResult PostNewUserTrackingData([FromForm] string userName, [FromForm] string consumableName, [FromForm] DateTime date, [FromForm] int amount)
        {
            try
            {

                string query = "INSERT INTO Users (userName, consumableName, date, amount) VALUES (@userName, @consumableName, @date, @amount)";
                string sqlDatasource = _configuration.GetConnectionString("DBcon");

                using (var myCon = new SqliteConnection(sqlDatasource))
                {
                    myCon.Open();

                    using (var myCommand = new SqliteCommand(query, myCon))
                    {

                        myCommand.Parameters.AddWithValue("@userName", userName);
                        myCommand.Parameters.AddWithValue("@consumableName", consumableName);
                        myCommand.Parameters.AddWithValue("@date", date);
                        myCommand.Parameters.AddWithValue("@amount", amount);
                        myCommand.ExecuteNonQuery();
                    }
                }

                _logger.LogInformation("PostNewUserTrackingData");
                return StatusCode(200, "All good, added new data in the hood.");
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


        // Endpoint: GP-28
        [SwaggerOperation(Summary = "Returns all logged data for a specified user.")]
        [HttpGet]
        [Route("GetTrackedUserData/{userName}")]
        public IActionResult GetTrackedUserData(string userName)
        {
            try
            {
                string query = "SELECT consumableName, date, amount FROM Users WHERE userName = @userName";
                DataTable table = new DataTable();
                string sqlDatasource = _configuration.GetConnectionString("DBcon");

                using (var myCon = new SqliteConnection(sqlDatasource))
                {
                    myCon.Open();

                    using (var myCommand = new SqliteCommand(query, myCon))
                    {
                        myCommand.Parameters.AddWithValue("@userName", userName);
                        using (var myReader = myCommand.ExecuteReader())
                        {
                            table.Load(myReader);
                        }
                    }
                }

                _logger.LogInformation("GetTrackedUserData");
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


        [SwaggerOperation(Summary = "Generate the database from data files.")]
        [HttpGet]
        [Route("GenerateDatabase")]
        public IActionResult GenerateDatabase()
        {
            DatabaseGenerator databaseGenerator = new DatabaseGenerator(_configuration.GetConnectionString("DBcon"));
            int result = databaseGenerator.GenerateDatabase();
            if (result == 0)
                return StatusCode(200, "All good in the hood.");
            else
                return StatusCode(500, "Grande problemas");
        }

    }
}