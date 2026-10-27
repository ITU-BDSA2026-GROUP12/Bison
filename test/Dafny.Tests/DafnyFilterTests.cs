using Bison.Razor.Model;
using DafnyAdapters;

namespace Dafny.Tests;

public class DafnyFilterTests {
    [Fact]
    public void FilterBy_ExactTaxon_ReturnsObservation() {
        // Arrange
        var root = new Taxon(1, "mammals");

        var observation = new Observation {
            Taxon = root
        };

        var observations = Dafny.Sequence<ObservationAdapter>.FromElements(new ObservationAdapter(observation));

        // Act
        var result = WrappedDafny.__default.WrappedFilterBy(new TaxonAdapter(root), observations);

        // Assert
        Assert.Single(result);
        Assert.Same(observation, result.Single().OriginalObservation);
    }

    [Fact]
    public void FilterBy_Subtaxon_ReturnsObservation() {
        // Arrange
        var root = new Taxon(1, "mammals");
        var child = new Taxon(2, "canidae", parent: root);
        var grandchild = new Taxon(3, "wolf", parent: child);

        var observation = new Observation {
            Taxon = grandchild
        };

        var observations = Dafny.Sequence<ObservationAdapter>.FromElements(new ObservationAdapter(observation));

        // Act
        var result = WrappedDafny.__default.WrappedFilterBy(new TaxonAdapter(root), observations);

        // Assert
        Assert.Single(result);
        Assert.Same(observation, result.Single().OriginalObservation);
    }

    [Fact]
    public void FilterBy_UnrelatedTaxon_ReturnsEmpty() {
        // Arrange
        var root = new Taxon(1, "canidae");
        var unrelated = new Taxon(2, "bovidae");

        var observation = new Observation {
            Taxon = unrelated
        };

        var observations = Dafny.Sequence<ObservationAdapter>.FromElements(new ObservationAdapter(observation));

        // Act
        var result = WrappedDafny.__default.WrappedFilterBy(new TaxonAdapter(root), observations);

        // Assert
        Assert.Empty(result);
    }
}