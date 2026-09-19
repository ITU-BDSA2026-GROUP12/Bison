namespace Model;

public class Taxon {
    public string TaxonId { get; set; } = string.Empty;

    public string? ParentNameUsageId { get; set; }

    public string? AcceptedNameUsageId { get; set; }

    public string TaxonomicStatus { get; set; } = string.Empty;

    public string TaxonRank { get; set; } = string.Empty;

    public string ScientificName { get; set; } = string.Empty;

    public string? ScientificNameAuthorship { get; set; }

    public string? Language { get; set; }

    public string? VernacularName { get; set; }

    public string? Merged { get; set; }
}