namespace Bison.Razor.Tests;

using Bison.SQLite;

public class BisonRazorObservationUnitTest {

    private readonly string _dbPath = Environment.GetEnvironmentVariable("BISONDBPATH")
            ?? Path.Combine(Path.GetTempPath(), "bison.db");

    [Fact]
    public async Task PeterFoundABigBird() {

        // Arrange
        DBFacade facade = new DBFacade(_dbPath);
        IObservationService service = new ObservationService(facade);

        // Act
        bool observationFound = false;
        var observations = service.GetObservations(1);
        foreach (var obs in observations) {
            if (obs.Author == "Peter" && obs.Message.ToLower() == "a big bird") {
                observationFound = true;
                break;
            }
        }

        // Assert
        Assert.True(observationFound);
    }

    [Fact]
    public async Task EduardFoundAHeron() {

        // Arrange
        DBFacade facade = new DBFacade(_dbPath);
        IObservationService service = new ObservationService(facade);

        // Act
        bool observationFound = false;
        var observations = service.GetObservationsFromAuthor("Eduard", 1);
        foreach (var obs in observations) {
            if (obs.Message.ToLower() == "a heron") {
                observationFound = true;
                break;
            }
        }

        // Assert
        Assert.True(observationFound);
    }

}