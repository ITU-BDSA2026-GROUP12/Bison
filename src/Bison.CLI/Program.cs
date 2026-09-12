using DocoptNet;
using Model;
using SimpleDB;

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

var database = CSVDatabase<Observation>.getInstance(Config.ObservationDatabase);
var commentDb = CSVDatabase<Comment>.getInstance(Config.CommentDatabase);

//lists all observations in the database
if (arguments["read"].IsTrue) {
    // we dont need streamreader its in the CSVDatabase class so this acts as that
    var records = database.Read();
    UserInterface.PrintObservations(records);
}

// Lists observations from a specific location
if (arguments["location"].IsTrue) {
    string location = arguments["<location>"].ToString();

    var records = database.Read()
        .Where(o => string.Equals(
            o.Location,
            location,
            StringComparison.OrdinalIgnoreCase))
        .ToList();

    if (records.Count > 0) {
        UserInterface.PrintObservations(records);
    }
    else {
        Console.WriteLine($"No observations found for location '{location}'.");
    }
}

//lists all comments for a specific observation
if (arguments["discussion"].IsTrue) {
    int observationId = int.Parse(arguments["<observationId>"].ToString());
    
    if (commentDb.Read().Any(o => o.ObservationId == observationId)) {
        var comments = commentDb.Read().Where(c => c.ObservationId == observationId);
        Console.WriteLine($"Comment/s for Observation {observationId}:");
        UserInterface.PrintDiscussion(comments);
    } else {
        Console.WriteLine($"Observation with ID {observationId} does not exist.");
        return;
    }
}

if (arguments["observe"].IsTrue) {
    // this is also refactored to use the CSVDatabase class, so we dont need to open the file here
    string message = arguments["<message>"].ToString();
    string location = args.Length >= 3 ? args[2]: "unknown";

    string author = Environment.UserName;
    long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    int id = database.Read().Count() + 1;

    var record = new Observation(id, author, message, timestamp, location);
    database.Store(record);
    UserInterface.PrintObservationId(id);
}

else if (arguments["comment"].IsTrue) {
    string message = arguments["<message>"].ToString();
    int observationId = int.Parse(arguments["<observationId>"].ToString());
    Comment(message, observationId, database, commentDb);
}

public partial class Program {
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    /// <param name="observationId"></param>
    /// <param name="database">We can probably delete this parameter when it becomes a singleton</param>
    /// <param name="commentDb">We can probably delete this parameter when it becomes a singleton</param>
    /// <returns>True if the comment is stored, False otherwise</returns>
    public static bool Comment(string message, int observationId, CSVDatabase<Observation>? database, CSVDatabase<Comment>? commentDb) {
        // this reuses the CSVDatabase class to store comments.
        string author = Environment.UserName;
        long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        if (database != null && database.Read().Any(o => o.ObservationId == observationId)) {
            var record = new Comment(observationId, author, message, timestamp);
            //the "?" after commentDb means it only calls commentDb.Store() if commentDb is not null
            commentDb?.Store(record);
            return true;
        } else {
            Console.WriteLine($"Observation with ID {observationId} does not exist.");
            return false;
        }
    }
}