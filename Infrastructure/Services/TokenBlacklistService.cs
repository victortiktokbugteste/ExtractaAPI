using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public interface ITokenBlacklistService
    {
        void BlacklistToken(string token, DateTime expiryTime);
        bool IsBlacklisted(string token);
    }

    public class TokenBlacklistService : ITokenBlacklistService
    {
        private readonly IMemoryCache _cache;

        public TokenBlacklistService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public void BlacklistToken(string token, DateTime expiryTime)
        {
            // Armazena o token na blacklist até sua expiração original
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(expiryTime);

            _cache.Set(GetCacheKey(token), true, cacheEntryOptions);
        }

        public bool IsBlacklisted(string token)
        {
            return _cache.TryGetValue(GetCacheKey(token), out _);
        }

        private string GetCacheKey(string token)
        {
            return $"BlacklistedToken_{token}";
        }
    }
} 