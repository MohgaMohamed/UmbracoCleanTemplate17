using Examine;
using Examine.Search;
using Lucene.Net.Search;
using Microsoft.Extensions.Logging;
using MOHPortal.Core.Umbraco.ContentSearch.Model;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Infrastructure.Examine;

namespace MOHPortal.Core.Umbraco.ContentSearch
{
    /// <summary>
    /// [MS] Wrapper for Umbraco's Examine Search Engine
    /// This class is meant to support various Content Search scenarios
    /// Such as: Culture-Based, Non-Culture-Based, All Content Types, Spicific Content Types
    /// </summary>
    public class SearchService : ISearchService
    {
        #region Private Fields
        private readonly ILogger<SearchService> _logger;
        private readonly IExamineManager _examineManager;
        private readonly ILocalizationService _localizationService;
        private readonly IPublishedContentQuery _publishedContentQuery;
        private readonly IContentTypeService _contentTypeService;
        private readonly IVariationContextAccessor _variationContextAccessor;

        #endregion

        public SearchService(
            ILogger<SearchService> logger,
            IExamineManager examineManager,
            ILocalizationService localizationService,
            IPublishedContentQuery publishedContentQuery,
            IContentTypeService contentTypeService,
            IVariationContextAccessor variationContextAccessor)
        {
            _logger = logger;
            _examineManager = examineManager;
            _localizationService = localizationService;
            _publishedContentQuery = publishedContentQuery;
            _contentTypeService = contentTypeService;
            BooleanQuery.MaxClauseCount = 2048;
            _variationContextAccessor = variationContextAccessor;
        }

        #region Public Methods
        /// <summary>
        /// [MS] Searches Umbraco's External Index using the Specified SearchText
        /// Within a specified document type alias (If exists) or all searchable Documents
        /// Supports Multi-Culture Context, Pagination & Ordering by a Specified Field Name and Direction
        /// </summary>
        /// <param name="query"> The Search Query Model</param>
        /// <returns>the IPublishedContent document found or Empty List if there's none</returns>
        public IEnumerable<IPublishedContent> MultiCultureSearch(SearchQueryDto query)
        {
            List<string> cultures = query.CurrentCulture is null ? GetAllCultureCodes() : [query.CurrentCulture.ToLower()];

            IEnumerable<IContentType> searchableTypes;
            if (string.IsNullOrWhiteSpace(query.DocumentTypeAlias))
            {
                IContentType? searchableContentType = _contentTypeService.Get(SearchConstants.SearchableCompositionAlias);
                searchableTypes = _contentTypeService.GetComposedOf(searchableContentType?.Id ?? default);
            }
            else
            {
                IContentType? targetContentType = _contentTypeService.Get(query.DocumentTypeAlias);
                searchableTypes = targetContentType is null ? [] : [targetContentType];
            }

            if (!searchableTypes.Any())
            {
                _logger.LogWarning("Searchable Types Returned Empty!");
                return [];
            }

            List<string> indexFields = GetSearchableFields(cultures, searchableTypes);
            if (!_examineManager.TryGetIndex(Constants.UmbracoIndexes.ExternalIndexName, out IIndex index))
            {
                _logger.LogWarning("Failed to Get Umbraco Extrenal Index");
                return [];
            }

            //Sanitization
            string[] searchTerms = [];
            if (!string.IsNullOrWhiteSpace(query.SearchText))
            {
                if(query.SearchText.Length < 80)
                {
                    searchTerms = [
                        .. query.SearchText
                            .Split([' ', '-'])
                            .Where(x => !x.IsNullOrWhiteSpace())
                            .Distinct()
                    ];
                }
                else
                {
                    searchTerms = [ query.SearchText ];
                }
            }

            IEnumerable<IExamineValue> examineValues = GetFuzzyExamineValues(searchTerms)
                .Concat(GetWildCardExamineValues(searchTerms))
                .Concat(searchTerms.Select(x => x.Boost(5)));

            IQuery examineSearchQuery = index.Searcher.CreateQuery(Constants.Applications.Content);
            string[] nodeNames = query.CurrentCulture.IsNullOrWhiteSpace() && cultures.Count == 1
                ? [ UmbracoExamineFieldNames.NodeNameFieldName ]
                : [.. cultures.Select(cc => $"{UmbracoExamineFieldNames.NodeNameFieldName}_{cc.ToLower()}")];

            IBooleanOperation examineQuery = examineSearchQuery
                .GroupedOr([.. indexFields, .. nodeNames], examineValues.ToArray());

            if (!string.IsNullOrWhiteSpace(query.DocumentTypeAlias))
            {
                examineQuery.And().NodeTypeAlias(query.DocumentTypeAlias);
            }
            else
            {
                examineQuery.And().GroupedOr([ UmbracoExamineFieldNames.ItemTypeFieldName ], searchableTypes.Select(x => (IExamineValue) new ExamineValue(Examineness.Explicit, x.Alias)).ToArray());
            }

            if (query.SortingOptions.HasValue && query.ShouldSortAscending)
            {
                examineQuery.OrderBy(query.SortingOptions.Value);
            }

            if (query.SortingOptions.HasValue && !query.ShouldSortAscending)
            {
                examineQuery.OrderByDescending(query.SortingOptions.Value);
            }

            QueryOptions options = QueryOptions.SkipTake(
                skip: (query.CurrentPage - 1) * query.PageSize,
                take: query.PageSize
            );

            IEnumerable<string> resultsIds = examineQuery
                .Execute(options)
                .Select(result => result.Id);

            IEnumerable<IPublishedContent> content = _publishedContentQuery.Content(resultsIds);
            if(!content.Any())
            {
                return [];
            }

            string? cult = query.CurrentCulture;
            if (cult.IsNullOrWhiteSpace())
            {
                cult = _variationContextAccessor.VariationContext?.Culture
                    ?? Thread.CurrentThread.CurrentCulture.Name;
            }

            return content.Where(x => x.HasCulture(cult));
        }
        #endregion

