using Model;
using Taxonomy;

namespace Taxonomy.Tests.Unit;

public class TaxonomyUnitTest {
    private const string PelecaniformesId = "pelecaniformes";
    private const string ArdeidaeId = "ardeidae";
    private const string ArdeaId = "ardea";
    private const string ArdeaCinereaId = "ardea-cinerea";

    private static TaxonomyStore CreateTaxonomy() {
        var taxa = new List<Taxon> {
            new() {
                TaxonId = PelecaniformesId,
                ScientificName = "Pelecaniformes",
                VernacularName = "Årefodede",
                Language = "dan",
                TaxonRank = "order"
            },

            new() {
                TaxonId = ArdeidaeId,
                ParentNameUsageId = PelecaniformesId,
                ScientificName = "Ardeidae",
                VernacularName = "Hejrer",
                Language = "dan",
                TaxonRank = "family"
            },

            new() {
                TaxonId = ArdeaId,
                ParentNameUsageId = ArdeidaeId,
                ScientificName = "Ardea",
                TaxonRank = "genus"
            },

            new() {
                TaxonId = ArdeaCinereaId,
                ParentNameUsageId = ArdeaId,
                ScientificName = "Ardea cinerea",
                VernacularName = "Fiskehejre",
                Language = "dan",
                TaxonRank = "species"
            }
        };

        return new TaxonomyStore(taxa);
    }

    [Fact]
    public void GetByIdReturnsTaxonWhenIdExists() {
        // Arrange
        var taxonomy = CreateTaxonomy();

        // Act
        var taxon = taxonomy.GetById(ArdeidaeId);

        // Assert
        Assert.NotNull(taxon);
        Assert.Equal("Ardeidae", taxon.ScientificName);
        Assert.Equal("Hejrer", taxon.VernacularName);
    }

    [Fact]
    public void GetByIdReturnsNullWhenIdDoesNotExist() {
        // Arrange
        var taxonomy = CreateTaxonomy();

        // Act
        var taxon = taxonomy.GetById("invalid-id");

        // Assert
        Assert.Null(taxon);
    }

    [Fact]
    public void GetByVernacularNameReturnsTaxonWhenNameExists() {
        // Arrange
        var taxonomy = CreateTaxonomy();

        // Act
        var taxon = taxonomy.GetByVernacularName("Fiskehejre");

        // Assert
        Assert.NotNull(taxon);
        Assert.Equal("Ardea cinerea", taxon.ScientificName);
    }

    [Fact]
    public void GetSupertaxonReturnsDirectParent() {
        // Arrange
        var taxonomy = CreateTaxonomy();

        // Act
        var parent = taxonomy.GetSupertaxon(ArdeidaeId);

        // Assert
        Assert.NotNull(parent);
        Assert.Equal("Pelecaniformes", parent.ScientificName );
    }

    [Fact]
    public void GetSubtaxaReturnsDirectChildren() {
        // Arrange
        var taxonomy = CreateTaxonomy();

        // Act
        var children = taxonomy.GetSubtaxa(ArdeidaeId);

        // Assert
        Assert.Contains(children, taxon => taxon.ScientificName == "Ardea");
    }

    [Fact]
    public void GetSubtaxaDoesNotReturnIndirectDescendants() {
        // Arrange
        var taxonomy = CreateTaxonomy();

        // Act
        var children = taxonomy.GetSubtaxa(ArdeidaeId);

        // Assert
        Assert.DoesNotContain(children, taxon => taxon.ScientificName == "Ardea cinerea");
    }

    [Fact]
    public void GetSupertaxonReturnsNullWhenTaxonHasNoParent() {
        // Arrange
        var taxonomy = CreateTaxonomy();

        // Act
        var parent = taxonomy.GetSupertaxon(PelecaniformesId);

        // Assert
        Assert.Null(parent);
    }

    [Fact]
    public void CanLoadEmbeddedTaxonomy() {
        // Arrange
        var taxonomy = new TaxonomyStore();

        // Act
        var taxon = taxonomy.GetByVernacularName("Fiskehejre");

        // Assert
        Assert.NotNull(taxon);
        Assert.Equal("Ardea cinerea", taxon.ScientificName);
    }
}