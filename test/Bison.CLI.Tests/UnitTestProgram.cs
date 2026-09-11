namespace Bison.CLI.Tests;

using Model;
using SimpleDB;

public class UnitTestProgram
{
    [Fact]
    public void CommentsReferencingNonExistingObservationIDsWillNotGetStored() {
        //Arrange
        var database = new CSVDatabase<Observation>("bison_observe_cli_db.csv");
        var commentDb = new CSVDatabase<Comment>("bison_comment_cli_db.csv");

        //Act
        var result = Program.Comment("", -1, database, commentDb);

        //Assert
        Assert.False(result);
    }

    [Fact]
    public void UnixTimestampPrintsCorrectlyInHumanReadableLocalTime() {
        //No Arrange

        //Act
        var result = UserInterface.TimestampToLocalDateString(0);

        //Assert
        Assert.Equal("01/01/70 01:00:00", result);
    }

    [Fact]
    public void FirstObservationInDBIsCorrect() {
        //Arrange
        var database = new CSVDatabase<Observation>("bison_observe_cli_db.csv");
        var records = database.Read();

        //Act
        var result = records.First().Message == "A bird at DR Byen";

        //Assert
        Assert.True(result);
    }
}