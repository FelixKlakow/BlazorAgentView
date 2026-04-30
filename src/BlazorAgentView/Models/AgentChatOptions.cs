namespace BlazorAgentView.Models;

public class AgentChatOptions
{
    public bool ShowTimestamps { get; set; } = true;
    public bool EnableMarkdown { get; set; } = true;
    public bool EnableVirtualization { get; set; } = false;
    /// <summary>
    /// When <c>true</c> (default), the messages container scrolls to the bottom
    /// when new messages are appended and the user is already near the bottom.
    /// Set to <c>false</c> to opt out of auto-scrolling entirely.
    /// </summary>
    public bool AutoScroll { get; set; } = true;
    public string? Theme { get; set; }
    public Dictionary<string, string> CssVariables { get; set; } = new();
}
