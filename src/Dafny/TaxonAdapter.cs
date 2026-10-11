using BisonTaxon = Bison.Razor.Model.Taxon;

namespace DafnyAdapters;

//wrap a Bison taxon so that it can be used by the dafny filter
public class TaxonAdapter
{

    //store reference to original taxon
    public BisonTaxon OriginalTaxon { get; }

    //wrap existing taxon in an adapter
    public TaxonAdapter(BisonTaxon taxon)
    {
        OriginalTaxon = taxon;
    }

    //check whether this taxon is the root taxon or a subtaxon of it.
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