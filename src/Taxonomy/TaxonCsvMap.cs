using CsvHelper.Configuration;
using Model;

namespace Taxonomy;

public sealed class TaxonCsvMap : ClassMap<Taxon> {
    public TaxonCsvMap() {
        Map(t => t.TaxonId)
            .Name("dwc:taxonID");

        Map(t => t.ParentNameUsageId)
            .Name("dwc:parentNameUsageID");

        Map(t => t.AcceptedNameUsageId)
            .Name("dwc:acceptedNameUsageID");

        Map(t => t.TaxonomicStatus)
            .Name("dwc:taxonomicStatus");

        Map(t => t.TaxonRank)
            .Name("dwc:taxonRank");

        Map(t => t.ScientificName)
            .Name("dwc:scientificName");

        Map(t => t.ScientificNameAuthorship)
            .Name("dwc:scientificNameAuthorship");

        Map(t => t.Language)
            .Name("dcterms:language");

        Map(t => t.VernacularName)
            .Name("dwc:vernacularName");

        Map(t => t.Merged)
            .Name("clb:merged");
    }
}