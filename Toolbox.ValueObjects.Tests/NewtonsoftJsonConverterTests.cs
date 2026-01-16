namespace Toolbox.ValueObjects.Tests;

#nullable enable
using Newtonsoft.Json;
using NUnit.Framework;

[TestFixture]
public sealed class NewtonsoftJsonConverterTests
{
    private static JsonSerializerSettings Settings<T>()
        where T : JsonConverter, new()
        => new JsonSerializerSettings
        {
            Converters = { new T() }
        };


    // -------------------------
    // STRING
    // -------------------------

    [Test]
    public void Serialize_String_ValueObject()
    {
        var email = Email.Create("user@test.com");

        var json = JsonConvert.SerializeObject(email, Settings<Email.NewtonsoftJsonConverter>());

        Assert.That(json, Is.EqualTo("\"user@test.com\""));
    }

    [Test]
    public void Deserialize_String_ValueObject()
    {
        var json = "\"user@test.com\"";

        var result = JsonConvert.DeserializeObject<Email>(
            json,
            Settings<Email.NewtonsoftJsonConverter>());

        Assert.That(result.Value, Is.EqualTo("user@test.com"));
    }

    [Test]
    public void Deserialize_Invalid_String_Throws()
    {
        Assert.That(
            () => JsonConvert.DeserializeObject<Email>(
                "\"not-an-email\"",
                Settings<Email.NewtonsoftJsonConverter>()),
            Throws.InstanceOf<JsonSerializationException>());
    }

    // -------------------------
    // NUMBER
    // -------------------------

    [Test]
    public void Serialize_Int_ValueObject()
    {
        var value = TestIntValueObject.Create(42);

        var json = JsonConvert.SerializeObject(
            value,
            Settings<TestIntValueObject.NewtonsoftJsonConverter>());

        Assert.That(json, Is.EqualTo("42"));
    }

    [Test]
    public void Deserialize_Int_ValueObject()
    {
        var json = "42";

        var result = JsonConvert.DeserializeObject<TestIntValueObject>(
            json,
            Settings<TestIntValueObject.NewtonsoftJsonConverter>());

        Assert.That(result.Value, Is.EqualTo(42));
    }

    [Test]
    public void Deserialize_Wrong_Token_Throws()
    {
        Assert.That(
            () => JsonConvert.DeserializeObject<TestIntValueObject>(
                "\"42\"",
                Settings<TestIntValueObject.NewtonsoftJsonConverter>()),
            Throws.InstanceOf<JsonSerializationException>());
    }

    // -------------------------
    // NULL HANDLING
    // -------------------------

    [Test]
    public void Deserialize_Null_Throws()
    {
        Assert.That(
            () => JsonConvert.DeserializeObject<TestIntValueObject>(
                "null",
                Settings<TestIntValueObject.NewtonsoftJsonConverter>()),
            Throws.InstanceOf<JsonSerializationException>());
    }
}