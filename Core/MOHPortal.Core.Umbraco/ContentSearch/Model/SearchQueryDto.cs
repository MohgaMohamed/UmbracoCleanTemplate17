using Examine.Search;

namespace FayoumGovPortal.Core.Umbraco.ContentSearch.Model
{
    public sealed record SearchQueryDto
    {
        public SearchQueryDto(
            string query,
            string? documentTypeAlias = null,
            string? currentCulture = null,
            int currentPage = 1,
            int pageSize = 10,
            SortableField? sortingOptions = null,
            bool shouldSortAscending = false)
        {
            SearchText = query;
            DocumentTypeAlias = documentTypeAlias;
            CurrentCulture = currentCulture;
            PageSize = pageSize == default ? 10 : pageSize;
            SortingOptions = sortingOptions;
            ShouldSortAscending = shouldSortAscending;
            CurrentPage = currentPage == default ? 1 : currentPage;
        }

        public string SearchText { get; }
        public string? DocumentTypeAlias { get; }
        public string? CurrentCulture { get; }
        public int CurrentPage { get; } = 1;
        public int PageSize { get; } = 10;
        public SortableField? SortingOptions { get; }
        public bool ShouldSortAscending { get; }
    }
}
