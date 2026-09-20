using System.Globalization;
using System.Reflection;
using CsvHelper;
using Model;

namespace Taxonomy;

public sealed class TaxonomyStore {
    private const string TaxonomyResourceName = "Taxonomy.joined.csv";

    private readonly Dictionary<string, Taxon> _taxaById;

    private readonly Dictionary<string, Taxon> _taxaByVernacularName;

    private readonly Dictionary<string, List<Taxon>> _childrenByParentId;

    // Loads the taxonomy from the embedded CSV resource.
    public TaxonomyStore() : this(LoadTaxa()) {}

    // Creates the lookup structures from a collection of taxa.
    // This constructor also makes the class easier to unit test without loading the real embedded CSV file.
    public TaxonomyStore(IEnumerable<Taxon> taxa) {
        var taxonList = taxa.ToList();

        // Index taxa by taxon ID.
        _taxaById = taxonList.ToDictionary(taxon => taxon.TaxonId);

        // Taxa stored by their vernacular name. Currently it's only by their Danish vernacular name.
        _taxaByVernacularName = new Dictionary<string, Taxon>(StringComparer.OrdinalIgnoreCase);

        foreach (var taxon in taxonList) {
            if (taxon.Language == "dan" && !string.IsNullOrWhiteSpace(taxon.VernacularName)) {
                _taxaByVernacularName.Add(taxon.VernacularName, taxon);
            }
        }

        // Group taxa by parent ID so direct subtaxa can be retrieved efficiently.
        _childrenByParentId = taxonList
            .Where(taxon => !string.IsNullOrWhiteSpace(taxon.ParentNameUsageId))
            .GroupBy(taxon => taxon.ParentNameUsageId!)
            .ToDictionary(group => group.Key, group => group.ToList());
    }

    public Taxon? GetById(string taxonId) {
        return _taxaById.GetValueOrDefault(taxonId);
    }

    public Taxon? GetByVernacularName(string vernacularName) {
        return _taxaByVernacularName.GetValueOrDefault(vernacularName);
    }

    public Taxon? GetSupertaxon(string taxonId) {
        var taxon = GetById(taxonId);

        if (taxon == null || string.IsNullOrWhiteSpace(taxon.ParentNameUsageId)) {
            return null;
        }

        return GetById(taxon.ParentNameUsageId);
    }

    public IReadOnlyList<Taxon> GetSubtaxa(string taxonId) {
        if (_childrenByParentId.TryGetValue(taxonId, out var children)) {
            return children;
        }

        return Array.Empty<Taxon>();
    }

    // Reads the taxonomy CSV from the compiled assembly and converts each CSV record into a Taxon object.
    private static List<Taxon> LoadTaxa() {
        Assembly assembly = typeof(TaxonomyStore).Assembly;

        using Stream stream = assembly.GetManifestResourceStream(TaxonomyResourceName)
            ?? throw new InvalidOperationException("Could not find embedded taxonomy CSV.");

        using var reader = new StreamReader(stream);

        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        csv.Context.RegisterClassMap<TaxonCsvMap>();

        return csv.GetRecords<Taxon>().ToList();
    }
}