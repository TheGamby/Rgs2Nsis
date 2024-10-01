using System.Text;

namespace Rgs2NsisConverterLib;

public static class RgsToNsisConverter
{
    public static string ConvertRgsToNsis(string? rgsContent) => GenerateNsisScript(new RgsParser(Tokenize(rgsContent)).Parse());

    internal static List<RgsToken> Tokenize(string? input)
    {
        ArgumentNullException.ThrowIfNull(input);

        var tokens = new List<RgsToken>();
        var index = 0;
        var length = input.Length;

        while (index < length)
        {
            var c = input[index];
            if (char.IsWhiteSpace(c))
            {
                index++;
                continue;
            }
            
            switch (c)
            {
                case '{':
                {
                    // Try to read a GUID enclosed in braces
                    var start = index;
                    var tempIndex = index;
                    tempIndex++; // Skip '{'

                    // GUID pattern: {xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}
                    if (tempIndex + 36 <= length)
                    {
                        var possibleGuid = input.Substring(start, 38);
                        if (IsGuid(possibleGuid))
                        {
                            // It's a GUID
                            index += 38; // Skip over the GUID
                            tokens.Add(new RgsToken { Type = RgsToken.TokenType.Identifier, Value = possibleGuid });
                        }
                        else
                        {
                            // Not a GUID, treat '{' as symbol
                            tokens.Add(new RgsToken { Type = RgsToken.TokenType.Symbol, Value = "{" });
                            index++;
                        }
                    }
                    else
                    {
                        // Not enough characters for a GUID, treat '{' as symbol
                        tokens.Add(new RgsToken { Type = RgsToken.TokenType.Symbol, Value = "{" });
                        index++;
                    }

                    break;
                }
                case '}':
                    tokens.Add(new RgsToken { Type = RgsToken.TokenType.Symbol, Value = "}" });
                    index++;
                    break;
                case '=':
                    tokens.Add(new RgsToken { Type = RgsToken.TokenType.Symbol, Value = "=" });
                    index++;
                    break;
                case '\'':
                {
                    // String literal
                    index++;
                    var start = index;
                    while (index < length && input[index] != '\'')
                        index++;
                    var value = input.Substring(start, index - start);
                    tokens.Add(new RgsToken { Type = RgsToken.TokenType.String, Value = value });
                    index++; // Skip closing quote
                    break;
                }
                default:
                {
                    if (char.IsLetter(c) || c == '_' || c == '%' || c == '-' || c == '.')
                    {
                        // Identifier
                        var start = index;
                        while (index < length && (char.IsLetterOrDigit(input[index]) || input[index] == '_' || input[index] == '%' || input[index] == '-' || input[index] == '.' || input[index] == '{' || input[index] == '}'))
                            index++;
                        var value = input.Substring(start, index - start);
                        tokens.Add(new RgsToken { Type = RgsToken.TokenType.Identifier, Value = value });
                    }
                    else
                    {
                        // Skip any other character
                        index++;
                    }

                    break;
                }
            }
        }
        tokens.Add(new RgsToken { Type = RgsToken.TokenType.EndOfFile });
        return tokens;
    }

    /// <summary>
    /// Check if the string is a GUID enclosed in braces
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    internal static bool IsGuid(string s)
    {
        if (s.Length != 38) return false;
        if (s[0] != '{' || s[37] != '}') return false;
        return Guid.TryParse(s.Substring(1, 36), out _);
    }

    private static string GenerateNsisScript(RegistryEntry rootEntry)
    {
        var sb = new StringBuilder();
        GenerateNsisScriptForEntry(rootEntry, sb);
        return sb.ToString();
    }

    private static void GenerateNsisScriptForEntry(RegistryEntry entry, StringBuilder sb)
    {
        foreach (var value in entry.Values)
        {
            var key = entry.KeyPath ?? "";

            switch (value.Type)
            {
                case "s":
                    sb.AppendLine($"WriteRegStr {entry.RootKey} \"{key}\" \"{value.Name}\" \"{value.Data}\"");
                    break;
                case "d":
                    sb.AppendLine($"WriteRegDWORD {entry.RootKey} \"{key}\" \"{value.Name}\" {value.Data}");
                    break;
                default:
                    sb.AppendLine($"# Unsupported value type '{value.Type}' for value '{value.Name}'");
                    break;
            }
        }

        foreach (var subEntry in entry.SubKeys)
        {
            GenerateNsisScriptForEntry(subEntry, sb);
        }
    }
}