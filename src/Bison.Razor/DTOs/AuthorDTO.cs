using Bison.Razor.Model;

namespace Bison.Razor.DTOs;

public record AuthorDTO(
    int AuthorId,
    string Name,
    string Email
);