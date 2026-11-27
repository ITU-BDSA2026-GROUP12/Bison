using System.Security.Cryptography.X509Certificates;
using NuGet.Protocol.Plugins;

namespace Bison.Razor.DTOs;

public record ObservationDTO(
    int ObservationId,
    string Author,
    string Message,
    string Timestamp,
    string? PictureUrl = null,
    string? License = null,
    bool IsPinned = false
);

