using CsvHelper.Configuration;
using System.Globalization;
using System.Collections.Generic;
using CsvHelper;
namespace SimpleDB;
// sealed is just a good practice to prevent inheritance, since this class is not designed to be inherited from.
// refactor into a singleton 
public sealed class CSVDatabase<T> : IDatabaseRepository<T>
{

    private readonly string _filePath;
    private static CSVDatabase<T>? _instance;


    // constructor for the CSVDatabase. It's private because of singleton.
    private CSVDatabase(string filePath)
    {
        _filePath = filePath;
    }

    // this is where we make sure there only comes one instance of the CSVDatabase class, and we return that instance when this method is called.
    public static CSVDatabase<T> getInstance(string filePath)
    {
        // basically here it checks if the object exist or not
        //if it does not exist it creates a new instance of it
        //if if does exist then it just reutns the existing one
        // in this way we can ensure there is only one instance of the obkect we wanna make. 
        if (_instance == null)
        {
            // here we create the instance IF it does not exist. 
            _instance = new CSVDatabase<T>(filePath);
        }

        // if the instance already exist it will just return the already exisitng object. (instance and object is the same btw)
        return _instance;
    }


    public IEnumerable<T> Read(int? limit = null)
    
    {
        // reader and csv open the file, while config turns missing fields into "null" (to accompany for location-less observations)
        using var reader = new StreamReader(_filePath);
        var config = new CsvConfiguration(CultureInfo.InvariantCulture) {
            MissingFieldFound = null
        };
        using var csv = new CsvReader(reader, config);
        
        // we create a list to store the records we read from the CSV file
        var result = new List<T>();
        foreach (var record in csv.GetRecords<T>())
        {
            if (limit.HasValue && result.Count >= limit.Value)
            {
                break;
            }

            result.Add(record);
            

        }

        // here we return the list of records we read from the CSV file
        return result;
       
    }

    // this method is used to store a record in the CSV file
    public void Store(T record)
    {
        string[] headers;

        // Reads the existing CSV header so records can be written in exactly the same column order.
        using (var reader = new StreamReader(_filePath))
        using (var headerCsv = new CsvReader(reader, CultureInfo.InvariantCulture)) {
            headerCsv.Read();
            headerCsv.ReadHeader();

            // Store the column names for use when writing the new record.
            headers = headerCsv.HeaderRecord ?? throw new InvalidOperationException("CSV file has no header.");
        }

        // here we open the file in append mode, so that we can add new records to the end of the file without overwriting existing records
        using var writer = new StreamWriter(_filePath, append: true);


        // this wrap the writer in a CsvWriter, which is a class provided by the CsvHelper library that makes it easy to write records to a CSV file
        using var csv = new CsvWriter(writer, System.Globalization.CultureInfo.InvariantCulture);

        // Gets the actual runtime type of the record so we can access its properties by name.
        var recordType = record!.GetType();

        // Writes each property in the same order as the columns in the CSV header.
        foreach (var header in headers) {
            var property = recordType.GetProperty(header);

            if (property == null) {
                throw new InvalidOperationException($"Property '{header}' was not found on type '{recordType.Name}'.");
            }

            // Gets the value of the matching property and writes it into the current CSV column.
            csv.WriteField(property.GetValue(record));
        }

        // Finishes the current CSV row and moves to the next record.
        csv.NextRecord();
    }
}


