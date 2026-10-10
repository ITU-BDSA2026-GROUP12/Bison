using BisonObservation = Bison.Razor.Model.Observation;

namespace DafnyAdapters;

public class ObservationAdapter
{

    //save original observation
    public BisonObservation OriginalObservation { get; }

    public ObservationAdapter(BisonObservation observation)
    {
        OriginalObservation = observation;
    }

    public TaxonAdapter getTaxon()
    {
        if (OriginalObservation.Taxon == null)
        {
            throw new InvalidOperationException(
                "Observation has no taxon");
        }

        return new TaxonAdapter(OriginalObservation.Taxon);
    }
}
