namespace Toolbox.CodeGeneration.Attributes;

using System;

[AttributeUsage(AttributeTargets.Struct | AttributeTargets.Class, Inherited = false)]
public sealed class ValueObjectAttribute(Type underlyingType) : Attribute
{
    public Type UnderlyingType { get; } = underlyingType;

    // При желании сюда можно докинуть настройки:
    // public bool AllowImplicitToPrimitive { get; init; } = false;
    // public bool ImplementComparable { get; init; } = false;
}

