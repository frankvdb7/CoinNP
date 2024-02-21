namespace COINNP.Client.Adapters;

internal class OffsetPersistorAdapter : Coin.Sdk.Common.Client.IOffsetPersister
{
    private readonly IOffsetPersister _offsetpersister;
    public OffsetPersistorAdapter(IOffsetPersister offsetPersister)
        => _offsetpersister = offsetPersister ?? throw new ArgumentNullException(nameof(offsetPersister));

    public long Offset
    {
        get => _offsetpersister.GetOffset();
        set => _offsetpersister.SetOffset(value);
    }
}
