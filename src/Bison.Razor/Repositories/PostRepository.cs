using Model;

namespace Bison.Razor.Repositories;

public class PostRepository : IPostRepository {

    private readonly DBFacade _dbFacade;

    public PostRepository(DBFacade dbFacade) {
        _dbFacade = dbFacade;
    }

    public List<ObservationViewModel> GetObservations(int page) {
        return _dbFacade.GetObservations(page);
    }

    public List<ObservationViewModel> GetObservationsFromAuthor(string author, int page) {
        return _dbFacade.GetObservationsFromAuthor(author, page);
    }

    public ObservationDetailsViewModel? GetObservationDetails(int observationId) {
        var observation = _dbFacade.GetObservation(observationId);

        if (observation == null) {
            return null;
        }

        var comments = GetComments(observationId);
        var proposals = GetProposals(observationId);

        return new ObservationDetailsViewModel(
            observationId,
            observation.Author,
            observation.Message,
            observation.Timestamp,
            comments,
            proposals
        );
    }

    public List<Comment> GetComments(int observationId) {
        return _dbFacade.GetComments(observationId);
    }

    public List<Proposal> GetProposals(int observationId) {
        return _dbFacade.GetProposals(observationId);
    }
}