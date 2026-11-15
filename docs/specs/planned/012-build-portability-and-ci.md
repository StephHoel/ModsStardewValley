# 012 - Solution: portable build and CI

**Priority:** P1  
**Area:** Solution / CI

## Context
`Utils.csproj` appears to contain an absolute Steam installation HintPath. The workflow `.github/workflows/upload.yaml` has been reported to reference a `niam` branch, a potentially nonexistent `src/` archive path, and placeholder release text.

## Requirements
- Remove machine-specific absolute game paths; use ModBuildConfig or a documented configurable game path.
- Add CI validation for pull requests and pushes: restore, build, and relevant tests.
- Separate validation from release/upload automation; release jobs should run only on explicit release triggers.
- Package each mod from its actual project output and validate required manifest files.
- Replace placeholders with project-specific metadata and fail clearly on missing configuration.

## Acceptance criteria
- A clean checkout builds in CI without a developer-specific Steam path.
- CI validates all supported projects.
- Release archives contain the intended mod files and no unrelated source tree.
- No placeholder values remain in active release automation.
