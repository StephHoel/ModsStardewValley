# 013 - TranslationSummary: validation and reporting

**Priority:** P2  
**Area:** TranslationSummary

## Context
The console tool compares `default.json` with translated files. Static inspection suggests `PopulateTranslationStatusEnum` may be invoked more than once per mod.

## Requirements
- Validate JSON syntax and report file path plus useful error location.
- Report missing, extra, and obsolete translation keys.
- Detect placeholder/token mismatches and preserve formatting tokens.
- Avoid duplicate status population and make processing deterministic.
- Provide meaningful process exit codes for CI integration.

## Acceptance criteria
- Fixtures cover valid, invalid, missing-key, extra-key, and placeholder-mismatch cases.
- Repeated runs produce stable output.
- CI can distinguish success, validation failures, and tool errors.
