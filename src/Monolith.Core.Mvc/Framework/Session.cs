using System;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Monolith.Core.Mvc.Framework
{
    public class Session : ISession
    {
        private readonly ILogger<Session> _logger;

        private readonly IHttpContextAccessor _accessor;

        public Session(
            ILogger<Session> logger,
            IHttpContextAccessor accessor)
        {
            _logger = logger;
            _accessor = accessor;
        }

        public T Get<T>(string key)
        {
            var value = _accessor.HttpContext?.Session.GetString(key);

            if (string.IsNullOrWhiteSpace(value))
            {
                return default;
            }

            try
            {
                return JsonSerializer.Deserialize<T>(value);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unable to parse value from '{key}'", ex);
            }

            return default;
        }

        public void Set<T>(string key, T value)
        {
            _accessor.HttpContext?.Session.SetString(key, JsonSerializer.Serialize(value));
        }

        public void Remove(string key)
        {
            _accessor.HttpContext?.Session.Remove(key);
        }

        public void Clear()
        {
            _accessor.HttpContext?.Session.Clear();
        }
    }
}
