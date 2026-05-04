namespace PolyhydraGames.Core.Models.Identifiers;

/// <summary>
/// Common contract for value objects that wrap a primitive identifier value.
/// </summary>
/// <typeparam name="TValue">The primitive backing value type.</typeparam>
public interface IStronglyTypedId<out TValue>
{
    /// <summary>
    /// Gets the wrapped primitive identifier value.
    /// </summary>
    TValue Value { get; }
}

/// <summary>
/// Common contract for strongly typed identifiers backed by a <see cref="System.Guid" />.
/// </summary>
public interface IGuidId : IStronglyTypedId<System.Guid>
{
}

/// <summary>
/// Common contract for strongly typed identifiers backed by a string.
/// </summary>
public interface IStringId : IStronglyTypedId<string>
{
}

/// <summary>
/// Common contract for string-backed identifiers where an empty value represents a missing identifier.
/// </summary>
public interface IOptionalStringId : IStringId
{
    /// <summary>
    /// Gets whether the identifier represents a missing value.
    /// </summary>
    bool IsEmpty { get; }
}
