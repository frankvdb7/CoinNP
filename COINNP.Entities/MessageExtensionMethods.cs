using COINNP.Entities.Common;
using COINNP.Entities.Messages;
using COINNP.Entities.Messages.Attributes;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace COINNP.Entities;

public static class MessageExtensionMethods
{
    // These dictionaries cache results from expensive reflection-based operations.
    private static readonly ConcurrentDictionary<Type, bool> _issinglemessagecache = new();
    private static readonly ConcurrentDictionary<Type, bool> _isfirstmessagecache = new();
    private static readonly ConcurrentDictionary<Type, bool> _islastmessagecache = new();
    private static readonly ConcurrentDictionary<Type, Type[]> _canfollowupcache = new();

    /// <summary>
    ///     Returns all <see cref="NumberSerie"/>s associated from the <see cref="Message"/>, if any.
    /// </summary>
    /// <param name="message">
    ///     The <see cref="Message"/> to get the <see cref="NumberSerie"/>s from.
    /// </param>
    /// <returns>
    ///     All <see cref="NumberSerie"/>s associated from the <see cref="Message"/>, if any.
    /// </returns>
    /// <exception cref="NotSupportedException">
    ///     Thrown when the <see cref="Message"/> doesn't support <see cref="NumberSerie"/>s.
    /// </exception>
    public static IEnumerable<NumberSerie> GetNumberSeries(this Message message)
        => TryGetNumberSeries(message, out var series)
        ? series
        : throw new NotSupportedException();

    /// <summary>
    ///     Returns all <see cref="NumberSerie"/>s associated from the <see cref="Message"/>, if any.
    /// </summary>
    /// <param name="message">
    ///     The <see cref="Message"/> to get the <see cref="NumberSerie"/>s from.
    /// </param>
    /// <param name="numberSeries">
    ///     When this method returns, contains all <see cref="NumberSerie"/>s contained in the <see cref="Message"/>.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> when <see cref="Message"/> contained <see cref="NumberSerie"/>s (which may be an
    ///     empty enumerable), <see langword="false"/> otherwise.
    /// </returns>
    public static bool TryGetNumberSeries(this Message message, [NotNullWhen(true)] out IEnumerable<NumberSerie>? numberSeries)
    {
        numberSeries = message switch
        {
            ActivationServiceNumber asn => asn.Items.Select(i => i.NumberSerie),
            Deactivation d => d.Items.Select(i => i.NumberSerie),
            DeactivationServiceNumber dsn => dsn.Items.Select(i => i.NumberSerie),
            EnumActivationNumber ean => ean.Items.Select(i => i.NumberSerie),
            EnumActivationRange ear => ear.Items.Select(i => i.NumberSerie),
            EnumDeactivationNumber edn => edn.Items.Select(i => i.NumberSerie),
            EnumDeactivationRange edr => edr.Items.Select(i => i.NumberSerie),
            PortingRequest pr => pr.Items.Select(i => i.NumberSerie),
            PortingRequestAnswer pra => pra.Items.Select(i => i.NumberSerie),
            PortingPerformed pp => pp.Items.Select(i => i.NumberSerie),
            RangeActivation ra => ra.Items.Select(i => i.NumberSerie),
            RangeDeactivation rda => rda.Items.Select(i => i.NumberSerie),
            TariffChangeServiceNumber tcsn => tcsn.Items.Select(i => i.NumberSerie),
            _ => null
        };
        return numberSeries is not null;
    }

    /// <summary>
    ///     Returns all <see cref="NumberSerie"/>s with the associated profiles from the <see cref="Message"/>, if any.
    /// </summary>
    /// <param name="message">
    ///     The <see cref="Message"/> to get the <see cref="NumberSerie"/>s  and profiles from.
    /// </param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException">
    ///     Thrown when the <see cref="Message"/> does'nt support profiles.
    /// </exception>
    public static IEnumerable<KeyValuePair<NumberSerie, IEnumerable<string>>> GetProfiles(this Message message)
        => TryGetProfiles(message, out var series)
        ? series
        : throw new NotSupportedException();

