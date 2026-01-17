namespace Toolbox.ValueObjects.Tests;

using NUnit.Framework;
using System;
using System.Globalization;
using System.Text;

[TestFixture]
public class Utf8SpanParsableTests
{
    // ===== Parse =====

    [Test]
    public void Parse_ValidUtf8_ReturnsValue()
    {
        ReadOnlySpan<byte> utf8 = "123"u8;
        IFormatProvider provider = new CultureInfo("en-US").NumberFormat; 

        var value = TestUIntValueObject.Parse(utf8, provider);

        Assert.That(value.Value, Is.EqualTo(123u));
    }

    [Test]
    public void Parse_Empty_ThrowsFormatException()
    {
        static void Act()
        {
            ReadOnlySpan<byte> utf8 = ReadOnlySpan<byte>.Empty;
            TestUIntValueObject.Parse(utf8, null);
        }

        Assert.That(Act, Throws.TypeOf<FormatException>()
            .With.Message.EqualTo("Input is empty."));
    }

    [Test]
    public void Parse_InvalidUtf8_ThrowsFormatException()
    {
        static void Act()
        {
            ReadOnlySpan<byte> utf8 = "abc"u8;
            TestUIntValueObject.Parse(utf8, null);
        }

        Assert.That(Act, Throws.TypeOf<FormatException>()
            .With.Message.EqualTo("Invalid UTF8 format."));
    }

    // ===== TryParse =====

    [Test]
    public void TryParse_ValidUtf8_ReturnsTrue()
    {
        ReadOnlySpan<byte> utf8 = "42"u8;

        var ok = TestUIntValueObject.TryParse(utf8, null, out var value);

        Assert.That(ok,          Is.True);
        Assert.That(value.Value, Is.EqualTo(42u));
    }

    [Test]
    public void TryParse_InvalidUtf8_ReturnsFalse()
    {
        ReadOnlySpan<byte> utf8 = "abc"u8;

        var ok = TestUIntValueObject.TryParse(utf8, null, out var value);

        Assert.That(ok,    Is.False);
        Assert.That(value, Is.EqualTo(default(TestUIntValueObject)));
    }

    [Test]
    public void TryParse_InvalidValueObject_ReturnsTrue()
    {
        ReadOnlySpan<byte> utf8 = "0"u8;

        var ok = TestUIntValueObject.TryParse(utf8, null, out var value);

        Assert.That(ok,    Is.True);
        Assert.That(value, Is.EqualTo(TestUIntValueObject.Create(0)));
    }
}
