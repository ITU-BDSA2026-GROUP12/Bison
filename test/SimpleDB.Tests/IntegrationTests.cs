using Model;
using SimpleDB;

//follows arrange --> act --> assert
namespace SimpleDB.Tests;
public class IntergrationTests{
    //test 1
    [Fact]
    public void StoredObservationCanBeReadAgain()
    {
        var filePath = Path.GetTempFileName(); //(Arrange)temporary file

        //add CSV header needed by CsvHelper when reading the file.
        File.WriteAllText(
        filePath,"ObservationId,Author,Message,Timestamp\n"
        );

        //create database using temporary file
        var database = new CSVDatabase<Observation>(filePath);

        //create new test observation
        var observation = new Observation(
            1,
            "Thea",
            "I saw a cat",
            1234567890
        );

        //(act)Store the observation in the CSV database
        database.Store(observation);

        //Read the first observation back from the database
        var result = database.Read().First();

        //(assert)check that the stored and retrieved observations are equal.
        Assert.Equal(observation, result);
    }

    //test two
    [Fact]
    public void ReadWithLimitReturnsCorrectNumberOfObservations()
    {
         var filePath = Path.GetTempFileName(); //(Arrange)temporary file

        //add CSV header needed by CsvHelper when reading the file.
        File.WriteAllText(
        filePath, "ObservationId,Author,Message,Timestamp\n"
        );

        //create database using temporary file
        var database = new CSVDatabase<Observation>(filePath);

        //create three new test observations
        var Observation1 = new Observation(
            1,
            "Thea",
            "I saw a cat",
            1234567890
        );

        var Observation2 = new Observation(
            2,
            "Frodo",
            "I saw a dragon",
            1234567891
        );

        var Observation3 = new Observation(
            3,
            "Gandalf",
            "I saw a troll",
            1234567892
        );
        
        //Store the observation in the CSV database
        database.Store(Observation1);
        database.Store(Observation2);
        database.Store(Observation3);

        //read observations from database but return only two
        var result = database.Read(2);
        
        //we should get only two observations back
        Assert.Equal(2,result.Count());
    }
}