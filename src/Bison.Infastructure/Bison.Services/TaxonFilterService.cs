using Bison.Razor.Model;
using DafnyAdapters;

namespace Bison.Razor;

public interface ITaxonFilterService {
    List<Observation> FilterByTaxon(Taxon root, IEnumerable<Observation> observations);
}

public class TaxonFilterService : ITaxonFilterService {
    public List<Observation> FilterByTaxon(Taxon root, IEnumerable<Observation> observations) {
        ArgumentNullException.ThrowIfNull(root);
        ArgumentNullException.ThrowIfNull(observations);

        // Wrap the selected taxon
        var rootAdapter = new TaxonAdapter(root);

        // Wrap the observations
        var observationAdapters = observations.Select(o => new ObservationAdapter(o)).ToArray();

        // Convert the observations to a Dafny sequence
        var dafnyObservations = Dafny.Sequence<ObservationAdapter>.FromElements(observationAdapters);

        // Call the generated Dafny filter
        var result = WrappedDafny.__default.WrappedFilterBy(rootAdapter, dafnyObservations);

        // Return the original C# observations
        return result.Select(adapter => adapter.OriginalObservation).ToList();
    }
}