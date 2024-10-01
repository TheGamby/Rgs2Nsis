namespace Rgs2NsisConverterLib.Test;

[TestFixture]
public class RgsToNsisConverterTest
{
    //Arrange
    private readonly string _guidStr = "{00000000-0000-0000-0000-000000000000}";
    private readonly string _nonGuidStr = "{InvalidGUID}";

    [Test]
    public void ConvertRgsToNsis_ValidString_Converts()
    {
        //Arrange
        var invalidRgsContent = "Invalid RGS Content";
        //Act
        Throws<KeyExpectedException>(() => RgsToNsisConverter.ConvertRgsToNsis(invalidRgsContent));
    }

    [Test]
    public void ConvertRgsToNsis_Null_Throws()
    {
        //Arrange
        string? nullRgsContent = null;
        //Act and Assert
        Throws<ArgumentNullException>(() => RgsToNsisConverter.ConvertRgsToNsis(nullRgsContent));
    }

    [Test]
    public void ConvertRgsToNsis_Empty_Throws()
    {
        //Arrange
        var emptyRgsContent = string.Empty;
        //Act and Assert
        Throws<KeyExpectedException>(() => RgsToNsisConverter.ConvertRgsToNsis(emptyRgsContent));
    }
    
    [Test]
    public void ConvertRgsToNsis_File_Ok()
    {
        //Arrange
        var input = File.ReadAllText("../../../../../test-data/input.rgs");
        var expectedResult = File.ReadAllText("../../../../../test-data/output.nsi");
        //Act
        var result = RgsToNsisConverter.ConvertRgsToNsis(input);
        That(expectedResult, Is.EqualTo(result));
    }
    
    [Test]
    public void IsGuid_ValidGuid_ReturnsTrue()
    {
        var result = RgsToNsisConverter.IsGuid(_guidStr);
        That(result, Is.True, "IsGuid returned false for valid GUID.");
    }

    [Test]
    public void IsGuid_InvalidGuid_ReturnsFalse()
    {
        var result = RgsToNsisConverter.IsGuid(_nonGuidStr);
        That(result, Is.False, "IsGuid returned true for invalid GUID.");
    }

    [Test]
    public void Tokenize_ValidInput_TokensGenerated()
    {
        //Arrange
        var validInput = "test";
        //Act
        var result = RgsToNsisConverter.Tokenize(validInput);
        That(result, Has.Count.EqualTo(2));
    }

    [Test]
    public void Tokenize_NullInput_ReturnsNoTokens()
    {
        //Arrange
        string? nullInput = null;
        //Act
        Throws<ArgumentNullException>(() => RgsToNsisConverter.Tokenize(nullInput));
    }

    [Test]
    public void Tokenize_EmptyInput_ReturnsNoTokens()
    {
        //Arrange
        var emptyInput = string.Empty;
        //Act
        var result = RgsToNsisConverter.Tokenize(emptyInput);
        That(result, Has.Count.EqualTo(1));
    }
}