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

    // Gets observations through SQL and makes a list of them to return.
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

        return observations;
    }

    // Gets observations from specific author through SQL and makes a list of them to return.
    public List<ObservationViewModel> GetObservationsFromAuthor(string author)
    {
        using var connection = CreateConnection();

        connection.Open();

        var command = connection.CreateCommand();

        command.CommandText = @"
            SELECT username, text, pub_date
            FROM observation
            JOIN user
            ON observation.author_id = user.user_id
            WHERE username = $author;
        ";

        command.Parameters.AddWithValue("$author", author);

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

        return observations;
    }
}