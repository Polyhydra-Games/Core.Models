using PolyhydraGames.Core.Interfaces;

namespace PolyhydraGames.Core.Models.Settings
{
#pragma warning disable CS0618 // Compatibility wrapper for the legacy synchronous settings API.
    public class JsonSetting<T> : Setting, ISettingValue<T>
    {
        private T _value = default!;
        public T Value
        {
            get => _value;
            set
            {
                if (_value?.Equals(value) ?? false)
                    return;
                _value = value;
                _settings.AddOrUpdateValue(_key, _value?.ToJson() ?? string.Empty);

            }
        }

        public JsonSetting(ISettings settings, string key, T defaultValue = default!) : base(settings, key)
        {
            _value = settings.GetValueOrDefault(_key, defaultValue?.ToString() ?? string.Empty).FromJson<T>();
        }
    }
#pragma warning restore CS0618
}
