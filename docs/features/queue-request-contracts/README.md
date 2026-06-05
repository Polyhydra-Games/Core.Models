---
title: Queue and Request Contracts
status: active
owner: @DreadBreadcrumb
created: 2026-05-20
updated: 2026-06-03
tags: [feature, Core.Models, media, queue, requests, contracts]
---

# Queue and Request Contracts

## Hermes Status

Hermes Kanban: pending promotion — canonical family anchor; checklist and sibling docs point here.

## Summary

`MediaPlaybackQueueRequest` and `MediaPlaybackQueue` provide portable, data-only queue shapes for media playback workflows.
They let operator apps and playout tools share queue snapshots and queue requests without moving execution behavior into `Core.Models`.

## Current Shape

- `MediaPlaybackQueueRequest` represents an intent to add or insert media into a queue.
- `MediaPlaybackQueueRequestItem` can carry a playlist item, embedded local media, or a path-based reference.
- `MediaPlaybackQueue` represents a queue snapshot after items have been accepted.
- `MediaPlaybackQueueItem` carries the per-item queue state.
- `MediaQueueItemState` captures the broad lifecycle for items inside a queue.

## Examples

- Append a playlist segment:
  - `Append = true`
  - `Items[0].PlaylistItem.RelativePath = video/02-episode.mp4`
- Insert a single clip:
  - `Append = false`
  - `InsertAtIndex = 0`
  - `Items[0].MediaFileId = nas:123`
- Queue a raw local file:
  - `Items[0].Media.Location.RelativePath = Movies/The Blob (1958).mkv`

## Boundary

- `Core.Models` owns the portable queue/request shape.
- Downstream repos own queue scheduling, playback execution, clipping, and operator controls.
- Consumers should map into these contracts rather than embedding queue state in app-specific DTOs.

## Validation

- Serialization coverage exists in the `Core.Models` test project.
- The queue/request family can now be adopted by `chop-it` or `app-busey-box` without changing their execution behavior first.

## Next Steps

- [x] Add at least one downstream mapping example in a consumer repo.
- [ ] Decide whether queue item state needs a second enum for playback vs request lifecycle.
