---
title: External Provider References Checklist
status: active
owner: @DreadBreadcrumb
created: 2026-05-20
updated: 2026-06-03
tags: [feature, checklist, Core.Models, providers]
---

# External Provider References Checklist

## Canonical Anchor

Canonical doc: [README](./README.md). This checklist mirrors the canonical family status and does not carry a separate Hermes marker.

## Discovery

- [x] Identify the provider data that belongs in the shared contract.
- [x] Keep provider SDKs and lookup logic downstream.

## Documentation

- [x] Create the feature folder.
- [x] Document the provider boundary.
- [x] Document the stable provider key/value conventions.

## Models

- [x] Add `MediaProviderReference`.
- [x] Add provider key/value fields.
- [x] Add optional display name and URL fields.
- [x] Add optional last-seen timestamp.
- [x] Support multiple providers on a media item.

## Testing

- [x] Add serialization round-trip coverage.
- [x] Cover multiple provider references on one enrichment payload.

## Follow-Up

- [x] Add a downstream mapping example in a consumer repo.
- [ ] Decide whether provider references need a role field.
