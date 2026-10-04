namespace Bison.Razor.Repositories;

using global::Model;

public interface IPostRepository {

    List<ObservationViewModel> GetObservations(int page);
    List<ObservationViewModel> GetObservationsFromAuthor(string author, int page);
    ObservationDetailsViewModel? GetObservationDetails(int observationId);
    List<Comment> GetComments(int observationId);
    List<Proposal> GetProposals(int observationId);
}