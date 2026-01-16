namespace Toolbox.ValueObjects.Tests;

[TestFixture]
public sealed class ValueObjectFactoryTests
{
    // -------------------------
    // BYTE
    // -------------------------
    [Test]
    public void Byte_Create_And_TryCreate_Work()
    {
        var value = (byte)123;

        var created = TestByteValueObject.Create(value);
        Assert.That(created.Value, Is.EqualTo(value));

        var success = TestByteValueObject.TryCreate(value, out var result);
        Assert.That(success,      Is.True);
        Assert.That(result.Value, Is.EqualTo(value));
    }

    // -------------------------
    // SHORT
    // -------------------------
    [Test]
    public void Short_Create_And_TryCreate_Work()
    {
        var value = (short)-12345;

        var created = TestShortValueObject.Create(value);
        Assert.That(created.Value, Is.EqualTo(value));

        var success = TestShortValueObject.TryCreate(value, out var result);
        Assert.That(success,      Is.True);
        Assert.That(result.Value, Is.EqualTo(value));
    }

    // -------------------------
    // INT
    // -------------------------
    [Test]
    public void Int_Create_And_TryCreate_Work()
    {
        var value = 42;

        var created = TestIntValueObject.Create(value);
        Assert.That(created.Value, Is.EqualTo(value));

        var success = TestIntValueObject.TryCreate(value, out var result);
        Assert.That(success,      Is.True);
        Assert.That(result.Value, Is.EqualTo(value));
    }

    // -------------------------
    // LONG
    // -------------------------
    [Test]
    public void Long_Create_And_TryCreate_Work()
    {
        var value = 123_456_789_0123L;

        var created = TestLongValueObject.Create(value);
        Assert.That(created.Value, Is.EqualTo(value));

        var success = TestLongValueObject.TryCreate(value, out var result);
        Assert.That(success,      Is.True);
        Assert.That(result.Value, Is.EqualTo(value));
    }

    // -------------------------
    // UINT
    // -------------------------
    [Test]
    public void UInt_Create_And_TryCreate_Work()
    {
        var value = 42u;

        var created = TestUIntValueObject.Create(value);
        Assert.That(created.Value, Is.EqualTo(value));

        var success = TestUIntValueObject.TryCreate(value, out var result);
        Assert.That(success,      Is.True);
        Assert.That(result.Value, Is.EqualTo(value));
    }

    // -------------------------
    // ULONG
    // -------------------------
    [Test]
    public void ULong_Create_And_TryCreate_Work()
    {
        var value = 999_999_999_999ul;

        var created = TestULongValueObject.Create(value);
        Assert.That(created.Value, Is.EqualTo(value));

        var success = TestULongValueObject.TryCreate(value, out var result);
        Assert.That(success,      Is.True);
        Assert.That(result.Value, Is.EqualTo(value));
    }

    // -------------------------
    // FLOAT
    // -------------------------
    [Test]
    public void Float_Create_And_TryCreate_Work()
    {
        var value = 123.45f;

        var created = TestFloatValueObject.Create(value);
        Assert.That(created.Value, Is.EqualTo(value));

        var success = TestFloatValueObject.TryCreate(value, out var result);
        Assert.That(success,      Is.True);
        Assert.That(result.Value, Is.EqualTo(value));
    }

    // -------------------------
    // DOUBLE
    // -------------------------
    [Test]
    public void Double_Create_And_TryCreate_Work()
    {
        var value = Math.PI;

        var created = TestDoubleValueObject.Create(value);
        Assert.That(created.Value, Is.EqualTo(value));

        var success = TestDoubleValueObject.TryCreate(value, out var result);
        Assert.That(success,      Is.True);
        Assert.That(result.Value, Is.EqualTo(value));
    }

    // -------------------------
    // DECIMAL
    // -------------------------
    [Test]
    public void Decimal_Create_And_TryCreate_Work()
    {
        var value = 123456.789m;

        var created = TestDecimalValueObject.Create(value);
        Assert.That(created.Value, Is.EqualTo(value));

        var success = TestDecimalValueObject.TryCreate(value, out var result);
        Assert.That(success,      Is.True);
        Assert.That(result.Value, Is.EqualTo(value));
    }

    // -------------------------
    // STRING
    // -------------------------
    [Test]
    public void String_Create_And_TryCreate_Work()
    {
        var value = "hello world";

        var created = TestStringValueObject.Create(value);
        Assert.That(created.Value, Is.EqualTo(value));

        var success = TestStringValueObject.TryCreate(value, out var result);
        Assert.That(success,      Is.True);
        Assert.That(result.Value, Is.EqualTo(value));
    }

    // -------------------------
    // STRING — edge cases
    // -------------------------
    [Test]
    public void String_Allows_Empty_And_Null_When_No_Validation()
    {
        var empty = TestStringValueObject.Create(string.Empty);
        Assert.That(empty.Value, Is.EqualTo(string.Empty));

        var success = TestStringValueObject.TryCreate(null!, out var result);
        Assert.That(success,      Is.True);
        Assert.That(result.Value, Is.Null);
    }
}