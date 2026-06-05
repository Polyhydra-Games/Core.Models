---
title: Model Contract Stability Checklist
status: active
owner: @DreadBreadcrumb
created: 2026-05-20
updated: 2026-06-03
tags: [feature, checklist, Core.Models, contracts]
---

# Model Contract Stability Checklist

## Canonical Anchor

Canonical doc: [README](./README.md). This checklist mirrors the canonical family status and does not carry a separate Hermes marker.

## Discovery

- [x] Identify the contract families that already belong in `Core.Models`.
- [x] Identify the downstream repos that must keep execution behavior out of the shared package.
- [x] Define the difference between stable contracts and future shared-model candidates.

## Documentation

- [x] Create the feature folder.
- [x] Document what qualifies for `Core.Models`.
- [x] Document what must stay in downstream repos.
- [x] Document the compatibility checklist for public model changes.

## Validation

- [x] Confirm current shared-model adoption via `core.nas`, `SvengoolieDB`, `chop-it`, and `app-busey-box`.
- [x] Confirm the current shared media contracts still serialize and round-trip cleanly.
- [ ] Add release-note guidance for future breaking contract families.

## Follow-Up

- [ ] Add a reusable compatibility template if more shared contract families land.
- [ ] Add a package/version policy note for future shared-model releases.
