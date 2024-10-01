namespace Rgs2NsisConverterLib.Test;

[TestFixture]
[TestOf(typeof(RegistryValue))]
public class RegistryValueTest
{
    // Test for Name Property
    [Test]
    public void Name_SetGetValue_ReturnsSetValue()
    {
        // Create an instance of RegistryValue
        var registryValue = new RegistryValue()
        {
            Name = "TestName",
        };

        // Assertion
        That(registryValue.Name, Is.EqualTo("TestName"));
    }

    [Test]
    public void Name_SetNullValue_ReturnsNull()
    {
        // Create an instance of RegistryValue
        var registryValue = new RegistryValue()
        {
            Name = null,
        };
        That(registryValue.Name, Is.Null);
    }

    // Test for Type Property
    [Test]
    public void Type_SetGetValue_ReturnsSetValue()
    {
        // Create an instance of RegistryValue
        var registryValue = new RegistryValue()
        {
            Type = "d",
        };
        That(registryValue.Type, Is.EqualTo("d"));
    }

    [Test]
    public void Type_SetNullValue_ReturnsNull()
    {
        // Create an instance of RegistryValue
        var registryValue = new RegistryValue()
        {
            Type = null,
        };
        That(registryValue.Type, Is.Null);
    }

    // Test for Data Property
    [Test]
    public void Data_SetGetValue_ReturnsSetValue()
    {
        // Create an instance of RegistryValue
        var registryValue = new RegistryValue()
        {
            Data = "TestData",
        };
        That(registryValue.Data, Is.EqualTo("TestData"));
    }

    [Test]
    public void Data_SetNullValue_ReturnsNull()
    {
        // Create an instance of RegistryValue
        var registryValue = new RegistryValue()
        {
            Data = null,
        };
        That(registryValue.Data, Is.Null);
    }
}
