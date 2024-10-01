namespace Rgs2NsisConverterLib;

public class RgsToken
{
    public static readonly RgsToken EndOfFile = new() { Type = TokenType.EndOfFile };
    
    public enum TokenType
    {
        Identifier,
        Symbol,
        String,
        EndOfFile
    }
    public TokenType Type { get; init; }
    public string? Value { get; init; }
}