using AgenticApiSentry.Agent;
using AgenticApiSentry.OpenApi;
using AgenticApiSentry.Tools;
using AgenticApiSentry.Reporting;

string specPathOrUrl = "samples/petstore.yaml";
string? baseUrlOverride = null;

if (args.Length > 0) specPathOrUrl = args[0];
for (int i = 0; i < args.Length; i++)
{
    if (args[i] == "--baseUrl" && i + 1 < args.Length)
    {
        baseUrlOverride = args[i + 1];
    }
}

// Load spec
var doc = await OpenApiLoader.LoadAsync(specPathOrUrl);

// Register tools (static, mission-bound)
var tools = new ITool[]
{
    new LintSpecTool(),
    new SecurityChecksTool(),
    new RateLimitHintTool(),
    new SamplerTool(new HttpClient(), baseUrlOverride)
};

var mission = """
Assess this API for readiness in regulated environments.
Identify spec quality issues and security gaps and provide concrete fixes.
Only use registered tools. Return a structured summary with severity.
""";

var executor = StaticAgentExecutor.CreateBuilder()
    .WithTools(tools)
    .WithMission(mission)
    .Build();

var results = await executor.RunAsync(doc);

// Write reports
Directory.CreateDirectory("out");
MarkdownReportWriter.Write("out/report.md", results);
JUnitWriter.Write("out/results.junit.xml", results);

Console.WriteLine("Done. See out/report.md and out/results.junit.xml");
