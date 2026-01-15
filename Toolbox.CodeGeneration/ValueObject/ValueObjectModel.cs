namespace Toolbox.CodeGeneration.ValueObject;

internal sealed class ValueObjectModel(
    string nameSpace,
    string typeName,
    string fullTypeName,
    string accessibility,
    bool isStruct,
    bool isRecord,
    string underlyingTypeFullName,
    bool allowImplicitToPrimitive,
    bool allowValidation,
    bool implementComparable,
    bool rawValueIsNullable)
{
    public string Namespace                { get; set; } = nameSpace;
    public string TypeName                 { get; set; } = typeName;
    public string FullTypeName             { get; set; } = fullTypeName;
    public string Accessibility            { get; set; } = accessibility;
    public bool   IsStruct                 { get; set; } = isStruct;
    public bool   IsRecord                 { get; set; } = isRecord;
    public string UnderlyingTypeFullName   { get; set; } = underlyingTypeFullName;
    public bool   AllowValidation          { get; set; } = allowValidation;
    public bool   AllowImplicitToPrimitive { get; set; } = allowImplicitToPrimitive;
    public bool   ImplementComparable      { get; set; } = implementComparable;
    public bool   RawValueIsNullable       { get; set; } = rawValueIsNullable;
}