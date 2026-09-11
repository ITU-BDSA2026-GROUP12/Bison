using DocoptNet;
using Model;
using SimpleDB;

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

var database = CSVDatabase<Observation>.getInstance(Config.ObservationDatabase);
var commentDb = CSVDatabase<Comment>.getInstance(Config.CommentDatabase);

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
    // this reuses the CSVDatabase class to store comments.
    string message = arguments["<message>"].ToString();
    string author = Environment.UserName;
    long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    int observationId = int.Parse(arguments["<observationId>"].ToString());
    
    if (database.Read().Any(o => o.ObservationId == observationId)) {
        var record = new Comment(observationId, author, message, timestamp);
        commentDb.Store(record);
    } else {
        Console.WriteLine($"Observation with ID {observationId} does not exist.");
        return;
    }
}
