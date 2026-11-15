# 003 - AutoWater: toggle keybind

**Priority:** P1  
**Area:** AutoWater

## Context
GMCM exposes a keybind (default reported as `O`) and its description promises activation toggling. Static inspection did not locate a current input handler that changes `IsActive`; runtime verification is required.

## Requirements
- Verify that the configured keybind toggles AutoWater on and off.
- If unsupported, implement the handler or correct the setting/description so it does not promise unavailable behavior.
- Respect custom keybinds and avoid handling input while the player is typing in a text field.
- Provide clear feedback only when appropriate and avoid repeated toggles from key-repeat events.

## Acceptance criteria
- Default and custom bindings both work.
- Typing in chat, menus, and text inputs does not accidentally toggle the feature.
- State and displayed configuration remain synchronized after toggling.
