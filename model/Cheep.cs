namespace Model
{
    //Get wiser on record here: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/record
    public record Cheep(
        string Author,
        string Message,
        long Timestamp
        );
}