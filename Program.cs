using System.Globalization;
using CsvHelper;
using Model;

if (args.Length == 0 || (args.Length > 0 && args[0] == "read")) {
    using (var reader = new StreamReader("bison_observe_cli_db.csv"))
    using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
    {
        var records = csv.GetRecords<Cheep>();
        UserInterface.PrintObservations(records);
    }
} else if (args.Length > 1 && args[0] == "observe") {
    string message = args[1];
    string author = Environment.UserName;
    long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

    var record = new Cheep(author, message, timestamp);
    using (var writer = new StreamWriter("bison_observe_cli_db.csv", append: true))
    using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
    {
        csv.WriteRecord(record);
        csv.NextRecord();
    }
}
