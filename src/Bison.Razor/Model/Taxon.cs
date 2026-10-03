// Taxon.cs
namespace Bison.Razor.Model;

public class Taxon
{
    public string DwcTaxonId { get; }
    public string? VernacularName { get; set; } 

    public Taxon? Parent { get; }

    private readonly List<Taxon> _children = new();
    public IReadOnlyList<Taxon> Children => _children;

    public Taxon(string dwcTaxonId, string? vernacularName = null, Taxon? parent = null)
    {
        DwcTaxonId = dwcTaxonId;
        VernacularName = vernacularName;
        Parent = parent;
        parent?._children.Add(this); 
    }
}