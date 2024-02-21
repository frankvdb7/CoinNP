namespace COINNP.Entities;

[AttributeUsage(AttributeTargets.Field)]
public class EnumValueAttribute : Attribute
{
    public string Value { get; private set; }

    public EnumValueAttribute(string value) => Value = value;
}