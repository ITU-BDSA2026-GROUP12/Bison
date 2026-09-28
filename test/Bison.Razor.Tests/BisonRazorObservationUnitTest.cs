namespace Bison.Razor.Tests;

using Bison.SQLite;

public class BisonRazorObservationUnitTest {
    
    
    [Fact]
    public void TheaTommySplitTest() {
        Assert.Equal("Thea tests above this fact. Tommy tests below", "Thea tests above this fact. Tommy tests below");
    }

    //TODO: Definition placed down here to avoid merge conflicts. Will be tidied up after merge
    private readonly string _dbPath = Path.GetFullPath(Path.Combine(
            AppContext.BaseDirectory,
            "..", "..", "..", "..", "..",
            "src", "Bison.SQLite", "data", "bison.db"));

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