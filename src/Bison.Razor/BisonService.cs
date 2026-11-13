using Bison.Razor.Model;
using Bison.Razor;
using Bison.Razor.Repositories;
using Bison.Razor.DTOs;
using Humanizer;

public interface IObservationService {
    public List<ObservationDTO> GetObservations(int page);
    public List<ObservationDTO> GetObservationsFromAuthor(string author, int page);
    public ObservationDetailsDTO? GetObservationDetails(int observationId);

    public void CreateObervation(string author, string message);
    public void PostComment(string author, int observationId, string message);
    public List<Bison.Razor.Model.Taxon> GetTaxons();
    public List<ObservationDTO> GetTimelineObservations(int page, int? taxonId = null, string? author = null);
}

public class ObservationService : IObservationService {
    private readonly IPostRepository _repository;

    private readonly ITaxonFilterService _taxonFilter;

    public ObservationService(IPostRepository repository, ITaxonFilterService? taxonFilter = null) {
        _repository = repository;
        _taxonFilter = taxonFilter ?? new TaxonFilterService();
    }

    public void CreateObervation(string author, string message) {
        _repository.CreateObservation(author, message, DateTime.UtcNow);
    }

    public List<ObservationDTO> GetObservations(int page) {
        return _repository.GetObservations(page);
    }

    public void PostComment(string author, int observationId, string message) {
        _repository.PostComment(author, observationId, message, DateTime.UtcNow);
    }

    public List<ObservationDTO> GetObservationsFromAuthor(string author, int page) {
        return _repository.GetObservationsFromAuthor(author, page);

    }

    // Collects all information needed for the observation page.
    public ObservationDetailsDTO? GetObservationDetails(int observationId) {
        //get observation from repository
        var observation = _repository.GetObservation(observationId);

        if (observation == null) {
            return null;
        }

        //Get comments and proposals as DTO's
        var commentDTOs = _repository.GetComments(observationId);
        var proposalDTOs = _repository.GetProposals(observationId);

        //Combine observation, comments and proposals into one DTO
        return new ObservationDetailsDTO(
            observationId,
            observation.Author,
            observation.Message,
            observation.Timestamp,
            commentDTOs,
            proposalDTOs,
            observation.PictureUrl
        );
    }

    private static string UnixTimeStampToDateTimeString(double unixTimeStamp) {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("dd-MM-yyyy HH:mm:ss");
    }

    public List<Bison.Razor.Model.Taxon> GetTaxons() {
        return _repository.GetTaxons();
    }

    public List<ObservationDTO> GetTimelineObservations(int page, int? taxonId = null, string? author = null) {
        page = Math.Max(1, page);

        // If no filter is selected, use the existing repository pagination.
        if (taxonId == null) {
            return author == null ? GetObservations(page) : GetObservationsFromAuthor(author, page);
        }

        var selectedTaxon = _repository.GetTaxons().FirstOrDefault(t => t.TaxonId == taxonId.Value);

        if (selectedTaxon == null) {
            return new List<ObservationDTO>();
        }

        var observations = _repository.GetObservationsForFiltering(author);

        // The existing Dafny filter expects a taxon on every observation.
        var candidates = observations.Where(o => o.Taxon != null).ToList();

        // Reuse the existing filter.
        var filtered = _taxonFilter.FilterByTaxon(selectedTaxon, candidates);

        // Paginate AFTER filtering.
        return filtered
            .OrderBy(o => o.PostId)
            .Skip((page - 1) * 32)
            .Take(32)
            .Select(o => new ObservationDTO(
                o.PostId,
                o.Author.Name,
                o.Text,
                o.TimeStamp.ToString(),
                o.PictureUrl
            ))
            .ToList();
    }
}
