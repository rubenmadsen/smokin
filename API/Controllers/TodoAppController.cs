using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.Data.Sqlite;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Diagnostics;


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


        [ApiExplorerSettings(IgnoreApi = true)]
        private float ConvertToGrams(String amount)
        {
            float g = 1.0f;
            if (amount.Contains("mg"))
                g *= 1000.0f;
            else if(amount.Contains("µg"))
                g *= 1000.0f;
            else if (amount.Contains("ng"))
                g *= 1000.0f;

            return 0.0f;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        private void ClearTable(String tableName)
        {
            string sqlDatasource = _configuration.GetConnectionString("DBcon");
            using (var connection = new SqliteConnection(sqlDatasource))
            {
                connection.Open();

                string sql = $"DROP TABLE IF EXISTS {tableName};";

                using (var command = new SqliteCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                    Console.WriteLine($"Table {tableName} has been dropped.");
                }
                
            }
        }
        [ApiExplorerSettings(IgnoreApi = true)]
        private void ParseCSVData(String filePath)
        {
            using (var reader = new StreamReader(filePath))
            {
                string headerLine = reader.ReadLine();
                string line;

                String lastCategory = "";
                while ((line = reader.ReadLine()) != null)
                {
                    var values = line.Split(',');

                    if (!values[0].Equals(""))
                        lastCategory = values[0]; 
                    string substance = values[1];
                    string cigPerPuff = values[2];
                    string eCigPerPuff = values[3];

                    Debug.WriteLine($"{lastCategory}, {substance}, {cigPerPuff}, {eCigPerPuff}");
                
                }

            }
        }

        [SwaggerOperation(Summary = "Clear the tables and regenerate and populate them.")]
        [HttpGet]
        [Route("GenerateDatabase")]
        public IActionResult GenerateDatabase()
        {
            this.ClearTable("Categories");
            this.ClearTable("Substances");
            string sqlDatasource = _configuration.GetConnectionString("DBcon");
            using (var connection = new SqliteConnection(sqlDatasource))
            {
                connection.Open();

                // Create table "Categories"
                string createCategoriesTableQuery = @"
                    CREATE TABLE IF NOT EXISTS Categories (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT NOT NULL,
                        Description TEXT
                    );";

                using (var command = new SqliteCommand(createCategoriesTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                    Console.WriteLine("Table 'Categories' created successfully.");
                }

                // Create table Substances
                string createSubstancesTableQuery = @"
                    CREATE TABLE IF NOT EXISTS Substances (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        CategoryId INTEGER NOT NULL,
                        Name TEXT NOT NULL,
                        Description TEXT,
                        FOREIGN KEY (CategoryId) REFERENCES Categories(Id)
                    );";
                using (var command = new SqliteCommand(createSubstancesTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                    Console.WriteLine("Table 'Categories' created successfully.");
                }
            }

            this.ParseCSVData("../Data/toxins.csv");
            return StatusCode(200, "All good in the hood.");
        }

    }
}