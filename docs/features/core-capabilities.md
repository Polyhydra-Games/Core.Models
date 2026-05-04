---
title: Core Capabilities
status: done
owner: @DreadBreadcrumb
priority: high
complexity: 1
created: 2026-03-19
updated: 2026-05-04
tags: [documentation, Core.Models, core]
---

# Core Capabilities

## Summary
Core.Models provides the shared contract layer for the media and platform apps in this workspace.

It is the right home for:

- canonical DTOs and value objects
- strongly typed identifiers
- enums and pure helpers
- serialization-friendly contracts that multiple repos consume

## Value Provided
- Keeps cross-repo model shapes stable.
- Prevents the same contract from being redefined in multiple app repos.
- Gives `core.nas`, `SvengoolieDB`, `chop-it`, and similar apps a common language.


## Newly Documented Existing Capabilities
- Core Application Logic
- Standardized Project Layout
