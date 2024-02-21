using Coin.Sdk.NP.Messages.V3;
using Coin.Sdk.NP.Service;
using COINNP.Client.Mapping;

namespace COINNP.Client.Adapters;

/// <summary>
/// Implements COIN's INumberPortabilityMessageListener and 'routes' all messages to OnMessage, which invokes a
/// user-defined messagehandler
/// </summary>
internal class NPMessageListenerAdapter : INumberPortabilityMessageListener
{
    private readonly INPMessageHandler _messagehandler;
    private readonly INumberPortabilityService _numberportabilityservice;
    private readonly IValueHelper _valuehelper;

    public NPMessageListenerAdapter(INumberPortabilityService numberPortabilityService, INPMessageHandler messageHandler, IValueHelper valueHelper)
    {
        _numberportabilityservice = numberPortabilityService ?? throw new ArgumentNullException(nameof(numberPortabilityService));
        _messagehandler = messageHandler ?? throw new ArgumentNullException(nameof(messageHandler));
        _valuehelper = valueHelper ?? throw new ArgumentNullException(nameof(valueHelper));
    }

    public async void OnActivationServiceNumber(string messageId, ActivationServiceNumberMessage message) => await OnMessage(messageId, message).ConfigureAwait(false);
    public async void OnCancel(string messageId, CancelMessage message) => await OnMessage(messageId, message).ConfigureAwait(false);
    public async void OnDeactivation(string messageId, DeactivationMessage message) => await OnMessage(messageId, message).ConfigureAwait(false);
    public async void OnDeactivationServiceNumber(string messageId, DeactivationServiceNumberMessage message) => await OnMessage(messageId, message).ConfigureAwait(false);
    public async void OnEnumActivationNumber(string messageId, EnumActivationNumberMessage message) => await OnMessage(messageId, message).ConfigureAwait(false);
    public async void OnEnumActivationOperator(string messageId, EnumActivationOperatorMessage message) => await OnMessage(messageId, message).ConfigureAwait(false);
    public async void OnEnumActivationRange(string messageId, EnumActivationRangeMessage message) => await OnMessage(messageId, message).ConfigureAwait(false);
    public async void OnEnumDeactivationNumber(string messageId, EnumDeactivationNumberMessage message) => await OnMessage(messageId, message).ConfigureAwait(false);
    public async void OnEnumDeactivationOperator(string messageId, EnumDeactivationOperatorMessage message) => await OnMessage(messageId, message).ConfigureAwait(false);
    public async void OnEnumDeactivationRange(string messageId, EnumDeactivationRangeMessage message) => await OnMessage(messageId, message).ConfigureAwait(false);
    public async void OnEnumProfileActivation(string messageId, EnumProfileActivationMessage message) => await OnMessage(messageId, message).ConfigureAwait(false);
    public async void OnEnumProfileDeactivation(string messageId, EnumProfileDeactivationMessage message) => await OnMessage(messageId, message).ConfigureAwait(false);
    public async void OnPortingPerformed(string messageId, PortingPerformedMessage message) => await OnMessage(messageId, message).ConfigureAwait(false);
    public async void OnPortingRequest(string messageId, PortingRequestMessage message) => await OnMessage(messageId, message).ConfigureAwait(false);
    public async void OnPortingRequestAnswer(string messageId, PortingRequestAnswerMessage message) => await OnMessage(messageId, message).ConfigureAwait(false);
    public async void OnPortingRequestAnswerDelayed(string messageId, PortingRequestAnswerDelayedMessage message) => await OnMessage(messageId, message).ConfigureAwait(false);
    public async void OnRangeActivation(string messageId, RangeActivationMessage message) => await OnMessage(messageId, message).ConfigureAwait(false);
    public async void OnRangeDeactivation(string messageId, RangeDeactivationMessage message) => await OnMessage(messageId, message).ConfigureAwait(false);
    public async void OnTariffChangeServiceNumber(string messageId, TariffChangeServiceNumberMessage message) => await OnMessage(messageId, message).ConfigureAwait(false);
    public async void OnErrorFound(string messageId, ErrorFoundMessage message) => await OnMessage(messageId, message).ConfigureAwait(false);

    private async Task OnMessage<T>(string messageId, INpMessage<T> message)
        where T : INpMessageContent
    {
        try
        {
            if (await _messagehandler.OnMessageAsync(messageId, message.FromCOIN(_valuehelper)).ConfigureAwait(false) == Acknowledgement.ACK)
            {
                await _numberportabilityservice.SendConfirmationAsync(messageId).ConfigureAwait(false);
            }
        }
        catch (Exception ex)
        {
            await _messagehandler.OnExceptionAsync(ex).ConfigureAwait(false);
        }
    }

    public async void OnException(Exception exception) => await _messagehandler.OnExceptionAsync(exception).ConfigureAwait(false);
    public async void OnKeepAlive() => await _messagehandler.OnKeepAliveAsync().ConfigureAwait(false);
    public async void OnUnknownMessage(string messageId, string message) => await _messagehandler.OnUnknownMessageAsync(messageId, message).ConfigureAwait(false);
}