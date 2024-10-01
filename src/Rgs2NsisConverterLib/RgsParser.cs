namespace Rgs2NsisConverterLib;

internal class RgsParser(List<RgsToken> tokens)
{
    private int _tokenIterator;

    public RegistryEntry Parse() => ParseRegistryEntry();
    private RgsToken CurrentToken() => _tokenIterator < tokens.Count ? tokens[_tokenIterator] : RgsToken.EndOfFile;

    private void MoveToNextToken()
    {
        if (_tokenIterator < tokens.Count)
        {
            _tokenIterator++;
        }
    }

    private bool Match(RgsToken.TokenType type, string? value = null)
    {
        var token = CurrentToken();
        if (token.Type != type || (value != null && token.Value != value)) return false;
        MoveToNextToken();
        return true;
    }

    private void Expect(RgsToken.TokenType type, string? value = null)
    {
        var token = CurrentToken();
        if (Match(type, value)) return;
        throw new Exception($"Expected token {type} '{value}' but found '{token.Value}'");
    }

    private RegistryEntry ParseRegistryEntry()
    {
        var entry = new RegistryEntry();

        // Parse the root key
        var token = CurrentToken();
        switch (token.Type)
        {
            case RgsToken.TokenType.Identifier when token.Value is "HKCR" or "HKLM" or "HKCU":
                entry.RootKey = token.Value;
                entry.KeyPath = ""; // Root path
                MoveToNextToken();
                Expect(RgsToken.TokenType.Symbol, "{");
                break;
            case RgsToken.TokenType.Symbol when token.Value == "{":
                // Root key is not specified, default to HKCR
                entry.RootKey = "HKCR";
                entry.KeyPath = ""; // Root path
                MoveToNextToken();
                break;
            default:
                throw new KeyExpectedException("Expected root key (HKCR, HKLM, HKCU) or '{'");
        }
        ParseRegistryEntryBody(entry, "");
        Expect(RgsToken.TokenType.Symbol, "}");
        return entry;
    }

    private void ParseRegistryEntryBody(RegistryEntry parentEntry, string? currentPath)
    {
        while (CurrentToken().Type != RgsToken.TokenType.Symbol || CurrentToken().Value != "}")
        {
            var token = CurrentToken();
            if (token.Type is RgsToken.TokenType.Identifier or RgsToken.TokenType.String)
            {
                string? keyName;
                if (token is { Type: RgsToken.TokenType.Identifier, Value: "NoRemove" or "ForceRemove" })
                {
                    var keyModifier = token.Value;
                    MoveToNextToken();
                    token = CurrentToken();
                    if (token.Type is RgsToken.TokenType.Identifier or RgsToken.TokenType.String)
                    {
                        keyName = token.Value?.Trim('\'');
                        MoveToNextToken();
                    }
                    else
                    {
                        throw new Exception($"Expected key name after {keyModifier}");
                    }
                }
                else
                {
                    keyName = token.Value?.Trim('\'');
                    MoveToNextToken();
                }

                var subEntry = new RegistryEntry
                {
                    RootKey = parentEntry.RootKey,
                    KeyPath = CombinePaths(currentPath, keyName)
                };
                parentEntry.SubKeys.Add(subEntry);

                if (Match(RgsToken.TokenType.Symbol, "="))
                {
                    // It's a value, not a key
                    var valueTypeToken = CurrentToken();
                    if (valueTypeToken.Type == RgsToken.TokenType.Identifier)
                    {
                        var valueType = valueTypeToken.Value;
                        MoveToNextToken();
                        var dataToken = CurrentToken();
                        if (dataToken.Type == RgsToken.TokenType.String)
                        {
                            var data = dataToken.Value;
                            MoveToNextToken();

                            var regValue = new RegistryValue
                            {
                                Name = "", // For default value of the key
                                Type = valueType,
                                Data = data
                            };
                            subEntry.Values.Add(regValue);
                        }
                        else
                        {
                            throw new Exception("Expected string value data");
                        }

                        // After parsing value, check if there is a '{' for sub keys
                        if (CurrentToken().Type != RgsToken.TokenType.Symbol || CurrentToken().Value != "{") continue;
                        MoveToNextToken(); // Consume '{'
                        ParseRegistryEntryBody(subEntry, subEntry.KeyPath);
                        Expect(RgsToken.TokenType.Symbol, "}");
                    }
                    else
                    {
                        throw new Exception("Expected value type (s, d, etc.)");
                    }
                }
                else if (Match(RgsToken.TokenType.Symbol, "{"))
                {
                    ParseRegistryEntryBody(subEntry, subEntry.KeyPath);
                    Expect(RgsToken.TokenType.Symbol, "}");
                }
            }
            else if (token is { Type: RgsToken.TokenType.Symbol, Value: "}" })
            {
                break;
            }
            else if (token is { Type: RgsToken.TokenType.Identifier, Value: "val" })
            {
                MoveToNextToken(); // Skip 'val'

                var valueNameToken = CurrentToken();
                if (valueNameToken.Type is RgsToken.TokenType.Identifier or RgsToken.TokenType.String)
                {
                    var valueName = valueNameToken.Value?.Trim('\'');
                    MoveToNextToken();

                    Expect(RgsToken.TokenType.Symbol, "=");

                    var typeToken = CurrentToken();
                    if (typeToken.Type == RgsToken.TokenType.Identifier)
                    {
                        var valueType = typeToken.Value;
                        MoveToNextToken();

                        var dataToken = CurrentToken();
                        if (dataToken.Type == RgsToken.TokenType.String)
                        {
                            var data = dataToken.Value;
                            MoveToNextToken();

                            var regValue = new RegistryValue
                            {
                                Name = valueName,
                                Type = valueType,
                                Data = data
                            };
                            parentEntry.Values.Add(regValue);
                        }
                        else
                        {
                            throw new Exception("Expected string value data");
                        }
                    }
                    else
                    {
                        throw new Exception("Expected value type (s, d, etc.)");
                    }
                }
                else
                {
                    throw new Exception("Expected value name after 'val'");
                }
            }
            else
            {
                throw new Exception($"Unexpected token '{token.Value}'");
            }
        }
    }

    private static string? CombinePaths(string? baseKey, string? subKey) => string.IsNullOrEmpty(baseKey) ? subKey : baseKey + "\\" + subKey;
}