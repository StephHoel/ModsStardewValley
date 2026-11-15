# 008 - Configure Machine Speed: runtime indexing

**Priority:** P2  
**Area:** Configure Machine Speed

## Context
`ConfigureAllMachines` appears to rebuild machine indexes by ID and legacy name on repeated calls. The actual performance impact should be measured before optimization.

## Requirements
- Profile the current path under representative machine counts and call frequency.
- Cache derived indexes only where profiling demonstrates benefit.
- Invalidate caches safely when configuration, save, or relevant content changes.
- Prefer stable qualified item IDs; retain legacy-name compatibility where needed.

## Acceptance criteria
- Benchmark results document baseline and outcome.
- Cache invalidation tests prevent stale configuration.
- Behavior remains identical for supported machine definitions.
