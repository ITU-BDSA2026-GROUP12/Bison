using CsvHelper.Configuration.Attributes;
using DocoptNet;
using Model;
using System.Net.Http.Json;

// Defines rules/valid ways to use the CLI. 
// For now, location is optional for "observe" due to old data lacking a location.
// Very whitespace sensitive here.
const string usage = @"Bison CLI.

Usage:
    bison read
    bison discussion <observationId>
    bison location <location>
    bison observe <message> [<location>]
    bison comment <message> <observationId>
";

//parse command lines using Docopt
var arguments = new Docopt().Apply(usage, args, version:"1.0", exit:true)!;

//lists all observations in the database
if (arguments["read"].IsTrue) {
    
    var records = await Program.Client.GetFromJsonAsync<IEnumerable<Observation>>(
    "/observations"
    );

    if (records != null) {
        UserInterface.PrintObservations(records);
    }
}

// Lists observations from a specific location
if (arguments["location"].IsTrue) {
    string location = arguments["<location>"].ToString();

    var allRecords = await Program.Client.GetFromJsonAsync<IEnumerable<Observation>>(
        "/observations"
    );

    // allrecords can be null so just return if its null
    if (allRecords == null)
    {
        return;
    }
    var records = Program.GetObservationsForLocation(allRecords, location).ToList();

    if (records.Count > 0) {
        UserInterface.PrintObservations(records);
    }
    else {
        UserInterface.PrintNoObsservationForLocation(location);
    }
}

// lists all comments for a specific observation
if (arguments["discussion"].IsTrue) {
    int observationId = int.Parse(arguments["<observationId>"].ToString());

    var comments = await Program.Client.GetFromJsonAsync<IEnumerable<Comment>>(
        $"/comments?ObservationId={observationId}"
    );
    
    if (comments != null && comments.Any()) {
        Console.WriteLine($"Comment/s for Observation {observationId}:");
        UserInterface.PrintDiscussion(comments);
    } else {
        Console.WriteLine($"Observation with ID {observationId} does not exist.");
        return;
    }
}

// Stores a new observation in the database
if (arguments["observe"].IsTrue) {
    string message = arguments["<message>"].ToString();
    string location = arguments["<location>"]?.ToString() ?? "unknown";

    string author = Environment.UserName;
    long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

    var record = new ObservationRequest(author, message, timestamp, location);
    var response = await Program.Client.PostAsJsonAsync("/observation", record);

    // Ensure the request was successful if its not it will throw a exception
    response.EnsureSuccessStatusCode();
}

// Stores a new comment for a specific observation
else if (arguments["comment"].IsTrue) {
    string message = arguments["<message>"].ToString();
    int observationId = int.Parse(arguments["<observationId>"].ToString());
    await CommentAsync(message, observationId);
}

public partial class Program {
    public static HttpClient Client { get; } = new HttpClient
    {
        BaseAddress = new Uri("http://localhost:5256")
    };

    /// <param name="message"></param>
    /// <param name="observationId"></param>
    /// <returns>True if the comment is stored, False otherwise</returns>
    public static async Task<bool> CommentAsync(string message, int observationId) {
        // this reuses the CSVDatabase class to store comments.
        string author = Environment.UserName;
        long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        var request = new CommentRequest(observationId, author, message, timestamp);
        var response = await Client.PostAsJsonAsync("/comment", request);
        response.EnsureSuccessStatusCode();

        return true;

    }

    // A getter method to make it easier to get observations for a certain location (makes unit tests easier)
    public static IEnumerable<Observation> GetObservationsForLocation(IEnumerable<Observation> observations, string location) {
        return observations.Where(o => string.Equals(o.Location, location, StringComparison.OrdinalIgnoreCase));
    }

}
