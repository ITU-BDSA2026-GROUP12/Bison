namespace Bison.Razor.DTOs;

//contains all information needed to display observation page
public record ObservationDetailsDTO(
    int ObservationId,
    string Author,
    string Message,
    string Timestamp,
    List<CommentDTO> Comments,
    List<ProposalDTO> Proposals
);