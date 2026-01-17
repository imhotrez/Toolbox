namespace Toolbox.ValueObjects.Tests;

using System.Globalization;
using NUnit.Framework;

[TestFixture]
public sealed class ParsableTests
{
    #region byte

    [Test]
    public void Byte_Parse_Valid()
    {
        var value = TestByteValueObject.Parse("42", CultureInfo.InvariantCulture);
        Assert.That(value.Value, Is.EqualTo((byte)42));
    }

    [Test]
    public void Byte_TryParse_Valid()
    {
        var ok = TestByteValueObject.TryParse("42", CultureInfo.InvariantCulture, out var value);
        Assert.That(ok,          Is.True);
        Assert.That(value.Value, Is.EqualTo((byte)42));
    }

    [Test]
    public void Byte_Parse_Invalid_Throws()
    {
        Assert.That(
            () => TestByteValueObject.Parse("abc", CultureInfo.InvariantCulture),
            Throws.Exception);
    }

    #endregion

    #region int

    [Test]
    public void Int_Parse_Valid()
    {
        var value = TestIntValueObject.Parse("123", CultureInfo.InvariantCulture);
        Assert.That(value.Value, Is.EqualTo(123));
    }

    [Test]
    public void Int_TryParse_Invalid_ReturnsFalse()
    {
        var ok = TestIntValueObject.TryParse("not-an-int", CultureInfo.InvariantCulture, out var value);
        Assert.That(ok,    Is.False);
        Assert.That(value, Is.EqualTo(default(TestIntValueObject)));
    }

    #endregion

    #region long

    [Test]
    public void Long_Parse_Valid()
    {
        var value = TestLongValueObject.Parse("9223372036854775807", CultureInfo.InvariantCulture);
        Assert.That(value.Value, Is.EqualTo(long.MaxValue));
    }

    #endregion

    #region uint

    [Test]
    public void UInt_Parse_Valid()
    {
        var value = TestUIntValueObject.Parse("42", CultureInfo.InvariantCulture);
        Assert.That(value.Value, Is.EqualTo(42u));
    }

    [Test]
    public void UInt_Parse_Negative_Throws()
    {
        Assert.That(
            () => TestUIntValueObject.Parse("-1", CultureInfo.InvariantCulture),
            Throws.Exception);
    }

    #endregion

    #region ulong

    [Test]
    public void ULong_Parse_Valid()
    {
        var value = TestULongValueObject.Parse("18446744073709551615", CultureInfo.InvariantCulture);
        Assert.That(value.Value, Is.EqualTo(ulong.MaxValue));
    }

    #endregion

    #region float

    [Test]
    public void Float_Parse_UsesInvariantCulture()
    {
        var value = TestFloatValueObject.Parse("1.5", CultureInfo.GetCultureInfo("fr-FR"));
        Assert.That(value.Value, Is.EqualTo(1.5f));
    }

    #endregion

    #region double

    [Test]
    public void Double_Parse_UsesInvariantCulture()
    {
        var value = TestDoubleValueObject.Parse("1.5", CultureInfo.GetCultureInfo("fr-FR"));
        Assert.That(value.Value, Is.EqualTo(1.5d));
    }

    #endregion

    #region decimal

    [Test]
    public void Decimal_Parse_Valid()
    {
        var value = TestDecimalValueObject.Parse("10.25", CultureInfo.InvariantCulture);
        Assert.That(value.Value, Is.EqualTo(10.25m));
    }

    #endregion

    #region string

    [Test]
    public void String_Parse_Valid()
    {
        var value = TestStringValueObject.Parse("hello", CultureInfo.InvariantCulture);
        Assert.That(value.Value, Is.EqualTo("hello"));
    }

    [Test]
    public void String_TryParse_Null_ReturnsTrue()
    {
        var ok = TestStringValueObject.TryParse((ReadOnlySpan<byte>)null, CultureInfo.InvariantCulture, out var value);
        Assert.Multiple(() =>
        {
            Assert.That(ok, Is.True);
            Assert.That(value, Is.EqualTo(TestStringValueObject.Create(string.Empty)));
        });
    }

    #endregion

    #region null handling

    [Test]
    public void Parse_NullString_ThrowsArgumentNullException()
    {
        Assert.Catch(
            typeof(FormatException),
            () => TestIntValueObject.Parse((ReadOnlySpan<byte>)null, CultureInfo.InvariantCulture),
            "Input is empty");
    }

    #endregion
}