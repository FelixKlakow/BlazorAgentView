using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using BlazorAgentView.Models;

namespace BlazorAgentView.Components;

public partial class AgentChatView : ComponentBase, IAsyncDisposable
{
    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;

    [Parameter] public IEnumerable<ChatMessage> Messages { get; set; } = Enumerable.Empty<ChatMessage>();
    [Parameter] public RenderFragment? UserInputContent { get; set; }
    [Parameter] public string? SystemPrompt { get; set; }
    [Parameter] public AgentChatOptions Options { get; set; } = new();
    [Parameter] public EventCallback<string> OnCancelTool { get; set; }
    [Parameter] public RenderFragment<ToolCall>? ToolContentTemplate { get; set; }

    private List<ChatMessage> _messageList = new();
    private ElementReference _containerRef;
    private ElementReference _messagesRef;
    private IJSObjectReference? _jsModule;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            try
            {
                _jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>(
                    "import", "./_content/BlazorAgentView/blazor-agent-view.js");
            }
            catch (JSDisconnectedException)
            {
                // Circuit gone (e.g. user navigated away) - safe to ignore.
            }
            catch (InvalidOperationException)
            {
                // JS interop is unavailable (e.g. during static SSR / prerender).
            }
            catch (TaskCanceledException)
            {
                // Component disposed before import completed.
            }
        }
        await ScrollToBottomAsync();
    }

    protected override void OnParametersSet()
    {
        _messageList = Messages.ToList();
    }

    private async Task ScrollToBottomAsync()
    {
        if (_jsModule is null) return;
        try
        {
            await _jsModule.InvokeVoidAsync("scrollToBottom", _messagesRef);
        }
        catch (JSDisconnectedException) { /* circuit gone */ }
        catch (InvalidOperationException) { /* JS unavailable */ }
        catch (TaskCanceledException) { /* component disposed */ }
    }

    public void AddMessage(ChatMessage message)
    {
        _messageList.Add(message);
        StateHasChanged();
    }

    public void AppendToMessage(string id, string text)
    {
        var message = _messageList.FirstOrDefault(m => m.Id == id);
        if (message != null)
        {
            message.Content += text;
            StateHasChanged();
        }
    }

    public void UpdateToolState(string messageId, string toolCallId, ToolState state, string? output = null)
    {
        var message = _messageList.FirstOrDefault(m => m.Id == messageId);
        if (message != null)
        {
            var toolCall = message.ToolCalls.FirstOrDefault(t => t.Id == toolCallId);
            if (toolCall != null)
            {
                toolCall.State = state;
                if (output != null) toolCall.Output = output;
                StateHasChanged();
            }
        }
    }

    private string ThemeClass => Options.Theme != null ? $"bav-theme-{Options.Theme}" : string.Empty;

    private string CssVariableStyle
    {
        get
        {
            if (Options.CssVariables.Count == 0) return string.Empty;
            return string.Join("; ", Options.CssVariables.Select(kv => $"{kv.Key}: {kv.Value}"));
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_jsModule is not null)
        {
            try
            {
                await _jsModule.DisposeAsync();
            }
            catch (JSDisconnectedException) { /* circuit gone */ }
            catch (InvalidOperationException) { /* JS unavailable */ }
            catch (TaskCanceledException) { /* already cancelled */ }
            finally
            {
                _jsModule = null;
            }
        }
        GC.SuppressFinalize(this);
    }
}