        #region Private Methods
        private List<string> GetSearchableFields(List<string> cultureCodes, IEnumerable<IContentType> contentsTypes)
        {
            List<string> indexFields = [];
            List<string> searchablePropertyEditorAlias =
            [
                Constants.PropertyEditors.Aliases.Label,
                Constants.PropertyEditors.Aliases.TextArea,
                Constants.PropertyEditors.Aliases.TextBox,
                Constants.PropertyEditors.Aliases.RichText,
                Constants.PropertyEditors.Aliases.Tags,
                Constants.PropertyEditors.Aliases.MarkdownEditor
            ];

            indexFields = contentsTypes.Aggregate(
                indexFields,
                (fields, contentType) =>
                {
                    cultureCodes.ForEach(
                        cultCode => fields.AddRange(
                            contentType.CompositionPropertyTypes
                            .Where(x => searchablePropertyEditorAlias.Contains(x.PropertyEditorAlias) && !x.Alias.Contains("meta"))
                            .Select(p => p.VariesByCulture() ? $"{p.Alias}_{cultCode.ToLower()}" : $"{p.Alias}")
                        )
                    );
                    return fields;
                }
            );
            
            return indexFields;
        }
        private List<IExamineValue> GetFuzzyExamineValues(string[] queryTerms)
            => queryTerms.Where(x => !string.IsNullOrWhiteSpace(x)).Select(term => term.Fuzzy(SearchConstants.TextSearchFuzziness)).ToList();
        private List<IExamineValue> GetWildCardExamineValues(string[] queryTerms)
            => queryTerms.Where(x => !string.IsNullOrWhiteSpace(x)).Select(term => term.MultipleCharacterWildcard()).ToList();
        private List<string> GetAllCultureCodes() =>
            _localizationService.GetAllLanguages().Select(x => x.IsoCode.ToLower()).ToList();
        #endregion
    }
}
