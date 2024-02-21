using COINNP.Entities.Common;
using COINNP.Entities.SequenceItems;
using C = Coin.Sdk.NP.Messages.V3;

namespace COINNP.Client.Mapping;

internal static class IEnumerableExtensions
{
    internal static EnumProfile FromCOINRepeats(this C.EnumProfileSeq repeats, IValueHelper valueHelper)
        => new(
            repeats.ProfileId
        );

    internal static List<C.EnumRepeats> ToCOINRepeats(this IEnumerable<EnumProfile> items, IValueHelper valueHelper)
        => valueHelper.ConvertItems(items, i => new C.EnumRepeats
        {
            Seq = new C.EnumProfileSeq
            {
                ProfileId = i.ProfileId
            }
        });

    internal static ActivationServiceNumberItem FromCOINRepeats(this C.ActivationServiceNumberRepeats repeats, IValueHelper valueHelper)
        => new(
              repeats.Seq.NumberSeries.FromCOIN(valueHelper),
              repeats.Seq.TariffInfo.FromCOIN(valueHelper),
              repeats.Seq.Pop
        );

    internal static List<C.ActivationServiceNumberRepeats> ToCOINRepeats(this IEnumerable<ActivationServiceNumberItem> items, IValueHelper valueHelper)
        => valueHelper.ConvertItems(items, i => new C.ActivationServiceNumberRepeats
        {
            Seq = new C.ActivationServiceNumberSeq
            {
                Pop = i.PoP,
                TariffInfo = i.TariffInfo.ToCOIN(valueHelper),
                NumberSeries = i.NumberSerie.ToCOIN(valueHelper)
            }
        });

    internal static DeactivationItem FromCOINRepeats(this C.DeactivationRepeats repeats, IValueHelper valueHelper)
        => new(
            repeats.Seq.NumberSeries.FromCOIN(valueHelper)
        );

    internal static List<C.DeactivationRepeats> ToCOINRepeats(this IEnumerable<DeactivationItem> items, IValueHelper valueHelper)
        => valueHelper.ConvertItems(items, i => new C.DeactivationRepeats
        {
            Seq = new C.DeactivationSeq
            {
                NumberSeries = i.NumberSerie.ToCOIN(valueHelper)
            }
        });

    internal static DeactivationServiceNumberItem FromCOINRepeats(this C.DeactivationServiceNumberRepeats repeats, IValueHelper valueHelper)
        => new(
            repeats.Seq.NumberSeries.FromCOIN(valueHelper),
            repeats.Seq.Pop
        );

    internal static List<C.DeactivationServiceNumberRepeats> ToCOINRepeats(this IEnumerable<DeactivationServiceNumberItem> items, IValueHelper valueHelper)
        => valueHelper.ConvertItems(items, i => new C.DeactivationServiceNumberRepeats
        {
            Seq = new C.DeactivationServiceNumberSeq
            {
                Pop = i.PoP,
                NumberSeries = i.NumberSerie.ToCOIN(valueHelper)
            }
        });

    internal static EnumNumberItem FromCOINRepeats(this C.EnumNumberRepeats repeats, IValueHelper valueHelper)
        => new(
             repeats.Seq.NumberSeries.FromCOIN(valueHelper),
             valueHelper.ConvertRepeats(repeats.Seq.Repeats, i => i.Seq.FromCOINRepeats(valueHelper))
        );

    internal static EnumOperatorItem FromCOINRepeats(this C.EnumOperatorRepeats repeats, IValueHelper valueHelper)
        => new(
             repeats.Seq.ProfileId,
             repeats.Seq.DefaultService
        );

    internal static ErrorFoundItem FromCOINRepeats(this C.ErrorFoundRepeats repeats, IValueHelper valueHelper)
        => new(
            repeats.Seq.PhoneNumber,
            repeats.Seq.ErrorCode,
            repeats.Seq.Description
        );

    internal static List<C.ErrorFoundRepeats> ToCOINRepeats(this IEnumerable<ErrorFoundItem> items, IValueHelper valueHelper)
        => valueHelper.ConvertItems(items, i => new C.ErrorFoundRepeats
        {
            Seq = new C.ErrorFoundSeq
            {
                Description = i.Description,
                ErrorCode = i.ErrorCode,
                PhoneNumber = i.PhoneNumber
            }
        });

    internal static PortingPerformedItem FromCOINRepeats(this C.PortingPerformedRepeats repeats, IValueHelper valueHelper)
        => new(
            repeats.Seq.NumberSeries.FromCOIN(valueHelper),
            valueHelper.ParseNullableBool(repeats.Seq.BackPorting),
            repeats.Seq.Repeats == null
                ? null
                : valueHelper.ConvertRepeats(repeats.Seq.Repeats, i => i.Seq.FromCOINRepeats(valueHelper)),
            repeats.Seq.Pop
        );

