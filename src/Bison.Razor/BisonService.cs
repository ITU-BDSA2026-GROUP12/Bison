using Model;
using Bison.Razor;
using Bison.Razor.Repositories;
using Bison.Razor.DTOs;

public interface IObservationService
{
    public List<ObservationDTO> GetObservations(int page);
    public List<ObservationDTO> GetObservationsFromAuthor(string author, int page);
    public ObservationDetailsDTO? GetObservationDetails(int observationId);
}

public class ObservationService : IObservationService
{
    private readonly IPostRepository _repository;

    public ObservationService(IPostRepository repository)
    {
        _repository = repository;
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
                observation.Timestamp
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
                observation.Timestamp
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
            comment.Author,
            comment.Message,
            UnixTimeStampToDateTimeString(comment.Timestamp)
             );

            commentDTOs.Add(dto);
        }

        //convert proposals to DTOs
        var proposalDTOs = new List<ProposalDTO>();
        foreach (var proposal in proposals)
        {
            var dto = new ProposalDTO(
                proposal.Author,
                proposal.TaxonId,
                UnixTimeStampToDateTimeString(proposal.Timestamp)
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
            proposalDTOs
        );
    }

    private static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("dd-MM-yyyy HH:mm:ss");
    }

}
