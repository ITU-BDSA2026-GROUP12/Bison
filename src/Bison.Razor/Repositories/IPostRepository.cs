namespace Bison.Razor.Repositories;

using Bison.Razor.Model;

using Bison.Razor.DTOs;

public interface IPostRepository {

    ObservationDTO? GetObservation(int observationId);
    List<ObservationDTO> GetObservations(int page);
    List<ObservationDTO> GetObservationsFromAuthor(string author, int page);
    List<CommentDTO> GetComments(int observationId);
    List<ProposalDTO> GetProposals(int observationId);
    public void CreateObservation(string author, string message, DateTime timestamp);
    public void PostComment(string author, int observationId, string message, DateTime timestamp);
    List<Model.Taxon> GetTaxons();
    List<Model.Observation> GetObservationsForFiltering(string? author = null);
}