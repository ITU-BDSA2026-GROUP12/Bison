using Model;

public static class UserInterface {
    public static void PrintObservations(IEnumerable<Cheep> obs) {
        foreach (var cheep in obs) {
            // Convert the stored Unix timestamp into local time.
            DateTimeOffset time = DateTimeOffset.FromUnixTimeSeconds(cheep.Timestamp).LocalDateTime;
            string date = time.ToString("MM'/'dd'/'yy HH:mm:ss");

            Console.WriteLine($"{cheep.Author} @ {date}: {cheep.Message}");
        }
    }

    public static void PrintObservationId(int id) {
        Console.WriteLine($"Observation stored with ID: {id}");
    }

    public static void PrintDiscussion(IEnumerable<Comment> comments) {
        foreach (var comment in comments) {
            DateTimeOffset time = DateTimeOffset.FromUnixTimeSeconds(comment.Timestamp).LocalDateTime;
            string date = time.ToString("MM'/'dd'/'yy HH:mm:ss");

            Console.WriteLine($"{comment.Author} @ {date}: {comment.Message}");
        }
    }
}