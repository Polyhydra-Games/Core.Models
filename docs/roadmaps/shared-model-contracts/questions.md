---
title: Shared Model Contracts Questions
status: open
owner: @DreadBreadcrumb
created: 2026-05-04
updated: 2026-06-03
tags: [roadmap, questions, Core.Models, shared-contracts]
---

# Shared Model Contracts Questions

## Canonical Anchor

Canonical doc: [README](./README.md). This file is supporting detail for the roadmap’s canonical Hermes anchor in README.md.

- [x] Should strongly typed identifiers move as concrete IDs, reusable base patterns, or both? Reusable base/converter patterns move now; concrete IDs stay downstream until ownership and compatibility are proven.
- [x] Should GUID-backed IDs always reject `Guid.Empty`, or should each ID type decide? Each concrete ID type decides; shared converters preserve the constructor policy.
- [x] Should string-backed IDs normalize whitespace to empty or reject whitespace by default? Each concrete ID type decides; strict and optional string IDs stay separate.
- [x] Should int-backed IDs allow zero? Each concrete ID type decides; shared helpers surface constructor validation.
- [x] Should JSON converters live directly in `Core.Models` or in a serialization-specific namespace? They live in `PolyhydraGames.Core.Models.Identifiers` with the ID contracts for the first pass.
- [x] Should media asset references use string role values, enums, or both? Both: `Role` stays a string and `Kind` stays an enum.
- [x] Should provider names be modeled as strings to avoid provider package churn? Yes. Provider keys stay string-based.
- [x] Should relationship endpoints use plain string IDs or a typed entity reference shape? Typed entity reference shape via `MediaRelationshipEndpoint`.
- [ ] Should playlists reference `LocalMediaFile`, `MediaAssetReference`, or both?
- [ ] Which downstream repo should validate media asset references first?
- [ ] Which duplicated API utility contracts actually exist in current repos?
- [ ] Should scan progress contracts live here or in `core.nas` until a second consumer appears?
- [ ] What package version should introduce the first post-media contract family?
