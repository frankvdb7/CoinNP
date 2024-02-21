using COINNP.Client.Exceptions;
using COINNP.Client.ResourceFiles;
using COINNP.Entities;
using C = Coin.Sdk.NP.Messages.V3;

namespace COINNP.Client.Mapping;

public static class COINJsonParser
{
    public static MessageEnvelope FromCOINJson(COINJson coinJson, int assumeVersion = COINTypeNameVersionHelper.DefaultVersion, IValueHelper? valueHelper = null)
        => FromCOINJson(coinJson.TypeName, coinJson.Json, assumeVersion, valueHelper);

    public static MessageEnvelope FromCOINJson(string typeName, string json, int assumeVersion = COINTypeNameVersionHelper.DefaultVersion, IValueHelper? valueHelper = null)
    {
        if (string.IsNullOrEmpty(typeName))
        {
            throw new ArgumentNullException(nameof(typeName));
        }

        if (string.IsNullOrEmpty(json))
        {
            throw new ArgumentNullException(nameof(json));
        }

        try
        {
            return C.Utils.Deserialize(COINTypeNameVersionHelper.AppendVersion(typeName, assumeVersion), json).FromCOIN(valueHelper ?? ValueHelper.Default);
        }
        catch (Exception ex)
        {
            throw new DeserializationException(Translations.ERR_Deserialization_Failed, json, typeName, ex);
        }
    }
}
