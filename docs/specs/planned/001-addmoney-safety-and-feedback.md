# 001 - AddMoney: balance safety and feedback

**Priority:** P1  
**Area:** AddMoney

## Context
The money command should not allow invalid values or produce misleading feedback when the player or balance state is unavailable.

## Requirements
- Validate that the target player is available before changing their balance.
- Accept only positive amounts; reject zero and negative values with actionable feedback.
- Prevent integer overflow and define behavior at the game's supported balance limit.
- Report the amount actually added, not merely the requested amount, if the game clamps the balance.
- Keep command parsing and user-facing messages consistent with the mod's localization conventions.

## Acceptance criteria
- Tests cover missing player, malformed input, zero, negative, normal, very large, and near-limit amounts.
- No invalid input changes the balance.
- Feedback accurately reflects the resulting balance change.
- Existing valid command usage remains compatible.
