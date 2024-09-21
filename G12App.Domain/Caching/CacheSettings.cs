using System;

namespace G12App.Domain.Caching
{
    public class CacheSettings(string Key)
    {
        public string Key { get; } = Key;

        public TimeSpan? ExpiresIn { get; set; } = TimeSpan.FromHours(1);
    }
}
