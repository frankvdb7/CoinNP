namespace COINNP.Entities.Messages.Attributes;

/// <summary>
/// Indicates which followup messages can follow after this message.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class FollowupMessagesAttribute : Attribute
{
    public Type[] MessageTypes { get; set; }
    public FollowupMessagesAttribute(params Type[] messageTypes)
        => MessageTypes = messageTypes;
}