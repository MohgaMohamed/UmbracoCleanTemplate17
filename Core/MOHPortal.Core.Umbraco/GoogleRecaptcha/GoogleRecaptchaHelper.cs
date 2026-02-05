using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MOHPortal.Core.Umbraco.GoogleRecaptcha.Contracts;
using MOHPortal.Core.Umbraco.GoogleRecaptcha.Models;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text.Json;
using Umbraco.Cms.Core.Strings;

namespace MOHPortal.Core.Umbraco.GoogleRecaptcha
{
    internal class GoogleRecaptchaHelper : IGoogleRecaptchaHelper
    {
        private const string OnLoadCallback = "OnRecaptchaLoad";
        private string ApiEndpoint { get; }
        private string SiteKey { get; }
        private string ClientKey { get; }
        private double ScoreThreshold { get; }
        private ILogger<GoogleRecaptchaHelper> Logger { get; }

        public GoogleRecaptchaHelper(
            IOptions<GoogleRecaptchaOptions> options,
            ILogger<GoogleRecaptchaHelper> logger)
        {
            GoogleRecaptchaOptions recaptchaSettings = options.Value;
            ValidateRecaptchaSettings(recaptchaSettings);

            ApiEndpoint = recaptchaSettings.ApiEndpoint ?? string.Empty;
            SiteKey = recaptchaSettings.SiteKey ?? string.Empty;
            ClientKey = recaptchaSettings.ClientSecret ?? string.Empty;
            ScoreThreshold = recaptchaSettings.ScoreThreshold;
            Logger = logger;
        }

        public HtmlEncodedString LoadRecaptchaApiScript()
            => new($"<script src='{ApiEndpoint}.js?onload={OnLoadCallback}&render={SiteKey}' async defer></script>");

        public HtmlEncodedString RenderGrecaptchaExecuteScript(string actionName, string inputId)
        {
            Dictionary<string, string> parameters = new()
            {
                { "SITE_KEY", SiteKey },
                { "ACTION_NAME", actionName },
                { "INPUT_ID", inputId },
                { "ON_LOAD_CALLBACK", OnLoadCallback }
            };

            string script =
            """
			<script async defer>
				function ON_LOAD_CALLBACK(){
					grecaptcha.ready(function() {
						grecaptcha.execute('SITE_KEY', { action: 'ACTION_NAME' }).then(function (token) {
							document.getElementById('INPUT_ID').value = token;
						});
					});
				}
			</script>
			""";

            foreach (KeyValuePair<string, string> item in parameters)
            {
                script = script.Replace(item.Key, item.Value);
            }

            return new HtmlEncodedString(script);
        }

        public HtmlEncodedString RenderCaptchaInputField(string inputId, string inputName)
        {
            string input = $"<input type='hidden' id='{inputId}' name='{inputName}' value=''>";
            return new HtmlEncodedString(input);
        }

        public async Task<bool> ValidateRecaptchaToken(string formToken)
        {
            Logger.LogInformation(
                "Starting GreCaptcha Validation \n Token: {Token}", 
                string.IsNullOrWhiteSpace(formToken) ? "NULL" : "[redacted]"
            );

            GoogleRecaptchaResponse? model = default;
            using HttpClient client = new();
            try
            {
                string endpoint = $"{ApiEndpoint}/siteverify?secret={ClientKey}&response={formToken}";
                string logEndpoint = $"{ApiEndpoint}/siteverify?secret=[redacted]&response=[redacted]";
                Logger.LogInformation(
                    "Sending GreCaptcha Validation Request \n Api: {Api}",
                    logEndpoint
                );

                HttpResponseMessage response = await client.PostAsync(endpoint, default);
                if(!response.IsSuccessStatusCode)
                {
                    Logger.LogWarning("Google Recaptcha Request was not Successful \n Response: {response}",
                        await response.Content.ReadAsStringAsync()
                    );
                    return false;
                }

                string textResponse = await response.Content.ReadAsStringAsync();
                Logger.LogInformation(
                   "GreCaptcha Validation Request Yielded the following response \n Response: {Response}",
                   textResponse
               );

                model = JsonConvert.DeserializeObject<GoogleRecaptchaResponse>(textResponse);
                if (model is null)
                {
                    Logger.LogError("Recaptcha V3 Response is Yielded a null Object");
                    return false;
                }
            }
            catch (Exception e)
            {
                Logger.LogError("An Exception was Thrown while Sending Recaptcha Request: \n {e}", e);
                return false;
            }

            if (model.Score < ScoreThreshold || !model.Success)
            {
                Logger.LogError("GreCaptcha Validation Failed with Score of {score} \n Response: \n {response}",
                    model.Score,
                    model.ToString()
                );
                return false;
            }

            Logger.LogInformation("Recaptcha Validation was Successful for {action} with Score of {score}", model.Action, model.Score);
            return true;
        }

        private void ValidateRecaptchaSettings(GoogleRecaptchaOptions settings)
        {
            string param = string.Empty;
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(settings.ApiEndpoint))
            {
                param += nameof(settings.ApiEndpoint) + ", ";
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(settings.ClientSecret))
            {
                param += nameof(settings.ClientSecret) + ", ";
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(settings.SiteKey))
            {
                param += nameof(settings.SiteKey) + ", ";
                isValid = false;
            }

            if (!isValid)
                Logger.LogWarning("No settings value was found for the following parameters: " + param);
        }
    }
}
