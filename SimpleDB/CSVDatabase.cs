namespace SimpleDB;

using System.Collections.Generic;
using CsvHelper;



// sealed is just a good practice to prevent inheritance, since this class is not designed to be inherited from.
public sealed class CSVDatabase<T> : IDatabaseRepository<T>
{
    private readonly string _filePath;

    public CSVDatabase(string filePath)
    {
        _filePath = filePath;
    }

    public IEnumerable<T> Read(int? limit = null)
    {
        // this two line we use to open our file 
        using var reader = new StreamReader(_filePath);
        using var csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture);
        
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
        // here we open the file in append mode, so that we can add new records to the end of the file without overwriting existing records
        using var writer = new StreamWriter(_filePath, append: true);


        // this wrap the writer in a CsvWriter, which is a class provided by the CsvHelper library that makes it easy to write records to a CSV file
        using var csv = new CsvWriter(writer, System.Globalization.CultureInfo.InvariantCulture);

        // here it formats the record as a CSV row and writes it to the file
        csv.WriteRecord(record);
        csv.NextRecord();
    }
}


