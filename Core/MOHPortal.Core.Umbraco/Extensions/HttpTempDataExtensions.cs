using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Text.Json;

namespace MOHPortal.Core.Umbraco.Extensions
{
    public static class HttpTempDataExtensions
    {
        public static void Put<T>(this ITempDataDictionary tempData, string key, T value) where T : class
        {
            tempData[key] = JsonSerializer.Serialize(value);
        }

        public static T? Get<T>(this ITempDataDictionary tempData, string key) where T : class
        {
            object? value;
            tempData.TryGetValue(key, out value);
            return value is null ? default : JsonSerializer.Deserialize<T>((string)value);
        }
    }
}
