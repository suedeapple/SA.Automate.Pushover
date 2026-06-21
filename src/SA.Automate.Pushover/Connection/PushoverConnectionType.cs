using Microsoft.Extensions.Options;
using SA.Automate.Pushover.Configuration;
using Umbraco.Automate.Core.Connections;


namespace SA.Automate.Pushover.Connection;

/// <summary>
/// Defines the Pushover connection type for Umbraco Automate.
/// Stores the user/group key per connection and validates credentials
/// against the Pushover user validation API before saving.
/// </summary>
[ConnectionType("pushover", "Pushover",
    Description = "Connect to Pushover",
    Group = "Messaging",
    Icon = "icon-plugin")]
public sealed class PushoverConnectionType : ConnectionTypeBase<PushoverConnectionSettings>
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IOptionsMonitor<PushoverSettings> _pushoverSettings;

    public PushoverConnectionType(
        ConnectionTypeInfrastructure infrastructure,
        IHttpClientFactory httpClientFactory,
        IOptionsMonitor<PushoverSettings> pushoverSettings)
        : base(infrastructure)
    {
        _httpClientFactory = httpClientFactory;
        _pushoverSettings = pushoverSettings;
    }

    /// <summary>
    /// Validates the connection by calling the Pushover user/group validation endpoint.
    /// Checks that both the user key (from the connection settings) and the API token
    /// (from appsettings) are present and accepted by the Pushover API.
    /// </summary>
    public override async Task<ConnectionValidationResult> ValidateAsync(
     object? settings,
     CancellationToken cancellationToken)
    {
        if (settings is not PushoverConnectionSettings typed)
            return ConnectionValidationResult.Failure("Connection settings are missing.");

        var pushoverSettings = settings as PushoverConnectionSettings;

        if (string.IsNullOrWhiteSpace(pushoverSettings?.UserKey))
            return ConnectionValidationResult.Failure("User key is required.");

        if (string.IsNullOrWhiteSpace(_pushoverSettings.CurrentValue.ApiToken))
        {
            return ConnectionValidationResult.Failure(
                $"No access token found for connection name '{pushoverSettings.UserKey}' in appsettings (Umbraco:Automate:Providers:Pushover:ApiToken).");
        }

        using var client = _httpClientFactory.CreateClient();

        // POST to the Pushover validation endpoint to confirm the user/group key and API token are valid
        var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["token"] = _pushoverSettings.CurrentValue.ApiToken,
            ["user"] = typed.UserKey
        });

        try
        {
            using var response = await client.PostAsync(
                "https://api.pushover.net/1/users/validate.json",
                content,
                cancellationToken);

            var body = await response.Content.ReadAsStringAsync(cancellationToken);

            // Pushover returns "status":1 in the response body on success
            return response.IsSuccessStatusCode && body.Contains("\"status\":1")
                ? ConnectionValidationResult.Success("Connected")
                : ConnectionValidationResult.Failure("Rejected the credentials.");
        }
        catch (Exception ex)
        {
            return ConnectionValidationResult.Failure("Could not reach service.", [ex.Message]);
        }
    }
}
