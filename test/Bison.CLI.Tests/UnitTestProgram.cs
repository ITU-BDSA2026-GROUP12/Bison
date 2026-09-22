namespace Bison.CLI.Tests;

using Model;

public class UnitTestProgram
{
    [Fact]
    public async Task CommentsReferencingNonExistingObservationIDsWillNotGetStored() {
        // Test no longer applicable here, webservice will need this test
    }

    [Fact]
    public void UnixTimestampPrintsCorrectlyInHumanReadableLocalTime() {
        //No Arrange

        //Act
        var result = UserInterface.TimestampToUnixDateString(0);

        //Assert
        Assert.Equal("01/01/70 00:00:00", result);
    }

    // ===== LOCATION TESTS ===== //
    [Fact]
    public void ObservationCanBeCreatedWithLocation() {
        // Arrange & Act
        var observation = new Observation(1, "testAuthor", "Golden goose!", 1234567890, "Vordingborg");

        // Assert
        Assert.Equal("Vordingborg", observation.Location);
    }

    [Fact]
    public void ObservationWithoutLocationDefaultsToUnknown() {
        // Arrange & Act
        var observation = new Observation(1, "testAuthor", "Nevermind, no golden goose", 1234567890);

        // Assert
        Assert.Equal("unknown", observation.Location);
    }

    [Fact]
    public void LocationSearchIgnoresCapitalization() {
        // Arrange
        var observations = new List<Observation> {
            new Observation(1, "author1", "Bird one", 1234567890, "Rødby")
        };

        // Act
        var result = Program.GetObservationsForLocation(observations, "rØdBy").ToList();

        // Assert
        Assert.Single(result);
        Assert.Equal("Rødby", result[0].Location);
    }

    [Fact]
    public void PrintObservationsDisplaysLocation() {
        // Arrange
        var observation = new Observation(1, "testAuthor", "I don't even know what I saw", 1234567890, "Næstved");

        var output = new StringWriter();    // Stores captured console output.
        var originalOutput = Console.Out;   // Save the normal console output.

        try {
            Console.SetOut(output);

            // Act
            UserInterface.PrintObservations(new List<Observation> { observation });
        }
        finally {
            Console.SetOut(originalOutput);
        }

        // Assert
        Assert.Contains("Næstved", output.ToString());
    }

    [Fact]
    public void LocationSearchReturnsMatchingObservations() {
        // Arrange
        var observations = new List<Observation> {
            new Observation(1, "author1", "Bird one", 1234567890, "Billund"),
            new Observation(2, "author2", "Bird two", 1234567891, "Billund"),
            new Observation(3, "author3", "Bird three", 1234567892, "Ribe")
        };

        // Act
        var result = Program.GetObservationsForLocation(observations, "Billund").ToList();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, o => o.ObservationId == 1);
        Assert.Contains(result, o => o.ObservationId == 2);
        Assert.DoesNotContain(result, o => o.ObservationId == 3);
    }

    [Fact]
    public void LocationSearchWithNoMatchesReturnsNoObservations() {
        // Arrange
        var observations = new List<Observation> {
            new Observation(1, "author1", "Bird one", 1234567890, "Skagen"),
            new Observation(2, "author2", "Bird two", 1234567891, "Odense")
        };

        // Act
        var result = Program.GetObservationsForLocation(observations, "København").ToList();

        // Assert
        Assert.Empty(result);
    }
}