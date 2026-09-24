using Model;

public static class UserInterface {
    public static void PrintMessage(string message) {
        Console.WriteLine(message);
    }

    public static void PrintObservations(IEnumerable<Cheep> obs) {
        foreach (var cheep in obs) {
            string dateString = TimestampToLocalDateString(cheep.Timestamp);
            // Observations inherit from Cheep, but Location only exists on Observation.
            if (cheep is Observation observation) {
                Console.WriteLine($"{cheep.Author} @ {dateString}: {cheep.Message} - Location: {observation.Location}");
            }
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

    public static void PrintNoObsservationForLocation(string location) {
        Console.WriteLine($"No observations found for location '{location}'.");
    }

    public static void PrintProposals(IEnumerable<Proposal> proposals) {
        foreach (var proposal in proposals) {
            string dateString = TimestampToLocalDateString(proposal.Timestamp);
            Console.WriteLine($"{proposal.Author} @ {dateString}: {proposal.TaxonId}");
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

    /// <summary>
    /// Converts Unix timestamp to human readable Unix date string
    /// </summary>
    /// <param name="timestamp">Unix timestamp</param>
    /// <returns></returns>
    public static string TimestampToUnixDateString(long timestamp) {
        DateTimeOffset time = DateTimeOffset.FromUnixTimeSeconds(timestamp);
        string dateString = time.ToString("MM'/'dd'/'yy HH':'mm':'ss");
        return dateString;
    }
}