using AgenticApiSentry.Agent;
using Microsoft.OpenApi.Models;

namespace AgenticApiSentry.Tools
{
    public sealed class RateLimitHintTool : ITool
    {
        public string Name => "RateLimitHint";

        public Task<ToolResult> ExecuteAsync(object context)
        {
            var doc = (OpenApiDocument)context;
            var hints = new List<string>();

            foreach (var (path, item) in doc.Paths)
            {
                foreach (var (opType, operation) in item.Operations)
                {
                    foreach (var (code, response) in operation.Responses)
                    {
                        if (response.Headers != null && response.Headers.Count > 0)
                        {
                            var headerNames = response.Headers.Keys.Select(k => k.ToLowerInvariant()).ToList();
                            if (headerNames.Any(h => h.Contains("ratelimit")) || headerNames.Any(h => h.Contains("x-rate")))
                            {
                                hints.Add($"{path} {opType}: response {code} includes potential rate-limit headers [{string.Join(", ", response.Headers.Keys)}]");
                            }
                        }
                    }
                }
            }

            var sev = "info";
            var summary = hints.Count == 0 ? "No explicit rate-limit headers detected" : $"{hints.Count} endpoints show rate-limit header hints";
            var data = new Dictionary<string, object> { ["hints"] = hints };
            return Task.FromResult(new ToolResult(Name, summary, sev, data));
        }
    }
}
