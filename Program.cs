using System.Globalization;
using CsvHelper;
using Model;
using SimpleDB;


var database = new CSVDatabase<Cheep>("bison_observe_cli_db.csv");

if (args.Length == 0 || (args.Length > 0 && args[0] == "read")) {
    // we dont need streamreader its in the CSVDatabase class so this acts as that
    var records = database.Read();
    UserInterface.PrintObservations(records);

} 

else if (args.Length > 1 && args[0] == "observe") {
    // this is also refactored to use the CSVDatabase class, so we dont need to open the file here
    string message = args[1];
    string author = Environment.UserName;
    long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

    var record = new Cheep(author, message, timestamp);
    database.Store(record);
    
}
