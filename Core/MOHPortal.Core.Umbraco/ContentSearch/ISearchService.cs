using FayoumGovPortal.Core.Umbraco.ContentSearch.Model;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace FayoumGovPortal.Core.Umbraco.ContentSearch
{
    public interface ISearchService
    {
        IEnumerable<IPublishedContent> MultiCultureSearch(SearchQueryDto query);
    }
}