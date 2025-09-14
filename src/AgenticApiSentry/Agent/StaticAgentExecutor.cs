namespace AgenticApiSentry.Agent
{
    public sealed class StaticAgentExecutor
    {
        private readonly IReadOnlyList<ITool> _tools;
        private readonly string _mission;

        private StaticAgentExecutor(IEnumerable<ITool> tools, string mission)
        {
            _tools = tools.ToList().AsReadOnly();
            _mission = mission;
        }

        // Single Builder definition only
        //public static Builder Builder() => new Builder();
        public static Builder CreateBuilder() => new Builder();

        public async Task<List<ToolResult>> RunAsync(object context)
        {
            var results = new List<ToolResult>();
            foreach (var tool in _tools)
            {
                try
                {
                    results.Add(await tool.ExecuteAsync(context));
                }
                catch (Exception ex)
                {
                    results.Add(new ToolResult(tool.Name, $"Tool crashed: {ex.Message}", "error"));
                }
            }
            return results;
        }

        public sealed class Builder
        {
            private readonly List<ITool> _tools = new();
            private string _mission = string.Empty;

            public Builder WithTools(IEnumerable<ITool> tools)
            {
                _tools.AddRange(tools);
                return this;
            }

            public Builder WithMission(string mission)
            {
                _mission = mission;
                return this;
            }

            public StaticAgentExecutor Build()
            {
                if (_tools.Count == 0)
                    throw new InvalidOperationException("No tools registered");
                if (string.IsNullOrWhiteSpace(_mission))
                    throw new InvalidOperationException("Mission is required");

                return new StaticAgentExecutor(_tools, _mission);
            }
        }
    }
}
