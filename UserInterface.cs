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
}