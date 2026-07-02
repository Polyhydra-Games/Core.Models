using PolyhydraGames.Core.Interfaces;

namespace PolyhydraGames.Core.Models.Settings
{
#pragma warning disable CS0618 // Compatibility wrapper for the legacy synchronous settings API.
    public class StringSetting : Setting, ISettingValue<string>
    {
        private string _value = string.Empty;
        public string Value
        {
            get => _value;
            set
            {
                if (_value?.Equals(value) ?? false)
                    return;
                _value = value;
                _settings.AddOrUpdateValue(_key, _value);

            }
        }

        public StringSetting(ISettings settings, string key, string defaultValue = "") : base(settings, key)
        {
            Value = settings.GetValueOrDefault(key, defaultValue);
        }
    }
#pragma warning restore CS0618
}
