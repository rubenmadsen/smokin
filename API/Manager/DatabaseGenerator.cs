using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using API.Models;
namespace API.Manager
{
   
    public class DatabaseGenerator
    {
        private readonly string sqlDatasource;
        public DatabaseGenerator(string sqlDatasource)
        {
            this.sqlDatasource = sqlDatasource;
        }

        private void ClearTable(string sqlDatasource, string tableName)
        {
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

        public int GenerateDatabase()
        {
            this.ClearTable(sqlDatasource, "Toxins");
            string deleteAllToxinsQuery = "DELETE FROM Toxins";
            
            //using (var connection = new SqliteConnection(this.sqlDatasource))
            //{
            //    connection.Open();
            //    using (var deleteCommand = new SqliteCommand(deleteAllToxinsQuery, connection))
            //    {
            //        int rowsAffected = deleteCommand.ExecuteNonQuery();
            //        Console.WriteLine($"{rowsAffected} rows deleted from the Toxins table.");
            //    }
            //}
            this.ClearTable(sqlDatasource, "Categories");
            
            this.ClearTable(sqlDatasource, "Consumables");
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

                // Create table Toxins
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

                // Create table Consumables
                string createConsumablesTableQuery = @"
                    CREATE TABLE IF NOT EXISTS Consumables (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        ToxinId INTEGER NOT NULL,
                        Name TEXT NOT NULL,
                        Amount REAL,
                        FOREIGN KEY (ToxinId) REFERENCES Toxins(Id)
                    );";
                using (var command = new SqliteCommand(createConsumablesTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                    Console.WriteLine("Table 'Consumables' created successfully.");
                }
            }

            this.ParseCSVToxins("../Data/toxins.csv");
            this.ParseCSVConsumables("../Data/toxins.csv");
            return 0;
        }
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
                        this.AddToxin(toxin, desc, curCategoryId);
                    }
                }
            }
        }
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
                    string substance = values[1];
                    
                    
                    if (values[1].Equals(""))
                        substance = values[0];

                    
                    using (var connection = new SqliteConnection(sqlDatasource))
                    {
                        connection.Open();
                        string getToxinQuery = "SELECT * FROM Toxins WHERE Name = @Name";
                        using (var command = new SqliteCommand(getToxinQuery, connection))
                        {
                            command.Parameters.AddWithValue("@Name", substance);

                            object result = command.ExecuteScalar();
                            
                            if (result != null)
                            {
                                int toxinId = Convert.ToInt32(result);
                                Console.WriteLine($"Toxin '{substance}' found with Id: {toxinId}");
                                if (!values[2].Equals("-"))
                                {
                                    Debug.WriteLine("Adding cigarette");
                                    this.AddConsumable("Cigarette", toxinId, ConvertToGrams(values[2]));
                                }
                                if (!values[3].Equals("-"))
                                {
                                    Debug.WriteLine("Adding E-cigarette");
                                    this.AddConsumable("E-Cigarette", toxinId, ConvertToGrams(values[2]));
                                }
                            }
                            else
                            {
                                Console.WriteLine($"No toxin found with the name '{substance}'.");
                            }
                            
                        }
                    }
                        
                    
                    //float cigPerPuff = ConvertToGrams(values[2]);
                    //float eCigPerPuff = ConvertToGrams(values[3]);

                    //Debug.WriteLine($"{lastCategory}, {substance}, {cigPerPuff}, {eCigPerPuff}");

                }

            }
        }
        private void AddConsumable(string name, int toxinId, float amount)
        {
            using (var connection = new SqliteConnection(sqlDatasource))
            {
                connection.Open();

                string insertConsumableQuery = @"
                    INSERT INTO Consumables (ToxinId, Name, Amount)
                    VALUES (@ToxinId, @Name, @Amount)";
                using (var command = new SqliteCommand(insertConsumableQuery, connection))
                {
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@ToxinId", toxinId);
                    command.Parameters.AddWithValue("@Amount", amount);
                    int rowsAffected = command.ExecuteNonQuery();
                    Console.WriteLine($"{rowsAffected} row(s) inserted into the Consumables table.");
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

        private int AddToxin(String toxinName, String description, int category)
        {

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
        private int AddCategory(String categoryName)
        {
            int id = -1;
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
    }
}
