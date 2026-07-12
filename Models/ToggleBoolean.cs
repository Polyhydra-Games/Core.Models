using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PolyhydraGames.Core.Models
{
    public class ToggleBoolean : INotifyPropertyChanged
    {
        private readonly string _title;
        private Func<bool> Get { get; }

        private Action<bool> Set { get; }

        public bool IsToggled
        {
            get { return Get(); }
            set
            {
                if (Get() == value) return;
                Set(value);
                OnPropertyChanged();
            }
        }
        public string Title => _title;

        public ToggleBoolean(string title, Func<bool> get, Action<bool> set)
        {
            _title = title;
            Get = get;
            Set = set;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
