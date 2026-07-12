using PolyhydraGames.Core.Interfaces;

namespace PolyhydraGames.Core.Models.Settings
{
#pragma warning disable CS0618 // Compatibility wrapper for the legacy synchronous settings API.
    public class DoubleSetting : Setting, ISettingValue<double>
    {
        private double _value;
        public double Value
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

        public DoubleSetting(ISettings settings, string key, double defaultValue = 0) : base(settings,key)
        {
            Value = settings.GetValueOrDefault(key, defaultValue);
        }
    }
#pragma warning restore CS0618
}
