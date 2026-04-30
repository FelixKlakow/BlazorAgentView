using Markdig;
using Microsoft.AspNetCore.Components;
using System.Text.RegularExpressions;

namespace BlazorAgentView.Services;

public class DefaultMarkdownRenderer : IMarkdownRenderer
{
    private static readonly MarkdownPipeline _pipeline = new MarkdownPipelineBuilder()
        .UseAdvancedExtensions()
        .Build();

    private static readonly Regex _dangerousTags = new(
        @"<\s*(script|iframe|object|embed|form|input|button|link|meta|style)[^>]*>.*?<\s*/\s*\1\s*>|<\s*(script|iframe|object|embed|form|input|button|link|meta|style)[^>]*/?>",
        RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);

    private static readonly Regex _onEventAttributes = new(
        @"\s+on\w+\s*=\s*[""'][^""']*[""']",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);

    public MarkupString Render(string markdown)
    {
        if (string.IsNullOrEmpty(markdown))
            return new MarkupString(string.Empty);

        var html = Markdown.ToHtml(markdown, _pipeline);
        html = _dangerousTags.Replace(html, string.Empty);
        html = _onEventAttributes.Replace(html, string.Empty);
        return new MarkupString(html);
    }
}
