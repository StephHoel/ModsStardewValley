# 004 - AutoWater: GMCM reset state

**Priority:** P1  
**Area:** AutoWater

## Context
Resetting GMCM configuration appears to replace the config instance. Callbacks or closures may continue referencing the previous instance.

## Requirements
- Ensure all GMCM callbacks read and write the current configuration object.
- Rebuild or rebind callbacks safely when configuration is reset or reloaded.
- Persist reset values through the normal save path.
- Avoid stale references and duplicate event subscriptions.

## Acceptance criteria
- Reset restores documented defaults in the UI and runtime.
- Saving and restarting preserves the reset values.
- Subsequent edits after reset affect runtime behavior.
