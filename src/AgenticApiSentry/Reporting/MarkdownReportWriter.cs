using AgenticApiSentry.Agent;

namespace AgenticApiSentry.Reporting
{
    public static class MarkdownReportWriter
    {
        public static void Write(string path, List<ToolResult> results)
        {
            using var sw = new StreamWriter(path);
            sw.WriteLine("# Agentic API Sentry Report");
            sw.WriteLine();
            sw.WriteLine($"Generated: {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC");
            sw.WriteLine();

            foreach (var r in results)
            {
                sw.WriteLine($"## {r.ToolName}");
                sw.WriteLine($"**Severity:** {r.Severity}");
                sw.WriteLine();
                sw.WriteLine(r.Summary);
                sw.WriteLine();

                if (r.Data != null && r.Data.TryGetValue("issues", out var issuesObj) && issuesObj is IEnumerable<object> issues)
                {
                    sw.WriteLine("### Issues");
                    foreach (var i in issues) sw.WriteLine($"- {i}");
                    sw.WriteLine();
                }

                if (r.Data != null && r.Data.TryGetValue("findings", out var findingsObj) && findingsObj is IEnumerable<object> findings)
                {
                    sw.WriteLine("### Findings");
                    foreach (var f in findings) sw.WriteLine($"- {f}");
                    sw.WriteLine();
                }

                if (r.Data != null && r.Data.TryGetValue("hints", out var hintsObj) && hintsObj is IEnumerable<object> hints)
                {
                    sw.WriteLine("### Hints");
                    foreach (var h in hints) sw.WriteLine($"- {h}");
                    sw.WriteLine();
                }

                if (r.Data != null && r.Data.TryGetValue("notes", out var notesObj) && notesObj is IEnumerable<object> notes)
                {
                    sw.WriteLine("### Notes");
                    foreach (var n in notes) sw.WriteLine($"- {n}");
                    sw.WriteLine();
                }
            }
        }
    }
}
