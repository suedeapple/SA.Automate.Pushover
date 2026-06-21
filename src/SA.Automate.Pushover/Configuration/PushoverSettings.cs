namespace SA.Automate.Pushover.Configuration;

/// <summary>
/// Global Pushover settings bound from the <c>appsettings.json</c> configuration section
/// <c>Umbraco:Automate:Providers:Suedeapple.Pushover</c>.
/// </summary>
public class PushoverSettings
{
    /// <summary>
    /// The configuration section path used to bind these settings.
    /// </summary>
    public const string SectionName = "Umbraco:Automate:Providers:SA.Automate.Pushover";

    /// <summary>
    /// The default number of seconds between retry attempts for emergency (priority 2) notifications.
    /// The message will be retried every 60 seconds until acknowledged or expired.
    /// </summary>
    private const string DefaultRetryValue = "60";

    /// <summary>
    /// The default number of seconds before an emergency (priority 2) notification expires.
    /// Defaults to 1800 seconds (30 minutes) if not acknowledged.
    /// </summary>
    private const string DefaultExpireValue = "1800";

    /// <summary>
    /// The Pushover application API token used to authenticate requests.
    /// Obtain this from your Pushover application dashboard at https://pushover.net/apps.
    /// </summary>
    public string ApiToken { get; set; } = string.Empty;

    /// <summary>
    /// How often (in seconds) Pushover will retry delivering an emergency (priority 2) notification
    /// until it is acknowledged. Must be at least 30 seconds. Defaults to 60.
    /// </summary>
    public string Retry { get; set; } = DefaultRetryValue;

    /// <summary>
    /// How long (in seconds) Pushover will continue retrying an emergency (priority 2) notification
    /// before giving up. Must be at most 10800 seconds (3 hours). Defaults to 1800 (30 minutes).
    /// </summary>
    public string Expire { get; set; } = DefaultExpireValue;

    /// <summary>Returns the default retry interval in seconds.</summary>
    public static string GetDefaultRetry() => DefaultRetryValue;

    /// <summary>Returns the default expiry duration in seconds.</summary>
    public static string GetDefaultExpire() => DefaultExpireValue;
}