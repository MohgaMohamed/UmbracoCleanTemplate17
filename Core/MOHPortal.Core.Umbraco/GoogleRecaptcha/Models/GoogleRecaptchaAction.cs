namespace MOHPortal.Core.Umbraco.GoogleRecaptcha.Models
{
    public class GoogleRecaptchaAction
    {
        public GoogleRecaptchaAction(string inputName, string actionName)
        {
            InputName = inputName;
            ActionName = actionName;
        }

        public string InputName { get; }
        public string ActionName { get; }
    }
}
