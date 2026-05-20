---
title: Queue and Request Contracts Checklist
status: active
owner: @DreadBreadcrumb
created: 2026-05-20
updated: 2026-05-20
tags: [feature, checklist, Core.Models, queue, requests]
---

# Queue and Request Contracts Checklist

## Discovery

- [x] Identify the queue and request scenarios that belong in the shared contract.
- [x] Keep playback execution, scheduling, and clipping downstream.

## Documentation

- [x] Create the feature folder.
- [x] Document the boundary and allowed roles.
- [x] Provide examples for append, insert, and raw-file queueing.

## Models

- [x] Add `MediaQueueItemState`.
- [x] Add `MediaPlaybackQueueRequestItem`.
- [x] Add `MediaPlaybackQueueRequest`.
- [x] Add `MediaPlaybackQueueItem`.
- [x] Add `MediaPlaybackQueue`.

## Testing

- [x] Add serialization round-trip coverage.
- [x] Add at least one downstream mapping example.

## Follow-Up

- [ ] Decide whether a dedicated queue result model is needed later.
- [x] Add downstream adoption examples in a real consumer repo.
