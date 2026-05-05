using Microsoft.AspNetCore.Components;

namespace BlazorAgentView.Models;

public class ToolCall
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string ToolName { get; set; } = string.Empty;
    public string? Input { get; set; }
    public string? Output { get; set; }
    public ToolState State { get; set; } = ToolState.Pending;
    public RenderFragment? CustomContent { get; set; }

    /// <summary>
    /// Optional secondary text rendered next to the tool name in the card header.
    /// Intended for a brief, at-a-glance summary (e.g. the file path or search query).
    /// </summary>
    public string? Subtitle { get; set; }

    /// <summary>
    /// Optional per-tool display mode override. When set it takes precedence
    /// over <see cref="AgentChatOptions.ToolCallDisplay"/>.
    /// </summary>
    public ToolCallDisplayMode? DisplayMode { get; set; }
}
