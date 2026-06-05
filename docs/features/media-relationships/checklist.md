---
title: Media Relationships Checklist
status: active
owner: @DreadBreadcrumb
created: 2026-05-20
updated: 2026-06-03
tags: [feature, checklist, Core.Models, relationships]
---

# Media Relationships Checklist

## Canonical Anchor

Canonical doc: [README](./README.md). This checklist mirrors the canonical family status and does not carry a separate Hermes marker.

## Discovery

- [x] Identify the relationship scenarios that belong in the shared contract.
- [x] Keep graph traversal, persistence, and resolution behavior downstream.

## Documentation

- [x] Create the feature folder.
- [x] Document the boundary and allowed roles.
- [x] Provide examples for episode-to-video, show-to-poster, media-to-trailer, ROM-to-box-art, and derived-asset relationships.

## Models

- [x] Add `MediaRelationshipKind`.
- [x] Add `MediaRelationshipEndpointKind`.
- [x] Add `MediaRelationshipEndpoint`.
- [x] Add `MediaRelationship`.
- [x] Include optional metadata escape hatches.

## Testing

- [x] Add serialization round-trip coverage.
- [x] Add at least one downstream mapping example.

## Follow-Up

- [ ] Decide whether relationship endpoints need additional fields for graph traversal hints.
- [x] Add downstream adoption examples in a real consumer repo.
