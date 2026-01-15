namespace Toolbox.ValueObjects.Tests;

#nullable enable

using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using CodeGeneration.Attributes;
using NUnit.Framework;




[TestFixture]
public sealed class SystemTextJsonValueObjectConverterTests
{
    private static JsonSerializerOptions CreateOptions<T>()
    {
        var options = new JsonSerializerOptions
        {
            
            
        };

        options.Converters.Add((JsonConverter)Activator.CreateInstance(
            typeof(T).GetNestedType("SystemTextJsonConverter", System.Reflection.BindingFlags.NonPublic)!
        )!);

        return options;
    }

    #region Positive serialization tests

    [TestCase((byte)42)]
    [TestCase((byte)0)]
    [TestCase(byte.MaxValue)]
    public void Serialize_ByteValueObject_Success(byte value)
    {
        var vo = TestByteValueObject.Create(value);
        var json = JsonSerializer.Serialize(vo, CreateOptions<TestByteValueObject>());
        Assert.That(json, Is.EqualTo(value.ToString(CultureInfo.InvariantCulture)));

        /*System.Text.Json.Utf8JsonReader reader = new Utf8JsonReader();
        byte[] b = reader.ValueSequence.IsSingleSegment ?  reader.ValueSequence.First.ToArray() : reader.ValueSequence. GetBytesFromBase64() */
    }

    [TestCase(42)]
    [TestCase(0)]
    [TestCase(int.MinValue)]
    [TestCase(int.MaxValue)]
    public void Serialize_IntValueObject_Success(int value)
    {
        var vo = TestIntValueObject.Create(value);
        var json = JsonSerializer.Serialize(vo, CreateOptions<TestIntValueObject>());
        Assert.That(json, Is.EqualTo(value.ToString(CultureInfo.InvariantCulture)));
    }

    [TestCase("abc")]
    [TestCase("")]
    [TestCase("123")]
    public void Serialize_StringValueObject_Success(string value)
    {
        var vo = TestStringValueObject.Create(value);
        var json = JsonSerializer.Serialize(vo, CreateOptions<TestStringValueObject>());
        Assert.That(json, Is.EqualTo($"\"{value}\""));
    }

    #endregion

    #region Positive deserialization tests

    [TestCase("42", 42)]
    [TestCase("0", 0)]
    [TestCase("-1", -1)]
    public void Deserialize_IntValueObject_Success(string json, int expected)
    {
        var vo = JsonSerializer.Deserialize<TestIntValueObject>(
            json, CreateOptions<TestIntValueObject>());

        Assert.That(vo.Value, Is.EqualTo(expected));

        /*Stream stream = new MemoryStream();
        System.Text.Json.Utf8JsonWriter writer = new Utf8JsonWriter(stream);
        writer.WriteString();*/
    }

    /*[TestCase("\"abc\"", "abc")]
    [TestCase("\"\"", "")]
    public void Deserialize_StringValueObject_Success(string json, string expected)
    {
        var vo = JsonSerializer.Deserialize<TestStringValueObject>(
            json, CreateOptions<TestStringValueObject>());

        Assert.That(vo.Value, Is.EqualTo(expected));
    }*/

    #endregion

    #region Negative: token mismatch

    [Test]
    public void Deserialize_Int_FromString_Throws()
    {
        var ex = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<TestIntValueObject>(
                "\"123\"", CreateOptions<TestIntValueObject>())
        );

        Assert.That(ex!.Message, Does.Contain("Invalid JSON token"));
    }

    [Test]
    public void Deserialize_String_FromNumber_Throws()
    {
        var ex = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<TestStringValueObject>(
                "123", CreateOptions<TestStringValueObject>())
        );

        Assert.That(ex!.Message, Does.Contain("Invalid JSON token"));
    }

    #endregion

    #region Negative: invalid numeric values

    [Test]
    public void Deserialize_Byte_OutOfRange_Throws()
    {
        var ex = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<TestByteValueObject>(
                "256", CreateOptions<TestByteValueObject>())
        );

        Assert.That(ex!.Message, Does.Contain("The JSON value could not be converted to Toolbox.ValueObjects.Tests.TestByteValueObject"));
    }

    [Test]
    public void Deserialize_UInt_Negative_Throws()
    {
        var ex = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<TestUIntValueObject>(
                "-1", CreateOptions<TestUIntValueObject>())
        );

        Assert.That(ex!.Message, Does.Contain("The JSON value could not be converted to Toolbox.ValueObjects.Tests.TestUIntValueObject"));
    }

    #endregion

    #region Null handling

    [Test]
    public void Deserialize_Null_Throws()
    {
        var ex = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<TestIntValueObject>(
                "null", CreateOptions<TestIntValueObject>())
        );

        Assert.That(ex!.Message, Does.Contain("Null is not allowed"));
    }

    #endregion

    #region Round-trip tests

    [TestCase(123)]
    [TestCase(-456)]
    public void RoundTrip_IntValueObject(int value)
    {
        var original = TestIntValueObject.Create(value);

        var json = JsonSerializer.Serialize(original, CreateOptions<TestIntValueObject>());
        var restored = JsonSerializer.Deserialize<TestIntValueObject>(
            json, CreateOptions<TestIntValueObject>());

        Assert.That(restored.Value, Is.EqualTo(original.Value));
    }

    [TestCase("hello")]
    [TestCase("42")]
    public void RoundTrip_StringValueObject(string value)
    {
        var original = TestStringValueObject.Create(value);

        var json = JsonSerializer.Serialize(original, CreateOptions<TestStringValueObject>());
        var restored = JsonSerializer.Deserialize<TestStringValueObject>(
            json, CreateOptions<TestStringValueObject>());

        Assert.That(restored.Value, Is.EqualTo(original.Value));
    }

    #endregion
}
