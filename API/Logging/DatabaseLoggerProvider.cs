namespace API.Logging
{
    using Microsoft.Data.Sqlite;
    using Microsoft.Extensions.Logging;
    using System;

    public class DatabaseLoggerProvider : ILoggerProvider
    {
        private readonly Func<string, LogLevel, bool> _filter;
        private readonly string _connectionString;

        public DatabaseLoggerProvider(Func<string, LogLevel, bool> filter, string connectionString)
        {
            _filter = filter;
            _connectionString = connectionString;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new DatabaseLogger(categoryName, _filter, _connectionString);
        }

        public void Dispose() { }
    }

    public class DatabaseLogger : ILogger
    {
        private readonly string _categoryName;
        private readonly Func<string, LogLevel, bool> _filter;
        private readonly string _connectionString;

        public DatabaseLogger(string categoryName, Func<string, LogLevel, bool> filter, string connectionString)
        {
            _categoryName = categoryName;
            _filter = filter;
            _connectionString = connectionString;
        }

        public IDisposable BeginScope<TState>(TState state) => null;

        public bool IsEnabled(LogLevel logLevel) => _filter(_categoryName, logLevel);

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            var message = formatter(state, exception);

            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                var tableExistsCommand = connection.CreateCommand();
                tableExistsCommand.CommandText = "SELECT count(*) FROM sqlite_master WHERE type='table' AND name='Logs'";
                var tableExists = (long)tableExistsCommand.ExecuteScalar() > 0;

                if (!tableExists)
                {
                    var createTableCommand = connection.CreateCommand();
                    createTableCommand.CommandText = @"CREATE TABLE Logs (
                                                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                                LogLevel TEXT,
                                                Category TEXT,
                                                Message TEXT,
                                                TimeStamp DATETIME
                                            )";
                    createTableCommand.ExecuteNonQuery();
                }

                using (var insertCommand = connection.CreateCommand())
                {
                    insertCommand.CommandText = @"INSERT INTO Logs (LogLevel, Category, Message, TimeStamp)
                                      VALUES (@LogLevel, @Category, @Message, @TimeStamp)";
                    insertCommand.Parameters.AddWithValue("@LogLevel", logLevel.ToString());
                    insertCommand.Parameters.AddWithValue("@Category", _categoryName);
                    insertCommand.Parameters.AddWithValue("@Message", message);
                    insertCommand.Parameters.AddWithValue("@TimeStamp", DateTime.UtcNow);

                    insertCommand.ExecuteNonQuery();
                }
            }
        }
    }

}
