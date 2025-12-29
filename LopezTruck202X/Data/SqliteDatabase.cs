using Microsoft.Data.Sqlite;

namespace LopezTruck202X.Data;

public sealed class SqliteDatabase
{
    private readonly string _connectionString;

    public SqliteDatabase(string databasePath)
    {
        _connectionString = new SqliteConnectionStringBuilder
        {
            DataSource = databasePath
        }.ToString();
    }

    public SqliteConnection CreateConnection() => new(_connectionString);
}
