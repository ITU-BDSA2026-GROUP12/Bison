namespace Model
{
    public record Observation(
        int Id,
        string Author,
        string Message,
        long Timestamp)
        :Cheep(Author, Message, Timestamp);
}