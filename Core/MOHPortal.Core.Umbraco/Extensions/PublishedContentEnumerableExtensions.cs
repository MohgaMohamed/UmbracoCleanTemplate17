using Umbraco.Cms.Core.Models.PublishedContent;

namespace FayoumGovPortal.Core.Umbraco.Extensions
{
    public static class PublishedContentEnumerableExtensions
    {
        public static IEnumerable<TTarget> SelectCasted<TTarget>(this IEnumerable<IPublishedContent> source)
            where TTarget : class, IPublishedContent
        {
            foreach (IPublishedContent item in source)
            {
                if(item is null)
                {
                    continue;
                }

                //if(item.SafeCast<TTarget>() is TTarget casted)
                //{
                //    yield return casted;
                //}
                if (item is TTarget casted)
                {
                    yield return casted;
                }
            }
        }
    }
}
