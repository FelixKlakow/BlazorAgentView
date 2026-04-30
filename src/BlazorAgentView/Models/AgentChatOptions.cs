namespace BlazorAgentView.Models;

public class AgentChatOptions
{
    public bool ShowTimestamps { get; set; } = true;
    public bool EnableMarkdown { get; set; } = true;
    public bool EnableVirtualization { get; set; } = false;
    public string? Theme { get; set; }
    public Dictionary<string, string> CssVariables { get; set; } = new();
}
