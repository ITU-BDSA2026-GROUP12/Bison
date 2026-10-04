namespace Bison.Razor.Repositories;

using global::Model;

public interface IPostRepository {

    ObservationViewModel? GetObservation(int observationId);
    List<ObservationViewModel> GetObservations(int page);
    List<ObservationViewModel> GetObservationsFromAuthor(string author, int page);
    List<Comment> GetComments(int observationId);
    List<Proposal> GetProposals(int observationId);
}