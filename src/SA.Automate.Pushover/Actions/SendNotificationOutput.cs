namespace SA.Automate.Pushover.Actions;

/// <summary>
/// Output produced by the <see cref="SendNotificationAction"/>.
/// </summary>
public sealed class SendNotificationOutput
{
    /// <summary>
    /// Gets the Pushover API response status (e.g., "success" or "error").
    /// </summary>
    public string? Status { get; init; }

    /// <summary>
    /// Gets the Pushover API response request ID, which can be used for tracking or debugging purposes.
    /// </summary>
    public string? Request { get; init; }
}