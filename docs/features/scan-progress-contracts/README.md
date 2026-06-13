---
title: Scan Progress Contracts
status: active
owner: @DreadBreadcrumb
created: 2026-05-20
updated: 2026-05-20
tags: [feature, Core.Models, media, scan, progress, contracts]
---

# Scan Progress Contracts

## Summary

`MediaScanProgress`, `MediaScanSummary`, and `IndexedMediaFileSummary` provide portable, data-only scan reporting for media indexing workflows.
They are designed for `core.nas`-style scanners and related tooling that need to surface scan state without moving filesystem walking or retry behavior into `Core.Models`.

## Current Shape

- `MediaScanProgress` captures in-flight progress.
- `MediaScanSummary` captures a completed or checkpointed scan overview.
- `IndexedMediaFileSummary` captures one indexed file in a compact shape.

## Boundary

- `Core.Models` owns the portable scan reporting shape.
- Downstream repos own filesystem traversal, scheduling, persistence, and retry policy.
- Consumers should map into these contracts rather than coupling UI or transport code to scanner internals.

## Validation

- Serialization coverage exists in the `Core.Models` test project.
- `core.nas` can adopt these contracts when it wants to surface richer scan telemetry without exposing its execution details.

## Next Steps

- [x] Add a downstream mapping example in `core.nas`.
- [ ] Decide whether scan summary needs a separate counts object later.
