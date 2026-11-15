# 014 - All mods: quality automation and regression tests

**Priority:** P1  
**Area:** Cross-cutting

## Context
The solution appears to lack a dedicated automated test project. Game integration is difficult to exercise in ordinary unit tests, so test boundaries should be deliberate.

## Requirements
- Introduce a test project for pure logic: configuration validation, time calculations, ID mapping, and translation comparison.
- Add narrow abstractions or fakes for GMCM and game-facing dependencies where this improves testability.
- Add build/load smoke checks for SMAPI and Harmony integration where feasible.
- Convert known historical reports (including shed/interior coverage, duplicate processing, and stutter) into regression scenarios, without treating unverified reports as confirmed defects.
- Document which behaviors require in-game manual verification.

## Acceptance criteria
- Core deterministic logic has repeatable automated tests.
- Tests run in CI and failures block validation.
- Manual smoke-test steps are documented for behavior that cannot be reliably unit-tested.
