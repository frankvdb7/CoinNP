using COINNP.Client.Exceptions;
using COINNP.Client.ResourceFiles;
using COINNP.Entities;
using Newtonsoft.Json;
using C = Coin.Sdk.NP.Messages.V3;

namespace COINNP.Client.Mapping;

public static class ToCOINJsonExtensionMethods
{
    private static readonly JsonSerializerSettings _defaultserializersettings = new()
    {
        NullValueHandling = NullValueHandling.Ignore,
        Formatting = Formatting.None
    };

    public static COINJson ToCOINJson(this MessageEnvelope messageEnvelope, int version = COINTypeNameVersionHelper.DefaultVersion, IValueHelper? valueHelper = null)
    {
        var me = messageEnvelope.ToCOINMessageEnvelope(valueHelper ?? ValueHelper.Default);
        var typename = COINTypeNameVersionHelper.AppendVersion(C.Utils.TypeName(me), version);
        try
        {
            return new COINJson(typename, JsonConvert.SerializeObject(me, _defaultserializersettings));
        }
        catch (Exception ex)
        {
            throw new SerializationException(Translations.ERR_Serialization_Failed, me, typename, ex);
        }
    }
}
