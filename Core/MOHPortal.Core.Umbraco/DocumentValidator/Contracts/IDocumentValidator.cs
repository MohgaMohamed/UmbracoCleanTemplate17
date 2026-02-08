using FayoumGovPortal.Core.Umbraco.DocumentValidator.Base;
using FayoumGovPortal.Core.Umbraco.DocumentValidator.Models;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace FayoumGovPortal.Core.Umbraco.DocumentValidator.Contracts
{
    /// <summary>
    /// [MS] This class Represents a Validator for a specific Document Type
    /// that should be Validated befor Saving Operation, implementing this interface
    /// will automatically register the implementing class in the IoC and will automatically invoke
    /// the Validate() method before the targeted ContentType is saved
    /// </summary>
    public interface IDocumentValidator<TValidator, TContentType> : IDocumentValidator
        where TValidator : ValidatorBase<TContentType>
        where TContentType : IPublishedContent
    {
    }

    /// <summary>
    /// [MS] This interface Marks all Document Validators during Compile Time,
    /// And IS NOT MEANT TO BE implemented directly, Document Validators should
    /// implement IDocumentValidator<TValidator, TContentType> instead.
    /// </summary>
    public interface IDocumentValidator
    {
        public DocumentValidationResult Validate(IContent model);
        public string DocumentTypeAlias { get; }
    }
}