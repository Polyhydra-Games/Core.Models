---
title: Shared Model Contracts Milestones
status: planned
owner: @DreadBreadcrumb
created: 2026-05-04
updated: 2026-05-20
tags: [roadmap, milestones, Core.Models, shared-contracts]
---

# Shared Model Contracts Milestones

## M1: Contract Boundary Documented

- [x] `model-contract-stability` feature folder exists.
- [x] Import criteria are documented.
- [x] Non-goals are documented.
- [x] Compatibility checklist exists.

## M2: Media Reference Foundation

- [x] Strongly typed identifier base contracts are implemented.
- [x] GUID-backed, string-backed, int-backed, and long-backed ID tests exist.
- [x] OAuth/Streaming migration guidance is documented.

## M3: Media Reference Foundation

- [x] Media asset references are implemented.
- [x] External provider references are implemented.
- [x] Media relationship contracts are implemented.
- [x] Serialization tests cover realistic catalog/media examples.

## M4: Operator Media Flow Contracts

- [x] Playlist contracts are implemented.
- [x] Queue/request contracts are implemented.
- [x] `chop-it` adoption path is documented without moving execution behavior.

## M5: API Utility Foundation

- [x] Generic result/readiness contracts are implemented if duplication is confirmed.
- [x] Paging/filtering/sorting contracts are implemented if duplication is confirmed.
- [x] At least one API-facing downstream consumer is named.

## M6: Scanner Progress Foundation

- [x] Scan progress contracts are implemented.
- [x] `core.nas` adoption path is documented.
- [x] No scanner execution behavior moved into `Core.Models`.

## M7: First Adoption Proof

- [x] One downstream repo maps to a new shared contract family.
- [x] Downstream tests pass.
- [x] Gaps are captured in docs.
- [x] Public contract names are confirmed or renamed before wider rollout.
