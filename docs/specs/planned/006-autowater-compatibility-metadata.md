# 006 - AutoWater: compatibility metadata

**Priority:** P2  
**Area:** AutoWater

## Context
The Nexus page has been reported to list SMAPI 3.0.0, while the inspected manifest reports 4.2.0.

## Requirements
- Determine the actual minimum supported SMAPI version from APIs used and release testing.
- Align manifest, Nexus metadata, README, and release notes.
- State supported Stardew Valley and SMAPI versions clearly.
- Do not raise or lower minimum versions without compatibility verification.

## Acceptance criteria
- Published metadata agrees across all distribution channels.
- The mod loads on the declared minimum version or the minimum is updated with evidence.
