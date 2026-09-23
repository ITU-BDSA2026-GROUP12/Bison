namespace Model
{
    public record Observation(
        int ObservationId,
        string Author,
        string Message,
        long Timestamp,
        string? Location = null // optional location
        ):Cheep(Author, Message, Timestamp)
        {
            public string Location { get; init; } =
                string.IsNullOrWhiteSpace(Location) ? "unknown" : Location!;    // No location = "unknown"
        }
}