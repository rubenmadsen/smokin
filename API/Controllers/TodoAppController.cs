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

namespace API.Controllers
{
    public class Toxin
    {
        [JsonPropertyName("toxin")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }
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
        private float ConvertToGrams(string amount)
        {
            string[] parts = amount.Split(" ");
            string unit = parts[1];
            string num = parts[0];
            if (num.StartsWith("<"))
                num = num.Substring(1);
            float.TryParse(num, out float value);

                if (unit.Contains("mg"))
                value /= 1000.0f; 
            else if (unit.Contains("µg"))
                value /= 1000000.0f; 
            else if (unit.Contains("ng"))
                value /= 1000000000.0f;

            return value;
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
        private int AddToxin(String toxinName, String description, int category)
        {

            string sqlDatasource = _configuration.GetConnectionString("DBcon");
            using (var connection = new SqliteConnection(sqlDatasource))
            {
                connection.Open();
                string insertToxinQuery = "INSERT INTO Toxins (CategoryId, Name, Description) VALUES (@CategoryId, @Name, @Description)";
                using (var insertToxinCommand = new SqliteCommand(insertToxinQuery, connection))
                {
                    insertToxinCommand.Parameters.AddWithValue("@CategoryId", category);
                    insertToxinCommand.Parameters.AddWithValue("@Name", toxinName);
                    insertToxinCommand.Parameters.AddWithValue("@Description", description);
                    insertToxinCommand.ExecuteNonQuery();
                    Console.WriteLine($"Toxin '{toxinName}' added under CategoryId {category}.");
                }
            }
                return 0;
        }
        [ApiExplorerSettings(IgnoreApi = true)]
        private int AddCategory(String categoryName)
        {
            int id = -1;
            string sqlDatasource = _configuration.GetConnectionString("DBcon");
            using (var connection = new SqliteConnection(sqlDatasource))
            {
                connection.Open();
                string getCategoryQuery = "SELECT Id FROM Categories WHERE Name = @Name";
                using (var getCategoryCommand = new SqliteCommand(getCategoryQuery, connection))
                {
                    getCategoryCommand.Parameters.AddWithValue("@Name", categoryName);
                    var result = getCategoryCommand.ExecuteScalar();

                    if (result == null)
                    {
                        // Step 2: If the category does not exist, insert it
                        string insertCategoryQuery = "INSERT INTO Categories (Name) VALUES (@Name); SELECT last_insert_rowid();";
                        using (var insertCategoryCommand = new SqliteCommand(insertCategoryQuery, connection))
                        {
                            insertCategoryCommand.Parameters.AddWithValue("@Name", categoryName);
                            id = Convert.ToInt32(insertCategoryCommand.ExecuteScalar());
                            Console.WriteLine($"Category '{categoryName}' inserted with Id {id}.");
                        }
                    }
                    else
                    {
                        // The category exists, get the Id
                        id = Convert.ToInt32(result);
                        Console.WriteLine($"Category '{categoryName}' already exists with Id {id}.");
                    }
                }
            }
            return id;
        }
        [ApiExplorerSettings(IgnoreApi = true)]
        private void ParseCSVToxins(String filePath)
        {
            String jsonPath = "../Data/toxin_descriptions.json";
            String jsonString;
            List<Toxin> toxins = new List<Toxin>();
            using (StreamReader r = new StreamReader(jsonPath))
            {
                jsonString = r.ReadToEnd();
                toxins = JsonSerializer.Deserialize<List<Toxin>>(jsonString);
            }

            using (var reader = new StreamReader(filePath))
            {
                string headerLine = reader.ReadLine();
                string line;

                int curCategoryId = -1;
                string sqlDatasource = _configuration.GetConnectionString("DBcon");
                using (var connection = new SqliteConnection(sqlDatasource))
                {
                    connection.Open();
                    while ((line = reader.ReadLine()) != null)
                    {
                        var values = line.Split(',');
                        string category = values[0];
                        string toxin = values[1];

                        if (!values[0].Equals(""))
                            curCategoryId = this.AddCategory(category);

                        if (toxin.Equals(""))
                            toxin = category;
                        Toxin tox = toxins.FirstOrDefault(t => t.Name.Equals(toxin));
                        String desc = "None";
                        if (tox != null)
                            desc = tox.Description;
                        else
                            Debug.WriteLine($"Could not find a match for {toxin}");
                        this.AddToxin(toxin, desc,  curCategoryId);
                       
                    }
                }

            }
        }


        [ApiExplorerSettings(IgnoreApi = true)]
        private void ParseCSVConsumables(String filePath)
        {
            using (var reader = new StreamReader(filePath))
            {
                string headerLine = reader.ReadLine();
                string line;

                String lastCategory = "";
                while ((line = reader.ReadLine()) != null)
                {
                    var values = line.Split(',');
                    string substance;
                    float cigPerPuff;
                    float eCigPerPuff;
                    Debug.WriteLine(line);
                    if (!values[0].Equals(""))
                        lastCategory = values[0];
                    if (!values[1].Equals(""))
                        substance = lastCategory;
                    else
                        substance = values[1];
                    cigPerPuff = ConvertToGrams(values[2]);
                    eCigPerPuff = ConvertToGrams(values[3]);

                    Debug.WriteLine($"{lastCategory}, {substance}, {cigPerPuff}, {eCigPerPuff}");

                }

            }
        }

        [SwaggerOperation(Summary = "Clear the tables and regenerate and populate them.")]
        [HttpGet]
        [Route("GenerateDatabase")]
        public IActionResult GenerateDatabase()
        {
            string deleteAllToxinsQuery = "DELETE FROM Toxins";

            string sqlDatasource = _configuration.GetConnectionString("DBcon");
            using (var connection = new SqliteConnection(sqlDatasource))
            {
                connection.Open();
                using (var deleteCommand = new SqliteCommand(deleteAllToxinsQuery, connection))
                {
                    int rowsAffected = deleteCommand.ExecuteNonQuery();
                    Console.WriteLine($"{rowsAffected} rows deleted from the Toxins table.");
                }
            }
            this.ClearTable("Categories");
            this.ClearTable("Toxins");
            using (var connection = new SqliteConnection(sqlDatasource))
            {
                connection.Open();

                // Create table "Categories"
                string createCategoriesTableQuery = @"
                    CREATE TABLE IF NOT EXISTS Categories (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT NOT NULL
                    );";

                using (var command = new SqliteCommand(createCategoriesTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                    Console.WriteLine("Table 'Categories' created successfully.");
                }

                // Create table Substances
                string createToxinsTableQuery = @"
                    CREATE TABLE IF NOT EXISTS Toxins (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        CategoryId INTEGER NOT NULL,
                        Name TEXT NOT NULL,
                        Description TEXT,
                        FOREIGN KEY (CategoryId) REFERENCES Categories(Id)
                    );";
                using (var command = new SqliteCommand(createToxinsTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                    Console.WriteLine("Table 'Categories' created successfully.");
                }
            }

            this.ParseCSVToxins("../Data/toxins.csv");
            //this.ParseCSVConsumables("../Data/toxins.csv");
            return StatusCode(200, "All good in the hood.");
        }

    }
}