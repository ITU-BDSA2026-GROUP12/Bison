using Bison.Razor.Model;
using Bison.Razor;
using Bison.Razor.Repositories;
using Bison.Razor.DTOs;
using Humanizer;

public interface IObservationService
{
    public List<ObservationDTO> GetObservations(int page);
    public List<ObservationDTO> GetObservationsFromAuthor(string author, int page);
    public ObservationDetailsDTO? GetObservationDetails(int observationId);

    public void CreateObervation(string author, string message);
    public List<Bison.Razor.Model.Taxon> GetTaxons();
    public List<ObservationDTO> GetTimelineObservations(int page, int? taxonId = null, string? author = null);
}

public class ObservationService : IObservationService
{
    private readonly IPostRepository _repository;

    private readonly ITaxonFilterService _taxonFilter;

    public ObservationService(IPostRepository repository, ITaxonFilterService? taxonFilter = null)
    {
        _repository = repository;
        _taxonFilter = taxonFilter ?? new TaxonFilterService();
    }

    public void CreateObervation(string author, string message) {
        DateTime timestamp = DateTime.UtcNow;
    }

    public List<ObservationDTO> GetObservations(int page)
    {
        //get our original observations so we can turn them into DTOs
        var observations = _repository.GetObservations(page);

        //empty list for the DTOs that will be sent to the view
        var observationDTOs = new List<ObservationDTO>();

        //convert each observation into a ObservationDTO
        foreach (var observation in observations)
        {
            var dto = new ObservationDTO(
                observation.ObservationId,
                observation.Author,
                observation.Message,
                observation.Timestamp,
                observation.PictureUrl
            );

            observationDTOs.Add(dto);
        }

        //return observationDTOs instead of original observations
        return observationDTOs;
    }

    public List<ObservationDTO> GetObservationsFromAuthor(string author, int page)
    {
        //get observations for a specific author
        var observations = _repository.GetObservationsFromAuthor(author, page);
        //empty list for the DTOs 
        var observationDTOs = new List<ObservationDTO>();

        //Convert observation into ObservationDTO
        foreach (var observation in observations)
        {
            var dto = new ObservationDTO(
                observation.ObservationId,
                observation.Author,
                observation.Message,
                observation.Timestamp,
                observation.PictureUrl
            );

            observationDTOs.Add(dto);
        }
        //return observationDTOs instead of original observations
        return observationDTOs;

    }

    // Collects all information needed for the observation page.
    public ObservationDetailsDTO? GetObservationDetails(int observationId)
    {
        //get observation from repository
        var observation = _repository.GetObservation(observationId);

        if (observation == null)
        {
            return null;
        }

        //get comments and proposals belonging to the observation
        var comments = _repository.GetComments(observationId);
        var proposals = _repository.GetProposals(observationId);

        //convert comments to DTOs
        var commentDTOs = new List<CommentDTO>();
        foreach (var comment in comments)
        {
            var dto = new CommentDTO(
            comment.Author.Name,
            comment.Text,
            comment.TimeStamp.ToString()
             );

            commentDTOs.Add(dto);
        }

        //convert proposals to DTOs
        var proposalDTOs = new List<ProposalDTO>();
        foreach (var proposal in proposals)
        {
            var dto = new ProposalDTO(
                proposal.Author.Name,
                proposal.Taxon.DwcTaxonId,
                proposal.TimeStamp.ToString()
            );
            proposalDTOs.Add(dto);
        }

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

    private static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
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
