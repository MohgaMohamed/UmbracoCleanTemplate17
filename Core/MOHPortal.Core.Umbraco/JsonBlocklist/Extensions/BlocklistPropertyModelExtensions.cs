using FayoumGovPortal.Core.Umbraco.DocumentValidator.Extensions;
using FayoumGovPortal.Core.Umbraco.JsonBlocklist.Base;
using FayoumGovPortal.Core.Umbraco.JsonBlocklist.Models;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models;

namespace FayoumGovPortal.Core.Umbraco.JsonBlocklist.Extensions
{
    public static class BlocklistPropertyModelExtensions
    {
        public static JsonBlockList<TContentData>? GetBlocklistPropertyValue<TContentData>(this IProperty property)
            where TContentData : JsonBlockListContentData
        {
            if (!property.PropertyType.PropertyEditorAlias.Equals(Constants.PropertyEditors.Aliases.BlockList) &&
                !property.PropertyType.PropertyEditorAlias.Equals(Constants.PropertyEditors.Aliases.BlockGrid))
            {
                return default;
            }

            string? rawStringValue = property.GetPrimitivePropertyValue<string>();
            if (string.IsNullOrWhiteSpace(rawStringValue)){
                return default;
            }

            JsonBlocklistFactory<TContentData> factory =
                JsonBlocklistFactory<TContentData>.CreateFromRawValue(rawStringValue);

            if (factory.CanBuild)
            {
                return factory.Build();
            }
            else
            {
                return factory
                    .InitializeBlocklist()
                    .Build();
            }
        }
    }
}
