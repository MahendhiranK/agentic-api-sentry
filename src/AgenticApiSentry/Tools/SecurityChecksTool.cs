using AgenticApiSentry.Agent;
using Microsoft.OpenApi.Models;

namespace AgenticApiSentry.Tools
{
    public sealed class SecurityChecksTool : ITool
    {
        public string Name => "SecurityChecks";

        public Task<ToolResult> ExecuteAsync(object context)
        {
            var doc = (OpenApiDocument)context;
            var findings = new List<string>();

            // 1) HTTPS check for servers
            if (doc.Servers != null && doc.Servers.Count > 0)
            {
                foreach (var s in doc.Servers)
                {
                    if (Uri.TryCreate(s.Url, UriKind.Absolute, out var uri))
                    {
                        if (uri.Scheme != Uri.UriSchemeHttps)
                            findings.Add($"Server not using HTTPS: {s.Url}");
                    }
                }
            }

            // 2) Security scheme presence
            bool hasSecurityScheme = doc.Components?.SecuritySchemes?.Count > 0;
            if (!hasSecurityScheme)
            {
                findings.Add("No security schemes defined in components.securitySchemes");
            }

            // 3) Any operation without responses 401/403? (best-effort check)
            foreach (var (path, item) in doc.Paths)
            {
                foreach (var (opType, operation) in item.Operations)
                {
                    var statusCodes = operation.Responses.Keys.Select(k => k.ToString()).ToHashSet();
                    if (!(statusCodes.Contains("401") || statusCodes.Contains("403")))
                    {
                        findings.Add($"{path} {opType}: no documented 401/403 responses");
                    }
                }
            }

            var sev = findings.Count == 0 ? "info" : "warn";
            var summary = findings.Count == 0 ? "Basic security checks passed" : $"{findings.Count} potential security gaps";
            var data = new Dictionary<string, object> { ["findings"] = findings };
            return Task.FromResult(new ToolResult(Name, summary, sev, data));
        }
    }
}
