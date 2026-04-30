using Microsoft.AspNetCore.Components;

namespace BlazorAgentView.Models;

public class ChatMessage
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public MessageRole Role { get; set; }
    public string Content { get; set; } = string.Empty;
    public List<ToolCall> ToolCalls { get; set; } = new();
    public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.UtcNow;
    public bool IsStreaming { get; set; }
    public RenderFragment? CustomContent { get; set; }
}
