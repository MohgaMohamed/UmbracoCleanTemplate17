using Umbraco.Cms.Core.Strings;

namespace MOHPortal.Core.Umbraco.GoogleRecaptcha.Contracts
{
    public interface IGoogleRecaptchaHelper
    {
        HtmlEncodedString LoadRecaptchaApiScript();
        HtmlEncodedString RenderCaptchaInputField(string inputId, string inputName);
        HtmlEncodedString RenderGrecaptchaExecuteScript(string actionName, string inputId);
        Task<bool> ValidateRecaptchaToken(string formToken);
    }
}
