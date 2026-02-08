using FayoumGovPortal.Core.Umbraco.Model.Gen;

namespace FayoumGovPortal.Core.Umbraco.ContentSearch
{
    public class SearchConstants
    {
        public const string SearchableCompositionAlias = nameof(SearchableComposition);
        public const string SearchPageDocumentAlias = "SearchPage";
        public const int NumberOfItemsPerPage = 10;
        public static float TextSearchFuzziness => 0.7f;
    }
}
