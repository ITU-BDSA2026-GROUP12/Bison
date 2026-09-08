
using Model;
using SimpleDB;
using DocoptNet;

//Defines rules/valid ways to use the CLI. Very whitespace sensitive here.
const string usage = @"Bison CLI.

Usage:
  bison read
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
    }
}
