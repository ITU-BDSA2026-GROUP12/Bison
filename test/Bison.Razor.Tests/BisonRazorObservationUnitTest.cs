namespace Bison.Razor.Tests;

using Bison.Razor;
using Bison.Razor.Repositories;
using Microsoft.EntityFrameworkCore;

public class BisonRazorObservationUnitTest {

    private static BisonDBContext CreateContext() {
        var options = new DbContextOptionsBuilder<BisonDBContext>().UseSqlite("Data Source=:memory:").Options;

        var context = new BisonDBContext(options);

        context.Database.OpenConnection();
        context.Database.EnsureCreated();

        DbInitializer.SeedDatabase(context);

        return context;
    }

    [Fact]
    public void SeededObservationCanBeFound() {
        // Arrange
        using var context = CreateContext();

        IPostRepository repository = new PostRepository(context);
        IObservationService service = new ObservationService(repository);

        // Act
        var observations = service.GetObservations(1);

        // Assert
        Assert.NotEmpty(observations);
    }

    [Fact]
    public void KnownSeededObservationCanBeFound() {
        // Arrange
        using var context = CreateContext();

        IPostRepository repository = new PostRepository(context);
        IObservationService service = new ObservationService(repository);

        // Act
        var observations = service.GetObservations(1);

        // Assert
        Assert.Contains(
            observations,
            observation => observation.Author == "Wendell Ballan"
        );
    }

    [Fact]
    public void ObservationsCanBeFilteredByAuthor() {
        // Arrange
        using var context = CreateContext();

        IPostRepository repository = new PostRepository(context);
        IObservationService service = new ObservationService(repository);

        // Act
        var observations = service.GetObservationsFromAuthor("Wendell Ballan", 1);

        // Assert
        Assert.NotEmpty(observations);

        Assert.All(
            observations,
            observation => Assert.Equal("Wendell Ballan", observation.Author)
        );
    }

    [Fact]
    public void ObservationCanBeRetrievedById() {
        // Arrange
        using var context = CreateContext();

        IPostRepository repository = new PostRepository(context);

        // Act
        var observation = repository.GetObservation(3);

        // Assert
        Assert.NotNull(observation);
        Assert.Equal(3, observation.ObservationId);
    }
    
    [Fact]
    public void ObservationHasSeededComments() {
        // Arrange
        using var context = CreateContext();

        IPostRepository repository = new PostRepository(context);

        // Act
        var comments = repository.GetComments(3);

        // Assert
        Assert.NotEmpty(comments);
    }
}