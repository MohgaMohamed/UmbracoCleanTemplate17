namespace MOHPortal.Core.Umbraco.DocumentValidator.Models
{
    public sealed record DocumentValidationResult
    {
        // Constructor for success case
        public static DocumentValidationResult Successful(string message = "") => 
            new DocumentValidationResult(true, string.Empty, message);

        // Constructor for failure case
        public static DocumentValidationResult Failure(string errorProperty, string errorMessage) => 
            new DocumentValidationResult(false, errorProperty, errorMessage);

        private DocumentValidationResult(bool success, string errorProperty, string errorMessage)
        {
            Success = success;
            ErrorProperty = errorProperty;
            ErrorMessage = errorMessage;
        }

        public bool Success { get; }
        public string ErrorProperty { get; }
        public string ErrorMessage { get; }
    }
}