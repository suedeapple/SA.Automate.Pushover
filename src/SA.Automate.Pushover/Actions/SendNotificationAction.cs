using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SA.Automate.Pushover.Configuration;
using SA.Automate.Pushover.Connection;
using SA.Automate.Pushover.Models;
using Umbraco.Automate.Core.Actions;

namespace SA.Automate.Pushover.Actions;


/// <summary>
/// Umbraco Automate action that sends a push notification via the Pushover API.
/// Supports optional title, sound, URL, and priority.
/// </summary>
[Action("pushover.SendNotification", "Send Pushover Notification",
    Description = "Sends a Pushover Notification",
    Group = "Messaging",
    Icon = "icon-bell",
    ConnectionTypeAlias = "pushover")]

public class SendNotificationAction : ActionBase<SendNotificationSettings, SendNotificationOutput>
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<SendNotificationAction> _logger;
    private readonly IOptionsMonitor<PushoverSettings> _pushoverSettings;

    public SendNotificationAction(
        ActionInfrastructure infrastructure,
        IHttpClientFactory httpClientFactory,
        ILogger<SendNotificationAction> logger,
        IOptionsMonitor<PushoverSettings> pushoverSettings)
        : base(infrastructure)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
        _pushoverSettings = pushoverSettings;
    }

    /// <summary>
    /// Executes the action by building the notification payload and posting it
    /// to the Pushover messages API. Returns a failed result on validation errors,
    /// missing configuration, or a non-success API response.
    /// </summary>
    public override async Task<ActionResult> ExecuteAsync(
     ActionContext context,
     CancellationToken cancellationToken)
    {
        var settings = context.GetSettings<SendNotificationSettings>();

        // Message is the only required field
        if (string.IsNullOrWhiteSpace(settings.Message))
        {
            return ActionResult.Failed(
                new ArgumentException("Message is required."),
                StepRunErrorCategory.Validation);
        }

        var connectionSettings = context.Connection?.GetSettings<PushoverConnectionSettings>();

        // Both the connection user key and the global API token must be present
        if (connectionSettings == null || string.IsNullOrWhiteSpace(connectionSettings.UserKey) || string.IsNullOrWhiteSpace(_pushoverSettings.CurrentValue.ApiToken))
        {
            return ActionResult.Failed(
                new ArgumentException("Pushover connection settings are not configured properly."),
                StepRunErrorCategory.Authentication);
        }

        try
        {
            var httpClient = _httpClientFactory.CreateClient();

            // Use the action-level user key if provided, otherwise fall back to the connection default
            var requestContent = new Dictionary<string, string>
            {
                { "token", _pushoverSettings.CurrentValue.ApiToken },
                { "user",  connectionSettings.UserKey },
                { "message", settings.Message }
            };

            if (!string.IsNullOrWhiteSpace(settings.Title))
            {
                requestContent["title"] = settings.Title;
            }

            // A custom uploaded sound name takes priority over the Sound dropdown when provided
            var sound = !string.IsNullOrWhiteSpace(settings.CustomSound) ? settings.CustomSound : settings.Sound;
            if (!string.IsNullOrWhiteSpace(sound))
            {
                requestContent["sound"] = sound;
            }

            if (!string.IsNullOrWhiteSpace(settings.Url))
            {
                requestContent["url"] = settings.Url;
            }

            if (!string.IsNullOrWhiteSpace(settings.UrlTitle))
            {
                requestContent["url_title"] = settings.UrlTitle;
            }

            // Map the Priority dropdown label to the Pushover API's numeric priority scale
            var priorityValue = settings.Priority switch
            {
                "Min" => -2,
                "Low" => -1,
                "High" => 1,
                "Max" => 2,
                _ => 0
            };

            if (priorityValue != 0)
            {
                requestContent["priority"] = priorityValue.ToString();
            }

            // For emergency priority (Max), retry and expire are required by the Pushover API
            if (priorityValue == 2)
            {
                // Use configured retry or fall back to default
                var retryValue = _pushoverSettings.CurrentValue.Retry;
                if (string.IsNullOrWhiteSpace(retryValue) || !int.TryParse(retryValue, out _))
                {
                    retryValue = PushoverSettings.GetDefaultRetry();
                }
                requestContent["retry"] = retryValue;

                // Use configured expire or fall back to default
                var expireValue = _pushoverSettings.CurrentValue.Expire;
                if (string.IsNullOrWhiteSpace(expireValue) || !int.TryParse(expireValue, out _))
                {
                    expireValue = PushoverSettings.GetDefaultExpire();
                }
                requestContent["expire"] = expireValue;
            }

            var content = new FormUrlEncodedContent(requestContent);

            var response = await httpClient.PostAsync(
                "https://api.pushover.net/1/messages.json",
                content,
                cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
                _logger.LogError("Pushover API request failed with status {StatusCode}: {ErrorContent}", 
                    response.StatusCode, errorContent);

                return ActionResult.Failed(
                    new Exception($"Pushover API returned status {response.StatusCode}"),
                    StepRunErrorCategory.InvalidResponse);
            }

            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

            // Deserialise the Pushover response to extract the status code and unique request ID
            var pushoverResponse = System.Text.Json.JsonSerializer.Deserialize<PushoverApiResponse>(responseContent);

            _logger.LogInformation("Notification sent successfully to Pushover - Status: {Status}, Request ID: {RequestId}", pushoverResponse?.Status, pushoverResponse?.Request);

            // Status 1 indicates success. Both values are saved as action output so subsequent
            // workflow steps can reference them via data bindings (e.g. to log or use in another workflow)
            return Success(new SendNotificationOutput
            {
                Status = pushoverResponse?.Status.ToString() ?? "unknown",
                Request = pushoverResponse?.Request ?? string.Empty,
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send notification to Pushover API");
            return ActionResult.Failed(ex, StepRunErrorCategory.InvalidResponse);
        }
    }

    }

