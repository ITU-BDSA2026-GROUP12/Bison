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

    // Get individual observation from the database using its ID.
    public ObservationViewModel? GetObservation(int observationId) {
        using var connection = CreateConnection();
        connection.Open();

        var command = connection.CreateCommand();

        command.CommandText = @"
            SELECT observation_id, username, text, pub_date
            FROM observation
            JOIN user
            ON observation.author_id = user.user_id
            WHERE observation.observation_id = $observationId;
        ";

        command.Parameters.AddWithValue("$observationId", observationId);

        using var reader = command.ExecuteReader();

        if (!reader.Read()) {
            return null;
        }

        return new ObservationViewModel(
            reader.GetInt32(0),
            reader.GetString(1),
            reader.GetString(2),
            reader.GetInt64(3).ToString()
        );
    }

    // Gets observations through SQL and makes a list of them to return.
    public List<ObservationViewModel> GetObservations(int page)
    {
        using var connection = CreateConnection();

        connection.Open();

        var command = connection.CreateCommand();

        // We need to determine if we want SQL queries to be capital or lower-case
        command.CommandText = @"
            SELECT observation_id, username, text, pub_date
            FROM observation
            JOIN user
            ON observation.author_id = user.user_id
            limit 32 offset $offset;
        ";

        command.Parameters.AddWithValue("$offset", (page - 1) * 32);

        using var reader = command.ExecuteReader();

        var observations = new List<ObservationViewModel>();

        while (reader.Read())
        {
            var observationId = reader.GetInt32(0);
            var username = reader.GetString(1);
            var text = reader.GetString(2);
            var timestamp = reader.GetInt64(3);

            observations.Add(
                new ObservationViewModel(
                    observationId,
                    username,
                    text,
                    timestamp.ToString()
                )
            );
        }

        return observations;
    }

    // Gets observations from specific author through SQL and makes a list of them to return.
    public List<ObservationViewModel> GetObservationsFromAuthor(string author, int page)
    {
        using var connection = CreateConnection();

        connection.Open();

        var command = connection.CreateCommand();


        
        command.CommandText = @"
            SELECT observation_id, username, text, pub_date
            FROM observation
            JOIN user
            ON observation.author_id = user.user_id
            WHERE username = $author
            limit 32 offset $offset;
        ";
        command.Parameters.AddWithValue("$author", author);
        command.Parameters.AddWithValue("$offset", (page - 1) * 32);

        using var reader = command.ExecuteReader();

        var observations = new List<ObservationViewModel>();

        while (reader.Read())
        {
            var observationId = reader.GetInt32(0);
            var username = reader.GetString(1);
            var text = reader.GetString(2);
            var timestamp = reader.GetInt64(3);

            observations.Add(
                new ObservationViewModel(
                    observationId,
                    username,
                    text,
                    timestamp.ToString()
                )
            );
        }

        return observations;
    }

    // Gets all comments belonging to a specific observation.
    public List<Comment> GetComments(int observationId) {
        using var connection = CreateConnection();
        connection.Open();

        var command = connection.CreateCommand();

        command.CommandText = @"
            SELECT username, text, pub_date
            FROM comment
            JOIN user
            ON comment.author_id = user.user_id
            WHERE comment.observation_id = $observationId;
        ";

        command.Parameters.AddWithValue("$observationId", observationId);

        using var reader = command.ExecuteReader();

        var comments = new List<Comment>();

        // Creates a Comment object for each matching row in the database.
        while (reader.Read()) {
            comments.Add(
                new Comment(
                    observationId,
                    reader.GetString(0),
                    reader.GetString(1),
                    reader.GetInt64(2)
                )
            );
        }

        return comments;
    }

    // Gets all taxon proposals belonging to a specific observation.
    public List<Proposal> GetProposals(int observationId) {
        using var connection = CreateConnection();
        connection.Open();

        var command = connection.CreateCommand();

        command.CommandText = @"
            SELECT username, taxon_id, pub_date
            FROM proposal
            JOIN user
            ON proposal.author_id = user.user_id
            WHERE proposal.observation_id = $observationId;
        ";

        command.Parameters.AddWithValue("$observationId", observationId);

        using var reader = command.ExecuteReader();

        var proposals = new List<Proposal>();

        // Creates a Proposal object for each matching row in the database.
        while (reader.Read()) {
            proposals.Add(
                new Proposal(
                    observationId,
                    reader.GetString(0),
                    reader.GetString(1),
                    reader.GetInt64(2)
                )
            );
        }

        return proposals;
    }
}