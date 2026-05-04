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
