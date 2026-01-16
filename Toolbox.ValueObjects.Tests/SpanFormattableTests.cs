namespace Toolbox.ValueObjects.Tests;

using System;
using System.Globalization;
using NUnit.Framework;

[TestFixture]
public class SpanFormattableTests
{
    // ---------- INT ----------
    [Test]
    public void Int_TryFormat_WritesToSpan()
    {
        var        value  = TestIntValueObject.Create(12345);
        Span<char> buffer = stackalloc char[16];

        var result = value.TryFormat(
            buffer,
            out var written,
            "D",
            CultureInfo.InvariantCulture);

        Assert.That(result,                        Is.True);
        Assert.That(written,                       Is.EqualTo(5));
        Assert.That(new string(buffer[..written]), Is.EqualTo("12345"));
    }

    // ---------- DOUBLE ----------
    [Test]
    public void Double_TryFormat_UsesInvariantCulture()
    {
        var        value  = TestDoubleValueObject.Create(1.5);
        Span<char> buffer = stackalloc char[16];

        value.TryFormat(
            buffer,
            out var written,
            "F1",
            CultureInfo.GetCultureInfo("fr-FR"));

        Assert.That(new string(buffer[..written]), Is.EqualTo("1.5"));
    }

    // ---------- DECIMAL ----------
    [Test]
    public void Decimal_ToString_UsesInvariantCulture()
    {
        var value = TestDecimalValueObject.Create(12.34m);

        var s = value.ToString();

        Assert.That(s, Is.EqualTo("12.34"));
    }

    // ---------- STRING ----------
    [Test]
    public void String_TryFormat_CopiesValue()
    {
        var        value  = TestStringValueObject.Create("hello");
        Span<char> buffer = stackalloc char[8];

        var ok = value.TryFormat(
            buffer,
            out var written,
            default,
            null);

        Assert.That(ok,                            Is.True);
        Assert.That(written,                       Is.EqualTo(5));
        Assert.That(new string(buffer[..written]), Is.EqualTo("hello"));
    }

    [Test]
    public void String_TryFormat_BufferTooSmall_ReturnsFalse()
    {
        var        value  = TestStringValueObject.Create("hello");
        Span<char> buffer = stackalloc char[4];

        var ok = value.TryFormat(
            buffer,
            out var written,
            default,
            null);

        Assert.That(ok,      Is.False);
        Assert.That(written, Is.EqualTo(0));
    }

    [Test]
    public void String_ToString_IgnoresFormat()
    {
        var value = TestStringValueObject.Create("abc");

        var s = value.ToString("X", CultureInfo.InvariantCulture);

        Assert.That(s, Is.EqualTo("abc"));
    }
}