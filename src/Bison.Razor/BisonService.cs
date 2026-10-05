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

        var observationDTOs = new List<ObservationDTO>();

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

        return observationDTOs;
    }

    public List<ObservationDTO> GetObservationsFromAuthor(string author, int page)
    {
        var observations = _repository.GetObservationsFromAuthor(author, page);
        var observationDTOs = new List<ObservationDTO>();

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

        return observationDTOs;

    }

    // Collects all information needed for the observation details page.
    public ObservationDetailsDTO? GetObservationDetails(int observationId)
    {
        //get observations
        var observation = _repository.GetObservation(observationId);

        if (observation == null)
        {
            return null;
        }

        //get comments
        var comments = _repository.GetComments(observationId);
        //get proposals
        var proposals = _repository.GetProposals(observationId);

        //convert comments to DTOs
        var commentDTOs = new List<CommentDTO>();

        //convert proposals to DTOs
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

        //return everything as one ObservationDetailsDTO
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
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }

}
