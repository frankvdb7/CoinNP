namespace COINNP.Client.Exceptions;

public class DeserializationException : COINNPException
{
    public string Json { get; init; }
    public string TypeName { get; init; }

    public DeserializationException(string message, string json, string typeName, Exception? innerException = null)
        : base(message, innerException)
    {
        Json = json;
        TypeName = typeName;
    }
}
