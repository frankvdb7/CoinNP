namespace COINNP.Entities.Messages.Attributes;

/// <summary>
/// Indicates no more followup messages after this message are possible.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class LastMessageAttribute : Attribute { }
