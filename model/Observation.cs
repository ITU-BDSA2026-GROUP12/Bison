namespace Model
{
    public record Observation(
        int ObservationId,
        string Author,
        string Message,
        long Timestamp
        ):Cheep(Author, Message, Timestamp);
}