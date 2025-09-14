# Agentic API Sentry (.NET 8) ![CI](https://github.com/MahendhiranK/agentic-api-sentry/actions/workflows/ci.yml/badge.svg) ![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)

**Agentic API Sentry** is a lightweight, **static, mission-bound agent** that evaluates any **OpenAPI 3.0 specification** for governance and compliance readiness.  
It helps architects, developers, and DevOps teams ensure API specs meet **quality, security, and observability standards** before moving into production.

## 📚 Author

Mahendhiran Krishnan  
LinkedIn: [Mahendhiran Krishnan](https://www.linkedin.com/in/mahendhiran-krishnan-04a5292b/)

## Overview
Works with or without an LLM. The v1 rules are code-based. If you add an LLM, it will summarize findings.  

Modern enterprises rely on APIs, but ensuring they are **secure, compliant, and resilient** is often overlooked.  
Agentic API Sentry bridges this gap by acting as an **immutable agent** that runs a fixed set of checks on any OpenAPI spec and generates actionable reports.  

This approach is especially useful for **regulated industries** (finance, healthcare, government) where **determinism and auditability** are critical.  

## 🚀 Features (V1)
- **Spec Linting** – Validates OpenAPI definitions, summaries, responses, and parameters.  
- **Security Checks** – Detects missing HTTPS servers, absent security schemes, or undocumented 401/403 responses.  
- **Rate-Limit Detection** – Identifies headers like `X-RateLimit` in API responses.  
- **Safe Sampling** – Optionally probes up to 5 GET endpoints when a base URL is provided.  
- **Reporting** – Generates both Markdown (`out/report.md`) and JUnit (`out/results.junit.xml`) outputs for CI/CD pipelines.  
- **Static Agent Execution** – Tools and mission are bound at runtime and cannot be altered, ensuring predictable behavior.  


## 📂 Repository Structure
```
agentic-api-sentry/
├── src/AgenticApiSentry/         # Core library & tools
│   ├── Agent/                    # Agent executor & interfaces
│   ├── Tools/                    # Spec lint, security, rate-limit, sampler
│   ├── OpenApi/                  # OpenAPI loader utilities
│   └── Reporting/                # Markdown & JUnit report writers
├── tests/AgenticApiSentry.Tests/ # Unit tests (xUnit)
├── samples/                      # Example OpenAPI specs (Bank API)
├── out/                          # Generated reports (gitignored)
├── README.md
├── LICENSE
└── agentic-api-sentry.sln
```

## ⚡ Quickstart

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)  
- (Optional) Internet access for loading remote OpenAPI specs  

### Run
```bash
# Clone
git clone https://github.com/<your-username>/agentic-api-sentry.git
cd agentic-api-sentry

# Build
dotnet build

# Evaluate sample Bank API spec
dotnet run --project src/AgenticApiSentry -- samples/bank.yaml

# Outputs
# - out/report.md
# - out/results.junit.xml

# Enable safe GET sampling with explicit base URL
dotnet run --project src/AgenticApiSentry -- samples/bank.yaml --baseUrl https://api.demo-bank.com/v1
```

## 📊 Example Report (Markdown Snippet)
```markdown
# Agentic API Sentry Report
Generated: 2025-09-14 12:00 UTC

## LintSpec
**Severity:** warn  
2 potential issues  
- /payments post: missing 401/403 error responses  
- /transactions get: missing detailed response description  

## SecurityChecks
**Severity:** warn  
2 potential security gaps  
- No explicit security schemes defined for operations (only global placeholder)  
- /accounts/{accountId} get: no documented 401/403 responses  

## RateLimitHint
**Severity:** info  
No explicit rate-limit headers detected in responses  

## Sampler
**Severity:** info  
Sampling disabled (no --baseUrl provided). Skipped real HTTP calls.
```

## Roadmap
- [ ] HTML report output  
- [ ] OpenTelemetry spans for observability  
- [ ] Policy-as-code (YAML/JSON) for custom rules  
- [ ] Postman collection export  
- [ ] Integration with GitHub Actions & Azure Pipelines  

## 📜 License
MIT License. See [LICENSE](LICENSE).
