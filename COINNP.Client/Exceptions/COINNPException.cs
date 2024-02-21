namespace COINNP.Client.Exceptions;
public abstract class COINNPException : Exception
{
    public COINNPException(string message, Exception? innerException = null)
        : base(message, innerException) { }
}