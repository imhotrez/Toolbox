namespace Toolbox.ValueObjects.Tests;

using System;
using System.Globalization;
using NUnit.Framework;

[TestFixture]
public sealed class SpanParsableTests
{
    // ---------- NUMERIC ----------

    [TestCase("123", 123)]
    [TestCase("0",   0)]
    public void Int_TryParse_Works(string input, int expected)
    {
        var ok = TestIntValueObject.TryParse(
            input.AsSpan(),
            CultureInfo.GetCultureInfo("fr-FR"),
            out var result);

        Assert.That(ok,           Is.True);
        Assert.That(result.Value, Is.EqualTo(expected));
    }

    [Test]
    public void Double_TryParse_UsesInvariantCulture()
    {
        var ok = TestDoubleValueObject.TryParse(
            "1.5".AsSpan(),
            CultureInfo.GetCultureInfo("fr-FR"),
            out var result);

        Assert.That(ok,           Is.True);
        Assert.That(result.Value, Is.EqualTo(1.5));
    }

    [Test]
    public void Double_TryParse_Comma_Fails()
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
            () => TestIntValueObject.Parse("abc".AsSpan(), null),
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

        Assert.That(ok,           Is.True);
        Assert.That(result.Value, Is.EqualTo("hello"));
    }

    // ---------- DOMAIN VALIDATION ----------

    [Test]
    public void Parse_DelegatesToDomainValidation()
    {
        Assert.That(
            () => Email.Parse("not-an-email".AsSpan(), null),
            Throws.TypeOf<FormatException>());
    }
}