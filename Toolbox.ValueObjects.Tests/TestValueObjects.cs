namespace Toolbox.ValueObjects.Tests;

using CodeGeneration.Attributes;

#region Test ValueObjects declarations

[ValueObject(typeof(byte))]
public readonly partial struct TestByteValueObject;

[ValueObject(typeof(short))]
public readonly partial struct TestShortValueObject;

[ValueObject(typeof(int))]
public readonly partial struct TestIntValueObject;

[ValueObject(typeof(long))]
public readonly partial struct TestLongValueObject;

[ValueObject(typeof(uint))]
public readonly partial struct TestUIntValueObject;

[ValueObject(typeof(ulong))]
public readonly partial struct TestULongValueObject;

[ValueObject(typeof(float))]
public readonly partial struct TestFloatValueObject;

[ValueObject(typeof(double))]
public readonly partial struct TestDoubleValueObject;

[ValueObject(typeof(decimal))]
public readonly partial struct TestDecimalValueObject;

[ValueObject(typeof(string))]
public readonly partial struct TestStringValueObject;

#endregion