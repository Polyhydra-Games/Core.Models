---
title: Scan Progress Contracts Checklist
status: active
owner: @DreadBreadcrumb
created: 2026-05-20
updated: 2026-06-03
tags: [feature, checklist, Core.Models, scan, progress]
---

# Scan Progress Contracts Checklist

## Canonical Anchor

Canonical doc: [README](./README.md). This checklist mirrors the canonical family status and does not carry a separate Hermes marker.

## Discovery

- [x] Identify the scan reporting scenarios that belong in the shared contract.
- [x] Keep filesystem walking, scheduling, persistence, and retry policy downstream.

## Documentation

- [x] Create the feature folder.
- [x] Document the boundary and allowed roles.
- [x] Document the current progress/summary/file shapes.

## Models

- [x] Add `MediaScanProgress`.
- [x] Add `MediaScanSummary`.
- [x] Add `IndexedMediaFileSummary`.

## Testing

- [x] Add serialization round-trip coverage.
- [x] Add at least one downstream mapping example.

## Follow-Up

- [ ] Decide whether a second summary shape is needed for partial scans.
- [x] Add downstream adoption examples in a real consumer repo.
