using System;

namespace PolyhydraGames.Core.Models
{
    public static class CacheHelper
    {
        public static Cache<T> CreateCache<T>(T? value, TimeSpan? expirationLength = default)
        {
            expirationLength ??= new TimeSpan(0, 5, 0);
            return new Cache<T>(value, expirationLength);
        }
    }
}
