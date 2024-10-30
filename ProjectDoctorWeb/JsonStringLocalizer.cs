using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using System.Text.Json;

namespace ProjectDoctorWeb
{
    public class JsonStringLocalizer : IStringLocalizer
    {
        private readonly Newtonsoft.Json.JsonSerializer _jsonSerializer = new();
        private readonly IDistributedCache _cache;

        public JsonStringLocalizer(IDistributedCache cache)
        {
            _cache = cache;
        }

        public LocalizedString this[string name]
        {
            get
            {
                var value = GetValue(name);
                return new LocalizedString(name, value);
            }
        }

        public LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                var acualValue = this[name];

                if (!acualValue.ResourceNotFound)
                {
                    return new LocalizedString(name, string.Format(acualValue.Value, arguments));
                }

                return acualValue;
            }
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            throw new NotImplementedException();
        }

        private string GetValue(string key)
        {
            var filePath = $"Resources/{Thread.CurrentThread.CurrentUICulture.Name}.json";

            var fullPath = Path.GetFullPath(filePath);

            if (!File.Exists(fullPath))
            {
                return string.Empty;
            }

            var cacheKey = $"localize-{Thread.CurrentThread.CurrentUICulture.Name}-{key}";

            var cacheValue = _cache.GetString(cacheKey);

            if (!string.IsNullOrEmpty(cacheValue))
            {
                return cacheValue;
            }

            var value = GetValueFromJson(key, fullPath);

            if (!string.IsNullOrEmpty(value))
            {
                _cache.SetString(cacheKey, value);
            }

            return value;
        }

        private string GetValueFromJson(string propertyName, string filePath)
        {

            if (string.IsNullOrEmpty(propertyName) || string.IsNullOrEmpty(filePath))
            {
                return string.Empty;
            }

            using FileStream stream = new(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);

            using StreamReader streamReader = new(stream);

            using JsonTextReader reader = new(streamReader);

            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.PropertyName && reader.Value as string == propertyName)
                {
                    reader.Read();
                    return _jsonSerializer.Deserialize<string>(reader);
                }
            }
            return string.Empty;
        }

    }

}
