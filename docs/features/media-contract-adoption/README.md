# Portable Media Contract Adoption

`Core.Models` owns dependency-free contracts that can cross process, machine, and repo boundaries. It does not own NAS scanning, provider clients, local playback, or app workflow.

## Canonical Identity

Use `LocalMediaFile.Location.RootId` plus `LocalMediaFile.Location.RelativePath` as the portable file identity.

- `RootId` names the configured storage root, not a workstation.
- `RelativePath` is slash-normalized and stable under that root.
- `FullPath` is optional and host-local; do not use it as cross-machine identity.
- `Url` is optional delivery information; do not use it to infer NAS inventory.

## Launch Identity

Use `MediaLaunchTarget` when a consumer needs a portable launch/select target:

- `RootId` and `RelativePath` are required for portable resolution.
- `MediaFileId` can carry the source catalog or API id when one exists.
- `Title`, `PosterUrl`, and `Kind` are display hints, not lookup keys.
- The canonical URI form is `media://{rootId}/{relativePath}` with URI-escaped path segments.

Example:

```json
{
  "rootId": "syn2-video",
  "relativePath": "Movies/The Blob (1958).mkv",
  "fileName": "The Blob (1958).mkv",
  "kind": "Video"
}
```

Equivalent launch URI:

```text
media://syn2-video/Movies/The%20Blob%20%281958%29.mkv
```

## Playlists

Use `MediaPlaylist` and `MediaPlaylistItem` for portable playlist or playout manifests.

- Prefer `MediaPlaylistItem.Media.Location.RootId` plus `RelativePath` when the item is backed by a known indexed file.
- Use `MediaPlaylistItem.RelativePath` for legacy app manifests that are scoped to a show or folder.
- Keep app-only hints in `Metadata` until they are common enough to promote.
- Validate manifests with `MediaPlaylistHelpers.Validate` before execution.

## Enrichment

Use `MediaEnrichmentMetadata` for catalog/provider detail that is not local inventory:

- titles, summaries, genres, release years, runtime, artwork, and provider IDs belong here
- hashes, scan roots, availability, local paths, and file size belong to NAS/file inventory
- provider-specific raw payloads should stay in provider apps unless normalized into shared fields

## Validation

Run the model tests after contract changes:

```bash
dotnet test Tests/PolyhydraGames.Core.Models.Test.csproj
```
