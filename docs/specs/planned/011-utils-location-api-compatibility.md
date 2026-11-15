# 011 - Utils: public location API compatibility

**Priority:** P1  
**Area:** Utils

## Context
`Utils/Locations.cs` appears to reference an internal SMAPI namespace: `StardewModdingAPI.Framework.ModLoading.Rewriters.StardewValley_1_6`. Internal APIs are not stable contracts.

## Requirements
- Replace internal SMAPI implementation references with supported public Stardew Valley/SMAPI APIs.
- Preserve the intended enumeration of farm, interior, and custom locations.
- Document behavior for unloaded locations and duplicate references.
- Keep consumers source-compatible where practical or document any required migration.

## Acceptance criteria
- No dependency on the identified internal namespace remains.
- Tests cover base-game interiors and custom locations.
- Build succeeds against the declared supported game/SMAPI versions.
