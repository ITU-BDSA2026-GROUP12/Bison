using BisonObservation = Bison.Razor.Model.Observation;

namespace DafnyAdapters;
//wraps a bison observation so that it can be used by the dafny filter
public class ObservationAdapter
{

    //store reference to original observation
    public BisonObservation OriginalObservation { get; }

    //Wrap an existing Bison observation in an adapter
    public ObservationAdapter(BisonObservation observation)
    {
        OriginalObservation = observation;
    }

    //provide the getTaxon() method that the Dafny filter needs
    public TaxonAdapter getTaxon()
    {
        if (OriginalObservation.Taxon == null)
        {
            throw new InvalidOperationException(
                "Observation has no taxon");
        }
        //wrap the original Taxon in TaxonAdapter
        return new TaxonAdapter(OriginalObservation.Taxon);
    }
}
