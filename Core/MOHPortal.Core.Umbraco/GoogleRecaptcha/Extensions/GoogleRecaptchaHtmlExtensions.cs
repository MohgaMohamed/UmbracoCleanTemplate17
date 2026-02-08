using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using FayoumGovPortal.Core.Umbraco.GoogleRecaptcha.Contracts;

namespace FayoumGovPortal.Core.Umbraco.GoogleRecaptcha.Extensions
{
    public static class GoogleRecaptchaHtmlExtensions
    {
        public static IHtmlContent RenderRecaptchaScripts(this IHtmlHelper htmlHelper, IGoogleRecaptchaHelper recaptcha, string actionName, string inputId)
        {
            HtmlContentBuilder builder = new();
            var loadScript = htmlHelper.Raw(recaptcha.LoadRecaptchaApiScript());
            var executeScript = htmlHelper.Raw(recaptcha.RenderGrecaptchaExecuteScript(actionName, inputId));
            builder.AppendHtml(loadScript);
            builder.AppendHtml(executeScript);

            return builder;
        }

        public static IHtmlContent RenderRecaptchaInput(this IHtmlHelper htmlHelper, IGoogleRecaptchaHelper recaptcha, string inputId, string inputName)
        {
            return htmlHelper.Raw(recaptcha.RenderCaptchaInputField(inputId, inputName));
        }
    }
}
