using COINNP.Entities.Messages;
using COINNP.Entities.Messages.Attributes;

namespace COINNP.Entities;
public static class MessageEnvelopeExtensionMethods
{
    /// <summary>
    /// Returns the <see cref="Message.DossierId"/> of the message contained in this <see cref="MessageEnvelope"/>.
    /// </summary>
    /// <returns>The <see cref="Message.DossierId"/> of the message contained in this <see cref="MessageEnvelope"/>.</returns>
    public static string GetDossierId(this MessageEnvelope messageEnvelope)
        => messageEnvelope.Body.DossierId;

    /// <summary>
    ///     Indicates wether the <see cref="MessageEnvelope"/> is (and can only be) the only
    ///     <see cref="MessageEnvelope"/> in a dossier.
    /// </summary>
    /// <returns>
    ///     <see langword="true"/> when the <see cref="MessageEnvelope"/> is (and can only be) the only
    ///     <see cref="MessageEnvelope"/> in a dossier, <see langword="false"/> otherwise.
    /// </returns>
    public static bool IsSingleMessage(this MessageEnvelope messageEnvelope)
        => messageEnvelope.Body.IsSingleMessage();

    /// <summary>
    ///     Indicates first <see cref="MessageEnvelope"/> of a <see cref="MessageEnvelope"/> flow.
    /// </summary>
    /// <returns>
    ///     <see langword="true"/> when no preceeding <see cref="MessageEnvelope"/>s are allowed,
    ///     <see langword="false"/> otherwise.
    /// </returns>
    public static bool IsFirstMessage(this MessageEnvelope messageEnvelope)
        => messageEnvelope.Body.IsFirstMessage();

    /// <summary>
    ///     Indicates no more followup <see cref="MessageEnvelope"/>s after this <see cref="MessageEnvelope"/> are
    ///     possible.
    /// </summary>
    /// <returns>
    ///     <see langword="true"/> when no more followup <see cref="MessageEnvelope"/>s after this
    ///     <see cref="MessageEnvelope"/> are possible, <see langword="true"/> otherwise.
    /// </returns>
    /// <remarks>
    ///     There are no more followup messages possible when EITHER the <see cref="LastMessageAttribute"/> OR the
    ///     <see cref="SingleMessageAttribute" /> is set.
    /// </remarks>
    public static bool IsLastMessage(this MessageEnvelope messageEnvelope)
        => messageEnvelope.Body.IsLastMessage();

    /// <summary>
    ///     Indicates wether the <see cref="MessageEnvelope"/> can be a followup message of the given
    ///     <see cref="MessageEnvelope"/>.
    /// </summary>
    /// <param name="self">
    ///     The <see cref="MessageEnvelope"/> to check if it is a valid followup of the given
    ///     <see cref="MessageEnvelope"/>.
    /// </param>
    /// <param name="messageEnvelope">
    ///     The <see cref="MessageEnvelope"/> to check for valid followup types.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> when the <see cref="MessageEnvelope"/> is a valid followup
    ///     <see cref="MessageEnvelope"/> of the given <see cref="MessageEnvelope"/> and the Dossier ID's are equal,
    ///     <see langword="false"/> otherwise. Note that this method ignores sender/receiver or blocking status of
    ///     <see cref="PortingRequestAnswer"/> for example.
    /// </returns>
    public static bool CanFollowUp(this MessageEnvelope self, MessageEnvelope messageEnvelope)
        => !messageEnvelope.IsLastMessage()
        && !messageEnvelope.IsSingleMessage()
        && !self.IsSingleMessage()
        && self.Body.CanFollowUp(messageEnvelope.Body);

}