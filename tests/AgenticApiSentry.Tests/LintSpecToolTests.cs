using System.IO;                  
using System.Threading.Tasks;  
using AgenticApiSentry.Tools;
using AgenticApiSentry.OpenApi;
using Xunit;

public class LintSpecToolTests
{
    [Fact]
    public async Task Lint_runs_without_crash()
    {
      var samplePath = Path.Combine(System.AppContext.BaseDirectory, "bank.yaml");
      var doc = await OpenApiLoader.LoadAsync(samplePath);

      var tool = new LintSpecTool();
      var result = await tool.ExecuteAsync(doc);

      Assert.NotNull(result);
      Assert.Equal("LintSpec", result.ToolName);
    }
}
