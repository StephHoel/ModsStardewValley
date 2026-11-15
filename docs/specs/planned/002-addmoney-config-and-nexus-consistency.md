# 002 - AddMoney: configuration and Nexus consistency

**Priority:** P2  
**Area:** AddMoney

## Context
The code default observed for `GoldToAdd` is `100000`, while the published Nexus description has been reported as “100.00 Gold”. The unit or formatting may be inconsistent.

## Requirements
- Confirm the intended unit and default amount from the implementation and release history.
- Align the config default, in-game description, README, changelog, and Nexus description.
- Explain whether the value is raw integer gold or a formatted currency amount.
- Avoid silently changing existing player configuration.

## Acceptance criteria
- One unambiguous default is documented everywhere.
- Existing config files retain their values unless an explicit migration is required.
- Documentation examples match actual command/config behavior.
