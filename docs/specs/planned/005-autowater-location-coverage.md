# 005 - AutoWater: location coverage

**Priority:** P1  
**Area:** AutoWater

## Context
AutoWater iterates `Game1.locations`, while `Utils.Locations.GetLocations()` appears to include additional interiors. Sheds, greenhouses, interiors, and custom locations may therefore be treated inconsistently.

## Requirements
- Define explicitly which location types should be processed.
- Verify coverage for farm buildings/interiors, greenhouse, sheds, and supported custom locations.
- Avoid processing the same location or crop more than once.
- Handle unloaded, inaccessible, or invalid locations safely.
- Document intentional exclusions.

## Acceptance criteria
- A location coverage test matrix exists.
- Supported locations are processed once; excluded locations are intentionally and consistently skipped.
- Runtime tests confirm no duplicate watering or performance regression.
