namespace Bison.Infastructure.DTOs;

public record ObservationDTO(
    int ObservationId,
    string Author,
    string Message,
    string Timestamp,
    string? PictureUrl = null,
    string? License = null,
    bool IsPinned = false
);