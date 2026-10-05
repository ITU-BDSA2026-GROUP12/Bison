namespace Bison.Razor.DTOs;

public record CommentDTO(
    string Author,
    string Message,
    string Timestamp
);