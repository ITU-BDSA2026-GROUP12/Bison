namespace Bison.Infastructure.DTOs;

public record CommentDTO(
    string Author,
    string Message,
    string Timestamp
);