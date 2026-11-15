# 009 - Configure Machine Speed: patch state isolation

**Priority:** P1  
**Area:** Configure Machine Speed

## Context
`MachineCollectPatch` uses static state such as `wasReady` and `previousHeld`. Shared state may leak between machines, players, or interleaved patch invocations.

## Requirements
- Identify the exact Harmony patch lifecycle and state ownership requirements.
- Replace shared mutable state with per-invocation or correctly scoped state.
- Ensure compatibility with other patches and avoid assumptions about call ordering.
- Keep patch application idempotent and report patch failures clearly.

## Acceptance criteria
- Tests cover interleaved operations on multiple machines and held-button transitions.
- One machine's state cannot affect another machine's processing.
- Compatibility smoke tests cover the supported Harmony/SMAPI environment.
