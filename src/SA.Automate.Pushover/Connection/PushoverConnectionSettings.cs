using Umbraco.Automate.Core.Settings;

namespace SA.Automate.Pushover.Connection;

/// <summary>
/// Stores the settings for a Pushover connection in Umbraco Automate.
/// Each connection holds a single user or group key. To send notifications to different
/// users or groups, create multiple connections in the back office and select the appropriate
/// one per workflow.
/// </summary>
public sealed class PushoverConnectionSettings
{
    /// <summary>
    /// The Pushover user or group key that notifications will be delivered to.
    /// Marked as sensitive so the value is masked in the back office.
    /// </summary>
    [Field(
     Label = "User/Group Key",
     Description = "Enter your key for notifications",
     IsSensitive = true,
     SortOrder = 1)]
    public string UserKey { get; set; } = string.Empty;
}

