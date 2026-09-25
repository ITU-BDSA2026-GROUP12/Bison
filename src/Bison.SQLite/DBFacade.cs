using Microsoft.Data.Sqlite;

namespace Bison.SQLite;

public class DBFacade
{
    private readonly string _dbPath;

    public DBFacade(string dbPath)
    {
        _dbPath = dbPath;
    }

    private SqliteConnection CreateConnection()
    {
        return new SqliteConnection($"Data Source={_dbPath}");
    }

    public void TestConnection()
    {
        
    }
}