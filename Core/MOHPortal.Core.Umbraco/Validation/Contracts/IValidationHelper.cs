namespace MOHPortal.Core.Umbraco.Validation.Contracts
{
    public interface IValidationHelper
    {
        Dictionary<string, bool> ValidateEmailList(List<string> mailList);
        bool ValidateMail(string mail);
        bool ValidateEgyptianMobileNumber(string mobileNumber);
        bool ValidateDigitsOnly(string value);
        bool ValidateEgyptianNationalId(string value);
        bool ValidateLongitudeLatitude(decimal longitude);
        bool HasPlaceholders(string input);
        public bool ValidateLettersOnly(string Text);
        bool ValidateUrl(string url);
    }
}
