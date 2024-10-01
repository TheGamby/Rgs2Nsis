namespace Rgs2NsisConverterLib;

public class RegistryValue
{
    public string? Name { get; init; } = string.Empty;
    public string? Type { get; init; } // 's' for string, 'd' for DWORD, etc.
    public string? Data { get; init; }
}