    internal static List<C.PortingPerformedRepeats> ToCOINRepeats(this IEnumerable<PortingPerformedItem> items, IValueHelper valueHelper)
        => valueHelper.ConvertItems(items, i => new C.PortingPerformedRepeats
        {
            Seq = new C.PortingPerformedSeq
            {
                NumberSeries = i.NumberSerie.ToCOIN(valueHelper),
                Repeats = i.EnumProfiles?.ToCOINRepeats(valueHelper),
                BackPorting = valueHelper.SerializeNullableBool(i.BackPorting),
                Pop = i.PoP
            }
        });

    internal static PortingRequestAnswerItem FromCOINRepeats(this C.PortingRequestAnswerRepeats repeats, IValueHelper valueHelper)
        => new(
             repeats.Seq.NumberSeries.FromCOIN(valueHelper),
             repeats.Seq.BlockingCode,
             valueHelper.ParseNullableDateTimeOffset(repeats.Seq.FirstPossibleDate),
             repeats.Seq.Note,
             repeats.Seq.DonorNetworkOperator,
             repeats.Seq.DonorServiceProvider
        );

    internal static List<C.PortingRequestAnswerRepeats> ToCOINRepeats(this IEnumerable<PortingRequestAnswerItem> items, IValueHelper valueHelper)
        => valueHelper.ConvertItems(items, i => new C.PortingRequestAnswerRepeats
        {
            Seq = new C.PortingRequestAnswerSeq
            {
                NumberSeries = i.NumberSerie.ToCOIN(valueHelper),
                BlockingCode = i.BlockingCode,
                FirstPossibleDate = valueHelper.SerializeNullableDateTimeOffset(i.FirstPossibleDate),
                Note = i.Note,
                DonorNetworkOperator = i.DonorNetworkOperator,
                DonorServiceProvider = i.DonorServiceProvider
            }
        });

    internal static PortingRequestItem FromCOINRepeats(this C.PortingRequestRepeats repeats, IValueHelper valueHelper)
        => new(
            repeats.Seq.NumberSeries.FromCOIN(valueHelper),
            repeats.Seq.Repeats == null
                ? null
                : valueHelper.ConvertRepeats(repeats.Seq.Repeats, i => i.Seq.FromCOINRepeats(valueHelper))
            );

    internal static List<C.PortingRequestRepeats> ToCOINRepeats(this IEnumerable<PortingRequestItem> items, IValueHelper valueHelper)
        => valueHelper.ConvertItems(items, i => new C.PortingRequestRepeats
        {
            Seq = new C.PortingRequestSeq
            {
                NumberSeries = i.NumberSerie.ToCOIN(valueHelper),
                Repeats = i.EnumProfiles?.ToCOINRepeats(valueHelper)
            }
        });

    internal static RangeItem FromCOINRepeats(this C.RangeRepeats repeats, IValueHelper valueHelper)
        => new(
            repeats.Seq.NumberSeries.FromCOIN(valueHelper),
            repeats.Seq.Pop
        );

    internal static List<C.RangeRepeats> ToCOINRepeats(this IEnumerable<RangeItem> items, IValueHelper valueHelper)
        => valueHelper.ConvertItems(items, i => new C.RangeRepeats
        {
            Seq = new C.RangeSeq
            {
                NumberSeries = i.NumberSerie.ToCOIN(valueHelper),
                Pop = i.PoP
            }
        });

    internal static TariffChangeServiceNumberItem FromCOINRepeats(this C.TariffChangeServiceNumberRepeats repeats, IValueHelper valueHelper)
        => new(
            repeats.Seq.NumberSeries.FromCOIN(valueHelper),
            repeats.Seq.TariffInfoNew.FromCOIN(valueHelper)
        );

    internal static List<C.TariffChangeServiceNumberRepeats> ToCOINRepeats(this IEnumerable<TariffChangeServiceNumberItem> items, IValueHelper valueHelper)
        => valueHelper.ConvertItems(items, i => new C.TariffChangeServiceNumberRepeats
        {
            Seq = new C.TariffChangeServiceNumberSeq
            {
                NumberSeries = i.NumberSerie.ToCOIN(valueHelper),
                TariffInfoNew = i.TariffInfoNew.ToCOIN(valueHelper)
            }
        });

    internal static List<C.EnumNumberRepeats> ToCOINRepeats(this IEnumerable<EnumNumberItem> items, IValueHelper valueHelper)
        => valueHelper.ConvertItems(items, i => new C.EnumNumberRepeats
        {
            Seq = new C.EnumNumberSeq
            {
                NumberSeries = i.NumberSerie.ToCOIN(valueHelper),
                Repeats = i.EnumProfiles?.ToCOINRepeats(valueHelper)
            }
        });

    internal static List<C.EnumOperatorRepeats> ToCOINRepeats(this IEnumerable<EnumOperatorItem> items, IValueHelper valueHelper)
        => valueHelper.ConvertItems(items, i => new C.EnumOperatorRepeats
        {
            Seq = new C.EnumOperatorSeq
            {
                ProfileId = i.ProfileId,
                DefaultService = i.DefaultService
            }
        });
}
