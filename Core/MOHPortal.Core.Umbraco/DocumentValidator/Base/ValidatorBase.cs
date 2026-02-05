using MOHPortal.Core.Umbraco.DocumentValidator.Models;
using System.Linq.Expressions;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace MOHPortal.Core.Umbraco.DocumentValidator.Base
{
    public abstract class ValidatorBase<TModel> where TModel : IPublishedContent
    {
        public string DocumentTypeAlias => typeof(TModel).Name.ToFirstLower();

        protected IProperty GetContentModelProperty<TValue>(Expression<Func<TModel, TValue>> selector, IContent model)
        {
            if (selector.Body is not MemberExpression expression)
            {
                throw new ArgumentException("Not a property expression.", nameof(selector));
            }

            string propertyAlias = expression.Member.Name;
            if (string.IsNullOrWhiteSpace(propertyAlias))
            {
                throw new InvalidOperationException($"Could not figure out property alias for property \"{expression.Member.Name}\".");
            }

            return model.Properties.First(x => x.Alias.Equals(propertyAlias, StringComparison.OrdinalIgnoreCase));
        }

        protected DocumentValidationResult FailedValidation(string errorProperty, string errorMessage) 
            => DocumentValidationResult.Failure(errorProperty, errorMessage);

        protected DocumentValidationResult SuccessfulValidation(string? message = default) 
            => DocumentValidationResult.Successful(message);
    }
}