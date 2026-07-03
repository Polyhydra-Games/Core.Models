using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PolyhydraGames.Core.Models.Streaming;

public enum RoomActorRole
{
    Viewer,
    Operator,
    Admin,
    Ai
}

public enum RoomComponentStatus
{
    Healthy,
    Degraded,
    Reconnecting,
    Offline
}

public enum RoomActivityKind
{
    PresenceJoined,
    PresenceLeft,
    ChatMessage,
    AiMessage,
    AiEnabled,
    AiDisabled,
    ModerationAction,
    HealthChanged,
    SourceChanged,
    OutputChanged,
    BroadcastStarted,
    BroadcastStopped,
    AdapterCommand
}

public sealed record RoomParticipantState
{
    public string ActorId { get; init; } = string.Empty;
    public string DisplayName { get; init; } = string.Empty;
    public RoomActorRole Role { get; init; }
    public bool IsGuest { get; init; }
    public string? ConnectionId { get; init; }
    public DateTimeOffset JoinedAt { get; init; } = DateTimeOffset.UtcNow;
    public DateTimeOffset LastSeenAt { get; init; } = DateTimeOffset.UtcNow;
}

public sealed record RoomHealthState
{
    public RoomComponentStatus Ingest { get; init; } = RoomComponentStatus.Offline;
    public RoomComponentStatus Playback { get; init; } = RoomComponentStatus.Offline;
    public RoomComponentStatus Chat { get; init; } = RoomComponentStatus.Offline;
    public RoomComponentStatus Reconnect { get; init; } = RoomComponentStatus.Healthy;

    [JsonIgnore]
    public bool IsHealthy =>
        Ingest == RoomComponentStatus.Healthy &&
        Playback == RoomComponentStatus.Healthy &&
        Chat == RoomComponentStatus.Healthy &&
        Reconnect != RoomComponentStatus.Reconnecting;
}

public sealed record RoomEventRecord
{
    public string RoomId { get; init; } = string.Empty;
    public RoomActivityKind Kind { get; init; }
    public string ActorId { get; init; } = string.Empty;
    public string DisplayName { get; init; } = string.Empty;
    public RoomActorRole ActorRole { get; init; }
    public bool IsGuest { get; init; }
    public string? TargetActorId { get; init; }
    public string? Message { get; init; }
    public string? Details { get; init; }
    public string? ConnectionId { get; init; }
    public string? SourceId { get; init; }
    public string? OutputId { get; init; }
    public DateTimeOffset Timestamp { get; init; } = DateTimeOffset.UtcNow;
}

public sealed record RoomSnapshot
{
    public string RoomId { get; init; } = string.Empty;
    public string? ActiveSourceId { get; init; }
    public string? ActiveOutputId { get; init; }
    public bool AiEnabled { get; init; }
    public int ViewerCount { get; init; }
    public RoomHealthState Health { get; init; } = new();
    public IReadOnlyList<RoomParticipantState> Participants { get; init; } = Array.Empty<RoomParticipantState>();
    public IReadOnlyList<RoomEventRecord> RecentEvents { get; init; } = Array.Empty<RoomEventRecord>();
    public DateTimeOffset UpdatedAt { get; init; } = DateTimeOffset.UtcNow;
}

public static class RoomPolicy
{
    public static bool CanControlAi(RoomActorRole role) => IsOperatorOrAdmin(role);
    public static bool CanModerate(RoomActorRole role) => IsOperatorOrAdmin(role);
    public static bool CanChangeSource(RoomActorRole role) => IsOperatorOrAdmin(role);
    public static bool CanChangeOutput(RoomActorRole role) => IsOperatorOrAdmin(role);
    public static bool CanControlAdapter(RoomActorRole role) => IsOperatorOrAdmin(role);
    public static bool CanControlBroadcast(RoomActorRole role) => IsOperatorOrAdmin(role);

    private static bool IsOperatorOrAdmin(RoomActorRole role) => role is RoomActorRole.Operator or RoomActorRole.Admin;
}

public enum StreamSourceKind
{
    EzcapLive,
    ObsRtmp,
    BuseyPlaylist,
    ChopItSegments,
    SvengoolieStation
}

public sealed record StreamSource(
    string SourceId,
    StreamSourceKind Kind,
    string DisplayName,
    string? Endpoint = null,
    string? BrowserEndpoint = null,
    bool IsPrimary = false,
    bool IsFallback = false,
    IReadOnlyDictionary<string, string>? Metadata = null);

public enum StreamOutputKind
{
    WebsiteHls,
    WebsiteLocal,
    TwitchRtmp,
    CustomRtmp
}

public sealed record StreamOutput(
    string OutputId,
    StreamOutputKind Kind,
    string DisplayName,
    string? Endpoint = null,
    bool IsPrimary = false,
    bool IsMirror = false,
    bool Strict = false,
    IReadOnlyDictionary<string, string>? Metadata = null);

public sealed record StreamAdapterCapability(
    string Command,
    string DisplayName,
    bool RequiresOperator = true,
    IReadOnlyDictionary<string, string>? Metadata = null);

public sealed record StreamAdapterStatus(
    string AdapterId,
    string AdapterType,
    string DisplayName,
    bool Healthy,
    string State,
    IReadOnlyList<StreamAdapterCapability> Capabilities,
    string? LastError = null,
    DateTimeOffset UpdatedAt = default);

public sealed record StreamControlCommand(
    string Command,
    IReadOnlyDictionary<string, string>? Arguments = null,
    string? ActorId = null,
    RoomActorRole ActorRole = RoomActorRole.Operator);

public sealed record StreamCommandResult(
    bool Success,
    string AdapterId,
    string Command,
    string Message,
    IReadOnlyDictionary<string, string>? Details = null,
    DateTimeOffset Timestamp = default);
