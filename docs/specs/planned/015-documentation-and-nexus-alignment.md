# 015 - All mods: documentation and distribution alignment

**Priority:** P2  
**Area:** Cross-cutting

## Context
Repository documentation, manifests, changelogs, and Nexus descriptions can drift, especially for dependencies, minimum versions, keybinds, defaults, and supported machine behavior.

## Requirements
- Define a consistent source of truth for mod name, version, dependencies, supported game/SMAPI versions, and configuration defaults.
- Align README, DESCRIPTION, changelog, manifest, and Nexus pages for each mod.
- Document keybinds, multiplayer/host-only behavior, custom machine limitations, and special processing rules where applicable.
- Verify translation language coverage and placeholder consistency.
- Include installation, upgrade, and troubleshooting guidance.

## Acceptance criteria
- Every distributed mod has complete and consistent metadata.
- Release checklist includes documentation review.
- User-facing claims match implemented and tested behavior.
