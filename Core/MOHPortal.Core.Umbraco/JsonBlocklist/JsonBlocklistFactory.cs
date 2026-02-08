using FayoumGovPortal.Core.Umbraco.JsonBlocklist.Base;
using FayoumGovPortal.Core.Umbraco.JsonBlocklist.Models;
using System.Text.Json;

namespace FayoumGovPortal.Core.Umbraco.JsonBlocklist
{
    public class JsonBlocklistFactory<TContentData> where TContentData : JsonBlockListContentData
    {
        private bool _isInitialized = false;
        private JsonBlockList<TContentData>? _blocklist;

        public bool CanBuild => _isInitialized;

        public static JsonBlocklistFactory<TContentData> CreateNew()
        {
            return new()
            {
                _isInitialized = false
            };
        }

        public static JsonBlocklistFactory<TContentData> CreateFromRawValue(string? rawValue)
        {
            try
            {
                JsonBlockList<TContentData>? jsonBlocklistModel =
                    JsonSerializer.Deserialize<JsonBlockList<TContentData>>(rawValue!, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                return new()
                {
                    _isInitialized = true,
                    _blocklist = jsonBlocklistModel
                };
            }
            catch (Exception)
            {
                return CreateNew();
            }

        }

        public JsonBlocklistFactory<TContentData> InitializeBlocklist(List<TContentData>? data = default)
        {
            _blocklist ??= new();

            if (data != null && data.Count != 0)
            {
                foreach (TContentData item in data)
                {
                    _blocklist.Add(item);
                }
            }

            _isInitialized = true;
            return this;
        }

        public JsonBlockList<TContentData> Build()
        {
            if (!_isInitialized || _blocklist is null)
            {
                throw new InvalidOperationException("Blocklist object is not Initialized, use AddContentData() first before building");
            }

            return _blocklist;
        }

    }
}
