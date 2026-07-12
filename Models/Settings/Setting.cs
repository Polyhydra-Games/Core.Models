
using PolyhydraGames.Core.Interfaces;

namespace PolyhydraGames.Core.Models.Settings
{
#pragma warning disable CS0618 // Base type for compatibility wrappers over the legacy synchronous settings API.
    public abstract class Setting
    {
        protected readonly ISettings _settings;
        protected readonly string _key;

        protected Setting(ISettings settings, string key)
        {
            _key = key;
            _settings = settings;
        }
    }
#pragma warning restore CS0618
}
