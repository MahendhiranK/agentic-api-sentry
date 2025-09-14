namespace AgenticApiSentry.Agent
{
    public interface ITool
    {
        string Name { get; }
        Task<ToolResult> ExecuteAsync(object context);
    }

    public record ToolResult(string ToolName, string Summary, string Severity, IDictionary<string, object>? Data = null);
}
