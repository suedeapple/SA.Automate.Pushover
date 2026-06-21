using System.Text.Json.Serialization;

namespace SA.Automate.Pushover.Models;

/// <summary>
/// Represents the response from the Pushover API.
/// </summary>
internal class PushoverApiResponse
{
    /// <summary>
    /// Gets the status code (1 for success, 0 for error).
    /// </summary>
    [JsonPropertyName("status")]
    public int Status { get; set; }

    /// <summary>
    /// Gets the request ID from Pushover, used for tracking and debugging.
    /// </summary>
    [JsonPropertyName("request")]
    public string? Request { get; set; }

    /// <summary>
    /// Gets error messages if the request failed.
    /// </summary>
    [JsonPropertyName("errors")]
    public List<string>? Errors { get; set; }
}
