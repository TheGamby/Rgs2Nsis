namespace Rgs2NsisConverterLib;

public class RegistryEntry
{
    public string? RootKey { get; set; } // HKCR, HKLM, HKCU
    public string? KeyPath { get; set; } // Relative path under the root key
    public List<RegistryValue> Values { get; } = [];
    public List<RegistryEntry> SubKeys { get; } = [];
}