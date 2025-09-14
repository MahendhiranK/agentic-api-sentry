using AgenticApiSentry.Agent;
using Microsoft.OpenApi.Models;
using System.Net.Http;

namespace AgenticApiSentry.Tools
{
    public sealed class SamplerTool : ITool
    {
        public string Name => "Sampler";
        private readonly HttpClient _http;
        private readonly string? _baseUrl;

        public SamplerTool(HttpClient http, string? baseUrlOverride)
        {
            _http = http;
            _baseUrl = baseUrlOverride;
        }

        public async Task<ToolResult> ExecuteAsync(object context)
        {
            var doc = (OpenApiDocument)context;
            var notes = new List<string>();

            if (string.IsNullOrWhiteSpace(_baseUrl))
            {
                return new ToolResult(Name,
                    "Sampling disabled (no --baseUrl provided). Skipped real HTTP calls.",
                    "info",
                    new Dictionary<string, object> { ["sampled"] = 0, ["skipped"] = "no baseUrl" });
            }

            int sampled = 0;
            foreach (var (path, item) in doc.Paths)
            {
                foreach (var (opType, operation) in item.Operations)
                {
                    if (opType != OperationType.Get) continue; // only GET for safety
                    if (path.Contains("{")) continue; // skip required params for safety

                    var url = CombineUrl(_baseUrl!, path);
                    try
                    {
                        using var req = new HttpRequestMessage(HttpMethod.Get, url);
                        using var resp = await _http.SendAsync(req);
                        notes.Add($"{opType} {url} => {(int)resp.StatusCode}");
                        sampled++;
                        if (sampled >= 5) break; // limit requests
                    }
                    catch (Exception ex)
                    {
                        notes.Add($"{opType} {url} => error: {ex.Message}");
                    }
                }
                if (sampled >= 5) break;
            }

            var sev = "info";
            var summary = sampled == 0 ? "No endpoints sampled" : $"Sampled {sampled} GET endpoints";
            var data = new Dictionary<string, object> { ["notes"] = notes, ["sampled"] = sampled };
            return new ToolResult(Name, summary, sev, data);
        }

        private static string CombineUrl(string baseUrl, string path)
        {
            if (baseUrl.EndsWith("/")) baseUrl = baseUrl.TrimEnd('/');
            return $"{baseUrl}{path}";
        }
    }
}
