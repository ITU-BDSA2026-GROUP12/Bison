namespace Bison.CLI.Tests;

using Model;
using SimpleDB;

public class UnitTestProgram
{
    [Fact]
    public void CommentsReferencingNonExistingObservationIDsWillNotGetStored() {
        //Arrange
        var database = CSVDatabase<Observation>.getInstance(Config.ObservationDatabase);
        var commentDb = CSVDatabase<Comment>.getInstance(Config.CommentDatabase);

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
        var database = CSVDatabase<Observation>.getInstance(Config.ObservationDatabase);
        var records = database.Read();

        //Act
        var result = records.First().Message == "A bird at DR Byen";

        //Assert
        Assert.True(result);
    }
}