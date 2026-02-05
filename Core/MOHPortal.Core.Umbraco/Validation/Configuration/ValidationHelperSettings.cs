namespace MOHPortal.Core.Umbraco.Validation.Configuration
{
    public class ValidationHelperSettings
    {
        public string MailValidationRegex { get; set; } = @"\A(?:[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?\.)+[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?)\Z";

        public string DigitsOnlyRegex { get; set; } = @"^\d*$";
        public string EgyptianMobileNumberRegex { get; set; } = @"^(\+20|0020|20|0)?1(([0125][0-9]{8}))$";

        public string LongitudeLatitudeValidationRegex { get; set; } = @"^[0-9]{1,3}(?:\.[0-9]{1,8})$";
        public string PlaceholdersRegex { get; set; } = @"\{\d+\}";
        public string LettersOnly { get; set; } = @"^[\u0621-\u064Aa-zA-Z\s]+$";

        public int EgyptianNationalIdLength { get; set; } = 14;
        public string UrlValidationRegex { get; set; } = @"^(https?:\/\/)?([\da-z\.-]+)\.([a-z\.]{2,6})([\/\w \.-]*)*\/?$";
    }
}
