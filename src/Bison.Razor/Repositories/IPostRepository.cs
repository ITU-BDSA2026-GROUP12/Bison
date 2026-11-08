namespace Bison.Razor.Repositories;

using Bison.Razor.Model;

public interface IPostRepository {

    ObservationViewModel? GetObservation(int observationId);
    List<ObservationViewModel> GetObservations(int page);
    List<ObservationViewModel> GetObservationsFromAuthor(string author, int page);
    List<Comment> GetComments(int observationId);
    List<Proposal> GetProposals(int observationId);
    public int CreateObservation(string author, string message, DateTime timestamp);
}