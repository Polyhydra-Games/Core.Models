---
title: Media Asset References Checklist
status: active
owner: @DreadBreadcrumb
created: 2026-05-20
updated: 2026-05-20
tags: [feature, checklist, Core.Models, references]
---

# Media Asset References Checklist

## Discovery

- [x] Identify the asset classes that belong in the shared contract.
- [x] Keep upload, storage, and delivery behavior downstream.

## Documentation

- [x] Create the feature folder.
- [x] Document the boundary and allowed roles.
- [x] Provide examples for poster, trailer, subtitle, CDN image, and local video references.

## Models

- [x] Add `MediaAssetReferenceKind`.
- [x] Add `MediaAssetReference`.
- [x] Include optional `LocalMediaFile` linkage.
- [x] Include optional `MediaFileLocation` linkage.
- [x] Include optional URL and metadata escape hatch.

## Testing

- [x] Add serialization round-trip coverage.
- [x] Add at least one downstream mapping example.

## Follow-Up

- [ ] Decide whether asset reference roles need a separate enum.
- [x] Add provider reference contracts or document why they stay downstream.
- [x] Add relationship contracts as a separate shared family.
