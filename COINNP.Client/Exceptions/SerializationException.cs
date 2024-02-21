namespace COINNP.Client.Exceptions;

public class SerializationException : COINNPException
{
    public object Object { get; init; }
    public string TypeName { get; init; }

    public SerializationException(string message, object obj, string typeName, Exception? innerException = null)
        : base(message, innerException)
    {
        Object = obj;
        TypeName = typeName;
    }
}