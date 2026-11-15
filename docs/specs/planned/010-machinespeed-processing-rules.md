# 010 - Configure Machine Speed: processing rules

**Priority:** P1  
**Area:** Configure Machine Speed

## Context
Processing behavior spans `MachineConfigurator`, `MachineList`, exclusions, `DayUpdate`, incubators, Crystalariums, and Casks. `ProcessCask` may invoke `DayUpdate` repeatedly until quality changes, with a reported upper bound of 100 iterations. These paths need explicit rules and runtime verification.

## Requirements
- Document the source of truth for machine speed and the precedence of configuration rules.
- Define exclusions and special handling for incubators, Crystalariums, and Casks.
- Bound loops and guarantee termination without skipping valid progression.
- Use stable qualified item IDs; provide a safe migration/fallback for legacy or localized names.
- Ensure daily processing does not run twice or violate game progression semantics.

## Acceptance criteria
- Each special machine category has documented expected behavior and tests.
- Boundary cases and loop termination are covered.
- Runtime tests confirm no duplicated output, skipped stages, or save corruption.
