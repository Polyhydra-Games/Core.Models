---
title: Media Models Questions
status: open
owner: @DreadBreadcrumb
created: 2026-05-04
updated: 2026-05-06
tags: [feature, questions, media, Core.Models]
---

# Media Models Questions

- [x] Should `MediaKind.Rom` be renamed to `GameRom` before broad adoption? No. Keep `Rom` for compatibility with current downstream consumers and extension helpers.
- [x] Should playlist models live in `Core.Models.Media.Playlists`, in `chop-it`, or in another shared package?
- [x] Should checksum metadata include an algorithm field? No. Keep the checksum as an owning-app string until a real downstream consumer needs an algorithm field.
- [ ] Should subtitle sidecar files stay under `Document`, or should subtitles become a first-class media kind?
- [ ] Which downstream repo should be the first adoption target: `core.nas`, `SvengoolieDB`, or `chop-it`?
- [ ] Should playlist source fields stay generic, or should a future model reference `MediaScanRoot` directly?
