using System;

namespace PolyhydraGames.Core.Models
{
    public class Cache<T>
    {
        public Cache(T? value, TimeSpan? expirationLength)
        {
            _value = value;
            ExpirationLength = expirationLength;
            CreationTime = DateTime.Now; // Set creation time to now
        }
        public TimeSpan? ExpirationLength { get; }
        private DateTime CreationTime;
        private T? _value;
        public bool IsExpired
        {
            get
            {
                if (_value == null) return true;
                if (ExpirationLength == null || ExpirationLength <= TimeSpan.Zero)
                    return false;
                return CreationTime + ExpirationLength < DateTime.Now;
            }
        }
        public T? Value
        {
            get => IsExpired ? default : _value;
            // If expired, return null
            set
            {
                CreationTime = DateTime.Now; // Reset creation time on set
                _value = value;
            }
        }
    }
}
