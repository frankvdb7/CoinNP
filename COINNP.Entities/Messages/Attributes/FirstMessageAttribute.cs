namespace COINNP.Entities.Messages.Attributes;

/// <summary>
/// Indicates this is a first message of a message flow.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class FirstMessageAttribute : Attribute { }
