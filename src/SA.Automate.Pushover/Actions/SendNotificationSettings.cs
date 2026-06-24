using Umbraco.Automate.Core.Settings;


namespace SA.Automate.Pushover.Actions;

/// <summary>
/// Defines the configurable settings for the Send Notification action in Umbraco Automate.
/// Each property maps to an editor field in the back office workflow step configuration.
/// </summary>
public class SendNotificationSettings
{
    /// <summary>An optional title displayed above the message in the notification.</summary>
    [Field(Label = "Title", Description = "An optional title to display above the message. Supports bindings.", SupportsBindings = true)]
    public string? Title { get; set; }

    /// <summary>The main body text of the notification. Required.</summary>
    [Field(
        Label = "Message",
        Description = "The main content of the notification to be sent. Supports bindings.",
        SortOrder = 1,
        SupportsBindings = true,
        EditorUiAlias = "Umb.PropertyEditorUi.TextArea",
        EditorConfig = """[{ "alias": "rows", "value": 4 }]""")]
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// The sound played on the device when the notification arrives.
    /// Defaults to the Pushover default sound. See https://pushover.net/api#sounds for available values.
    /// </summary>
    [Field(
        Label = "Sound",
        Description = "The sound to play on the device. See https://pushover.net/api#sounds.",
        SortOrder = 2,
        EditorUiAlias = "Umb.PropertyEditorUi.Dropdown",
        EditorConfig = """[{ "alias": "items", "value": ["pushover", "bike", "bugle", "cashregister", "classical", "cosmic", "falling", "gamelan", "incoming", "intermission", "magic", "mechanical", "pianobar", "siren", "spacealarm", "tugboat", "alien", "climb", "persistent", "echo", "updown", "vibrate", "none"] }]""")]
    public string? Sound  { get; set; } = "pushover";

    /// <summary>
    /// An optional custom sound name, for sounds uploaded to your Pushover account.
    /// When provided, this overrides the <see cref="Sound"/> selection.
    /// </summary>
    [Field(
        Label = "Custom Sound",
        Description = "Override Sound with a custom sound name uploaded to your Pushover account. Takes priority over Sound if provided. Supports bindings.",
        SortOrder = 3,
        SupportsBindings = true)]
    public string? CustomSound { get; set; }

    /// <summary>
    /// An optional URL attached to the notification. Supports https://, mailto:, tel:, and other URI schemes.
    /// </summary>
    [Field(Label = "URL",  Description = "An optional URL to be included with the notification that users can open directly from Pushover. Supports standard URLs as well as mailto:, tel:, and other URI schemes. Supports bindings." , SortOrder = 4, SupportsBindings = true)]
    public string? Url { get; set; }

    /// <summary>An optional display title for the URL link. When omitted, the raw URL is shown.</summary>
    [Field(Label = "URL Title", Description = "An optional title for the URL link. If not provided, the URL will be displayed as-is. Supports bindings.", SortOrder = 5, SupportsBindings = true)]
    public string? UrlTitle { get; set; }

    /// <summary>
    /// Controls the urgency of the notification.
    /// Min = -2, Low = -1, Default = 0, High = 1, Max = 2 (emergency, requires retry and expire).
    /// Defaults to "Default".
    /// </summary>
    [Field(
        Label = "Priority",
        Description = "Set the notification priority level.",
        SortOrder = 6,
        EditorUiAlias = "Umb.PropertyEditorUi.Dropdown",
        EditorConfig = """[{ "alias": "items", "value": ["Min", "Low", "Default", "High", "Max"] }]""")]
    public string Priority { get; set; } = "Default";

}