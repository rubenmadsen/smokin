using CsvHelper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Reflection.PortableExecutable;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace API.Manager
{
    public class Toxin
    {
        [JsonPropertyName("toxin")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }
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
                connection.Open();



                // Create table "Categories"
                string createCategoriesTableQuery = @"
                    CREATE TABLE IF NOT EXISTS Categories (
                        
                        categoryName TEXT PRIMARY KEY
                    );";

                using (var command = new SqliteCommand(createCategoriesTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                    Console.WriteLine("Table 'Categories' created successfully.");
                }

                // Create table Substances
                string createToxinsTableQuery = @"
                    CREATE TABLE IF NOT EXISTS Toxins (
                        toxinName TEXT PRIMARY KEY,
                        categoryName TEXT REFERENCES Categories,
                        description TEXT NOT NULL
                        
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

            //ParseCSVToxins();
            return 0;
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
