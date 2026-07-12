# Media contracts

`PolyhydraGames.Core.Models.Media` models media items, local files, locations, technical metadata, assets, provider references, relationships, and launch targets. `MediaPathHelpers` normalizes extensions and relative paths and classifies known extensions into `MediaKind` values.

The `Media.Playlists` namespace models playlists and playback queues, including items, playback options, queue state, and validation results. `Media.Scanning` models scan roots, progress, summaries, and indexed-file summaries.

These models may carry a full path or public URL supplied by a consumer. They do not access the file system, scan media, or expose a storage service.
