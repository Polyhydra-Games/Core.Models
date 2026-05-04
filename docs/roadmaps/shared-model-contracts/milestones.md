---
title: Shared Model Contracts Milestones
status: planned
owner: @DreadBreadcrumb
created: 2026-05-04
updated: 2026-05-04
tags: [roadmap, milestones, Core.Models, shared-contracts]
---

# Shared Model Contracts Milestones

## M1: Contract Boundary Documented

- [ ] `model-contract-stability` feature folder exists.
- [ ] Import criteria are documented.
- [ ] Non-goals are documented.
- [ ] Compatibility checklist exists.

## M2: Media Reference Foundation

- [x] Strongly typed identifier base contracts are implemented.
- [x] GUID-backed, string-backed, int-backed, and long-backed ID tests exist.
- [x] OAuth/Streaming migration guidance is documented.

## M3: Media Reference Foundation

- [ ] Media asset references are implemented.
- [ ] External provider references are implemented.
- [ ] Media relationship contracts are implemented.
- [ ] Serialization tests cover realistic catalog/media examples.

## M4: Operator Media Flow Contracts

- [ ] Playlist contracts are implemented.
- [ ] Queue/request contracts are implemented.
- [ ] `chop-it` adoption path is documented without moving execution behavior.

## M5: API Utility Foundation

- [ ] Generic result/readiness contracts are implemented if duplication is confirmed.
- [ ] Paging/filtering/sorting contracts are implemented if duplication is confirmed.
- [ ] At least one API-facing downstream consumer is named.

## M6: Scanner Progress Foundation

- [ ] Scan progress contracts are implemented.
- [ ] `core.nas` adoption path is documented.
- [ ] No scanner execution behavior moved into `Core.Models`.

## M7: First Adoption Proof

- [ ] One downstream repo maps to a new shared contract family.
- [ ] Downstream tests pass.
- [ ] Gaps are captured in docs.
- [ ] Public contract names are confirmed or renamed before wider rollout.
