using Model;

namespace Bison.Razor.Repositories;

public class PostRepository : IPostRepository {

    private readonly DBFacade _dbFacade;

    public PostRepository(DBFacade dbFacade) {
        _dbFacade = dbFacade;
    }

    public ObservationViewModel? GetObservation(int observationId) {
        return _dbFacade.GetObservation(observationId);
    }

    public List<ObservationViewModel> GetObservations(int page) {
        return _dbFacade.GetObservations(page);
    }

    public List<ObservationViewModel> GetObservationsFromAuthor(string author, int page) {
        return _dbFacade.GetObservationsFromAuthor(author, page);
    }

    public List<Comment> GetComments(int observationId) {
        return _dbFacade.GetComments(observationId);
    }

    public List<Proposal> GetProposals(int observationId) {
        return _dbFacade.GetProposals(observationId);
    }
}