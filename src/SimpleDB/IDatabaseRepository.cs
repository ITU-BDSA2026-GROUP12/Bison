// this is a interface for a generic database repository that can read and store records of type T
// keep in mind T is just a placeholder for any type, it could be a class, int, string, double or like whatever.
// the reason its smart its because of we can use this interface to create a database repository that can read and store any type.
// it eliminates redundant code and makes it easier to maintain and extend the codebase in the future.

namespace SimpleDB;
    public interface IDatabaseRepository<T>{
    public IEnumerable<T> Read(int? limit = null);
    public void Store(T record);
}