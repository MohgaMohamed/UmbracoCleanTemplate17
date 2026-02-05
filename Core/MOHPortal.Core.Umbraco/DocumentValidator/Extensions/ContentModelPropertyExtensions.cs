using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core;
using System.Text.Json;
using MOHPortal.Core.Umbraco.DocumentValidator.Base;
using Umbraco.Cms.Core.Media.EmbedProviders;

namespace MOHPortal.Core.Umbraco.DocumentValidator.Extensions
{
    /// <summary>
    /// [MS] Helper Functions for dealing with Umbraco's Intermediate Value Types existing in an IContent model such as
    /// Multi-Node Tree Picker Values, Tags Property Values, Blocklists, Media Pickers, etc...
    /// </summary>
    public static class ContentModelPropertyExtensions
    {
        public static TValue? GetPrimitivePropertyValue<TValue>(this IProperty property)
        {
            IPropertyValue? propertyValue = property.Values.FirstOrDefault(v => v.EditedValue != null);
            Attempt<TValue>? conversion = propertyValue?.EditedValue?.TryConvertTo<TValue>();
            if (conversion is null || !conversion.Value.Success)
                return default;

            return conversion.Value.Result;
        }

        public static List<GuidUdi> GetMultiNodeTreePickerPropertyValues(this IProperty property)
        {
            if (!property.PropertyType.PropertyEditorAlias.Equals(Constants.PropertyEditors.Aliases.MultiNodeTreePicker))
            {
                throw new InvalidOperationException(
                    $"Property Type must be {Constants.PropertyEditors.Aliases.MultiNodeTreePicker}, Try looking for a method containing the word '{property.PropertyType.PropertyEditorAlias}'");
            }

            IPropertyValue? result = property.Values.FirstOrDefault();
            if (result == null || result.EditedValue == null || result.EditedValue is not string stringValues)
            {
                return [];
            }

            List<GuidUdi> udis = [];
            foreach (string stringUdiValue in stringValues.Split(','))
            {
                //if (!UdiParser.TryParse(stringUdiValue!, out Udi? udi) || udi?.SafeCast<GuidUdi>() is not GuidUdi guidUdi)
                //{
                //    continue;
                //}
                if (!UdiParser.TryParse(stringUdiValue!, out Udi? udi) || udi is not GuidUdi guidUdi)
                {
                    continue;
                }
                udis.Add(guidUdi);
            }

            return udis;
        }

        public static string[] GetTagsPropertyValue(this IProperty property)
        {
            if (!property.PropertyType.PropertyEditorAlias.Equals(Constants.PropertyEditors.Aliases.Tags))
            {
                throw new InvalidOperationException(
                    $"Property Type must be {Constants.PropertyEditors.Aliases.Tags}, Try looking for a method containing the word '{property.PropertyType.PropertyEditorAlias}'");
            }

            IPropertyValue? value = property.Values.FirstOrDefault();
            if (value is null)
            {
                return [];
            }

            string? text = value.EditedValue?.ToString();
            if (string.IsNullOrWhiteSpace(text))
            {
                return [];
            }

            if (!text.Contains(','))
            {
                return [text];
            }

            return text.Split(",");
        }

        public static string[] GetJsonTagsPropertyValue(this IProperty property)
        {
            if (!property.PropertyType.PropertyEditorAlias.Equals(Constants.PropertyEditors.Aliases.Tags))
            {
                throw new InvalidOperationException(
                    $"Property Type must be {Constants.PropertyEditors.Aliases.MultiNodeTreePicker}, Try looking for a method containing the word '{property.PropertyType.PropertyEditorAlias}'");
            }

            IPropertyValue? value = property.Values.FirstOrDefault();
            if (value is null)
            {
                return [];
            }

            string? text = value.EditedValue?.ToString();
            if (string.IsNullOrWhiteSpace(text))
            {
                return [];
            }

            return JsonSerializer.Deserialize<List<string>>(text)?.ToArray() ?? [];
        }

        public static List<MediaPickerEntry> GetMediaPickerPropertyValues(this IProperty property)
        {
            IPropertyValue? applicationFormsValue = property.Values.FirstOrDefault();
            if (applicationFormsValue == null || applicationFormsValue.EditedValue == null)
            {
                return [];
            }

            string? rawValue = applicationFormsValue.EditedValue as string;
            if (string.IsNullOrWhiteSpace(rawValue))
            {
                return [];
            }

            //Is a Raw Udi Prefix
            if(rawValue.StartsWith(Constants.Conventions.Udi.Prefix))
            {
                //Is a Multiple CSV Udis
                if(rawValue.Contains(','))
                {
                    return rawValue.Split(',').Select(x =>
                    {
                        return new MediaPickerEntry()
                        {
                            Key = property.Key,
                            MediaKey = UdiParser.TryParse(x, out GuidUdi? udi) ? udi.Guid : default
                        };
                    }).ToList();
                }

                return [
                    new MediaPickerEntry()
                    {
                        Key = property.Key,
                        MediaKey = UdiParser.TryParse(rawValue, out GuidUdi? udi) ? udi.Guid : default
                    }
                ];
            }

            try
            {
                return JsonSerializer.Deserialize<List<MediaPickerEntry>>(rawValue) ?? [];
            }
            catch(Exception)
            {
                return [];
            }
        }

        //TODO[AS]: Add check box values to be here
    }
}
