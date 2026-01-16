namespace Toolbox.ValueObjects;

using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using CodeGeneration.Attributes;

[ValueObject(typeof(int))]
public readonly partial struct OrderId;