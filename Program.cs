using System.Globalization;
using CsvHelper;
using Model;
using SimpleDB;
using DocoptNet;

//Defines rules/valid ways to use the CLI. Very whitespace sensitive here.
const string usage = @"Bison CLI.

Usage:
  bison read
  bison observe <message>

";

//parse command lines using Docopt
var arguments = new Docopt().Apply(usage, args, version:"1.0", exit:true)!;

var database = new CSVDatabase<Cheep>("bison_observe_cli_db.csv");

if (arguments["read"].IsTrue) {
    // we dont need streamreader its in the CSVDatabase class so this acts as that
    var records = database.Read();
    UserInterface.PrintObservations(records);
} 

else if (arguments["observe"].IsTrue) {
    // this is also refactored to use the CSVDatabase class, so we dont need to open the file here
    string message = arguments["<message>"].ToString();
    string author = Environment.UserName;
    long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

    var record = new Cheep(author, message, timestamp);
    database.Store(record);
}
