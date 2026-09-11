
using Model;
using SimpleDB;
using DocoptNet;

//Defines rules/valid ways to use the CLI. Very whitespace sensitive here.
const string usage = @"Bison CLI.

Usage:
    bison read
    bison discussion <observationId>
    bison observe <message>
    bison comment <message> <observationId>
";

//parse command lines using Docopt
var arguments = new Docopt().Apply(usage, args, version:"1.0", exit:true)!;

var database = new CSVDatabase<Observation>("bison_observe_cli_db.csv");
var commentDb = new CSVDatabase<Comment>("bison_comment_cli_db.csv");

//lists all observations in the database
if (arguments["read"].IsTrue) {
    // we dont need streamreader its in the CSVDatabase class so this acts as that
    var records = database.Read();
    UserInterface.PrintObservations(records);
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
    string author = Environment.UserName;
    long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    int id = database.Read().Count() + 1;

    var record = new Observation(id, author, message, timestamp);
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