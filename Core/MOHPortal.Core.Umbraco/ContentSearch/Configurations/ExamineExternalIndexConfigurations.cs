using Examine.Lucene;
using Examine;
using Microsoft.Extensions.Options;
using Umb = Umbraco.Cms.Core;

namespace FayoumGovPortal.Core.Umbraco.ContentSearch.Configurations
{
    public sealed class ExamineExternalIndexConfigurations : IConfigureNamedOptions<LuceneDirectoryIndexOptions>
    {
        public void Configure(string? name, LuceneDirectoryIndexOptions options)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return;
            }

            //Use This to Configure Lucene-Specific field Indexing
            //Example
            //switch (name)
            //{
            //    case Umb.Constants.UmbracoIndexes.ExternalIndexName:
            //        options.FieldDefinitions.AddOrUpdate(
            //            new FieldDefinition("endDate", FieldDefinitionTypes.Integer));
            //        options.FieldDefinitions.AddOrUpdate(
            //            new FieldDefinition("startDate", FieldDefinitionTypes.Integer));
            //        break;
            //}
        }

        public void Configure(LuceneDirectoryIndexOptions options)
            => Configure(string.Empty, options);
    }
}