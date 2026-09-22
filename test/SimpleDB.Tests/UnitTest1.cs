namespace SimpleDB.Tests;

using Model;

public class UnitTest1
{
    [Fact]
    public void FirstObservationInDBIsCorrect() {
        //Arrange
        var database = CSVDatabase<Observation>.getInstance(@"..\..\..\..\..\src\Bison.CLI\bison_observe_cli_db.csv");
        var records = database.Read();

        //Act
        var result = records.First().Message == "A bird at DR Byen";

        //Assert
        Assert.True(result);
    }
}