# 007 - Configure Machine Speed: GMCM lifecycle

**Priority:** P2  
**Area:** Configure Machine Speed

## Context
GMCM registration appears to be initiated from `RenderedActiveMenu`, which can run repeatedly and may cause redundant registration work.

## Requirements
- Register GMCM integration once at the appropriate lifecycle event (normally `GameLaunched`).
- Keep config read/write callbacks current and persist changes through the established config flow.
- Handle GMCM being absent without errors.
- Avoid duplicate subscriptions or repeated registration.

## Acceptance criteria
- Registration occurs once per game launch.
- Changes in GMCM take effect and persist after restart.
- The mod works when GMCM is not installed.
