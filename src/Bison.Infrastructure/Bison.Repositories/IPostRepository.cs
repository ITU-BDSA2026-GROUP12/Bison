namespace Bison.Infastructure.Repositories;

using Bison.Core.Model;

using Bison.Infastructure.DTOs;
using Bison.Infastructure.Database;

public interface IPostRepository {

    ObservationDTO? GetObservation(int observationId);
    List<ObservationDTO> GetObservations(int page);
    List<ObservationDTO> GetObservationsFromAuthor(string author, int page);
    List<CommentDTO> GetComments(int observationId);
    List<ProposalDTO> GetProposals(int observationId);

    List<Taxon> GetTaxons();
    List<Observation> GetObservationsForFiltering(string? author = null);
    int CountObservations(string? author = null);
    AuthorDTO? GetAuthor(string authorName);
    List<CommentDTO>? GetCommentsByAuthor(string authorName);
    List<ProposalDTO>? GetProposalsByAuthor(string authorName);
    public void CreateObservation(string author, string message, DateTime timestamp, string License);
    public void PostComment(string author, int observationId, string message, DateTime timestamp);
    public void PostProposal(string author, int observationId, int taxonId, string message, DateTime timestamp);
    void TogglePinnedObservation(string author, int observationId);
}