    /// <summary>
    ///     Returns all <see cref="NumberSerie"/>s with the associated profiles from the <see cref="Message"/>, if any.
    /// </summary>
    /// <param name="message">
    ///     The <see cref="Message"/> to get the <see cref="NumberSerie"/>s  and profiles from.
    /// </param>
    /// <param name="profiles"></param>
    /// <returns>
    ///     <see langword="true"/> when <see cref="Message"/> contained profiles (which may be an empty enumerable),
    ///     <see langword="false"/> otherwise.
    /// </returns>
    public static bool TryGetProfiles(this Message message, [NotNullWhen(true)] out IEnumerable<KeyValuePair<NumberSerie, IEnumerable<string>>>? profiles)
    {
        profiles = message switch
        {
            EnumActivationNumber ean => ean.Items.Select(i => new KeyValuePair<NumberSerie, IEnumerable<string>>(i.NumberSerie, i.EnumProfiles.Select(p => p.ProfileId))),
            EnumActivationRange ear => ear.Items.Select(i => new KeyValuePair<NumberSerie, IEnumerable<string>>(i.NumberSerie, i.EnumProfiles.Select(p => p.ProfileId))),
            EnumDeactivationNumber edn => edn.Items.Select(i => new KeyValuePair<NumberSerie, IEnumerable<string>>(i.NumberSerie, i.EnumProfiles.Select(p => p.ProfileId))),
            EnumDeactivationRange edr => edr.Items.Select(i => new KeyValuePair<NumberSerie, IEnumerable<string>>(i.NumberSerie, i.EnumProfiles.Select(p => p.ProfileId))),
            PortingRequest pr => pr.Items.Select(i => new KeyValuePair<NumberSerie, IEnumerable<string>>(i.NumberSerie, i.EnumProfiles.Select(p => p.ProfileId))),
            PortingPerformed pp => pp.Items.Select(i => new KeyValuePair<NumberSerie, IEnumerable<string>>(i.NumberSerie, i.EnumProfiles.Select(p => p.ProfileId))),
            _ => null
        };
        return profiles is not null;
    }

    /// <summary>
    ///     Indicates wether the <see cref="Message"/> is (and can only be) the only <see cref="Message"/> in a dossier.
    /// </summary>
    /// <returns>
    ///     <see langword="true"/> when the <see cref="Message"/> is (and can only be) the only <see cref="Message"/>
    ///     in a dossier,  <see langword="true"/> otherwise.
    /// </returns>
    public static bool IsSingleMessage(this Message message)
        => _issinglemessagecache.GetOrAdd(
                message.GetType(),
                t => t.GetCustomAttributes<SingleMessageAttribute>().FirstOrDefault() is not null
            );

    /// <summary>
    ///     Indicates wether the <see cref="Message"/> is the first of a <see cref="Message"/> flow.
    /// </summary>
    /// <returns>
    ///     <see langword="true"/> when no preceeding <see cref="Message"/>s are allowed, <see langword="false"/>
    ///     otherwise.
    /// </returns>
    public static bool IsFirstMessage(this Message message)
        => _isfirstmessagecache.GetOrAdd(
                message.GetType(),
                t => t.GetCustomAttributes<FirstMessageAttribute>().FirstOrDefault() is not null || IsSingleMessage(message)
            );

    /// <summary>
    ///     Indicates no more followup <see cref="Message"/>s after this <see cref="Message"/> are possible.
    /// </summary>
    /// <returns>
    ///     <see langword="true"/> when no more followup <see cref="Message"/>s after this <see cref="Message"/> are
    ///     possible, <see langword="false"/> otherwise.
    /// </returns>
    /// <remarks>
    ///     There are no more followup messages possible when EITHER the <see cref="LastMessageAttribute "/> OR the
    ///     <see cref="SingleMessageAttribute" /> is set.
    /// </remarks>
    public static bool IsLastMessage(this Message message)
        => _islastmessagecache.GetOrAdd(
                message.GetType(),
                t => t.GetCustomAttributes<LastMessageAttribute>().FirstOrDefault() is not null || IsSingleMessage(message)
            );

    /// <summary>
    ///     Indicates wether the <see cref="Message"/> can be a followup message of the given <see cref="Message"/>.
    /// </summary>
    /// <param name="self">
    ///     The <see cref="Message"/> to check if it is a valid followup of the given <see cref="Message"/>.
    /// </param>
    /// <param name="message">
    ///     The <see cref="Message"/> to check for valid followup types.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> when the <see cref="Message"/> is a valid followup message of the given
    ///     <see cref="Message"/> and the Dossier ID's are equal, <see langword="false"/> otherwise. Note that this
    ///     method ignores blocking status of  <see cref="PortingRequestAnswer"/> for example.
    /// </returns>
    public static bool CanFollowUp(this Message self, Message message)
        => !message.IsLastMessage()
        && !message.IsSingleMessage()
        && !self.IsSingleMessage()
        && _canfollowupcache.GetOrAdd(
            message.GetType(),
            t => t.GetCustomAttributes<FollowupMessagesAttribute>().FirstOrDefault()?.MessageTypes ?? Array.Empty<Type>()
        ).Contains(self.GetType()) && (self.DossierId == message.DossierId);
}
