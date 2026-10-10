using BisonTaxon = Bison.Razor.Model.Taxon;

namespace DafnyAdapters;

public class TaxonAdapter
{

    //save reference for excisting Taxon
    public BisonTaxon OriginalTaxon { get; }

    public TaxonAdapter(BisonTaxon taxon)
    {
        OriginalTaxon = taxon;
    }

    //Check if it adds up with Taxon hierachy 
    public bool isSubTaxon(TaxonAdapter ancestor)
    {
        BisonTaxon? current = OriginalTaxon;

        while (current != null)
        {
            if (current.TaxonId == ancestor.OriginalTaxon.TaxonId)
            {
                return true;
            }

            current = current.Parent;
        }

        return false;
    }
}