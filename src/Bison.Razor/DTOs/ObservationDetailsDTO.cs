namespace Bison.Razor.DTOs;

public record ObservationDetailsDTO(
    int ObservationId,
    string Author,
    string Message,
    string Timestamp,
    List<CommentDTO> Comments,
    List<ProposalDTO> Proposals
);