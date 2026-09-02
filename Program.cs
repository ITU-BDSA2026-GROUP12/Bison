if (args.Length > 0 && args[0] == "read") {
    string[] lines = File.ReadAllLines("bison_observe_cli_db.csv");

    foreach (string line in lines.Skip(1)) {
        string[] parts = line.Split(',');

        string author = parts[0];
        string observation = parts[1].Trim('"');
        string timestamp = parts[2];

        DateTimeOffset time = DateTimeOffset.FromUnixTimeSeconds(long.Parse(timestamp));
        string date = time.ToString("MM'/'dd'/'yy HH:mm:ss");

        Console.WriteLine($"{author} @ {date}: {observation}");
    }
} else if (args.Length > 1 && args[0] == "observe") {
    string observation = args[1];
    string author = Environment.UserName;
    long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

    string newLine = $"{author},\"{observation}\",{timestamp}";

    File.AppendAllLines(
        "bison_observe_cli_db.csv",
        [newLine]
    );
}