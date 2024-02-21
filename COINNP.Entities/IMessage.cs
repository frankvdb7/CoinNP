namespace COINNP.Entities;

public interface IMessage
{
    int MessageCode { get; }
    string DossierId { get; init; }
}

public interface IMessage<T>
     where T : Message
{
    Header Header { get; init; }
    Message Body { get; init; }
}
