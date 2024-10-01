namespace Rgs2NsisConverterLib.Test;

[TestFixture]
[TestOf(typeof(RgsParser))]
public class RgsParserTest
{
    private RgsParser CreateRgsParser(List<RgsToken> tokens)
    {
        return new RgsParser(tokens);
    }

    [Test]
    public void Parse_ShouldReturnRegistryEntry()
    {
        var tokens = new List<RgsToken>
        {
            new() { Type = RgsToken.TokenType.Identifier, Value = "HKCR" },
            new() { Type = RgsToken.TokenType.Symbol, Value = "{" },
            new() { Type = RgsToken.TokenType.Symbol, Value = "}" }
        };
        var rgsParser = CreateRgsParser(tokens);
        var result = rgsParser.Parse();
        Assert.Multiple(() =>
        {
            That(result, Is.Not.Null);
        });
        That(result.RootKey, Is.EqualTo("HKCR"));
    }

    [Test]
    public void Parse_ShouldThrowExceptionInvalidToken()
    {
        var tokens = new List<RgsToken>
        {
            new() { Type = RgsToken.TokenType.String, Value = "Invalid" },
            new() { Type = RgsToken.TokenType.Symbol, Value = "{" },
            new() { Type = RgsToken.TokenType.Symbol, Value = "}" }
        };
        var rgsParser = CreateRgsParser(tokens);
        Throws<KeyExpectedException>(() => rgsParser.Parse());
    }

    [Test]
    public void Parse_ShouldReturnRegistryEntryWithEmptyKeyPath()
    {
        var tokens = new List<RgsToken>
        {
            new() { Type = RgsToken.TokenType.Symbol, Value = "{" },
            new() { Type = RgsToken.TokenType.Symbol, Value = "}" }
        };
        var rgsParser = CreateRgsParser(tokens);
        var result = rgsParser.Parse();
        Multiple(() =>
        {
            That(result, Is.Not.Null);
            That(result.RootKey, Is.EqualTo("HKCR"));
            That(result.KeyPath, Is.EqualTo(""));
        });
    }

    [Test]
    public void Parse_ShouldReturnRegistryEntryWithKeyPath()
    {
        var tokens = new List<RgsToken>
        {
            new() { Type = RgsToken.TokenType.Identifier, Value = "HKLM" },
            new() { Type = RgsToken.TokenType.Symbol, Value = "{" },
            new() { Type = RgsToken.TokenType.Identifier, Value = "Software" },
            new() { Type = RgsToken.TokenType.Symbol, Value = "}" }
        };
        var rgsParser = CreateRgsParser(tokens);
        var result = rgsParser.Parse();
        Multiple(() =>
        {
            That(result, Is.Not.Null);
            That(result.RootKey, Is.EqualTo("HKLM"));
            That(result.KeyPath, Is.Empty);
            That(result.SubKeys, Has.Count.EqualTo(1));
            That(result.SubKeys[0].RootKey, Is.EqualTo("HKLM"));
            That(result.SubKeys[0].KeyPath, Is.EqualTo("Software"));
        });
    }

    [Test]
    public void Parse_ShouldThrowExceptionWhenRootKeyIsInvalid()
    {
        var tokens = new List<RgsToken>
        {
            new() { Type = RgsToken.TokenType.Identifier, Value = "INVALID" },
            new() { Type = RgsToken.TokenType.Symbol, Value = "{" },
            new() { Type = RgsToken.TokenType.Symbol, Value = "}" }
        };
        var rgsParser = CreateRgsParser(tokens);
        Throws<KeyExpectedException>(() => rgsParser.Parse());
    }

    [Test]
    public void Parse_ShouldThrowExceptionWhenUnexpectedEndOfFile()
    {
        var tokens = new List<RgsToken>
        {
            new() { Type = RgsToken.TokenType.Identifier, Value = "HKCR" },
            new() { Type = RgsToken.TokenType.Symbol, Value = "{" }
            // Missing closing brace
        };
        var rgsParser = CreateRgsParser(tokens);
        Throws<Exception>(() => rgsParser.Parse());
    }
    
}
