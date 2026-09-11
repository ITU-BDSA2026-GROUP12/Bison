using Model;

public static class UserInterface {
    public static void PrintObservations(IEnumerable<Cheep> obs) {
        foreach (var cheep in obs) {
            string dateString = TimestampToLocalDateString(cheep.Timestamp);
            Console.WriteLine($"{cheep.Author} @ {dateString}: {cheep.Message}");
        }
    }

    public static void PrintObservationId(int id) {
        Console.WriteLine($"Observation stored with ID: {id}");
    }

    public static void PrintDiscussion(IEnumerable<Comment> comments) {
        foreach (var comment in comments) {
            string dateString = TimestampToLocalDateString(comment.Timestamp);
            Console.WriteLine($"{comment.Author} @ {dateString}: {comment.Message}");
        }
    }

    /// <summary>
    /// Converts Unix timestamp to human readable local date string
    /// </summary>
    /// <param name="timestamp">Unix timestamp</param>
    /// <returns></returns>
    public static string TimestampToLocalDateString(long timestamp) {
        DateTimeOffset time = DateTimeOffset.FromUnixTimeSeconds(timestamp).LocalDateTime;
        string dateString = time.ToString("MM'/'dd'/'yy HH':'mm':'ss");
        return dateString;
    }
}