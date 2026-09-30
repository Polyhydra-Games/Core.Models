

using PolyhydraGames.Core.Interfaces;

namespace PolyhydraGames.Core.Models.Settings
{
#pragma warning disable CS0618 // Compatibility wrapper for the legacy synchronous settings API.
    public class BoolSetting : Setting, ISettingValue<bool>
    {
        private bool _value;
        public bool Value
        {
            get => _value;
            set
            {
                if (_value.Equals(value))
                    return;
                _value = value;
                _settings.AddOrUpdateValue(_key, _value);
            }
        }

        public BoolSetting(ISettings settings, string key, bool defaultValue = false) : base(settings, key)
        {
            Value = settings.GetValueOrDefault(key, defaultValue);
        }
    }
#pragma warning restore CS0618
}
