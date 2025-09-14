using System.Threading.Tasks;  
using AgenticApiSentry.Tools;
using AgenticApiSentry.OpenApi;
using Xunit;

public class LintSpecToolTests
{
    [Fact]
    public async Task Lint_runs_without_crash()
    {
        var doc = await OpenApiLoader.LoadAsync("samples/bank.yaml");
        var tool = new LintSpecTool();
        var result = await tool.ExecuteAsync(doc);
        Assert.NotNull(result);
        Assert.Equal("LintSpec", result.ToolName);
    }
}
