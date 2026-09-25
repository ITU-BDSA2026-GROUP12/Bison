using Microsoft.Data.Sqlite;
using Model;

namespace Bison.SQLite;

public class DBFacade
{
    private readonly string _dbPath;

    public DBFacade(string dbPath)
    {
        _dbPath = dbPath;
    }

    private SqliteConnection CreateConnection()
    {
        return new SqliteConnection($"Data Source={_dbPath}");
    }

    public List<ObservationViewModel> GetObservations()
    {
        using var connection = CreateConnection();

        connection.Open();

        var command = connection.CreateCommand();

        // We need to determine if we want SQL queries to be capital or lower-case
        command.CommandText = @"
            SELECT username, text, pub_date
            FROM observation
            JOIN user
            ON observation.author_id = user.user_id;
        ";

        using var reader = command.ExecuteReader();

        var observations = new List<ObservationViewModel>();

        while (reader.Read())
        {
            var username = reader.GetString(0);
            var text = reader.GetString(1);
            var timestamp = reader.GetInt64(2);

            observations.Add(
                new ObservationViewModel(
                    username,
                    text,
                    timestamp.ToString()
                )
            );
        }
    }

    public void TestConnection()
    {
        
    }
}