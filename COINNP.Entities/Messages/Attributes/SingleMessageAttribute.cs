namespace COINNP.Entities.Messages.Attributes;

/// <summary>
/// Indicates the message is (and can only be) the only message in a dossier.
/// </summary>
/// <remarks>
/// This attribute implicates FirstMessage and LastMessage because IsFirstMessage(...) and IsLastMessage(...) check
/// for this attribute as well.
/// </remarks>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class SingleMessageAttribute : Attribute { }
