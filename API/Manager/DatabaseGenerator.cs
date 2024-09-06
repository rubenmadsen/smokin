using CsvHelper;
using Microsoft.Data.Sqlite;
using System.Globalization;
using System.Text.Json.Serialization;
using System.Data.SQLite;
using API.Models;
using Newtonsoft.Json;


namespace API.Manager
{

    public class DatabaseGenerator
    {
        private readonly string sqlDatasource;
        public DatabaseGenerator(string sqlDatasource)
        {
            this.sqlDatasource = sqlDatasource;
        }



      

        public int GenerateDatabase()
        {
           
            
 
            using (var connection = new SqliteConnection(sqlDatasource))
            {
                connection.Open(); //kolla om den behövs



                // Create table "Categories"  Ändra till två scheman, en för csv en för json
                string createCategoriesTableQuery = @"
                    CREATE TABLE IF NOT EXISTS Categories (
                        
                        categoryName TEXT 
                    );";

                using (var command = new SqliteCommand(createCategoriesTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                    Console.WriteLine("Table 'Categories' created successfully.");
                }

                // Create table Toxins
                string createToxinsTableQuery = @"
                    CREATE TABLE IF NOT EXISTS Toxins (
                        toxinName TEXT PRIMARY KEY,
                        categoryName TEXT REFERENCES Categories(categoryName),
                        description TEXT 
                        
                    );";
                using (var command = new SqliteCommand(createToxinsTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                    Console.WriteLine("Table 'Toxins' created successfully.");
                }

                // Create table Consumables
                string createConsumablesTableQuery = @"
                    CREATE TABLE IF NOT EXISTS Consumables (
                        
                       toxinName TEXT PRIMARY KEY REFERENCES Toxins, 
                        nameConsumable TEXT NOT NULL,
                        amount REAL
                        
                    );";
                using (var command = new SqliteCommand(createConsumablesTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                    Console.WriteLine("Table 'Consumables' created successfully.");
                }
            }

            ParseCSVToxins();
            ParseJSONToxins();
            
            return 0;
        }

       



        public void ParseJSONToxins()
        {
            string filePathJSON = "../Data/toxin_descriptions.json";

            if (!File.Exists(filePathJSON))
            {
                Console.WriteLine("JSON file not found.");
                return;
            }

            try
            {
                using (var connection = new SQLiteConnection(sqlDatasource))
                {
                    connection.Open();

                    var jsonData = File.ReadAllText(filePathJSON);
                    var toxinList = JsonConvert.DeserializeObject<List<Toxin>>(jsonData);

                    
                    foreach (var toxin in toxinList)
                    {
                        string updateQuery = "UPDATE Toxins SET description = @description WHERE toxinName = @toxinName OR categoryName = @toxinName ";

                        Console.WriteLine($"Executing SQL: {updateQuery}");
                        Console.WriteLine($"Parameters: toxinName={toxin.toxin}, description={toxin.Description}");

                        using (var command = new SQLiteCommand(updateQuery, connection))
                        {
                            command.Parameters.AddWithValue("@toxinName", toxin.toxin);
                            command.Parameters.AddWithValue("@description", toxin.Description);
                            
                            command.ExecuteNonQuery();

                          
                        }
                    }

                    connection.Close(); 
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Något blev fel :(");
            }
        }

        private void ParseCSVToxins()
        {
            string filePathCSV = "../Data/toxins.csv";

            if (!File.Exists(filePathCSV))
            {
                Console.WriteLine("CSV file not found.");
                return;
            }

            try
            {
                using (var connection = new SQLiteConnection(sqlDatasource))
                {
                    connection.Open();

                    using (var reader = new StreamReader(filePathCSV))
                    using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                    {
                        
                        csv.Read();
                        csv.ReadHeader();

                        var toxinRecords = new List<Toxin>();
                        var lastCategory = new Category();

                        while (csv.Read())
                        {
                            var toxinName = csv.GetField<string>("Toxic Compound");
                            var categoryName = csv.GetField<string>("Toxic Compound Type");

                            // Use the last non-empty category if the current one is empty
                            if (string.IsNullOrWhiteSpace(categoryName))
                            {
                                categoryName = lastCategory.Name;
                            }
                            else
                            {
                                lastCategory = new Category { Name = categoryName };
                            }

                            // Create a new Toxin object
                            var toxin = new Toxin
                            {
                                toxin = toxinName,
                                Category = lastCategory
                            };

                            toxinRecords.Add(toxin);
                        }

                        // Insert records into the database
                        foreach (var toxin in toxinRecords)
                        {
                            string insertQuery = "INSERT INTO Toxins (toxinName, categoryName) VALUES (@toxinName, @categoryName)";

                            Console.WriteLine($"Executing SQL: {insertQuery}");
                            Console.WriteLine($"Parameters: toxinName={toxin.toxin}, categoryName={toxin.Category.Name}");

                            using (var command = new SQLiteCommand(insertQuery, connection))
                            {
                                command.Parameters.AddWithValue("@toxinName", toxin.toxin);
                                command.Parameters.AddWithValue("@categoryName", toxin.Category.Name);

                                command.ExecuteNonQuery();
                            }
                        }

                        // Verify the insertion
                        string selectQuery = "SELECT COUNT(*) FROM Toxins";
                        using (var selectCommand = new SQLiteCommand(selectQuery, connection))
                        {
                            var count = selectCommand.ExecuteScalar();
                            Console.WriteLine($"Number of rows in Toxins table: {count}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }


        private void AddConsumable(string toxinName, string nameConsumable, float amount)
        {
            using (var connection = new SqliteConnection(sqlDatasource))
            {
                connection.Open();

                string insertConsumableQuery = @"
                    INSERT INTO Consumables (toxinName, nameConsumable, amount)
                    VALUES (@toxinName, @nameConsumable, @amount)";
                using (var command = new SqliteCommand(insertConsumableQuery, connection))
                {
                    command.Parameters.AddWithValue("@toxinName", toxinName);
                    command.Parameters.AddWithValue("@nameConsumable", nameConsumable);
                    command.Parameters.AddWithValue("@amount", amount);
                    int rowsAffected = command.ExecuteNonQuery();
                    Console.WriteLine($"{rowsAffected} row(s) inserted into the Consumables table.");
                }
            }

        }
      

        private void AddToxin(String toxinName, String categoryName, String description)
        {

            using (var connection = new SqliteConnection(sqlDatasource))
            {
                connection.Open();
                string insertToxinQuery = "INSERT INTO Toxins (toxinName, categoryName, description) VALUES (@toxinName, @categoryName, @description)";
                using (var insertToxinCommand = new SqliteCommand(insertToxinQuery, connection))
                {
                    insertToxinCommand.Parameters.AddWithValue("@toxinName", toxinName);
                    insertToxinCommand.Parameters.AddWithValue("@categoryName", categoryName);
                    insertToxinCommand.Parameters.AddWithValue("@description", description);
                   
                }
            }
            
        }
        private void AddCategory(String categoryName)
        {

            using (var connection = new SqliteConnection(sqlDatasource))
            {
                connection.Open();
                string insertCategoryQuery = "INSERT INTO Categories (categoryName) VALUES (@categoryName)";
                using (var insertCategoryCommand = new SqliteCommand(insertCategoryQuery, connection))
                {
                    insertCategoryCommand.Parameters.AddWithValue("@categoryName", categoryName);
                    
                }
            } 
        }

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
    }

}
