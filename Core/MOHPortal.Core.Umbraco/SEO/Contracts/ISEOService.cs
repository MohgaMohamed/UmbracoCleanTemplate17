using MOHPortal.Core.Umbraco.SEO.Models;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace MOHPortal.Core.Umbraco.SEO.Contracts
{
    public interface ISEOService
    {
        public SeoModel GetSeoModel(IPublishedContent content);

    }
}
