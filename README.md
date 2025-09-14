# Agentic API Sentry (.NET 8)

A small, **static, mission-bound agent** that evaluates any **OpenAPI 3** spec for governance readiness.
It lints the spec, runs basic security checks, optionally probes safe GET endpoints, and generates:

- `out/report.md` — human-readable summary
- `out/results.junit.xml` — CI-friendly test report

Works with or without an LLM. The v1 rules are code-based. If you add an LLM, it will summarize findings.

## Quickstart

```bash
# .NET 8 SDK required
git clone https://github.com/<you>/agentic-api-sentry.git
cd agentic-api-sentry

dotnet build
dotnet run --project src/AgenticApiSentry -- samples/petstore.yaml

# Outputs:
# - out/report.md
# - out/results.junit.xml
```

## Features (v1)
- **Spec ingest**: OpenAPI 3 from file or URL
- **Tools**:
  - `LintSpecTool`: basic quality checks
  - `SecurityChecksTool`: simple auth/HTTPS presence checks
  - `RateLimitHintTool`: detects common rate-limit headers in examples
  - `SamplerTool`: *safe* probing of GET endpoints (disabled unless base URL set)
- **Static Agent**: binds mission and tools at build time
- **Outputs**: Markdown and JUnit

## Safety
`SamplerTool` only issues real HTTP requests when you pass a explicit `--baseUrl` argument.
It only sends **GET** requests and skips endpoints with required path parameters.

## Usage

```bash
# Use a local file
dotnet run --project src/AgenticApiSentry -- samples/petstore.yaml

# Use a remote URL
dotnet run --project src/AgenticApiSentry -- https://petstore3.swagger.io/api/v3/openapi.json

# Enable safe GET sampling with explicit base URL
dotnet run --project src/AgenticApiSentry -- samples/petstore.yaml --baseUrl https://petstore3.swagger.io/api/v3
```

## Roadmap
- HTML report
- OpenTelemetry spans per tool
- Policy file for custom rules
- Postman collection export

## License
MIT
