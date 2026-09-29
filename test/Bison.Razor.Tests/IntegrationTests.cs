using Model;
using Bison.SQLite;
using Microsoft.Data.Sqlite;

//follows arrange --> act --> assert
namespace Bison.Razor.Tests;

public class IntergrationTests
{
    [Fact]
    //test that we want to get a specifc observation from ID
    public void GetObservationReturnsObservationFromDatabase()
    {
        //make a temporary database file for the test
        var dbPath = Path.GetTempFileName();

        try
        {
            //open a connection to the temporary SQLite database
            using (var connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                //open so we can send SQL commands to the database
                connection.Open();

                //create the tables needed by DBFacade.GetObservation() and insert some test data
                var command = connection.CreateCommand();

                command.CommandText = @"
        CREATE TABLE user (
            user_id INTEGER PRIMARY KEY AUTOINCREMENT,
            username STRING NOT NULL,
            email STRING NOT NULL
        );

        CREATE TABLE observation (
            observation_id INTEGER PRIMARY KEY AUTOINCREMENT,
            author_id INTEGER NOT NULL,
            text STRING NOT NULL,
            pub_date INTEGER
        );

        INSERT INTO user (username, email)
        VALUES ('Thea', 'thea@test.dk');

        INSERT INTO observation (author_id, text, pub_date)
        VALUES (1, 'I saw a cat', 1234567890);
    ";
                //execute our commands
                command.ExecuteNonQuery();
            }

            //retrieve observation 1
            var database = new DBFacade(dbPath);
            var result = database.GetObservation(1);

            //verify that the expected observation was retrieved
            Assert.NotNull(result);
            Assert.Equal(1, result.ObservationId);
            Assert.Equal("Thea", result.Author);
            Assert.Equal("I saw a cat", result.Message);
        }
        finally
        {
            //delete temporary database, even if the test fails
            File.Delete(dbPath);
        }
    }

    //test that a page of observations can be retrieved from the database
    [Fact]
    public void GetObservationsReturnsExpectedObservation()
    {
        var dbPath = Path.GetTempFileName();

        try
        {
            //open a connection to the temporary SQLite database
            using (var connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();
                var command = connection.CreateCommand();

                command.CommandText = @"
        CREATE TABLE user (
            user_id INTEGER PRIMARY KEY AUTOINCREMENT,
            username STRING NOT NULL,
            email STRING NOT NULL
        );

        CREATE TABLE observation (
            observation_id INTEGER PRIMARY KEY AUTOINCREMENT,
            author_id INTEGER NOT NULL,
            text STRING NOT NULL,
            pub_date INTEGER
        );

        INSERT INTO user (username, email)
        VALUES ('Thea', 'thea@test.dk');

        INSERT INTO observation (author_id, text, pub_date)
        VALUES (1, 'I saw a cat', 1234567890);
    ";

                command.ExecuteNonQuery();
            }

            var database = new DBFacade(dbPath);

            //retrieve first page of observations
            var result = database.GetObservations(1);

            //verify that observations are retrurned
            Assert.NotEmpty(result);

            //verify that the expected observation excists in the result
            Assert.Contains(result, observation =>
                observation.Author == "Thea" &&
                observation.Message == "I saw a cat");

        }
        finally
        {
            File.Delete(dbPath);
        }
    }
}