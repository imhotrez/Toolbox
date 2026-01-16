namespace Toolbox.ValueObjects.Tests;

using System;
using System.Globalization;
using System.Text;
using NUnit.Framework;

[TestFixture]
public sealed class Utf8SpanFormattableTests
{
    // ---------- NUMERIC ----------

    [TestCase(1.5,     "1.5")]
    [TestCase(1234.25, "1234.25")]
    public void Double_TryFormat_Utf8_UsesInvariantCulture(double input, string expected)
    {
        var        value  = TestDoubleValueObject.Create(input);
        Span<byte> buffer = stackalloc byte[32];

        var result = value.TryFormat(
            buffer,
            out var written,
            "G",
            CultureInfo.GetCultureInfo("fr-FR"));

        Assert.That(result, Is.True);
        Assert.That(
            Encoding.UTF8.GetString(buffer[..written]),
            Is.EqualTo(expected));
    }

    [Test]
    public void Int_TryFormat_Utf8_Works()
    {
        var        value  = TestIntValueObject.Create(42);
        Span<byte> buffer = stackalloc byte[16];

        value.TryFormat(buffer, out var written, default, CultureInfo.InvariantCulture);

        Assert.That(
            Encoding.UTF8.GetString(buffer[..written]),
            Is.EqualTo("42"));
    }

    // ---------- STRING ----------

    [Test]
    public void String_TryFormat_Utf8_Works()
    {
        var        value  = TestStringValueObject.Create("hello");
        Span<byte> buffer = stackalloc byte[16];

        var result = value.TryFormat(buffer, out var written, default, null);

        Assert.That(result, Is.True);
        Assert.That(
            Encoding.UTF8.GetString(buffer[..written]),
            Is.EqualTo("hello"));
    }

    [Test]
    public void String_TryFormat_WithFormat_Throws()
    {
        var value = TestStringValueObject.Create("hello");

        Assert.That(
            () =>
            {
                Span<byte> buffer = stackalloc byte[16];
                value.TryFormat(buffer, out _, "X", CultureInfo.InvariantCulture);
            },
            Throws.TypeOf<FormatException>());
    }


    // ---------- BOUNDARY ----------

    [Test]
    public void Utf8_TryFormat_BufferTooSmall_ReturnsFalse()
    {
        var        value  = TestStringValueObject.Create("hello");
        Span<byte> buffer = stackalloc byte[2];

        var result = value.TryFormat(buffer, out var written, default, null);

        Assert.That(result,  Is.False);
        Assert.That(written, Is.EqualTo(0));
    }
}