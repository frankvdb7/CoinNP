namespace COINNP.Entities.Common;
public static class NumberSerieExtensionMethods
{
    public static string ToFriendlyString(this NumberSerie numberSerie)
        => numberSerie.End.Equals(numberSerie.Start)
        ? numberSerie.Start
        : $"{numberSerie.Start}-{numberSerie.End}";
}
