namespace COINNP.Entities;

public record MessageEnvelope(
    Header Header,
    Message Body
);