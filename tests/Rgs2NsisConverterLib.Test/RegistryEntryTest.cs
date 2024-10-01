using System.Diagnostics;

namespace Rgs2NsisConverterLib.Test;

[TestFixture]
[TestOf(typeof(RegistryEntry))]
public class RegistryEntryTests
{
    [Test]
    public void PropertyRootKey_WhenSet_ShouldGetSameValue()
    {
        var registryEntry = new RegistryEntry { RootKey = "HKLM" };
        That(registryEntry.RootKey, Is.EqualTo("HKLM"));
    }

    [Test]
    public void PropertyRootKey_WhenNull_ShouldGetNull()
    {
        var registryEntry = new RegistryEntry { RootKey = null };
        That(registryEntry.RootKey, Is.Null);
    }

    [Test]
    public void PropertyKeyPath_WhenSet_ShouldGetSameValue()
    {
        var registryEntry = new RegistryEntry { KeyPath = @"Software\MyApp" };
        That(registryEntry.KeyPath, Is.EqualTo(@"Software\MyApp"));
    }

    [Test]
    public void PropertyKeyPath_WhenNull_ShouldGetNull()
    {
        var registryEntry = new RegistryEntry { KeyPath = null };
        That(registryEntry.KeyPath, Is.Null);
    }

    [Test]
    public void PropertyValues_WhenAdded_ShouldContainValue()
    {
        var registryValue = new RegistryValue { Name = "Version", Type = "String", Data = "1.0.0.0" };
        var registryEntry = new RegistryEntry();
        registryEntry.Values.Add(registryValue);
        That(registryEntry.Values.Contains(registryValue), Is.True);
    }

    [Test]
    public void PropertyValues_WhenEmpty_ShouldBeEmpty()
    {
        var registryEntry = new RegistryEntry();
        That(registryEntry.Values, Is.Empty);
    }

    [Test]
    public void PropertySubKeys_WhenAdded_ShouldContainKey()
    {
        var subKey = new RegistryEntry { RootKey = "HKLM", KeyPath = @"Software\MyApp" };
        var registryEntry = new RegistryEntry();
        registryEntry.SubKeys.Add(subKey);
        That(registryEntry.SubKeys.Contains(subKey), Is.True);
    }

    [Test]
    public void PropertySubKeys_WhenEmpty_ShouldBeEmpty()
    {
        var registryEntry = new RegistryEntry();
        That(registryEntry.SubKeys, Is.Empty);
    }
}
