namespace Bison.Infrastructure.DTOs;

public record CommentDTO(
    string Author,
    string Message,
    string Timestamp
);