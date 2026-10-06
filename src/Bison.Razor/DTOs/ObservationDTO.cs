namespace Bison.Razor.DTOs;

public record ObservationDTO(
    int ObservationId,
    string Author,
    string Message,
    string Timestamp
);