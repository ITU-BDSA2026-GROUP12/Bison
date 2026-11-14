namespace Bison.Razor.Repositories;

using Bison.Razor.Model;

public interface IPostRepository {

    ObservationViewModel? GetObservation(int observationId);
    List<ObservationViewModel> GetObservations(int page);
    List<ObservationViewModel> GetObservationsFromAuthor(string author, int page);
    List<Comment> GetComments(int observationId);
    List<Proposal> GetProposals(int observationId);
    public void CreateObservation(string author, string message, DateTime timestamp);
    public void PostComment(string author, int observationId, string message, DateTime timestamp);
    public void PostProposal(string author, int observationId, int taxonId, string message, DateTime timestamp);
    List<Model.Taxon> GetTaxons();
    List<Model.Observation> GetObservationsForFiltering(string? author = null);
}