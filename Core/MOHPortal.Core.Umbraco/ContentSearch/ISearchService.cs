using MOHPortal.Core.Umbraco.ContentSearch.Model;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace MOHPortal.Core.Umbraco.ContentSearch
{
    public interface ISearchService
    {
        IEnumerable<IPublishedContent> MultiCultureSearch(SearchQueryDto query);
    }
}