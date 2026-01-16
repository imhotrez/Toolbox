namespace Toolbox.ValueObjects.Tests;

using System;
using System.Globalization;
using NUnit.Framework;

[TestFixture]
public sealed class SpanParsableTests
{
    // ---------- NUMERIC ----------

    [TestCase("42", 42)]
    [TestCase("0", 0)]
    public void Int_TryParse_Works(string input, int expected)
    {
        var ok = TestIntValueObject.TryParse(
            input.AsSpan(),
            CultureInfo.GetCultureInfo("fr-FR"),
            out var result);

        Assert.That(ok, Is.True);
        Assert.That(result.Value, Is.EqualTo(expected));
    }

    [Test]
    public void Double_TryParse_UsesInvariantCulture()
    {
        var ok = TestDoubleValueObject.TryParse(
            "1.5".AsSpan(),
            CultureInfo.GetCultureInfo("fr-FR"),
            out var result);

        Assert.That(ok, Is.True);
        Assert.That(result.Value, Is.EqualTo(1.5));
    }

    [Test]
    public void Double_TryParse_WithComma_Fails()
    {
        var ok = TestDoubleValueObject.TryParse(
            "1,5".AsSpan(),
            CultureInfo.InvariantCulture,
            out _);

        Assert.That(ok, Is.False);
    }

    [Test]
    public void Int_Parse_Invalid_Throws()
    {
        Assert.That(
            () => TestIntValueObject.Parse("abc".AsSpan(), CultureInfo.InvariantCulture),
            Throws.TypeOf<FormatException>());
    }

    // ---------- STRING ----------

    [Test]
    public void String_TryParse_Works()
    {
        var ok = TestStringValueObject.TryParse(
            "hello".AsSpan(),
            null,
            out var result);

        Assert.That(ok, Is.True);
        Assert.That(result.Value, Is.EqualTo("hello"));
    }

    [Test]
    public void String_Parse_DelegatesToValidation()
    {
        Assert.That(
            () => Email.Parse("not-an-email".AsSpan(), null),
            Throws.TypeOf<FormatException>());
    }

    // ---------- BOUNDARY ----------

    [Test]
    public void EmptySpan_Parse_Fails_ForNumeric()
    {
        var ok = TestIntValueObject.TryParse(
            ReadOnlySpan<char>.Empty,
            null,
            out _);

        Assert.That(ok, Is.False);
    }
}
