namespace Rgs2NsisConverterLib.Test;

[TestFixture]
[TestOf(typeof(RgsToken))]
public class RgsTokenTest
{
    [Test]
    public void TokenTypeIdentifier_ShouldHaveExpectedValue()
    {
        var token = new RgsToken { Type = RgsToken.TokenType.Identifier, Value = "SampleIdentifier" };
        Multiple(() =>
        {
            That(token.Type, Is.EqualTo(RgsToken.TokenType.Identifier));
        });
        That(token.Value, Is.EqualTo("SampleIdentifier"));
    }

    [Test]
    public void TokenTypeSymbol_ShouldHaveExpectedValue()
    {
        var token = new RgsToken { Type = RgsToken.TokenType.Symbol, Value = "SampleSymbol" };
        Multiple(() =>
        {
            That(token.Type, Is.EqualTo(RgsToken.TokenType.Symbol));
        });
        That(token.Value, Is.EqualTo("SampleSymbol"));
    }

    [Test]
    public void TokenTypeString_ShouldHaveExpectedValue()
    {
        var token = new RgsToken { Type = RgsToken.TokenType.String, Value = "SampleString" };
        Multiple(() =>
        {
            That(token.Type, Is.EqualTo(RgsToken.TokenType.String));
        });
        That(token.Value, Is.EqualTo("SampleString"));
    }

    [Test]
    public void TokenTypeEndOfFile_ShouldHaveExpectedValue()
    {
        That(RgsToken.EndOfFile.Type, Is.EqualTo(RgsToken.TokenType.EndOfFile));
    }

    [Test]
    public void TokenValueMayBeNull()
    {
        var token = new RgsToken { Type = RgsToken.TokenType.Symbol, Value = null };
        That(token.Value, Is.Null);
    }
}
