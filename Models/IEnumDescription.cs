namespace PolyhydraGames.Core.Models;

public interface IEnumDescription
{
    /// <summary>
    /// Name or Title
    /// </summary>
    string Title { get; }
    /// <summary>
    /// Friendly description
    /// </summary>
    string Description { get; }
}