namespace Bison.Razor.Tests;

using Bison.Razor;
using Bison.Razor.Repositories;

public class BisonRazorObservationUnitTest {

    private readonly string _dbPath = Environment.GetEnvironmentVariable("BISONDBPATH")
            ?? Path.Combine(Path.GetTempPath(), "bison.db");

    [Fact]
    public void PeterFoundABigBird() {

        // Arrange
        DBFacade facade = new DBFacade(_dbPath);
        IPostRepository repository = new PostRepository(facade);
        IObservationService service = new ObservationService(repository);

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
    public void EduardFoundAHeron() {

        // Arrange
        DBFacade facade = new DBFacade(_dbPath);
        IPostRepository repository = new PostRepository(facade);
        IObservationService service = new ObservationService(repository);

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