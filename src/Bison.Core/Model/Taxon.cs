namespace Bison.Razor.Model;

public class Taxon
{
    public int TaxonId { get; set; }
    public string DwcTaxonId { get; set; } = string.Empty;
    public string dwc_TaxonID {
        get => DwcTaxonId;
        set => DwcTaxonId = value;
    }
    public string? VernacularName { get; set; } 
    public int? ParentId { get; set; }
    public Taxon? Parent { get; set; }
    private readonly List<Taxon> _children = new();
    public List<Taxon> Children => _children;

    public Taxon() {}
    
    
    public Taxon(int taxonId, string dwcTaxonId, string? vernacularName = null, Taxon? parent = null)
    {
        TaxonId = taxonId;
        DwcTaxonId = dwcTaxonId;
        VernacularName = vernacularName;
        Parent = parent;
        parent?._children.Add(this); 
    }
}