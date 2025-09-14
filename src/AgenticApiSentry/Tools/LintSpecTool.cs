using AgenticApiSentry.Agent;
using Microsoft.OpenApi.Models;

namespace AgenticApiSentry.Tools
{
    public sealed class LintSpecTool : ITool
    {
        public string Name => "LintSpec";

        public Task<ToolResult> ExecuteAsync(object context)
        {
            var doc = (OpenApiDocument)context;
            var issues = new List<string>();

            foreach (var (path, item) in doc.Paths)
            {
                foreach (var (opType, operation) in item.Operations)
                {
                    if (string.IsNullOrWhiteSpace(operation.Summary))
                        issues.Add($"{path} {opType}: missing summary");
                    if (operation.Responses.Count == 0)
                        issues.Add($"{path} {opType}: missing responses");
                    if (operation.Parameters != null)
                    {
                        foreach (var p in operation.Parameters)
                        {
                            if (string.IsNullOrWhiteSpace(p.Name))
                                issues.Add($"{path} {opType}: parameter with no name");
                        }
                    }
                }
            }

            var sev = issues.Count == 0 ? "info" : "warn";
            var summary = issues.Count == 0 ? "Spec looks consistent" : $"{issues.Count} potential issues";
            var data = new Dictionary<string, object> { ["issues"] = issues };
            return Task.FromResult(new ToolResult(Name, summary, sev, data));
        }
    }
}
