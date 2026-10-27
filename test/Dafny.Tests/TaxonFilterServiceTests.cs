using Bison.Razor;
using Bison.Razor.Model;

namespace Dafny.Tests;

public class TaxonFilterServiceTests {
    private readonly TaxonFilterService _service = new();

    [Fact]
    public void FilterByTaxon_EmptyList_ReturnsEmpty() {
        // Arrange
        var root = new Taxon(1, "mammals");
        var observations = new List<Observation>();

        // Act
        var result = _service.FilterByTaxon(root, observations);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void FilterByTaxon_MixedObservations_ReturnsOnlyMatches() {
        // Arrange
        var root = new Taxon(1, "mammals");
        var child = new Taxon(2, "wolf", parent: root);
        var unrelated = new Taxon(3, "bird");

        var observation1 = new Observation { Taxon = root };
        var observation2 = new Observation { Taxon = unrelated };
        var observation3 = new Observation { Taxon = child };

        var observations = new List<Observation> {
            observation1,
            observation2,
            observation3
        };

        // Act
        var result = _service.FilterByTaxon(root, observations);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Same(observation1, result[0]);
        Assert.Same(observation3, result[1]);
    }

    [Fact]
    public void FilterByTaxon_NullRoot_ThrowsException() {
        // Arrange
        var observations = new List<Observation>();

        // Act and Assert
        Assert.Throws<ArgumentNullException>(() => _service.FilterByTaxon(null!, observations));
    }

    [Fact]
    public void FilterByTaxon_NullObservations_ThrowsException() {
        // Arrange
        var root = new Taxon(1, "mammals");

        // Act and Assert
        Assert.Throws<ArgumentNullException>(() => _service.FilterByTaxon(root, null!));
    }
}